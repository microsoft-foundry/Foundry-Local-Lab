![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 11: Klicanje orodij z lokalnimi modeli

> **Cilj:** Omogočiti vašemu lokalnemu modelu klicanje zunanjih funkcij (orodij), da lahko pridobi podatke v realnem času, izvede izračune ali komunicira z API-ji — vse to varno na vaši napravi.

## Kaj je klicanje orodij?

Klicanje orodij (imenovano tudi **klicanje funkcij**) omogoča jezikovnemu modelu, da zahteva izvajanje funkcij, ki jih definirate. Namesto da bi model ugibal odgovor, prepozna, kdaj bi orodje pomagalo, in vrne strukturirano zahtevo, da vaša koda izvede funkcijo. Vaša aplikacija izvrši funkcijo, pošlje rezultat nazaj, model pa to informacijo vključi v končni odgovor.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Ta vzorec je ključen za gradnjo agentov, ki lahko:

- **Preiskujejo žive podatke** (vreme, cene delnic, poizvedbe v bazi)
- **Izvedejo natančne izračune** (matematika, pretvorba enot)
- **Izvedejo dejanja** (pošiljanje elektronskih sporočil, ustvarjanje zahtevkov, posodabljanje zapisov)
- **Dostopajo do zasebnih sistemov** (notranji API-ji, datotečni sistemi)

---

## Kako deluje klicanje orodij

Proces klicanja orodij ima štiri faze:

| Faza | Kaj se zgodi |
|------|--------------|
| **1. Določanje orodij** | Opisujete razpoložljive funkcije z uporabo JSON Sheme — ime, opis in parametri |
| **2. Odločitev modela** | Model prejme vaše sporočilo in definicije orodij. Če orodje pomaga, vrne odgovor `tool_calls` namesto besedilnega odgovora |
| **3. Izvajanje lokalno** | Vaša koda pregleda klic orodja, izvede funkcijo in zbere rezultat |
| **4. Končni odgovor** | Pošljete rezultat orodja nazaj modelu, ki ustvari svoj končni odgovor |

> **Ključna točka:** Model nikoli ne izvrši kode. Samo *zahteva*, da se orodje pokliče. Vaša aplikacija odloča, ali bo zahtevo izpolnila — s tem imate poln nadzor.

---

## Kateri modeli podpirajo klicanje orodij?

Ne podpira vsak model klicanja orodij. V trenutnem katalogu Foundry Local imajo naslednji modeli možnost klicanja orodij:

| Model | Velikost | Klicanje orodij |
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

> **Namig:** Za ta laboratorij uporabljamo **qwen2.5-0.5b** — je majhen (822 MB prenos), hiter in ima zanesljivo podporo klicanja orodij.

---

## Cilji učenja

Do konca tega laboratorija boste znali:

- Razložiti vzorec klicanja orodij in zakaj je pomemben za AI agente
- Določiti sheme orodij z uporabo OpenAI formata klicanja funkcij
- Obvladovati večkratni tok pogovora pri klicanju orodij
- Izvajati klice orodij lokalno in vračati rezultate modelu
- Izbrati pravilen model za scenarije klicanja orodij

---

## Zahteve

| Zahteva | Podrobnosti |
|---------|-------------|
| **Foundry Local CLI** | Nameščen in na vaši `PATH` ([Del 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript ali C# SDK nameščen ([Del 2](part2-foundry-local-sdk.md)) |
| **Model, ki podpira klicanje orodij** | qwen2.5-0.5b (samodejno se bo prenesel) |

---

## Vaje

### Vaja 1 — Razumevanje poteka klicanja orodij

Pred pisanjem kode si oglejte diagram zaporedja:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Ključne opazke:**

1. Orodja vnaprej definirate kot JSON Schema objekte
2. Odgovor modela vsebuje `tool_calls` namesto običajne vsebine
3. Vsak klic orodja ima edinstven `id`, na katerega se morate sklicevati pri vračanju rezultatov
4. Model vidi vsa prejšnja sporočila *plus* rezultate orodij pri generiranju končnega odgovora
5. Več klicev orodij se lahko zgodi v enem samem odgovoru

> **Diskusija:** Zakaj model vrača klice orodij namesto neposrednega izvajanja funkcij? Katere varnostne prednosti to prinaša?

---

### Vaja 2 — Določanje shem orodij

Orodja definiramo z uporabo standardnega OpenAI formata za klicanje funkcij. Vsako orodje zahteva:

- **`type`**: Vedno `"function"`
- **`function.name`**: Opisno ime funkcije (npr. `get_weather`)
- **`function.description`**: Jasna razlaga — model to uporablja za odločitev, kdaj poklicati orodje
- **`function.parameters`**: JSON Schema objekt, ki opisuje pričakovane argumente

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

> **Dobri nasveti za opise orodij:**
> - Bodite specifični: "Pridobi trenutno vreme za določen kraj" je boljše kot "Pridobi vreme"
> - Jasno opišite parametre: model prebere te opise za pravilen vnos vrednosti
> - Oznaka obveznih in neobveznih parametrov — pomaga modelu odločiti, kaj vprašati

---

### Vaja 3 — Zaženite primere klicanja orodij

Vsak vzorec v jeziku definira dve orodji (`get_weather` in `get_population`), pošlje vprašanje, ki sproži uporabo orodja, to lokalno izvede in rezultat pošlje nazaj za končni odgovor.

<details>
<summary><strong>🐍 Python</strong></summary>

**Zahteve:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Zaženi:**
```bash
python foundry-local-tool-calling.py
```

**Pričakovani izhod:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Pregled kode** (`python/foundry-local-tool-calling.py`):

```python
# Orodja definirajte kot seznam shem funkcij
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

# Pošljite z orodji — model lahko namesto vsebine vrne klice orodij
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Preverite, ali model želi poklicati orodje
if response.choices[0].message.tool_calls:
    # Izvedite orodje in pošljite rezultat nazaj
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Zahteve:**
```bash
cd javascript
npm install
```

**Zaženi:**
```bash
node foundry-local-tool-calling.mjs
```

**Pričakovani izhod:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Pregled kode** (`javascript/foundry-local-tool-calling.mjs`):

Ta primer uporablja nativni Foundry Local SDK `ChatClient` namesto OpenAI SDK, kar prikazuje priročnost metode `createChatClient()`:

```javascript
// Pridobite ChatClient neposredno iz modela objekta
const chatClient = model.createChatClient();

// Pošljite z orodji — ChatClient obravnava format združljiv z OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Preverite klice orodij
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Izvedite orodja in pošljite rezultate nazaj
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Zahteve:**
```bash
cd csharp
dotnet restore
```

**Zaženi:**
```bash
dotnet run toolcall
```

**Pričakovani izhod:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Pregled kode** (`csharp/ToolCalling.cs`):

C# uporablja pomočnika `ChatTool.CreateFunctionTool` za definicijo orodij:

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

### Vaja 4 — Potek pogovora pri klicanju orodij

Razumevanje strukture sporočil je ključno. Tukaj je celoten potek, ki prikazuje polje `messages` v vsaki fazi:

**Faza 1 — Začetna zahteva:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Faza 2 — Model odgovori z `tool_calls` (namesto vsebine):**
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

**Faza 3 — Dodate sporočilo asistenta IN rezultat orodja:**
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

**Faza 4 — Model ustvari končni odgovor z uporabo rezultata orodja.**

> **Pomembno:** `tool_call_id` v sporočilu orodja mora ustrezati `id` iz klica orodja. Tako model povezuje rezultate z zahtevami.

---

### Vaja 5 — Več klicev orodij

Model lahko v enem odgovoru zahteva več klicev orodij. Poskusite spremeniti uporabnikovo sporočilo, da sprožite več klicev:

```python
# V Pythonu — spremeni uporabniško sporočilo:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// V JavaScript-u — spremenite uporabniško sporočilo:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Model naj vrne dva `tool_calls` — enega za `get_weather` in enega za `get_population`. Vaša koda to že podpira, ker prehaja skozi vse klice orodij.

> **Preizkusite:** Spremenite uporabnikovo sporočilo in znova zaženite vzorec. Ali model kliče obe orodji?

---

### Vaja 6 — Dodajte svoje orodje

Razširite enega od vzorcev z novim orodjem. Na primer dodajte orodje `get_time`:

1. Določite shemo orodja:
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

2. Dodajte izvršilno logiko:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # V pravi aplikaciji uporabite knjižnico za časovni pas
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... obstoječa orodja ...
```

3. Dodajte orodje v polje `tools` in testirajte z vprašanjem: "Koliko je ura v Tokiu?"

> **Izziv:** Dodajte orodje, ki izvede izračun, na primer `convert_temperature`, ki pretvarja med Celzijem in Fahrenheiti. Preizkusite z vprašanjem: "Pretvori 100°F v Celzija."

---

### Vaja 7 — Klicanje orodij z SDK ChatClient (JavaScript)

JavaScript primer že uporablja nativni SDK `ChatClient` namesto OpenAI SDK. Gre za priročno funkcijo, ki odstrani potrebo po ročni konstrukciji OpenAI klienta:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient je ustvarjen neposredno iz modelnega objekta
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat kot drugi parameter sprejme orodja
const response = await chatClient.completeChat(messages, tools);
```

Primerjajte s pristopom v Pythonu, ki eksplicitno uporablja OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Oba vzorca sta pravilna. `ChatClient` je bolj priročen; OpenAI SDK omogoča dostop do celotnega nabora OpenAI parametrov.

> **Preizkusite:** Spremenite JavaScript primer, da uporablja OpenAI SDK namesto `ChatClient`. Potrebovali boste `import OpenAI from "openai"` in konstrukcijo klienta z endpointom iz `manager.urls[0]`.

---

### Vaja 8 — Razumevanje `tool_choice`

Parameter `tool_choice` nadzira, ali model *mora* uporabiti orodje ali ima prosto izbiro:

| Vrednost | Vedenje |
|----------|---------|
| `"auto"` | Model sam odloči, ali bo poklical orodje (privzeto) |
| `"none"` | Model ne bo poklical nobenega orodja, tudi če so na voljo |
| `"required"` | Model mora poklicati vsaj eno orodje |
| `{"type": "function", "function": {"name": "get_weather"}}` | Model mora poklicati določeno orodje |

Preizkusite vsako možnost v Python vzorcu:

```python
# Prisilite model, da pokliče get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Opomba:** Vse možnosti `tool_choice` morda niso podprte pri vsakem modelu. Če model ne podpira `"required"`, lahko nastavitve ignorira in se vede kot `"auto"`.

---

## Pogoste pasti

| Težava | Rešitev |
|---------|---------|
| Model nikoli ne kliče orodij | Preverite, da uporabljate model, ki podpira klicanje orodij (npr. qwen2.5-0.5b). Oglejte si tabelo zgoraj. |
| Neskladje `tool_call_id` | Vedno uporabite `id` iz odgovora klica orodja, ne trdo kodirane vrednosti |
| Model vrne neveljaven JSON v `arguments` | Manjši modeli občasno proizvedejo neveljaven JSON. Zavijte `JSON.parse()` v try/catch blok |
| Model kliče orodje, ki ne obstaja | Dodajte privzeti upravljalnik v vašo funkcijo `execute_tool` |
| Neskončna zanka klicanja orodij | Nastavite maksimalno število krogov (npr. 5), da preprečite zanke brez konca |

---

## Ključne ugotovitve

1. **Klicanje orodij** omogoča modelom, da zahtevajo izvajanje funkcij namesto ugibanja odgovorov  
2. Model **nikoli ne izvrši kode**; vaša aplikacija odloča, kaj se izvaja  
3. Orodja so definirana kot **JSON Schema** objekti, ki sledijo OpenAI formatu klicanja funkcij  
4. Pogovor uporablja **večkratni vzorec**: uporabnik, nato asistent (tool_calls), potem orodje (rezultati), nato asistent (končni odgovor)  
5. Vedno uporabljajte **model, ki podpira klicanje orodij** (Qwen 2.5, Phi-4-mini)  
6. SDK funkcija `createChatClient()` ponuja priročen način za klice orodij brez gradnje lastnega OpenAI klienta

---

Nadaljujte na [Del 12: Gradnja spletnega vmesnika za Zava Creative Writer](part12-zava-ui.md) in dodajte brskalniški vmesnik večagentnemu procesu z realnočasnim pretakanjem.

---

[← Del 10: Lastni modeli](part10-custom-models.md) | [Del 12: Zava Writer UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Omejitev odgovornosti**:
Ta dokument je bil preveden z uporabo AI prevajalske storitve [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, vas prosimo, da upoštevate, da avtomatski prevodi lahko vsebujejo napake ali netočnosti. Izvirni dokument v maternem jeziku velja za avtoritativni vir. Za kritične informacije priporočamo strokovni človeški prevod. Za morebitna nesporazume ali napačne razlage, ki izhajajo iz uporabe tega prevoda, ne odgovarjamo.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->