# Bekende problemen — Foundry Local Workshop

Problemen die zijn ondervonden tijdens het bouwen en testen van deze workshop op een **Snapdragon X Elite (ARM64)** apparaat met Windows, met Foundry Local SDK v0.9.0, CLI v0.8.117 en .NET SDK 10.0.

> **Laatst gevalideerd:** 2026-03-11

---

## 1. Snapdragon X Elite CPU wordt niet herkend door ONNX Runtime

**Status:** Open
**Ernst:** Waarschuwing (niet-blokkerend)
**Component:** ONNX Runtime / cpuinfo
**Reproductie:** Elke Foundry Local service start op Snapdragon X Elite hardware

Elke keer dat de Foundry Local service start, worden twee waarschuwingen weergegeven:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Impact:** De waarschuwingen zijn cosmetisch — inferentie werkt correct. Ze verschijnen echter bij elke uitvoering en kunnen deelnemers aan de workshop verwarren. De ONNX Runtime cpuinfo bibliotheek moet worden bijgewerkt om Qualcomm Oryon CPU-kernen te herkennen.

**Verwacht:** Snapdragon X Elite zou moeten worden herkend als een ondersteunde ARM64 CPU zonder foutmeldingen op het foutniveau te genereren.

---

## 2. SingleAgent NullReferenceException bij eerste uitvoering

**Status:** Open (intermitterend)  
**Ernst:** Kritiek (crash)  
**Component:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reproductie:** Voer `dotnet run agent` uit — crasht onmiddellijk na het laden van het model

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Context:** Regel 37 roept `model.IsCachedAsync(default)` aan. De crash deed zich voor bij de eerste uitvoering van de agent na een verse `foundry service stop`. Latere uitvoeringen met dezelfde code slaagden.

**Impact:** Intermitterend — suggereert een raceconditie in de initialisatie van de SDK-service of catalogusquery. De `GetModelAsync()`-aanroep kan terugkeren voordat de service volledig klaar is.

**Verwacht:** `GetModelAsync()` zou moeten blokkeren totdat de service gereed is of een duidelijke foutmelding geven als de service nog niet klaar is.

---

## 3. C# SDK vereist expliciete RuntimeIdentifier

**Status:** Open — geregistreerd in [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Ernst:** Documentatieprobleem  
**Component:** `Microsoft.AI.Foundry.Local` NuGet-pakket  
**Reproductie:** Maak een .NET 8+ project zonder `<RuntimeIdentifier>` in de `.csproj`

Bouw mislukt met:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Oorzaak:** De vereiste van de RID is verwacht — de SDK bevat native binaries (P/Invoke in `Microsoft.AI.Foundry.Local.Core` en ONNX Runtime), dus .NET moet weten welke platform-specifieke bibliotheek moet worden geladen.

Dit is gedocumenteerd op MS Learn ([Hoe native chat completions te gebruiken](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), waar de run-instructies tonen:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Gebruikers moeten echter steeds de `-r` vlag onthouden, wat makkelijk vergeten wordt.

**Workaround:** Voeg een automatische detectie fallback toe aan je `.csproj` zodat `dotnet run` zonder vlaggen werkt:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` is een ingebouwde MSBuild-eigenschap die automatisch de RID van het host-systeem resolveert. De testprojecten van de SDK gebruiken dit patroon al. Expliciete `-r` vlaggen worden nog steeds gerespecteerd wanneer opgegeven.

> **Opmerking:** De workshop `.csproj` bevat deze fallback zodat `dotnet run` out-of-the-box op elk platform werkt.

**Verwacht:** De `.csproj` template in de MS Learn documentatie zou dit automatische detectiepatroon moeten bevatten zodat gebruikers de `-r` vlag niet hoeven te onthouden.

---

## 4. JavaScript Whisper — Audio-transcriptie geeft lege/binaire output terug

**Status:** Open (regressie — verslechterd sinds eerste melding)  
**Ernst:** Groot  
**Component:** JavaScript Whisper-implementatie (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reproductie:** Voer `node foundry-local-whisper.mjs` uit — alle audiobestanden geven lege of binaire output in plaats van teksttranscriptie

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Oorspronkelijk gaf alleen het 5e audiobestand lege output; vanaf v0.9.x geven alle 5 bestanden een enkele byte (`\ufffd`) in plaats van getranscribeerde tekst. De Python Whisper-implementatie met de OpenAI SDK transcribeert dezelfde bestanden correct.

**Verwacht:** `createAudioClient()` zou teksttranscriptie moeten teruggeven die overeenkomt met de Python/C# implementaties.

---

## 5. C# SDK levert alleen net8.0 — geen officiële .NET 9 of .NET 10 doel

**Status:** Open  
**Ernst:** Documentatieprobleem  
**Component:** `Microsoft.AI.Foundry.Local` NuGet-pakket v0.9.0  
**Installatiecommando:** `dotnet add package Microsoft.AI.Foundry.Local`

Het NuGet-pakket levert slechts één doel-framework:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Er is geen `net9.0` of `net10.0` TFM inbegrepen. Ter vergelijking, het begeleidende pakket `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) levert `net8.0`, `net9.0`, `net10.0`, `net472` en `netstandard2.0`.

### Compatibiliteitstest

| Doelframework | Bouwen | Uitvoeren | Opmerkingen |
|---------------|--------|-----------|-------------|
| net8.0 | ✅ | ✅ | Officieel ondersteund |
| net9.0 | ✅ | ✅ | Bouwt via forward-compat — gebruikt in workshopsamples |
| net10.0 | ✅ | ✅ | Bouwt en draait via forward-compat met .NET 10.0.3 runtime |

De net8.0 assembly laadt op nieuwere runtimes via .NET’s forward-compat mechanisme, waardoor de build slaagt. Dit is echter ongedocumenteerd en niet getest door het SDK-team.

### Waarom de samples net9.0 targeten

1. **.NET 9 is de laatste stabiele release** — de meeste workshopdeelnemers hebben dit geïnstalleerd  
2. **Forward compatibility werkt** — de net8.0 assembly in het NuGet-pakket draait goed op de .NET 9 runtime  
3. **.NET 10 (preview/RC)** is te nieuw om te targeten in een workshop die voor iedereen moet werken

**Verwacht:** Toekomstige SDK releases zouden `net9.0` en `net10.0` TFMs naast `net8.0` moeten toevoegen om het patroon van `Microsoft.Agents.AI.OpenAI` te volgen en gevalideerde ondersteuning te bieden voor nieuwere runtimes.

---

## 6. JavaScript ChatClient Streaming gebruikt callbacks, geen async iterators

**Status:** Open  
**Ernst:** Documentatieprobleem  
**Component:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

De `ChatClient` die wordt geretourneerd door `model.createChatClient()` biedt een `completeStreamingChat()` methode, maar gebruikt een **callbackpatroon** in plaats van een async iterable terug te geven:

```javascript
// ❌ Dit werkt NIET — gooit "stream is niet asynchroon iterabel"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Correct patroon — geef een callback mee
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Impact:** Ontwikkelaars die vertrouwd zijn met het async iteratiepatroon (`for await`) van de OpenAI SDK zullen verwarrende fouten tegenkomen. De callback moet een geldige functie zijn, anders gooit de SDK "Callback must be a valid function."

**Verwacht:** Documenteer het callbackpatroon in de SDK-referentie. Alternatief, voeg ondersteuning toe voor het async iterable patroon voor consistentie met de OpenAI SDK.

---

## Omgevingsgegevens

| Component | Versie |
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