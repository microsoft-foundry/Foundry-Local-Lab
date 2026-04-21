# Kendte Problemer — Foundry Local Workshop

Problemer opstået ved opbygning og test af denne workshop på en **Snapdragon X Elite (ARM64)** enhed med Windows, med Foundry Local SDK v0.9.0, CLI v0.8.117 og .NET SDK 10.0.

> **Sidst valideret:** 2026-03-11

---

## 1. Snapdragon X Elite CPU Genkendes Ikke af ONNX Runtime

**Status:** Åben  
**Alvorlighed:** Advarsel (ikke-blokerende)  
**Komponent:** ONNX Runtime / cpuinfo  
**Reproduktion:** Hver opstart af Foundry Local-service på Snapdragon X Elite hardware

Hver gang Foundry Local-servicen starter, udstedes to advarsler:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Indvirkning:** Advarslerne er kosmetiske — inferens fungerer korrekt. Dog vises de ved hver kørsel og kan forvirre workshopdeltagere. ONNX Runtime cpuinfo-biblioteket skal opdateres til at genkende Qualcomm Oryon CPU-kerner.

**Forventet:** Snapdragon X Elite burde genkendes som en understøttet ARM64 CPU uden at udsende fejlmeddelelser.

---

## 2. SingleAgent NullReferenceException ved Første Kørsel

**Status:** Åben (intermitterende)  
**Alvorlighed:** Kritisk (nedbrud)  
**Komponent:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reproduktion:** Kør `dotnet run agent` — crasher straks efter modelindlæsning

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Kontekst:** Linje 37 kalder `model.IsCachedAsync(default)`. Nedbruddet skete ved første kørsel af agenten efter en frisk `foundry service stop`. Efterfølgende kørsel med samme kode lykkedes.

**Indvirkning:** Intermitterende — antyder en løbskonflikt i SDK’ets serviceinitialisering eller katalogforespørgsel. `GetModelAsync()`-kaldet kan returnere før servicen er fuldt klar.

**Forventet:** `GetModelAsync()` skal enten blokere indtil servicen er klar eller returnere en tydelig fejlmeddelelse, hvis initialisering ikke er fuldført.

---

## 3. C# SDK Kræver Eksplicit RuntimeIdentifier

**Status:** Åben — spores i [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Alvorlighed:** Dokumentationshul  
**Komponent:** `Microsoft.AI.Foundry.Local` NuGet-pakke  
**Reproduktion:** Opret et .NET 8+ projekt uden `<RuntimeIdentifier>` i `.csproj`

Build fejler med:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Årsag:** Kravet om RID er forventet — SDK leverer native binære filer (P/Invoke til `Microsoft.AI.Foundry.Local.Core` og ONNX Runtime), så .NET skal kende den platformspecifikke biblioteksopløsning.

Dette er dokumenteret på MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), hvor kørselsinstruktionerne viser:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Brugere skal dog huske `-r` flaget hver gang, hvilket er let at glemme.

**Workaround:** Tilføj en auto-detektions fallback til dit `.csproj`, så `dotnet run` virker uden flag:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` er en indbygget MSBuild-egenskab, der automatisk afløser værtsmaskinens RID. SDK’ets egne testprojekter bruger allerede dette mønster. Eksplicitte `-r` flag respekteres stadig, når de angives.

> **Bemærk:** Workshop-`.csproj` inkluderer denne fallback, så `dotnet run` virker direkte på enhver platform.

**Forventet:** `.csproj` skabelonen i MS Learn-dokumentationen bør inkludere dette auto-detektionsmønster, så brugere ikke behøver huske `-r` flaget.

---

## 4. JavaScript Whisper — Lydtranskription Returnerer Tom eller Binær Output

**Status:** Åben (regression — forværret siden første rapport)  
**Alvorlighed:** Alvorlig  
**Komponent:** JavaScript Whisper-implementering (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reproduktion:** Kør `node foundry-local-whisper.mjs` — alle lydfiler returnerer tom eller binær output i stedet for teksttranskription

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Oprindeligt returnerede kun den 5. lydfil tom; fra v0.9.x returnerer alle 5 filer et enkelt byte (`\ufffd`) i stedet for transskriberet tekst. Python Whisper-implementeringen med OpenAI SDK transskriberer de samme filer korrekt.

**Forventet:** `createAudioClient()` skal returnere teksttranskription, som matcher Python/C# implementeringerne.

---

## 5. C# SDK Leverer Kun net8.0 — Ingen Officiel .NET 9 eller .NET 10 Target

**Status:** Åben  
**Alvorlighed:** Dokumentationshul  
**Komponent:** `Microsoft.AI.Foundry.Local` NuGet-pakke v0.9.0  
**Installationskommando:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet-pakken leverer kun ét target framework:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Der medfølger ingen `net9.0` eller `net10.0` TFM. Til sammenligning leverer følgesvendpakken `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472` og `netstandard2.0`.

### Kompatibilitetstestning

| Target Framework | Byg | Kør | Noter |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | Officielt understøttet |
| net9.0 | ✅ | ✅ | Bygger via forward-compat — brugt i workshop eksempler |
| net10.0 | ✅ | ✅ | Bygger og kører via forward-compat med .NET 10.0.3 runtime |

net8.0-assembly indlæses på nyere runtime via .NET’s forward-kompatibilitetsmekanisme, så byggeprocessen lykkes. Dette er dog ikke dokumenteret eller testet af SDK-teamet.

### Derfor Målretter Eksemplerne net9.0

1. **.NET 9 er den seneste stabile version** — de fleste workshopdeltagere vil have den installeret  
2. **Forward kompatibilitet fungerer** — net8.0-assembly i NuGet-pakken kører på .NET 9 runtime uden problemer  
3. **.NET 10 (preview/RC)** er for ny til at målrette i en workshop, der skal fungere for alle

**Forventet:** Fremtidige SDK-udgivelser bør overveje at tilføje `net9.0` og `net10.0` TFMs ved siden af `net8.0` for at matche mønsteret brugt af `Microsoft.Agents.AI.OpenAI` og sikre valideret support for nyere runtimeversioner.

---

## 6. JavaScript ChatClient Streaming Bruger Callbacks, Ikke Async Iteratorer

**Status:** Åben  
**Alvorlighed:** Dokumentationshul  
**Komponent:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient`, som returneres af `model.createChatClient()`, tilbyder metoden `completeStreamingChat()`, men den bruger et **callback-mønster** frem for at returnere en async iterable:

```javascript
// ❌ Dette virker IKKE — kaster "stream er ikke asynkront itererbar"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Korrekt mønster — giv en callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Indvirkning:** Udviklere vant til OpenAI SDK’ets async iteration (`for await`) vil opleve forvirrende fejl. Callback-funktionen skal være gyldig, ellers smider SDK’en "Callback must be a valid function."

**Forventet:** Dokumentér callback-mønstret i SDK-reference. Alternativt understøt async iterable-mønstret for konsistens med OpenAI SDK.

---

## Miljødetaljer

| Komponent | Version |
|-----------|---------|
| OS | Windows 11 ARM64 |
| Hardware | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er blevet oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi stræber efter nøjagtighed, bedes du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det originale dokument på dets oprindelige sprog bør betragtes som den autoritative kilde. For kritisk information anbefales professionel menneskelig oversættelse. Vi påtager os intet ansvar for misforståelser eller fejltolkninger, der opstår som følge af brugen af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->