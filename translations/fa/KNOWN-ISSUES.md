# مشکلات شناخته‌شده — کارگاه محلی فاندری

مسائل مواجه‌شده هنگام ساخت و تست این کارگاه روی دستگاه **Snapdragon X Elite (ARM64)** که ویندوز اجرا می‌کند، با Foundry Local SDK نسخه ۰.۹.۰، CLI نسخه ۰.۸.۱۱۷ و .NET SDK نسخه ۱۰.۰.

> **آخرین اعتبارسنجی:** ۲۰۲۶-۰۳-۱۱

---

## ۱. CPU Snapdragon X Elite توسط ONNX Runtime شناسایی نمی‌شود

**وضعیت:** باز  
**شدت:** هشدار (مسدودکننده نیست)  
**مولفه:** ONNX Runtime / cpuinfo  
**روش تولید:** هر بار شروع سرویس Foundry Local روی سخت‌افزار Snapdragon X Elite

هر بار سرویس Foundry Local آغاز می‌شود، دو هشدار صادر می‌گردد:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**تأثیر:** این هشدارها صرفاً ظاهری هستند — استنتاج به درستی کار می‌کند. با این حال، آنها در هر اجرا ظاهر شده و ممکن است شرکت‌کنندگان کارگاه را دچار سردرگمی کنند. کتابخانه cpuinfo در ONNX Runtime نیازمند به‌روزرسانی برای شناسایی هسته‌های CPU Qualcomm Oryon است.

**انتظار:** Snapdragon X Elite باید به عنوان یک CPU ARM64 پشتیبانی شده بدون ارسال پیام‌های خطا در سطح خطا شناسایی شود.

---

## ۲. NullReferenceException در SingleAgent در اجرای اول

**وضعیت:** باز (ناگهانی)  
**شدت:** بحرانی (کرش)  
**مولفه:** Foundry Local C# SDK + Microsoft Agent Framework  
**روش تولید:** اجرای `dotnet run agent` — بلافاصله پس از بارگذاری مدل کرش می‌کند

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**زمینه:** خط ۳۷ فراخوانی `model.IsCachedAsync(default)` را انجام می‌دهد. این کرش در اولین بار اجرای ایجنت پس از یک `foundry service stop` تازه رخ داده است. اجراهای بعدی با همان کد موفق بوده‌اند.

**تأثیر:** ناپایدار — نشان‌دهنده وجود شرایط رقابتی در راه‌اندازی سرویس یا پرس‌وجوی کاتالوگ SDK است. فراخوانی `GetModelAsync()` ممکن است قبل از آماده شدن کامل سرویس بازگردد.

**انتظار:** `GetModelAsync()` باید یا تا آماده شدن سرویس مسدود بماند یا اگر سرویس هنوز راه‌اندازی نشده است، پیام خطای روشنی برگرداند.

---

## ۳. SDK سی‌شارپ نیازمند تعیین صریح RuntimeIdentifier است

**وضعیت:** باز — در [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) پیگیری می‌شود  
**شدت:** خلأ اسنادی  
**مولفه:** بسته NuGet `Microsoft.AI.Foundry.Local`  
**روش تولید:** ایجاد پروژه .NET 8+ بدون `<RuntimeIdentifier>` در فایل `.csproj`

ساخت با خطا مواجه می‌شود:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**علت اصلی:** نیاز به RID انتظار می‌رود — زیرا SDK فایل‌های باینری نیتیو (فراخوانی P/Invoke به `Microsoft.AI.Foundry.Local.Core` و ONNX Runtime) را شامل می‌شود، بنابراین .NET نیاز دارد بداند کدام کتابخانه اختصاصی پلتفرم را بارگذاری کند.

این موضوع در مستندات MS Learn ([چگونه از چت کامل نیتیو استفاده کنیم](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)) آمده است، که دستور اجرای آن شامل:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
با این حال، کاربران باید هر بار گزینه `-r` را به خاطر بسپارند که آسان است فراموش شود.

**راه‌حل موقت:** افزودن تشخیص خودکار به `.csproj` که `dotnet run` بدون هیچ گزینه‌ای کار کند:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` یک ویژگی داخلی MSBuild است که به صورت خودکار RID میزبان را حل می‌کند. پروژه‌های تست خود SDK نیز از این الگو استفاده می‌کنند. گزینه‌های صریح `-r` همچنان زمانی که ارائه شوند، رعایت می‌گردند.

> **توجه:** فایل `.csproj` کارگاه این تشخیص خودکار را شامل می‌شود تا `dotnet run` به طور پیش‌فرض روی هر پلتفرمی کار کند.

**انتظار:** قالب `.csproj` در مستندات MS Learn باید این الگوی تشخیص خودکار را داشته باشد تا کاربران مجبور نباشند گزینه `-r` را به خاطر بسپارند.

---

## ۴. Whisper جاوااسکریپت — رونوشت صوتی خروجی خالی یا باینری برمی‌گرداند

**وضعیت:** باز (بازگشتی — نسبت به گزارش اولیه بدتر شده)  
**شدت:** عمده  
**مولفه:** پیاده‌سازی JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**روش تولید:** اجرای `node foundry-local-whisper.mjs` — همه فایل‌های صوتی خروجی خالی یا باینری به جای متن رونویسی شده برمی‌گردانند

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
در ابتدا فقط فایل صوتی پنجم خروجی خالی داشت؛ از نسخه ۰.۹.x، همه ۵ فایل یک بایت (`\ufffd`) خروجی می‌دهند به جای متن رونویسی شده. پیاده‌سازی Python Whisper که از OpenAI SDK استفاده می‌کند این فایل‌ها را به درستی رونویسی می‌کند.

**انتظار:** `createAudioClient()` باید رونویسی متنی مشابه پیاده‌سازی‌های Python و C# برگرداند.

---

## ۵. SDK سی‌شارپ فقط net8.0 را شامل می‌شود — هدف رسمی برای .NET 9 یا .NET 10 ندارد

**وضعیت:** باز  
**شدت:** خلأ اسنادی  
**مولفه:** بسته NuGet `Microsoft.AI.Foundry.Local` نسخه ۰.۹.۰  
**دستور نصب:** `dotnet add package Microsoft.AI.Foundry.Local`

بسته NuGet فقط یک چارچوب هدف واحد دارد:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
هیچ TFM‌ برای `net9.0` یا `net10.0` شامل نشده است. در مقابل، بسته همراه `Microsoft.Agents.AI.OpenAI` (نسخه ۱.۰.۰-rc3) `net8.0`، `net9.0`، `net10.0`، `net472` و `netstandard2.0` را شامل می‌شود.

### تست سازگاری

| چارچوب هدف | ساخت | اجرا | توضیحات |
|------------|-------|------|---------|
| net8.0     | ✅    | ✅   | به طور رسمی پشتیبانی شده |
| net9.0     | ✅    | ✅   | با سازگاری رو به جلو ساخته شده — در نمونه‌های کارگاه استفاده شده |
| net10.0    | ✅    | ✅   | با سازگاری رو به جلو و اجرای .NET 10.0.3 ساخته و اجرا می‌شود |

کامپایل net8.0 روی ران‌تایم‌های جدیدتر از طریق مکانیزم سازگاری رو به جلو .NET بارگذاری می‌شود، بنابراین ساخت موفق است. اما این موضوع توسط تیم SDK مستند نشده و تست نشده است.

### چرا نمونه‌ها net9.0 هدف قرار می‌دهند

1. **.NET 9 آخرین نسخه پایدار است** — اکثر شرکت‌کنندگان کارگاه آن را نصب دارند  
2. **سازگاری رو به جلو کار می‌کند** — اسمبلی net8.0 در بسته NuGet روی ران‌تایم .NET 9 بدون مشکل اجرا می‌شود  
3. **.NET 10 (پیش‌نمایش/RC)** خیلی جدید است که در کارگاهی که برای همه باید کار کند، هدف قرار گیرد

**انتظار:** انتشارهای آینده SDK باید افزودن TFMs مربوط به `net9.0` و `net10.0` را در کنار `net8.0` مد نظر قرار دهند تا با الگوهای استفاده‌شده در `Microsoft.Agents.AI.OpenAI` مطابقت کند و پشتیبانی تاییدشده‌ای برای ران‌تایم‌های جدیدتر فراهم نماید.

---

## ۶. Javascript ChatClient استریم را با Callback انجام می‌دهد، نه Iterator غیرهمزمان

**وضعیت:** باز  
**شدت:** خلأ اسنادی  
**مولفه:** `foundry-local-sdk` JavaScript نسخه ۰.۹.x — `ChatClient.completeStreamingChat()`

کلاسی که `model.createChatClient()` برمی‌گرداند متدی به نام `completeStreamingChat()` دارد، اما از الگوی **Callback** استفاده می‌کند نه اینکه یک iterable غیرهمزمان برگرداند:

```javascript
// ❌ این کار نمی‌کند — خطا می‌دهد "جریان قابل تکرار به صورت ناهمزمان نیست"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ الگوی صحیح — ارسال یک تابع فراخوان بازگشتی
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**تأثیر:** توسعه‌دهندگان آشنا به الگوی تکرار async در OpenAI SDK (`for await`) با خطاهای گیج‌کننده مواجه می‌شوند. باید callback یک تابع معتبر باشد در غیر اینصورت SDK خطای "Callback must be a valid function." می‌دهد.

**انتظار:** الگوی callback در مستندات SDK توضیح داده شود. به عنوان جایگزین، الگوی iterable غیرهمزمان نیز پشتیبانی شود تا با OpenAI SDK همخوانی داشته باشد.

---

## جزئیات محیط

| مولفه                | نسخه          |
|----------------------|--------------|
| سیستم‌عامل           | Windows 11 ARM64 |
| سخت‌افزار            | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI     | 0.8.117      |
| Foundry Local SDK (C#)| 0.9.0        |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK        | 2.9.0        |
| .NET SDK             | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x     |
| foundry-local-sdk (JS) | 0.9.x        |
| Node.js              | 18+          |
| ONNX Runtime         | 1.18+        |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**سلب مسئولیت**:  
این سند با استفاده از سرویس ترجمه هوش مصنوعی [Co-op Translator](https://github.com/Azure/co-op-translator) ترجمه شده است. در حالی که ما تلاش می‌کنیم دقت را حفظ کنیم، لطفاً توجه داشته باشید که ترجمه‌های خودکار ممکن است شامل اشتباهات یا نواقص باشند. سند اصلی به زبان اصلی خود باید به عنوان منبع معتبر در نظر گرفته شود. برای اطلاعات حیاتی، ترجمه حرفه‌ای انسانی توصیه می‌شود. ما مسئول هیچ گونه سوءتفاهم یا تفسیر نادرستی که از استفاده این ترجمه ناشی شود، نیستیم.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->