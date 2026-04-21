# المشكلات المعروفة — ورشة Foundry Local

المشكلات التي تم مواجهتها أثناء بناء واختبار هذه الورشة على جهاز **Snapdragon X Elite (ARM64)** يعمل بنظام Windows، مع Foundry Local SDK v0.9.0، CLI v0.8.117، و .NET SDK 10.0.

> **آخر تحقق:** 2026-03-11

---

## 1. وحدة المعالجة المركزية Snapdragon X Elite غير معروفة من ONNX Runtime

**الحالة:** مفتوح  
**شدة:** تحذير (غير معيق)  
**المكون:** ONNX Runtime / cpuinfo  
**كيفية التكرار:** عند كل بدء خدمة Foundry Local على جهاز Snapdragon X Elite  

في كل مرة يتم فيها بدء خدمة Foundry Local، يتم إصدار تحذيرين:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**التأثير:** التحذيرات تجميلية — inference تعمل بشكل صحيح. ومع ذلك، تظهر في كل تشغيل ويمكن أن تربك المشاركين في الورشة. مكتبة ONNX Runtime cpuinfo بحاجة إلى تحديث للتعرف على أنوية وحدة المعالجة المركزية Qualcomm Oryon.

**المتوقع:** يجب أن يتم التعرف على Snapdragon X Elite كوحدة CPU ARM64 مدعومة دون إصدار رسائل بخطأ.

---

## 2. استثناء SingleAgent NullReferenceException في التشغيل الأول

**الحالة:** مفتوح (متقطع)  
**الشدة:** حرجة (تعطل)  
**المكون:** Foundry Local C# SDK + Microsoft Agent Framework  
**كيفية التكرار:** تشغيل `dotnet run agent` — يتعطل فورًا بعد تحميل النموذج  

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**السياق:** السطر 37 يستدعي `model.IsCachedAsync(default)`. حدث التعطل في التشغيل الأول للوكيل بعد `foundry service stop` جديد. النجاحات حدثت في التشغيلات اللاحقة باستخدام نفس الكود.

**التأثير:** متقطع — يشير إلى حالة تسابق في تهيئة الخدمة أو استعلام الكتالوج في SDK. قد يعود الاتصال `GetModelAsync()` قبل اكتمال جاهزية الخدمة.

**المتوقع:** يجب أن يحجب `GetModelAsync()` حتى تكون الخدمة جاهزة أو يرجع رسالة خطأ واضحة إذا لم تنتهِ الخدمة من التهيئة.

---

## 3. SDK لـ C# يتطلب RuntimeIdentifier صريح

**الحالة:** مفتوح — متابع في [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**الشدة:** نقص في التوثيق  
**المكون:** حزمة NuGet `Microsoft.AI.Foundry.Local`  
**كيفية التكرار:** إنشاء مشروع .NET 8+ بدون `<RuntimeIdentifier>` في `.csproj`  

يفشل البناء برسالة:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**السبب الجذري:** المطلوبة لـ RID متوقعة — حيث تحتوي الـ SDK على ثنائيات أصلية (P/Invoke في `Microsoft.AI.Foundry.Local.Core` و ONNX Runtime)، لذا يحتاج .NET لمعرفة أي مكتبة تخص النظام الأساسي لتحميلها.

هذا موثق في MS Learn ([كيفية استخدام تكامل الدردشة الأصلية](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp))، حيث تعليمات التشغيل تظهر:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
لكن يجب على المستخدمين تذكر العلم `-r` في كل مرة، وهذا سهل نسيانه.

**الحل المؤقت:** أضف كشفًا تلقائيًا احتياطيًا إلى `.csproj` الخاص بك ليعمل `dotnet run` بدون أي أعلام:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` هو خاصية MSBuild مضمّنة تحل تلقائيًا إلى RID لجهاز المضيف. مشاريع الاختبار الخاصة بالـ SDK تستخدم هذا النمط. لا تزال أعلام `-r` الصريحة محترمة عند تقديمها.

> **ملاحظة:** يتضمن `.csproj` الخاص بالورشة هذا الاحتياطي ليعمل `dotnet run` مباشرة على أي منصة.

**المتوقع:** يجب أن يتضمن قالب `.csproj` في وثائق MS Learn هذا النمط الاكتشافي التلقائي حتى لا يحتاج المستخدمون لتذكر العلم `-r`.

---

## 4. JavaScript Whisper — تفريغ الصوت يعيد مخرجات فارغة/ثنائية

**الحالة:** مفتوح (تراجع — أسوأ مما كان عند التقرير الأول)  
**الشدة:** رئيسي  
**المكون:** تنفيذ JavaScript لـ Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**كيفية التكرار:** تشغيل `node foundry-local-whisper.mjs` — تُعيد كل ملفات الصوت إما مخرجات فارغة أو ثنائية بدلًا من نص التفريغ  

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
في الأصل، فقط الملف الصوتي الخامس كان يعيد فارغًا؛ لكن من v0.9.x، تعيد كل الملفات الخمسة بايتًا واحدًا (`\ufffd`) بدلًا من نص التفريغ. تنفيذ Python لـ Whisper باستخدام OpenAI SDK يفرغ نفس الملفات بشكل صحيح.

**المتوقع:** يجب أن تعيد `createAudioClient()` تفريغ نصي مطابق لتنفيذات Python/C#.

---

## 5. SDK لـ C# يشحن فقط net8.0 — لا هدف رسمي لـ .NET 9 أو .NET 10

**الحالة:** مفتوح  
**الشدة:** نقص في التوثيق  
**المكون:** حزمة NuGet `Microsoft.AI.Foundry.Local` v0.9.0  
**أمر التثبيت:** `dotnet add package Microsoft.AI.Foundry.Local`  

حزمة NuGet تشحن فقط إطار بناء هدف واحد:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
لا يوجد إطار هدف `net9.0` أو `net10.0`. بالمقابل، الحزمة المرافقة `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) توفر `net8.0`، `net9.0`، `net10.0`، `net472`، و `netstandard2.0`.

### اختبارات التوافق

| إطار الهدف | البناء | التشغيل | ملاحظات |  
|------------|---------|-----------|----------|  
| net8.0 | ✅ | ✅ | مدعوم رسميًا |  
| net9.0 | ✅ | ✅ | يبنى باستخدام التوافق الأمامي — مستخدم في عينات الورشة |  
| net10.0 | ✅ | ✅ | يبنى ويشغل باستخدام التوافق الأمامي مع وقت تشغيل .NET 10.0.3 |  

تُحمّل التجميعية net8.0 على أوقات التشغيل الأحدث من خلال آلية التوافق الأمامي لـ .NET، لذا ينجح البناء. لكن هذا غير موثق ولا تختبره فرق SDK.

### لماذا تستهدف العينات net9.0

1. **.NET 9 هو الإصدار المستقر الأحدث** — معظم المشاركين لديهم هذا مثبت  
2. **التوافق الأمامي يعمل** — التجميعية net8.0 في حزمة NuGet تعمل على وقت تشغيل .NET 9 بدون مشاكل  
3. **.NET 10 (المعاينة/RC)** جديد جدًا للاستهداف في ورشة يجب أن تعمل للجميع  

**المتوقع:** يجب أن تفكر إصدارات SDK المستقبلية في إضافة إطارات هدف `net9.0` و `net10.0` إلى جانب `net8.0` لمطابقة نمط `Microsoft.Agents.AI.OpenAI` وتوفير دعم موثق لأوقات التشغيل الأحدث.

---

## 6. تدفق الدردشة في JavaScript ChatClient يستخدم رد الاتصال وليس المتكررات غير المتزامنة

**الحالة:** مفتوح  
**الشدة:** نقص في التوثيق  
**المكون:** JavaScript `foundry-local-sdk` v0.9.x — `ChatClient.completeStreamingChat()`  

يوفر `ChatClient` الناتج من `model.createChatClient()` طريقة `completeStreamingChat()`، لكنها تستخدم **نمط رد الاتصال** بدلًا من إرجاع متكرر غير متزامن:

```javascript
// ❌ هذا لا يعمل — يرمي "التدفق ليس قابلًا للتكرار بشكل غير متزامن"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ النمط الصحيح — مرر دالة رد النداء
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**التأثير:** للمطورين المألوفين على نمط التكرار غير المتزامن الخاص بـ OpenAI SDK (`for await`) ستظهر لهم أخطاء مربكة. يجب أن تكون ردود الاتصال دالة صحيحة وإلا يعيد SDK الخطأ "Callback must be a valid function."

**المتوقع:** توثيق نمط رد الاتصال في مرجع SDK. أو دعم نمط المتكرر غير المتزامن للتماشي مع OpenAI SDK.

---

## تفاصيل البيئة

| المكون | الإصدار |  
|--------|----------|  
| نظام التشغيل | Windows 11 ARM64 |  
| الأجهزة | Snapdragon X Elite (X1E78100) |  
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
**تنويه**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة الآلية [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى للحفاظ على الدقة، يُرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو عدم دقة. يجب اعتبار المستند الأصلي بلغته الأصلية المصدر المعتمد. للمعلومات الحرجة، يُنصح بالاستعانة بالترجمة البشرية المهنية. نحن غير مسؤولين عن أي سوء فهم أو تفسير ناتج عن استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->