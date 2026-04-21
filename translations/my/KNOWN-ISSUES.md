# သိရှိထားသောပြဿနာများ — Foundry Local Workshop

Windows တွင် Snapdragon X Elite (ARM64) စက်ပစ္စည်းဖြင့် Foundry Local SDK v0.9.0, CLI v0.8.117 နှင့် .NET SDK 10.0 ကိုအသုံးပြုကာ workshop ကိုတည်ဆောက်ပြီးစမ်းသပ်စဉ်တွေ့ရှိထားသောပြဿနာများ။

> **နောက်ဆုံးအတည်ပြုထားသောရက်စွဲ:** 2026-03-11

---

## 1. Snapdragon X Elite CPU ကို ONNX Runtime မှ မသိရှိခြင်း

**အခြေအနေ:** ဖွင့်ထားသည်  
**ထိခိုက်မှုအဆင့်:** ထိခိုက်မှုမရှိသောသတိပေးချက်  
**အပိုင်း:** ONNX Runtime / cpuinfo  
**ပြန်လည်ထုတ်လုပ်မှု:** Snapdragon X Elite ပစ္စည်းပေါ်၌ Foundry Local service မည်သည့်အချိန်မဆို ဆောင်ရွက်သည်။

Foundry Local service တစ်ခုစတင်တိုင်း သတိပေးချက် နှစ်ချက် ထွက်ပေါ်လာသည်-

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**ထိခိုက်မှု:** သတိပေးချက်များမှာ အလှဆင်ခြင်းသာဖြစ်ပြီး ခန့်မှန်းချက်မှန်ကန်စွာလုပ်ဆောင်နေသည်။ သို့သော် run များတိုင်း တွေ့ရခြင်းကြောင့် workshop ပါဝင်သူများကို ရှင်းလင်းပေးရန်လိုအပ်သည်။ ONNX Runtime cpuinfo စာကြည့်လက်ကူကို Qualcomm Oryon CPU core များသိရှိနိုင်ရန် အပ်ဒိတ် လုပ်ရန်လိုအပ်သည်။

**မျှော်မှန်းထားချက်:** Snapdragon X Elite ကို error-level မက်ဆေ့ခ်် မပေးပဲ ARM64 CPU အနေဖြင့် သုံးနိုင်ပါစေ။

---

## 2. SingleAgent NullReferenceException ပထမတစ်ကြိမ် run ခေါ်သည့်အခါ

**အခြေအနေ:** ဖွင့်ထား (တခါတလေ)  
**ထိခိုက်မှုအဆင့်:** အရေးပေါ် (ကုန်ကျမှု)  
**အပိုင်း:** Foundry Local C# SDK + Microsoft Agent Framework  
**ပြန်လည်ထုတ်လုပ်မှု:** `dotnet run agent` ခေါ်ပါက မော်ဒယ် load ပြီး ချက်ချင်း crash ဖြစ်သည်။

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**သဘောတရား:** line 37 တွင် `model.IsCachedAsync(default)` ကိုခေါ်သည်။ agent ရဲ့ ပထမ run တွင် fresh `foundry service stop` ပြီးနောက် crash ဖြစ်ခဲ့သည်။ နောက်ပိုင်းတွင် အတူတူ code ဖြင့် run များအောင်မြင်ခဲ့သည်။

**ထိခိုက်မှု:** အခါအားလျော်စွာဖြစ်သည်။ SDK service initialization သို့မဟုတ် catalog query တွင် race condition ဖြစ်နိုင်သည်။ `GetModelAsync()` ခေါ်ဆိုမှုသည် service ပြီးစီးမှ မဖြစ်မီ ပြန်ကျော်မှု ဖြစ်နိုင်သည်။

**မျှော်မှန်းထားချက်:** `GetModelAsync()` သည် service ပြီးစီးမှ စောင့်ဆိုင်းပေးပါမည်၊ သို့မဟုတ် service အဆင်မပြေသေးလျှင် error message တိကျသောအရာ ပြန်ပေးရမည်။

---

## 3. C# SDK တွင် RuntimeIdentifier ကို ပေါ်လွင်စွာဖော်ပြရန် လိုအပ်သည်

**အခြေအနေ:** ဖွင့်ထား — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) တွင် လိုက်လံဖြေရှင်း中  
**ထိခိုက်မှုအဆင့်:** စာတမ်းရေးရာ လွတ်ဟပ်မှု  
**အပိုင်း:** `Microsoft.AI.Foundry.Local` NuGet package  
**ပြန်လည်ထုတ်လုပ်မှု:** `.csproj` အတွင်း `<RuntimeIdentifier>` မပါသော .NET 8+ ပရောဂျက်တစ်ခု ဖန်တီးခြင်း

Build မအောင်မြင်ပါ။

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**အကြောင်းရင်းအဓိက:** RID လိုအပ်ချက်သည် မျှော်မှန်းထားသည်။ SDK မှ native binaries (P/Invoke ဖြင့် `Microsoft.AI.Foundry.Local.Core` နှင့် ONNX Runtime) ထုတ်ပေးသောကြောင့်, .NET မှ platform-specific library ကို resolution လုပ်ရမည်။

MS Learn တွင် စာတမ်းထဲ ( [How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp) ) run များသုံးနည်းမှာ -

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
သို့သော် အသုံးပြုသူများသည် `-r` flag ကို တစ်ချိန်ချိန် လုံးမှတ်ရန်လိုအပ်ပြီး မမေ့သင့်ပါ။

**နှိုင်းယှဉ်နည်း:** `.csproj` တွင် auto-detect fallback ထည့်သွင်းပြီး `dotnet run` သည် flag မလိုပဲ အလုပ်လုပ်စေမည်။

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` သည် host စက်၏ RID ကို အလိုအလျောက်ပြန်ဖြေရှင်းသော MSBuild property တစ်ခုဖြစ်သည်။ SDK ၏ မိမိစမ်းသပ်ပရောဂျက်များမှာ ဒီနည်းကိုပုံမှန်အသုံးပြုသည်။ `-r` flag ကိုထည့်သွင်းခြင်းကိုလည်း ကြိုဆိုသည်။

> **မှတ်ချက်:** workshop `.csproj` တွင် fallback ကိုထည့်သွင်းပြီး platform များအပေါ် သက်ဆိုင်ရာ `dotnet run` အလုပ်လုပ်ပါသည်။

**မျှော်မှန်းထားချက်:** MS Learn စာတမ်း၏ `.csproj` template တွင် အလိုအလျောက်ခွဲခြင်းကဏ္ဍပါသင့်၍ အသုံးပြုသူများက `-r` flag ကို မဖောက်မသိမှတ်ရန်ဖြစ်စေပါ။

---

## 4. JavaScript Whisper — အသံမှတ်တမ်းကို အကျဉ်း/Binary ထုတ်လွှင့်ခြင်းဖြစ်နေသည်

**အခြေအနေ:** ဖွင့်ထား (regression — အစပျိုးမှ နောက်ပိုင်းပိုဆိုးလာသည်)  
**ထိခိုက်မှုအဆင့်:** အရေးကြီး  
**အပိုင်း:** JavaScript Whisper အကောင်အထည် (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**ပြန်လည်ထုတ်လုပ်မှု:** `node foundry-local-whisper.mjs` run ပြုလုပ်ပါက အသံဖိုင်များအားလုံးသည် အကျဉ်း သို့မဟုတ် binary output အဖြစ် ထုတ်ပြန်သည်။ အသံစာသားတွေအစား မထုတ်ပေး။

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
အစပိုင်းတွင် ငါးကြိမ်အနက် ပင် ၅ ကြိမ် အကျဉ်းထွက်သော်လည်း၊ v0.9.x မှ စ၍ ဖိုင်ငါးခုစလုံး \ufffd တစ်ဘိုက်သာ ထုတ်ပြန်သည်။ OpenAI SDK အသုံးပြု Python Whisper ကို ဖိုင်တူတူမှန်ကန်စွာ စာသားရရှိသည်။

**မျှော်မှန်းထားချက်:** `createAudioClient()` သည် Python/C# အသုံးပြုမှုများနှင့် ကိုက်ညီသော စာသား မှတ်တမ်း ထုတ်ပေးရပါမည်။

---

## 5. C# SDK သည် net8.0 ဟူသော Target တစ်ခုသာ ပါဝင်ပြီး Official .NET 9 သို့မဟုတ် .NET 10 Target မပါဝင်

**အခြေအနေ:** ဖွင့်ထား  
**ထိခိုက်မှုအဆင့်:** စာတမ်းရေးရာ လွတ်ဟပ်မှု  
**အပိုင်း:** `Microsoft.AI.Foundry.Local` NuGet package v0.9.0  
**ထည့်သွင်းရေးနည်း:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet package သည် target framework တစ်ခုတည်းသာပါရှိသည်-

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
`net9.0` သို့မဟုတ် `net10.0` TFM မပါဝင်ပါ။ အဲဒီနေရာမှာ အတူတူဖြစ်သော `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) package မှာ `net8.0`, `net9.0`, `net10.0`, `net472` နှင့် `netstandard2.0` ပါဝင်သည်။

### ကိုက်ညီမှု စမ်းသပ်ခြင်း

| Target Framework | Build | Run | မှတ်ချက်  |
|-----------------|-------|-----|---------|
| net8.0 | ✅ | ✅ | တရားဝင်ထောက်ခံသည် |
| net9.0 | ✅ | ✅ | forward-compat ဖြင့် build — workshop sample များတွင် အသုံးပြုသည် |
| net10.0 | ✅ | ✅ | .NET 10.0.3 runtime ဖြင့် forward-compat build နှင့် run ဝါးသည် |

net8.0 assembly သည် .NET ၏ forward-compatibility mechanism ဖြင့် နောက်ဆုံး runtime များ၌ load ရနိုင်ပြီး build အောင်မြင်သည်။ ထိုသို့ဖြစ်ခြင်းသည် SDK team မှ မမွမ်းမံခဲ့သော၊ စမ်းသပ်ချက်မလုပ်ခဲ့သေးပါ။

### မည်သို့လေး lock များ net9.0 ကို ရွေးချယ်သနည်း

1. **.NET 9 သည် အဆင့်မြင့် stable release ဖြစ်သည်** — workshop ပါဝင်သူများ အများစု ထည့်ထားလိမ့်မည်  
2. **Forward compatibility အလုပ်လုပ်သည်** — net8.0 assembly သည် .NET 9 runtime အပေါ်တွင် ကျဆင်းမှုမရှိ run နိုင်သည်  
3. **.NET 10 (preview/RC)** သည် workshop နှင့် မလိုက်ဖက်သော အထူးအသစ်ဖြစ်နေသည်

**မျှော်မှန်းထားချက်:** နောက်ထပ် SDK များတွင် `net9.0` နှင့် `net10.0` TFMs ကို `net8.0` နှင့်အတူ ထည့်သွင်း သုံးစွဲသူများအား နောက်ဆုံး runtime များသုံးစွဲခြင်းအတွက် အတည်ပြုထောက်ခံမှု ပေးသင့်သည်။

---

## 6. JavaScript ChatClient Streaming မှာ Async Iterator မဟုတ်ဘဲ Callback ကို သုံးသည်

**အခြေအနေ:** ဖွင့်ထား  
**ထိခိုက်မှုအဆင့်:** စာတမ်းရေးရာ လွတ်ဟပ်မှု  
**အပိုင်း:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` မှ ပြန်လာသော `ChatClient` တွင် `completeStreamingChat()` method ရှိသော်လည်း **callback ပုံစံ** သာသုံးပြီး async iterable မဟုတ်ပါ။

```javascript
// ❌ ဒါဟာ လည်ပတ်မှာ မဟုတ်ပါ — "stream သည် async iterable မဟုတ်ပါ" ဆိုပြီး error ပစ်တယ်
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ မှန်ကန်တဲ့ နမူနာ — callback ကို ပေးပို့ပါ
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**ထိခိုက်မှု:** OpenAI SDK ၏ async iteration ပုံစံ (`for await`) ကို သိသူများအတွက် အမှားများစွာဖြစ်ပေါ်နိုင်သည်။ Callback သည် function ဖြစ်ရမည်၊ မဟုတ်လျှင် SDK က "Callback must be a valid function." error ပေးပါသည်။

**မျှော်မှန်းထားချက်:** SDK မှာ callback ပိုင်းပုံစံကို စာတမ်းများတွင် ဖော်ပြပေးရမည်။ ထို့အပြင် OpenAI SDK နှင့် ကိုက်ညီစေရန် async iterable ပုံစံကိုလည်း ထောက်ပံ့သင့်သည်။

---

## ပတ်ဝန်းကျင် အသေးစိတ်

| အပိုင်း | ဗားရှင်း |
|-----------|---------|
| OS | Windows 11 ARM64 |
| ပစ္စည်းပစ္စယောက် | Snapdragon X Elite (X1E78100) |
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
**သတိပေးချက်**:
ဤစာတမ်းကို AI ဘာသာပြန်ဝန်ဆောင်မှု [Co-op Translator](https://github.com/Azure/co-op-translator) အသုံးပြု၍ ဘာသာပြန်ထားပါသည်။ ကျွန်ုပ်တို့သည်တိကျမှုကိုကြိုးပမ်းပါသော်လည်း အလိုအလျောက်ဘာသာပြန်မှုတွင် အမှားများ သို့မဟုတ် မှားယွင်းမှုများ ပါဝင်နိုင်ကြောင်း ကျေးဇူးပြု၍သတိပြုပါ။ တိုင်းရင်းဘာသာဖြင့် ရေးသားထားသည့် မူရင်းစာတမ်းကို တရားဝင်ရင်းမြစ်အဖြစ်ယူဆရန် လိုအပ်ပါသည်။ အရေးကြီးသော အချက်အလက်များအတွက် သက်ဆိုင်ရာ ပရော်ဖက်ရှင်နယ် လူ့ဘာသာပြန်အား အသုံးပြုရန် အကြံပြုပါသည်။ ဤဘာသာပြန်ချက် အသုံးပြုခြင်းကြောင့် ဖြစ်ပေါ်လာနိုင်သည့် နားမလည်မှုများ သို့မဟုတ် မှားယွင်းဖော်ပြချက်များအတွက် ကျွန်ုပ်တို့အနေဖြင့် တာဝန်မထားရှိပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->