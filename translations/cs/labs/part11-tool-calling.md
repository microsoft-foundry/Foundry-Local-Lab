![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Část 11: Volání nástrojů s místními modely

> **Cíl:** Umožnit vašemu místnímu modelu volat externí funkce (nástroje), aby mohl získávat aktuální data, provádět výpočty nebo komunikovat s API — vše privátně běžící na vašem zařízení.

## Co je volání nástrojů?

Volání nástrojů (také známé jako **volání funkcí**) umožňuje jazykovému modelu požádat o provedení funkcí, které definujete. Místo toho, aby model hádal odpověď, rozpozná, kdy by nástroj pomohl, a vrátí strukturovaný požadavek ke spuštění vašeho kódu. Vaše aplikace spustí funkci, pošle výsledek zpět a model tuto informaci zapracuje do své konečné odpovědi.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Tento vzor je zásadní pro vytváření agentů, kteří mohou:

- **Zjišťovat živá data** (počasí, ceny akcií, dotazy do databází)
- **Provádět přesné výpočty** (matematika, převody jednotek)
- **Provádět akce** (odesílat e-maily, vytvářet tikety, aktualizovat záznamy)
- **Přistupovat k soukromým systémům** (interní API, souborové systémy)

---

## Jak volání nástrojů funguje

Proces volání nástrojů má čtyři fáze:

| Fáze | Co se děje |
|-------|-----------|
| **1. Definice nástrojů** | Popíšete dostupné funkce pomocí JSON Schématu — název, popis a parametry |
| **2. Rozhodnutí modelu** | Model dostane vaši zprávu plus definice nástrojů. Pokud by nástroj pomohl, vrátí místo textové odpovědi `tool_calls` |
| **3. Lokální spuštění** | Váš kód analyzuje volání nástroje, spustí funkci a sebere výsledek |
| **4. Konečná odpověď** | Posíláte výsledek nástroje zpět modelu, který vytváří konečnou odpověď |

> **Klíčové:** Model nikdy nevykonává kód. Pouze *požaduje* volání nástroje. Vaše aplikace rozhoduje, zda tento požadavek splní — tím máte plnou kontrolu.

---

## Které modely podporují volání nástrojů?

Ne všechny modely podporují volání nástrojů. V současném katalogu Foundry Local mají možnost volání nástrojů tyto modely:

| Model | Velikost | Volání nástrojů |
|-------|----------|:---------------:|
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

> **Tip:** Pro tento lab použijeme **qwen2.5-0.5b** — je malý (822 MB ke stažení), rychlý a spolehlivě podporuje volání nástrojů.

---

## Výukové cíle

Na konci tohoto labu budete schopni:

- Vysvětlit vzor volání nástrojů a proč je důležitý pro AI agenty
- Definovat schémata nástrojů pomocí formátu OpenAI pro volání funkcí
- Zpracovat vícekolový konverzační tok volání nástrojů
- Lokálně provádět volání nástrojů a vracet výsledky modelu
- Vybrat správný model pro scénáře volání nástrojů

---

## Požadavky

| Požadavek | Detaily |
|-----------|---------|
| **Foundry Local CLI** | Nainstalováno a v `PATH` ([Část 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Nainstalovaný Python, JavaScript nebo C# SDK ([Část 2](part2-foundry-local-sdk.md)) |
| **Model podporující volání nástrojů** | qwen2.5-0.5b (bude staženo automaticky) |

---

## Cvičení

### Cvičení 1 — Pochopit tok volání nástrojů

Před psaním kódu si prostudujte tento sekvenční diagram:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Klíčové poznatky:**

1. Nástroje definujete dopředu jako JSON Schema objekty
2. Odpověď modelu obsahuje `tool_calls` místo běžného obsahu
3. Každé volání nástroje má jedinečné `id`, na které musíte odkázat při vracení výsledků
4. Model vidí všechny předchozí zprávy *plus* výsledky nástrojů při generování konečné odpovědi
5. Ve jedné odpovědi může být více volání nástrojů

> **Diskuze:** Proč model vrací volání nástrojů místo přímého spuštění funkcí? Jaké bezpečnostní výhody to poskytuje?

---

### Cvičení 2 — Definování schémat nástrojů

Nástroje se definují pomocí standardního formátu OpenAI pro volání funkcí. Každý nástroj potřebuje:

- **`type`**: Vždy `"function"`
- **`function.name`**: Popisný název funkce (např. `get_weather`)
- **`function.description`**: Jasný popis — model ho používá k rozhodnutí, kdy volat nástroj
- **`function.parameters`**: JSON Schema objekt popisující očekávané argumenty

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

> **Doporučené postupy pro popisy nástrojů:**
> - Buďte konkrétní: "Získat aktuální počasí pro dané město" je lepší než "Získat počasí"
> - Jasně popište parametry: model čte tyto popisy, aby vyplnil správné hodnoty
> - Označte povinné a nepovinné parametry — pomáhá to modelu rozhodnout, co se ptát

---

### Cvičení 3 — Spuštění příkladů volání nástrojů

Každý příklad jazyka definuje dva nástroje (`get_weather` a `get_population`), odešle otázku vyvolávající použití nástroje, spustí nástroj lokálně a pošle výsledek zpět pro konečnou odpověď.

<details>
<summary><strong>🐍 Python</strong></summary>

**Požadavky:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Spuštění:**
```bash
python foundry-local-tool-calling.py
```

**Očekávaný výstup:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Komentář ke kódu** (`python/foundry-local-tool-calling.py`):

```python
# Definujte nástroje jako seznam schémat funkcí
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

# Odeslat s nástroji — model může vrátit tool_calls místo obsahu
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Zkontrolujte, zda model chce zavolat nástroj
if response.choices[0].message.tool_calls:
    # Proveďte nástroj a zašlete výsledek zpět
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Požadavky:**
```bash
cd javascript
npm install
```

**Spuštění:**
```bash
node foundry-local-tool-calling.mjs
```

**Očekávaný výstup:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Komentář ke kódu** (`javascript/foundry-local-tool-calling.mjs`):

Tento příklad používá nativní Foundry Local SDK `ChatClient` místo OpenAI SDK, což ukazuje pohodlnou metodu `createChatClient()`:

```javascript
// Získejte ChatClient přímo z objektu modelu
const chatClient = model.createChatClient();

// Odeslat pomocí nástrojů — ChatClient zpracovává formát kompatibilní s OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Zkontrolujte volání nástrojů
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Spusťte nástroje a odešlete výsledky zpět
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Požadavky:**
```bash
cd csharp
dotnet restore
```

**Spuštění:**
```bash
dotnet run toolcall
```

**Očekávaný výstup:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Komentář ke kódu** (`csharp/ToolCalling.cs`):

C# používá pomocnou metodu `ChatTool.CreateFunctionTool` pro definici nástrojů:

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

### Cvičení 4 — Tok konverzace volání nástrojů

Pochopení struktury zpráv je kritické. Zde je úplný tok, ukazující pole `messages` v každé fázi:

**Fáze 1 — Počáteční požadavek:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Fáze 2 — Model odpovídá `tool_calls` (nikoliv obsahem):**
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

**Fáze 3 — Přidáte zprávu od asistenta A výsledek nástroje:**
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

**Fáze 4 — Model vytvoří konečnou odpověď s využitím výsledku nástroje.**

> **Důležité:** `tool_call_id` ve zprávě nástroje musí odpovídat `id` z volání nástroje. Model spojuje výsledky s požadavky tímto způsobem.

---

### Cvičení 5 — Více volání nástrojů

Model může požádat o několik volání nástrojů v jedné odpovědi. Zkuste změnit uživatelskou zprávu tak, aby vyvolala více volání:

```python
# V Pythonu — změňte uživatelskou zprávu:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// V JavaScriptu — změňte uživatelskou zprávu:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Model by měl vrátit dvě `tool_calls` — jednu pro `get_weather` a jednu pro `get_population`. Váš kód to již zvládá, protože prochází všechny volání nástrojů.

> **Vyzkoušejte:** Upravte uživatelskou zprávu a spusťte příklad znovu. Volá model oba nástroje?

---

### Cvičení 6 — Přidejte si vlastní nástroj

Rozšiřte jeden z příkladů o nový nástroj. Například přidejte nástroj `get_time`:

1. Definujte schéma nástroje:
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

2. Přidejte logiku vykonání:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # V reálné aplikaci použijte knihovnu pro časová pásma
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... existující nástroje ...
```

3. Přidejte nástroj do pole `tools` a otestujte otázkou: "Kolik je hodin v Tokyu?"

> **Výzva:** Přidejte nástroj, který provede výpočet, například `convert_temperature` pro převod mezi Celsiem a Fahrenheitem. Otestujte s: "Převést 100°F na Celsia."

---

### Cvičení 7 — Volání nástrojů s ChatClientem SDK (JavaScript)

JavaScriptový příklad už používá nativní `ChatClient` SDK místo OpenAI SDK. Jedná se o pohodlnou funkci, která eliminuje potřebu ručně vytvářet OpenAI klienta:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient je vytvořen přímo z modelu objektu
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat přijímá nástroje jako druhý parametr
const response = await chatClient.completeChat(messages, tools);
```

Porovnejte to s přístupem v Pythonu, který explicitně využívá OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Oba vzory jsou platné. `ChatClient` je pohodlnější; OpenAI SDK dává přístup ke všem parametrům OpenAI.

> **Vyzkoušejte:** Upravte JavaScriptový příklad tak, aby místo `ChatClient` používal OpenAI SDK. Budete potřebovat `import OpenAI from "openai"` a vytvořit klienta s endpointem z `manager.urls[0]`.

---

### Cvičení 8 — Porozumění parametru tool_choice

Parametr `tool_choice` řídí, zda model *musí* použít nástroj, nebo může volit:

| Hodnota | Chování |
|---------|---------|
| `"auto"` | Model rozhodne, zda vyvolat nástroj (výchozí) |
| `"none"` | Model nebude volat žádné nástroje, ani pokud jsou poskytnuty |
| `"required"` | Model musí zavolat alespoň jeden nástroj |
| `{"type": "function", "function": {"name": "get_weather"}}` | Model musí zavolat specifikovaný nástroj |

Vyzkoušejte každou možnost v pythonovém příkladu:

```python
# Přinutit model zavolat get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Poznámka:** Ne všechny možnosti `tool_choice` jsou podporovány každým modelem. Pokud model nepodporuje `"required"`, může toto nastavení ignorovat a chovat se jako `"auto"`.

---

## Běžné problémy

| Problém | Řešení |
|---------|---------|
| Model nikdy nevolá nástroje | Zkontrolujte, že používáte model podporující volání nástrojů (např. qwen2.5-0.5b). Podívejte se na tabulku výše. |
| Neshoda `tool_call_id` | Vždy používejte `id` z odpovědi volání nástroje, ne pevně zakódovanou hodnotu |
| Model vrací špatně formátovaný JSON v `arguments` | Menší modely občas generují neplatný JSON. Obalte `JSON.parse()` do try/catch bloku |
| Model volá neexistující nástroj | Přidejte výchozí handler ve vaší funkci `execute_tool` |
| Nekonečná smyčka volání nástrojů | Nastavte maximální počet kol (např. 5), aby se zabránilo nekonečnému cyklu |

---

## Klíčové poznatky

1. **Volání nástrojů** umožňuje modelům požadovat vykonání funkcí místo odhadování odpovědí
2. Model **nikdy nevykonává kód**; rozhodnutí, co spustit, patří vaší aplikaci
3. Nástroje jsou definovány jako **JSON Schema** objekty podle OpenAI formátu volání funkcí
4. Konverzace používá **vícekolový vzor**: uživatel, pak asistent (tool_calls), pak nástroj (výsledky), nakonec asistent (konečná odpověď)
5. Vždy používejte **model podporující volání nástrojů** (Qwen 2.5, Phi-4-mini)
6. SDK metoda `createChatClient()` poskytuje pohodlný způsob, jak posílat požadavky na volání nástrojů bez nutnosti stavět OpenAI klienta

---

Pokračujte do [Části 12: Vytvoření webového uživatelského rozhraní pro Zava Creative Writer](part12-zava-ui.md), kde přidáte frontend do multipopupového pipeline s reálným streamováním.

---

[← Část 10: Vlastní modely](part10-custom-models.md) | [Část 12: Zava Writer UI →](part12-zava-ui.md)