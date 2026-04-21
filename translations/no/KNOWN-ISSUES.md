# Kjente problemer — Foundry Local Workshop

Problemer oppdaget under bygging og testing av denne workshopen på en **Snapdragon X Elite (ARM64)** enhet som kjører Windows, med Foundry Local SDK v0.9.0, CLI v0.8.117, og .NET SDK 10.0.

> **Sist validert:** 2026-03-11

---

## 1. Snapdragon X Elite CPU Ikke Gjenkjent av ONNX Runtime

**Status:** Åpen  
**Alvorlighetsgrad:** Advarsel (ikke-blokkerende)  
**Komponent:** ONNX Runtime / cpuinfo  
**Reproduksjon:** Hver gang Foundry Local-tjenesten startes på Snapdragon X Elite maskinvare

Hver gang Foundry Local-tjenesten startes, vises to advarsler:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Innvirkning:** Advarslene er kosmetiske — inferens fungerer korrekt. Men de vises ved hver kjøring og kan forvirre deltakerne i workshopen. ONNX Runtime cpuinfo-biblioteket må oppdateres for å gjenkjenne Qualcomm Oryon CPU-kjerner.

**Forventet:** Snapdragon X Elite skal gjenkjennes som en støttet ARM64-CPU uten å vise feilnivå-meldinger.

---

## 2. SingleAgent NullReferenceException ved Første Kjøring

**Status:** Åpen (sporadisk)  
**Alvorlighetsgrad:** Kritisk (krasj)  
**Komponent:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reproduksjon:** Kjør `dotnet run agent` — krasjer umiddelbart etter modellinnlasting

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Kontekst:** Linje 37 kaller `model.IsCachedAsync(default)`. Krasjet oppstod ved første kjøring av agenten etter en fersk `foundry service stop`. Påfølgende kjøringer med samme kode lyktes.

**Innvirkning:** Sporadisk — tyder på en race-tilstand i SDKs tjenesteinitialisering eller katalogspørring. `GetModelAsync()` kan returnere før tjenesten er helt klar.

**Forventet:** `GetModelAsync()` skal enten blokkere til tjenesten er klar eller returnere en tydelig feilmelding hvis tjenesten ikke er ferdig initialisert.

---

## 3. C# SDK Krever Eksplisitt RuntimeIdentifier

**Status:** Åpen — sporet i [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Alvorlighetsgrad:** Dokumentasjonsmangel  
**Komponent:** `Microsoft.AI.Foundry.Local` NuGet-pakke  
**Reproduksjon:** Lag et .NET 8+ prosjekt uten `<RuntimeIdentifier>` i `.csproj`

Bygget feiler med:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Årsak:** Kravet til RID er forventet — SDK-en inkluderer native binærfiler (P/Invoke inn i `Microsoft.AI.Foundry.Local.Core` og ONNX Runtime), så .NET må vite hvilken plattform-spesifikk bibliotek som skal løses opp.

Dette er dokumentert på MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), der kjøreinstruksjonene viser:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Men brukere må huske `-r` flagget hver gang, noe som er lett å glemme.

**Workaround:** Legg til en auto-deteksjon fallback i `.csproj` slik at `dotnet run` fungerer uten noen flagg:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` er en innebygget MSBuild-egenskap som automatisk løser vertsmaskinens RID. SDK-ens egne testprosjekter bruker allerede dette mønsteret. Eksplisitte `-r` flagg blir fortsatt respektert når de oppgis.

> **Merk:** Workshopens `.csproj` inkluderer denne fallbacken slik at `dotnet run` fungerer rett ut av boksen på hvilken som helst plattform.

**Forventet:** `.csproj`-malen i MS Learn-dokumentasjonen bør inkludere dette autodeteksjonsmønsteret slik at brukere ikke trenger å huske `-r` flagget.

---

## 4. JavaScript Whisper — Lydtranskripsjon Returnerer Tomt/Binar Output

**Status:** Åpen (regresjon — forverret siden første rapport)  
**Alvorlighetsgrad:** Stor  
**Komponent:** JavaScript Whisper-implementasjon (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reproduksjon:** Kjør `node foundry-local-whisper.mjs` — alle lydfiler returnerer tomt eller binært output i stedet for teksttranskripsjon

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Opprinnelig var det kun den 5. lydfilen som returnerte tomt; fra og med v0.9.x returnerer alle 5 filene en enkelt byte (`\ufffd`) i stedet for transkribert tekst. Python Whisper-implementasjonen som bruker OpenAI SDK transkriberer de samme filene korrekt.

**Forventet:** `createAudioClient()` skal returnere teksttranskripsjon som samsvarer med Python/C#-implementasjonene.

---

## 5. C# SDK Sender Kun net8.0 — Ingen Offisiell .NET 9 eller .NET 10 Mål

**Status:** Åpen  
**Alvorlighetsgrad:** Dokumentasjonsmangel  
**Komponent:** `Microsoft.AI.Foundry.Local` NuGet-pakke v0.9.0  
**Install Kommando:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet-pakken inkluderer kun ett mål-rammeverk:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Ingen `net9.0` eller `net10.0` TFM er inkludert. Til sammenligning sender ledsagerpakken `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472`, og `netstandard2.0`.

### Kompatibilitetstesting

| Målrammeverk | Bygg | Kjør | Notater |
|--------------|-------|-----|---------|
| net8.0 | ✅ | ✅ | Offisielt støttet |
| net9.0 | ✅ | ✅ | Bygges via fremoverkompatibilitet — brukt i workshop-eksempler |
| net10.0 | ✅ | ✅ | Bygges og kjører via fremoverkompatibilitet med .NET 10.0.3 runtime |

net8.0-assemblyen lastes på nyere runtime gjennom .NET sin fremoverkompatibilitetsmekanisme, så byggingen lykkes. Dette er likevel ikke dokumentert eller testet av SDK-teamet.

### Hvorfor eksemplene målretter net9.0

1. **.NET 9 er siste stabile utgivelse** — de fleste workshop-deltakere vil ha den installert  
2. **Fremoverkompatibilitet fungerer** — net8.0-assemblyen i NuGet-pakken kjører på .NET 9 runtime uten problemer  
3. **.NET 10 (preview/RC)** er for ny til å målrette i en workshop som skal fungere for alle  

**Forventet:** Fremtidige SDK-utgivelser bør vurdere å legge til `net9.0` og `net10.0` TFM ved siden av `net8.0` for å matche mønsteret brukt av `Microsoft.Agents.AI.OpenAI` og for å tilby validert støtte for nyere runtime.

---

## 6. JavaScript ChatClient Streaming Bruker Callbacks, Ikke Async Iterators

**Status:** Åpen  
**Alvorlighetsgrad:** Dokumentasjonsmangel  
**Komponent:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` som returneres av `model.createChatClient()` tilbyr en `completeStreamingChat()`-metode, men den bruker et **callback-mønster** i stedet for å returnere en async iterable:

```javascript
// ❌ Dette fungerer IKKE — kaster "stream er ikke asynkront itererbar"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Korrekt mønster — send en callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Innvirkning:** Utviklere som er vant til OpenAI SDKs async iterasjonsmønster (`for await`) vil møte forvirrende feil. Callbacken må være en gyldig funksjon ellers kaster SDK-en "Callback must be a valid function."

**Forventet:** Dokumenter callback-mønsteret i SDK-referansen. Alternativt, støtt async iterable-mønsteret for konsistens med OpenAI SDK.

---

## Miljødetaljer

| Komponent | Versjon |
|-----------|---------|
| OS | Windows 11 ARM64 |
| Maskinvare | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |