![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Część 11: Wywoływanie narzędzi z lokalnymi modelami

> **Cel:** Umożliwienie Twojemu lokalnemu modelowi wywoływania zewnętrznych funkcji (narzędzi), aby mógł pobierać dane w czasie rzeczywistym, wykonywać obliczenia lub komunikować się z API — wszystko działa prywatnie na Twoim urządzeniu.

## Czym jest wywoływanie narzędzi?

Wywoływanie narzędzi (znane również jako **wywoływanie funkcji**) pozwala modelowi językowemu na żądanie wykonania zdefiniowanych przez Ciebie funkcji. Zamiast zgadywać odpowiedź, model rozpoznaje, kiedy narzędzie mogłoby pomóc, i zwraca ustrukturyzowane żądanie do wykonania przez Twój kod. Twoja aplikacja uruchamia funkcję, odsyła wynik, a model uwzględnia tę informację w ostatecznej odpowiedzi.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Ten wzorzec jest kluczowy do budowania agentów, którzy mogą:

- **Pobierać dane na żywo** (pogoda, ceny akcji, zapytania do bazy danych)
- **Wykonywać precyzyjne obliczenia** (matematyka, konwersje jednostek)
- **Wykonywać działania** (wysyłać maile, tworzyć zgłoszenia, aktualizować rekordy)
- **Dostęp do prywatnych systemów** (wewnętrzne API, systemy plików)

---

## Jak działa wywoływanie narzędzi

Proces wywoływania narzędzi ma cztery etapy:

| Etap | Co się dzieje |
|-------|--------------|
| **1. Definiowanie narzędzi** | Opisujesz dostępne funkcje używając JSON Schema — nazwa, opis i parametry |
| **2. Decyzja modelu** | Model odbiera Twoją wiadomość wraz z definicjami narzędzi. Jeśli narzędzie by pomogło, zwraca odpowiedź `tool_calls` zamiast treści tekstowej |
| **3. Lokalne wykonanie** | Twój kod interpretuje wywołanie narzędzia, uruchamia funkcję i zbiera wynik |
| **4. Ostateczna odpowiedź** | Wysyłasz wynik narzędzia z powrotem do modelu, który generuje końcową odpowiedź |

> **Kluczowa sprawa:** Model nigdy nie wykonuje kodu. Tylko *prosi* o wywołanie narzędzia. Twoja aplikacja decyduje, czy spełnić to żądanie — to daje Ci pełną kontrolę.

---

## Które modele obsługują wywoływanie narzędzi?

Nie każdy model obsługuje wywoływanie narzędzi. W aktualnym katalogu Foundry Local modele z tą funkcjonalnością to:

| Model | Rozmiar | Wywoływanie narzędzi |
|-------|---------|:--------------------:|
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

> **Wskazówka:** W tym laboratorium używamy **qwen2.5-0.5b** — jest mały (pobranie 822 MB), szybki i ma niezawodne wsparcie dla wywoływania narzędzi.

---

## Cele nauki

Po zakończeniu tego laboratorium będziesz potrafił:

- Wyjaśnić wzorzec wywoływania narzędzi i dlaczego jest istotny dla agentów AI
- Definiować schematy narzędzi korzystając z formatu wywoływania funkcji OpenAI
- Obsługiwać konwersację wieloetapową wywoływania narzędzi
- Wykonywać lokalnie wywołania narzędzi i przesyłać wyniki do modelu
- Wybrać odpowiedni model do scenariuszy wywoływania narzędzi

---

## Wymagania wstępne

| Wymaganie | Szczegóły |
|-----------|-----------|
| **Foundry Local CLI** | Zainstalowany i dostępny w Twojej ścieżce `PATH` ([Część 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Zainstalowany SDK w Pythonie, JavaScript lub C# ([Część 2](part2-foundry-local-sdk.md)) |
| **Model z obsługą wywoływania narzędzi** | qwen2.5-0.5b (zostanie pobrany automatycznie) |

---

## Ćwiczenia

### Ćwiczenie 1 — Zrozumienie przepływu wywoływania narzędzi

Zanim napiszesz kod, przeanalizuj ten diagram sekwencji:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Kluczowe obserwacje:**

1. Definiujesz narzędzia wcześniej jako obiekty JSON Schema
2. Odpowiedź modelu zawiera `tool_calls` zamiast standardowej treści
3. Każde wywołanie narzędzia ma unikalne `id`, do którego musisz się odwołać przy zwracaniu wyników
4. Model widzi wszystkie poprzednie wiadomości *plus* wyniki narzędzi podczas generowania ostatecznej odpowiedzi
5. W jednej odpowiedzi może być wiele wywołań narzędzi

> **Dyskusja:** Dlaczego model zwraca wywołania narzędzi, zamiast wykonywać funkcje bezpośrednio? Jakie to ma zalety z punktu widzenia bezpieczeństwa?

---

### Ćwiczenie 2 — Definiowanie schematów narzędzi

Narzędzia definiuje się używając standardowego formatu wywoływania funkcji OpenAI. Każde narzędzie potrzebuje:

- **`type`**: Zawsze `"function"`
- **`function.name`**: Opisowa nazwa funkcji (np. `get_weather`)
- **`function.description`**: Jasny opis — model używa tego, aby zdecydować, kiedy wywołać narzędzie
- **`function.parameters`**: Obiekt JSON Schema opisujący oczekiwane argumenty

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

> **Najlepsze praktyki dla opisów narzędzi:**
> - Bądź konkretny: "Pobierz aktualną pogodę dla podanego miasta" jest lepsze niż "Pobierz pogodę"
> - Opisuj parametry jasno: model czyta te opisy, aby wypełnić odpowiednimi wartościami
> - Oznacz parametry wymagane i opcjonalne — to pomaga modelowi zdecydować, o co zapytać

---

### Ćwiczenie 3 — Uruchamianie przykładów wywoływania narzędzi

Każdy przykład językowy definiuje dwa narzędzia (`get_weather` i `get_population`), wysyła pytanie wywołujące użycie narzędzi, wykonuje wywołania lokalnie i wysyła wynik z powrotem, aby uzyskać ostateczną odpowiedź.

<details>
<summary><strong>🐍 Python</strong></summary>

**Wymagania:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Uruchom:**
```bash
python foundry-local-tool-calling.py
```

**Oczekiwany wynik:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Omówienie kodu** (`python/foundry-local-tool-calling.py`):

```python
# Zdefiniuj narzędzia jako listę schematów funkcji
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

# Wyślij z narzędziami — model może zwrócić tool_calls zamiast treści
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Sprawdź, czy model chce wywołać narzędzie
if response.choices[0].message.tool_calls:
    # Wykonaj narzędzie i odeślij wynik z powrotem
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Wymagania:**
```bash
cd javascript
npm install
```

**Uruchom:**
```bash
node foundry-local-tool-calling.mjs
```

**Oczekiwany wynik:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Omówienie kodu** (`javascript/foundry-local-tool-calling.mjs`):

Ten przykład używa natywnego SDK Foundry Local `ChatClient`, zamiast OpenAI SDK, pokazując wygodną metodę `createChatClient()`:

```javascript
// Pobierz ChatClient bezpośrednio z obiektu modelu
const chatClient = model.createChatClient();

// Wysyłaj za pomocą narzędzi — ChatClient obsługuje format kompatybilny z OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Sprawdź wywołania narzędzi
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Wykonaj narzędzia i wyślij wyniki z powrotem
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Wymagania:**
```bash
cd csharp
dotnet restore
```

**Uruchom:**
```bash
dotnet run toolcall
```

**Oczekiwany wynik:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Omówienie kodu** (`csharp/ToolCalling.cs`):

C# używa pomocnika `ChatTool.CreateFunctionTool` do definiowania narzędzi:

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

### Ćwiczenie 4 — Przebieg konwersacji wywoływania narzędzi

Zrozumienie struktury wiadomości jest kluczowe. Oto pełny przebieg, pokazujący tablicę `messages` na każdym etapie:

**Etap 1 — Żądanie początkowe:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Etap 2 — Model odpowiada wywołaniami `tool_calls` (zamiast treści):**
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

**Etap 3 — Dodajesz wiadomość asystenta ORAZ wynik narzędzia:**
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

**Etap 4 — Model generuje ostateczną odpowiedź wykorzystując wynik narzędzia.**

> **Ważne:** `tool_call_id` w wiadomości narzędzia musi odpowiadać `id` wywołania narzędzia. Tak model dopasowuje wyniki do żądań.

---

### Ćwiczenie 5 — Wielokrotne wywołania narzędzi

Model może poprosić o kilka wywołań narzędzi w jednej odpowiedzi. Spróbuj zmienić wiadomość użytkownika tak, aby wywołać kilka wywołań:

```python
# W Pythonie — zmień komunikat użytkownika:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// W JavaScript — zmień wiadomość użytkownika:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Model powinien zwrócić dwa wywołania `tool_calls` — jedno dla `get_weather`, drugie dla `get_population`. Twój kod już to obsługuje, bo iteruje po wszystkich wywołaniach.

> **Spróbuj:** Zmodyfikuj wiadomość użytkownika i uruchom przykład ponownie. Czy model wywoła oba narzędzia?

---

### Ćwiczenie 6 — Dodaj własne narzędzie

Rozszerz jeden z przykładów o nowe narzędzie. Na przykład dodaj narzędzie `get_time`:

1. Zdefiniuj schemat narzędzia:
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

2. Dodaj logikę wykonania:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # W prawdziwej aplikacji użyj biblioteki stref czasowych
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... istniejące narzędzia ...
```

3. Dodaj narzędzie do tablicy `tools` i przetestuj pytaniem: "Która jest godzina w Tokio?"

> **Wyzwanie:** Dodaj narzędzie wykonujące obliczenia, np. `convert_temperature`, które konwertuje między stopniami Celsjusza a Fahrenheita. Przetestuj pytaniem: "Przelicz 100°F na Celsjusza."

---

### Ćwiczenie 7 — Wywoływanie narzędzi z ChatClient SDK (JavaScript)

Przykład JavaScript korzysta już z natywnego `ChatClient` SDK zamiast OpenAI SDK. To wygodna funkcja, która usuwa potrzebę manualnego tworzenia klienta OpenAI:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient jest tworzony bezpośrednio z obiektu modelu
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat akceptuje narzędzia jako drugi parametr
const response = await chatClient.completeChat(messages, tools);
```

Porównaj to z podejściem w Pythonie, które używa OpenAI SDK bezpośrednio:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Oba wzorce są poprawne. `ChatClient` jest bardziej wygodny; OpenAI SDK daje dostęp do pełnego zakresu parametrów OpenAI.

> **Spróbuj:** Zmodyfikuj przykład JavaScript, aby używać OpenAI SDK zamiast `ChatClient`. Będziesz potrzebować `import OpenAI from "openai"` i skonstruujesz klienta z endpointem `manager.urls[0]`.

---

### Ćwiczenie 8 — Zrozumienie `tool_choice`

Parametr `tool_choice` kontroluje, czy model *musi* użyć narzędzia, czy może wybierać dowolnie:

| Wartość | Zachowanie |
|---------|------------|
| `"auto"` | Model decyduje, czy wywołać narzędzie (domyślnie) |
| `"none"` | Model nie wywoła żadnego narzędzia, nawet jeśli są dostępne |
| `"required"` | Model musi wywołać przynajmniej jedno narzędzie |
| `{"type": "function", "function": {"name": "get_weather"}}` | Model musi wywołać określone narzędzie |

Wypróbuj każdą opcję w przykładzie Python:

```python
# Wymuś na modelu wywołanie get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Uwaga:** Nie wszystkie opcje `tool_choice` muszą być obsługiwane przez każdy model. Jeśli model nie obsługuje `"required"`, może zignorować tę opcję i działać jak `"auto"`.

---

## Typowe problemy

| Problem | Rozwiązanie |
|---------|-------------|
| Model nigdy nie wywołuje narzędzi | Upewnij się, że używasz modelu obsługującego wywoływanie narzędzi (np. qwen2.5-0.5b). Sprawdź powyższą tabelę. |
| Niezgodność `tool_call_id` | Zawsze używaj `id` z odpowiedzi wywołania narzędzia, nie wartości na sztywno |
| Model zwraca niepoprawny JSON w `arguments` | Mniejsze modele czasem generują nieprawidłowy JSON. Opakuj `JSON.parse()` w try/catch |
| Model wywołuje nieistniejące narzędzie | Dodaj obsługę domyślną w funkcji `execute_tool` |
| Nieskończona pętla wywoływania narzędzi | Ustaw maksymalną liczbę rund (np. 5), aby uniknąć zapętlenia |

---

## Najważniejsze informacje

1. **Wywoływanie narzędzi** pozwala modelom prosić o wykonanie funkcji zamiast zgadywania odpowiedzi
2. Model **nigdy nie wykonuje kodu**; to Twoja aplikacja decyduje, co uruchomić
3. Narzędzia definiuje się jako obiekty **JSON Schema** zgodne z formatem wywoływania funkcji OpenAI
4. Konwersacja przebiega w **wielu turach**: użytkownik, potem asystent (`tool_calls`), potem narzędzie (wyniki), potem asystent (odpowiedź końcowa)
5. Zawsze używaj **modelu obsługującego wywoływanie narzędzi** (Qwen 2.5, Phi-4-mini)
6. Metoda SDK `createChatClient()` to wygodny sposób na wywołania narzędzi bez konieczności tworzenia klienta OpenAI ręcznie

---

Kontynuuj do [Część 12: Budowanie interfejsu webowego dla Zava Creative Writer](part12-zava-ui.md), aby dodać przeglądarkowy frontend do wieloagentowego pipeline z transmisją na żywo.

---

[← Część 10: Modele niestandardowe](part10-custom-models.md) | [Część 12: Zava Writer UI →](part12-zava-ui.md)