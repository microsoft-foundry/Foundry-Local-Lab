![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Deel 11: Tool Calling met Lokale Modellen

> **Doel:** Maak het mogelijk voor je lokale model om externe functies (tools) aan te roepen zodat het real-time data kan ophalen, berekeningen kan uitvoeren of kan communiceren met API's — allemaal privé draaiend op jouw apparaat.

## Wat is Tool Calling?

Tool calling (ook wel **function calling**) laat een taalmodel functies uitvoeren die jij definieert. In plaats van een antwoord te raden herkent het model wanneer een tool zou helpen en retourneert het een gestructureerd verzoek om jouw code uit te voeren. Je applicatie voert de functie uit, stuurt het resultaat terug, en het model verwerkt die informatie in het definitieve antwoord.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Dit patroon is essentieel voor het bouwen van agents die kunnen:

- **Live data opzoeken** (weer, aandelenkoersen, database query's)
- **Nauwkeurige berekeningen uitvoeren** (wiskunde, eenheden omrekenen)
- **Acties ondernemen** (e-mails versturen, tickets aanmaken, records bijwerken)
- **Toegang krijgen tot privé systemen** (interne API's, bestandssystemen)

---

## Hoe werkt Tool Calling?

De tool-calling flow kent vier fasen:

| Fase | Wat gebeurt er |
|-------|--------------|
| **1. Definieer tools** | Je beschrijft beschikbare functies met JSON Schema — naam, beschrijving en parameters |
| **2. Model beslist** | Het model ontvangt je bericht plus de tool-definities. Als een tool helpt, retourneert het een `tool_calls` reactie in plaats van een tekstantwoord |
| **3. Voer lokaal uit** | Je code parseert de tool-aanroep, voert de functie uit en verzamelt het resultaat |
| **4. Definitief antwoord** | Je stuurt het tool-resultaat terug naar het model, dat het definitieve antwoord produceert |

> **Belangrijk:** Het model voert nooit code uit. Het *verzoekt alleen* dat een tool wordt aangeroepen. Jouw applicatie beslist of dat verzoek gehonoreerd wordt — zo houd je volledige controle.

---

## Welke Modellen Ondersteunen Tool Calling?

Niet elk model ondersteunt tool calling. In de huidige Foundry Local catalogus hebben de volgende modellen tool-calling mogelijkheden:

| Model | Grootte | Tool Calling |
|-------|---------|:-----------:|
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

> **Tip:** Voor deze lab gebruiken we **qwen2.5-0.5b** — het is klein (822 MB download), snel, en heeft betrouwbare tool-calling ondersteuning.

---

## Leerdoelen

Aan het eind van deze lab kun je:

- Het tool-calling patroon uitleggen en waarom het belangrijk is voor AI agents
- Toolschemas definiëren met het OpenAI function-calling formaat
- De multi-turn tool-calling gespreksflow afhandelen
- Tool-aanroepen lokaal uitvoeren en resultaten teruggeven aan het model
- Het juiste model kiezen voor tool-calling scenario's

---

## Vereisten

| Vereiste | Details |
|----------|---------|
| **Foundry Local CLI** | Geïnstalleerd en in je `PATH` ([Deel 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript, of C# SDK geïnstalleerd ([Deel 2](part2-foundry-local-sdk.md)) |
| **Een tool-calling model** | qwen2.5-0.5b (wordt automatisch gedownload) |

---

## Oefeningen

### Oefening 1 — Begrijp de Tool-Calling Flow

Bestudeer, voordat je code schrijft, dit sequentiediagram:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Belangrijke punten:**

1. Je definieert de tools vooraf als JSON Schema objecten
2. De respons van het model bevat `tool_calls` in plaats van gewone content
3. Elke tool-aanroep heeft een unieke `id` waar je naar moet verwijzen bij het terugsturen van resultaten
4. Het model ziet alle voorgaande berichten *plus* de toolresultaten bij het genereren van het definitieve antwoord
5. Meerdere tool-aanroepen kunnen in één respons voorkomen

> **Discussie:** Waarom geeft het model tool-aanroepen terug in plaats van functies direct uit te voeren? Welke veiligheidsvoordelen biedt dit?

---

### Oefening 2 — Toolschemas Definiëren

Tools worden gedefinieerd met het standaard OpenAI function-calling formaat. Elke tool heeft nodig:

- **`type`**: Altijd `"function"`
- **`function.name`**: Een beschrijvende functienaam (bijv. `get_weather`)
- **`function.description`**: Een duidelijke omschrijving — het model gebruikt dit om te bepalen wanneer de tool te gebruiken
- **`function.parameters`**: Een JSON Schema object dat de verwachte argumenten beschrijft

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

> **Best practices voor tool-beschrijvingen:**
> - Wees specifiek: "Haal het huidige weer op voor een bepaalde stad" is beter dan "Weer ophalen"
> - Beschrijf parameters helder: het model leest deze beschrijvingen om juiste waarden in te vullen
> - Markeer welke parameters verplicht zijn en welke optioneel — dit helpt het model bij vragen

---

### Oefening 3 — Voer de Tool-Calling Voorbeelden uit

Elke taalvoorbeelden definiëren twee tools (`get_weather` en `get_population`), sturen een vraag die gebruik van tools triggert, voeren de tool lokaal uit en sturen het resultaat terug voor een definitief antwoord.

<details>
<summary><strong>🐍 Python</strong></summary>

**Vereisten:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Uitvoeren:**
```bash
python foundry-local-tool-calling.py
```

**Verwachte output:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Code uitleg** (`python/foundry-local-tool-calling.py`):

```python
# Definieer tools als een lijst van functiemodellen
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

# Verzenden met tools — het model kan tool_calls in plaats van inhoud teruggeven
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Controleer of het model een tool wil aanroepen
if response.choices[0].message.tool_calls:
    # Voer de tool uit en stuur het resultaat terug
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Vereisten:**
```bash
cd javascript
npm install
```

**Uitvoeren:**
```bash
node foundry-local-tool-calling.mjs
```

**Verwachte output:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Code uitleg** (`javascript/foundry-local-tool-calling.mjs`):

Dit voorbeeld gebruikt de native Foundry Local SDK's `ChatClient` in plaats van de OpenAI SDK, wat de handige `createChatClient()` methode toont:

```javascript
// Haal een ChatClient direct uit het modelobject
const chatClient = model.createChatClient();

// Verstuur met tools — ChatClient verwerkt het OpenAI-compatibele formaat
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Controleer op tool-aanroepen
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Voer tools uit en stuur resultaten terug
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Vereisten:**
```bash
cd csharp
dotnet restore
```

**Uitvoeren:**
```bash
dotnet run toolcall
```

**Verwachte output:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Code uitleg** (`csharp/ToolCalling.cs`):

C# gebruikt de helper `ChatTool.CreateFunctionTool` om tools te definiëren:

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

### Oefening 4 — De Tool-Calling Gespreksflow

Begrip van de berichtstructuur is cruciaal. Hier is de volledige flow, met de `messages` array in elke fase:

**Fase 1 — Initiële aanvraag:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Fase 2 — Model reageert met tool_calls (geen content):**
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

**Fase 3 — Je voegt het assistant bericht EN het tool resultaat toe:**
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

**Fase 4 — Model genereert het definitieve antwoord met behulp van het tool resultaat.**

> **Belangrijk:** De `tool_call_id` in het toolbericht moet overeenkomen met de `id` van de tool-aanroep. Zo koppelt het model resultaten aan verzoeken.

---

### Oefening 5 — Meerdere Tool Calls

Een model kan meerdere tool-aanroepen in één antwoord doen. Probeer het gebruikerbericht aan te passen zodat meerdere aanroepen getriggerd worden:

```python
# In Python — wijzig het gebruikersbericht:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// In JavaScript — wijzig het gebruikersbericht:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Het model zou twee `tool_calls` moeten retourneren — één voor `get_weather` en één voor `get_population`. Je code verwerkt dit al omdat het door alle tool-aanroepen loopt.

> **Probeer:** Pas het gebruikerbericht aan en voer het voorbeeld opnieuw uit. Roept het model beide tools aan?

---

### Oefening 6 — Voeg je Eigen Tool toe

Breid een van de voorbeelden uit met een nieuwe tool. Bijvoorbeeld, voeg een `get_time` tool toe:

1. Definieer het tool schema:
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

2. Voeg de uitvoeringslogica toe:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Gebruik in een echte app een tijdzonebibliotheek
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... bestaande tools ...
```

3. Voeg de tool toe aan de `tools` array en test met: "Hoe laat is het in Tokyo?"

> **Uitdaging:** Voeg een tool toe die een berekening uitvoert, bijvoorbeeld `convert_temperature` die Celsius naar Fahrenheit omzet. Test met: "Zet 100°F om naar Celsius."

---

### Oefening 7 — Tool Calling met de SDK's ChatClient (JavaScript)

Het JavaScript voorbeeld gebruikt al de native SDK `ChatClient` in plaats van de OpenAI SDK. Dit is een handige functie die je de noodzaak bespaart een OpenAI client handmatig te construeren:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient wordt direct vanuit het modelobject aangemaakt
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat accepteert tools als tweede parameter
const response = await chatClient.completeChat(messages, tools);
```

Vergelijk dit met de Python aanpak die expliciet de OpenAI SDK gebruikt:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Beide patronen zijn geldig. De `ChatClient` is handiger; de OpenAI SDK geeft toegang tot het volledige scala aan OpenAI parameters.

> **Probeer:** Pas het JavaScript voorbeeld aan om de OpenAI SDK te gebruiken in plaats van `ChatClient`. Je hebt dan `import OpenAI from "openai"` nodig en maakt de client met de endpoint van `manager.urls[0]`.

---

### Oefening 8 — Tool_choice Begrijpen

De parameter `tool_choice` geeft aan of het model *moet* kiezen voor een tool of vrij is:

| Waarde | Gedrag |
|--------|--------|
| `"auto"` | Model beslist zelf of het een tool aanroept (standaard) |
| `"none"` | Model roept geen tools aan, ook al zijn ze beschikbaar |
| `"required"` | Model moet minstens één tool aanroepen |
| `{"type": "function", "function": {"name": "get_weather"}}` | Model moet de gespecificeerde tool aanroepen |

Probeer elke optie in het Python voorbeeld:

```python
# Dwing het model om get_weather aan te roepen
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Let op:** Niet alle `tool_choice` opties worden door elk model ondersteund. Als een model `"required"` niet ondersteunt, negeert het deze en gedraagt zich als `"auto"`.

---

## Veelvoorkomende Valstrikken

| Probleem | Oplossing |
|----------|-----------|
| Model roept nooit tools aan | Zorg dat je een tool-calling model gebruikt (bijv. qwen2.5-0.5b). Kijk naar de tabel hierboven. |
| `tool_call_id` komt niet overeen | Gebruik altijd de `id` uit de tool call respons, niet een vaste waarde |
| Model geeft onjuiste JSON terug in `arguments` | Kleinere modellen produceren soms ongeldig JSON. Gebruik try/catch rond `JSON.parse()` |
| Model roept een niet-bestaande tool aan | Voeg een default handler toe in je `execute_tool` functie |
| Oneindige tool-calling loop | Stel een maximaal aantal rondes in (bijv. 5) om te voorkomen dat het blijft doorgaan |

---

## Belangrijkste Inzichten

1. **Tool calling** laat modellen functies uitvoeren in plaats van antwoorden te raden
2. Het model **voert nooit code uit**; jouw applicatie bepaalt wat er draait
3. Tools definiëren als **JSON Schema** objecten volgens het OpenAI function-calling formaat
4. Het gesprek volgt een **multi-turn patroon**: gebruiker, dan assistant (tool_calls), dan tool (resultaten), dan assistant (definitief antwoord)
5. Gebruik altijd een **model dat tool calling ondersteunt** (Qwen 2.5, Phi-4-mini)
6. De SDK `createChatClient()` biedt een handige manier om tool-calling verzoeken te doen zonder zelf een OpenAI client te bouwen

---

Ga verder naar [Deel 12: Een Web UI bouwen voor de Zava Creative Writer](part12-zava-ui.md) om een browsergebaseerde frontend toe te voegen aan de multi-agent pijplijn met realtime streaming.

---

[← Deel 10: Custom Modellen](part10-custom-models.md) | [Deel 12: Zava Writer UI →](part12-zava-ui.md)