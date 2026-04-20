![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Teil 11: Tool Calling mit lokalen Modellen

> **Ziel:** Ermöglichen Sie Ihrem lokalen Modell, externe Funktionen (Tools) aufzurufen, damit es Echtzeitdaten abrufen, Berechnungen durchführen oder mit APIs interagieren kann – alles privat auf Ihrem Gerät ausgeführt.

## Was ist Tool Calling?

Tool Calling (auch bekannt als **Function Calling**) ermöglicht es einem Sprachmodell, die Ausführung von von Ihnen definierten Funktionen anzufordern. Statt eine Antwort zu erraten, erkennt das Modell, wann ein Tool hilfreich wäre, und gibt eine strukturierte Anfrage zurück, die Ihr Code ausführen soll. Ihre Anwendung führt die Funktion aus, sendet das Ergebnis zurück und das Modell integriert diese Information in seine endgültige Antwort.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Dieses Muster ist entscheidend für den Aufbau von Agenten, die:

- **Live-Daten abrufen** (Wetter, Aktienkurse, Datenbankabfragen)
- **Exakte Berechnungen durchführen** (Mathematik, Einheitenumrechnungen)
- **Aktionen ausführen** (E-Mails senden, Tickets erstellen, Datensätze aktualisieren)
- **Auf private Systeme zugreifen** (interne APIs, Dateisysteme)

---

## Wie funktioniert Tool Calling?

Der Tool-Calling-Ablauf besteht aus vier Phasen:

| Phase | Was passiert |
|-------|-------------|
| **1. Tools definieren** | Sie beschreiben verfügbare Funktionen mit JSON Schema – Name, Beschreibung und Parameter |
| **2. Modell entscheidet** | Das Modell erhält Ihre Nachricht plus die Tool-Definitionen. Wenn ein Tool hilfreich wäre, gibt es eine `tool_calls`-Antwort anstelle einer Textantwort zurück |
| **3. Lokal ausführen** | Ihr Code analysiert den Tool-Aufruf, führt die Funktion aus und sammelt das Ergebnis |
| **4. Endgültige Antwort** | Sie senden das Tool-Ergebnis zurück an das Modell, das daraufhin seine endgültige Antwort generiert |

> **Wichtig:** Das Modell führt niemals Code aus. Es *fordert nur an*, dass ein Tool aufgerufen wird. Ihre Anwendung entscheidet, ob diese Anforderung ausgeführt wird – so behalten Sie die volle Kontrolle.

---

## Welche Modelle unterstützen Tool Calling?

Nicht jedes Modell unterstützt Tool Calling. Im aktuellen Foundry Local Katalog haben folgende Modelle Tool-Calling-Funktionalität:

| Modell | Größe | Tool Calling |
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

> **Tipp:** Für dieses Lab verwenden wir **qwen2.5-0.5b** – es ist klein (822 MB Download), schnell und bietet zuverlässige Tool-Calling-Unterstützung.

---

## Lernziele

Am Ende dieses Labs können Sie:

- Das Tool-Calling-Muster erklären und warum es für KI-Agenten wichtig ist
- Tool-Schemas im OpenAI Function-Calling-Format definieren
- Das Multi-Turn Tool-Calling-Konversationsmuster handhaben
- Tool-Aufrufe lokal ausführen und Ergebnisse an das Modell zurückgeben
- Das richtige Modell für Tool-Calling-Szenarien auswählen

---

## Voraussetzungen

| Voraussetzung | Details |
|-------------|---------|
| **Foundry Local CLI** | Installiert und in Ihrem `PATH` ([Teil 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python-, JavaScript- oder C#-SDK installiert ([Teil 2](part2-foundry-local-sdk.md)) |
| **Ein Tool-Calling-Modell** | qwen2.5-0.5b (wird automatisch heruntergeladen) |

---

## Übungen

### Übung 1 — Den Tool-Calling-Fluss verstehen

Bevor Sie Code schreiben, studieren Sie dieses Sequenzdiagramm:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Wichtige Beobachtungen:**

1. Sie definieren die Tools vorab als JSON Schema Objekte
2. Die Antwort des Modells enthält `tool_calls` statt regulärem Inhalt
3. Jeder Tool-Aufruf hat eine eindeutige `id`, auf die Sie beim Zurücksenden der Ergebnisse verweisen müssen
4. Das Modell sieht alle vorherigen Nachrichten *plus* die Tool-Ergebnisse, wenn es die finale Antwort generiert
5. Mehrere Tool-Aufrufe können in einer einzelnen Antwort enthalten sein

> **Diskussion:** Warum gibt das Modell Tool-Aufrufe zurück, anstatt Funktionen direkt auszuführen? Welche Sicherheitsvorteile ergeben sich daraus?

---

### Übung 2 — Tool-Schemas definieren

Tools werden mit dem Standard OpenAI Function-Calling-Format definiert. Jedes Tool benötigt:

- **`type`**: Immer `"function"`
- **`function.name`**: Einen beschreibenden Funktionsnamen (z. B. `get_weather`)
- **`function.description`**: Eine klare Beschreibung – das Modell nutzt diese, um zu entscheiden, wann das Tool aufzurufen ist
- **`function.parameters`**: Ein JSON Schema Objekt, das die erwarteten Argumente beschreibt

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

> **Beste Praktiken für Tool-Beschreibungen:**
> - Seien Sie spezifisch: "Aktuelles Wetter für eine Stadt abrufen" ist besser als "Wetter abrufen"
> - Beschreiben Sie Parameter klar: Das Modell liest diese Beschreibungen, um die richtigen Werte einzusetzen
> - Markieren Sie erforderliche vs optionale Parameter – das hilft dem Modell bei der Entscheidung, was es anfragen soll

---

### Übung 3 — Tool-Calling-Beispiele ausführen

Jedes Sprachbeispiel definiert zwei Tools (`get_weather` und `get_population`), sendet eine Frage, die den Tool-Einsatz auslöst, führt das Tool lokal aus und sendet das Ergebnis für eine finale Antwort zurück.

<details>
<summary><strong>🐍 Python</strong></summary>

**Voraussetzungen:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Ausführen:**
```bash
python foundry-local-tool-calling.py
```

**Erwartete Ausgabe:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Code-Durchgang** (`python/foundry-local-tool-calling.py`):

```python
# Definieren Sie Werkzeuge als eine Liste von Funktionsschemata
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

# Senden Sie mit Werkzeugen — das Modell kann tool_calls anstelle von Inhalt zurückgeben
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Überprüfen Sie, ob das Modell ein Werkzeug aufrufen möchte
if response.choices[0].message.tool_calls:
    # Führen Sie das Werkzeug aus und senden Sie das Ergebnis zurück
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Voraussetzungen:**
```bash
cd javascript
npm install
```

**Ausführen:**
```bash
node foundry-local-tool-calling.mjs
```

**Erwartete Ausgabe:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Code-Durchgang** (`javascript/foundry-local-tool-calling.mjs`):

Dieses Beispiel verwendet das native Foundry Local SDK `ChatClient` anstelle des OpenAI SDK und zeigt die bequeme Methode `createChatClient()`:

```javascript
// Erhalte einen ChatClient direkt vom Modellobjekt
const chatClient = model.createChatClient();

// Senden mit Werkzeugen — ChatClient verarbeitet das OpenAI-kompatible Format
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Überprüfe auf Werkzeugaufrufe
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Führe Werkzeuge aus und sende Ergebnisse zurück
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Voraussetzungen:**
```bash
cd csharp
dotnet restore
```

**Ausführen:**
```bash
dotnet run toolcall
```

**Erwartete Ausgabe:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Code-Durchgang** (`csharp/ToolCalling.cs`):

C# verwendet den Hilfsaufruf `ChatTool.CreateFunctionTool`, um Tools zu definieren:

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

### Übung 4 — Der Tool-Calling-Gesprächsfluss

Das Verständnis der Nachrichtenstruktur ist entscheidend. Hier ist der komplette Ablauf, der das `messages`-Array in jeder Phase zeigt:

**Phase 1 — Initiale Anfrage:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Phase 2 — Modell antwortet mit tool_calls (kein Inhalt):**
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

**Phase 3 — Sie fügen die Assistenten-Nachricht UND das Tool-Ergebnis hinzu:**
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

**Phase 4 — Modell erzeugt die finale Antwort mit dem Tool-Ergebnis.**

> **Wichtig:** Die `tool_call_id` in der Tool-Nachricht muss mit der `id` aus dem Tool-Aufruf übereinstimmen. So kann das Modell Ergebnisse mit Anfragen verbinden.

---

### Übung 5 — Mehrere Tool-Aufrufe

Ein Modell kann in einer einzigen Antwort mehrere Tool-Aufrufe anfordern. Versuchen Sie, die Benutzernachricht so zu ändern, dass mehrere Aufrufe ausgelöst werden:

```python
# In Python — ändere die Benutzer-Nachricht:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// In JavaScript — ändere die Benutzermeldung:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Das Modell sollte zwei `tool_calls` zurückgeben – einen für `get_weather` und einen für `get_population`. Ihr Code verarbeitet das bereits, da er alle Tool-Aufrufe durchläuft.

> **Probieren Sie es aus:** Ändern Sie die Benutzernachricht und führen Sie das Beispiel erneut aus. Ruft das Modell beide Tools auf?

---

### Übung 6 — Eigenes Tool hinzufügen

Erweitern Sie eines der Beispiele mit einem neuen Tool. Fügen Sie zum Beispiel ein `get_time` Tool hinzu:

1. Definieren Sie das Tool-Schema:
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

2. Fügen Sie die Ausführungslogik hinzu:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # In einer echten Anwendung eine Zeitzonenbibliothek verwenden
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... vorhandene Werkzeuge ...
```

3. Fügen Sie das Tool dem `tools`-Array hinzu und testen Sie mit: "Wie spät ist es in Tokio?"

> **Herausforderung:** Fügen Sie ein Tool hinzu, das eine Berechnung durchführt, z. B. `convert_temperature`, das zwischen Celsius und Fahrenheit umrechnet. Testen Sie mit: "Wandle 100°F in Celsius um."

---

### Übung 7 — Tool Calling mit dem SDK `ChatClient` (JavaScript)

Das JavaScript-Beispiel verwendet bereits das native SDK `ChatClient` anstelle des OpenAI SDK. Dies ist eine Komfortfunktion, die es überflüssig macht, einen OpenAI-Client selbst zu erstellen:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient wird direkt aus dem Modellobjekt erstellt
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat akzeptiert Werkzeuge als zweiten Parameter
const response = await chatClient.completeChat(messages, tools);
```

Vergleichen Sie dies mit dem Python-Ansatz, der das OpenAI SDK explizit verwendet:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Beide Muster sind gültig. `ChatClient` ist bequemer; das OpenAI SDK bietet Zugriff auf die vollständige Palette der OpenAI-Parameter.

> **Probieren Sie es aus:** Ändern Sie das JavaScript-Beispiel so, dass es das OpenAI SDK anstelle von `ChatClient` verwendet. Sie benötigen `import OpenAI from "openai"` und müssen den Client mit dem Endpunkt aus `manager.urls[0]` erstellen.

---

### Übung 8 — Verständnis von tool_choice

Der Parameter `tool_choice` steuert, ob das Modell *muss* ein Tool verwenden oder frei wählen kann:

| Wert | Verhalten |
|-------|-----------|
| `"auto"` | Modell entscheidet, ob ein Tool aufgerufen wird (Standard) |
| `"none"` | Modell ruft keine Tools auf, auch wenn verfügbar |
| `"required"` | Modell muss mindestens ein Tool aufrufen |
| `{"type": "function", "function": {"name": "get_weather"}}` | Das Modell muss das angegebene Tool aufrufen |

Probieren Sie jede Option im Python-Beispiel aus:

```python
# Erzwingen Sie, dass das Modell get_weather aufruft
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Hinweis:** Nicht alle `tool_choice`-Optionen werden von jedem Modell unterstützt. Wenn ein Modell `"required"` nicht unterstützt, ignoriert es die Einstellung und verhält sich wie `"auto"`.

---

## Häufige Fehlerquellen

| Problem | Lösung |
|---------|----------|
| Modell ruft niemals Tools auf | Stellen Sie sicher, dass Sie ein Tool-Calling-Modell verwenden (z. B. qwen2.5-0.5b). Prüfen Sie die Tabelle oben. |
| `tool_call_id` stimmt nicht überein | Verwenden Sie immer die `id` aus der Tool-Call-Antwort, nicht einen festkodierten Wert |
| Modell liefert fehlerhaftes JSON in `arguments` zurück | Kleinere Modelle produzieren gelegentlich ungültiges JSON. Packen Sie `JSON.parse()` in ein try/catch |
| Modell ruft ein nicht vorhandenes Tool auf | Fügen Sie in Ihrer `execute_tool`-Funktion einen Standard-Handler hinzu |
| Endlosschleife beim Tool-Calling | Setzen Sie eine maximale Anzahl an Durchläufen (z. B. 5), um unkontrollierte Schleifen zu verhindern |

---

## Wichtige Erkenntnisse

1. **Tool Calling** erlaubt Modellen, die Ausführung von Funktionen anzufordern statt Antworten zu erraten
2. Das Modell **führt niemals Code aus**; Ihre Anwendung entscheidet, was ausgeführt wird
3. Tools werden als **JSON Schema**-Objekte definiert, die dem OpenAI Function-Calling-Format folgen
4. Die Konversation verwendet ein **Multi-Turn-Muster**: Nutzer, dann Assistent (`tool_calls`), dann Tool (Ergebnisse), dann Assistent (finale Antwort)
5. Verwenden Sie immer ein **Modell, das Tool Calling unterstützt** (Qwen 2.5, Phi-4-mini)
6. Das SDK `createChatClient()` bietet eine bequeme Möglichkeit, Tool-Calling-Anfragen zu stellen, ohne einen OpenAI-Client selbst zu erstellen

---

Fahren Sie fort mit [Teil 12: Aufbau einer Web-UI für den Zava Creative Writer](part12-zava-ui.md), um eine browserbasierte Benutzeroberfläche für die Multi-Agent-Pipeline mit Echtzeit-Streaming hinzuzufügen.

---

[← Teil 10: Eigene Modelle](part10-custom-models.md) | [Teil 12: Zava Writer UI →](part12-zava-ui.md)