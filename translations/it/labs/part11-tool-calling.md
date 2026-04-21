![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 11: Chiamata di Strumenti con Modelli Locali

> **Obiettivo:** Abilitare il tuo modello locale a chiamare funzioni esterne (strumenti) così da poter recuperare dati in tempo reale, eseguire calcoli o interagire con API — tutto in esecuzione privata sul tuo dispositivo.

## Cos’è la Chiamata di Strumenti?

La chiamata di strumenti (nota anche come **function calling**) permette a un modello linguistico di richiedere l’esecuzione di funzioni che definisci. Invece di indovinare una risposta, il modello riconosce quando uno strumento sarebbe utile e restituisce una richiesta strutturata da eseguire nel tuo codice. La tua applicazione esegue la funzione, invia il risultato e il modello incorpora quell’informazione nella risposta finale.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Questo schema è essenziale per costruire agenti che possono:

- **Consultare dati in tempo reale** (tempo, prezzi azionari, query di database)
- **Eseguire calcoli precisi** (matematica, conversioni di unità)
- **Prendere azioni** (inviare email, creare ticket, aggiornare record)
- **Accedere a sistemi privati** (API interne, file system)

---

## Come Funziona la Chiamata di Strumenti

Il flusso della chiamata di strumenti ha quattro fasi:

| Fase | Cosa Succede |
|-------|-------------|
| **1. Definire strumenti** | Descrivi le funzioni disponibili usando JSON Schema — nome, descrizione e parametri |
| **2. Decisione del modello** | Il modello riceve il tuo messaggio più la definizione degli strumenti. Se serve uno strumento, restituisce una risposta `tool_calls` anziché un testo |
| **3. Esecuzione locale** | Il tuo codice interpreta la chiamata allo strumento, esegue la funzione e raccoglie il risultato |
| **4. Risposta finale** | Invi il risultato dello strumento al modello, che produce la risposta finale |

> **Punto chiave:** Il modello non esegue mai codice. Richiede solo che venga chiamato uno strumento. La tua applicazione decide se esaudire questa richiesta — così mantieni il pieno controllo.

---

## Quali Modelli Supportano la Chiamata di Strumenti?

Non tutti i modelli supportano la chiamata di strumenti. Nel catalogo attuale di Foundry Local, i seguenti modelli hanno questa capacità:

| Modello | Dimensione | Chiamata Strumenti |
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

> **Suggerimento:** Per questo laboratorio usiamo **qwen2.5-0.5b** — è piccolo (822 MB da scaricare), veloce e ha supporto affidabile per la chiamata di strumenti.

---

## Obiettivi di Apprendimento

Al termine di questo laboratorio sarai in grado di:

- Spiegare lo schema di chiamata strumenti e perché è importante per gli agenti AI
- Definire schemi strumenti usando il formato OpenAI per la chiamata di funzioni
- Gestire il flusso di conversazione multi-turn chiamata strumenti
- Eseguire chiamate strumenti localmente e restituire i risultati al modello
- Scegliere il modello giusto per scenari di chiamata di strumenti

---

## Prerequisiti

| Requisito | Dettagli |
|-------------|---------|
| **Foundry Local CLI** | Installato e nel tuo `PATH` ([Parte 1](part1-getting-started.md)) |
| **Foundry Local SDK** | SDK Python, JavaScript o C# installato ([Parte 2](part2-foundry-local-sdk.md)) |
| **Un modello con chiamata strumenti** | qwen2.5-0.5b (verrà scaricato automaticamente) |

---

## Esercizi

### Esercizio 1 — Comprendere il Flusso di Chiamata Strumenti

Prima di scrivere codice, studia questo diagramma di sequenza:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Osservazioni chiave:**

1. Definisci gli strumenti in anticipo come oggetti JSON Schema
2. La risposta del modello contiene `tool_calls` invece di contenuto regolare
3. Ogni chiamata a uno strumento ha un `id` univoco che devi riferire quando restituisci i risultati
4. Il modello vede tutti i messaggi precedenti *più* i risultati degli strumenti nel generare la risposta finale
5. Possono esserci più chiamate a strumenti in una singola risposta

> **Discussione:** Perché il modello restituisce chiamate a strumenti invece di eseguire funzioni direttamente? Quali vantaggi di sicurezza offre questo?

---

### Esercizio 2 — Definizione di Schemi per Strumenti

Gli strumenti si definiscono usando il formato standard OpenAI per la chiamata di funzioni. Ogni strumento necessita di:

- **`type`**: Sempre `"function"`
- **`function.name`**: Un nome descrittivo della funzione (es. `get_weather`)
- **`function.description`**: Una descrizione chiara — il modello la usa per decidere quando chiamare lo strumento
- **`function.parameters`**: Un oggetto JSON Schema che descrive gli argomenti attesi

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

> **Buone pratiche per descrivere strumenti:**
> - Sii specifico: "Ottieni il meteo attuale per una data città" è meglio di "Ottieni meteo"
> - Descrivi chiaramente i parametri: il modello legge queste descrizioni per inserire i valori corretti
> - Indica parametri obbligatori vs opzionali — aiuta il modello a decidere cosa chiedere

---

### Esercizio 3 — Esegui Esempi di Chiamata Strumenti

Ogni esempio di codice definisce due strumenti (`get_weather` e `get_population`), invia una domanda che attiva l’uso dello strumento, esegue lo strumento localmente e invia il risultato indietro per una risposta finale.

<details>
<summary><strong>🐍 Python</strong></summary>

**Prerequisiti:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Esegui:**
```bash
python foundry-local-tool-calling.py
```

**Output previsto:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Analisi del codice** (`python/foundry-local-tool-calling.py`):

```python
# Definisci gli strumenti come una lista di schemi di funzione
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

# Invia con gli strumenti — il modello potrebbe restituire chiamate_a_strumenti invece del contenuto
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Verifica se il modello vuole chiamare uno strumento
if response.choices[0].message.tool_calls:
    # Esegui lo strumento e invia il risultato indietro
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Prerequisiti:**
```bash
cd javascript
npm install
```

**Esegui:**
```bash
node foundry-local-tool-calling.mjs
```

**Output previsto:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Analisi del codice** (`javascript/foundry-local-tool-calling.mjs`):

Questo esempio usa il `ChatClient` nativo Foundry Local SDK invece dell’OpenAI SDK, mostrando il comodo metodo `createChatClient()`:

```javascript
// Ottieni un ChatClient direttamente dall'oggetto modello
const chatClient = model.createChatClient();

// Invia con strumenti — ChatClient gestisce il formato compatibile con OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Controlla le chiamate agli strumenti
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Esegui gli strumenti e invia indietro i risultati
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Prerequisiti:**
```bash
cd csharp
dotnet restore
```

**Esegui:**
```bash
dotnet run toolcall
```

**Output previsto:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Analisi del codice** (`csharp/ToolCalling.cs`):

C# usa l’helper `ChatTool.CreateFunctionTool` per definire gli strumenti:

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

### Esercizio 4 — Il Flusso di Conversazione con Chiamata Strumenti

Comprendere la struttura dei messaggi è fondamentale. Ecco il flusso completo, mostrando l’array `messages` in ogni fase:

**Fase 1 — Richiesta iniziale:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Fase 2 — Il modello risponde con `tool_calls` (non contenuto):**
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

**Fase 3 — Tu aggiungi il messaggio assistente E il risultato dello strumento:**
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

**Fase 4 — Il modello produce la risposta finale usando il risultato dello strumento.**

> **Importante:** Il `tool_call_id` nel messaggio dello strumento deve corrispondere all’`id` della chiamata strumento. Così il modello associa risultati e richieste.

---

### Esercizio 5 — Chiamate Multiple a Strumenti

Un modello può richiedere diverse chiamate a strumenti in una singola risposta. Prova a modificare il messaggio utente per attivare più chiamate:

```python
# In Python — modifica il messaggio dell'utente:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// In JavaScript — modifica il messaggio dell'utente:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Il modello dovrebbe restituire due `tool_calls` — uno per `get_weather` e uno per `get_population`. Il tuo codice li gestisce già perché scorre tutte le chiamate.

> **Provalo:** Modifica il messaggio utente ed esegui di nuovo l’esempio. Il modello chiama entrambi gli strumenti?

---

### Esercizio 6 — Aggiungi Uno Strumento Personale

Estendi uno degli esempi con un nuovo strumento. Per esempio, aggiungi `get_time`:

1. Definisci lo schema dello strumento:
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

2. Aggiungi la logica di esecuzione:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # In una vera app, usa una libreria per i fusi orari
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... strumenti esistenti ...
```

3. Aggiungi lo strumento all’array `tools` e prova con: "Che ore sono a Tokyo?"

> **Sfida:** Aggiungi uno strumento che svolge un calcolo, come `convert_temperature` che converte tra Celsius e Fahrenheit. Testalo con: "Converti 100°F in Celsius."

---

### Esercizio 7 — Chiamata Strumenti con ChatClient dell’SDK (JavaScript)

L’esempio JavaScript usa già il `ChatClient` nativo SDK invece dell’OpenAI SDK. Questa è una comodità che elimina la necessità di costruire un client OpenAI manualmente:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient è creato direttamente dall'oggetto modello
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat accetta gli strumenti come secondo parametro
const response = await chatClient.completeChat(messages, tools);
```

Confrontalo con l’approccio Python che usa esplicitamente l’OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Entrambi gli schemi sono validi. Il `ChatClient` è più comodo; l’OpenAI SDK dà accesso a tutta la gamma di parametri OpenAI.

> **Provalo:** Modifica l’esempio JavaScript per usare l’OpenAI SDK invece di `ChatClient`. Ti serviranno `import OpenAI from "openai"` e creare il client con l’endpoint da `manager.urls[0]`.

---

### Esercizio 8 — Comprendere tool_choice

Il parametro `tool_choice` controlla se il modello *deve* usare uno strumento o può scegliere liberamente:

| Valore | Comportamento |
|-------|-----------|
| `"auto"` | Il modello decide se chiamare uno strumento (default) |
| `"none"` | Il modello non chiamerà alcuno strumento, anche se presenti |
| `"required"` | Il modello deve chiamare almeno uno strumento |
| `{"type": "function", "function": {"name": "get_weather"}}` | Il modello deve chiamare lo strumento specificato |

Prova ogni opzione nell’esempio Python:

```python
# Forza il modello a chiamare get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Nota:** Non tutte le opzioni `tool_choice` sono supportate da ogni modello. Se un modello non supporta `"required"`, potrebbe ignorare l’impostazione e comportarsi come `"auto"`.

---

## Trappole Comuni

| Problema | Soluzione |
|---------|----------|
| Il modello non chiama mai strumenti | Assicurati di usare un modello con supporto (es. qwen2.5-0.5b). Controlla la tabella sopra. |
| `tool_call_id` non corrisponde | Usa sempre l’`id` della risposta chiamata strumento, non un valore codificato a mano |
| Il modello restituisce JSON malformato in `arguments` | I modelli più piccoli possono produrre JSON non valido. Usa un try/catch intorno a `JSON.parse()` |
| Il modello chiama uno strumento inesistente | Aggiungi un gestore di default nella tua funzione `execute_tool` |
| Ciclo infinito di chiamata strumenti | Definisci un numero massimo di turni (es. 5) per prevenire loop senza fine |

---

## Punti Chiave

1. **La chiamata di strumenti** permette ai modelli di richiedere l’esecuzione di funzioni invece di indovinare risposte
2. Il modello **non esegue mai codice**; la tua applicazione decide cosa eseguire
3. Gli strumenti sono definiti come oggetti **JSON Schema** seguendo il formato OpenAI della chiamata di funzioni
4. La conversazione usa uno schema **multi-turn**: utente, poi assistente (tool_calls), poi strumento (risultati), poi assistente (risposta finale)
5. Usa sempre un **modello che supporta la chiamata strumenti** (Qwen 2.5, Phi-4-mini)
6. Il metodo `createChatClient()` dello SDK fornisce un modo comodo per fare richieste di chiamata strumenti senza costruire manualmente un client OpenAI

---

Continua con [Parte 12: Costruire una UI Web per il Zava Creative Writer](part12-zava-ui.md) per aggiungere un’interfaccia browser al pipeline multi-agente con streaming in tempo reale.

---

[← Parte 10: Modelli Personalizzati](part10-custom-models.md) | [Parte 12: UI Zava Writer →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:
Questo documento è stato tradotto utilizzando il servizio di traduzione AI [Co-op Translator](https://github.com/Azure/co-op-translator). Pur impegnandoci per l’accuratezza, si prega di notare che le traduzioni automatiche possono contenere errori o imprecisioni. Il documento originale nella sua lingua nativa deve essere considerato la fonte autorevole. Per informazioni critiche, si raccomanda una traduzione professionale effettuata da un umano. Non siamo responsabili per eventuali malintesi o interpretazioni errate derivanti dall’uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->