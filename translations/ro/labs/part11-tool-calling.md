![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partea 11: Apelarea Uneltelor cu Modele Locale

> **Obiectiv:** Permite modelului tău local să apeleze funcții externe (unelte) pentru a putea obține date în timp real, efectua calcule sau interacționa cu API-uri — toate rulând privat pe dispozitivul tău.

## Ce este Apelarea Uneltelor?

Apelarea uneltelor (cunoscută și ca **apelarea funcțiilor**) permite unui model de limbaj să solicite execuția funcțiilor pe care le definești. În loc să ghicească un răspuns, modelul recunoaște când o unealtă ar fi de ajutor și returnează o solicitare structurată pentru codul tău să o execute. Aplicația ta rulează funcția, trimite rezultatul înapoi, iar modelul încorporează acea informație în răspunsul final.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Acest tipar este esențial pentru a construi agenți care pot:

- **Căuta date live** (vreme, prețuri de acțiuni, interogări de baze de date)
- **Efectua calcule precise** (matematică, conversii de unități)
- **Întreprinde acțiuni** (trimite emailuri, creează tichete, actualizează înregistrări)
- **Accesa sisteme private** (API-uri interne, sisteme de fișiere)

---

## Cum Funcționează Apelarea Uneltelor

Fluxul apelării uneltelor are patru etape:

| Etapă | Ce se întâmplă |
|-------|----------------|
| **1. Definirea uneltelor** | Descrii funcțiile disponibile folosind JSON Schema — nume, descriere și parametri |
| **2. Modelul decide** | Modelul primește mesajul tău plus definițiile uneltelor. Dacă o unealtă ar ajuta, returnează un răspuns `tool_calls` în loc de un răspuns text |
| **3. Execută local** | Codul tău analizează apelul uneală, rulează funcția și colectează rezultatul |
| **4. Răspuns final** | Trimiți rezultatul uneală înapoi modelului, care produce răspunsul final |

> **Aspect cheie:** Modelul nu execută niciodată cod. El doar *solicită* ca o unealtă să fie apelată. Aplicația ta decide dacă onorează acea solicitare — astfel rămâi în control total.

---

## Care Modele Suportă Apelarea Uneltelor?

Nu toate modelele suportă apelarea uneltelor. În catalogul curent Foundry Local, următoarele modele au această funcționalitate:

| Model | Mărime | Apelare Unelte |
|-------|--------|:-------------:|
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

> **Sfat:** Pentru acest laborator folosim **qwen2.5-0.5b** — este mic (822 MB de descărcat), rapid și are suport fiabil pentru apelarea uneltelor.

---

## Obiective de Învățare

La finalul acestui laborator vei putea să:

- Explici tiparul de apelare a uneltelor și de ce este important pentru agenții AI
- Defini schemele uneltelor folosind formatul de apelare a funcțiilor OpenAI
- Gestionezi fluxul conversațional multi-turn pentru apelarea uneltelor
- Execuți apeluri de unelte local și trimiți rezultatele modelului
- Alegi modelul potrivit pentru scenariile de apelare unelte

---

## Precondiții

| Cerință | Detalii |
|---------|---------|
| **Foundry Local CLI** | Instalată și în `PATH` ([Partea 1](part1-getting-started.md)) |
| **Foundry Local SDK** | SDK Python, JavaScript sau C# instalat ([Partea 2](part2-foundry-local-sdk.md)) |
| **Un model care suportă apelarea uneltelor** | qwen2.5-0.5b (va fi descărcat automat) |

---

## Exerciții

### Exercițiul 1 — Înțelegerea Fluxului de Apelare a Uneltelor

Înainte să scrii cod, studiază acest diagramă de secvență:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Observații cheie:**

1. Definesti uneltele inițial ca obiecte JSON Schema
2. Răspunsul modelului conține `tool_calls` în loc de conținut obișnuit
3. Fiecare apel unealtă are un `id` unic la care trebuie să faci referire când trimiți rezultatul înapoi
4. Modelul vede toate mesajele anterioare *plus* rezultatele uneltelor când generează răspunsul final
5. Mai multe apeluri la unelte pot avea loc într-un singur răspuns

> **Discuție:** De ce returnează modelul apeluri la unelte în loc să execute funcțiile direct? Ce avantaje de securitate oferă aceasta?

---

### Exercițiul 2 — Definirea Schemelor Uneltelor

Uneltele sunt definite folosind formatul standard OpenAI de apelare a funcțiilor. Fiecare unealtă necesită:

- **`type`**: întotdeauna `"function"`
- **`function.name`**: un nume descriptiv pentru funcție (de ex. `get_weather`)
- **`function.description`**: o descriere clară — modelul o folosește pentru a decide când să apeleze unealta
- **`function.parameters`**: un obiect JSON Schema care descrie argumentele așteptate

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

> **Cele mai bune practici pentru descrierile uneltelor:**
> - Fii specific: "Obține vremea curentă pentru un oraș" este mai bun decât "Obține vremea"
> - Descrie clar parametrii: modelul citește aceste descrieri pentru a completa corect valorile
> - Marchează parametrii obligatorii vs opționali — ajută modelul să decidă ce trebuie să solicite

---

### Exercițiul 3 — Rulează Exemplele de Apelare a Uneltelor

Fiecare exemplu de limbaj definește două unelte (`get_weather` și `get_population`), trimite o întrebare care declanșează folosirea uneltelor, execută unealta local și trimite rezultatul înapoi pentru un răspuns final.

<details>
<summary><strong>🐍 Python</strong></summary>

**Precondiții:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Rulează:**
```bash
python foundry-local-tool-calling.py
```

**Rezultat așteptat:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Explicație cod** (`python/foundry-local-tool-calling.py`):

```python
# Define uneltele ca o listă de scheme de funcții
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

# Trimite împreună cu uneltele — modelul poate returna apeluri către unelte în loc de conținut
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Verifică dacă modelul dorește să apeleze o unealtă
if response.choices[0].message.tool_calls:
    # Execută unealta și trimite rezultatul înapoi
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Precondiții:**
```bash
cd javascript
npm install
```

**Rulează:**
```bash
node foundry-local-tool-calling.mjs
```

**Rezultat așteptat:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Explicație cod** (`javascript/foundry-local-tool-calling.mjs`):

Acest exemplu folosește `ChatClient` nativ din Foundry Local SDK, nu OpenAI SDK, demonstrând metoda convenabilă `createChatClient()`:

```javascript
// Obțineți un ChatClient direct din obiectul modelului
const chatClient = model.createChatClient();

// Trimiteți cu unelte — ChatClient gestionează formatul compatibil cu OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Verificați pentru apeluri către unelte
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Executați uneltele și trimiteți rezultatele înapoi
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Precondiții:**
```bash
cd csharp
dotnet restore
```

**Rulează:**
```bash
dotnet run toolcall
```

**Rezultat așteptat:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Explicație cod** (`csharp/ToolCalling.cs`):

C# folosește ajutorul `ChatTool.CreateFunctionTool` pentru a defini uneltele:

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

### Exercițiul 4 — Fluxul Conversațional pentru Apelare Unelte

Înțelegerea structurii mesajelor este critică. Iată fluxul complet, arătând matricea `messages` la fiecare etapă:

**Etapa 1 — Solicitarea inițială:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Etapa 2 — Modelul răspunde cu `tool_calls` (nu conținut):**
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

**Etapa 3 — Adaugi mesajul asistentului ȘI rezultatul uneală:**
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

**Etapa 4 — Modelul produce răspunsul final folosind rezultatul uneală.**

> **Important:** `tool_call_id` din mesajul unealtă trebuie să corespundă cu `id` din apelul unealtă. Astfel, modelul asociază rezultatele cu solicitările.

---

### Exercițiul 5 — Apeluri Multiple la Unelte

Un model poate solicita mai multe apeluri la unelte într-un singur răspuns. Încearcă să modifici mesajul utilizatorului pentru a declanșa apeluri multiple:

```python
# În Python — schimbă mesajul utilizatorului:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// În JavaScript — modifică mesajul utilizatorului:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Modelul ar trebui să returneze două `tool_calls` — unul pentru `get_weather` și unul pentru `get_population`. Codul tău gestionează deja asta pentru că parcurge toate apelurile unealtelor.

> **Încearcă:** Modifică mesajul utilizatorului și rulează din nou exemplul. Modelul apelează ambele unelte?

---

### Exercițiul 6 — Adaugă o Unealtă Proprie

Extinde unul din exemple cu o unealtă nouă. De exemplu, adaugă o unealtă `get_time`:

1. Defineste schema uneltei:
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

2. Adaugă logica de execuție:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Într-o aplicație reală, folosiți o bibliotecă pentru fusul orar
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... unelte existente ...
```

3. Adaugă unealta în matricea `tools` și testează cu: "Cât este ceasul în Tokyo?"

> **Provocare:** Adaugă o unealtă care efectuează un calcul, de exemplu `convert_temperature` care face conversia între Celsius și Fahrenheit. Testeaz-o cu: "Convertește 100°F în Celsius."

---

### Exercițiul 7 — Apelarea Uneltelor cu ChatClient-ul SDK (JavaScript)

Exemplul JavaScript deja folosește `ChatClient` nativ din SDK în locul OpenAI SDK. Aceasta este o metodă convenabilă care elimină necesitatea de a construi manual un client OpenAI:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient este creat direct din obiectul model
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat acceptă uneltele ca al doilea parametru
const response = await chatClient.completeChat(messages, tools);
```

Compară asta cu abordarea în Python care folosește explicit OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Ambele tipare sunt valide. `ChatClient` este mai convenabil; OpenAI SDK oferă acces la gama completă de parametri OpenAI.

> **Încearcă:** Modifică exemplul JavaScript să folosească OpenAI SDK în loc de `ChatClient`. Vei avea nevoie de `import OpenAI from "openai"` și să construiești clientul cu endpoint-ul din `manager.urls[0]`.

---

### Exercițiul 8 — Înțelegerea parametrului tool_choice

Parametrul `tool_choice` controlează dacă modelul *trebuie* să folosească o unealtă sau poate alege liber:

| Valoare | Comportament |
|---------|--------------|
| `"auto"` | Modelul decide dacă apelează o unealtă (implicit) |
| `"none"` | Modelul nu va apela unelte, nici dacă sunt disponibile |
| `"required"` | Modelul trebuie să apeleze cel puțin o unealtă |
| `{"type": "function", "function": {"name": "get_weather"}}` | Modelul trebuie să apeleze unealta specificată |

Încearcă fiecare opțiune în exemplul Python:

```python
# Forțează modelul să apeleze get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Notă:** Nu toate opțiunile `tool_choice` sunt suportate de fiecare model. Dacă un model nu suportă `"required"`, poate ignora setarea și să se comporte ca `"auto"`.

---

## Capcane Comune

| Problemă | Soluție |
|----------|---------|
| Modelul nu apelează niciodată uneltele | Asigură-te că folosești un model care suportă apelarea uneltelor (ex. qwen2.5-0.5b). Consultă tabelul de mai sus. |
| Potrivire greșită `tool_call_id` | Folosește întotdeauna `id` din răspunsul apelului unealtă, nu o valoare hardcodată |
| Modelul returnează JSON rău format în `arguments` | Modelele mai mici uneori produc JSON invalid. Învelește `JSON.parse()` într-un try/catch |
| Modelul apelează o unealtă care nu există | Adaugă un handler implicit în funcția ta `execute_tool` |
| Bucle infinite de apelare unelte | Setează un număr maxim de runde (ex. 5) pentru a preveni bucle infinite |

---

## Concluzii Cheie

1. **Apelarea uneltelor** permite modelelor să solicite execuția funcțiilor în loc să ghicească răspunsuri
2. Modelul **nu execută niciodată cod**; aplicația ta decide ce se execută
3. Uneltele sunt definite ca obiecte **JSON Schema** urmând formatul OpenAI de apelare funcții
4. Conversația folosește un **tipar multi-turn**: utilizator, apoi asistent (tool_calls), apoi unealtă (rezultate), apoi asistent (răspuns final)
5. Folosește întotdeauna **un model care suportă apelarea uneltelor** (Qwen 2.5, Phi-4-mini)
6. `createChatClient()` din SDK oferă o metodă convenabilă de a face cereri de apelare unelte fără a construi un client OpenAI

---

Continuă cu [Partea 12: Construirea unei UI Web pentru Zava Creative Writer](part12-zava-ui.md) pentru a adăuga o interfață browser-based la pipeline-ul multi-agent cu streaming în timp real.

---

[← Partea 10: Modele Personalizate](part10-custom-models.md) | [Partea 12: Interfață Zava Writer →](part12-zava-ui.md)