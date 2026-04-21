![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 11: Tööriistade kutsumine kohalike mudelitega

> **Eesmärk:** Võimaldada sinu kohaliku mudeli kutsuda väliseid funktsioone (tööriistu), et see saaks pärida reaalajas andmeid, teha arvutusi või suhelda API-dega — kõik käivitub privaatselt sinu seadmes.

## Mis on tööriistade kutsumine?

Tööriistade kutsumine (tuntud ka kui **funktsioonide kutsumine**) võimaldab keelemudelil taotleda sinu määratletud funktsioonide käivitamist. Selle asemel, et arvata vastust, tuvastab mudel, millal tööriist oleks abiks, ja tagastab struktuurse päringu sinu koodi täitmiseks. Sinu rakendus käivitab funktsiooni, saadab tulemuse tagasi ning mudel kaasab selle info oma lõpliku vastuse koostamisse.

![Tööriistade kutsumise voog](../../../images/tool-calling-flow.svg)

See muster on oluline agentide ehitamiseks, kes saavad:

- **Pärida otse reaalajas andmeid** (ilmaennustus, aktsiahinnad, andmebaasi päringud)
- **Teha täpseid arvutusi** (matemaatika, ühikute teisendused)
- **Teha toiminguid** (saata e-kirju, luua pileteid, uuendada kirjeid)
- **Juurdepääsu privaatsetele süsteemidele** (sise API-d, failisüsteemid)

---

## Kuidas tööriistade kutsumine toimib

Tööriistade kutsumise voog koosneb neljast etapist:

| Etapp | Mis juhtub |
|-------|------------|
| **1. Määra tööriistad** | Kirjeldad saadaolevaid funktsioone JSON Schema abil — nimi, kirjeldus ja parameetrid |
| **2. Mudel otsustab** | Mudel saab sinu sõnumi koos tööriistade definitsioonidega. Kui tööriist oleks abiks, tagastab ta `tool_calls` vastuse tekstivastuse asemel |
| **3. Käita kohapeal** | Sinu kood analüüsib tööriista kutse, käivitab funktsiooni ja kogub tulemuse |
| **4. Lõplik vastus** | Saadad tööriista tulemuse tagasi mudelile, kes genereerib lõpliku vastuse |

> **Võtmekoht:** Mudel ei käivita kunagi koodi. Ta ainult *taotleb*, et tööriist kutsutakse. Sinu rakendus otsustab, kas taotlusele vastu tulla — sellega jääd sa täies ulatuses kontrolli alla.

---

## Millised mudelid toetavad tööriistade kutsumist?

Kõik mudelid ei toeta tööriistade kutsumist. Praeguses Foundry Local kataloogis on järgmised mudelid tööriistade kutsumise võimega:

| Mudel            | Suurus  | Tööriistade kutsumine |
|------------------|---------|:---------------------:|
| qwen2.5-0.5b     | 822 MB  | ✅                    |
| qwen2.5-1.5b     | 1.8 GB  | ✅                    |
| qwen2.5-7b       | 6.3 GB  | ✅                    |
| qwen2.5-14b      | 11.3 GB | ✅                    |
| qwen2.5-coder-0.5b | 822 MB  | ✅                    |
| qwen2.5-coder-1.5b | 1.8 GB  | ✅                    |
| qwen2.5-coder-7b | 6.3 GB  | ✅                    |
| qwen2.5-coder-14b | 11.3 GB | ✅                    |
| phi-4-mini       | 4.6 GB  | ✅                    |
| phi-3.5-mini     | 2.6 GB  | ❌                    |
| phi-4            | 10.4 GB | ❌                    |

> **Vihje:** Selle töötoa jaoks kasutame **qwen2.5-0.5b** — see on väike (822 MB allalaadimist), kiire ja usaldusväärse tööriistade kutsumisega.

---

## Õpieesmärgid

Selle töötoa lõpuks oskad:

- Selgitada tööriistade kutsumise mustrit ja miks see on oluline tehisintellekti agentidele
- Määratleda tööriistade skeemid OpenAI funktsioonide kutsumise formaadis
- Haldada mitme ringiga tööriistade kutsumise vestlusvoogu
- Käivitada tööriistade kutsed kohapeal ja tagastada tulemused mudelile
- Valida sobiv mudel tööriistade kutsumise stsenaariumite jaoks

---

## Eeltingimused

| Nõue                 | Detailid                                      |
|----------------------|----------------------------------------------|
| **Foundry Local CLI** | Paigaldatud ja `PATH`-is ([Osa 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Paigaldatud Python, JavaScript või C# SDK ([Osa 2](part2-foundry-local-sdk.md)) |
| **Tööriistade kutsumise mudel** | qwen2.5-0.5b (laetakse automaatselt alla) |

---

## Harjutused

### Harjutus 1 — Saada aru tööriistade kutsumise voost

Enne koodi kirjutamist vaata seda järjestuse diagrammi:

![Tööriistade kutsumise järjestuse diagramm](../../../images/tool-calling-sequence.svg)

**Peamised tähelepanekud:**

1. Sa määratled tööriistad ette JSON Schema objektidena
2. Mudeli vastus sisaldab `tool_calls` mitte tavalist sisu
3. Igal tööriistakutsel on unikaalne `id`, mida pead kasutama tulemuste tagastamisel
4. Mudel näeb kõiki eelnevaid sõnumeid *pluss* tööriista tulemusi lõpliku vastuse genereerimisel
5. Ühes vastuses võib olla mitu tööriistakutset

> **Arutelu:** Miks mudel tagastab tööriistakutsed selle asemel, et funktsioone otse käivitada? Milliseid turvaeeliseid see pakub?

---

### Harjutus 2 — Tööriistade skeemide määratlemine

Tööriistad on määratletud OpenAI funktsioonide kutsumise standardformaadis. Iga tööriista jaoks on vaja:

- **`type`**: Alati `"function"`
- **`function.name`**: Kirjeldav funktsiooni nimi (nt `get_weather`)
- **`function.description`**: Selge kirjeldus — mudel kasutab seda otsustamiseks, millal tööriista kutsuda
- **`function.parameters`**: JSON Schema objekt, mis kirjeldab oodatud argumendid

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

> **Parimad tavad tööriistade kirjeldamiseks:**
> - Ole täpne: "Hangi antud linna hetkeilm" on parem kui "Hangi ilm"
> - Kirjelda parameetreid selgelt: mudel loeb neid kirjeldusi, et täita õiged väärtused
> - Märgi kohustuslikud ja valikulised parameetrid — see aitab mudelil otsustada, mida küsida

---

### Harjutus 3 — Käivita tööriistakutsude näited

Iga näidiskood määratleb kaks tööriista (`get_weather` ja `get_population`), saadab küsimuse, mis kutsub tööriista kasutusele, käivitab tööriista kohapeal ja saadab tulemuse tagasi lõpliku vastuse jaoks.

<details>
<summary><strong>🐍 Python</strong></summary>

**Eeltingimused:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Käivita:**
```bash
python foundry-local-tool-calling.py
```

**Oodatav väljund:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Koodi läbivaatus** (`python/foundry-local-tool-calling.py`):

```python
# Määra tööriistad funktsiooniskeemide loendina
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

# Saada koos tööriistadega — mudel võib vastuseks saata tool_calls asemel sisu
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Kontrolli, kas mudel soovib tööriista kutsuda
if response.choices[0].message.tool_calls:
    # Käivita tööriist ja saada tulemus tagasi
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Eeltingimused:**
```bash
cd javascript
npm install
```

**Käivita:**
```bash
node foundry-local-tool-calling.mjs
```

**Oodatav väljund:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Koodi läbivaatus** (`javascript/foundry-local-tool-calling.mjs`):

See näide kasutab native Foundry Local SDK `ChatClient`-i OpenAI SDK asemel, demonstreerides mugavat `createChatClient()` meetodit:

```javascript
// Saa ChatClient otse mudeli objektist
const chatClient = model.createChatClient();

// Saada tööriistadega — ChatClient käsitleb OpenAI-ga ühilduvat formaati
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Kontrolli tööriistakõnesid
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Käivita tööriistad ja saada tulemused tagasi
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Eeltingimused:**
```bash
cd csharp
dotnet restore
```

**Käivita:**
```bash
dotnet run toolcall
```

**Oodatav väljund:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Koodi läbivaatus** (`csharp/ToolCalling.cs`):

C# kasutab tööriistade määratlemiseks abimeest `ChatTool.CreateFunctionTool`:

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

### Harjutus 4 — Tööriistade kutsumise vestlusvoog

Sõnumistruktuuri mõistmine on kriitiline. Siin on täielik voog, mis näitab `messages` massiivi igas etapis:

**Etapp 1 — Algne päring:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Etapp 2 — Mudel vastab `tool_calls`-ga (mitte sisuga):**
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

**Etapp 3 — Sa lisad assistendi sõnumi JA tööriista tulemuse:**
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

**Etapp 4 — Mudel genereerib lõpliku vastuse tööriista tulemuse põhjal.**

> **Tähtis:** `tool_call_id` tööriista sõnumis peab vastama tööriistakutse `id`-le. Nii seob mudel tulemused vastustega.

---

### Harjutus 5 — Mitme tööriista kutsumine

Mudel võib nõuda mitme tööriistakutse tegemist ühes vastuses. Proovi muuta kasutaja sõnumit, et käivitada mitu kutset:

```python
# Pythoni keeles — muuda kasutaja sõnumit:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// JavaScriptis — muuda kasutaja sõnumit:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Mudel peaks tagastama kaks `tool_calls` — ühe `get_weather` ja ühe `get_population` jaoks. Sinu kood juba haldab seda, sest see läbib kõiki kutsesid.

> **Proovi:** Muuda kasutaja sõnumit ja jookse näide uuesti. Kas mudel kutsub mõlemad tööriistad?

---

### Harjutus 6 — Lisa oma tööriist

Laienda ühte näitest uue tööriistaga. Näiteks lisa `get_time` tööriist:

1. Määra tööriista skeem:
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

2. Lisa täitmislõik:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Tegeliku rakenduse puhul kasuta ajavööndi teeki
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... olemasolevad tööriistad ...
```

3. Lisa tööriist `tools` massiivi ja testi küsimusega: "Mis kell on Tokyos?"

> **Väljakutse:** Lisa tööriist, mis teeb arvutuse, näiteks `convert_temperature`, mis teisendab Celsiuse ja Fahrenheiti vahel. Testi sellega: "Teisenda 100°F Celsiuseks."

---

### Harjutus 7 — Tööriistade kutsumine SDK ChatClientiga (JavaScript)

JavaScripti näide kasutab juba SDK natiivset `ChatClient`-i OpenAI SDK asemel. See on mugavusfunktsioon, mis eemaldab vajaduse ise OpenAI klienti konstrueerida:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient luuakse otse mudeliobjektist
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat võtab tööriistad vastu teise argumendina
const response = await chatClient.completeChat(messages, tools);
```

Võrdle seda Python lähenemisega, mis kasutab OpenAI SDK-d ekspliciitselt:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Mõlemad mustrid on kehtivad. `ChatClient` on mugavam; OpenAI SDK annab juurdepääsu täispikale OpenAI parameetrite valikule.

> **Proovi:** Muuda JavaScripti näidet nii, et see kasutab OpenAI SDK-d `ChatClient` asemel. Sul on vaja `import OpenAI from "openai"` ja klient konstrueerida `manager.urls[0]` kaudu.

---

### Harjutus 8 — Tööriista valiku mõistmine (`tool_choice`)

`tool_choice` parameeter kontrollib, kas mudel *peab* tööriista kasutama või saab vabalt valida:

| Väärtus | Käitumine |
|---------|-----------|
| `"auto"` | Mudel otsustab, kas tööriist kutstakse (vaikimisi) |
| `"none"` | Mudel ei kutsu ühtegi tööriista, ka kui need on olemas |
| `"required"` | Mudel peab kutsuma vähemalt ühe tööriista |
| `{"type": "function", "function": {"name": "get_weather"}}` | Mudel peab kutsuma määratletud tööriista |

Proovi kõiki valikuid Pythoni näites:

```python
# Sunni mudelit kutsuma get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Märkus:** Mitte kõik `tool_choice` valikud ei pruugi olla kõigi mudelite poolt toetatud. Kui mudel ei toeta `"required"` valikut, võib ta ignoreerida seda ja käituda nagu `"auto"`.

---

## Sageli esinevad lõksud

| Probleem                | Lahendus                                           |
|-------------------------|---------------------------------------------------|
| Mudel ei kutsu kunagi tööriistu | Veendu, et kasutad tööriistade kutsumise mudelit (nt qwen2.5-0.5b). Vaata üle tabel ülal. |
| `tool_call_id` väärtus ei kattu | Kasuta alati `id`-d, mis tuleb tööriistakutse vastusest, mitte koodi sisse kirjutatud väärtust |
| Mudel tagastab vigase JSON `arguments`-is | Väiksemad mudelid võivad mõnikord toota kehtetut JSON-i. Ümbritse `JSON.parse()` katse/ploki (`try/catch`) sisse |
| Mudel kutsub tööriista, mis ei eksisteeri | Lisa vaikehaldaja oma `execute_tool` funktsiooni |
| Lõputu tööriistade kutsumise silmus | Sea maksimum ringide arv (nt 5), et vältida lõputuid silmuseid |

---

## Peamised järeldused

1. **Tööriistade kutsumine** võimaldab mudelitel taotleda funktsioonide käivitamist, mitte arvata vastuseid
2. Mudel **ei käivita kunagi koodi**; sinu rakendus otsustab, mida käivitada
3. Tööriistad on määratletud kui **JSON Schema** objektid, mis järgivad OpenAI funktsioonide kutsumise formaati
4. Vestlus toimub **mitme ringiga mustri** järgi: kasutaja, siis assistent (tool_calls), siis tööriist (tulemused), siis assistent (lõplik vastus)
5. Kasuta alati **mudelit, mis toetab tööriistade kutsumist** (Qwen 2.5, Phi-4-mini)
6. SDK `createChatClient()` pakub mugava viisi tööriistakutsete tegemiseks ilma OpenAI klienti ise konstrueerimata

---

Jätka [Osa 12: Veebiliidese loomine Zava Creative Writerile](part12-zava-ui.md), et lisada brauseripõhine kasutajaliides mitme agenti torujuhtmele reaalajas voogedastusega.

---

[← Osa 10: Kohandatud mudelid](part10-custom-models.md) | [Osa 12: Zava Writer UI →](part12-zava-ui.md)