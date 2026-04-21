![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 11 dalis: Įrankių kvietimas naudojant vietinius modelius

> **Tikslas:** Suteikti savo vietiniam modeliui galimybę kviesti išorines funkcijas (įrankius), kad jis galėtų gauti realaus laiko duomenis, atlikti skaičiavimus arba sąveikauti su API — visa tai vykstant privčiai jūsų įrenginyje.

## Kas yra įrankių kvietimas?

Įrankių kvietimas (dar vadinamas **funkcijų kvietimu**) leidžia kalbos modeliui prašyti vykdyti jūsų apibrėžtas funkcijas. Vietoje spėjimo atsakymo modelis atpažįsta, kada įrankis galėtų padėti, ir pateikia struktūruotą užklausą jūsų kodui vykdyti. Jūsų programa atlieka funkciją, siunčia rezultatą atgal, o modelis įtraukia šią informaciją į galutinį atsakymą.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Šis modelis yra esminis kuriant agentus, kurie gali:

- **Gauti tiesioginius duomenis** (oro sąlygas, akcijų kainas, duomenų bazės užklausas)
- **Atlikti tikslinius skaičiavimus** (matematika, vienetų konvertavimas)
- **Imtis veiksmų** (siųsti el. laiškus, kurti bilietus, atnaujinti įrašus)
- **Prieiti prie privačių sistemų** (vidiniai API, failų sistemos)

---

## Kaip veikia įrankių kvietimas

Įrankių kvietimo procesas susideda iš keturių etapų:

| Etapas | Kas vyksta |
|--------|------------|
| **1. Apibrėžti įrankius** | Aprašote prieinamas funkcijas naudodami JSON Schema — pavadinimą, aprašymą ir parametrus |
| **2. Modelis sprendžia** | Modelis gauna jūsų žinutę ir įrankių apibrėžimus. Jei įrankis padės, grąžina `tool_calls` atsakymą vietoje teksto |
| **3. Vykdyti vietoje** | Jūsų kodas analizuoja įrankio kvietimą, vykdo funkciją ir surenka rezultatą |
| **4. Galutinis atsakymas** | Siunčiate įrankio rezultatą atgal modeliui, kuris suformuoja galutinį atsakymą |

> **Svarbu:** Modelis niekada nevykdo kodo. Jis tik *prašosi*, kad įrankis būtų iškviestas. Jūsų aplikacija nusprendžia, ar įvykdyti kvietimą — tai suteikia pilną kontrolę.

---

## Kurie modeliai palaiko įrankių kvietimą?

Ne visi modeliai palaiko įrankių kvietimą. Dabartiniame Foundry Local kataloge šie modeliai turi įrankių kvietimo funkciją:

| Modelis | Dydis | Įrankių kvietimas |
|---------|-------|:-----------------:|
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

> **Patarimas:** Šiam laboratoriniam darbui naudojame **qwen2.5-0.5b** — jis mažas (822 MB atsisiuntimas), greitas ir patikimai palaiko įrankių kvietimą.

---

## Mokymosi tikslai

Baigę šią pamoką galėsite:

- Paaiškinti įrankių kvietimo modelį ir jo svarbą AI agentams
- Apibrėžti įrankių schemas naudodami OpenAI funkcijų kvietimo formatą
- Tvarkyti daugkartinio pokalbio įrankių kvietimo srautą
- Vykdyti įrankių kvietimus vietoje ir grąžinti rezultatus modeliui
- Pasirinkti tinkamą modelį įrankių kvietimų scenarijams

---

## Reikalingos žinios

| Reikalavimas | Detalės |
|--------------|---------|
| **Foundry Local CLI** | Įdiegta ir pridėta į jūsų `PATH` ([1 dalis](part1-getting-started.md)) |
| **Foundry Local SDK** | Įdiegta Python, JavaScript arba C# SDK ([2 dalis](part2-foundry-local-sdk.md)) |
| **Įrankių kvietimo modelis** | qwen2.5-0.5b (bus atsisiųstas automatiškai) |

---

## Užduotys

### Užduotis 1 — Suprasti įrankių kvietimo srautą

Prieš rašant kodą, išstudijuokite šią sekų diagramą:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Pagrindiniai pastebėjimai:**

1. Įrankiai apibrėžiami iš anksto kaip JSON Schema objektai
2. Modelio atsakyme yra `tool_calls` vietoje įprasto turinio
3. Kiekvienas įrankio kvietimas turi unikalų `id`, į kurį reikia kreiptis grąžinant rezultatus
4. Modelis mato visas ankstesnes žinutes *plius* įrankių rezultatus generuodamas galutinį atsakymą
5. Viename atsakyme gali būti keli įrankių kvietimai

> **Diskusija:** Kodėl modelis pateikia įrankių kvietimus, o ne vykdo funkcijas tiesiogiai? Kokias saugumo naudas tai suteikia?

---

### Užduotis 2 — Įrankių schemų apibrėžimas

Įrankiai aprašomi naudojant standartinį OpenAI funkcijų kvietimo formatą. Kiekvienam įrankiui reikia:

- **`type`**: Visada `"function"`
- **`function.name`**: Aprašomas funkcijos pavadinimas (pvz., `get_weather`)
- **`function.description`**: Aiškus aprašymas — modelis naudoja tai nuspręsti, kada kviesti įrankį
- **`function.parameters`**: JSON Schema objektas aprašantis laukiamus argumentus

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

> **Geriausios praktikos aprašant įrankius:**
> - Būkite konkretūs: "Gauti dabartinę oro situaciją tam tikrame mieste" geriau nei "Gauti orą"
> - Aiškiai aprašykite parametrus: modelis skaito aprašymus, kad žinotų, kaip užpildyti reikšmes
> - Pažymėkite privalomus ir neprivalomus parametrus — tai padeda modeliui nuspręsti, ko prašyti

---

### Užduotis 3 — Vykdyti įrankių kvietimo pavyzdžius

Kiekvienas kalbos pavyzdys apibrėžia du įrankius (`get_weather` ir `get_population`), siunčia klausimą, kurį aktyvuoja įrankių naudojimą, vykdo įrankį vietoje ir grąžina rezultatą, kad gauti galutinį atsakymą.

<details>
<summary><strong>🐍 Python</strong></summary>

**Reikalinga:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Vykdyti:**
```bash
python foundry-local-tool-calling.py
```

**Laukiamas rezultatas:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kodo paaiškinimas** (`python/foundry-local-tool-calling.py`):

```python
# Apibrėžkite įrankius kaip funkcijų schemų sąrašą
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

# Siųskite su įrankiais — modelis gali grąžinti tool_calls vietoje turinio
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Patikrinkite, ar modelis nori iškviesti įrankį
if response.choices[0].message.tool_calls:
    # Vykdykite įrankį ir grąžinkite rezultatą
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Reikalinga:**
```bash
cd javascript
npm install
```

**Vykdyti:**
```bash
node foundry-local-tool-calling.mjs
```

**Laukiamas rezultatas:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kodo paaiškinimas** (`javascript/foundry-local-tool-calling.mjs`):

Šis pavyzdys naudoja vietinį Foundry Local SDK `ChatClient`, o ne OpenAI SDK, parodydamas patogią `createChatClient()` funkciją:

```javascript
// Gauti ChatClient tiesiogiai iš modelio objekto
const chatClient = model.createChatClient();

// Siųsti su įrankiais — ChatClient tvarko OpenAI suderinamą formatą
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Patikrinti įrankių kvietimus
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Vykdyti įrankius ir grąžinti rezultatus
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Reikalinga:**
```bash
cd csharp
dotnet restore
```

**Vykdyti:**
```bash
dotnet run toolcall
```

**Laukiamas rezultatas:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kodo paaiškinimas** (`csharp/ToolCalling.cs`):

C# naudoja `ChatTool.CreateFunctionTool` pagalbinę priemonę įrankiams aprašyti:

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

### Užduotis 4 — Įrankių kvietimo pokalbio srautas

Svarbu suprasti žinučių struktūrą. Štai pilnas srautas su `messages` masyvu kiekviename etape:

**1 etapas — pradinė užklausa:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**2 etapas — modelis atsako su tool_calls (ne turiniu):**
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

**3 etapas — jūs pridedate asistentų žinutę IR įrankio rezultatą:**
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

**4 etapas — modelis suformuoja galutinį atsakymą panaudodamas įrankio rezultatą.**

> **Svarbu:** Įrankio žinutės `tool_call_id` turi atitikti įrankio kvietimo `id`. Tokiu būdu modelis susieja rezultatus su užklausomis.

---

### Užduotis 5 — Keli įrankių kvietimai

Modelis gali prašyti keletos įrankių kvietimų viename atsakyme. Pakeiskite vartotojo žinutę, kad suaktyvintumėte kelis kvietimus:

```python
# Python kalboje — pakeiskite vartotojo pranešimą:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// JavaScript — pakeiskite naudotojo žinutę:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Modelis turėtų pateikti du `tool_calls` — vieną `get_weather` ir kitą `get_population`. Jūsų kodas tai jau palaiko, nes cikliškai apdoroja visus kvietimus.

> **Išbandykite:** Pakeiskite vartotojo žinutę ir paleiskite dar kartą. Ar modelis kviečia abu įrankius?

---

### Užduotis 6 — Pridėkite savo įrankį

Išplėskite vieną iš pavyzdžių nauju įrankiu. Pavyzdžiui, pridėkite įrankį `get_time`:

1. Aprašykite įrankio schemą:
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

2. Pridėkite vykdymo logiką:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Tikroje programoje naudokite laiko juostos biblioteką
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... esami įrankiai ...
```

3. Pridėkite įrankį į `tools` masyvą ir išbandykite su klausimu: "Kiek dabar laiko Tokijuje?"

> **Iššūkis:** Sukurkite įrankį, atliekantį skaičiavimą, pavyzdžiui, `convert_temperature`, kuris konvertuoja Fahrenheit į Celsijų. Išbandykite: "Konvertuoti 100°F į Celsijų."

---

### Užduotis 7 — Įrankių kvietimas su SDK ChatClient (JavaScript)

JavaScript pavyzdyje jau naudojamas SDK vietinis `ChatClient`, o ne OpenAI SDK. Tai patogumo funkcija, nebereikalaujanti jums kurti OpenAI kliento:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient sukuriamas tiesiogiai iš modelio objekto
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat kaip antrą parametrą priima įrankius
const response = await chatClient.completeChat(messages, tools);
```

Palyginkite su Python pavyzdžiu, kuriame aiškiai naudojamas OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Abi schemos galioja. `ChatClient` patogesnis; OpenAI SDK suteikia prieigą prie visų OpenAI parametrų.

> **Pabandykite:** Pakeiskite JavaScript pavyzdį naudoti OpenAI SDK vietoje `ChatClient`. Reikės `import OpenAI from "openai"` ir kliento sukurimo su „manager.urls[0]“ galiniu tašku.

---

### Užduotis 8 — Suprasti `tool_choice`

`tool_choice` parametras kontroliuoja, ar modelis *privalo* naudoti įrankį, ar gali laisvai rinktis:

| Reikšmė | Elgsena |
|---------|----------|
| `"auto"` | Modelis sprendžia, ar kviesti įrankį (numatytoji) |
| `"none"` | Modelis nekvies jokių įrankių, net jei jie pateikti |
| `"required"` | Modelis turi kviesti bent vieną įrankį |
| `{"type": "function", "function": {"name": "get_weather"}}` | Modelis turi kviesti nurodytą įrankį |

Išbandykite kiekvieną variantą Python pavyzdyje:

```python
# Priversti modelį iškviesti get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Pastaba:** Ne visi `tool_choice` parametrai palaikomi kiekvieno modelio. Jei modelis nepalaiko `"required"`, jis gali ignoruoti nustatymą ir veikti kaip `"auto"`.

---

## Dažnos problemos

| Problema | Sprendimas |
|----------|------------|
| Modelis niekada nekviečia įrankių | Įsitikinkite, kad naudojate modelį, palaikantį įrankių kvietimą (pvz., qwen2.5-0.5b). Patikrinkite aukščiau lentelę. |
| `tool_call_id` neatitinka | Visada naudokite `id` iš įrankio kvietimo atsakymo, ne kietai užkoduotą reikšmę |
| Modelis grąžina neteisingą JSON argumentuose | Mažesni modeliai kartais sukuria netinkamą JSON. Naudokite `JSON.parse()` su try/catch bloku |
| Modelis kviečia neegzistuojantį įrankį | Pridėkite numatytą tvarkyklę savo `execute_tool` funkcijoje |
| Begalinis įrankių kvietimų ciklas | Nustatykite maksimalią ratų skaičių (pvz., 5), kad išvengtumėte begalinių ciklų |

---

## Pagrindinės išvados

1. **Įrankių kvietimas** leidžia modeliams prašyti funkcijų vykdymo vietoje spėjant atsakymus
2. Modelis **niekada nevykdo kodo**; aplikacija sprendžia, ką vykdyti
3. Įrankiai aprašomi kaip **JSON Schema** objektai, laikantis OpenAI funkcijų kvietimo formato
4. Pokalbis vyksta **daugkartiniu modeliu**: vartotojas, asistentas (tool_calls), įrankis (rezultatai), asistentas (galutinis atsakymas)
5. Visada naudokite **modelį, palaikantį įrankių kvietimą** (Qwen 2.5, Phi-4-mini)
6. SDK `createChatClient()` suteikia patogią galimybę siųsti įrankių kvietimus be OpenAI kliento kūrimo

---

Tęskite į [12 dalį: Web UI kūrimas Zava Creative Writer](part12-zava-ui.md), kad pridėtumėte naršyklės sąsają daugiaagentinei sekai su realaus laiko transliacija.

---

[← 10 dalis: Individualūs modeliai](part10-custom-models.md) | [12 dalis: Zava Writer UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:  
Šis dokumentas buvo išverstas naudojant dirbtinio intelekto vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors siekiame tikslumo, atkreipkite dėmesį, kad automatiniai vertimai gali turėti klaidų ar netikslumų. Originalus dokumentas gimtąja kalba turėtų būti laikomas autoritetingu šaltiniu. Kritinei informacijai rekomenduojamas profesionalus žmogaus vertimas. Mes neatsakome už bet kokius nesusipratimus ar klaidingus vertimus, kylančius iš šio vertimo naudojimo.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->