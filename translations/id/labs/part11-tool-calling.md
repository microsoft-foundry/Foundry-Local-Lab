![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bagian 11: Pemanggilan Alat dengan Model Lokal

> **Tujuan:** Memungkinkan model lokal Anda memanggil fungsi eksternal (alat) sehingga dapat mengambil data waktu nyata, melakukan perhitungan, atau berinteraksi dengan API — semua berjalan secara pribadi di perangkat Anda.

## Apa Itu Pemanggilan Alat?

Pemanggilan alat (juga dikenal sebagai **pemanggilan fungsi**) memungkinkan model bahasa meminta eksekusi fungsi yang Anda definisikan. Alih-alih menebak jawaban, model mengenali saat alat akan membantu dan mengembalikan permintaan terstruktur untuk kode Anda eksekusi. Aplikasi Anda menjalankan fungsi, mengirim hasil kembali, dan model menggabungkan informasi tersebut ke dalam respons akhirnya.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Polanya penting untuk membangun agen yang dapat:

- **Melihat data langsung** (cuaca, harga saham, kueri database)
- **Melakukan perhitungan tepat** (matematika, konversi satuan)
- **Mengambil tindakan** (mengirim email, membuat tiket, memperbarui catatan)
- **Mengakses sistem privat** (API internal, sistem berkas)

---

## Cara Kerja Pemanggilan Alat

Alur pemanggilan alat memiliki empat tahap:

| Tahap | Apa yang Terjadi |
|-------|------------------|
| **1. Definisikan alat** | Anda mendeskripsikan fungsi yang tersedia menggunakan JSON Schema — nama, deskripsi, dan parameter |
| **2. Model memutuskan** | Model menerima pesan Anda plus definisi alat. Jika alat akan membantu, ia mengembalikan respons `tool_calls` bukannya jawaban teks |
| **3. Eksekusi lokal** | Kode Anda menguraikan panggilan alat, menjalankan fungsi, dan mengumpulkan hasil |
| **4. Jawaban akhir** | Anda mengirim hasil alat kembali ke model, yang menghasilkan respons akhirnya |

> **Poin penting:** Model tidak pernah menjalankan kode. Ia hanya *meminta* agar alat dipanggil. Aplikasi Anda yang memutuskan apakah permintaan itu dipenuhi — ini menjaga Anda tetap dalam kendali penuh.

---

## Model Mana yang Mendukung Pemanggilan Alat?

Tidak semua model mendukung pemanggilan alat. Dalam katalog Foundry Local saat ini, model berikut memiliki kemampuan pemanggilan alat:

| Model | Ukuran | Pemanggilan Alat |
|-------|--------|:----------------:|
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

> **Tip:** Untuk lab ini kita menggunakan **qwen2.5-0.5b** — ukurannya kecil (822 MB unduhan), cepat, dan memiliki dukungan pemanggilan alat yang handal.

---

## Tujuan Pembelajaran

Pada akhir lab ini Anda akan dapat:

- Menjelaskan pola pemanggilan alat dan mengapa itu penting bagi agen AI
- Mendefinisikan skema alat menggunakan format pemanggilan fungsi OpenAI
- Menangani alur percakapan pemanggilan alat multi-langkah
- Menjalankan panggilan alat secara lokal dan mengembalikan hasil ke model
- Memilih model yang tepat untuk skenario pemanggilan alat

---

## Prasyarat

| Persyaratan | Detail |
|-------------|---------|
| **Foundry Local CLI** | Terpasang dan ada di `PATH` Anda ([Bagian 1](part1-getting-started.md)) |
| **Foundry Local SDK** | SDK Python, JavaScript, atau C# terpasang ([Bagian 2](part2-foundry-local-sdk.md)) |
| **Model pemanggilan alat** | qwen2.5-0.5b (akan diunduh otomatis) |

---

## Latihan

### Latihan 1 — Memahami Alur Pemanggilan Alat

Sebelum menulis kode, pelajari diagram urutan ini:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Pengamatan utama:**

1. Anda mendefinisikan alat terlebih dahulu sebagai objek JSON Schema  
2. Respons model berisi `tool_calls` bukan konten biasa  
3. Setiap panggilan alat memiliki `id` unik yang harus Anda referensikan saat mengembalikan hasil  
4. Model melihat semua pesan sebelumnya *plus* hasil alat saat menghasilkan jawaban akhir  
5. Beberapa pemanggilan alat bisa terjadi dalam satu respons

> **Diskusi:** Mengapa model mengembalikan panggilan alat daripada mengeksekusi fungsi langsung? Keuntungan keamanan apa yang diberikan?

---

### Latihan 2 — Mendefinisikan Skema Alat

Alat didefinisikan menggunakan format pemanggilan fungsi standar OpenAI. Setiap alat membutuhkan:

- **`type`**: Selalu `"function"`
- **`function.name`**: Nama fungsi yang deskriptif (misalnya `get_weather`)  
- **`function.description`**: Deskripsi jelas — model menggunakan ini untuk memutuskan kapan memanggil alat  
- **`function.parameters`**: Objek JSON Schema yang mendeskripsikan argumen yang diharapkan  

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

> **Praktik terbaik untuk deskripsi alat:**
> - Spesifik: "Dapatkan cuaca saat ini untuk kota tertentu" lebih baik daripada "Dapatkan cuaca"  
> - Deskripsikan parameter dengan jelas: model membaca deskripsi ini untuk mengisi nilai yang tepat  
> - Tandai parameter wajib vs opsional — ini membantu model memutuskan apa yang perlu diminta  

---

### Latihan 3 — Menjalankan Contoh Pemanggilan Alat

Setiap contoh bahasa mendefinisikan dua alat (`get_weather` dan `get_population`), mengirim pertanyaan yang memicu penggunaan alat, menjalankan alat secara lokal, dan mengirim hasil kembali untuk jawaban akhir.

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
  
**Output yang diharapkan:**  
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**Penjelasan kode** (`python/foundry-local-tool-calling.py`):

```python
# Definisikan tools sebagai daftar skema fungsi
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

# Kirim dengan tools — model mungkin mengembalikan tool_calls alih-alih konten
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Periksa apakah model ingin memanggil sebuah tool
if response.choices[0].message.tool_calls:
    # Jalankan tool dan kirim hasilnya kembali
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
  
**Output yang diharapkan:**  
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**Penjelasan kode** (`javascript/foundry-local-tool-calling.mjs`):

Contoh ini menggunakan `ChatClient` bawaan Foundry Local SDK alih-alih OpenAI SDK, menunjukkan kemudahan metode `createChatClient()`:

```javascript
// Dapatkan ChatClient langsung dari objek model
const chatClient = model.createChatClient();

// Kirim dengan alat — ChatClient menangani format yang kompatibel dengan OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Periksa panggilan alat
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Jalankan alat dan kirim hasil kembali
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
  
**Output yang diharapkan:**  
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**Penjelasan kode** (`csharp/ToolCalling.cs`):

C# menggunakan helper `ChatTool.CreateFunctionTool` untuk mendefinisikan alat:

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

### Latihan 4 — Alur Percakapan Pemanggilan Alat

Memahami struktur pesan sangat penting. Berikut alur lengkap, menampilkan array `messages` pada setiap tahap:

**Tahap 1 — Permintaan awal:**  
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```
  
**Tahap 2 — Model merespons dengan `tool_calls` (bukan konten):**  
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
  
**Tahap 3 — Anda menambahkan pesan asisten DAN hasil alat:**  
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
  
**Tahap 4 — Model menghasilkan jawaban akhir menggunakan hasil alat.**

> **Penting:** `tool_call_id` di pesan alat harus cocok dengan `id` dari panggilan alat. Ini cara model mengasosiasikan hasil dengan permintaan.

---

### Latihan 5 — Beberapa Panggilan Alat

Model bisa meminta beberapa panggilan alat dalam satu respons. Coba ubah pesan pengguna untuk memicu beberapa panggilan:

```python
# Dalam Python — ubah pesan pengguna:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```
  
```javascript
// Dalam JavaScript — ubah pesan pengguna:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```
  
Model harus mengembalikan dua `tool_calls` — satu untuk `get_weather` dan satu untuk `get_population`. Kode Anda sudah menangani ini karena melakukan loop untuk semua panggilan alat.

> **Coba:** Modifikasi pesan pengguna dan jalankan contoh lagi. Apakah model memanggil kedua alat tersebut?

---

### Latihan 6 — Tambahkan Alat Anda Sendiri

Perluas salah satu contoh dengan alat baru. Misalnya, tambahkan alat `get_time`:

1. Definisikan skema alat:  
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
  
2. Tambahkan logika eksekusi:  
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Dalam aplikasi nyata, gunakan perpustakaan zona waktu
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... alat yang ada ...
```
  
3. Tambahkan alat ke array `tools` dan uji dengan: "Jam berapa di Tokyo?"

> **Tantangan:** Tambahkan alat yang melakukan perhitungan, seperti `convert_temperature` yang mengonversi antara Celsius dan Fahrenheit. Uji dengan: "Konversi 100°F ke Celsius."

---

### Latihan 7 — Pemanggilan Alat dengan ChatClient SDK (JavaScript)

Contoh JavaScript sudah menggunakan `ChatClient` native SDK daripada OpenAI SDK. Ini fitur kemudahan yang menghilangkan kebutuhan membangun klien OpenAI sendiri:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient dibuat langsung dari objek model
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat menerima tools sebagai parameter kedua
const response = await chatClient.completeChat(messages, tools);
```
  
Bandingkan dengan pendekatan Python yang menggunakan OpenAI SDK secara eksplisit:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```
  
Kedua pola valid. `ChatClient` lebih mudah; OpenAI SDK memberikan akses ke seluruh parameter OpenAI.

> **Coba:** Modifikasi contoh JavaScript untuk menggunakan OpenAI SDK alih-alih `ChatClient`. Anda perlu `import OpenAI from "openai"` dan membuat klien dengan endpoint dari `manager.urls[0]`.

---

### Latihan 8 — Memahami tool_choice

Parameter `tool_choice` mengontrol apakah model *harus* menggunakan alat atau dapat memilih bebas:

| Nilai | Perilaku |
|-------|----------|
| `"auto"` | Model memutuskan apakah akan memanggil alat (default) |
| `"none"` | Model tidak akan memanggil alat apapun, meskipun diberikan |
| `"required"` | Model harus memanggil setidaknya satu alat |
| `{"type": "function", "function": {"name": "get_weather"}}` | Model harus memanggil alat tertentu |

Coba setiap opsi di contoh Python:

```python
# Memaksa model untuk memanggil get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```
  
> **Catatan:** Tidak semua opsi `tool_choice` didukung oleh setiap model. Jika model tidak mendukung `"required"`, mungkin mengabaikan pengaturan dan berperilaku seperti `"auto"`.

---

## Kesalahan Umum

| Masalah | Solusi |
|---------|---------|
| Model tidak pernah memanggil alat | Pastikan Anda menggunakan model pemanggilan alat (misalnya qwen2.5-0.5b). Periksa tabel di atas. |
| Ketidakcocokan `tool_call_id` | Selalu gunakan `id` dari respons panggilan alat, bukan nilai yang dikodekan keras |
| Model mengembalikan JSON rusak di `arguments` | Model kecil kadang menghasilkan JSON tidak valid. Bungkus `JSON.parse()` dengan try/catch |
| Model memanggil alat yang tidak ada | Tambahkan handler default di fungsi `execute_tool` Anda |
| Loop pemanggilan alat tak berujung | Tetapkan jumlah putaran maksimum (misal 5) untuk mencegah loop tak terkendali |

---

## Intisari

1. **Pemanggilan alat** memungkinkan model meminta eksekusi fungsi daripada menebak jawaban  
2. Model **tidak pernah menjalankan kode**; aplikasi Anda memutuskan apa yang dijalankan  
3. Alat didefinisikan sebagai objek **JSON Schema** mengikuti format pemanggilan fungsi OpenAI  
4. Percakapan menggunakan pola **multilangkah**: pengguna, lalu asisten (tool_calls), lalu alat (hasil), lalu asisten (jawaban akhir)  
5. Selalu gunakan **model yang mendukung pemanggilan alat** (Qwen 2.5, Phi-4-mini)  
6. `createChatClient()` SDK menyediakan cara mudah untuk membuat permintaan pemanggilan alat tanpa membangun klien OpenAI sendiri  

---

Lanjut ke [Bagian 12: Membangun UI Web untuk Zava Creative Writer](part12-zava-ui.md) untuk menambahkan antarmuka berbasis browser pada pipeline multi-agent dengan streaming waktu nyata.

---

[← Bagian 10: Model Kustom](part10-custom-models.md) | [Bagian 12: UI Penulis Zava →](part12-zava-ui.md)