# ਜਾਣੀ-ਪਹਚਾਣੀ ਸਮੱਸਿਆਵਾਂ — Foundry Local ਵਰਕਸ਼ਾਪ

ਇਸ ਵਰਕਸ਼ਾਪ ਨੂੰ **Snapdragon X Elite (ARM64)** ਉਪਰ Windows 'ਤੇ ਚਲਾਉਂਦੇ ਸਮੇਂ Foundry Local SDK v0.9.0, CLI v0.8.117 ਅਤੇ .NET SDK 10.0 ਨਾਲ ਬਣਾਉਣ ਅਤੇ ਟੈਸਟ ਕਰਨ ਦੌਰਾਨ ਸਾਹਮਣਾ ਹੋਈ ਸਮੱਸਿਆਵਾਂ।

> **ਆਖਰੀ ਵੈਰੀਫਾਈਡ:** 2026-03-11

---

## 1. Snapdragon X Elite CPU ਨੂੰ ONNX Runtime ਵੱਲੋਂ ਪਛਾਣਿਆ ਨਹੀਂ ਗਿਆ

**ਸਤਿਤੀ:** ਖੁੱਲਾ  
**ਗੰਭੀਰਤਾ:** ਚੇਤਾਵਨੀ (ਗੈਰ-ਰੋਕੂ)  
**ਕੰਪੋਨੇਟ:** ONNX Runtime / cpuinfo  
**ਦੁਹਰਾਵਣਾ:** Snapdragon X Elite ਹਾਰਡਵੇਅਰ 'ਤੇ ਹਰ Foundry Local ਸਰਵਿਸ ਦੀ ਸ਼ੁਰੂਆਤ 'ਤੇ

Foundry Local ਸਰਵਿਸ ਦੀ ਹਰ ਸ਼ੁਰੂਆਤ 'ਤੇ ਦੋ ਚੇਤਾਵਨੀਆਂ ਜਾਰੀ ਕੀਤੀਆਂ ਜਾਂਦੀਆਂ ਹਨ:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**ਅਸਰ:** ਚੇਤਾਵਨੀਆਂ ਸਿਰਫ਼ ਖੂਬਸੂਰਤੀ ਲਈ ਹਨ — ਇਨਫਰੈਂਸ ਸਹੀ ਤਰ੍ਹਾਂ ਕੰਮ ਕਰਦਾ ਹੈ। ਪਰ ਇਹ ਹਰ ਦੌੜ 'ਤੇ ਆਉਂਦੀਆਂ ਹਨ, ਜੋ ਵਰਕਸ਼ਾਪ ਵਿੱਚ ਭਾਗ ਲੈਣ ਵਾਲਿਆਂ ਲਈ ਭ੍ਰਮਿਤਕ ਹੈ। ONNX Runtime ਦੀ cpuinfo ਲਾਇਬ੍ਰੇਰੀ ਨੂੰ Qualcomm Oryon CPU ਕੋਰਾਂ ਨੂੰ ਪਛਾਣਨ ਲਈ ਅਪਡੇਟ ਕਰਨ ਦੀ ਲੋੜ ਹੈ।  

**ਉਮੀਦ:** Snapdragon X Elite ਨੂੰ ਸਹਾਇਤਾਵਿਦ ARM64 CPU ਵਜੋਂ ਪਛਾਣਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ ਬਿਨਾਂ ਕਿਸੇ ਗਲਤੀ-ਪੱਧਰੀ ਸਨੇਹੇ ਦੇ।  

---

## 2. SingleAgent NullReferenceException ਪਹਿਲੀ ਦੌੜ 'ਤੇ

**ਸਤਿਤੀ:** ਖੁੱਲਾ (ਕਦੇ-ਕਦੇ)  
**ਗੰਭੀਰਤਾ:** ਅਤੱਖੜ (ਕ੍ਰੈਸ਼)  
**ਕੰਪੋਨੇਟ:** Foundry Local C# SDK + Microsoft Agent Framework  
**ਦੁਹਰਾਵਣਾ:** `dotnet run agent` ਚਲਾਓ — ਮਾਡਲ ਲੋਡ ਹੋਣ ਦੇ ਤੁਰੰਤ ਬਾਅਦ ਕ੍ਰੈਸ਼

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**ਸੰਦਰਭ:** ਲਾਈਨ 37 `model.IsCachedAsync(default)` ਨੂੰ ਕਾਲ ਕਰਦੀ ਹੈ। ਇਹ ਕ੍ਰੈਸ਼ ਏਜੰਟ ਦੀ ਪਹਿਲੀ ਦੌੜ 'ਤੇ ਵਾਪਰਿਆ ਸੀ ਜਦੋਂ ਤਾਜ਼ਾ `foundry service stop` ਤੋਂ ਬਾਅਦ ਚਲਾਇਆ ਗਿਆ। ਮਗਰੋਂ ਵਾਲੀਆਂ ਦੌੜਾਂ ਸਫਲ ਰਹੀਆਂ।  

**ਅਸਰ:** ਕਦੇ-ਕਦੇ — ਇਨ੍ਹਾਂ ਨਿਸ਼ਾਨਿਆਂ ਦੇ ਅਨੁਸਾਰ SDK ਦੇ ਸਰਵਿਸ ਸ਼ੁਰੂਆਤ ਜਾਂ ਕੈਟਲੋਗ ਕ੍ਵੈਰੀ ਵਿੱਚ ਰੇਸ ਸਥਿਤੀ ਹੋ ਸਕਦੀ ਹੈ। `GetModelAsync()` ਕਾਲ ਸਰਵਿਸ ਪੂਰੀ ਤਰ੍ਹਾਂ ਤਿਆਰ ਹੋਣ ਤੋਂ ਪਹਿਲਾਂ ਹੀ ਵਾਪਸ ਆ ਸਕਦੀ ਹੈ।  

**ਉਮੀਦ:** `GetModelAsync()` ਨੂੰ ਚਾਹੀਦਾ ਹੈ ਕਿ ਜਾਂ ਤਾਂ ਸਰਵਿਸ ਤਿਆਰ ਹੋਣ ਤੱਕ ਰੁਕੇ ਜਾਂ ਜੇ ਸਰਵਿਸ ਹੋਰ ਤੇਅਰ ਨਹੀਂ ਹੈ ਤਾਂ ਸਪਸ਼ਟ ਗਲਤੀ ਸੁਨੇਹਾ ਵਾਪਸ ਕਰੇ।  

---

## 3. C# SDK ਨੂੰ Explicit RuntimeIdentifier ਦੀ ਲੋੜ ਹੈ

**ਸਤਿਤੀ:** ਖੁੱਲਾ — ਟ੍ਰੈਕ ਕੀਤਾ ਜਾ ਰਿਹਾ ਹੈ [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**ਗੰਭੀਰਤਾ:** ਦਸਤਾਵੇਜ਼ ਸਮੱਸਿਆ  
**ਕੰਪੋਨੇਟ:** `Microsoft.AI.Foundry.Local` NuGet ਪੈਕੇਜ  
**ਦੁਹਰਾਵਣਾ:** `.csproj` ਵਿੱਚ `<RuntimeIdentifier>` ਬਿਨਾਂ .NET 8+ ਪ੍ਰੋਜੈਕਟ ਬਣਾਓ

ਬਿਲਡ ਅਸਫਲ ਹੁੰਦਾ ਹੈ:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**ਮੂਲ ਕਾਰਨ:** RID ਦੀ ਲੋੜ ਉਮੀਦ ਕੀਤੀ ਜਾਂਦੀ ਹੈ — SDK ਦੇ ਨੇਟਿਵ ਬਾਇਨਰੀਜ਼ ਸ਼ਿਪ ਹੁੰਦੇ ਹਨ (P/Invoke `Microsoft.AI.Foundry.Local.Core` ਅਤੇ ONNX Runtime ਵਿੱਚ), ਇਸ ਲਈ .NET ਨੂੰ ਪਲੇਟਫਾਰਮ-ਖਾਸ ਲਾਇਬ੍ਰੇਰੀ ਦਾ ਪਤਾ ਹੋਣਾ ਚਾਹੀਦਾ ਹੈ।  

ਇਹ MS Learn ’ਤੇ ਦਸਤਾਵੇਜ਼ ਕੀਤਾ ਗਿਆ ਹੈ ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)) ਜਿੱਥੇ ਚਲਾਉਣ ਦੇ ਹੁਕਮ ਦਿਖਾਏ ਗਏ ਹਨ:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
ਹਾਲਾਂਕਿ, ਉਪਭੋਗਤਾਵਾਂ ਨੂੰ ਹਰ ਵਾਰ `-r` ਫਲੈਗ ਨੂੰ ਯਾਦ ਰੱਖਣਾ ਪੈਂਦਾ ਹੈ, ਜੋ ਕਿ ਭੁਲਾਣਾ ਆਸਾਨ ਹੈ।  

**ਤਰਕ:** ਆਪਣੀ `.csproj` ਵਿੱਚ ਆਟੋ-ਡਿਟੈਕਟ ਫਾਲਬੈਕ ਸ਼ਾਮਿਲ ਕਰੋ ਤਾਂ ਜੋ `dotnet run` ਸਾਰੇ ਫਲੈਗਾਂ ਬਿਨਾਂ ਕੰਮ ਕਰੇ:  

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` MSBuild ਦੀ ਬਣੀ ਬਣਾਈ ਪ੍ਰਾਪਰਟੀ ਹੈ ਜੋ ਹੋਸਟ ਮਸ਼ੀਨ ਦਾ RID ਆਪੇ ਹੀ ਹੱਲ ਕਰਦਾ ਹੈ। SDK ਦੇ ਆਪਣੇ ਟੈਸਟ ਪ੍ਰੋਜੈਕਟ ਪਹਿਲਾਂ ਹੀ ਇਸ ਪੈਟਰਨ ਨੂੰ ਵਰਤਦੇ ਹਨ। ਜਦੋਂ `-r` ਫਲੈਗ ਦਿੱਤਾ ਜਾਂਦਾ ਹੈ ਤਾਂ ਉਹ ਸਤਿਕਾਰ ਕਰਦੀਆਂ ਹਨ।  

> **ਦਿਆਨ:** ਵਰਕਸ਼ਾਪ `.csproj` ਵਿੱਚ ਇਹ ਫਾਲਬੈਕ ਹੈ ਤਾਂ ਜੋ `dotnet run` ਕਿਸੇ ਵੀ ਪਲੇਟਫਾਰਮ 'ਤੇ ਬਣਦਾ ਹੈ।  

**ਉਮੀਦ:** MS Learn ਦਸਤਾਵੇਜ਼ਾਂ ਵਿੱਚ `.csproj` ਟੈਂਪਲੇਟ ਨੂੰ ਇਹ ਆਟੋ-ਡਿਟੈਕਟ ਪੈਟਰਨ ਸ਼ਾਮਿਲ ਕਰਨਾ ਚਾਹੀਦਾ ਹੈ ਤਾਂ ਜੋ ਉਪਭੋਗਤਾਵਾਂ ਨੂੰ `-r` ਫਲੈਗ ਯਾਦ ਰੱਖਣ ਦੀ ਲੋੜ ਨਾ ਪਵੇ।  

---

## 4. ਜਾਵਾਸਕ੍ਰਿਪਟ ਵਿਸਪਰ — ਆਡੀਓ ਟ੍ਰਾਂਸਕ੍ਰਿਸ਼ਨ ਖਾਲੀ/ਬਾਈਨਰੀ ਨਤੀਜਾ ਦਿੰਦਾ ਹੈ

**ਸਤਿਤੀ:** ਖੁੱਲਾ (ਰੇਗਰੈਸ਼ਨ — ਸ਼ੁਰੂਆਤੀ ਰਿਪੋਰਟ ਤੋਂ ਬਦਤਰ)  
**ਗੰਭੀਰਤਾ:** ਮੁੱਖ  
**ਕੰਪੋਨੇਟ:** ਜਾਵਾਸਕ੍ਰਿਪਟ ਵਿਸਪਰ ਕਾਰਜਾਂਸ਼ (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**ਦੁਹਰਾਵਣਾ:** `node foundry-local-whisper.mjs` ਚਲਾਓ — ਸਾਰੇ ਆਡੀਓ ਫਾਇਲਾਂ ਟੈਕਸਟ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਬਜਾਏ ਖਾਲੀ ਜਾਂ ਬਾਈਨਰੀ ਨਤੀਜਾ ਦਿੰਦੀਆਂ ਹਨ  

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
ਮੂਲ ਤੌਰ 'ਤੇ ਸਿਰਫ ਪੰਜਵੀਂ ਆਡੀਓ ਫਾਇਲ ਖਾਲੀ ਆਉਂਦੀ ਸੀ; ਪਰ v0.9.x ਤੋਂ ਸਾਰੇ 5 ਫਾਇਲਾਂ ਇੱਕ ਬਾਈਟ (`\ufffd`) ਵਾਪਸ ਕਰਦੀਆਂ ਹਨ ਜਦਕਿ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕੀਤੇ ਟੈਕਸਟ ਦੀ ਥਾਂ। ਪਾਇਥਨ ਵਿਸਪਰ OpenAI SDK ਵਰਤਦਾ ਟੈਸਟ ਕੀਤੇ ਫਾਇਲਾਂ ਨੂੰ ਠੀਕ ਟ੍ਰਾਂਸਕ੍ਰਾਇਬ ਕਰਦਾ ਹੈ।  

**ਉਮੀਦ:** `createAudioClient()` ਨੂੰ ਪਾਇਥਨ/ਸੀ# ਇੰਪਲੀਮੈਂਟੇਸ਼ਨਾਂ ਦੇ ਸੰਗਤ ਟੈਕਸਟ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਵਾਪਸ ਕਰਨੀ ਚਾਹੀਦੀ ਹੈ।  

---

## 5. C# SDK ਸਿਰਫ net8.0 ਨੂੰ ਸ਼ਿਪ ਕਰਦਾ ਹੈ — ਅਧਿਕਾਰਿਕ .NET 9 ਜਾਂ .NET 10 ਟਾਰਗਟ ਨਹੀਂ

**ਸਤਿਤੀ:** ਖੁੱਲਾ  
**ਗੰਭੀਰਤਾ:** ਦਸਤਾਵੇਜ਼ ਸਮੱਸਿਆ  
**ਕੰਪੋਨੇਟ:** `Microsoft.AI.Foundry.Local` NuGet ਪੈਕੇਜ v0.9.0  
**ਇੰਸਟਾਲ ਹੁਕਮ:** `dotnet add package Microsoft.AI.Foundry.Local`  

NuGet ਪੈਕੇਜ ਸਿਰਫ ਇੱਕ ਟਾਰਗਟ ਫ੍ਰੇਮਵਰਕ ਸ਼ਿਪ ਕਰਦਾ ਹੈ:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
`net9.0` ਜਾਂ `net10.0` TFM ਸ਼ਾਮਿਲ ਨਹੀਂ। ਇਸਦੇ ਮੁਕਾਬਲੇ ਸਾਥੀ ਪੈਕੇਜ `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472`, ਅਤੇ `netstandard2.0` ਸ਼ਿਪ ਕਰਦਾ ਹੈ।  

### ਮੈਲ ਖਾਣ ਦੀ ਟੈਸਟਿੰਗ

| ਟਾਰਗਟ ਫ੍ਰੇਮਵਰਕ | ਬਿਲਡ | ਚਲਾਉ | ਟਿੱਪਣੀਆਂ |  
|-----------------|--------|-------|----------|  
| net8.0          | ✅     | ✅    | ਅਧਿਕਾਰਿਕ ਤੌਰ ਤੇ ਸਹਾਇਤਾਪ੍ਰਾਪਤ |  
| net9.0          | ✅     | ✅    | ਫਾਰਵਰਡ-ਕੰਪੈਟ ਵਰਤ ਕੇ ਬਿਲਡ ਹੁੰਦਾ ਹੈ — ਵਰਕਸ਼ਾਪ ਸੈਂਪਲਾਂ ਵਿੱਚ ਵਰਤਿਆ ਗਿਆ |  
| net10.0         | ✅     | ✅    | .NET 10.0.3 ਰਨਟਾਈਮ ਨਾਲ ਫਾਰਵਰਡ-ਕੰਪੈਟ ਸਹਾਇਤਾ ਨਾਲ ਬਿਲਡ ਅਤੇ ਚਲਦਾ ਹੈ |  

net8.0 ਅਸੰਬਲੀ ਨਵੇਂ ਰਨਟਾਈਮਾਂ 'ਤੇ .NET ਦੇ ਫਾਰਵਰਡ-ਕੰਪੈਟ ਮਕੈਨਿਜ਼ਮ ਰਾਹੀਂ ਲੋਡ ਹੁੰਦੀ ਹੈ, ਇਸ ਕਰਕੇ ਬਿਲਡ ਸਫਲ ਹੁੰਦਾ ਹੈ। ਹਾਲਾਂਕਿ, ਇਹ SDK ਟੀਮ ਵੱਲੋਂ ਗੈਰ-ਦਸਤਾਵੇਜ਼ਤ ਅਤੇ ਗੈਰ-ਟੈਸਟ ਹੈ।  

### ਸੈਂਪਲਾਂ ਕਿਉਂ net9.0 ਟਾਰਗਟ ਕਰਦੇ ਹਨ

1. **.NET 9 ਨਵੀਨਤਮ ਸਥਿਰ ਵਰਜਨ ਹੈ** — ਬਹੁਤ ਸਾਰੇ ਵਰਕਸ਼ਾਪ ਭਾਗੀਦਾਰਾਂ ਕੋਲ ਇਹ ਹੋਵੇਗਾ  
2. **ਫਾਰਵਰਡ ਕੰਪੈਟ ਐਬੇਜ ਕਰਦਾ ਹੈ** — NuGet ਪੈਕੇਜ ਵਿੱਚ ਲੁਕਾਇਆ net8.0 ਅਸੰਬਲੀ .NET 9 ਰਨਟਾਈਮ 'ਤੇ ਸਹੀ ਚਲਦੀ ਹੈ  
3. **.NET 10 (ਪ੍ਰੀਵਿਊ/RC)** ਬਹੁਤ ਨਵਾਂ ਹੈ ਜੋ ਸਾਰੇ ਲਈ ਵਰਕਸ਼ਾਪ ਵਿੱਚ ਟਾਰਗਟ ਨਹੀਂ ਕੀਤਾ ਜਾ ਸਕਦਾ  

**ਉਮੀਦ:** ਭਵਿੱਖ ਦੇ SDK ਰਿਲੀਜ਼ਾਂ ਨੂੰ `net9.0` ਅਤੇ `net10.0` TFMs `net8.0` ਦੇ ਨਾਲ ਸ਼ਾਮਿਲ ਕਰਨ ਬਾਰੇ ਸੋਚਣਾ ਚਾਹੀਦਾ ਹੈ, ਜਿਸਨਾਲ `Microsoft.Agents.AI.OpenAI` ਵਰਗੇ ਪੈਟਰਨ ਦੇ ਤੌਰ ਤੇ ਮੁਹੱਈਆ ਕੀਤੀ ਗਈ ਸਹਾਇਤਾ ਨਵੇਂ ਰਨਟਾਈਮਾਂ ਲਈ ਪ੍ਰਮਾਣਿਤ ਸਹਾਇਤਾ ਦੇਣਾ ਪ੍ਰਭਾਵਸ਼ਾਲੀ ਬਣੇ।  

---

## 6. ਜਾਵਾਸਕ੍ਰਿਪਟ ChatClient ਸਟ੍ਰੀਮਿੰਗ ਕਾਲਬੈਕ ਵਰਤਦੀ ਹੈ, Async Iterators ਨਹੀਂ

**ਸਤਿਤੀ:** ਖੁੱਲਾ  
**ਗੰਭੀਰਤਾ:** ਦਸਤਾਵੇਜ਼ ਸਮੱਸਿਆ  
**ਕੰਪੋਨੇਟ:** `foundry-local-sdk` ਜਾਵਾਸਕ੍ਰਿਪਟ v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` ਵੱਲੋਂ ਵਾਪਸ ਕੀਤਾ ਗਿਆ `ChatClient` ਇੱਕ `completeStreamingChat()` ਮੈਥਡ ਦਿੰਦਾ ਹੈ, ਪਰ ਇਹ async iterable ਵਾਪਸ ਕਰਨ ਦੇ ਬਜਾਏ **ਕਾਲਬੈਕ ਪੈਟਰਨ** ਵਰਤਦਾ ਹੈ:

```javascript
// ❌ ਇਹ ਕੰਮ ਨਹੀਂ ਕਰਦਾ — "ਸਟਰੀਮ ਆਸਿੰਕ ਆਈਟਰੇਬਲ ਨਹੀਂ ਹੈ" ਦਿੱਖਦਾ ਹੈ
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ ਸਹੀ ਪੈਟਰਨ — ਇੱਕ ਕਾਲਬੈਕ ਪਾਸ ਕਰੋ
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**ਅਸਰ:** ਜੋ ਵਿਕਾਸਕਾਰ OpenAI SDK ਦੇ async iteration ਪੈਟਰਨ (`for await`) ਨਾਲ ਜਾਣੂ ਹਨ, ਉਹ ਗਲਤੀਆਂ ਨਾਲ ਮੁਕਾਬਲਾ ਕਰਨਗੇ। ਕਾਲਬੈਕ ਨੂੰ ਸਹੀ ਫੰਕਸ਼ਨ ਹੋਣਾ ਜਰੂਰੀ ਹੈ ਨਹੀਂ ਤਾਂ SDK "Callback must be a valid function." ਫੇਲ੍ਹ ਦਿੰਦਾ ਹੈ।  

**ਉਮੀਦ:** SDK ਰੈਫ਼ਰੈਂਸ ਵਿੱਚ ਕਾਲਬੈਕ ਪੈਟਰਨ ਦੀ ਦਸਤਾਵੇਜ਼ਤਾ ਕਰੋ। ਵਿਕਲਪ ਵਜੋਂ, OpenAI SDK ਨਾਲ ਸੰਗਤੀ ਬਣਾਈ ਰੱਖਣ ਲਈ async iterable ਪੈਟਰਨ ਲਈ ਸਹਾਇਤਾ ਦਿਓ।  

---

## ਵਾਤਾਵਰਨ ਵੇਰਵੇ

| ਕੰਪੋਨੇਟ                | ਵਰਜ਼ਨ        |  
|------------------------|--------------|  
| OS                     | Windows 11 ARM64 |  
| ਹਾਰਡਵੇਅਰ               | Snapdragon X Elite (X1E78100) |  
| Foundry Local CLI       | 0.8.117      |  
| Foundry Local SDK (C#)  | 0.9.0        |  
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3  |  
| OpenAI C# SDK           | 2.9.0        |  
| .NET SDK                | 9.0.312, 10.0.104 |  
| foundry-local-sdk (Python) | 0.5.x     |  
| foundry-local-sdk (JS)  | 0.9.x        |  
| Node.js                 | 18+          |  
| ONNX Runtime            | 1.18+        |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਇਸ ਕਾਰਜ ਨੂੰ ਧਿਆਨ ਵਿੱਚ ਰੱਖਦੇ ਹੋਏ**:
ਇਹ ਦਸਤਾਵੇਜ਼ AI ਅਨੁਵਾਦ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੁਆਰਾ ਅਨੁਵਾਦ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਕਿ ਅਸੀਂ ਸਹੀਤਾ ਲਈ ਯਤਨ ਕਰਦੇ ਹਾਂ, ਕਿਰਪਾ ਕਰਕੇ ਧਿਆਨ ਵਿੱਚ ਰੱਖੋ ਕਿ ਸਵੈਚਲਿਤ ਅਨੁਵਾਦਾਂ ਵਿੱਚ ਗਲਤੀਆਂ ਜਾਂ ਅਸਪਸ਼ਟਤਾ ਹੋ ਸਕਦੀ ਹੈ। ਮੂਲ ਦਸਤਾਵੇਜ਼ ਆਪਣੇ ਮੂਲ ਭਾਸ਼ਾ ਵਿੱਚ ਅਧਿਕਾਰਕ ਸਰੋਤ ਮੰਨਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਮਹੱਤਵਪੂਰਨ ਜਾਣਕਾਰੀ ਲਈ ਪੇਸ਼ੇਵਰ ਮਨੁੱਖੀ ਅਨੁਵਾਦ ਦੀ ਸਿਫਾਰਿਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ। ਇਸ ਅਨੁਵਾਦ ਦੀ ਵਰਤੋਂ ਤੋਂ ਹੋਣ ਵਾਲੀਆਂ ਕਿਸੇ ਵੀ ਗਲਤਫਹਿਮੀਆਂ ਜਾਂ ਬੁਝਣਾਂ ਲਈ ਅਸੀਂ ਜ਼ਿੰਮੇਵਾਰ ਨਹੀਂ ਹਾਂ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->