![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagian 11: Panggilan Alat dengan Model Tempatan

> **Matlamat:** Membolehkan model tempatan anda memanggil fungsi luaran (alat) supaya ia boleh mendapatkan data masa nyata, melakukan pengiraan, atau berinteraksi dengan API — semua berjalan secara peribadi pada peranti anda.

## Apakah Panggilan Alat?

Panggilan alat (juga dikenali sebagai **panggilan fungsi**) membolehkan model bahasa meminta pelaksanaan fungsi yang anda tentukan. Daripada meneka jawapan, model mengenal pasti bila alat akan membantu dan memulangkan permintaan berstruktur untuk kod anda laksanakan. Aplikasi anda menjalankan fungsi, menghantar hasil kembali, dan model memasukkan maklumat itu ke dalam jawapan akhir.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Corak ini penting untuk membina agen yang boleh:

- **Mencari data langsung** (cuaca, harga saham, pertanyaan pangkalan data)
- **Melakukan pengiraan tepat** (matematik, penukaran unit)
- **Mengambil tindakan** (menghantar e-mel, membuat tiket, mengemas kini rekod)
- **Akses sistem peribadi** (API dalaman, sistem fail)

---

## Bagaimana Panggilan Alat Berfungsi

Aliran panggilan alat mempunyai empat peringkat:

| Peringkat | Apa yang Berlaku |
|-----------|------------------|
| **1. Tentukan alat** | Anda menghuraikan fungsi yang tersedia menggunakan Skema JSON — nama, penerangan, dan parameter |
| **2. Model membuat keputusan** | Model menerima mesej anda serta definisi alat. Jika alat diperlukan, ia mengembalikan respons `tool_calls` bukannya jawapan teks |
| **3. Laksana secara tempatan** | Kod anda mengurai panggilan alat, menjalankan fungsi, dan mengumpul hasil |
| **4. Jawapan akhir** | Anda menghantar hasil alat kembali ke model, yang menghasilkan respons akhir |

> **Titik penting:** Model tidak pernah melaksanakan kod. Ia hanya *meminta* supaya alat dipanggil. Aplikasi anda yang menentukan sama ada permintaan itu diterima — ini memastikan anda mempunyai kawalan penuh.

---

## Model Mana yang Menyokong Panggilan Alat?

Tidak semua model menyokong panggilan alat. Dalam katalog Foundry Local sekarang, model berikut mempunyai keupayaan panggilan alat:

| Model | Saiz | Panggilan Alat |
|-------|------|:-------------:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b | 6.3 GB | ✅ |
| qwen2.5-14b | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b | 6.3 GB | ✅ |
| qwen2.5-coder-14b | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4 | 10.4 GB | ❌ |

> **Petua:** Untuk makmal ini kami menggunakan **qwen2.5-0.5b** — ia kecil (muat turun 822 MB), pantas, dan mempunyai sokongan panggilan alat yang boleh dipercayai.

---

## Objektif Pembelajaran

Menjelang akhir makmal ini anda akan dapat:

- Terangkan corak panggilan alat dan kenapa ia penting untuk agen AI
- Tentukan skema alat menggunakan format panggilan fungsi OpenAI
- Tangani aliran perbualan panggilan alat berbilang giliran
- Laksanakan panggilan alat secara tempatan dan kembalikan hasil kepada model
- Pilih model yang sesuai untuk senario panggilan alat

---

## Prasyarat

| Keperluan | Butiran |
|-----------|---------|
| **Foundry Local CLI** | Telah dipasang dan terdapat dalam `PATH` anda ([Bahagian 1](part1-getting-started.md)) |
| **Foundry Local SDK** | SDK Python, JavaScript, atau C# telah dipasang ([Bahagian 2](part2-foundry-local-sdk.md)) |
| **Model panggilan alat** | qwen2.5-0.5b (akan dimuat turun secara automatik) |

---

## Latihan

### Latihan 1 — Memahami Aliran Panggilan Alat

Sebelum menulis kod, kaji rajah urutan berikut:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Pemerhatian utama:**

1. Anda menentukan alat dahulu sebagai objek Skema JSON
2. Respons model mengandungi `tool_calls` bukannya kandungan biasa
3. Setiap panggilan alat mempunyai `id` unik yang anda mesti rujuk semasa mengembalikan hasil
4. Model melihat semua mesej sebelumnya *dan* hasil alat semasa menghasilkan jawapan akhir
5. Berbilang panggilan alat boleh berlaku dalam satu respons

> **Perbincangan:** Kenapa model memulangkan panggilan alat dan bukan melaksanakan fungsi secara langsung? Apakah kelebihan keselamatan yang ia berikan?

---

### Latihan 2 — Menentukan Skema Alat

Alat ditentukan menggunakan format panggilan fungsi OpenAI yang standard. Setiap alat perlu:

- **`type`**: Sentiasa `"function"`
- **`function.name`**: Nama fungsi yang menggambarkan (contoh `get_weather`)
- **`function.description`**: Penerangan jelas — model gunakan ini untuk menentukan bila perlu memanggil alat
- **`function.parameters`**: Objek Skema JSON yang menerangkan argumen yang dijangka

```json
{
  "type": "function",
  "function": {
    "name": "get_weather",
    "description": "Get the current weather for a given city",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. London"
        }
      },
      "required": ["city"]
    }
  }
}
```

> **Amalan terbaik untuk penerangan alat:**
> - Jadi spesifik: "Dapatkan cuaca semasa untuk bandar tertentu" lebih baik daripada "Dapatkan cuaca"
> - Terangkan parameter dengan jelas: model membaca penerangan ini untuk mengisi nilai yang betul
> - Tandakan parameter wajib dan pilihan — ini membantu model menentukan apa yang perlu ditanya

---

### Latihan 3 — Jalankan Contoh Panggilan Alat

Setiap contoh bahasa mentakrifkan dua alat (`get_weather` dan `get_population`), menghantar soalan yang mencetuskan penggunaan alat, melaksanakan alat secara tempatan, dan mengembalikan hasil untuk jawapan akhir.

<details>
<summary><strong>🐍 Python</strong></summary>

**Prasyarat:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Jalankan:**
```bash
python foundry-local-tool-calling.py
```

**Output dijangka:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Penerangan kod** (`python/foundry-local-tool-calling.py`):

```python
# Takrifkan alatan sebagai senarai skema fungsi
tools = [
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "Get the current weather for a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "The city name"}
                },
                "required": ["city"]
            }
        }
    }
]

# Hantar dengan alatan — model mungkin mengembalikan panggilan_alatan dan bukan kandungan
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Periksa jika model mahu memanggil alat
if response.choices[0].message.tool_calls:
    # Laksanakan alat dan hantar keputusan kembali
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Prasyarat:**
```bash
cd javascript
npm install
```

**Jalankan:**
```bash
node foundry-local-tool-calling.mjs
```

**Output dijangka:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Penerangan kod** (`javascript/foundry-local-tool-calling.mjs`):

Contoh ini menggunakan `ChatClient` asli Foundry Local SDK dan bukan OpenAI SDK, memperlihatkan kemudahan kaedah `createChatClient()`:

```javascript
// Dapatkan ChatClient terus dari objek model
const chatClient = model.createChatClient();

// Hantar dengan alat — ChatClient mengendalikan format yang serasi dengan OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Periksa panggilan alat
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Laksanakan alat dan hantar hasil kembali
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Prasyarat:**
```bash
cd csharp
dotnet restore
```

**Jalankan:**
```bash
dotnet run toolcall
```

**Output dijangka:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Penerangan kod** (`csharp/ToolCalling.cs`):

C# menggunakan pembantu `ChatTool.CreateFunctionTool` untuk menentukan alat:

```csharp
ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
    functionName: "get_weather",
    functionDescription: "Get the current weather for a given city",
    functionParameters: BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "city": { "type": "string", "description": "The city name" }
        },
        "required": ["city"]
    }
    """));

var options = new ChatCompletionOptions();
options.Tools.Add(getWeatherTool);

// Check FinishReason to see if tools were called
if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    // Execute tools and send results back
    ...
}
```

</details>

---

### Latihan 4 — Aliran Perbualan Panggilan Alat

Memahami struktur mesej adalah penting. Berikut adalah aliran lengkap, menunjukkan tatasusunan `messages` pada setiap peringkat:

**Peringkat 1 — Permintaan awal:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Peringkat 2 — Model membalas dengan `tool_calls` (bukan kandungan):**
```json
{
  "role": "assistant",
  "tool_calls": [
    {
      "id": "call_abc123",
      "type": "function",
      "function": {
        "name": "get_weather",
        "arguments": "{\"city\": \"London\"}"
      }
    }
  ]
}
```

**Peringkat 3 — Anda menambah mesej pembantu DAN hasil alat:**
```json
[
  {"role": "system", "content": "..."},
  {"role": "user", "content": "What is the weather like in London?"},
  {"role": "assistant", "tool_calls": [...]},
  {
    "role": "tool",
    "tool_call_id": "call_abc123",
    "content": "{\"city\": \"London\", \"temperature\": \"18°C\", \"condition\": \"Partly cloudy\"}"
  }
]
```

**Peringkat 4 — Model menghasilkan jawapan akhir menggunakan hasil alat.**

> **Penting:** `tool_call_id` dalam mesej alat mesti sepadan dengan `id` dari panggilan alat. Ini cara model mengaitkan hasil dengan permintaan.

---

### Latihan 5 — Berbilang Panggilan Alat

Model boleh meminta beberapa panggilan alat dalam satu respons. Cuba ubah mesej pengguna untuk mencetuskan panggilan berganda:

```python
# Dalam Python — tukar mesej pengguna:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// Dalam JavaScript — tukar mesej pengguna:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Model sepatutnya memulangkan dua `tool_calls` — satu untuk `get_weather` dan satu untuk `get_population`. Kod anda sudah mengendalikan ini kerana ia melalui semua panggilan alat.

> **Cuba:** Ubah mesej pengguna dan jalankan contoh semula. Adakah model memanggil kedua-dua alat?

---

### Latihan 6 — Tambah Alat Sendiri

Kembangkan salah satu contoh dengan alat baru. Contohnya, tambah alat `get_time`:

1. Tentukan skema alat:
```json
{
  "type": "function",
  "function": {
    "name": "get_time",
    "description": "Get the current time in a given city's timezone",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. Tokyo"
        }
      },
      "required": ["city"]
    }
  }
}
```

2. Tambah logik pelaksanaan:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Dalam aplikasi sebenar, gunakan perpustakaan zon masa
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... alat sedia ada ...
```

3. Tambah alat ke dalam tatasusunan `tools` dan uji dengan soalan: "Berapakah masa di Tokyo?"

> **Cabaran:** Tambah alat yang melakukan pengiraan, seperti `convert_temperature` yang menukarkan antara Celsius dan Fahrenheit. Uji dengan soalan: "Tukar 100°F ke Celsius."

---

### Latihan 7 — Panggilan Alat dengan ChatClient SDK (JavaScript)

Contoh JavaScript sudah menggunakan `ChatClient` asli SDK berbanding OpenAI SDK. Ini ciri kemudahan yang menghilangkan keperluan membina klien OpenAI sendiri:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient dicipta terus dari objek model
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat menerima alat sebagai parameter kedua
const response = await chatClient.completeChat(messages, tools);
```

Bandingkan dengan pendekatan Python yang menggunakan OpenAI SDK secara jelas:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Kedua-dua corak adalah sah. `ChatClient` lebih mudah; OpenAI SDK memberikan akses penuh kepada semua parameter OpenAI.

> **Cuba:** Ubah contoh JavaScript untuk menggunakan OpenAI SDK dan bukannya `ChatClient`. Anda perlu `import OpenAI from "openai"` dan bina klien dengan titik hujung daripada `manager.urls[0]`.

---

### Latihan 8 — Memahami tool_choice

Parameter `tool_choice` mengawal sama ada model *mesti* menggunakan alat atau boleh memilih secara bebas:

| Nilai | Tingkah laku |
|-------|--------------|
| `"auto"` | Model menentukan sama ada memanggil alat (lalai) |
| `"none"` | Model tidak akan memanggil sebarang alat walaupun disediakan |
| `"required"` | Model mesti memanggil sekurang-kurangnya satu alat |
| `{"type": "function", "function": {"name": "get_weather"}}` | Model mesti memanggil alat yang ditentukan |

Cuba setiap pilihan dalam contoh Python:

```python
# Paksa model untuk memanggil get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Nota:** Tidak semua pilihan `tool_choice` disokong oleh setiap model. Jika model tidak menyokong `"required"`, mungkin ia mengabaikan tetapan itu dan berkelakuan seperti `"auto"`.

---

## Perangkap Biasa

| Masalah | Penyelesaian |
|---------|--------------|
| Model tidak pernah memanggil alat | Pastikan anda menggunakan model yang menyokong panggilan alat (contohnya qwen2.5-0.5b). Semak jadual di atas. |
| `tool_call_id` tidak sepadan | Sentiasa guna `id` dari respons panggilan alat, bukan nilai keras |
| Model memulangkan JSON rosak dalam `arguments` | Model lebih kecil kadangkala menghasilkan JSON tidak sah. Bungkus `JSON.parse()` dalam try/catch |
| Model memanggil alat yang tidak wujud | Tambah pengendali lalai dalam fungsi `execute_tool` anda |
| Gelung panggilan alat tanpa henti | Tetapkan bilangan maksimum pusingan (contoh: 5) untuk mengelakkan gelung tanpa kawalan |

---

## Kesimpulan Utama

1. **Panggilan alat** membolehkan model meminta pelaksanaan fungsi daripada meneka jawapan
2. Model **tidak pernah melaksanakan kod**; aplikasi anda yang menentukan apa yang dijalankan
3. Alat ditentukan sebagai objek **Skema JSON** mengikut format panggilan fungsi OpenAI
4. Perbualan menggunakan **corak berbilang giliran**: pengguna, pembantu (tool_calls), alat (keputusan), pembantu (jawapan akhir)
5. Sentiasa guna **model yang menyokong panggilan alat** (Qwen 2.5, Phi-4-mini)
6. `createChatClient()` SDK menyediakan cara mudah untuk membuat permintaan panggilan alat tanpa membina klien OpenAI

---

Teruskan ke [Bahagian 12: Membangun UI Web untuk Penulis Kreatif Zava](part12-zava-ui.md) untuk menambah antara muka berasaskan pelayar kepada saluran multi-agen dengan penstriman masa nyata.

---

[← Bahagian 10: Model Tersuai](part10-custom-models.md) | [Bahagian 12: UI Penulis Zava →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila maklum bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidaktepatan. Dokumen asal dalam bahasa asalnya harus dianggap sebagai sumber yang sahih. Untuk maklumat penting, terjemahan profesional oleh manusia adalah disyorkan. Kami tidak bertanggungjawab atas sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->