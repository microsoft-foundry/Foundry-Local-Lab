![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 11: Tool Calling med Lokale Modeller

> **Mål:** Gør din lokale model i stand til at kalde eksterne funktioner (værktøjer), så den kan hente realtidsdata, udføre beregninger eller interagere med API’er — alt sammen kørende privat på din enhed.

## Hvad er Tool Calling?

Tool calling (også kendt som **function calling**) giver et sprogmodel mulighed for at anmode om udførelse af funktioner, som du definerer. I stedet for at gætte et svar, genkender modellen, hvornår et værktøj ville hjælpe, og returnerer en struktureret anmodning til at få din kode til at udføre funktionen. Din applikation kører funktionen, sender resultatet tilbage, og modellen inkorporerer denne information i sit endelige svar.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Dette mønster er essentielt for at bygge agenter, der kan:

- **Slå live-data op** (vejr, aktiekurser, databaseforespørgsler)
- **Udføre præcise beregninger** (matematik, enhedskonverteringer)
- **Foretage handlinger** (sende e-mails, oprette tickets, opdatere optegnelser)
- **Få adgang til private systemer** (interne API’er, filsystemer)

---

## Sådan virker Tool Calling

Tool-calling flowet har fire trin:

| Trin | Hvad sker der |
|-------|---------------|
| **1. Definér værktøjer** | Du beskriver tilgængelige funktioner med JSON Schema — navn, beskrivelse og parametre |
| **2. Modellen beslutter** | Modellen modtager din besked plus værktøjsdefinitionerne. Hvis et værktøj ville hjælpe, returnerer den et `tool_calls` svar i stedet for tekst |
| **3. Udfør lokalt** | Din kode fortolker tool call, kører funktionen og indsamler resultatet |
| **4. Endeligt svar** | Du sender tool-resultatet tilbage til modellen, som producerer sit endelige svar |

> **Vigtig pointe:** Modellen udfører aldrig kode. Den *anmoder kun* om, at et værktøj kaldes. Din applikation bestemmer, om anmodningen skal efterkommes — det holder dig fuldt ud i kontrol.

---

## Hvilke Modeller Understøtter Tool Calling?

Ikke alle modeller understøtter tool calling. I den nuværende Foundry Local-katalog har følgende modeller tool-calling kapabilitet:

| Model | Størrelse | Tool Calling |
|-------|-----------|:------------:|
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

> **Tip:** Til denne lab bruger vi **qwen2.5-0.5b** — den er lille (822 MB download), hurtig og har pålidelig support til tool-calling.

---

## Læringsmål

Når du er færdig med denne lab, vil du kunne:

- Forklare tool-calling mønsteret og hvorfor det er vigtigt for AI-agenter
- Definere toolschemas ved hjælp af OpenAI function-calling formatet
- Håndtere multi-turn tool-calling samtaleflow
- Udføre tool calls lokalt og returnere resultater til modellen
- Vælge den rigtige model til tool-calling scenarier

---

## Forudsætninger

| Krav | Detaljer |
|-------|----------|
| **Foundry Local CLI** | Installeret og på din `PATH` ([Del 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript eller C# SDK installeret ([Del 2](part2-foundry-local-sdk.md)) |
| **En tool-calling model** | qwen2.5-0.5b (vil blive downloadet automatisk) |

---

## Øvelser

### Øvelse 1 — Forstå Tool-Calling Flowet

Før du skriver kode, studer dette sekvensdiagram:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Vigtige observationer:**

1. Du definerer værktøjerne forud som JSON Schema objekter
2. Modellens svar indeholder `tool_calls` i stedet for almindeligt indhold
3. Hver tool call har et unikt `id`, som du skal referere til, når du returnerer resultater
4. Modellen ser alle tidligere beskeder *plus* tool resultaterne, når den genererer det endelige svar
5. Flere tool calls kan ske i ét svar

> **Diskussion:** Hvorfor returnerer modellen tool calls i stedet for at udføre funktionerne direkte? Hvilke sikkerhedsmæssige fordele giver det?

---

### Øvelse 2 — Definere Tool Schemas

Værktøjer defineres med standard OpenAI function-calling format. Hvert værktøj skal have:

- **`type`**: Altid `"function"`
- **`function.name`**: Et beskrivende funktionsnavn (fx `get_weather`)
- **`function.description`**: En klar beskrivelse — modellen bruger denne til at beslutte, hvornår værktøjet skal kaldes
- **`function.parameters`**: Et JSON Schema objekt, der beskriver de forventede argumenter

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

> **Gode praksisser for værktøjsbeskrivelser:**
> - Vær specifik: "Hent aktuelt vejr for en given by" er bedre end "Hent vejr"
> - Beskriv parametre tydeligt: modellen læser disse beskrivelser for at udfylde korrekte værdier
> - Marker påkrævede vs valgfrie parametre — det hjælper modellen med at afgøre, hvad den skal spørge efter

---

### Øvelse 3 — Kør Tool-Calling Eksemplerne

Hver sprogsample definerer to værktøjer (`get_weather` og `get_population`), sender et spørgsmål der udløser tool brug, udfører værktøjet lokalt, og sender resultatet tilbage for et endeligt svar.

<details>
<summary><strong>🐍 Python</strong></summary>

**Forudsætninger:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Kør:**
```bash
python foundry-local-tool-calling.py
```

**Forventet output:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kodegennemgang** (`python/foundry-local-tool-calling.py`):

```python
# Definer værktøjer som en liste over funktionsskemaer
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

# Send med værktøjer — modellen kan returnere tool_calls i stedet for indhold
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Tjek om modellen vil kalde et værktøj
if response.choices[0].message.tool_calls:
    # Udfør værktøjet og send resultatet tilbage
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Forudsætninger:**
```bash
cd javascript
npm install
```

**Kør:**
```bash
node foundry-local-tool-calling.mjs
```

**Forventet output:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kodegennemgang** (`javascript/foundry-local-tool-calling.mjs`):

Dette eksempel bruger den native Foundry Local SDK’s `ChatClient` i stedet for OpenAI SDK, hvilket demonstrerer den praktiske `createChatClient()` metode:

```javascript
// Få en ChatClient direkte fra modelobjektet
const chatClient = model.createChatClient();

// Send med værktøjer — ChatClient håndterer OpenAI-kompatibelt format
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Tjek for værktøjskald
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Udfør værktøjer og send resultater tilbage
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Forudsætninger:**
```bash
cd csharp
dotnet restore
```

**Kør:**
```bash
dotnet run toolcall
```

**Forventet output:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kodegennemgang** (`csharp/ToolCalling.cs`):

C# bruger hjælpemetoden `ChatTool.CreateFunctionTool` til at definere værktøjer:

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

### Øvelse 4 — Tool-Calling Samtaleflowet

At forstå beskedstrukturen er afgørende. Her er det komplette flow, der viser `messages` arrayet på hvert trin:

**Trin 1 — Første anmodning:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Trin 2 — Model svarer med tool_calls (ikke indhold):**
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

**Trin 3 — Du tilføjer assistentbeskeden OG tool-resultatet:**
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

**Trin 4 — Model producerer det endelige svar med brug af tool-resultatet.**

> **Vigtigt:** `tool_call_id` i tool-beskeden skal matche `id` fra tool call. Det er sådan modellen forbinder resultater med forespørgsler.

---

### Øvelse 5 — Flere Tool Calls

En model kan anmode om flere tool calls i ét svar. Prøv at ændre brugerbeskeden for at udløse flere kald:

```python
# I Python — ændr brugermeddelelsen:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// I JavaScript — ændre brugermeddelelsen:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Modellen bør returnere to `tool_calls` — ét til `get_weather` og ét til `get_population`. Din kode håndterer dette allerede, fordi den gennemløber alle tool calls.

> **Prøv:** Ændr brugerbeskeden og kør eksemplet igen. Kalder modellen begge værktøjerne?

---

### Øvelse 6 — Tilføj Dit Eget Værktøj

Udvid et af eksemplerne med et nyt værktøj. For eksempel tilføj et `get_time` værktøj:

1. Definér tool schemaet:
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

2. Tilføj udførelseslogikken:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # I en rigtig app, brug et tidszonebibliotek
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... eksisterende værktøjer ...
```

3. Tilføj værktøjet til `tools` arrayet og test med: "Hvad er klokken i Tokyo?"

> **Udfordring:** Tilføj et værktøj, der udfører en beregning, fx `convert_temperature`, som konverterer mellem Celsius og Fahrenheit. Test med: "Konverter 100°F til Celsius."

---

### Øvelse 7 — Tool Calling med SDK’ets ChatClient (JavaScript)

JavaScript-eksemplet bruger allerede SDK’ets native `ChatClient` i stedet for OpenAI SDK. Dette er en bekvemmelighedsfunktion, der fjerner behovet for selv at oprette en OpenAI-klient:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient oprettes direkte fra modelobjektet
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat accepterer værktøjer som en anden parameter
const response = await chatClient.completeChat(messages, tools);
```

Sammenlign dette med Python-tilgangen, som eksplicit bruger OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Begge mønstre er gyldige. `ChatClient` er mere bekvemt; OpenAI SDK giver dig adgang til hele spektret af OpenAI-parametre.

> **Prøv:** Ændr JavaScript-eksemplet til at bruge OpenAI SDK i stedet for `ChatClient`. Du skal bruge `import OpenAI from "openai"` og oprette klienten med endpoint fra `manager.urls[0]`.

---

### Øvelse 8 — Forståelse af tool_choice

`tool_choice` parameteren kontrollerer, om modellen *skal* bruge et værktøj eller kan vælge frit:

| Værdi | Opførsel |
|--------|----------|
| `"auto"` | Modellen beslutter, om den vil kalde et værktøj (standard) |
| `"none"` | Modellen vil ikke kalde nogen værktøjer, selvom de findes |
| `"required"` | Modellen skal kalde mindst ét værktøj |
| `{"type": "function", "function": {"name": "get_weather"}}` | Modellen skal kalde det specificerede værktøj |

Prøv hver mulighed i Python-eksemplet:

```python
# Tving modellen til at kalde get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Bemærk:** Ikke alle `tool_choice` muligheder understøttes af alle modeller. Hvis en model ikke understøtter `"required"`, kan den ignorere indstillingen og opføre sig som `"auto"`.

---

## Almindelige Faldgruber

| Problem | Løsning |
|---------|----------|
| Modellen kalder aldrig værktøjer | Sørg for at bruge en tool-calling model (fx qwen2.5-0.5b). Tjek tabellen ovenfor. |
| `tool_call_id` stemmer ikke overens | Brug altid `id` fra tool call svaret, ikke en hardcoded værdi |
| Model returnerer ugyldig JSON i `arguments` | Mindre modeller kan ind imellem lave ugyldig JSON. Pak `JSON.parse()` ind i try/catch |
| Model kalder et værktøj, der ikke eksisterer | Tilføj en standardhandler i din `execute_tool` funktion |
| Uendeligt tool-calling loop | Sæt en maksimum antal runder (fx 5) for at forhindre uendelige løkker |

---

## Vigtige Indsigter

1. **Tool calling** giver modeller mulighed for at anmode om funktionseksekvering fremfor at gætte svar
2. Modellen **udfører aldrig kode**; det er din applikation, der bestemmer hvad der skal køres
3. Værktøjer defineres som **JSON Schema** objekter i OpenAI function-calling format
4. Samtalen bruger et **multi-turn mønster**: bruger, derefter assistent (tool_calls), så værktøj (resultater), og til sidst assistent (endeligt svar)
5. Brug altid en **model der understøtter tool calling** (Qwen 2.5, Phi-4-mini)
6. SDK’ets `createChatClient()` giver en nem måde at lave tool-calling anmodninger uden at bygge en OpenAI klient selv

---

Fortsæt til [Del 12: Byg en Web UI til Zava Creative Writer](part12-zava-ui.md) for at tilføje et browserbaseret frontend til multi-agent pipeline med realtids streaming.

---

[← Del 10: Tilpassede Modeller](part10-custom-models.md) | [Del 12: Zava Writer UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi bestræber os på nøjagtighed, skal du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det oprindelige dokument på dets modersmål bør betragtes som den autoritative kilde. For kritisk information anbefales professionel menneskelig oversættelse. Vi påtager os intet ansvar for misforståelser eller fejltolkninger, der opstår ved brug af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->