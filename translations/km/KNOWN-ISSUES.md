# បញ្ហាដែលបានស្គាល់ — សិក្ខាសាលាផ្ទាល់ Foundry

បញ្ហាដែលបានប្រទះពេលកំពុងសាងសង់និងសាកល្បងសិក្ខាសាលានេះលើឧបករណ៍ **Snapdragon X Elite (ARM64)** ដែលរត់ Windows ជាមួយ Foundry Local SDK v0.9.0, CLI v0.8.117, និង .NET SDK 10.0។

> **បានផ្ទៀងផ្ទាត់ចុងក្រោយ៖** 2026-03-11

---

## 1. មិនមានការទទួលស្គាល់ Snapdragon X Elite CPU ដោយ ONNX Runtime

**ស្ថានភាព៖** បើក
**ភាពធ្ងន់ធ្ងរ៖** ព្រមាន (មិនបាច់ឈប់)
**ផ្នែក៖** ONNX Runtime / cpuinfo
**វិធីធ្វើម្ដងទៀត៖** បើកសេវា Foundry Local រាល់ពេលលើឧបករណ៍ Snapdragon X Elite

រាល់ពេលសេវា Foundry Local ចាប់ផ្តើម មានសញ្ញាព្រមានពីរចេញ:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**ផលប៉ះពាល់៖** សញ្ញាព្រមានគឺមានតែរូបរាងប៉ុណ្ណោះ — ការធ្វើInferenceត្រូវបានធ្វើបានត្រឹមត្រូវ។ ប៉ុន្តែវាពេលដំណើរការរាល់ដំណើរការនិងអាចបង្កការប្រកបប្រព្រឹត្តឲ្យអ្នកចូលរួមសិក្ខាសាលាមួយចំនួនស្រងាត់ចិត្ត។ បណ្ណាល័យ ONNX Runtime cpuinfo ត្រូវការធ្វើបច្ចុប្បន្នភាពដើម្បីទទួលស្គាល់ Qualcomm Oryon CPU cores។

**គោលបំណង៖** Snapdragon X Elite គួរត្រូវបានទទួលស្គាល់ជាឧបករណ៍ ARM64 CPU ដែលគាំទ្រដោយមិនបង្ហាញសារកំហុសកម្រិតកំហុស។

---

## 2. SingleAgent NullReferenceException នៅលើRunដំបូង

**ស្ថានភាព៖** បើក (ច្រើនពេលមួយ)
**ភាពធ្ងន់ធ្ងរ៖** សំខាន់ (បំផ្លាញ)
**ផ្នែក៖** Foundry Local C# SDK + Microsoft Agent Framework
**វិធីធ្វើម្ដងទៀត៖** រត់ `dotnet run agent` — បង្ករឧប្បទានភ្លាមបន្ទាប់ពីបន្ទុកម៉ូដែល

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**បរិបទ៖** បន្ទាត់ 37 ហៅ `model.IsCachedAsync(default)`។ ការបំផ្លាញបានកើតឡើងនៅលើការរត់ដំបូងនៃ agent បន្ទាប់ពីបាន `foundry service stop` ថ្មី។ ការរត់បន្ទាប់មានជោគជ័យជាមួយកូដដដែល។

**ផលប៉ះពាល់៖** ច្រើនពេល — បញ្ជាក់ពីការប្រកួតប្រជែងនៅក្នុងការចាប់ផ្តើមសេវាឬសំណួរកាតាឡុកក្នុង SDK។ ការហៅ `GetModelAsync()` អាចត្រឡប់មកមុនពេលសេវាត្រូវបានរៀបចំរួច។

**គោលបំណង៖** `GetModelAsync()` គួរត្រូវប៊្លុករហូតដល់សេវា​រៀបចំរួចឬត្រឡប់សារកំហុសច្បាស់លាស់ប្រសិនបើសេវាអត់បានបញ្ចប់ការចាប់ផ្តើម។

---

## 3. C# SDK ត្រូវការ RuntimeIdentifier បញ្ជាក់ច្បាស់

**ស្ថានភាព៖** បើក — តាមដាននៅ [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)
**ភាពធ្ងន់ធ្ងរ៖** ចន្លោះឯកសារ
**ផ្នែក៖** កញ្ចប់ NuGet `Microsoft.AI.Foundry.Local`
**វិធីធ្វើម្ដងទៀត៖** បង្កើតគម្រោង .NET 8+ ដោយគ្មាន `<RuntimeIdentifier>` ក្នុង `.csproj`

ការសាងសង់បរាជ័យជាមួយ៖

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**មូលហេតុដើម៖** តម្រូវការតម្លៃ RID គឺមានន័យថា — SDK ផ្ដល់បណ្ណាល័យគ្រឿងធម្មជាតិក្នុងស្រុក (P/Invoke ទៅ `Microsoft.AI.Foundry.Local.Core` និង ONNX Runtime) ដូច្នេះ .NET ព្រមានពីបណ្ណាល័យជាពិសេសសម្រាប់វេទិកា។

មានឯកសារពាក់ព័ន្ធនៅ MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)) ដែលបង្ហាញក្នុងវិធីហៅ:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

ប៉ុន្តែអ្នកប្រើត្រូវចាំថាវា `-r` ក្នុងរាល់ដំណើរការ ដែលងាយជ្រេចខ្លួន។

**វិធីដោះស្រាយ៖** បន្ថែម fallback អូតូរក្រុមជាមួយ `.csproj` ដូច្នេះ `dotnet run` ធ្វើការ Hebwithout any flags:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` គឺជាគុណលក្ខណៈ MSBuild ដែលកំណត់ជាមួយ RID គ្រួសជាម៉េស៊ីនផ្ទាល់ខ្លួនយ៉ាងស្វ័យប្រវត្តិ។ គម្រោងសាកល្បងរបស់ SDK មានប្រើរបៀបនេះរួចហើយ។ សញ្ញា `-r` ដែលបានផ្ដល់អាចប្រើបានដូចធម្មតា។

> **ចំណាំ៖** `.csproj` នៃសិក្ខាសាលាជាដើមមាន fallback នេះហើយ ដូច្នេះ `dotnet run` ធ្វើការលើគ្រប់វេទិកា។

**គោលបំណង៖** គំរូ `.csproj` នៅក្នុងឯកសារ MS Learn គួរតែលូតលាស់នូវរបៀបស្វ័យរកនេះ ដើម្បីប្រើប្រាស់ដោយមិនបាច់ចាំ `-r` ។

---

## 4. JavaScript Whisper — ការបម្លែងសំលេងទៅអត្ថបទបង្ហាញជាឆ្នូត/២មិនមានលទ្ធផល

**ស្ថានភាព៖** បើក (ត្រឡប់ក្រោយ — ធ្វើអោយកាន់តែអាក្រក់ចាប់តាំងពីរបាយការណ៍ដំបូង)
**ភាពធ្ងន់ធ្ងរ៖** សំខាន់
**ផ្នែក៖** ការអនុវត្ត JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**វិធីធ្វើម្ដងទៀត៖** រត់ `node foundry-local-whisper.mjs` — អត្ថបទសំលេងទាំងអស់ត្រឡប់មកជារលកទទេឬឯកសារចម្លាក់ យ៉ាងម្ដេចក៏ដោយមិនមែនជាអត្ថបទបកប្រែ

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

ដើមតែឯកសារសំលេងទី 5 ត្រឡប់ទទេ; ចាប់ពី v0.9.x ឯកសារទាំង 5 ទៅតែបង្ហាញបាយតែមួយ (`\ufffd`) ជំនួសអត្ថបទបកប្រែ។ ការអនុវត្ត Python Whisper ដែលប្រើ OpenAI SDK បកប្រែឯកសារទាំងនោះបានត្រឹមត្រូវ។

**គោលបំណង៖** `createAudioClient()` គួរត្រឡប់អត្ថបទបកប្រែដែលផ្គូផ្គងនឹងការអនុវត្ត Python/C# ។

---

## 5. C# SDK ផ្ដល់តែ net8.0 — មិនមានគោលដៅ .NET 9 ឬ .NET 10 ទេ

**ស្ថានភាព៖** បើក
**ភាពធ្ងន់ធ្ងរ៖** ចន្លោះឯកសារ
**ផ្នែក៖** កញ្ចប់ NuGet `Microsoft.AI.Foundry.Local` v0.9.0
**ពាក្យបញ្ជាទាញដំឡើង៖** `dotnet add package Microsoft.AI.Foundry.Local`

កញ្ចប់ NuGet ផ្ដល់គោលដៅ framework តែមួយ:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

គ្មាន TFM `net9.0` ឬ `net10.0` រួមបញ្ចូលទេ។ ផ្ទុយទៅវិញ កញ្ចប់ជាគូសហការ `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) ផ្ដល់ `net8.0`, `net9.0`, `net10.0`, `net472`, និង `netstandard2.0`។

### ការសាកល្បងភាពប្រសើរភាព

| គោលដៅ Framework | សាងសង់ | ដំណើរការ | កំណត់សម្គាល់ |
|-----------------|---------|-----------|------------|
| net8.0 | ✅ | ✅ | គាំទ្រយ៉ាងផ្លូវការជា |
| net9.0 | ✅ | ✅ | សាងសង់តាមការភ្ជាប់មុខ — ប្រើក្នុងគំរូសិក្ខាសាលា |
| net10.0 | ✅ | ✅ | សាងសង់ហើយដំណើរការតាម forward-compat ជាមួយ runtime .NET 10.0.3 |

សមាសធាតុ net8.0 ផ្ទុកលើ runtime ថ្មីៗតាមរបៀប forward-compatibility របស់ .NET ដូច្នេះការសាងសង់ជោគជ័យ។ ទោះជាយ៉ាងណា វាក៏មិនមានឯកសារបង្ហាញនិងមិនបានសាកល្បងដោយក្រុម SDK។

### មូលហេតុដែលគំរូមានគោលដៅ net9.0

1. **.NET 9 គឺជាកំណែចុងក្រោយដែលមានស្ថេរភាព** — អ្នកចូលរួមសិក្ខាសាលាពីភាគច្រើនមានវាជាដំឡើងរួច
2. **Compatibility ម៉ាស្សាសាកល** — សមាសធាតុ net8.0 នៅក្នុង NuGet កញ្ចប់បើកដំណើរការបានលើ runtime .NET 9 ដោយគ្មានបញ្ហា
3. **.NET 10 (ពិនិត្យមើល/RC)** ជាថ្មីពេកសម្រាប់ប្រើក្នុងសិក្ខាសាលាដែលគួរតែដំណើរការដល់មនុស្សទាំងអស់

**គោលបំណង៖** កំណែ SDK អនាគតគួរតែពិចារណាបន្ថែម TFMs `net9.0` និង `net10.0` ជាមួយ `net8.0` ដើម្បីផ្គូផ្គងនឹងគំរូដែល Microsoft.Agents.AI.OpenAI ប្រើ និងផ្តល់ការគាំទ្រត្រូវបានផ្ទៀងផ្ទាត់សម្រាប់ runtime ថ្មីៗ។

---

## 6. JavaScript ChatClient Streaming ប្រើ Callbacks មិនមែន Async Iterators

**ស្ថានភាព៖** បើក
**ភាពធ្ងន់ធ្ងរ៖** ចន្លោះឯកសារ
**ផ្នែក៖** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` ដែលបានត្រឡប់ពី `model.createChatClient()` ផ្ដល់មេធត `completeStreamingChat()` ប៉ុន្តែវាប្រើរបៀប **callback** មិនមែនជារបៀប async iterable:

```javascript
// ❌ វានៅ​មិន​ដំណើរ​ការ — បង្ហាយ "stream មិនមែន iterable មិនស៊ីញក្រោនទេ"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ រចនាប័ទ្ម​ជាក់លាក់ — បញ្ជូន callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**ផលប៉ះពាល់៖** អ្នកអwickdevelopដែលស្គាល់របៀប async iteration របស់ OpenAI SDK (`for await`) នឹងជួបកំហុសចំលងចិត្ត។ Callback ត្រូវតែជាអនុគមន៍ត្រឹមត្រូវ បើមិនដូច្នោះ SDK នឹងបង្ហាញ "Callback must be a valid function."

**គោលបំណង៖** ឯកសារពិពណ៌នារបៀប callback នៅក្នុងឯកសារយោង SDK។ ជាជម្រើសជំនួសគួរគាំទ្ររបៀប async iterable ដើម្បីស្របទៅនឹង OpenAI SDK។

---

## ព័ត៌មានលម្អិតបរិស្ថាន

| ផ្នែក | កំណែ |
|-----------|---------|
| ប្រព័ន្ធប្រតិបត្តិការ | Windows 11 ARM64 |
| ហេារទ័រ | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |