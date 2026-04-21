# தெரிந்த பிரச்சனைகள் — Foundry லோகல் தொழிற்கூடம்

**Snapdragon X Elite (ARM64)** சாதனத்தில் Windows இயக்குநிலையில், Foundry Local SDK v0.9.0, CLI v0.8.117, மற்றும் .NET SDK 10.0 உடன் இந்த தொழிற்கூடத்தை உருவாக்கியதில் மற்றும் சோதனை செய்ததில் ஏற்பட்ட பிரச்சனைகள்.

> **கடைசியாக சரிபார்க்கப்பட்ட தேதி:** 2026-03-11

---

## 1. Snapdragon X Elite CPU ONNX Runtime மூலம் அங்கீகரிக்கப்படவில்லை

**நிலை:** திறந்தது  
**தீவிரம்:** எச்சரிக்கை (மூடாத)  
**கூறுகள்:** ONNX Runtime / cpuinfo  
**மீண்டும் உருவாக்கம்:** Snapdragon X Elite இயந்திரத்தில் ஒவ்வொரு Foundry Local சேவை துவக்கம்

எல்லா முறையும் Foundry Local சேவை துவங்கும் போது இரண்டு எச்சரிக்கைகள் வெளியாகின்றன:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**பிடிப்புக் குறைவு:** எச்சரிக்கைகள் தோற்ற அமைப்பு குறிப்பு — கணிப்பு சரியாக இயங்குகிறது. இருப்பினும், அவை ஒவ்வொரு சுற்றிலும் தோன்றுவதால் தொழிற்கூட உறுப்பினர்களுக்கு குழப்பம் ஏற்படுகிறது. ONNX Runtime cpuinfo நூலகம் Qualcomm Oryon CPU மையங்களை அங்கீகரிக்க புதுப்பிக்கப்பட வேண்டும்.

**எதிர்பார்ப்பு:** Snapdragon X Elite ஒரு ஆதரிக்கப்பட்ட ARM64 CPU ஆக அங்கீகரிக்கப்பட வேண்டும், பிழை நிலை செய்திகளை வெளியிடாமல்.

---

## 2. SingleAgent NullReferenceException முதல் ஓட்டத்தில்

**நிலை:** திறந்தது (இடைவேளை)  
**தீவிரம்:** கடுமையானது (கடி முடிவு)  
**கூறுகள்:** Foundry Local C# SDK + Microsoft Agent Framework  
**மீண்டும் உருவாக்கம்:** `dotnet run agent` இயக்கவும் — மாதிரி ஏற்றிய பிறகு உடனடி கடிவாய்ப்பு

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**சூழல்:** வரி 37 இல் `model.IsCachedAsync(default)` அழைக்கப்படுகிறது. இந்த கடிவாய்ப்பு முதற்கண் agent ஆரம்பிப்பதில் ஏற்பட்டது, புதிதாக `foundry service stop` பிறகு. பின்வரும் ஓட்டங்கள் ஒரே குறியீட்டுடன் வெற்றிகரமாக நடந்தன.

**பிடிப்பு:** இடைவேளை — SDK சேவை தொடக்கத்தில் அல்லது பட்டியல் விசாரணையில் ஓர் போட்டி நிலையை உணர்த்துகிறது. `GetModelAsync()` சேவை முழுமையாக தயாராகாதவரை முன்னதாக திருப்பலாம்.

**எதிர்பார்ப்பு:** சேவை தயார் ஆகும் வரை `GetModelAsync()` தடுமாற வேண்டும் அல்லது சேவை இன்னும் ஆரம்பமாகவில்லை என்ற தெளிவான பிழை செய்தியுடன் திரும்ப வேண்டும்.

---

## 3. C# SDK இல் தெளிவான RuntimeIdentifier தேவை

**நிலை:** திறந்தது — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) இல் கண்காணிப்பு  
**தீவிரம்:** ஆவண இடைவேளை  
**கூறுகள்:** `Microsoft.AI.Foundry.Local` NuGet பேக்கேஜ்  
**மீண்டும் உருவாக்கம்:** `.csproj` இல் `<RuntimeIdentifier>` இணைக்காமல் .NET 8+ திட்டம் உருவாக்குதல்

கட்டமைப்பு தோல்வி:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**மூலம்:** RID தேவை உறுதியானது — SDK இயந்திர இனப்படுத்திய பைனரிகள் (P/Invoke மூலம் `Microsoft.AI.Foundry.Local.Core` மற்றும் ONNX Runtime) வழங்குகிறது, எனவே .NET கண்டறிய வேண்டியதுவே எந்த தள விசேஷ நூலகம்.

இது MS Learn ஆவணத்தில் ([native chat completions எப்படி பயன்படுத்துவது](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)) விவரிக்கப்படுகிறது, ஓட்டும் கட்டளைகள்:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
ஆனால், பயனர்கள் ஒவ்வொரு முறையும் `-r` கொடுப்பதை மறக்கக்கூடியது.

**தற்காலிகதீர்வு:** `.csproj` இல் தானாக கண்டறியும் பதிலாக சேர்க்கவும், இதனால் `dotnet run` எந்த கொடுப்புகளும் இல்லாமல் இயக்கும்:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` என்பது உள்ளமைவு MSBuild சொத்து, அது இயந்திரத்தின் RID ஐ தானாக தீர்மானிக்கிறது. SDK-வின் சொந்த சோதனை திட்டங்கள் இந்த முறையை ஏற்கனவே பயன்படுத்துகின்றன. தெளிவான `-r` கொடுப்புகள் வழங்கப்பட்டால் அவை நிலைத்திருக்கும்.

> **குறிப்பு:** தொழிற்கூட `.csproj` இல் இந்த fallback அடங்கியுள்ளது; எனவே எந்த தளத்திலும் `dotnet run` அசல் அலகாக செய்யும்.

**எதிர்பார்ப்பு:** MS Learn ஆவண `.csproj` வார்ப்புரு இந்த தானாக கண்டறிதல் முறையை அடங்க வேண்டும், பயனர்கள் `-r` கொடுப்பதை நினைவுகூர வேண்டாமாக செய்ய.

---

## 4. JavaScript Whisper — ஆடியோ மாற்றம் காலியாக அல்லது பைனரி வெளியீடு தருகிறது

**நிலை:** திறந்தது (முன்னைய செய்தி விட மோசமாகி உள்ளது)  
**தீவிரம்:** முக்கியம்  
**கூறுகள்:** JavaScript Whisper செயலாக்கம் (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**மீண்டும் உருவாக்கம்:** `node foundry-local-whisper.mjs` இயக்கவும் — அனைத்து ஆடியோ கோப்புகளிலும் உரை மாற்றத்தின் பதிலாக காலி அல்லது பைனரி வெளியீடு

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
ஆரம்பத்தில் ஐந்தாம் ஆடியோ கோப்பே காலியாக இருந்தது; v0.9.x இல், ஐந்திலும் தனி பைட் (`\ufffd`) வரும், உரையாற்றல் மூலம் அல்ல. Python Whisper OpenAI SDK பயன்படுத்தி விதிகளை சரியாக மார்க்கசெய்கிறது.

**எதிர்பார்ப்பு:** `createAudioClient()` Python/C# வடிவுப்போன்ற உரை மாற்றத்தை திருப்ப வேண்டும்.

---

## 5. C# SDK net8.0 மட்டுமே அனுப்புகிறது — அதிகாரப்பூர்வ .NET 9 அல்லது .NET 10 இலக்கு இல்லை

**நிலை:** திறந்தது  
**தீவிரம்:** ஆவண இடைவேளை  
**கூறுகள்:** `Microsoft.AI.Foundry.Local` NuGet v0.9.0  
**நிறுவும் கட்டளை:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet பேக்கேஜ் ஒரே இலக்கு கட்டமைப்பு மட்டும் அனுப்புகிறது:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
`net9.0` அல்லது `net10.0` TFM இல்லை. மாறாக, துணை பேக்கேஜ் `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472`, மற்றும் `netstandard2.0` அனுப்புகிறது.

### பொருந்துதன்மை சோதனை

| இலக்கு கட்டமைப்பு | கட்டமை | இயக்க | குறிப்புகள் |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | அதிகாரப்பூர்வ ஆதரவு |
| net9.0 | ✅ | ✅ | முன்னுதவி மூலம் கட்டமைக்கிறது — தொழிற்கூட மாதிரிகளில் பயன்படுத்தப்படுகிறது |
| net10.0 | ✅ | ✅ | .NET 10.0.3 இயக்க மோசடி மூலம் கட்டமைக்கிறது மற்றும் இயக்குகிறது |

net8.0 தொகுப்பு புதிய இயக்கிகளில் .NET முன்னுதவி முறையாக ஏற்றுகிறது, ஆகையால் கட்டமைப்பு வெற்றி பெறுகிறது. ஆனால், SDK அணி இதனை ஆவணப்படுத்தவில்லை மற்றும் சோதிக்கவில்லை.

### மாதிரிகள் net9.0 இலக்கை ஏன் தேர்ந்தெடுக்கும்

1. **.NET 9 என்பது சமீபத்திய நிலையான வெளியீடு** — பெரும்பான்மையான தொழிற்கூட உறுப்பினர்களுக்கு நிறுவப்பட்டுள்ளது  
2. **முன்னுதவி வேலை செய்கிறது** — NuGet பேக்கேஜ் net8.0 தொகுப்பின் .NET 9 இயக்கி மேல் சிக்கலின்றி இயங்குகிறது  
3. **.NET 10 (முன்விளக்கம்/RC)** அனைவருக்கும் வேலை செய்யும் தொழிற்கூடு ஆக சிக்கலுடன் இருக்கிறது  

**எதிர்பார்ப்பு:** எதிர்கால SDK வெளியீடுகள் `Microsoft.Agents.AI.OpenAI` போல `net9.0` மற்றும் `net10.0` TFM-களை `net8.0` உடன் சேர்க்க பரிசீலிக்க வேண்டும், புதிய இயக்கிகள் ஆதரவு உறுதி செய்யப்பட தேவையுள்ளது.

---

## 6. JavaScript ChatClient ஒளிபரப்பு கால் பேக்களோடு செய்கிறது, Async Iterators அல்ல

**நிலை:** திறந்தது  
**தீவிரம்:** ஆவண இடைவேளை  
**கூறுகள்:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` மூலம் வரும் `ChatClient` க்கு `completeStreamingChat()` என்ற முறை உள்ளது, ஆனால் அது ஒரு **callback முறை** பயன்படுத்துகிறது, async iterable ஆகவில்லை:

```javascript
// ❌ இது வேலை செய்யாது — "stream is not async iterable" என்ற தவறை உருவாக்குகிறது
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ சரியான மாதிரியானது — ஒரு callback ஐ வழங்கவும்
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**பிடிப்பு:** OpenAI SDK இன் async iteration முறைக்கு (`for await`) பழகிய டெவலப்பர்கள் குழப்ப கருத்தொடர்பில் பிழைகள் சந்திக்கலாம். Callback சரியான செயல்பாடு ஆக இருக்க வேண்டும் இல்லையெனில் SDK "Callback must be a valid function." என்ற பிழையை உருவாக்கும்.

**எதிர்பார்ப்பு:** SDK குறிப்புகளில் callback முறை ஆவணப்படுத்த வேண்டும். அல்லது async iterable முறை OpenAI SDK உடன் ஒப்போன்று ஆதரிக்க வேண்டும்.

---

## சுற்றுச்சூழல் விவரங்கள்

| கூறு | பதிப்பு |
|-----------|---------|
| OS | Windows 11 ARM64 |
| சாதனம் | Snapdragon X Elite (X1E78100) |
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
**பதிவு விடுவிப்பு**:  
இந்த ஆவணம் AI மொழிபெயர்ப்பு சேவை [Co-op Translator](https://github.com/Azure/co-op-translator) をப் பயன்படுத்தி மொழிபெயர்க்கப்பட்டுள்ளது. நாங்கள் துல்லியத்திற்காக முயற்சிக்கும் போதிலும், தானியங்கி மொழிபெயர்ப்புகளில் பிழைகள் அல்லது தவறான தகவல்கள் இருக்கக்கூடும் என்பதை தயவுசெய்து கவனத்தில் கொள்ளவும். அதன் செவ்விய மொழியில் உள்ள மாறாத ஆவணம் அதிகாரப்பூர்வ மூலமாகக் கருதப்பட வேண்டும். முக்கியமான தகவல்களுக்கு, தொழில்முறை மனித மொழிபெயர்ப்பை பரிந்துரைக்கிறோம். இந்த மொழிபெயர்ப்பைப் பயன்படுத்துவதால் ஏற்படும் எந்த விதமான தவறான புரிதல்களுக்கும் அல்லது தவறான விளக்கங்களுக்கும் நாம் பொறுப்பேற்கமாட்டோம்.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->