# తెలుసుకున్న సమస్యలు — Foundry Local వర్క్‌షాప్

Windows మీద Snapdragon X Elite (ARM64) పరికరంపై Foundry Local SDK v0.9.0, CLI v0.8.117, మరియు .NET SDK 10.0తో ఈ వర్క్‌షాప్ నిర్మాణం మరియు పరీక్ష చేసే సమయంలో ఎదురైన సమస్యలు.

> **చివరిసారి ధృవీకరించబడింది:** 2026-03-11

---

## 1. Snapdragon X Elite CPU ను ONNX Runtime గుర్తించదు

**స్థితి:** తెరిచి ఉంది  
**తీవ్రత:** హెచ్చరిక (అవరోధం కలిగించదని)  
**కంపోనెంట్:** ONNX Runtime / cpuinfo  
**పునః ఉత్పత్తి:** ప్రతి సారి Snapdragon X Elite హార్డ్‌వేర్‌పై Foundry Local సేవ ప్రారంభం

Foundry Local సేవ ప్రతి సారి ప్రారంభించినప్పుడు రెండు హెచ్చరికలు వస్తాయి:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**ప్రభావం:** హెచ్చరికలు రూపకల్పనాత్మకంగా మాత్రమే ఉంటాయి — నిర్ధారణ సరిగ్గా పనిచేస్తోంది. అయితే, అవి ప్రతి పరుగులో కనిపిస్తాయి మరియు వర్క్‌షాప్ పాల్గొనేవారిని గందరగోळలో పడేస్తాయి. Qualcomm Oryon CPU కోర్లను గుర్తించడానికి ONNX Runtime cpuinfo లైబ్రరీని నవీకరించాలి.

**అందుకున్న ఆశయం:** Snapdragon X Elite ARM64 CPU గా గమనించబడాలి, తప్పుల స్థాయి సందేశాలు వస్తుండకూడదు.

---

## 2. SingleAgent మొదటి పరుగులో NullReferenceException

**స్థితి:** తెరిచి ఉంది (మధ్యతరగతి)  
**తీవ్రత:** కీలక (క్రాష్)  
**కంపోనెంట్:** Foundry Local C# SDK + Microsoft Agent Framework  
**పునః ఉత్పత్తి:** `dotnet run agent` అమలు చేయండి — మోడల్ లోడ్ తర్వాత వెంటనే క్రాష్ అవుతుంది

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**సందర్భం:** లైన్ 37లో `model.IsCachedAsync(default)` కాల్ జరుగుతుంది. క్రాష్ Foundry సేవను తాజా `foundry service stop` తర్వాత ఏజెంట్ మొదటి రన్‌లో జరిగింది. అదే కోడ్‌తో తరువాత రన్లు విజయవంతం అయ్యాయి.

**ప్రభావం:** మధ్యతరగతి — SDK సేవ ప్రారంభంలో లేదా క్యాటలాగ్ క్వెరీలో రేస్ కండిషన్ సూచన. `GetModelAsync()` సేవ పూర్తిగా సిద్ధం కాక ముందు తిరిగి వచ్చి ఉండవచ్చు.

**అందుకున్న ఆశయం:** `GetModelAsync()` సేవ సిద్ధం కావటానికి బ్లాక్ చేయాలి లేదా సేవ ప్రారంభం కాలేదు అయితే స్పష్టమైన తప్పిద సందేశాన్ని ఇవ్వాలి.

---

## 3. C# SDKకి స్పష్టమైన RuntimeIdentifier అవసరం

**స్థితి:** తెరిచి ఉంది — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)లో ట్రాక్ చేయబడుతోంది  
**తీవ్రత:** డాక్యుమెంటేషన్ లో వ్యత్యాసం  
**కంపోనెంట్:** `Microsoft.AI.Foundry.Local` NuGet ప్యాకేజ్  
**పునః ఉత్పత్తి:** `.csproj`లో `<RuntimeIdentifier>` లేకుండా .NET 8+ ప్రాజెక్ట్ సృష్టించాలి

బిల్డ్ విఫలమవుతుంది:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**మూల కారణం:** RID అవసరం అనుకున్నది — SDK స్థానిక బైనరీలను పంపిణీ చేస్తుంది (P/Invoke ద్వారా `Microsoft.AI.Foundry.Local.Core` మరియు ONNX Runtimeకి), కాబట్టి .NETకు ఏ ప్లాట్‌ఫాం-ప్రత్యేక లైబ్రరీని రిజాల్వ్ చేయాలో తెలుసుకోవాలి.

ఇది MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp))లో డాక్యుమెంట్స్‌లో ఉంది, అందులో అమలుశీర్షికలు ఇలా ఉంటాయి:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
అయితే, వినియోగదారులు ప్రతి సారి `-r` ఫ్లాగ్ మర్చిపోకుండా ఉండాలి, ఇది సులభం కాదు.

**తాత్కాలిక పరిష్కారం:** `.csproj`లో ఆటో-డిటెక్ట్ ప్యాటర్న్ జోడించండి, తద్వారా `dotnet run`కు ఎలాంటి ఫ్లాగ్ అవసరం లేకుండా పనిచేస్తుంది:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` ఒక ప్లగ్-ఇన్ MSBuild ప్రాపర్టీ, ఇది హోస్ట్ మెషీన్ యొక్క RIDని ఆటోమేటిక్ గా పరిష్కరిస్తుంది. SDK యొక్క ప్రయోగాలు ఇప్పటికే ఈ పద్ధతిని ఉపయోగిస్తున్నాయి. స్పష్టమైన `-r` ఫ్లాగులు అందించినపుడు కూడా గౌరవించబడతాయి.

> **గమనిక:** వర్క్‌షాప్ `.csproj`లో ఈ ఆటో-డిటెక్ట్ ప్యాటర్న్ జోడించబడింది, అందువల్ల `dotnet run` ఏ ప్లాట్‌ఫార్‌మ్‌పైనైనా ఫలితంగా పనిచేస్తుంది.

**అందుకున్న ఆశయం:** MS Learn డాక్స్ `.csproj` టెంప్లేట్ ఈ ఆటో-డిటెక్ట్ పద్ధతిని కలిగి ఉండాలి, అందువల్ల యూజర్లు `-r` ఫ్లాగ్ మర్చిపోవడం లేదు.

---

## 4. JavaScript Whisper — ఆడియో ట్రాన్స్క్రిప్షన్ ఖాళీ/బైనరీ అవుట్పుట్ ఇస్తుంది

**స్థితి:** తెరిచి ఉంది (పునరావృతం — ప్రాథమిక నివేదిక తర్వాత మరింత పడ్డింది)  
**తీవ్రత:** ప్రధానమైనది  
**కంపోనెంట్:** JavaScript Whisper అమలు (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**పునః ఉత్పత్తి:** `node foundry-local-whisper.mjs` నడపండి — అన్ని ఆడియో ఫైళ్ళు ఖాళీ లేదా బైనరీ అవుట్పుట్ ఇస్తున్నాయి, టెక్స్ట్ ట్రాన్స్క్రిప్షన్ తప్పుగా

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
మొదటగా 5వ ఆడియో ఫైల్ మాత్రమే ఖాళీగా వస్తుండేది; v0.9.x నాటికి అన్ని 5 ఫైళ్ళు ఒకే బైట్ (`\ufffd`) ఇస్తున్నాయి, ట్రాన్స్క్రైబ్ టెక్స్ట్ కాదిగా. Python Whisper అమలు OpenAI SDK ఉపయోగించి అదే ఫైళ్ళను సరిగా ట్రాన్స్క్రائب చేస్తుంది.

**అందుకున్న ఆశయం:** `createAudioClient()` Python/C# అమలులతో సరిపోయే టెక్స్ట్ ట్రాన్స్క్రిప్షన్ ఇవ్వాలి.

---

## 5. C# SDK కేవలం net8.0 మాత్రమే పంపిణీ చేస్తుంది — అధికారిక .NET 9 లేదా .NET 10 లక్ష్యం లేదు

**స్థితి:** తెరిచి ఉంది  
**తీవ్రత:** డాక్యుమెంటేషన్ లో వ్యత్యాసం  
**కంపోనెంట్:** `Microsoft.AI.Foundry.Local` NuGet ప్యాకేజ్ v0.9.0  
**ఇన్‌స్టాల్ కమాండ్:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet ప్యాకేజ్ కేవలం ఒకే లక్ష్య ఫ్రేమ్‌వర్క్‌ను పంపిణీ చేస్తుంది:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
`net9.0` లేదా `net10.0` TFM లు లేవు. బదులుగా సహచర ప్యాకేజ్ `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472`, మరియు `netstandard2.0`ను పంపిణీ చేస్తుంది.

### అనుకూలత పరీక్ష

| లక్ష్య ఫ్రేమ్‌వర్క్ | బిల్డ్ | నడపడం | గమనికలు |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | అధికారికంగా మద్దతు ఉంటుంది |
| net9.0 | ✅ | ✅ | ఫార్వర్డ్-కంపాటితో బిల్డ్ అవుతుంది — వర్క్‌షాప్ నమూనాల్లో ఉపయోగిస్తారు |
| net10.0 | ✅ | ✅ | .NET 10.0.3 రన్‌టైమ్తో ఫార్వర్డ్-కంపాటితో బిల్డ్ అవుతుంది మరియు నడుస్తుంది |

net8.0 అసెంబ్లీ కొత్త రన్‌టైమ్‌లపై .NET ఫార్వర్డ్-కంపాటిబిలిటీ ద్వారా లోడ్ అవుతుంది, కాబట్టి బిల్డ్ విజయవంతం అవుతుంది. అయితే, SDK బృందం ఇది డాక్యుమెంటేషన్ చేయలేదు మరియు పరీక్షించలేదు.

### నమూనాలు net9.0 లక్ష్యం ఎందుకు

1. **.NET 9 తాజా స్థిర విడుదల** — చాలా వర్క్‌షాప్ పాల్గొనేవారు దీనిని ఇన్‌స్టాల్ చేసుకున్నారు  
2. **ఫార్వర్డ్ కంపాటబిలిటీ పనిచేస్తుంది** — NuGetలో net8.0 అసెంబ్లీ .NET 9 రన్‌టైమ్‌పై సరిగా నడుస్తుంది  
3. **.NET 10 (పరిశీలన/RC)** చాలా కొత్తది, అందువల్ల ప్రతిఒక్కరికీ పనిచేసే వర్క్‌షాప్ లక్ష్యంగా తీసుకోవడం కష్టం

**అందుకున్న ఆశయం:** భవిష్యత్ SDK విడుదలలు `net9.0` మరియు `net10.0` TFMsను `net8.0`తో పాటు చేర్చాలని పరిగణించాలి, ఇది `Microsoft.Agents.AI.OpenAI` లో ఉపయోగించే పద్ధతికి సరిపోతుంది మరియు కొత్త రన్‌టైమ్‌లకు ధృవీకరించబడిన మద్దతు అందిస్తుంది.

---

## 6. JavaScript ChatClient Streaming కాల్‌బ్యాక్‌లు వాడుతుంది, Async Iterators కాదు

**స్థితి:** తెరిచి ఉంది  
**తీవ్రత:** డాక్యుమెంటేషన్ లో వ్యత్యాసం  
**కంపోనెంట్:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` ద్వారా ఇచ్చే `ChatClient` ఒక `completeStreamingChat()` పద్ధతిని అందిస్తుంది, కానీ async iterable కాకుండా **callback ప్యాటర్న్** ఉపయోగిస్తుంది:

```javascript
// ❌ ఇది పనిచెయ్యదు — "stream is not async iterable" అని తప్పు చూపిస్తుంది
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ సరైన నమూనా — కాల్‌బ్యాక్‌ను పాస్ చేయండి
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**ప్రభావం:** OpenAI SDK async iteration ప్యాటర్న్ (`for await`)కు పరిచయమున్న డెవలపర్లు గందరగోళమైన తప్పిదాలు ఎదుర్కొంటారు. కాల్‌బ్యాక్ సరైన ఫంక్షన్ కాకపోతే SDK "Callback must be a valid function." అని ఎర్రర్ ఇస్తుంది.

**అందుకున్న ఆశయం:** SDK రిఫరెన్స్‌లో కాల్‌బ్యాక్ ప్యాటర్న్ డాక్యుమెంట్ చేయాలి. లేదా, OpenAI SDKతో సరిపోలిక కోసం async iterable ప్యాటర్న్‌కు మద్దతు ఇవ్వాలి.

---

## పర్యావరణ వివరాలు

| కంపోనెంట్ | వెర్షన్ |
|-----------|---------|
| OS | Windows 11 ARM64 |
| హార్డ్‌వేర్ | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |