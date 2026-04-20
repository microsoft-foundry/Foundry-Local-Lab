# পরিচিত সমস্যা — Foundry Local ওয়ার্কশপ

**Snapdragon X Elite (ARM64)** ডিভাইসের উপর Windows চালিয়ে Foundry Local SDK v0.9.0, CLI v0.8.117, এবং .NET SDK 10.0 সহ এই ওয়ার্কশপ তৈরি ও পরীক্ষার সময় পাওয়া সমস্যাসমূহ।

> **সর্বশেষ যাচাই:** ২০২৬-০৩-১১

---

## ১. Snapdragon X Elite CPU ONNX Runtime দ্বারা সনাক্ত হয় না

**অবস্থা:** খোলা  
**গুরুত্ব:** সতর্কতা (বাধা সৃষ্টি করে না)  
**উপাদান:** ONNX Runtime / cpuinfo  
**পুনরুত্পাদন:** Snapdragon X Elite হার্ডওয়্যারে প্রতিটি Foundry Local সার্ভিস শুরু করার সময়

প্রতিবার Foundry Local সার্ভিস শুরু হলে, দুটি সতর্কতা প্রদর্শিত হয়:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**প্রভাব:** সতর্কতাগুলো কেবল বাইরের, ইনফারেন্স সঠিকভাবে কাজ করে। তবে এগুলো প্রতি রানে প্রদর্শিত হওয়ায় ওয়ার্কশপ অংশগ্রহণকারীদের বিভ্রান্ত করতে পারে। Qualcomm Oryon CPU cores সনাক্ত করার জন্য ONNX Runtime cpuinfo লাইব্রেরি আপডেট করতে হবে।

**প্রত্যাশিত:** Snapdragon X Elite কে একটি সমর্থিত ARM64 CPU হিসেবে সনাক্ত করা উচিত যেখান থেকে ত্রুটিমূলক স্তরের বার্তা না প্রদর্শিত হয়।

---

## ২. প্রথম চালনায় SingleAgent NullReferenceException

**অবস্থা:** খোলা (মাঝেমধ্যে ঘটে)  
**গুরুত্ব:** গুরুতর (ক্র্যাশ)  
**উপাদান:** Foundry Local C# SDK + Microsoft Agent Framework  
**পুনরুত্পাদন:** `dotnet run agent` চালানো — মডেল লোডের পর সঙ্গে সঙ্গেই ক্র্যাশ হয়

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**পরিপ্রেক্ষিত:** লাইন ৩৭ এ `model.IsCachedAsync(default)` কল করা হয়। ক্র্যাশটি ঘটে এজেন্টের প্রথম রানে যখন `foundry service stop` এর পর। পরবর্তী একই কোডের রানে সফল হয়েছে।

**প্রভাব:** মাঝে মাঝে ঘটে — SDK-এর সার্ভিস আরম্ভ বা ক্যাটালগ ক্যোয়ারির মধ্যে একটি রেস কন্ডিশনের ইঙ্গিত দেয়। `GetModelAsync()` কল সার্ভিস সম্পূর্ণ প্রস্তুত হওয়ার আগে রিটার্ন করতে পারে।

**প্রত্যাশিত:** `GetModelAsync()` সার্ভিস প্রস্তুত হওয়া পর্যন্ত ব্লক করবে অথবা পরিষ্কার ত্রুটি বার্তা দিবে যদি সার্ভিস এখনও ইনি্শিয়ালাইজ হয়নি।

---

## ৩. C# SDK-তে স্পষ্ট RuntimeIdentifier প্রয়োজন

**অবস্থা:** খোলা — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) এ ট্র্যাক করা হচ্ছে  
**গুরুত্ব:** ডকুমেন্টেশন ফাঁক  
**উপাদান:** `Microsoft.AI.Foundry.Local` NuGet প্যাকেজ  
**পুনরুত্পাদন:** `.csproj`-এ `<RuntimeIdentifier>` ছাড়া .NET 8+ প্রজেক্ট তৈরি করা

নির্মাণ ব্যর্থ হয়:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**মূল কারণ:** RID প্রয়োজনীয়তা প্রত্যাশিত — SDK তৃতীয় পক্ষের নেটিভ বাইনারি (P/Invoke করে `Microsoft.AI.Foundry.Local.Core` এবং ONNX Runtime) বহন করে, তাই .NET-কে জানা প্রয়োজন কোন প্ল্যাটফর্ম-নির্দিষ্ট লাইব্রেরি লোড করতে হবে।

এটি MS Learn এ ডকুমেন্টেড ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), যেখানে রান কমান্ড প্রদর্শিত হয়:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
তবে, ব্যবহারকারীরা `-r` ফ্ল্যাগটি প্রতিবার মনে রাখতে হবে, যা খুবই সহজে ভুলে যেতে পারে।

**সমাধান:** আপনার `.csproj`-এ একটি অটো-ডিটেক্ট ফ্যালব্যাক যোগ করুন যাতে `dotnet run` ফ্ল্যাগ ছাড়াও কাজ করে:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` একটি বিল্ট-ইন MSBuild প্রোপার্টি যা হোস্ট মেশিনের RID অটোম্যাটিক্যালি রেজল্ভ করে। SDK-এর নিজস্ব টেস্ট প্রজেক্টগুলো ইতিমধ্যে এই প্যাটার্ন ব্যবহার করে। স্পষ্ট `-r` ফ্ল্যাগ প্রদান করলে সেগুলো অগ্রাধিকার পায়।

> **বিঃদ্রঃ:** ওয়ার্কশপের `.csproj`-এ এই ফ্যালব্যাক অন্তর্ভুক্ত আছে যাতে `dotnet run` যে কোনও প্ল্যাটফর্মে বক্স থেকে কাজ করে।

**প্রত্যাশিত:** MS Learn ডকুমেন্টেশনের `.csproj` টেম্পলেটে এই অটো-ডিটেক্ট প্যাটার্ন থাকা উচিত যেন ব্যবহারকারীদের প্রতিবার `-r` ফ্ল্যাগ মনে রাখতে না হয়।

---

## ৪. JavaScript Whisper — অডিও লিপিবদ্ধকরণে খালি/বাইনারি আউটপুট ফেরত দেয়

**অবস্থা:** খোলা (পুনরাবৃত্তি — প্রাথমিক রিপোর্টের পর অবস্থা খারাপ হয়েছে)  
**গুরুত্ব:** প্রধান  
**উপাদান:** JavaScript Whisper বাস্তবায়ন (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**পুনরুত্পাদন:** `node foundry-local-whisper.mjs` চালানো — সব অডিও ফাইলের জন্য টেক্সট ট্রান্সক্রিপশনের পরিবর্তে খালি বা বাইনারি আউটপুট আসে

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
প্রথমে শুধুমাত্র পঞ্চম অডিও ফাইল খালি আসত; v0.9.x থেকে ৫টি ফাইলের সবকটির জন্য একটি একক বাইট (`\ufffd`) আসে, টেক্সট ট্রান্সক্রিপশনের পরিবর্তে। Python Whisper বাস্তবায়নের OpenAI SDK একই ফাইল সঠিকভাবে লিপিবদ্ধ করে।

**প্রত্যাশিত:** `createAudioClient()` এর টেক্সট ট্রান্সক্রিপশন Python/C# বাস্তবায়নের মতো আসা উচিত।

---

## ৫. C# SDK শুধুমাত্র net8.0 সরবরাহ করে — অফিসিয়ালি .NET 9 বা .NET 10 টার্গেট নেই

**অবস্থা:** খোলা  
**গুরুত্ব:** ডকুমেন্টেশন ফাঁক  
**উপাদান:** `Microsoft.AI.Foundry.Local` NuGet প্যাকেজ v0.9.0  
**ইনস্টল কমান্ড:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet প্যাকেজ শুধুমাত্র একটি টার্গেট ফ্রেমওয়ার্ক সরবরাহ করে:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
`net9.0` বা `net10.0` TFM অন্তর্ভুক্ত নয়। তুলনায়, সঙ্গতিপূর্ণ প্যাকেজ `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472`, এবং `netstandard2.0` সরবরাহ করে।

### সামঞ্জস্য পরীক্ষণ

| টার্গেট ফ্রেমওয়ার্ক | নির্মাণ | চালানো | মন্তব্য |
|---------------------|---------|--------|----------|
| net8.0 | ✅ | ✅ | অফিসিয়ালি সমর্থিত |
| net9.0 | ✅ | ✅ | ফরোয়ার্ড-কম্প্যাট ব্যবহার করে বিল্ড — ওয়ার্কশপ স্যাম্পলে ব্যবহৃত |
| net10.0 | ✅ | ✅ | .NET 10.0.3 রানটাইম সহ ফরোয়ার্ড-কম্প্যাটিজন মাধ্যমে বিল্ড ও রান |

net8.0 অ্যাসেম্বলি নতুন রানটাইমগুলিতে .NET-এর ফরোয়ার্ড-কম্প্যাটিজন প্রক্রিয়ার মাধ্যমে লোড হয়, তাই নির্মাণ সফল হয়। তবে SDK টিম দ্বারা এটা ডকুমেন্টেড বা পরীক্ষা করা হয়নি।

### কেন স্যাম্পলগুলো net9.0 টার্গেট করে

1. **.NET 9 সর্বশেষ স্থিতিশীল রিলিজ** — অধিকাংশ ওয়ার্কশপ অংশগ্রহণকারীর কাছে এটি ইনস্টল থাকে  
2. **ফরোয়ার্ড কম্প্যাট কাজ করে** — NuGet প্যাকেজের net8.0 অ্যাসেম্বলি .NET 9 রানটাইমে নির্বিঘ্নে চলে  
3. **.NET 10 (প্রিভিউ/আরসি)** খুব নতুন; সব ব্যবহারকারীর জন্য কাজ করার মতো ওয়ার্কশপে এটি টার্গেট করা সম্ভব নয়

**প্রত্যাশিত:** ভবিষ্যত SDK রিলিজে `net9.0` ও `net10.0` TFMs যোগ করার কথা ভাবা উচিত যাতে `Microsoft.Agents.AI.OpenAI` এর মত প্যাটার্ন মেনে নতুন রানটাইমগুলোর জন্য যাচাইযোগ্য সমর্থন দেওয়া যায়।

---

## ৬. JavaScript ChatClient স্ট্রিমিং কোলব্যাক ব্যবহার করে, Async Iterators নয়

**অবস্থা:** খোলা  
**গুরুত্ব:** ডকুমেন্টেশন ফাঁক  
**উপাদান:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` দ্বারা ফেরত দেয়া `ChatClient` একটি `completeStreamingChat()` মেথড দেয়, তবে এটি async iterable ফেরত দেয় না বরং **callback প্যাটার্ন** ব্যবহার করে:

```javascript
// ❌ এটি কাজ করে না — "stream is not async iterable" ত্রুটি দেয়
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ সঠিক প্যাটার্ন — একটি কলব্যাক পাস করুন
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**প্রভাব:** OpenAI SDK-এর async iteration প্যাটার্ন (`for await`) সঙ্গে পরিচিত ডেভেলপারদের বিভ্রান্তিকর ত্রুটি সম্মুখীন হতে হবে। কলব্যাক একটি বৈধ ফাংশন হতে হবে, নইলে SDK "Callback must be a valid function." ত্রুটি দেয়।

**প্রত্যাশিত:** SDK রেফারেন্সে কলব্যাক প্যাটার্ন ডকুমেন্ট করা উচিত। বিকল্প হিসেবে, OpenAI SDK’র সঙ্গে সামঞ্জস্যের জন্য async iterable প্যাটার্নকে সমর্থন করা উচিত।

---

## পরিবেশের বিবরণ

| উপাদান | সংস্করণ |
|-----------|---------|
| OS | Windows 11 ARM64 |
| হার্ডওয়্যার | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |