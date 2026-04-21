![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Μέρος 11: Κλήση Εργαλείων με Τοπικά Μοντέλα

> **Στόχος:** Ενεργοποιήστε το τοπικό σας μοντέλο να καλεί εξωτερικές συναρτήσεις (εργαλεία) ώστε να μπορεί να ανακτά δεδομένα σε πραγματικό χρόνο, να κάνει υπολογισμούς ή να αλληλεπιδρά με APIs — όλα λειτουργώντας ιδιωτικά στη συσκευή σας.

## Τι Είναι η Κλήση Εργαλείων;

Η κλήση εργαλείων (γνωστή και ως **κλήση συναρτήσεων**) επιτρέπει σε ένα γλωσσικό μοντέλο να ζητά την εκτέλεση συναρτήσεων που ορίζετε. Αντί να μαντεύει μια απάντηση, το μοντέλο αναγνωρίζει πότε ένα εργαλείο θα βοηθούσε και επιστρέφει μια δομημένη αίτηση για να εκτελέσει ο κώδικάς σας τη λειτουργία. Η εφαρμογή σας τρέχει τη συνάρτηση, στέλνει το αποτέλεσμα πίσω, και το μοντέλο ενσωματώνει αυτή την πληροφορία στην τελική του απάντηση.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Αυτό το μοτίβο είναι απαραίτητο για την κατασκευή πρακτόρων που μπορούν να:

- **Επιχειρήσουν ζωντανά δεδομένα** (καιρός, τιμές μετοχών, ερωτήματα βάσεων δεδομένων)
- **Κάνουν ακριβείς υπολογισμούς** (μαθηματικά, μετατροπές μονάδων)
- **Προβαίνουν σε ενέργειες** (αποστολή email, δημιουργία εισητηρίων, ενημέρωση εγγραφών)
- **Πρόσβαση σε ιδιωτικά συστήματα** (εσωτερικά APIs, συστήματα αρχείων)

---

## Πώς Λειτουργεί η Κλήση Εργαλείων

Η ροή κλήσης εργαλείων έχει τέσσερα στάδια:

| Στάδιο | Τι Συμβαίνει |
|-------|-------------|
| **1. Ορισμός εργαλείων** | Περιγράφετε τις διαθέσιμες συναρτήσεις χρησιμοποιώντας JSON Schema — όνομα, περιγραφή και παραμέτρους |
| **2. Απόφαση μοντέλου** | Το μοντέλο λαμβάνει το μήνυμά σας μαζί με τους ορισμούς εργαλείων. Αν ένα εργαλείο βοηθάει, επιστρέφει μια απάντηση `tool_calls` αντί απάντησης κειμένου |
| **3. Τοπική εκτέλεση** | Ο κώδικάς σας αναλύει την κλήση εργαλείου, τρέχει τη συνάρτηση και συλλέγει το αποτέλεσμα |
| **4. Τελική απάντηση** | Στέλνετε το αποτέλεσμα εργαλείου πίσω στο μοντέλο, που παράγει την τελική του απάντηση |

> **Σημαντικό:** Το μοντέλο δεν εκτελεί ποτέ κώδικα. Απλά *ζητά* να καλεστεί ένα εργαλείο. Η εφαρμογή σας αποφασίζει αν θα σεβαστεί αυτό το αίτημα — έτσι διατηρείτε πλήρη έλεγχο.

---

## Ποια Μοντέλα Υποστηρίζουν Κλήση Εργαλείων;

Δεν υποστηρίζουν όλα τα μοντέλα την κλήση εργαλείων. Στον τρέχοντα κατάλογο Foundry Local, τα παρακάτω μοντέλα έχουν δυνατότητα κλήσης εργαλείων:

| Μοντέλο | Μέγεθος | Κλήση Εργαλείων |
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

> **Συμβουλή:** Για αυτό το εργαστήριο χρησιμοποιούμε το **qwen2.5-0.5b** — είναι μικρό (822 MB για λήψη), γρήγορο και έχει αξιόπιστη υποστήριξη κλήσης εργαλείων.

---

## Μαθησιακοί Στόχοι

Μέχρι το τέλος αυτού του εργαστηρίου θα μπορείτε να:

- Εξηγήσετε το μοτίβο κλήσης εργαλείων και γιατί είναι σημαντικό για πρακτορες AI
- Ορίσετε σχήματα εργαλείων χρησιμοποιώντας τη μορφή κλήσης συναρτήσεων OpenAI
- Διαχειρίζεστε τη ροή συνομιλίας πολλαπλών βημάτων με κλήσεις εργαλείων
- Εκτελείτε τις κλήσεις εργαλείων τοπικά και επιστρέφετε τα αποτελέσματα στο μοντέλο
- Επιλέγετε το κατάλληλο μοντέλο για σενάρια κλήσης εργαλείων

---

## Προαπαιτούμενα

| Απαίτηση | Λεπτομέρειες |
|-------------|---------|
| **Foundry Local CLI** | Εγκατεστημένο και στο `PATH` σας ([Μέρος 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript ή C# SDK εγκατεστημένο ([Μέρος 2](part2-foundry-local-sdk.md)) |
| **Ένα μοντέλο κλήσης εργαλείων** | qwen2.5-0.5b (θα κατέβει αυτόματα) |

---

## Ασκήσεις

### Άσκηση 1 — Κατανόηση της Ροής Κλήσης Εργαλείων

Πριν γράψετε κώδικα, μελετήστε αυτό το διάγραμμα ακολουθίας:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Βασικές παρατηρήσεις:**

1. Ορίζετε τα εργαλεία εκ των προτέρων ως αντικείμενα JSON Schema
2. Η απάντηση του μοντέλου περιέχει `tool_calls` αντί για κανονικό περιεχόμενο
3. Κάθε κλήση εργαλείου έχει μοναδικό `id` που πρέπει να αναφέρετε κατά την επιστροφή αποτελεσμάτων
4. Το μοντέλο βλέπει όλα τα προηγούμενα μηνύματα *συν* τα αποτελέσματα εργαλείων όταν παράγει την τελική απάντηση
5. Πολλαπλές κλήσεις εργαλείων μπορούν να συμβούν σε μία και μόνο απάντηση

> **Συζήτηση:** Γιατί το μοντέλο επιστρέφει κλήσεις εργαλείων αντί να εκτελεί συναρτήσεις απευθείας; Ποιες είναι οι πλεονεκτικές ασφάλειας που προσφέρει αυτό;

---

### Άσκηση 2 — Ορισμός Σχημάτων Εργαλείων

Τα εργαλεία ορίζονται χρησιμοποιώντας το πρότυπο κλήσης συναρτήσεων OpenAI. Κάθε εργαλείο χρειάζεται:

- **`type`**: Πάντα `"function"`
- **`function.name`**: Ένα περιγραφικό όνομα συνάρτησης (π.χ. `get_weather`)
- **`function.description`**: Μια σαφής περιγραφή — το μοντέλο το χρησιμοποιεί για να αποφασίσει πότε να καλέσει το εργαλείο
- **`function.parameters`**: Ένα αντικείμενο JSON Schema που περιγράφει τα αναμενόμενα ορίσματα

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

> **Καλές πρακτικές για περιγραφές εργαλείων:**
> - Να είστε συγκεκριμένοι: "Λήψη τωρινού καιρού για μια δεδομένη πόλη" είναι καλύτερο από "Λήψη καιρού"
> - Περιγράψτε τις παραμέτρους καθαρά: το μοντέλο διαβάζει αυτές τις περιγραφές για να συμπληρώσει τις σωστές τιμές
> - Επισημάνετε απαραίτητες έναντι προαιρετικών παραμέτρων — αυτό βοηθά το μοντέλο να αποφασίσει τι να ζητήσει

---

### Άσκηση 3 — Εκτέλεση Παραδειγμάτων Κλήσης Εργαλείων

Κάθε δείγμα γλώσσας ορίζει δύο εργαλεία (`get_weather` και `get_population`), στέλνει μια ερώτηση που προκαλεί χρήση εργαλείου, εκτελεί το εργαλείο τοπικά και στέλνει το αποτέλεσμα πίσω για τελική απάντηση.

<details>
<summary><strong>🐍 Python</strong></summary>

**Προαπαιτούμενα:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Εκτέλεση:**
```bash
python foundry-local-tool-calling.py
```

**Αναμενόμενο αποτέλεσμα:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Ανάλυση κώδικα** (`python/foundry-local-tool-calling.py`):

```python
# Ορίστε εργαλεία ως μια λίστα με σχήματα συναρτήσεων
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

# Στείλτε με εργαλεία — το μοντέλο μπορεί να επιστρέψει κλήσεις εργαλείων αντί για περιεχόμενο
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Ελέγξτε αν το μοντέλο θέλει να καλέσει ένα εργαλείο
if response.choices[0].message.tool_calls:
    # Εκτελέστε το εργαλείο και στείλτε το αποτέλεσμα πίσω
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Προαπαιτούμενα:**
```bash
cd javascript
npm install
```

**Εκτέλεση:**
```bash
node foundry-local-tool-calling.mjs
```

**Αναμενόμενο αποτέλεσμα:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Ανάλυση κώδικα** (`javascript/foundry-local-tool-calling.mjs`):

Αυτό το παράδειγμα χρησιμοποιεί το εγγενές Foundry Local SDK `ChatClient` αντί για το OpenAI SDK, δείχνοντας τη βολική μέθοδο `createChatClient()`:

```javascript
// Λάβετε έναν ChatClient απευθείας από το αντικείμενο μοντέλου
const chatClient = model.createChatClient();

// Στείλτε με εργαλεία — ο ChatClient διαχειρίζεται τη μορφή συμβατή με OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Ελέγξτε για κλήσεις εργαλείων
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Εκτελέστε τα εργαλεία και στείλτε πίσω τα αποτελέσματα
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Προαπαιτούμενα:**
```bash
cd csharp
dotnet restore
```

**Εκτέλεση:**
```bash
dotnet run toolcall
```

**Αναμενόμενο αποτέλεσμα:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Ανάλυση κώδικα** (`csharp/ToolCalling.cs`):

Η C# χρησιμοποιεί τον βοηθό `ChatTool.CreateFunctionTool` για τον ορισμό εργαλείων:

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

### Άσκηση 4 — Η Ροή Συνομιλίας Κλήσης Εργαλείων

Η κατανόηση της δομής των μηνυμάτων είναι κρίσιμη. Εδώ είναι η πλήρης ροή, που δείχνει τον πίνακα `messages` σε κάθε στάδιο:

**Στάδιο 1 — Αρχικό αίτημα:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Στάδιο 2 — Το μοντέλο απαντά με `tool_calls` (όχι περιεχόμενο):**
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

**Στάδιο 3 — Προσθέτετε το μήνυμα βοηθού ΚΑΙ το αποτέλεσμα του εργαλείου:**
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

**Στάδιο 4 — Το μοντέλο παράγει την τελική απάντηση χρησιμοποιώντας το αποτέλεσμα του εργαλείου.**

> **Σημαντικό:** Το `tool_call_id` στο μήνυμα του εργαλείου πρέπει να ταιριάζει με το `id` από την κλήση εργαλείου. Αυτός είναι ο τρόπος που το μοντέλο συνδέει τα αποτελέσματα με τα αιτήματα.

---

### Άσκηση 5 — Πολλαπλές Κλήσεις Εργαλείων

Ένα μοντέλο μπορεί να ζητήσει πολλές κλήσεις εργαλείων σε μία απάντηση. Δοκιμάστε να αλλάξετε το μήνυμα χρήστη ώστε να ενεργοποιήσει πολλές κλήσεις:

```python
# Στην Python — αλλάξτε το μήνυμα του χρήστη:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// Στο JavaScript — αλλάξτε το μήνυμα χρήστη:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Το μοντέλο πρέπει να επιστρέψει δύο `tool_calls` — ένα για το `get_weather` και ένα για το `get_population`. Ο κώδικάς σας το χειρίζεται ήδη γιατί επεξεργάζεται όλες τις κλήσεις εργαλείων.

> **Δοκιμάστε το:** Τροποποιήστε το μήνυμα χρήστη και τρέξτε το παράδειγμα ξανά. Καλεί το μοντέλο και τα δύο εργαλεία;

---

### Άσκηση 6 — Προσθέστε το δικό σας Εργαλείο

Επεκτείνετε ένα από τα δείγματα με ένα νέο εργαλείο. Για παράδειγμα, προσθέστε εργαλείο `get_time`:

1. Ορίστε το σχήμα εργαλείου:
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

2. Προσθέστε τη λογική εκτέλεσης:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Σε μια πραγματική εφαρμογή, χρησιμοποιήστε μια βιβλιοθήκη ζώνης ώρας
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... υπάρχοντα εργαλεία ...
```

3. Προσθέστε το εργαλείο στον πίνακα `tools` και δοκιμάστε με: "Τι ώρα είναι στο Τόκιο;"

> **Πρόκληση:** Προσθέστε ένα εργαλείο που κάνει υπολογισμό, όπως το `convert_temperature` που μετατρέπει μεταξύ Κελσίου και Φαρενάιτ. Δοκιμάστε το με: "Μετατρέψτε 100°F σε Κελσίου."

---

### Άσκηση 7 — Κλήση Εργαλείων με το ChatClient του SDK (JavaScript)

Το δείγμα JavaScript ήδη χρησιμοποιεί το εγγενές `ChatClient` του SDK αντί για το OpenAI SDK. Πρόκειται για μια διευκόλυνση που αφαιρεί την ανάγκη κατασκευής πελάτη OpenAI μόνος σας:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Το ChatClient δημιουργείται απευθείας από το αντικείμενο μοντέλου
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// Η completeChat δέχεται εργαλεία ως δεύτερο παράμετρο
const response = await chatClient.completeChat(messages, tools);
```

Συγκρίνετε το με την προσέγγιση Python που χρησιμοποιεί ρητά το OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Και τα δύο μοτίβα είναι έγκυρα. Το `ChatClient` είναι πιο βολικό· το OpenAI SDK σας δίνει πρόσβαση σε ολόκληρο το φάσμα παραμέτρων OpenAI.

> **Δοκιμάστε το:** Τροποποιήστε το δείγμα JavaScript για να χρησιμοποιήσει το OpenAI SDK αντί για το `ChatClient`. Θα χρειαστείτε `import OpenAI from "openai"` και να κατασκευάσετε τον πελάτη με το endpoint από το `manager.urls[0]`.

---

### Άσκηση 8 — Κατανόηση του `tool_choice`

Η παράμετρος `tool_choice` ελέγχει αν το μοντέλο *πρέπει* να χρησιμοποιήσει εργαλείο ή μπορεί να επιλέξει ελεύθερα:

| Τιμή | Συμπεριφορά |
|-------|-----------|
| `"auto"` | Το μοντέλο αποφασίζει αν θα καλέσει εργαλείο (προεπιλογή) |
| `"none"` | Το μοντέλο δεν θα καλέσει κανένα εργαλείο, ακόμα κι αν παρέχονται |
| `"required"` | Το μοντέλο πρέπει να καλέσει τουλάχιστον ένα εργαλείο |
| `{"type": "function", "function": {"name": "get_weather"}}` | Το μοντέλο πρέπει να καλέσει το συγκεκριμένο εργαλείο |

Δοκιμάστε κάθε επιλογή στο παράδειγμα Python:

```python
# Αναγκάστε το μοντέλο να καλέσει get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Σημείωση:** Δεν υποστηρίζονται όλες οι επιλογές `tool_choice` από κάθε μοντέλο. Αν ένα μοντέλο δεν υποστηρίζει το `"required"`, ίσως αγνοήσει τη ρύθμιση και συμπεριφέρεται ως `"auto"`.

---

## Συνηθισμένα Σφάλματα

| Πρόβλημα | Λύση |
|---------|----------|
| Το μοντέλο δεν καλεί ποτέ εργαλεία | Βεβαιωθείτε ότι χρησιμοποιείτε μοντέλο με κλήση εργαλείων (π.χ. qwen2.5-0.5b). Ελέγξτε τον παραπάνω πίνακα. |
| Ασυμφωνία `tool_call_id` | Χρησιμοποιήστε πάντα το `id` από την απόκριση κλήσης εργαλείου, όχι σταθερή τιμή |
| Το μοντέλο επιστρέφει κακής μορφής JSON στα `arguments` | Τα μικρότερα μοντέλα μερικές φορές παράγουν μη έγκυρο JSON. Περίκλειστε το `JSON.parse()` σε try/catch |
| Το μοντέλο καλεί εργαλείο που δεν υπάρχει | Προσθέστε έναν προεπιλεγμένο χειριστή στη λειτουργία `execute_tool` |
| Άπειρος βρόχος κλήσης εργαλείων | Ορίστε μέγιστο αριθμό γύρων (π.χ. 5) για να αποφύγετε ανεξέλεγκτα loops |

---

## Κύρια Συμπεράσματα

1. Η **κλήση εργαλείων** επιτρέπει στα μοντέλα να ζητούν εκτέλεση συναρτήσεων αντί να μαντεύουν απαντήσεις
2. Το μοντέλο **δεν εκτελεί ποτέ κώδικα**· η εφαρμογή σας αποφασίζει τι θα τρέξει
3. Τα εργαλεία ορίζονται ως **αντικείμενα JSON Schema** ακολουθώντας το πρότυπο κλήσης συναρτήσεων OpenAI
4. Η συνομιλία χρησιμοποιεί μοτίβο **πολλαπλών γύρων**: χρήστης, μετά βοηθός (tool_calls), μετά εργαλείο (αποτελέσματα), τέλος βοηθός (τελική απάντηση)
5. Να χρησιμοποιείτε πάντα **μοντέλο που υποστηρίζει κλήση εργαλείων** (Qwen 2.5, Phi-4-mini)
6. Η μέθοδος `createChatClient()` του SDK παρέχει βολικό τρόπο να γίνονται αιτήματα κλήσης εργαλείων χωρίς να κατασκευάζετε πελάτη OpenAI

---

Συνεχίστε στο [Μέρος 12: Δημιουργία Web UI για τον Zava Creative Writer](part12-zava-ui.md) για να προσθέσετε ένα διεπαφή περιηγητή στην πολυ-πρακτορική ροή με μετάδοση σε πραγματικό χρόνο.

---

[← Μέρος 10: Προσαρμοσμένα Μοντέλα](part10-custom-models.md) | [Μέρος 12: Zava Writer UI →](part12-zava-ui.md)