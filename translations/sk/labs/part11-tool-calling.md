![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Časť 11: Volanie nástrojov s lokálnymi modelmi

> **Cieľ:** Umožniť vášmu lokálnemu modelu volať externé funkcie (nástroje), aby mohol získavať aktuálne dáta, vykonávať výpočty alebo komunikovať s API — všetko bežiace súkromne na vašom zariadení.

## Čo je volanie nástrojov?

Volanie nástrojov (tiež známe ako **volanie funkcií**) umožňuje jazykovému modelu požiadať o vykonanie funkcií, ktoré definujete. Namiesto hádania odpovede model rozpozná, kedy by pomohol nástroj, a vráti štruktúrovanú požiadavku na vykonanie vašim kódom. Vaša aplikácia spustí funkciu, pošle výsledok späť a model túto informáciu zahrnie do svojej konečnej odpovede.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Tento vzor je nevyhnutný pre budovanie agentov, ktorí môžu:

- **Vyhľadávať živé dáta** (počasie, ceny akcií, databázové dotazy)
- **Vykonávať presné výpočty** (matematika, prevody jednotiek)
- **Podniknúť akcie** (odoslať emaily, vytvoriť tikety, aktualizovať záznamy)
- **Prístup k súkromným systémom** (interné API, súborové systémy)

---

## Ako funguje volanie nástrojov

Priebeh volania nástrojov má štyri fázy:

| Fáza | Čo sa deje |
|-------|-------------|
| **1. Definovať nástroje** | Popíšete dostupné funkcie pomocou JSON Schémy — meno, popis a parametre |
| **2. Rozhodnutie modelu** | Model dostane vašu správu aj definície nástrojov. Ak by pomohol nástroj, vráti odpoveď `tool_calls` namiesto textu |
| **3. Lokálne vykonanie** | Váš kód spracuje volanie nástroja, spustí funkciu a zozbiera výsledok |
| **4. Konečná odpoveď** | Pošlete výsledok nástroja späť modelu, ktorý vytvorí konečnú odpoveď |

> **Kľúčový bod:** Model nikdy nevykonáva kód. Iba *požiada* o zavolanie nástroja. Vaša aplikácia rozhoduje, či túto požiadavku splní — čím máte plnú kontrolu.

---

## Ktoré modely podporujú volanie nástrojov?

Nie každý model podporuje volanie nástrojov. V aktuálnom katalógu Foundry Local majú schopnosť volať nástroje tieto modely:

| Model | Veľkosť | Volanie nástrojov |
|-------|---------|:-----------------:|
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

> **Tip:** Pre tento kurz používame **qwen2.5-0.5b** — je malý (822 MB na stiahnutie), rýchly a spoľahlivo podporuje volanie nástrojov.

---

## Ciele učenia

Na konci tohto kurzu budete vedieť:

- Vysvetliť vzor volania nástrojov a prečo je dôležitý pre AI agentov
- Definovať schémy nástrojov použitím formátu OpenAI na volanie funkcií
- Spracovať multi-turn priebeh konverzácie volania nástrojov
- Lokálne vykonávať volania nástrojov a vracať výsledky modelu
- Vybrať správny model pre scenáre volania nástrojov

---

## Požiadavky

| Požiadavka | Detail |
|-------------|---------|
| **Foundry Local CLI** | Nainštalovaný a v `PATH` ([Časť 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript alebo C# SDK nainštalované ([Časť 2](part2-foundry-local-sdk.md)) |
| **Model podporujúci volanie nástrojov** | qwen2.5-0.5b (bude stiahnutý automaticky) |

---

## Cvičenia

### Cvičenie 1 — Pochopenie priebehu volania nástrojov

Pred písaním kódu si preštudujte tento sekvenčný diagram:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Kľúčové pozorovania:**

1. Nástroje definujete dopredu ako objekty JSON Schema
2. Odpoveď modelu obsahuje `tool_calls` namiesto bežného obsahu
3. Každé volanie nástroja má jedinečné `id`, ktoré musíte pri vracaní výsledkov použiť
4. Model vidí všetky predchádzajúce správy *plus* výsledky nástrojov pri generovaní konečnej odpovede
5. V jednej odpovedi môže byť viacero volaní nástrojov

> **Diskusia:** Prečo model vracia volania nástrojov namiesto priameho vykonania funkcií? Aké bezpečnostné výhody to poskytuje?

---

### Cvičenie 2 — Definovanie schém nástrojov

Nástroje sa definujú pomocou štandardného formátu OpenAI na volanie funkcií. Každý nástroj potrebuje:

- **`type`**: Vždy `"function"`
- **`function.name`**: Popisné meno funkcie (napr. `get_weather`)
- **`function.description`**: Jasný popis — model to použije na rozhodnutie, kedy volanie uskutočniť
- **`function.parameters`**: Objekt JSON Schema, ktorý popisuje očakávané argumenty

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

> **Odporúčania pre popisy nástrojov:**
> - Buďte konkrétny: „Získaj aktuálne počasie pre dané mesto“ je lepšie než „Získaj počasie“
> - Jasne popíšte parametre: model číta tieto popisy a vyplní správne hodnoty
> - Označte povinné a nepovinné parametre — pomáha to modelu rozhodnúť, čo požadovať

---

### Cvičenie 3 — Spustenie príkladov volania nástrojov

Každý jazykový príklad definuje dva nástroje (`get_weather` a `get_population`), pošle otázku, ktorá spustí použitie nástroja, lokálne vykoná nástroj a výsledok pošle späť pre konečnú odpoveď.

<details>
<summary><strong>🐍 Python</strong></summary>

**Požiadavky:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Spustenie:**
```bash
python foundry-local-tool-calling.py
```

**Očakávaný výstup:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Prechod kódom** (`python/foundry-local-tool-calling.py`):

```python
# Definujte nástroje ako zoznam schém funkcií
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

# Poslať s nástrojmi — model môže namiesto obsahu vrátiť volania nástrojov
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Skontrolujte, či model chce volať nástroj
if response.choices[0].message.tool_calls:
    # Vykonajte nástroj a pošlite výsledok naspäť
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Požiadavky:**
```bash
cd javascript
npm install
```

**Spustenie:**
```bash
node foundry-local-tool-calling.mjs
```

**Očakávaný výstup:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Prechod kódom** (`javascript/foundry-local-tool-calling.mjs`):

Tento príklad používa natívny `ChatClient` Foundry Local SDK namiesto OpenAI SDK, čo demonštruje pohodlnú metódu `createChatClient()`:

```javascript
// Získajte ChatClient priamo z objektu modelu
const chatClient = model.createChatClient();

// Poslať pomocou nástrojov — ChatClient spracováva formát kompatibilný s OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Skontrolujte volania nástrojov
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Spustite nástroje a odošlite späť výsledky
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Požiadavky:**
```bash
cd csharp
dotnet restore
```

**Spustenie:**
```bash
dotnet run toolcall
```

**Očakávaný výstup:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Prechod kódom** (`csharp/ToolCalling.cs`):

C# používa pomocnú metódu `ChatTool.CreateFunctionTool` na definovanie nástrojov:

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

### Cvičenie 4 — Priebeh konverzácie volania nástrojov

Pochopenie štruktúry správ je kritické. Tu je kompletný priebeh, ktorý ukazuje pole `messages` v každej fáze:

**Fáza 1 — Počiatočná požiadavka:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Fáza 2 — Model odpovedá `tool_calls` (nie obsah):**
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

**Fáza 3 — Pridáte správy asistenta A NÁSTROJ výsledok:**
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

**Fáza 4 — Model vytvorí konečnú odpoveď použitím výsledku nástroja.**

> **Dôležité:** `tool_call_id` v správe nástroja musí zodpovedať `id` z volania nástroja. Tak model spája výsledky s požiadavkami.

---

### Cvičenie 5 — Viacnásobné volania nástrojov

Model môže v jednej odpovedi požiadať o niekoľko volaní nástrojov. Skúste upraviť používateľskú správu tak, aby spustila viac volaní:

```python
# V Pythone — zmeň používateľskú správu:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// V JavaScripte — zmeň správu používateľa:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Model by mal vrátiť dve `tool_calls` — jedno pre `get_weather` a druhé pre `get_population`. Váš kód to už spracováva, pretože prechádza všetky volania.

> **Vyskúšajte:** Zmeňte správu používateľa a spustite príklad znova. Volá model oba nástroje?

---

### Cvičenie 6 — Pridajte vlastný nástroj

Rozšírte jeden z príkladov o nový nástroj. Napríklad pridajte nástroj `get_time`:

1. Definujte schému nástroja:
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

2. Pridajte logiku vykonania:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # V skutočnej aplikácii použite knižnicu na časové pásma
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... existujúce nástroje ...
```

3. Pridajte nástroj do poľa `tools` a otestujte otázkou: „Koľko je hodín v Tokiu?“

> **Výzva:** Pridajte nástroj, ktorý vykonáva výpočet, napríklad `convert_temperature` na prevod medzi Celziom a Fahrenheita. Otestujte: „Preveď 100°F na Celziove stupne.“

---

### Cvičenie 7 — Volanie nástrojov s ChatClient SDK (JavaScript)

JavaScriptový príklad už používa natívny `ChatClient` SDK namiesto OpenAI SDK. Táto funkcia uľahčuje volania bez nutnosti vytvárať OpenAI klienta:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient je vytvorený priamo z modelového objektu
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat prijíma nástroje ako druhý parameter
const response = await chatClient.completeChat(messages, tools);
```

Porovnajte to s Python prístupom, ktorý explicitne používa OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Oba vzory sú platné. `ChatClient` je pohodlnejší; OpenAI SDK ponúka prístup ku všetkým parametrom OpenAI.

> **Vyskúšajte:** Upravte JavaScriptový príklad tak, aby používal OpenAI SDK namiesto `ChatClient`. Budete potrebovať `import OpenAI from "openai"` a vytvoriť klienta s endpointom zo `manager.urls[0]`.

---

### Cvičenie 8 — Pochopenie parametra tool_choice

Parameter `tool_choice` riadi, či model *musí* použiť nástroj alebo má voľbu:

| Hodnota | Správanie |
|---------|-----------|
| `"auto"` | Model rozhodne, či nástroj zavolať (predvolené) |
| `"none"` | Model nevolá žiadne nástroje, aj keď sú k dispozícii |
| `"required"` | Model musí zavolať aspoň jeden nástroj |
| `{"type": "function", "function": {"name": "get_weather"}}` | Model musí zavolať zadaný nástroj |

Vyskúšajte každú možnosť v Pythone:

```python
# Núť model volať get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Poznámka:** Nie všetky hodnoty `tool_choice` môžu podporovať všetky modely. Ak model nepodporuje `"required"`, môže nastavenie ignorovať a správať sa ako `"auto"`.

---

## Časté problémy

| Problém | Riešenie |
|---------|----------|
| Model nikdy nevolá nástroje | Overte, či používate model podporujúci volanie nástrojov (napr. qwen2.5-0.5b). Pozrite tabuľku vyššie. |
| Neshoda `tool_call_id` | Vždy používajte `id` z odpovede volania nástroja, nie hardcodovanú hodnotu |
| Model vracia neplatný JSON v `arguments` | Menšie modely občas generujú neplatný JSON. Obalte `JSON.parse()` do try/catch |
| Model volá neexistujúci nástroj | Pridajte predvolený handler vo funkcii `execute_tool` |
| Nekonečná slučka volania nástrojov | Nastavte max počet kôl (napr. 5), aby ste zabránili nekonečným slučkám |

---

## Kľúčové poznatky

1. **Volanie nástrojov** umožňuje modelom žiadať o vykonanie funkcií namiesto hádania odpovedí
2. Model **nikdy nevykonáva kód**; vaša aplikácia rozhoduje, čo spustiť
3. Nástroje sa definujú ako objekty **JSON Schema** podľa formátu OpenAI volania funkcií
4. Konverzácia používa **multi-turn vzor**: používateľ, potom asistent (tool_calls), potom nástroj (výsledky), potom asistent (konečná odpoveď)
5. Vždy používajte **model s podporou volania nástrojov** (Qwen 2.5, Phi-4-mini)
6. SDK metóda `createChatClient()` poskytuje pohodlný spôsob, ako robiť volania nástrojov bez potreby vytvárať OpenAI klienta

---

Pokračujte na [Časť 12: Budovanie webového UI pre Zava Creative Writer](part12-zava-ui.md) a pridajte prehliadačové rozhranie k multi-agentnému pipeline s prúdením v reálnom čase.

---

[← Časť 10: Vlastné modely](part10-custom-models.md) | [Časť 12: Zava Writer UI →](part12-zava-ui.md)