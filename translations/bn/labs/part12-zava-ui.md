![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# পার্ট ১২: জাভা ক্রিয়েটিভ রাইটারের জন্য ওয়েব ইউআই তৈরি করা

> **লক্ষ্য:** জাভা ক্রিয়েটিভ রাইটারে একটি ব্রাউজার-ভিত্তিক ফ্রন্ট এন্ড সংযোজন করা যাতে আপনি মাল্টি-এজেন্ট পাইপলাইনকে রিয়েল টাইমে দেখতে পারেন, লাইভ এজেন্ট স্ট্যাটাস ব্যাজ এবং স্ট্রীমড আর্টিকেল টেক্সট, যা সবই একটি একক লোকাল ওয়েব সার্ভার থেকে পরিবেশিত।

[পার্ট ৭](part7-zava-creative-writer.md) তে আপনি জাভা ক্রিয়েটিভ রাইটারকে **CLI অ্যাপ্লিকেশন** (জাভাস্ক্রিপ্ট, C#) এবং একটি **হেডলেস API** (পাইথন) হিসাবে অন্বেষণ করেছিলেন। এই ল্যাবে আপনি একটি শেয়ার্ড **ভ্যানিলা HTML/CSS/JavaScript** ফ্রন্ট এন্ডকে প্রতিটি ব্যাকএন্ডের সাথে সংযোগ করবেন যাতে ব্যবহারকারীরা টার্মিনালের পরিবর্তে ব্রাউজারের মাধ্যমে পাইপলাইনের সাথে যোগাযোগ করতে পারেন।

---

## আপনি কি শিখবেন

| উদ্দেশ্য | বিবরণ |
|-----------|-------------|
| ব্যাকএন্ড থেকে স্ট্যাটিক ফাইল পরিবেশন করা | আপনার API রুটের পাশাপাশি একটি HTML/CSS/JS ডিরেক্টরি মাউন্ট করুন |
| ব্রাউজারে স্ট্রীমিং NDJSON গ্রহণ | Fetch API এবং `ReadableStream` ব্যবহার করে নিউলাইন-সীমাবদ্ধ JSON পড়ুন |
| একক স্ট্রীমিং প্রোটোকল | নিশ্চিত করুন পাইথন, জাভাস্ক্রিপ্ট, এবং C# ব্যাকএন্ড একই মেসেজ ফরম্যাট ইমিট করে |
| প্রগতি UI আপডেট | এজেন্ট স্ট্যাটাস ব্যাজ আপডেট করুন এবং টোকেন অনুসারে আর্টিকেল টেক্সট স্ট্রীম করুন |
| CLI অ্যাপে HTTP লেয়ার যোগ করুন | বিদ্যমান অর্কেস্ট্রেটর লজিককে Express-স্টাইল সার্ভারে (JS) বা ASP.NET Core মিমিনাল API-এ (C#) মোড়ানো |

---

## স্থাপত্য

UI হলো একটি একক সেট স্ট্যাটিক ফাইল (`index.html`, `style.css`, `app.js`) যা তিনটি ব্যাকএন্ডেই শেয়ার করা হয়েছে। প্রতিটি ব্যাকএন্ড একই দুটি রুট প্রকাশ করে:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| রুট | পদ্ধতি | উদ্দেশ্য |
|-------|--------|---------|
| `/` | GET | স্ট্যাটিক UI পরিবেশন করে |
| `/api/article` | POST | মাল্টি-এজেন্ট পাইপলাইন চালানো এবং NDJSON স্ট্রীম করা |

ফ্রন্ট এন্ড একটি JSON বডি পাঠায় এবং নিউলাইন-সীমাবদ্ধ JSON মেসেজের স্ট্রীম হিসেবে রেসপন্স পড়ে। প্রতিটি মেসেজে একটি `type` ফিল্ড থাকে যা UI সঠিক প্যানেল আপডেট করতে ব্যবহার করে:

| মেসেজ টাইপ | অর্থ |
|-------------|---------|
| `message` | স্ট্যাটাস আপডেট (যেমন "শুধু গবেষক এজেন্ট কাজ শুরু করছে...") |
| `researcher` | গবেষণা ফলাফল প্রস্তুত |
| `marketing` | পণ্য অনুসন্ধান ফলাফল প্রস্তুত |
| `writer` | লেখক শুরু বা শেষ হয়েছে (ধার্য `{ start: true }` বা `{ complete: true }`) |
| `partial` | লেখকের একটি একক স্ট্রীমড টোকেন (ধার্য `{ text: "..." }`) |
| `editor` | সম্পাদক মূল্যায়ন প্রস্তুত |
| `error` | কিছু সমস্যা হয়েছে |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## পূর্বপ্রয়োজনীয়তা

- সম্পন্ন করুন [পার্ট ৭: জাভা ক্রিয়েটিভ রাইটার](part7-zava-creative-writer.md)
- Foundry Local CLI ইনস্টল করুন ও `phi-3.5-mini` মডেল ডাউনলোড করুন
- একটি আধুনিক ওয়েব ব্রাউজার (Chrome, Edge, Firefox, বা Safari)

---

## শেয়ার্ড UI

কোনো ব্যাকএন্ড কোড স্পর্শ করার আগে, সাময়িকভাবে সেই ফ্রন্ট এন্ডটি এক্সপ্লোর করুন যা তিনটি ভাষার ট্র্যাক ব্যবহার করবে। ফাইলগুলো থাকে `zava-creative-writer-local/ui/`:

| ফাইল | উদ্দেশ্য |
|------|---------|
| `index.html` | পেজ লেআউট: ইনপুট ফর্ম, এজেন্ট স্ট্যাটাস ব্যাজ, আর্টিকেল আউটপুট এলাকা, সংগ্রাহ্য বিস্তারিত প্যানেল |
| `style.css` | স্ট্যাটাস ব্যাজের রঙের অবস্থা সহ নূন্যতম স্টাইলিং (অপেক্ষা, কার্যকর, সমাপ্ত, ত্রুটি) |
| `app.js` | Fetch কল, `ReadableStream` লাইন রিডার, এবং DOM আপডেট লজিক |

> **টিপ:** সরাসরি `index.html` ব্রাউজারে খুলুন লেআউট প্রিভিউ করতে। এখন পর্যন্ত কিছুই কাজ করবে না কারণ কোনো ব্যাকএন্ড নেই, তবে কাঠামো দেখতে পারবেন।

### স্ট্রীম রিডার কীভাবে কাজ করে

`app.js` এর মূল ফাংশন রেসপন্স বডি ধাপে ধাপে পড়ে এবং নিউলাইন সীমায় ভাগ করে:

```javascript
async function readStream(response) {
  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = "";

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;

    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split("\n");
    buffer = lines.pop(); // অসম্পূর্ণ শেষ লাইন রাখুন

    for (const line of lines) {
      const trimmed = line.trim();
      if (!trimmed) continue;
      try {
        const msg = JSON.parse(trimmed);
        if (msg && msg.type) handleMessage(msg);
      } catch { /* skip non-JSON lines */ }
    }
  }
}
```

প্রতিটি পার্স করা মেসেজ `handleMessage()` এ প্রেরণ করা হয়, যা `msg.type` এর ভিত্তিতে প্রাসঙ্গিক DOM উপাদান আপডেট করে।

---

## অনুশীলন

### অনুশীলন ১: Python ব্যাকএন্ড ইউআই সহ চালানো

Python (FastAPI) ভেরিয়েন্ট ইতিমধ্যে একটি স্ট্রীমিং API এন্ডপয়েন্ট রেখেছে। একমাত্র পরিবর্তন হলো `ui/` ফোল্ডারকে স্ট্যাটিক ফাইল হিসেবে মাউন্ট করা।

**১.১** Python API ডিরেক্টরিতে যান এবং ডিপেনডেন্সি ইনস্টল করুন:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**১.২** সার্ভার শুরু করুন:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**১.৩** ব্রাউজারে `http://localhost:8000` খুলুন। আপনি তিনটি টেক্সট ফিল্ড এবং একটি "Generate Article" বোতামসহ জাভা ক্রিয়েটিভ রাইটার UI দেখতে পাবেন।

**১.৪** ডিফল্ট মান ব্যবহার করে **Generate Article** ক্লিক করুন। প্রত্যেক এজেন্ট তার কাজ শেষ করার সাথে সাথেই এজেন্ট স্ট্যাটাস ব্যাজগুলি "Waiting" থেকে "Running" এবং পরে "Done" তে পরিবর্তিত হচ্ছে দেখুন, এবং আর্টিকেল টেক্সট টোকেন-বাই-টোকেন আউটপুট প্যানেলে স্ট্রীম হচ্ছে।

> **সমস্যা সমাধান:** যদি পৃষ্ঠা UI না দেখিয়ে JSON রেসপন্স দেখায়, নিশ্চিত করুন আপনি আপডেট হওয়া `main.py` চালাচ্ছেন যা স্ট্যাটিক ফাইল মাউন্ট করে। `/api/article` এন্ডপয়েন্ট এখনও তার মূল রাস্তায় কাজ করে; স্ট্যাটিক ফাইল মাউন্ট অন্য সব রুটে UI পরিবেশন করে।

**কীভাবে কাজ করে:** আপডেট হওয়া `main.py` এর শেষে একটি লাইন যুক্ত করা হয়েছে:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

এটি `zava-creative-writer-local/ui/` থেকে প্রতিটি ফাইল স্ট্যাটিক অ্যাসেট হিসেবে পরিবেশন করে, `index.html` ডিফল্ট ডকুমেন্ট হিসেবে। `/api/article` POST রুটটি স্ট্যাটিক মাউন্টের আগে রেজিস্টার করা হয়, তাই এটি অগ্রাধিকার পায়।

---

### অনুশীলন ২: JavaScript ভ্যারিয়েন্টে একটি ওয়েব সার্ভার যোগ করা

JavaScript ভ্যারিয়েন্ট বর্তমানে একটি CLI অ্যাপ্লিকেশন (`main.mjs`)। একটি নতুন ফাইল, `server.mjs`, একই এজেন্ট মডিউলগুলিকে একটি HTTP সার্ভারের পেছনে মোড়ানো এবং শেয়ার্ড UI পরিবেশন করে।

**২.১** JavaScript ডিরেক্টরিতে যান এবং ডিপেনডেন্সি ইনস্টল করুন:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**২.২** ওয়েব সার্ভার শুরু করুন:

```bash
node server.mjs
```

```powershell
node server.mjs
```

আপনি দেখতে পাবেন:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**২.৩** ব্রাউজারে `http://localhost:3000` খুলুন এবং **Generate Article** ক্লিক করুন। একই UI জাভাস্ক্রিপ্ট ব্যাকএন্ডের বিরুদ্ধে একইভাবে কাজ করবে।

**কোড অধ্যয়ন করুন:** `server.mjs` খুলুন এবং মূল প্যাটার্নগুলি লক্ষ্য করুন:

- **স্ট্যাটিক ফাইল পরিবেশন** Node.js বিল্ট-ইন `http`, `fs`, এবং `path` মডিউল ব্যবহার করে কোন অতিরিক্ত ফ্রেমওয়ার্ক ছাড়াই করা হয়েছে।
- **পাথে-ট্র্যাভার্সাল সুরক্ষা** অনুরোধকৃত পথ স্বাভাবিক করে এবং নিশ্চিত করে এটি `ui/` ডিরেক্টরির ভিতরেই থাকে।
- **NDJSON স্ট্রীমিং** `sendLine()` হেল্পার ব্যবহার করে যা প্রতিটি অবজেক্ট সিরিয়ালাইজ করে, অভ্যন্তরীণ নিউলাইনগুলি সরিয়ে এবং একটি ট্রেইলিং নিউলাইন যোগ করে।
- **এজেন্ট অর্কেস্ট্রেশন** বিদ্যমান `researcher.mjs`, `product.mjs`, `writer.mjs`, এবং `editor.mjs` মডিউল অপরিবর্তিত reused হয়।

<details>
<summary>server.mjs থেকে মূল অংশ</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// গবেষক
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// লেখক (স্ট্রিমিং)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### অনুশীলন ৩: C# ভ্যারিয়েন্টে একটি মিমিনাল API যোগ করা

C# ভ্যারিয়েন্ট বর্তমানে একটি কনসোল অ্যাপ্লিকেশন। একটি নতুন প্রকল্প, `csharp-web`, ASP.NET Core মিমিনাল API ব্যবহার করে একই পাইপলাইনকে ওয়েব সার্ভিস হিসাবে এক্সপোজ করে।

**৩.১** C# ওয়েব প্রকল্পে যান এবং প্যাকেজগুলি পুনরুদ্ধার করুন:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**৩.২** ওয়েব সার্ভার চালু করুন:

```bash
dotnet run
```

```powershell
dotnet run
```

আপনি দেখতে পাবেন:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**৩.৩** ব্রাউজারে `http://localhost:5000` খুলুন এবং **Generate Article** ক্লিক করুন।

**কোড অধ্যয়ন করুন:** `csharp-web` ডিরেক্টরিতে `Program.cs` খুলুন এবং লক্ষ্য করুন:

- প্রকল্প ফাইল `Microsoft.NET.Sdk` এর পরিবর্তে `Microsoft.NET.Sdk.Web` ব্যবহার করে, যা ASP.NET Core সমর্থন যোগ করে।
- স্ট্যাটিক ফাইল `UseDefaultFiles` এবং `UseStaticFiles` মাধ্যমে শেয়ার্ড `ui/` ডিরেক্টরির দিকে পরিবেশন করা হয়।
- `/api/article` এন্ডপয়েন্ট NDJSON লাইন সরাসরি `HttpContext.Response` এ লেখে এবং প্রতিটি লাইনের পরে ফ্লাশ করে রিয়েল-টাইম স্ট্রীমিং নিশ্চিত করে।
- সব এজেন্ট লজিক (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) কনসোল ভার্সনের মতোই।

<details>
<summary>csharp-web/Program.cs থেকে মূল অংশ</summary>

```csharp
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/event-stream; charset=utf-8";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    // Researcher
    await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
    var researchResult = RunResearcher(body.Research, feedback);
    await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

    // Writer (streaming)
    foreach (var update in completionUpdates)
    {
        if (update.ContentUpdate.Count > 0)
        {
            var text = update.ContentUpdate[0].Text;
            await SendLine(new { type = "partial", message = "token", data = new { text } });
        }
    }
});
```

</details>

---

### অনুশীলন ৪: এজেন্ট স্ট্যাটাস ব্যাজ অন্বেষণ করা

এখন যেহেতু আপনার ইউআই কাজ করছে, দেখুন ফ্রন্ট এন্ড কীভাবে স্ট্যাটাস ব্যাজ আপডেট করে।

**৪.১** `zava-creative-writer-local/ui/app.js` আপনার এডিটরে খুলুন।

**৪.২** `handleMessage()` ফাংশন খুঁজুন। লক্ষ্য করুন এটি কীভাবে মেসেজ টাইপগুলো DOM আপডেটের সাথে ম্যাপ করে:

| মেসেজ টাইপ | UI অ্যাকশন |
|-------------|------------|
| `message` যার ভিতরে "researcher" আছে | Researcher ব্যাজকে "Running" সেট করে |
| `researcher` | Researcher ব্যাজকে "Done" সেট করে এবং Research Results প্যানেল পূরণ করে |
| `marketing` | Product Search ব্যাজকে "Done" সেট করে এবং Product Matches প্যানেল পূরণ করে |
| `writer` যেখানে `data.start` আছে | Writer ব্যাজকে "Running" সেট করে এবং আর্টিকেল আউটপুট পরিষ্কার করে |
| `partial` | টোকেন টেক্সট আর্টিকেল আউটপুটে যোগ করে |
| `writer` যেখানে `data.complete` আছে | Writer ব্যাজকে "Done" সেট করে |
| `editor` | Editor ব্যাজকে "Done" সেট করে এবং Editor Feedback প্যানেল পূরণ করে |

**৪.৩** আর্টিকেলের নিচে থাকা সংগ্রাহ্যযোগ্য "Research Results", "Product Matches", এবং "Editor Feedback" প্যানেলগুলি খুলে প্রতিটি এজেন্ট যে কাঁচা JSON উৎপন্ন করেছে তা পরিদর্শন করুন।

---

### অনুশীলন ৫: UI কাস্টমাইজ করা (অতিরিক্ত)

এই উন্নতিগুলোর যেকোনো একটি বা একাধিক চেষ্টা করুন:

**৫.১ শব্দ গণনা যোগ করুন।** লেখক শেষ হওয়ার পর, আউটপুট প্যানেলের নিচে আর্টিকেলের শব্দ সংখ্যা দেখান। আপনি এটি `handleMessage`-এ গণনা করতে পারেন যখন `type === "writer"` এবং `data.complete` সত্য:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**৫.২ পুনরায় চেষ্টা সূচক যোগ করুন।** যখন সম্পাদক সংশোধনের অনুরোধ করে, পাইপলাইন আবার চালানো হয়। স্ট্যাটাস প্যানেলে "Revision 1" বা "Revision 2" ব্যানার দেখান। একটি `message` টাইপ শুনুন যাতে "Revision" থাকে এবং একটি নতুন DOM উপাদান আপডেট করুন।

**৫.৩ ডার্ক মোড।** একটি টগল বোতাম এবং `<body>`-তে `.dark` ক্লাস যোগ করুন। `style.css`-এ `body.dark` সিলেক্টর ব্যবহার করে ব্যাকগ্রাউন্ড, টেক্সট এবং প্যানেল রঙ ওভাররাইড করুন।

---

## সংক্ষিপ্তসার

| আপনি কি করেছেন | কিভাবে |
|-------------|-----|
| Python ব্যাকএন্ড থেকে UI পরিবেশন করেছেন | FastAPI তে `StaticFiles` দিয়ে `ui/` ফোল্ডার মাউন্ট করেছেন |
| JavaScript ভ্যারিয়েন্টে HTTP সার্ভার যোগ করেছেন | বিল্ট-ইন Node.js `http` মডিউল ব্যবহার করে `server.mjs` তৈরি করেছেন |
| C# ভ্যারিয়েন্টে ওয়েব API যোগ করেছেন | ASP.NET Core মিমিনাল API সহ নতুন `csharp-web` প্রকল্প তৈরি করেছেন |
| ব্রাউজারে স্ট্রীমিং NDJSON গ্রহণ করেছেন | `fetch()` ও `ReadableStream` ব্যবহার করে লাইন-বাই-লাইন JSON পার্স করেছেন |
| বাস্তব সময়ে UI আপডেট করেছেন | মেসেজ টাইপ ম্যাপ করে DOM আপডেট করেছেন (ব্যাজ, টেক্সট, সংগ্রাহ্য প্যানেল) |

---

## প্রধান শিখনবিন্দু

১. একটি **শেয়ার্ড স্ট্যাটিক ফ্রন্ট এন্ড** যে কোনো ব্যাকএন্ডের সাথে কাজ করতে পারে যা একই স্ট্রীমিং প্রোটোকল অনুসরণ করে, যা OpenAI-অনুরূপ API প্যাটার্নের মূল্যকে শক্তিশালী করে।
২. **নিউলাইন-সীমাবদ্ধ JSON (NDJSON)** একটি সরল স্ট্রীমিং ফরম্যাট যা ব্রাউজারের `ReadableStream` API-র সাথে প্রাকৃতিকভাবে কাজ করে।
৩. **Python ভ্যারিয়েন্ট** সবচেয়ে কম পরিবর্তন চেয়েছে কারণ তার আগেই FastAPI এন্ডপয়েন্ট ছিল; JavaScript এবং C# ভার্সনে একটি পাতলা HTTP মোড়ক প্রয়োজন হয়েছে।
৪. UI-কে **ভ্যানিলা HTML/CSS/JS** হিসেবে রাখা হয়েছে যাতে বিল্ড টুলস, ফ্রেমওয়ার্ক নির্ভরতা ও অতিরিক্ত জটিলতা এড়ানো যায় ও কর্মশালা শিক্ষার্থীদের সুবিধা হয়।
৫. একই এজেন্ট মডিউলগুলি (Researcher, Product, Writer, Editor) পরিবর্তন ছাড়াই পুনরায় ব্যবহার করা হয়েছে; শুধু পরিবহন লেয়ার পরিবর্তিত হয়েছে।

---

## আরও পড়ুন

| রিসোর্স | লিংক |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

চালিয়ে যান [পার্ট ১৩: ওয়ার্কশপ সমাপ্ত](part13-workshop-complete.md) যেখানে আপনি এই ওয়ার্কশপে নির্মিত সবকিছুর সারাংশ পাবেন।

---
[← অংশ ১১: টুল কলিং](part11-tool-calling.md) | [অংশ ১৩: কর্মশালা সম্পন্ন →](part13-workshop-complete.md)