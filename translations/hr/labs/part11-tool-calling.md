![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Dio 11: Pozivanje alata s lokalnim modelima

> **Cilj:** Omogućite svom lokalnom modelu da poziva vanjske funkcije (alate) kako bi mogao dohvatiti podatke u stvarnom vremenu, izvoditi izračune ili komunicirati s API-jima — sve to privatno na vašem uređaju.

## Što je pozivanje alata?

Pozivanje alata (poznato i kao **pozivanje funkcija**) omogućuje jezičnom modelu da zatraži izvršavanje funkcija koje definirate. Umjesto da pogađa odgovor, model prepoznaje kada bi alat mogao pomoći i vraća strukturirani zahtjev da vaš kod izvrši funkciju. Vaša aplikacija izvršava funkciju, šalje rezultat natrag, a model uključuje te informacije u svoj konačni odgovor.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Ovaj obrazac je ključan za izradu agenata koji mogu:

- **Dohvatiti žive podatke** (vrijeme, cijene dionica, upiti u bazu podataka)
- **Izvršavati precizne izračune** (matematiku, pretvorbu jedinica)
- **Poduzimati radnje** (slanje elektroničke pošte, kreiranje tiketova, ažuriranje zapisa)
- **Pristupati privatnim sustavima** (interni API-ji, datotečni sustavi)

---

## Kako funkcionira pozivanje alata

Proces pozivanja alata ima četiri faze:

| Faza | Što se događa |
|-------|--------------|
| **1. Definirajte alate** | Opisujete dostupne funkcije koristeći JSON Schema — naziv, opis i parametri |
| **2. Model odlučuje** | Model prima vašu poruku plus definicije alata. Ako alat može pomoći, vraća odgovor `tool_calls` umjesto tekstualnog odgovora |
| **3. Izvršavanje lokalno** | Vaš kod obrađuje poziv alata, izvršava funkciju i prikuplja rezultat |
| **4. Konačni odgovor** | Šaljete rezultat alata modelu, koji generira konačni odgovor |

> **Ključna točka:** Model nikad ne izvršava kod. On samo *zahtijeva* da se alat pozove. Vaša aplikacija odlučuje hoće li prihvatiti zahtjev — to vam daje potpunu kontrolu.

---

## Koji modeli podržavaju pozivanje alata?

Nisu svi modeli podržavaju pozivanje alata. U trenutnom Foundry Local katalogu sljedeći modeli imaju tu mogućnost:

| Model | Veličina | Pozivanje alata |
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

> **Savjet:** Za ovaj laboratorij koristimo **qwen2.5-0.5b** — mali je (822 MB), brz i ima pouzdanu podršku za pozivanje alata.

---

## Ciljevi učenja

Do kraja ovog laboratorija moći ćete:

- Objasniti obrazac pozivanja alata i zašto je važan za AI agente
- Definirati sheme alata pomoću OpenAI formata za pozivanje funkcija
- Upravljati višekratnim tijekovima pozivanja alata u razgovoru
- Izvršavati pozive alata lokalno i vraćati rezultate modelu
- Odabrati pravi model za scenarije pozivanja alata

---

## Preduvjeti

| Zahtjev | Detalji |
|---------|---------|
| **Foundry Local CLI** | Instaliran i dodan u vaš `PATH` ([Dio 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Instaliran Python, JavaScript ili C# SDK ([Dio 2](part2-foundry-local-sdk.md)) |
| **Model s podrškom za pozivanje alata** | qwen2.5-0.5b (povučen automatski) |

---

## Vježbe

### Vježba 1 — Razumjeti tijek pozivanja alata

Prije pisanja koda proučite dijagram toka:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Ključne primjedbe:**

1. Definirate alate unaprijed kao JSON Schema objekte
2. Odgovor modela sadrži `tool_calls` umjesto običnog sadržaja
3. Svaki poziv alata ima jedinstveni `id` na koji se morate pozvati prilikom vraćanja rezultata
4. Model vidi sve prethodne poruke *plus* rezultate alata prilikom generiranja konačnog odgovora
5. U jednom odgovoru može biti više poziva alata

> **Rasprava:** Zašto model vraća pozive alata umjesto da izravno izvrši funkcije? Koje sigurnosne prednosti to pruža?

---

### Vježba 2 — Definiranje shema alata

Alati se definiraju korištenjem standardnog OpenAI formata za pozivanje funkcija. Svaki alat treba:

- **`type`**: Uvijek `"function"`
- **`function.name`**: Opisni naziv funkcije (npr. `get_weather`)
- **`function.description`**: Jasan opis — model koristi to da odluči kada pozvati alat
- **`function.parameters`**: JSON Schema objekt koji opisuje očekivane argumente

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

> **Najbolje prakse za opise alata:**
> - Budite specifični: "Dohvati trenutno vrijeme za određeni grad" je bolje od "Dohvati vrijeme"
> - Jasno opišite parametre: model čita opise kako bi ispravno popunio vrijednosti
> - Označite obavezne i opcionalne parametre — to pomaže modelu prilikom odlučivanja što tražiti

---

### Vježba 3 — Pokrenite primjere pozivanja alata

Svaki primjer na jeziku definira dva alata (`get_weather` i `get_population`), šalje pitanje koje pokreće korištenje alata, izvršava alat lokalno i šalje rezultat natrag za konačni odgovor.

<details>
<summary><strong>🐍 Python</strong></summary>

**Preduvjeti:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Pokrenite:**
```bash
python foundry-local-tool-calling.py
```

**Očekivani izlaz:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Objašnjenje koda** (`python/foundry-local-tool-calling.py`):

```python
# Definirajte alate kao popis shema funkcija
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

# Pošaljite s alatima — model može vratiti tool_calls umjesto sadržaja
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Provjerite želi li model pozvati alat
if response.choices[0].message.tool_calls:
    # Izvršite alat i pošaljite rezultat natrag
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Preduvjeti:**
```bash
cd javascript
npm install
```

**Pokrenite:**
```bash
node foundry-local-tool-calling.mjs
```

**Očekivani izlaz:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Objašnjenje koda** (`javascript/foundry-local-tool-calling.mjs`):

Ovaj primjer koristi nativni Foundry Local SDK `ChatClient` umjesto OpenAI SDK, što pokazuje praktičnost metode `createChatClient()`:

```javascript
// Dohvati ChatClient izravno iz objekta modela
const chatClient = model.createChatClient();

// Pošalji s alatima — ChatClient obrađuje format kompatibilan s OpenAI-jem
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Provjeri pozive alata
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Izvrši alate i pošalji rezultate natrag
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Preduvjeti:**
```bash
cd csharp
dotnet restore
```

**Pokrenite:**
```bash
dotnet run toolcall
```

**Očekivani izlaz:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Objašnjenje koda** (`csharp/ToolCalling.cs`):

C# koristi pomoćnu funkciju `ChatTool.CreateFunctionTool` za definiranje alata:

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

### Vježba 4 — Tijek razgovora pozivanja alata

Razumijevanje strukture poruka je ključno. Evo cjelokupnog toka, prikazujući polje `messages` u svakoj fazi:

**Faza 1 — Početni zahtjev:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Faza 2 — Model odgovara s `tool_calls` (ne s sadržajem):**
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

**Faza 3 — Dodajete poruku asistenta I rezultat alata:**
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

**Faza 4 — Model proizvodi konačni odgovor koristeći rezultat alata.**

> **Važno:** `tool_call_id` u poruci alata mora odgovarati `id` iz poziva alata. Time model povezuje rezultate sa zahtjevima.

---

### Vježba 5 — Višestruki pozivi alata

Model može zatražiti nekoliko poziva alata u jednom odgovoru. Promijenite korisničku poruku da pokrenete više poziva:

```python
# U Pythonu — promijenite korisničku poruku:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// U JavaScriptu — promijeni korisničku poruku:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Model bi trebao vratiti dva `tool_calls` — jedan za `get_weather` i jedan za `get_population`. Vaš kod to već obrađuje jer prolazi kroz sve pozive alata.

> **Isprobajte:** Promijenite korisničku poruku i pokrenite primjer ponovno. Poziva li model oba alata?

---

### Vježba 6 — Dodajte vlastiti alat

Proširite jedan od primjera novim alatom. Na primjer, dodajte alat `get_time`:

1. Definirajte shemu alata:
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

2. Dodajte logiku izvršavanja:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # U stvarnoj aplikaciji koristite biblioteku za vremenske zone
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... postojeći alati ...
```

3. Dodajte alat u niz `tools` i testirajte upitom: "Koliko je sati u Tokiju?"

> **Izazov:** Dodajte alat koji izvodi izračun, primjerice `convert_temperature` koji pretvara između Celzijusa i Fahrenheita. Testirajte s: "Pretvori 100°F u Celzijuse."

---

### Vježba 7 — Pozivanje alata s SDK-ovim ChatClientom (JavaScript)

JavaScript primjer već koristi nativni `ChatClient` SDK umjesto OpenAI SDK. Ovo je praktična značajka koja uklanja potrebu da sami konstruirate OpenAI klijenta:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient se kreira direktno iz model objekta
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat prihvaća alate kao drugi parametar
const response = await chatClient.completeChat(messages, tools);
```

Usporedite to s Python pristupom koji eksplicitno koristi OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Oba obrasca su valjana. `ChatClient` je zgodniji; OpenAI SDK daje pristup svim parametrima OpenAI-ja.

> **Isprobajte:** Promijenite JavaScript primjer da koristi OpenAI SDK umjesto `ChatClient`. Trebat će vam `import OpenAI from "openai"` i konstrukcija klijenta s endpointom iz `manager.urls[0]`.

---

### Vježba 8 — Razumijevanje `tool_choice`

Parametar `tool_choice` kontrolira mora li model koristiti alat ili može slobodno birati:

| Vrijednost | Ponašanje |
|------------|-----------|
| `"auto"` | Model odlučuje hoće li pozvati alat (zadano) |
| `"none"` | Model neće pozivati nikakve alate, čak i ako su dostupni |
| `"required"` | Model mora pozvati barem jedan alat |
| `{"type": "function", "function": {"name": "get_weather"}}` | Model mora pozvati navedeni alat |

Isprobajte svaku opciju u Python primjeru:

```python
# Natjerajte model da pozove get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Napomena:** Nisu sve `tool_choice` opcije podržane od strane svakog modela. Ako model ne podržava `"required"`, može zanemariti postavku i ponašati se kao `"auto"`.

---

## Uobičajene pogreške

| Problem | Rješenje |
|---------|----------|
| Model nikad ne poziva alate | Provjerite koristite li model s podrškom za pozivanje alata (npr. qwen2.5-0.5b). Pogledajte tablicu iznad. |
| Neusklađenost `tool_call_id` | Uvijek koristite `id` iz odgovora na poziv alata, a ne fiksnu vrijednost |
| Model vraća neispravan JSON u `arguments` | Manji modeli ponekad generiraju neispravan JSON. Upakirajte `JSON.parse()` u try/catch |
| Model poziva alat koji ne postoji | Dodajte zadani handler u funkciji `execute_tool` |
| Beskonačna petlja pozivanja alata | Postavite maksimalan broj krugova (npr. 5) kako biste spriječili beskonačne petlje |

---

## Ključne spoznaje

1. **Pozivanje alata** omogućuje modelima da traže izvršavanje funkcija umjesto da pogađaju odgovore
2. Model **nikad ne izvršava kod**; vaša aplikacija odlučuje što se izvršava
3. Alati su definirani kao **JSON Schema** objekti prema OpenAI formatu za pozivanje funkcija
4. Razgovor koristi **višekratni obrazac**: korisnik, zatim asistent (tool_calls), zatim alat (rezultati), zatim asistent (konačni odgovor)
5. Uvijek koristite **model koji podržava pozivanje alata** (Qwen 2.5, Phi-4-mini)
6. SDK-ova metoda `createChatClient()` pruža praktičan način za slanje zahtjeva za pozivanje alata bez izgradnje OpenAI klijenta

---

Nastavite na [Dio 12: Izrada web sučelja za Zava kreativnog pisca](part12-zava-ui.md) kako biste dodali pregledničko sučelje višemodalnom agentu s streamingom u stvarnom vremenu.

---

[← Dio 10: Prilagođeni modeli](part10-custom-models.md) | [Dio 12: Zava Writer UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje od odgovornosti**:  
Ovaj je dokument preveden koristeći AI uslugu za prevođenje [Co-op Translator](https://github.com/Azure/co-op-translator). Iako težimo točnosti, imajte na umu da automatski prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na izvornom jeziku treba se smatrati službenim izvorom. Za kritične informacije preporučuje se profesionalni ljudski prijevod. Ne odgovaramo za bilo kakva nesporazume ili kriva tumačenja koja proizlaze iz korištenja ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->