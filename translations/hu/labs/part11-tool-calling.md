![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 11. rész: Eszközhívás helyi modellekkel

> **Cél:** Engedélyezd, hogy a helyi modelljeid külső függvényeket (eszközöket) hívjanak meg, így valós idejű adatokat szerezhet be, számításokat végezhet, vagy API-kkal léphet kapcsolatba – mindez privát módon, a saját eszközödön futtatva.

## Mi az az eszközhívás?

Az eszközhívás (más néven **függvényhívás**) lehetővé teszi, hogy egy nyelvi modell a te általad definiált függvények futtatását kérje le. Ahelyett, hogy csak találgatna egy választ, a modell felismeri, mikor lenne egy eszköz hasznos, és strukturált kérést ad vissza, amelyet a te kódod végrehajt. Az alkalmazás futtatja a függvényt, visszaküldi az eredményt, és a modell azt beépíti a végső válaszába.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Ez a mintázat elengedhetetlen olyan ügynökök építéséhez, akik képesek:

- **Élő adatok lekérdezésére** (időjárás, részvényárfolyamok, adatbázis-lekérdezések)
- **Pontos számítások elvégzésére** (matematika, mértékegység átváltás)
- **Műveletek elvégzésére** (email küldése, jegyek létrehozása, rekordok frissítése)
- **Privát rendszerek elérésére** (belső API-k, fájlrendszerek)

---

## Hogyan működik az eszközhívás

Az eszközhívás folyamata négy szakaszból áll:

| Szakasz | Mi történik? |
|---------|--------------|
| **1. Eszközök definiálása** | Az elérhető függvények JSON Schema segítségével történő leírása — név, leírás és paraméterek |
| **2. Modell döntése** | A modell megkapja az üzenetedet és az eszközdefiníciókat. Ha egy eszköz hasznos lenne, `tool_calls` választ ad vissza a szöveges válasz helyett |
| **3. Helyi végrehajtás** | A kódod elemzi az eszközhívást, lefuttatja a függvényt, és összegyűjti az eredményt |
| **4. Végső válasz** | Az eszköz eredményét visszaküldöd a modellnek, amely elkészíti végleges válaszát |

> **Fontos:** A modell soha nem hajt végre kódot. Csak *kéri*, hogy egy eszközt hívjanak meg. Az alkalmazás dönt arról, hogy eleget tesz-e a kérésnek — így teljes kontrollod megmarad.

---

## Mely modellek támogatják az eszközhívást?

Nem minden modell támogatja az eszközhívást. A jelenlegi Foundry Local katalógusban a következő modellek képesek eszközhívásra:

| Modell | Méret | Eszközhívás |
|--------|-------|:-----------:|
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

> **Tipp:** Ehhez a laborhoz a **qwen2.5-0.5b** modellt használjuk — kicsi (822 MB letöltés), gyors, és megbízható eszközhívás támogatással rendelkezik.

---

## Tanulási célok

A labor végére képes leszel:

- Elmagyarázni az eszközhívás mintázatát és jelentőségét az AI ügynökök számára
- Meghatározni az eszköz sémákat az OpenAI függvényhívási formátum alapján
- Kezelni az eszközhívás többfordulós beszélgetési folyamatát
- Helyileg végrehajtani eszközhívásokat és visszaküldeni az eredményeket a modellnek
- Megfelelő modellt választani eszközhívó helyzetekhez

---

## Előfeltételek

| Követelmény | Részletek |
|-------------|-----------|
| **Foundry Local CLI** | Telepítve és a PATH-on ([11. rész](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript vagy C# SDK telepítve ([2. rész](part2-foundry-local-sdk.md)) |
| **Eszközhívó modell** | qwen2.5-0.5b (automatikusan letöltődik) |

---

## Gyakorlatok

### 1. gyakorlat — Értsd meg az eszközhívás folyamatát

Kód írása előtt tanulmányozd ezt a sorrendi diagramot:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Fő megfigyelések:**

1. Az eszközöket előre JSON Schema objektumokként definiálod
2. A modell válasza `tool_calls` elemeket tartalmaz a normál tartalom helyett
3. Minden eszközhívásnak egyedi `id`-ja van, amire hivatkoznod kell az eredmények visszaküldésekor
4. A modell látja az összes korábbi üzenetet *plusz* az eszközök eredményeit a végső válasz generálásakor
5. Egy válaszban több eszközhívás is lehet

> **Megbeszélés:** Miért ad vissza a modell eszközhívásokat ahelyett, hogy közvetlenül végrehajtana függvényeket? Milyen biztonsági előnyökkel jár ez?

---

### 2. gyakorlat — Eszközsémák meghatározása

Az eszközöket a szabványos OpenAI függvényhívási formátumban definiáljuk. Minden eszközhöz szükséges:

- **`type`**: Mindig `"function"`
- **`function.name`**: Egy leíró függvénynév (például `get_weather`)
- **`function.description`**: Egy világos leírás — ezt használja a modell hogy eldöntse, mikor hívja meg az eszközt
- **`function.parameters`**: Egy JSON Schema objektum, amely leírja a várt argumentumokat

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

> **Legjobb gyakorlatok az eszközleírásokhoz:**
> - Légy specifikus: "Szerezd meg az adott város aktuális időjárását" jobb, mint "Időjárás lekérése"
> - Paramétereket tisztán írd le: a modell ez alapján tölti ki a helyes értékeket
> - Jelöld, hogy mely paraméterek kötelezőek és melyek opcionálisak — ez segíti a modellt a kérdések feltevésében

---

### 3. gyakorlat — Futtasd az eszközhívás példákat

Minden nyelvi minta két eszközt határoz meg (`get_weather` és `get_population`), küld egy kérdést, amely aktiválja az eszközhasználatot, helyben futtatja az eszközt, majd visszaküldi az eredményt a végső válaszhoz.

<details>
<summary><strong>🐍 Python</strong></summary>

**Előfeltételek:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Futtatás:**
```bash
python foundry-local-tool-calling.py
```

**Várt kimenet:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kódbemutató** (`python/foundry-local-tool-calling.py`):

```python
# Definiálja az eszközöket egy függvény sémák listájaként
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

# Küldje el az eszközökkel együtt — a modell eszköz hívásokat (tool_calls) adhat vissza tartalom helyett
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Ellenőrizze, hogy a modell eszközt akar-e hívni
if response.choices[0].message.tool_calls:
    # Hajtsa végre az eszközt, és küldje vissza az eredményt
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Előfeltételek:**
```bash
cd javascript
npm install
```

**Futtatás:**
```bash
node foundry-local-tool-calling.mjs
```

**Várt kimenet:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kódbemutató** (`javascript/foundry-local-tool-calling.mjs`):

Ez a példa a natív Foundry Local SDK `ChatClient` osztályát használja az OpenAI SDK helyett, bemutatva a kényelmes `createChatClient()` metódust:

```javascript
// Közvetlenül a modell objektumból szerezze be a ChatClient-et
const chatClient = model.createChatClient();

// Küldés eszközökkel — a ChatClient kezeli az OpenAI-kompatibilis formátumot
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Ellenőrizze az eszközhívásokat
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Futtassa az eszközöket és küldje vissza az eredményeket
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Előfeltételek:**
```bash
cd csharp
dotnet restore
```

**Futtatás:**
```bash
dotnet run toolcall
```

**Várt kimenet:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kódbemutató** (`csharp/ToolCalling.cs`):

A C# az `ChatTool.CreateFunctionTool` segédfüggvényt használja az eszközök definiálására:

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

### 4. gyakorlat — Az eszközhívás beszélgetési folyamata

Az üzenetstruktúra megértése kulcsfontosságú. Itt a teljes folyamat, minden szakaszban látható a `messages` tömb:

**1. szakasz — Kezdeti kérés:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**2. szakasz — A modell eszközhívásokkal válaszol (nem tartalommal):**
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

**3. szakasz — Hozzáadod az asszisztens üzenetet ÉS az eszköz eredményét:**
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

**4. szakasz — A modell a végső választ készíti az eszköz eredmény felhasználásával.**

> **Fontos:** Az eszközüzenet `tool_call_id` mezőjének meg kell egyeznie az eszközhívás `id` mezőjével. Így köti össze a modell az eredményeket a kérésekkel.

---

### 5. gyakorlat — Több eszközhívás

Egy válaszban a modell több eszközhívást is kérhet. Próbáld meg módosítani a felhasználói üzenetet, hogy több hívást indítson:

```python
# Pythonban — változtasd meg a felhasználói üzenetet:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// JavaScript-ben — változtasd meg a felhasználói üzenetet:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

A modellnek két `tool_calls` elemet kell visszaadnia — egyet a `get_weather`, egyet a `get_population` eszközhöz. A kódod már kezeli ezt, mert végigmegy az összes eszközhíváson.

> **Próbáld ki:** Módosítsd a felhasználói üzenetet és futtasd újra a mintát. Meghívja mindkét eszközt a modell?

---

### 6. gyakorlat — Adj hozzá saját eszközt

Bővítsd az egyik mintát egy új eszközzel. Például adj hozzá egy `get_time` eszközt:

1. Definiáld az eszköz sémáját:
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

2. Add hozzá a végrehajtó logikát:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Egy valódi alkalmazásban használj időzóna könyvtárat
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... meglévő eszközök ...
```

3. Add hozzá az eszközt a `tools` tömbhöz és teszteld a kérdéssel: "Hány óra van Tokióban?"

> **Kihívás:** Adj hozzá egy olyan eszközt, amely számítást végez, például `convert_temperature`, ami Celsius és Fahrenheit között vált. Teszteld azzal: "Konvertáld a 100°F-ot Celsiusra."

---

### 7. gyakorlat — Eszközhívás az SDK ChatClient-jével (JavaScript)

A JavaScript minta már az SDK natív `ChatClient` osztályát használja az OpenAI SDK helyett. Ez egy kényelmi megoldás, amely megszünteti az OpenAI kliens kézi konstrukcióját:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// A ChatClient közvetlenül a modell objektumból jön létre
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// a completeChat második paraméterként eszközöket fogad el
const response = await chatClient.completeChat(messages, tools);
```

Ezt hasonlítsd össze a Python megközelítéssel, ahol explicit az OpenAI SDK-t használjuk:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Mindkét mintázat helyes. A `ChatClient` kényelmesebb; az OpenAI SDK a teljes paraméterkészlethez biztosít hozzáférést.

> **Próbáld ki:** Módosítsd a JavaScript mintát, hogy az OpenAI SDK-t használja `ChatClient` helyett. Szükséged lesz az `import OpenAI from "openai"` sorra, és a `manager.urls[0]` alapján létrehozott kliensre.

---

### 8. gyakorlat — A tool_choice paraméter megértése

A `tool_choice` paraméter szabályozza, hogy a modell *kell-e* eszközt használnia vagy szabadon dönthet:

| Érték | Viselkedés |
|-------|------------|
| `"auto"` | A modell eldönti, hív-e eszközt (alapértelmezett) |
| `"none"` | A modell egyáltalán nem hív eszközt, még ha elérhető is |
| `"required"` | A modellnek legalább egy eszközt meg kell hívnia |
| `{"type": "function", "function": {"name": "get_weather"}}` | A modellnek az adott eszközt kell hívnia |

Próbáld ki mindegyiket a Python mintában:

```python
# Kényszerítse a modellt a get_weather hívására
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Megjegyzés:** Nem minden modell támogat minden `tool_choice` beállítást. Ha az adott modell nem támogatja a `"required"` opciót, az figyelmen kívül hagyhatja azt és az `"auto"` viselkedést mutathatja.

---

## Gyakori buktatók

| Probléma | Megoldás |
|----------|----------|
| A modell soha nem hív eszközöket | Ellenőrizd, hogy eszközhívó modellt használsz (pl. qwen2.5-0.5b). Nézd meg a fenti táblázatot. |
| `tool_call_id` eltérés | Mindig az eszközhívás válaszban kapott `id`-t használd, ne hardkodolt értéket |
| A modell hibás JSON-t ad vissza `arguments`-ben | A kisebb modellek néha érvénytelen JSON-t generálnak. Csomagold a `JSON.parse()`-t try/catch-be |
| A modell ismeretlen eszközt hív | Adj a `execute_tool` függvényedhez alapértelmezett kezelőt |
| Végtelen eszközhívó ciklus | Állíts be maximális fordulószámot (pl. 5), hogy megakadályozd a végtelen ciklust |

---

## Fontos összefoglaló

1. Az **eszközhívás** lehetővé teszi, hogy a modellek függvényeket kérjenek a találgatás helyett
2. A modell **soha nem hajt végre kódot**; az alkalmazás dönt arról, mi fusson
3. Az eszközök **JSON Schema** objektumok a OpenAI függvényhívási formátumának megfelelően
4. A beszélgetés **többfordulós mintázat szerint** zajlik: felhasználó, asszisztens (eszközhívások), eszköz (eredmények), asszisztens (végső válasz)
5. Mindig olyan modellt válassz, amely támogatja az eszközhívást (Qwen 2.5, Phi-4-mini)
6. Az SDK `createChatClient()` metódusa kényelmes módot kínál eszközhívásokhoz anélkül, hogy külön OpenAI klienst kéne létrehozni

---

Folytasd a [12. rész: Webes UI építése a Zava Kreatív Íróhoz](part12-zava-ui.md), hogy böngésző alapú frontenddel egészítsd ki a többügynökös folyamatot valós idejű streaminggel.

---

[← 10. rész: Egyedi modellek](part10-custom-models.md) | [12. rész: Zava Writer UI →](part12-zava-ui.md)