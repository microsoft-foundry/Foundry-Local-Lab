![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 11: Verktygssamtal med Lokala Modeller

> **Mål:** Möjliggör för din lokala modell att anropa externa funktioner (verktyg) så att den kan hämta realtidsdata, utföra beräkningar eller interagera med API:er — allt körs privat på din enhet.

## Vad är Verktygssamtal?

Verktygssamtal (även känt som **funktionanrop**) låter en språkmodell begära att funktioner du definierar exekveras. Istället för att gissa ett svar, känner modellen igen när ett verktyg skulle hjälpa till och returnerar en strukturerad förfrågan för din kod att köra. Din applikation kör funktionen, skickar tillbaka resultatet, och modellen inkorporerar den informationen i sitt slutgiltiga svar.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Det här mönstret är avgörande för att bygga agenter som kan:

- **Slå upp live-data** (väder, aktiekurser, databasfrågor)
- **Utföra precisa beräkningar** (matematik, enhetsomvandlingar)
- **Utföra åtgärder** (skicka e-post, skapa ärenden, uppdatera poster)
- **Åtkomst till privata system** (interna API:er, filsystem)

---

## Hur Verktygssamtal Fungerar

Verktygssamtalsflödet har fyra steg:

| Steg | Vad som händer |
|-------|-------------|
| **1. Definiera verktyg** | Du beskriver tillgängliga funktioner med JSON Schema — namn, beskrivning och parametrar |
| **2. Modellen bestämmer** | Modellen får ditt meddelande plus verktygsdefinitionerna. Om ett verktyg skulle hjälpa, returnerar den ett `tool_calls`-svar istället för ett text svar |
| **3. Exekvera lokalt** | Din kod tolkar verktygsanropet, kör funktionen, och samlar in resultatet |
| **4. Slutgiltigt svar** | Du skickar verktygsresultatet tillbaka till modellen, som producerar sitt slutgiltiga svar |

> **Viktigt:** Modellen exekverar aldrig kod. Den *begär* bara att ett verktyg ska anropas. Din applikation avgör om förfrågan ska tillgodoses — detta ger dig full kontroll.

---

## Vilka Modeller Stöder Verktygssamtal?

Inte alla modeller stöder verktygssamtal. I den nuvarande Foundry Local-katalogen har följande modeller verktygsanropsfunktionalitet:

| Modell | Storlek | Verktygssamtal |
|-------|------|:---:|
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

> **Tips:** För denna labb använder vi **qwen2.5-0.5b** — den är liten (822 MB nedladdning), snabb, och har pålitligt stöd för verktygssamtal.

---

## Inlärningsmål

I slutet av denna labb kommer du kunna:

- Förklara verktygssamtalsmönstret och varför det är viktigt för AI-agenter
- Definiera verktygsscheman med OpenAI:s funktionsanropsformat
- Hantera den flerstegs konversationsflödet för verktygssamtal
- Exekvera verktygsanrop lokalt och returnera resultat till modellen
- Välja rätt modell för verktygssamtalsscenarier

---

## Förutsättningar

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installerad och i din `PATH` ([Del 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python-, JavaScript- eller C# SDK installerad ([Del 2](part2-foundry-local-sdk.md)) |
| **En verktygssamtalsmodell** | qwen2.5-0.5b (kommer att laddas ner automatiskt) |

---

## Övningar

### Övning 1 — Förstå Verktygssamtalsflödet

Innan du skriver kod, studera detta sekvensdiagram:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Viktiga observationer:**

1. Du definierar verktygen i förväg som JSON Schema-objekt  
2. Modellens svar innehåller `tool_calls` istället för vanligt innehåll  
3. Varje verktygsanrop har ett unikt `id` som du måste hänvisa till när du skickar tillbaka resultat  
4. Modellen ser alla tidigare meddelanden *plus* verktygsresultaten när den genererar det slutgiltiga svaret  
5. Flera verktygsanrop kan ske i ett enda svar

> **Diskussion:** Varför returnerar modellen verktygsanrop istället för att exekvera funktioner direkt? Vilka säkerhetsfördelar ger detta?

---

### Övning 2 — Definiera Verktygsscheman

Verktyg definieras med standardformatet för OpenAI:s funktionsanrop. Varje verktyg kräver:

- **`type`**: Alltid `"function"`  
- **`function.name`**: Ett beskrivande funktionsnamn (exempelvis `get_weather`)  
- **`function.description`**: En tydlig beskrivning — modellen använder den för att avgöra när verktyget ska anropas  
- **`function.parameters`**: Ett JSON Schema-objekt som beskriver förväntade argument

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

> **Bästa praxis för verktygsbeskrivningar:**
> - Var specifik: "Hämta aktuellt väder för en given stad" är bättre än "Hämta väder"  
> - Beskriv parametrar tydligt: modellen läser dessa beskrivningar för att fylla i rätt värden  
> - Markera vilka parametrar som är obligatoriska vs valfria — det hjälper modellen att avgöra vad den ska fråga efter

---

### Övning 3 — Kör Verktygssamtalsexemplen

Varje kodexempel definierar två verktyg (`get_weather` och `get_population`), skickar en fråga som triggar verktygsanvändning, exekverar verktyget lokalt, och skickar tillbaka resultatet för ett slutgiltigt svar.

<details>
<summary><strong>🐍 Python</strong></summary>

**Förutsättningar:**  
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```
  
**Kör:**  
```bash
python foundry-local-tool-calling.py
```
  
**Förväntad utdata:**  
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**Kodgenomgång** (`python/foundry-local-tool-calling.py`):  

```python
# Definiera verktyg som en lista med funktionsscheman
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

# Skicka med verktyg — modellen kan returnera tool_calls istället för innehåll
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Kontrollera om modellen vill anropa ett verktyg
if response.choices[0].message.tool_calls:
    # Kör verktyget och skicka resultatet tillbaka
    ...
```
  
</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Förutsättningar:**  
```bash
cd javascript
npm install
```
  
**Kör:**  
```bash
node foundry-local-tool-calling.mjs
```
  
**Förväntad utdata:**  
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**Kodgenomgång** (`javascript/foundry-local-tool-calling.mjs`):  

Detta exempel använder Foundry Locals inbyggda SDK `ChatClient` istället för OpenAI SDK, vilket visar bekvämligheten med metoden `createChatClient()`:

```javascript
// Hämta en ChatClient direkt från modellobjektet
const chatClient = model.createChatClient();

// Skicka med verktyg — ChatClient hanterar OpenAI-kompatibelt format
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Kontrollera efter verktygsanrop
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Kör verktyg och skicka tillbaka resultaten
    ...
}
```
  
</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Förutsättningar:**  
```bash
cd csharp
dotnet restore
```
  
**Kör:**  
```bash
dotnet run toolcall
```
  
**Förväntad utdata:**  
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**Kodgenomgång** (`csharp/ToolCalling.cs`):  

C# använder hjälpfunktionen `ChatTool.CreateFunctionTool` för att definiera verktyg:

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

### Övning 4 — Verktygssamtalskonversationens Flöde  

Att förstå meddelandestrukturen är kritiskt. Här är hela flödet, som visar `messages`-arrayen i varje steg:

**Steg 1 — Initial förfrågan:**  
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```
  
**Steg 2 — Modellen svarar med tool_calls (inte innehåll):**  
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
  
**Steg 3 — Du lägger till assistentmeddelandet OCH verktygsresultatet:**  
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
  
**Steg 4 — Modellen producerar slutgiltigt svar med hjälp av verktygsresultatet.**

> **Viktigt:** `tool_call_id` i verktygsmeddelandet måste matcha `id` från verktygsanropet. Detta är hur modellen kopplar ihop resultat med förfrågningar.

---

### Övning 5 — Flera Verktygsanrop  

En modell kan begära flera verktygsanrop i ett enda svar. Prova att ändra användarmeddelandet för att trigga flera anrop:

```python
# I Python — ändra användarmeddelandet:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```
  
```javascript
// I JavaScript — ändra användarmeddelandet:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```
  
Modellen bör returnera två `tool_calls` — ett för `get_weather` och ett för `get_population`. Din kod hanterar detta redan eftersom den loopar igenom alla verktygsanrop.

> **Testa:** Modifiera användarmeddelandet och kör exemplet igen. Anropar modellen båda verktygen?

---

### Övning 6 — Lägg till Eget Verktyg  

Utöka ett av exemplen med ett nytt verktyg. Till exempel, lägg till verktyget `get_time`:

1. Definiera verktygsschemat:  
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
  
2. Lägg till exekveringslogik:  
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # I en riktig app, använd ett bibliotek för tidszoner
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... befintliga verktyg ...
```
  
3. Lägg till verktyget i `tools`-arrayen och testa med: "Vad är klockan i Tokyo?"

> **Utmaning:** Lägg till ett verktyg som utför en beräkning, exempelvis `convert_temperature` som konverterar mellan Celsius och Fahrenheit. Testa med: "Konvertera 100°F till Celsius."

---

### Övning 7 — Verktygssamtal med SDK:s ChatClient (JavaScript)  

JavaScript-exemplet använder redan SDK:s inbyggda `ChatClient` istället för OpenAI SDK. Detta är en bekvämlighetsfunktion som tar bort behovet av att bygga en OpenAI-klient själv:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient skapas direkt från modellobjektet
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat accepterar verktyg som en andra parameter
const response = await chatClient.completeChat(messages, tools);
```
  
Jämför detta med Python-metoden som explicit använder OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```
  
Båda mönstren är giltiga. `ChatClient` är mer bekvämt; OpenAI SDK ger dig tillgång till hela utbudet av OpenAI-parametrar.

> **Testa:** Modifiera JavaScript-exemplet för att använda OpenAI SDK istället för `ChatClient`. Du behöver `import OpenAI from "openai"` och skapa klienten med endpoint från `manager.urls[0]`.

---

### Övning 8 — Förstå tool_choice  

Parametern `tool_choice` styr om modellen *måste* använda ett verktyg eller kan välja fritt:

| Värde | Beteende |
|-------|-----------|
| `"auto"` | Modellen bestämmer själv om den ska anropa ett verktyg (standard) |
| `"none"` | Modellen kommer inte anropa några verktyg, även om de finns tillgängliga |
| `"required"` | Modellen måste anropa minst ett verktyg |
| `{"type": "function", "function": {"name": "get_weather"}}` | Modellen måste anropa det angivna verktyget |

Prova varje alternativ i Python-exemplet:

```python
# Tvinga modellen att kalla på get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```
  
> **Notera:** Inte alla `tool_choice`-alternativ stöds av varje modell. Om en modell inte stöder `"required"` kan den ignorera inställningen och bete sig som `"auto"`.

---

## Vanliga Fallgropar

| Problem | Lösning |
|---------|----------|
| Modell anropar aldrig verktyg | Säkerställ att du använder en verktygssamtalsmodell (t.ex. qwen2.5-0.5b). Kolla tabellen ovan. |
| `tool_call_id` stämmer inte | Använd alltid `id` från verktygsanropssvaret, inte ett hårdkodat värde |
| Modell returnerar ogiltig JSON i `arguments` | Mindre modeller kan ibland producera ogiltig JSON. Lägg `JSON.parse()` i en try/catch |
| Modell anropar ett icke-existerande verktyg | Lägg till en standardhanterare i din `execute_tool`-funktion |
| Oändlig verktygssamtalsloop | Sätt en maxgräns för antal varv (t.ex. 5) för att förhindra oändliga loopar |

---

## Viktiga Slutsatser

1. **Verktygssamtal** låter modeller begära funktionsexekvering istället för att gissa svar  
2. Modellen **exekverar aldrig kod**; din applikation bestämmer vad som ska köras  
3. Verktyg definieras som **JSON Schema**-objekt enligt OpenAI:s funktionsanropsformat  
4. Konversationen använder ett **flerstegs-mönster**: användare, sedan assistent (tool_calls), sedan verktyg (resultat), sedan assistent (slutgiltigt svar)  
5. Använd alltid en **modell som stöder verktygssamtal** (Qwen 2.5, Phi-4-mini)  
6. SDK:s `createChatClient()` ger ett bekvämt sätt att göra verktygssamtalsförfrågningar utan att själv konstruera en OpenAI-klient

---

Fortsätt till [Del 12: Bygga ett Webbgränssnitt för Zava Creative Writer](part12-zava-ui.md) för att lägga till ett webbläsarbaserat frontend till multi-agent pipeline med realtidsströmning.

---

[← Del 10: Anpassade Modeller](part10-custom-models.md) | [Del 12: Zava Writer UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfriskrivning**:
Detta dokument har översatts med hjälp av AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, vänligen notera att automatiska översättningar kan innehålla fel eller brister. Det ursprungliga dokumentet på dess modersmål bör betraktas som den auktoritativa källan. För kritisk information rekommenderas professionell mänsklig översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår från användningen av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->