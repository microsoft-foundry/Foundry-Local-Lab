# معروف مسائل — Foundry لوکل ورکشاپ

اس ورکشاپ کو **Snapdragon X Elite (ARM64)** ڈیوائس پر Windows چلانے کے دوران Foundry Local SDK v0.9.0، CLI v0.8.117، اور .NET SDK 10.0 کے ساتھ بنانے اور ٹیسٹ کرنے کے دوران پیش آنے والے مسائل۔

> **آخری تصدیق:** 2026-03-11

---

## 1. Snapdragon X Elite CPU کو ONNX Runtime نہیں پہچانتا

**حیثیت:** کھلا
**شدت:** وارننگ (غیر معطّل)
**جزو:** ONNX Runtime / cpuinfo
**دہرانا:** Snapdragon X Elite ہارڈویئر پر ہر Foundry Local سروس شروع کرنے پر

ہر بار جب Foundry Local سروس شروع ہوتی ہے، دو وارننگز آتی ہیں:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**اثر:** وارننگز صرف ظاہری ہیں — inference بالکل درست کام کرتا ہے۔ تاہم، یہ ہر بار چلانے پر نظر آتی ہیں اور ورکشاپ کے شرکاء کو الجھا سکتی ہیں۔ ONNX Runtime cpuinfo لائبریری کو Qualcomm Oryon CPU cores کو پہچاننے کے لیے اپ ڈیٹ کرنے کی ضرورت ہے۔

**متوقع:** Snapdragon X Elite کو ایک معاون ARM64 CPU کے طور پر پہچانا جانا چاہیے بغیر ایرر لیول پیغامات جاری کیے۔

---

## 2. SingleAgent ابتدائی چلانے پر NullReferenceException

**حیثیت:** کھلا (دھڑا دھڑا ہوتا ہے)
**شدت:** اہم (کراس)
**جزو:** Foundry Local C# SDK + Microsoft Agent Framework
**دہرانا:** `dotnet run agent` چلائیں — ماڈل لوڈ کے فوراً بعد کریش ہوتا ہے

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**سیاق و سباق:** لائن 37 پر `model.IsCachedAsync(default)` کال ہوتی ہے۔ agent کے پہلے چلانے پر یہ کریش ہوا جب `foundry service stop` کے بعد نیا سیشن شروع ہوا۔ بعد کے چلانے کامیاب رہے۔

**اثر:** دھڑا دھڑا ہوتا ہے — یہ SDK کی سروس آغاز یا کیٹلاگ پوچھ گچھ میں ریس کنڈیشن کی نشاندہی کرتا ہے۔ `GetModelAsync()` کال سروس کے مکمل تیار ہونے سے پہلے واپس آ سکتی ہے۔

**متوقع:** `GetModelAsync()` کو چاہیے کہ یا تو سروس کے تیار ہونے تک بلاک کرے یا واضح ایرر پیغام دے اگر سروس مکمل طور پر شروع نہیں ہوئی ہے۔

---

## 3. C# SDK کو واضح RuntimeIdentifier کی ضرورت ہے

**حیثیت:** کھلا — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) میں ٹریک کیا جا رہا ہے
**شدت:** دستاویزات کا خلا
**جزو:** `Microsoft.AI.Foundry.Local` NuGet پیکیج
**دہرانا:** `.csproj` میں `<RuntimeIdentifier>` کے بغیر .NET 8+ پروجیکٹ بنائیں

بلڈ ناکام ہوتا ہے:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**اصل وجہ:** RID کی ضرورت متوقع ہے — SDK میں اصل بائنریز (P/Invoke into `Microsoft.AI.Foundry.Local.Core` اور ONNX Runtime) شامل ہیں، لہٰذا .NET کو معلوم ہونا چاہیے کہ کونسی مخصوص پلیٹ فارم کی لائبریری لوڈ کرنی ہے۔

یہ MS Learn پر دستاویزی ہے ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp))، جہاں رن ہدایات یہ دکھاتی ہیں:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

تاہم، صارفین کو ہر بار `-r` فلیگ یاد رکھنا مشکل ہوتا ہے جو بھول جانا آسان ہے۔

**حل:** اپنے `.csproj` میں ایک خودکار پتہ لگانے والا فیل بیک شامل کریں تاکہ `dotnet run` بغیر کسی فلیگ کے کام کرے:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` ایک بلٹ ان MSBuild پراپرٹی ہے جو میزبان مشین کا RID خود بخود حل کرتی ہے۔ SDK کے اپنے ٹیسٹ پروجیکٹس پہلے ہی اس طریقہ کار کو استعمال کرتے ہیں۔ جب واضح طور پر `-r` فلیگ دیا جائے تو وہ بھی تسلیم کیا جاتا ہے۔

> **نوٹ:** ورکشاپ کا `.csproj` اس فیل بیک کو شامل کرتا ہے تاکہ `dotnet run` کسی بھی پلیٹ فارم پر براہ راست کام کرے۔

**متوقع:** MS Learn کی دستاویز میں `.csproj` ٹیمپلیٹ کو اس خودکار پتہ لگانے والے پیٹرن کے ساتھ شامل کیا جائے تاکہ صارفین کو `-r` فلیگ یاد رکھنے کی ضرورت نہ ہو۔

---

## 4. JavaScript Whisper — آڈیو ٹرانسکرپشن خالی یا بائنری آؤٹ پٹ دیتا ہے

**حیثیت:** کھلا (رجعت — ابتدائی رپورٹ سے بدتر ہوا)
**شدت:** بڑا مسئلہ
**جزو:** JavaScript Whisper نفاذ (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**دہرانا:** `node foundry-local-whisper.mjs` چلائیں — تمام آڈیو فائلیں ٹیکسٹ ٹرانسکرپشن کی بجائے خالی یا بائنری آؤٹ پٹ دیتی ہیں

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

اصل میں صرف پانچویں آڈیو فائل خالی واپس آتی تھی؛ v0.9.x میں، تمام 5 فائلیں ایک بائیٹ (`\ufffd`) دیتی ہیں بجائے ٹرانسکرپٹ کے۔ Python Whisper نفاذ OpenAI SDK استعمال کرتے ہوئے وہی فائلیں درست ٹرانسکرائب کرتا ہے۔

**متوقع:** `createAudioClient()` کو Python/C# نفاذ کے مطابق متن کی ٹرانسکرپشن واپس دینی چاہیے۔

---

## 5. C# SDK صرف net8.0 کے ساتھ آتا ہے — سرکاری .NET 9 یا .NET 10 ہدف نہیں ہے

**حیثیت:** کھلا
**شدت:** دستاویزات کا خلا
**جزو:** `Microsoft.AI.Foundry.Local` NuGet پیکیج v0.9.0
**انسٹال کمانڈ:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet پیکیج میں صرف ایک ہدف فریم ورک شامل ہے:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

کوئی `net9.0` یا `net10.0` TFM شامل نہیں ہے۔ اس کے برعکس، کمپینین پیکیج `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472`، اور `netstandard2.0` فراہم کرتا ہے۔

### مطابقت کی جانچ

| ہدف فریم ورک | بلڈ | چلانا | نوٹس |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | باضابطہ مدد یافتہ |
| net9.0 | ✅ | ✅ | فارورڈ-کمپیٹیبل کے ذریعے بلڈ ہوتا ہے — ورکشاپ کے نمونوں میں استعمال ہوا ہے |
| net10.0 | ✅ | ✅ | .NET 10.0.3 رن ٹائم کے ساتھ فارورڈ-کمپیٹیبل بلڈ اور رن ہوتا ہے |

net8.0 اسمبلی نئے رن ٹائمز پر .NET کی فارورڈ-کمپیٹیبلیٹی میکانزم کے ذریعے لوڈ ہوتی ہے، اس لیے بلڈ کامیاب ہوتا ہے۔ تاہم، یہ دستاویزی نہیں ہے اور SDK ٹیم نے اس کی جانچ نہیں کی۔

### کیوں نمونے net9.0 کو ہدف بناتے ہیں

1. **.NET 9 تازہ ترین مستحکم ریلیز ہے** — زیادہ تر ورکشاپ شرکاء کے پاس موجود ہو گا
2. **فارورڈ کمپیٹیبلیٹی کام کرتی ہے** — NuGet پیکیج کی net8.0 اسمبلی .NET 9 رن ٹائم پر بغیر مسئلے چلتی ہے
3. **.NET 10 (پری ویو/RC)** ورکشاپ کے لیے بہت نیا ہے جسے سب کے لیے کام کرنا چاہیے

**متوقع:** مستقبل کے SDK ریلیزز میں `net9.0` اور `net10.0` TFM کو `net8.0` کے ساتھ شامل کرنے پر غور کیا جائے تاکہ `Microsoft.Agents.AI.OpenAI` کی طرح کا پیٹرن بنایا جا سکے اور نئے رن ٹائمز کے لیے تصدیق شدہ مدد فراہم کی جا سکے۔

---

## 6. JavaScript ChatClient Streaming کال بیکس استعمال کرتا ہے، Async Iterators نہیں

**حیثیت:** کھلا
**شدت:** دستاویزات کا خلا
**جزو:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` جو `model.createChatClient()` سے حاصل ہوتا ہے، ایک `completeStreamingChat()` طریقہ فراہم کرتا ہے، لیکن یہ async iterable واپس کرنے کی بجائے **کال بیک پیٹرن** استعمال کرتا ہے:

```javascript
// ❌ یہ کام نہیں کرتا — "stream async iterable نہیں ہے" کی خرابی دیتا ہے
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ درست انداز — کال بیک پاس کریں
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**اثر:** OpenAI SDK کے async iteration پیٹرن (`for await`) سے واقف ڈیولپرز کو الجھن والے ایررز کا سامنا ہو گا۔ کال بیک کو ایک درست فنکشن ہونا چاہیے ورنہ SDK "Callback must be a valid function." کی خرابی پھینکتا ہے۔

**متوقع:** SDK ریفرنس میں کال بیک پیٹرن کی دستاویزی کی جائے۔ متبادل طور پر، OpenAI SDK کے مطابقت کے لیے async iterable پیٹرن کی حمایت بھی کی جائے۔

---

## ماحولیاتی تفصیلات

| جزو | ورژن |
|-----------|---------|
| OS | Windows 11 ARM64 |
| ہارڈویئر | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |