# Kända problem — Foundry Local Workshop

Problem som uppstod vid uppbyggnad och testning av denna workshop på en **Snapdragon X Elite (ARM64)**-enhet som kör Windows, med Foundry Local SDK v0.9.0, CLI v0.8.117 och .NET SDK 10.0.

> **Senast verifierad:** 2026-03-11

---

## 1. Snapdragon X Elite CPU Känns Inte Igen av ONNX Runtime

**Status:** Öppen
**Allvar:** Varning (icke-blockerande)
**Komponent:** ONNX Runtime / cpuinfo
**Reproduktion:** Varje start av Foundry Local-tjänst på Snapdragon X Elite-hårdvara

Varje gång Foundry Local-tjänsten startar skickas två varningar:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**Påverkan:** Varningarna är kosmetiska — inferens fungerar korrekt. Dock visas de vid varje körning och kan förvirra workshopdeltagare. ONNX Runtime cpuinfo-biblioteket behöver uppdateras för att känna igen Qualcomm Oryon CPU-kärnor.

**Förväntat:** Snapdragon X Elite bör kännas igen som en stödjad ARM64-CPU utan att skicka felnivåmeddelanden.

---

## 2. SingleAgent NullReferenceException vid Första Körningen

**Status:** Öppen (intermittent)
**Allvar:** Kritisk (krasch)
**Komponent:** Foundry Local C# SDK + Microsoft Agent Framework
**Reproduktion:** Kör `dotnet run agent` — kraschar omedelbart efter modellinläsning

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**Kontext:** Rad 37 anropar `model.IsCachedAsync(default)`. Kraschen hände vid första körningen av agenten efter en ny `foundry service stop`. Efterföljande körningar med samma kod lyckades.

**Påverkan:** Intermittent — tyder på en tävlingssituation i SDK:ns tjänsteinitialisering eller katalogfråga. Anropet `GetModelAsync()` kan returnera innan tjänsten är helt redo.

**Förväntat:** `GetModelAsync()` bör antingen blockera tills tjänsten är redo eller ge ett tydligt felmeddelande om tjänsten inte färdigställt initialiseringen.

---

## 3. C# SDK Kräver Explicit RuntimeIdentifier

**Status:** Öppen — spåras i [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)
**Allvar:** Dokumentationsbrist
**Komponent:** `Microsoft.AI.Foundry.Local` NuGet-paket
**Reproduktion:** Skapa ett .NET 8+ projekt utan `<RuntimeIdentifier>` i `.csproj`

Bygget misslyckas med:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**Orsak:** RID-kravet är förväntat — SDK levererar inbyggda binärer (P/Invoke till `Microsoft.AI.Foundry.Local.Core` och ONNX Runtime), så .NET behöver veta vilken plattformspecifik biblioteksfil som ska lösas.

Detta dokumenteras på MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), där körinstruktionerna visar:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

Användare måste dock komma ihåg `-r`-flaggan varje gång, vilket är lätt att glömma.

**Lösning:** Lägg till en automatisk upptäcktsfallback i din `.csproj` så att `dotnet run` fungerar utan flaggor:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` är en inbyggd MSBuild-egenskap som automatiskt löser värddatorns RID. SDK:ns egna testprojekt använder redan detta mönster. Explicit angivna `-r`-flaggar respekteras fortfarande.

> **Obs:** Workshopens `.csproj` inkluderar denna fallback så att `dotnet run` fungerar direkt på alla plattformar.

**Förväntat:** `.csproj`-mallen i MS Learn-dokumentationen bör inkludera detta autodetekteringsmönster så att användare slipper komma ihåg `-r`-flaggan.

---

## 4. JavaScript Whisper — Ljudtranskription Returnerar Tomt/ Binärt Output

**Status:** Öppen (regression — försämrats sedan första rapporten)
**Allvar:** Stor
**Komponent:** JavaScript Whisper-implementation (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**Reproduktion:** Kör `node foundry-local-whisper.mjs` — alla ljudfiler returnerar tomt eller binärt output istället för texttranskription

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

Ursprungligen var det bara den femte ljudfilen som returnerade tomt; från och med v0.9.x returnerar alla 5 filer en enda byte (`\ufffd`) istället för transkriberad text. Python Whisper-implementeringen som använder OpenAI SDK transkriberar samma filer korrekt.

**Förväntat:** `createAudioClient()` bör returnera texttranskription som matchar Python/C#-implementationerna.

---

## 5. C# SDK Levererar Endast net8.0 — Ingen Officiell .NET 9 eller .NET 10 Målplattform

**Status:** Öppen
**Allvar:** Dokumentationsbrist
**Komponent:** `Microsoft.AI.Foundry.Local` NuGet-paket v0.9.0
**Installationskommando:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet-paketet levererar endast ett enda målramverk:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

Ingen `net9.0` eller `net10.0` TFM ingår. Däremot levererar komplementpaketet `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472` och `netstandard2.0`.

### Kompatibilitetstestning

| Målramverk | Bygg | Kör | Noteringar |
|------------|------|-----|------------|
| net8.0     | ✅   | ✅  | Officiellt stöd |
| net9.0     | ✅   | ✅  | Bygger via bakåtkompatibilitet — används i workshopexempel |
| net10.0    | ✅   | ✅  | Bygger och kör via bakåtkompatibilitet med .NET 10.0.3-runtime |

net8.0-assemblyn laddas på nyare runtime-versioner genom .NET:s bakåtkompatibilitetsmekanism, så bygget lyckas. Detta är dock odokumenterat och otestat av SDK-teamet.

### Varför Exempeln Målinriktar net9.0

1. **.NET 9 är den senaste stabila releasen** — de flesta workshopdeltagare har det installerat
2. **Bakåtkompatibilitet fungerar** — net8.0-assemblyn i NuGet-paketet körs på .NET 9-runtime utan problem
3. **.NET 10 (preview/RC)** är för ny för att riktas mot i en workshop som ska fungera för alla

**Förväntat:** Framtida SDK-versioner bör överväga att lägga till `net9.0` och `net10.0` TFM tillsammans med `net8.0` för att matcha mönstret i `Microsoft.Agents.AI.OpenAI` och ge validerat stöd för nyare runtime-versioner.

---

## 6. JavaScript ChatClient Streaming Använder Callback, Inte Async Iterators

**Status:** Öppen
**Allvar:** Dokumentationsbrist
**Komponent:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` som returneras av `model.createChatClient()` tillhandahåller en metod `completeStreamingChat()`, men den använder ett **callback-mönster** istället för att returnera en async iterable:

```javascript
// ❌ Detta fungerar INTE — kastar "strömmen är inte asynkront itererbar"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Korrekt mönster — skicka en callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**Påverkan:** Utvecklare som är vana vid OpenAI SDK:s async iterator-mönster (`for await`) kommer att möta förvirrande fel. Callbacken måste vara en giltig funktion annars kastar SDK-felet "Callback must be a valid function."

**Förväntat:** Dokumentera callback-mönstret i SDK-referensen. Alternativt stödja async iterable-mönstret för konsekvens med OpenAI SDK.

---

## Miljödetaljer

| Komponent | Version |
|-----------|---------|
| OS | Windows 11 ARM64 |
| Hårdvara | Snapdragon X Elite (X1E78100) |
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
**Ansvarsfriskrivning**:  
Detta dokument har översatts med hjälp av AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, vänligen var medveten om att automatiska översättningar kan innehålla fel eller oegentligheter. Det ursprungliga dokumentet på dess modersmål bör betraktas som den auktoritativa källan. För kritisk information rekommenderas professionell människoorderad översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår vid användning av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->