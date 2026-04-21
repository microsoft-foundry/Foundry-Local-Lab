![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 11: Verktøysanrop med lokale modeller

> **Mål:** Gjør det mulig for din lokale modell å kalle eksterne funksjoner (verktøy) slik at den kan hente sanntidsdata, utføre beregninger eller interagere med API-er — alt kjørende privat på din enhet.

## Hva er verktøysanrop?

Verktøysanrop (også kjent som **funksjonsanrop**) lar en språkmodell be om utførelse av funksjoner du definerer. I stedet for å gjette et svar, gjenkjenner modellen når et verktøy vil hjelpe og returnerer en strukturert forespørsel for at koden din skal utføre. Applikasjonen din kjører funksjonen, sender resultatet tilbake, og modellen inkorporerer denne informasjonen i sitt endelige svar.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Dette mønsteret er essensielt for å bygge agenter som kan:

- **Hente levende data** (vær, aksjekurser, databaseforespørsler)
- **Utføre presise beregninger** (matematikk, enhetskonverteringer)
- **Utføre handlinger** (sende e-poster, opprette saker, oppdatere poster)
- **Tilgang til private systemer** (interne API-er, filsystemer)

---

## Hvordan fungerer verktøysanrop?

Flyten for verktøysanrop har fire steg:

| Steg | Hva skjer |
|-------|-------------|
| **1. Definer verktøy** | Du beskriver tilgjengelige funksjoner ved hjelp av JSON Schema — navn, beskrivelse og parametere |
| **2. Modellen avgjør** | Modellen mottar meldingen din pluss verktøydefinisjonene. Hvis et verktøy vil hjelpe, returnerer den et `tool_calls` svar i stedet for tekst |
| **3. Kjør lokalt** | Koden din analyserer verktøysanropet, kjører funksjonen og samler inn resultatet |
| **4. Endelig svar** | Du sender verktøyresultatet tilbake til modellen, som produserer sitt endelige svar |

> **Viktig punkt:** Modellen kjører aldri kode. Den *ber bare* om at et verktøy kalles. Applikasjonen din bestemmer om forespørselen skal godtas — dette gir deg full kontroll.

---

## Hvilke modeller støtter verktøysanrop?

Ikke alle modeller støtter verktøysanrop. I det nåværende Foundry Local-katalogen har følgende modeller støtte for verktøysanrop:

| Modell | Størrelse | Verktøysanrop |
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

> **Tips:** For dette laboratoriet bruker vi **qwen2.5-0.5b** — den er liten (822 MB nedlasting), rask og har pålitelig støtte for verktøysanrop.

---

## Læringsmål

Etter dette laboratoriet vil du kunne:

- Forklare verktøysanrop-mønsteret og hvorfor det er viktig for AI-agenter
- Definere verktøy-skjemaer ved bruk av OpenAI-funksjonsanropsformatet
- Håndtere multi-turn verktøysanropssamtale
- Utføre verktøysanrop lokalt og returnere resultater til modellen
- Velge riktig modell for verktøysanrop-scenarier

---

## Forutsetninger

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installert og i din `PATH` ([Del 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript eller C# SDK installert ([Del 2](part2-foundry-local-sdk.md)) |
| **En modell med verktøysanrop** | qwen2.5-0.5b (lastes ned automatisk) |

---

## Øvelser

### Øvelse 1 — Forstå verktøysanropsflyten

Før du skriver kode, studer dette sekvensdiagrammet:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Nøkkelobservasjoner:**

1. Du definerer verktøyene på forhånd som JSON Schema-objekter
2. Modellens svar inneholder `tool_calls` i stedet for vanlig innhold
3. Hvert verktøysanrop har en unik `id` du må referere til når du returnerer resultater
4. Modellen ser alle tidligere meldinger *pluss* verktøyresultatene når det genereres endelig svar
5. Flere verktøysanrop kan skje i ett svar

> **Diskusjon:** Hvorfor returnerer modellen verktøysanrop i stedet for å kjøre funksjoner direkte? Hvilke sikkerhetsfordeler gir dette?

---

### Øvelse 2 — Definere verktøy-skjemaer

Verktøy defineres ved hjelp av standard OpenAI-funksjonsanropsformat. Hvert verktøy trenger:

- **`type`**: Alltid `"function"`
- **`function.name`**: Et beskrivende funksjonsnavn (f.eks. `get_weather`)
- **`function.description`**: En klar beskrivelse — modellen bruker denne for å avgjøre når verktøyet skal kalles
- **`function.parameters`**: Et JSON Schema-objekt som beskriver forventede argumenter

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

> **Beste praksis for verktøybeskrivelser:**
> - Vær spesifikk: «Hent dagens vær for en gitt by» er bedre enn «Hent vær»
> - Beskriv parametere tydelig: modellen leser disse beskrivelsene for å fylle inn riktige verdier
> - Merk påkrevde vs valgfri parametere — dette hjelper modellen til å vite hva den skal spørre om

---

### Øvelse 3 — Kjør eksemplene for verktøysanrop

Hvert språkeksempel definerer to verktøy (`get_weather` og `get_population`), sender et spørsmål som utløser verktøybruk, kjører verktøyet lokalt, og sender resultatet tilbake for et endelig svar.

<details>
<summary><strong>🐍 Python</strong></summary>

**Forutsetninger:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Kjør:**
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

**Kodegjennomgang** (`python/foundry-local-tool-calling.py`):

```python
# Definer verktøy som en liste over funksjonsskjemaer
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

# Send med verktøy — modellen kan returnere tool_calls i stedet for innhold
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Sjekk om modellen ønsker å bruke et verktøy
if response.choices[0].message.tool_calls:
    # Utfør verktøyet og send resultatet tilbake
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Forutsetninger:**
```bash
cd javascript
npm install
```

**Kjør:**
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

**Kodegjennomgang** (`javascript/foundry-local-tool-calling.mjs`):

Dette eksemplet bruker den native Foundry Local SDKs `ChatClient` i stedet for OpenAI SDK, noe som viser bekvemmeligheten med `createChatClient()`-metoden:

```javascript
// Hent en ChatClient direkte fra modellobjektet
const chatClient = model.createChatClient();

// Send med verktøy — ChatClient håndterer OpenAI-kompatibelt format
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Sjekk for verktøysanrop
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Utfør verktøy og send resultater tilbake
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Forutsetninger:**
```bash
cd csharp
dotnet restore
```

**Kjør:**
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

**Kodegjennomgang** (`csharp/ToolCalling.cs`):

C# bruker hjelpen `ChatTool.CreateFunctionTool` for å definere verktøy:

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

### Øvelse 4 — Verktøysanropssamtalens flyt

Forståelse av meldingsstrukturen er kritisk. Her er hele flyten, som viser `messages`-arrayet på hvert trinn:

**Steg 1 — Innledende forespørsel:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Steg 2 — Modell svarer med tool_calls (ikke innhold):**
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

**Steg 3 — Du legger til assistentmeldingen OG verktøyresultatet:**
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

**Steg 4 — Modellen produserer endelig svar med verktøyresultatet.**

> **Viktig:** `tool_call_id` i verktøymeldingen må samsvare med `id` fra verktøysanropet. Slik kobler modellen resultater og forespørsler.

---

### Øvelse 5 — Flere verktøysanrop

En modell kan be om flere verktøysanrop i ett svar. Prøv å endre brukermeldingen for å trigge flere anrop:

```python
# I Python — endre brukerbeskjeden:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// I JavaScript — endre brukermeldingen:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Modellen skal returnere to `tool_calls` — en for `get_weather` og en for `get_population`. Koden din håndterer dette allerede fordi den løper gjennom alle verktøysanrop.

> **Prøv det:** Endre brukermeldingen og kjør eksemplet på nytt. Kaller modellen begge verktøy?

---

### Øvelse 6 — Legg til ditt eget verktøy

Utvid ett av eksemplene med et nytt verktøy. For eksempel, legg til et `get_time`-verktøy:

1. Definer verktøyskjema:
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

2. Legg til kjørelogikken:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # I en ekte app, bruk et tidssonebibliotek
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... eksisterende verktøy ...
```

3. Legg verktøyet til i `tools`-arrayet og test med: "Hva er klokka i Tokyo?"

> **Utfordring:** Legg til et verktøy som utfører en beregning, som `convert_temperature` som konverterer mellom Celsius og Fahrenheit. Test med: "Konverter 100°F til Celsius."

---

### Øvelse 7 — Verktøysanrop med SDKs ChatClient (JavaScript)

JavaScript-eksemplet bruker allerede SDKs native `ChatClient` i stedet for OpenAI SDK. Dette er en bekvemmelighetsfunksjon som eliminierer behovet for å bygge en OpenAI-klient selv:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient opprettes direkte fra modellobjektet
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat aksepterer verktøy som en annen parameter
const response = await chatClient.completeChat(messages, tools);
```

Sammenlign dette med Python-tilnærmingen som eksplisitt bruker OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Begge mønstrene er gyldige. `ChatClient` er mer praktisk; OpenAI SDK gir deg tilgang til hele spekteret av OpenAI-parametere.

> **Prøv det:** Endre JavaScript-eksemplet til å bruke OpenAI SDK i stedet for `ChatClient`. Du må importere `OpenAI` fra "openai" og bygge klienten med endepunktet fra `manager.urls[0]`.

---

### Øvelse 8 — Forstå tool_choice

`tool_choice`-parameteren styrer om modellen *må* bruke et verktøy eller kan velge fritt:

| Verdi | Atferd |
|-------|-----------|
| `"auto"` | Modellen avgjør om den skal kalle et verktøy (standard) |
| `"none"` | Modellen vil ikke kalle noen verktøy, selv om de er tilgjengelige |
| `"required"` | Modellen må kalle minst ett verktøy |
| `{"type": "function", "function": {"name": "get_weather"}}` | Modellen må kalle det spesifiserte verktøyet |

Prøv hvert alternativ i Python-eksemplet:

```python
# Tving modellen til å kalle get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Merk:** Ikke alle `tool_choice`-alternativer støttes av alle modeller. Hvis en modell ikke støtter `"required"`, kan den ignorere innstillingen og oppføre seg som `"auto"`.

---

## Vanlige fallgruver

| Problem | Løsning |
|---------|----------|
| Modell kaller aldri verktøy | Sørg for at du bruker en modell med verktøysanrop (f.eks. qwen2.5-0.5b). Se tabellen ovenfor. |
| `tool_call_id` samsvarer ikke | Bruk alltid `id` fra verktøysanrop-responsen, ikke en hardkodet verdi |
| Modell returnerer feilformatert JSON i `arguments` | Mindre modeller kan tidvis produsere ugyldig JSON. Legg `JSON.parse()` i try/catch |
| Modell kaller et verktøy som ikke finnes | Legg til en standardbehandler i din `execute_tool`-funksjon |
| Uendelig verktøysanropsløkke | Sett en maksimal rundegrense (f.eks. 5) for å unngå løkker |

---

## Viktige punkter å ta med seg

1. **Verktøysanrop** lar modeller be om funksjonsutførelse i stedet for å gjette svar
2. Modellen **kjører aldri kode**; applikasjonen din avgjør hva som kjører
3. Verktøy defineres som **JSON Schema**-objekter etter OpenAI-funksjonsanropsformatet
4. Samtalen bruker et **multi-turn-mønster**: bruker, så assistent (tool_calls), så verktøy (resultater), deretter assistent (endelig svar)
5. Bruk alltid en **modell som støtter verktøysanrop** (Qwen 2.5, Phi-4-mini)
6. SDKs `createChatClient()` gir en praktisk måte å gjøre verktøysanrop uten å bygge OpenAI-klienten manuelt

---

Fortsett til [Del 12: Bygge en web-UI for Zava Creative Writer](part12-zava-ui.md) for å legge til et nettleserbasert grensesnitt til multi-agent-pipelinen med sanntidsstrømming.

---

[← Del 10: Egendefinerte modeller](part10-custom-models.md) | [Del 12: Zava Writer UI →](part12-zava-ui.md)