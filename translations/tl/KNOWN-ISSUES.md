# Mga Kilalang Isyu — Foundry Local Workshop

Mga isyu na naranasan habang binubuo at tine-test ang workshop na ito sa isang **Snapdragon X Elite (ARM64)** na device na tumatakbo sa Windows, gamit ang Foundry Local SDK v0.9.0, CLI v0.8.117, at .NET SDK 10.0.

> **Huling na-validate:** 2026-03-11

---

## 1. Hindi Nakikilala ang Snapdragon X Elite CPU ng ONNX Runtime

**Status:** Bukas  
**Seryosidad:** Babala (hindi pumipigil)  
**Komponente:** ONNX Runtime / cpuinfo  
**Pag-ulit:** Bawat pagsisimula ng Foundry Local service sa Snapdragon X Elite hardware

Sa bawat pagsisimula ng Foundry Local service, dalawang babala ang lumalabas:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Epekto:** Kosmetiko lamang ang mga babala — tama naman ang pag-infer. Subalit, lumalabas ang mga ito sa bawat takbo at maaaring malito ang mga kalahok sa workshop. Kailangang i-update ang ONNX Runtime cpuinfo library upang makilala ang Qualcomm Oryon CPU cores.

**Inaasahan:** Dapat nakikilala ang Snapdragon X Elite bilang suportadong ARM64 CPU nang hindi naglalabas ng error-level na mga mensahe.

---

## 2. SingleAgent NullReferenceException sa Unang Takbo

**Status:** Bukas (paminsan-minsan)  
**Seryosidad:** Kritikal (crash)  
**Komponente:** Foundry Local C# SDK + Microsoft Agent Framework  
**Pag-ulit:** Patakbuhin ang `dotnet run agent` — agad na nag-crash matapos mag-load ng model

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Konteksto:** Tinatawag ng Linya 37 ang `model.IsCachedAsync(default)`. Nag-crash ito sa unang takbo ng agent pagkatapos ng bagong `foundry service stop`. Matagumpay naman ang mga sumusunod na takbo sa parehong code.

**Epekto:** Paminsan-minsan — nagpapahiwatig ng race condition sa service initialisation o catalog query ng SDK. Maaaring tumugon ang `GetModelAsync()` bago handa ang service.

**Inaasahan:** Dapat mag-block ang `GetModelAsync()` hanggang handa na ang service o maglabas ng malinaw na error message kung hindi pa tapos ang initialisation.

---

## 3. Kinakailangan ng C# SDK ang Eksaktong RuntimeIdentifier

**Status:** Bukas — sinusubaybayan sa [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Seryosidad:** Kakulangan sa dokumentasyon  
**Komponente:** `Microsoft.AI.Foundry.Local` NuGet package  
**Pag-ulit:** Gumawa ng .NET 8+ na proyekto nang walang `<RuntimeIdentifier>` sa `.csproj`

Nagkakaroon ng error sa build:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Pinagmulan:** Inaasahan ang RID requirement — naglalaman ang SDK ng mga native binaries (P/Invoke sa `Microsoft.AI.Foundry.Local.Core` at ONNX Runtime), kaya kailangan malaman ng .NET kung aling platform-specific na library ang dapat i-resolve.

Nakatala ito sa MS Learn ([Paano gamitin ang native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), kung saan ipinapakita ang mga tagubilin sa pagpapatakbo:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Ngunit kailangang alalahanin ng mga user ang `-r` flag sa bawat takbo, na madaling makalimutan.

**Solusyon:** Magdagdag ng auto-detect fallback sa iyong `.csproj` upang gumana ang `dotnet run` nang walang flags:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
Ang `$(NETCoreSdkRuntimeIdentifier)` ay built-in na MSBuild property na awtomatikong nagreresolba sa RID ng host machine. Ginagamit na ito ng mga test project ng SDK. Pinapahalagahan pa rin ang eksaktong `-r` flags kapag ibinigay.

> **Paalala:** Kasama sa workshop `.csproj` ang fallback na ito kaya’t gumagana agad ang `dotnet run` sa kahit anong platform.

**Inaasahan:** Dapat isama ang pattern na ito sa `.csproj` template ng MS Learn upang hindi na kailanganin ng users na alalahanin ang `-r` flag.

---

## 4. JavaScript Whisper — Naghahatid ng Empty/Binary Output ang Audio Transcription

**Status:** Bukas (regresyon — lumala mula unang report)  
**Seryosidad:** Malaki  
**Komponente:** JavaScript Whisper implementation (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Pag-ulit:** Patakbuhin ang `node foundry-local-whisper.mjs` — lahat ng audio files ay nagbabalik ng empty o binary output sa halip na text transcription

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Noong una, empty lang ang ika-5 audio file; sa v0.9.x, lahat ng 5 files ay nagbabalik ng isang byte (`\ufffd`) sa halip na na-transcribe na teksto. Tama naman ang transcription sa Python Whisper implementation gamit ang OpenAI SDK.

**Inaasahan:** Dapat magbalik ang `createAudioClient()` ng text transcription na pareho sa Python/C# implementations.

---

## 5. Naglalaman ang C# SDK ng net8.0 Lamang — Walang Official na Target para sa .NET 9 o .NET 10

**Status:** Bukas  
**Seryosidad:** Kakulangan sa dokumentasyon  
**Komponente:** `Microsoft.AI.Foundry.Local` NuGet package v0.9.0  
**Install command:** `dotnet add package Microsoft.AI.Foundry.Local`

Naglalaman lamang ang NuGet package ng isang target framework:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Walang `net9.0` o `net10.0` TFM na kasama. Sa kabilang banda, ang kaakibat na package na `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) ay naglalaman ng `net8.0`, `net9.0`, `net10.0`, `net472`, at `netstandard2.0`.

### Pagsubok sa Compatibility

| Target Framework | Build | Run | Tala |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | Opisyal na suportado |
| net9.0 | ✅ | ✅ | Naayos gamit ang forward-compat — ginagamit sa mga sample ng workshop |
| net10.0 | ✅ | ✅ | Nag-build at tumatakbo gamit ang forward-compat sa .NET 10.0.3 runtime |

Nila-load ang net8.0 assembly sa mas bagong mga runtime sa pamamagitan ng forward-compatibility ng .NET, kaya nagtatagumpay ang build. Ngunit hindi ito dokumentado o nasubok ng SDK team.

### Bakit Target ng Mga Sample ang net9.0

1. **.NET 9 ang pinakabagong stable release** — karamihan ng mga kalahok sa workshop ay mayroon nito
2. **Gumagana ang forward compatibility** — tumatakbo ang net8.0 assembly sa NuGet package sa .NET 9 runtime nang walang problema
3. **.NET 10 (preview/RC)** ay masyadong bago para gawing target sa isang workshop na kailangan gumana para sa lahat

**Inaasahan:** Dapat isaalang-alang ng mga susunod na release ng SDK ang pagdagdag ng `net9.0` at `net10.0` TFMs kasabay ng `net8.0` upang tumugma sa pattern ng `Microsoft.Agents.AI.OpenAI` at magbigay ng validadong suporta para sa mas bagong mga runtime.

---

## 6. Ang JavaScript ChatClient Streaming ay Gumagamit ng Callbacks, Hindi Async Iterators

**Status:** Bukas  
**Seryosidad:** Kakulangan sa dokumentasyon  
**Komponente:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

Ang `ChatClient` na ibinabalik ng `model.createChatClient()` ay mayroong `completeStreamingChat()` method, ngunit gumagamit ito ng **callback pattern** sa halip na magbalik ng async iterable:

```javascript
// ❌ Hindi ito gumagana — naglalabas ng "stream is not async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Tamang paraan — magpasa ng callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Epekto:** Ang mga developer na sanay sa async iteration pattern ng OpenAI SDK (`for await`) ay makakaranas ng nakalilitong mga error. Kailangang valid na function ang callback o maglalabas ang SDK ng error na "Callback must be a valid function."

**Inaasahan:** Didokumento ang callback pattern sa SDK reference. Bilang alternatibo, suportahan ang async iterable pattern para sa pagkakapareho sa OpenAI SDK.

---

## Detalye ng Kapaligiran

| Komponente | Bersyon |
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
**Paunawa**:  
Ang dokumentong ito ay isinalin gamit ang AI translation service na [Co-op Translator](https://github.com/Azure/co-op-translator). Habang sinisikap naming maging tumpak, pakitandaan na maaaring may mga pagkakamali o hindi pagkakatugma ang mga awtomatikong salin. Ang orihinal na dokumento sa kanyang orihinal na wika ang dapat ituring na pangunahing pinagkukunan. Para sa mahahalagang impormasyon, ipinapayo ang propesyonal na salin ng tao. Hindi kami mananagot sa anumang hindi pagkakaunawaan o maling interpretasyon na nagmula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->