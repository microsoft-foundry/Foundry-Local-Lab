# ತಿಳಿದುಬಂದ ಸಮಸ್ಯೆಗಳು — Foundry_Local_Workshop

**Snapdragon X Elite (ARM64)** ಸಾಧನದಲ್ಲಿ Windows ಚಾಲಿತ Foundry Local SDK v0.9.0, CLI v0.8.117 ಮತ್ತು .NET SDK 10.0 ಬಳಸಿ ಈ ವರ್ಕ್‌ಶಾಪ್ ನಿರ್ಮಿಸುವಾಗ ಮತ್ತು ಪರೀಕ್ಷಿಸುವಾಗ ಎದುರಾದ ಸಮಸ್ಯೆಗಳು.

> **ಕೊನೆಯ ದೃಢೀಕರಣ:** 2026-03-11

---

## 1. Snapdragon X Elite CPU ಅನ್ನು ONNX Runtime ಗುರುತಿಸುವುದಿಲ್ಲ

**ಸ್ಥಿತಿ:** ತೆರೆಯಲಾಗಿದೆ  
**ತೀವ್ರತೆ:** ಎಚ್ಚರಿಕೆ (ತಡೆಹಿಡಿಯದ)  
**ಸಂಯೋಜಕ:** ONNX Runtime / cpuinfo  
**ಪುನರಾವರ್ತನೆ:** Snapdragon X Elite ಹಾರ್ಡ್‌ವೇರ್‌ನಲ್ಲಿ ಪ್ರತಿಬಿಟ್ಟು Foundry Local ಸೇವೆ ಪ್ರಾರಂಭವಾಗುವಾಗ

Foundry Local ಸೇವೆ ಪ್ರತಿ ಸಾರಿ ಪ್ರಾರಂಭವಾಗುವಾಗ, ಎರಡು ಎಚ್ಚರಿಕೆಗಳನ್ನು ನೀಡುತ್ತದೆ:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**ಪ್ರಭಾವ:** ಈ ಎಚ್ಚರಿಕೆಗಳು ವಾಸ್ತವವಾಗಿ ಸೌಂದರ್ಯಮಾತ್ರ — ಇನ್ಫರೆನ್ಸ್ ಸರಿಯಾಗಿ ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತದೆ. ಆದಾಗ್ಯೂ, ಅವು ಪ್ರತಿ ಕಾರ್ಯಾಚರಣೆಯಲ್ಲಿ ಕಾಣಿಸಿಕೊಳ್ಳುತ್ತವೆ ಮತ್ತು ವರ್ಕ್‌ಶಾಪ್ ಹಾಜರಾಗುವವರನ್ನು ಗೊಂದಲಕ್ಕೆ ಮುಡುಪಾಗಬಹುದು. Qualcomm Oryon CPU ಕೋರ್‌ಗಳನ್ನು ಗುರುತಿಸಲು ONNX Runtime cpuinfo ಗ್ರಂಥಾಲಯವನ್ನು ನವೀಕರಿಸಲಾಗಬೇಕು.

**ನಿರೀಕ್ಷಿಸಲಾಗಿದೆ:** Snapdragon X Elite ARM64 CPU ಆಗಿ ಗೃಹೀತವಾಗಬೇಕು ಮತ್ತು ದೋಷ-ಮಟ್ಟದ ಸಂದೇಶಗಳನ್ನು ಉಟ್ಟುಕೊಳ್ಳದಿರಬೇಕು.

---

## 2. SingleAgent ಮೊದಲ ಓಟದಲ್ಲಿ NullReferenceException

**ಸ್ಥಿತಿ:** ತೆರೆಯಲಾಗಿದೆ (ಎಲ್ಲಾ ಸಮಯವಲ್ಲ)  
**ತೀವ್ರತೆ:** ಗಂಭೀರ (ಕ್ರ್ಯಾಷ್)  
**ಸಂಯೋಜಕ:** Foundry Local C# SDK + Microsoft Agent Framework  
**ಪುನರಾವರ್ತನೆ:** `dotnet run agent` ಚಾಲನೆ — ಮಾದರಿ ಲೋಡ್ ನಂತರ ತಕ್ಷಣ ಕ್ರ್ಯಾಷ್ ಆಗುತ್ತದೆ

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**ಸಂದರ್ಭ:** ಸಾಲು 37 ರಲ್ಲಿ `model.IsCachedAsync(default)` ಕರೆ. ಕ್ರ್ಯಾಷ್ ಆಗಿದ್ದು, ಮೊದಲ ಬಾರಿ ಏಜೆಂಟ್ ಚಾಲನೆಯಾದಾಗ ಮತ್ತು `foundry service stop` ನಂತರ. ನಂತರದ ಸಾಧನೇಗಳು ಯಶಸ್ವಿಯಾಗಿ ನಡೆದವು.

**ಪ್ರಭಾವ:** ಮಧ್ಯಂತರ — SDK ಸೇವೆಯ ಆರಂಭಿಕೆ ಅಥವಾ ಕ್ಯಾಟಲಾಝ್ ಪ್ರಶ್ನೆಯಲ್ಲಿ ಒಂದು ಸ್ಪರ್ಧಾತ್ಮಕ ಸ್ಥಿತಿ ಕಂಡುಬಂದಿದೆ. `GetModelAsync()` ಕರೆ ಸೇವೆ ಪೂರ್ಣವಾಗಿ ಸಿದ್ಧವಾಗಲು ಮೊದಲು ಹಿಂತಿರುಗಬಹುದು.

**ನಿರೀಕ್ಷಿಸಲಾಗಿದೆ:** `GetModelAsync()` ಸೇವೆ ಸಿದ್ಧವಾಗುವವರೆಗೆ ಕಾಯಬೇಕು ಅಥವಾ ಸೇವೆಯ ಪ್ರಾರಂಭ ಸಿದ್ಧದಾಗದಿದ್ದರೆ ಸ್ಪಷ್ಟ ದೋಷ ಸಂದೇಶವನ್ನು ಕೊಡಬೇಕು.

---

## 3. C# SDKಗೆ ಸ್ಪಷ್ಟ runtimeIdentifier ಬೇಕು

**ಸ್ಥಿತಿ:** ತೆರೆಯಲಾಗಿದೆ — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) ನಲ್ಲಿ ಟ್ರ್ಯಾಕ್ ಆಗುತ್ತಿದೆ  
**ತೀವ್ರತೆ:** ಡಾಕ್ಯುಮೆಂಟೇಷನ್ ಹಿನದುಳಿವುದಾಗಿದೆ  
**ಸಂಯೋಜಕ:** `Microsoft.AI.Foundry.Local` NuGet ಪ್ಯಾಕೇಜ್  
**ಪುನರಾವರ್ತನೆ:** `.csproj` ನಲ್ಲಿ `<RuntimeIdentifier>` ಇಲ್ಲದ .NET 8+ ಪ್ರಾಜೆಕ್ಟ್ ರಚನೆ

ನಿರ್ಮಾಣ ವಿಫಲವಾಗಿದೆ:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**ಮುಖ್ಯ ಕಾರಣ:** RID ಅಗತ್ಯವಿದೆ — SDK ನೆಟಿವ್ ಬೈನರಿಗಳನ್ನು ( `Microsoft.AI.Foundry.Local.Core` ಮತ್ತು ONNX Runtime ಗೆ P/Invoke) ಸಾಗಿಸುತ್ತದೆ, ಆದ್ದರಿಂದ .NET ಗೆ ಯಾವ ಪ್ಲಾಟ್‌ಫಾರ್ಮ್-ನಿರ್ದಿಷ್ಟ ಗ್ರಂಥಾಲಯವನ್ನು ಪರಿಹರಿಸುವುದು ಎಂದು ತಿಳಿದುಕೊಳ್ಳಬೇಕು.

ಇದನ್ನು MS Learn ನಲ್ಲಿ ದಾಖಲಿಸಲಾಗಿದೆ ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), ಚಾಲನೆ ಸೂಚನೆಗಳಲ್ಲಿ:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
ಆದರೆ, ಬಳಕೆದಾರರು ಪ್ರತಿ ಬಾರಿ `-r` ಧ್ವಜವನ್ನು ನೆನಪು ಮಾಡಿಕೊಳ್ಳಬೇಕಾಗುತ್ತದೆ, ಅದು ಮರೆಯಲು ಸುಲಭ.

**ಮಾರ್ಗಾಂಶ:** `.csproj` ಗೆ ಸ್ವಯಂ ಕಂಡುಹಿಡಿಯುವ ಬ್ಯಾಕಪ್ ಸೇರಿಸಿ, ಹಾಗೇ `dotnet run` ಯಾವುದೇ ಧ್ವಜ ಇಲ್ಲದೆ ಕಾರ್ಯನಿರ್ವಹಿಸಲಿ:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` ಒಂದು ಒಳಗಿನ MSBuild ಗುಣಲಕ್ಷಣವಾಗಿದ್ದು, ಆತಿಥ್ಯ ಯಂತ್ರದ RID ಅನ್ನು avtomatic ಆಗಿ ಪರಿಹರಿಸುತ್ತದೆ. SDK ಯ ತಮಗೆ ಸಂಬಂಧಿಸಿದ ಪರೀಕ್ಷಾ ಪ್ರಾಜೆಕ್ಟ್‌ಗಳು ಇದೇ ಮಾದರಿಯನ್ನು ಬಳಸಿವೆ. ಸ್ಪಷ್ಟ `-r` ಧ್ವಜಗಳನ್ನು ನೀಡಿದಾಗ ಇನ್ನೂ ಗೌರವಿಸಲಾಗುತ್ತದೆ.

> **ಗಮನಿಸಿ:** ವರ್ಕ್‌ಶಾಪ್ `.csproj` ಈ ಬ್ಯಾಕಪ್ ಅನ್ನು ಹೊಂದಿದೆ, ಆದ್ದರಿಂದ ಯಾವುದೇ ಪ್ಲಾಟ್‌ಫಾರ್ಮ್‌ನಲ್ಲಿ ಡೊಟ್‌ನೆಟ್ ರನ್ ಸ್ವಯಂಚಾಲಿತವಾಗಿ ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತದೆ.

**ನಿರೀಕ್ಷಿಸಲಾಗಿದೆ:** MS Learn ಡಾಕ್ಯುಮೆಂಟೇಷನ್‌ನ `.csproj` ಟೆಂಪ್ಲೇಟಿನಲ್ಲಿ ಈ ಸ್ವಯಂ-ಕಂಡುಹಿಡಿಯುವ ಮಾದರಿಯನ್ನು ಸೇರಿಸಿ, ಬಳಕೆದಾರರು `-r` ಧ್ವಜವನ್ನು ನೆನಪಿಸಲು ಬೇಡ.

---

## 4. ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ Whisper — ಧ್ವನಿಮಾಹಿತಿಯ ಪಠ್ಯಾಂತರ ಖಾಲಿ/ಬೈನರಿ ಹೊರತಾಗಿದೆ

**ಸ್ಥಿತಿ:** ತೆರೆಯಲಾಗಿದೆ (ಹಿಂದಿನ ವರದಿಯಲ್ಲಿ ಹೀನಾಯ ಮನವೊಲಿಕೆ)  
**ತೀವ್ರತೆ:** ಮುಖ್ಯವಾಗಿ  
**ಸಂಯೋಜಕ:** ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ Whisper ಅನುಷ್ಠಾನ (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**ಪುನರಾವರ್ತನೆ:** `node foundry-local-whisper.mjs` ಚಲಾವಣೆ — ಎಲ್ಲಾ ಧ್ವನಿಮಾಹಿತಿ ಕಡತಗಳು ಖಾಲಿ ಅಥವಾ ಬೈನರಿ ಔಟ್‌ಪುಟ್ ಅನ್ನು ಹಿಂಪಡೆಯುತ್ತವೆ, ಪಠ್ಯಾಂತರ ಬದಲು

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
ಮೊದಲಿಗೆ 5ನೇ ಧ್ವನಿಮಾಹಿತಿ ಮಾತ್ರ ಖಾಲಿ ಆಗಿತ್ತು; v0.9.x ನಿಂದ ಎಲ್ಲಾ 5 ಕಡತಗಳು ಒಂದು ಬೈಟ್ (`\ufffd`) ಅನ್ನು ಹಿಂತಿರುಗಿಸುತ್ತಿವೆ ಪಠ್ಯಾಂತರ ಬದಲು. OpenAI SDK ಬಳಸಿ Python Whisper ಅನುಷ್ಠಾನವು ಅದೇ ಕಡತಗಳನ್ನು ಸರಿಯಾಗಿ ಪಠ್ಯಾಂತರಿಸುತ್ತದೆ.

**ನಿರೀಕ್ಷಿಸಲಾಗಿದೆ:** `createAudioClient()` ಪಠ್ಯಾಂತರವನ್ನು Python/C# ಅನುಷ್ಠಾನಗಳ ಸಮಾನವಾಗಿ ಹಿಂತಿರುಗಿಸಬೇಕು.

---

## 5. C# SDK ಕೇವಲ net8.0 ಜೋಡಣೆ ಮಾಡುತ್ತದೆ — ಅಧಿಕೃತ .NET 9 ಅಥವಾ .NET 10 ಗುರಿ ಇಲ್ಲ

**ಸ್ಥಿತಿ:** ತೆರೆಯಲಾಗಿದೆ  
**ತೀವ್ರತೆ:** ಡಾಕ್ಯುಮೆಂಟೇಷನ್ ಗ್ಯಾಪ್  
**ಸಂಯೋಜಕ:** `Microsoft.AI.Foundry.Local` NuGet ಪ್ಯಾಕೇಜ್ v0.9.0  
**ಸ್ಥಾಪನೆ ಆಜ್ಞೆ:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet ಪ್ಯಾಕೇಜ್ ಕೇವಲ ಒಂದು ಗುರಿ ಪರಿಮಾಣ ಚಲಾವಣೆ:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
`net9.0` ಅಥವಾ `net10.0` TFM ಸೇರಿಲ್ಲ. ಹೋಲಿಕೆಯಾಗಿ, ಹೊಂದಾಣಿಕೆಯ ಪ್ಯಾಕೇಜ್ `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472` ಮತ್ತು `netstandard2.0` ಅನ್ನು ಹೊಂದಿದೆ.

### ಹೊಂದಾಣಿಕೆ ಪರೀಕ್ಷೆ

| ಗುರಿ ಫ್ರೆ임್ವರ್ಕ್ | ನಿರ್ಮಾಣ | ಚಾಲನೆ | ಟಿಪ್ಪಣಿಗಳು |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | ಅಧಿಕೃತವಾಗಿ ಬೆಂಬಲಿತವಾಗಿದೆ |
| net9.0 | ✅ | ✅ | ಫಾರ್ವರ್ಡ್-ಕಂಪ್ಯಾಟಿ ಮೂಲಕ ನಿರ್ಮಾಣ — ವರ್ಕ್‌ಶಾಪ್ ಮಾದರಿಗಳಲ್ಲಿ ಬಳಸಲಾಗಿದೆ |
| net10.0 | ✅ | ✅ | .NET 10.0.3 ರನ್‌ಟೈಮ್ ಮೂಲಕ ಫಾರ್ವರ್ಡ್-ಕಂಪ್ಯಾಟಿ ಮೂಲಕ ನಿರ್ಮಾಣ ಮತ್ತು ಚಾಲನೆ |

net8.0 ಅಸೆಂಬ್ಲಿಯು ಹೊಸ ರನ್‌ಟೈಮ್‌ಗಳಲ್ಲಿ .NET ನ ಫಾರ್ವರ್ಡ್-ಕಂಪ್ಯಾಟಿಬಿಲಿಟಿ ವ್ಯವಸ್ಥೆಯ ಮೂಲಕ ಲೋಡ್ ಆಗುತ್ತಿದ್ದು, ಹೀಗಾಗಿ ನಿರ್ಮಾಣ ಯಶಸ್ವಿಯಾಗುತ್ತದೆ. ಆದಾಗ್ಯೂ, ಇದು SDK ತಂಡದಿಂದ ದಾಖಲಾಗದೇ ಹಾಗೂ ಪರೀಕ್ಷೆಯಾಗದೇ ಇದೆ.

### ಮಾದರಿಗಳು net9.0 ಗುರಿ ಯಾಕೆ?

1. **.NET 9 ಇತ್ತೀಚಿನ ಸ್ಥಿರ ಬಿಡುಗಡೆ** — ಬಹುತೇಕ ವರ್ಕ್‌ಶಾಪ್ ಭಾಗವಹಿಸುವವರು ಇದನ್ನು ಸ್ಥಾಪಿಸಿದ್ದಾರೆ  
2. **ಫಾರ್ವರ್ಡ್-ಕಂಪ್ಯಾಟಿ ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತದೆ** — NuGet ಪ್ಯಾಕೇಜ್‌ನ net8.0 ಅಸೆಂಬ್ಲಿ .NET 9 ರನ್‌ಟೈಮ್‌ನಲ್ಲಿ ಸುಲಭವಾಗಿ ಓಡುತ್ತದೆ  
3. **.NET 10 (ಪ್ರೀವ್ಯೂ/RC)** ಎಲ್ಲರಿಗೂ ಕೆಲಸ ಮಾಡುವ ವರ್ಕ್‌ಶಾಪ್ ಗುರಿಯಾಗಲು ಇನ್ನೂ ಹೆಚ್ಚು ಹೊಸದಾಗಿದೆ

**ನಿರೀಕ್ಷಿಸಲಾಗಿದೆ:** ಭವಿಷ್ಯದ SDK ಬಿಡುಗಡೆಗಳು `net9.0` ಮತ್ತು `net10.0` TFMs ಗಳು `net8.0` ಜೊತೆಗೆ ಸೇರಿಸಲು ಪರಿಗಣಿಸಬೇಕು, ಮತ್ತು `Microsoft.Agents.AI.OpenAI` ಬಳಸಿರುವ ಮಾದರಿಯಂತೆ, ಹೊಸ ರನ್‌ಟೈಮ್‌ಗಳಿಗೆ ದೃಢೀಕೃತ ಬೆಂಬಲ ನೀಡಲು.

---

## 6. ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ChatClient ಸ್ಟ್ರೀಮಿಂಗ್ ಕಲ್ಬ್ಯಾಕ್ಸ್ ಬಳಸುತ್ತದೆ, Async Iterators ಅಲ್ಲ

**ಸ್ಥಿತಿ:** ತೆರೆಯಲಾಗಿದೆ  
**ತೀವ್ರತೆ:** ಡಾಕ್ಯುಮೆಂಟೇಷನ್ ಗ್ಯಾಪ್  
**ಸಂಯೋಜಕ:** `foundry-local-sdk` ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` ನೀಡುವ `ChatClient` ನಲ್ಲಿ `completeStreamingChat()` ವಿಧಾನವಿದೆ, ಆದರೆ ಅದು **ಕಾಲ್‌ಬ್ಯಾಕ್ ಮಾದರಿಯನ್ನು** ಬಳಸಿ, async iterable ಅನ್ನು ಹಿಂತಿರುಗಿಸುವುದಿಲ್ಲ:

```javascript
// ❌ ಇದು ಕಾರ್ಯನಿರ್ವಹಿಸುವುದಿಲ್ಲ — "stream is not async iterable" ಎಂದು ತಳ್ಳುತ್ತದೆ
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ ಸರಿಯಾದ ಮಾದರಿ — callback ಅನ್ನು ಪಾಸ್ ಮಾಡಿ
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**ಪ್ರಭಾವ:** OpenAI SDK ಯ async iteration (`for await`) ಮಾದರಿಯನ್ನು ಪರಿಚಿತರು ಗೊಂದಲಕಾರಿ ದೋಷಗಳನ್ನು ಎದುರಿಸುತ್ತಾರೆ. ಕಲ್ಬ್ಯಾಕ್ ದಾಖಲೆ ನಿಜವಾದ ಫಂಕ್ಷನ್ ಆಗಿರಬೇಕು ಇಲ್ಲದಿದ್ದರೆ SDK "Callback must be a valid function." ಎಂದು ಎರ್ರರ್ ಹಿಂಪಡೆಯುತ್ತದೆ.

**ನಿರೀಕ್ಷಿಸಲಾಗಿದೆ:** SDK ರೆಫರೆನ್ಸ್‌ನಲ್ಲಿ ಕಲ್ಬ್ಯಾಕ್ ಮಾದರಿಯನ್ನು ದಾಖಲಿಸಬೇಕು. ಬಲಪಟ್ಟು OpenAI SDK ಯ async iterable ಮಾದರಿಯನ್ನು ಬೆಂಬಲಿಸುವ ಆಯ್ಕೆಯನ್ನು ನೀಡುವುದು ಉತ್ತಮ.

---

## ಪರಿಸರ ವಿವರಗಳು

| ಸಂಯೋಜಕ | ಆವೃತ್ತಿ |
|-----------|---------|
| OS | Windows 11 ARM64 |
| ಹಾರ್ಡ್‌ವೇರ್ | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |