# Instrukcje dla Agenta Kodującego

Ten plik zapewnia kontekst dla agentów kodujących AI (GitHub Copilot, Copilot Workspace, Codex itp.) pracujących w tym repozytorium.

## Przegląd Projektu

To jest **praktyczne warsztaty** na temat tworzenia aplikacji AI z [Foundry Local](https://foundrylocal.ai) — lekkim środowiskiem uruchomieniowym, które pobiera, zarządza i obsługuje modele językowe całkowicie na urządzeniu poprzez API zgodne z OpenAI. Warsztat zawiera przewodniki krok po kroku oraz gotowe do uruchomienia przykłady kodu w Python, JavaScript i C#.

## Struktura Repozytorium

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Szczegóły dotyczące języków i frameworków

### Python
- **Lokalizacja:** `python/`, `zava-creative-writer-local/src/api/`
- **Zależności:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Kluczowe pakiety:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Minimalna wersja:** Python 3.9+
- **Uruchomienie:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Lokalizacja:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Zależności:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Kluczowe pakiety:** `foundry-local-sdk`, `openai`
- **System modułów:** moduły ES (`.mjs` pliki, `"type": "module"`)
- **Minimalna wersja:** Node.js 18+
- **Uruchomienie:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Lokalizacja:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Pliki projektu:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Kluczowe pakiety:** `Microsoft.AI.Foundry.Local` (nie-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — nadzbiór z QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Cel:** .NET 9.0 (warunkowe TFM: `net9.0-windows10.0.26100` na Windows, `net9.0` gdzie indziej)
- **Uruchomienie:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Konwencje Kodowania

### Ogólne
- Wszystkie przykłady kodu to **samodzielne pliki przykładowe** — bez współdzielonych bibliotek narzędziowych czy abstrakcji.
- Każdy przykład działa niezależnie po zainstalowaniu swoich zależności.
- Klucze API zawsze ustawione są na `"foundry-local"` — Foundry Local używa tego jako symbol zastępczy.
- Bazowe adresy URL to `http://localhost:<port>/v1` — port jest dynamiczny i wykrywany podczas działania przez SDK (`manager.urls[0]` w JS, `manager.endpoint` w Pythonie).
- SDK Foundry Local zarządza uruchomieniem usługi i wykrywaniem punktów końcowych; zaleca się stosowanie wzorców SDK zamiast na sztywno ustalonych portów.

### Python
- Używaj SDK `openai` z `OpenAI(base_url=..., api_key="not-required")`.
- Użyj `FoundryLocalManager()` z `foundry_local` do zarządzania cyklem życia usługi przez SDK.
- Streaming: iteruj po obiekcie `stream` z `for chunk in stream:`.
- Brak anotacji typów w plikach przykładowych (zachowaj zwięzłość dla uczestników warsztatu).

### JavaScript
- Składnia modułów ES: `import ... from "..."`.
- Używaj `OpenAI` z `"openai"` oraz `FoundryLocalManager` z `"foundry-local-sdk"`.
- Wzorzec inicjalizacji SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Stosowane jest top-level `await`.

### C#
- Nullable włączone, implicit usings, .NET 9.
- Używaj `FoundryLocalManager.StartServiceAsync()` do zarządzania cyklem życia usługi przez SDK.
- Streaming: `CompleteChatStreaming()` z `foreach (var update in completionUpdates)`.
- Główny `csharp/Program.cs` jest routerem CLI, wywołującym statyczne metody `RunAsync()`.

### Wywoływanie narzędzi
- Wywoływanie narzędzi obsługują tylko niektóre modele: rodzina **Qwen 2.5** (`qwen2.5-*`) oraz **Phi-4-mini** (`phi-4-mini`).
- Schematy narzędzi stosują format JSON wywoływania funkcji kompatybilny z OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Konwersacja używa wzorca wielokrotnej wymiany: użytkownik → asystent (wywołania narzędzi) → narzędzie (wyniki) → asystent (odpowiedź końcowa).
- `tool_call_id` w wiadomościach wyników narzędzi musi odpowiadać `id` z wywołania narzędzia przez model.
- Python korzysta bezpośrednio z SDK OpenAI; JavaScript używa natywnego `ChatClient` SDK (`model.createChatClient()`); C# używa OpenAI SDK z `ChatTool.CreateFunctionTool()`.

### ChatClient (Natywny klient SDK)
- JavaScript: `model.createChatClient()` zwraca `ChatClient` z `completeChat(messages, tools?)` i `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` zwraca standardowy `ChatClient`, który można używać bez importu pakietu OpenAI NuGet.
- Python nie ma natywnego ChatClient — używaj SDK OpenAI wraz z `manager.endpoint` i `manager.api_key`.
- **Ważne:** w JavaScript `completeStreamingChat` używa **wzoru callback**, a nie asynchronicznej iteracji.

### Modele wnioskowania
- `phi-4-mini-reasoning` owija swoje rozumowanie w tagi `<think>...</think>` przed odpowiedzią końcową.
- Parsuj te tagi, by rozdzielić tok rozumowania od odpowiedzi, gdy jest to potrzebne.

## Przewodniki laboratoryjne

Pliki laboratoriów znajdują się w `labs/` w formacie Markdown. Mają spójną strukturę:
- Nagłówek z logo
- Tytuł i cel
- Przegląd, cele uczenia się, wymagania wstępne
- Sekcje wyjaśniające koncepcje z diagramami
- Numerowane ćwiczenia z blokami kodu i oczekiwanym wyjściem
- Tabela podsumowująca, kluczowe wnioski, dalsza lektura
- Link do następnej części

Podczas edycji treści laboratoriów:
- Zachowaj istniejące formatowanie Markdown i hierarchię sekcji.
- Bloki kodu muszą mieć określony język (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Podawaj warianty poleceń bash i PowerShell tam, gdzie system OS ma znaczenie.
- Używaj stylów wywołania `> **Note:**`, `> **Tip:**`, `> **Troubleshooting:**`.
- Tabele używają formatu słupkowego `| Nagłówek | Nagłówek |`.

## Komendy budowania i testowania

| Akcja | Komenda |
|--------|---------|
| **Przykłady Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Przykłady JS** | `cd javascript && npm install && node <script>.mjs` |
| **Przykłady C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generuj diagramy** | `npx mmdc -i <input>.mmd -o <output>.svg` (wymaga globalnej instalacji `npm install`) |

## Zewnętrzne zależności

- **Foundry Local CLI** musi być zainstalowany na maszynie deweloperskiej (`winget install Microsoft.FoundryLocal` lub `brew install foundrylocal`).
- **Usługa Foundry Local** działa lokalnie i udostępnia REST API kompatybilne z OpenAI na dynamicznym porcie.
- Żadne usługi chmurowe, klucze API ani subskrypcje Azure nie są wymagane do uruchomienia któregokolwiek przykładu.
- Część 10 (modele niestandardowe) dodatkowo wymaga `onnxruntime-genai` i pobiera wagi modeli z Hugging Face.

## Pliki, których nie należy commitować

Plik `.gitignore` powinien wykluczać (i wyklucza w większości):
- `.venv/` — wirtualne środowiska Pythona
- `node_modules/` — zależności npm
- `models/` — skompilowane modele ONNX (duże pliki binarne, generowane w części 10)
- `cache_dir/` — cache pobierania modeli Hugging Face
- `.olive-cache/` — katalog roboczy Microsoft Olive
- `samples/audio/*.wav` — wygenerowane próbki audio (odtwarzane przez `python samples/audio/generate_samples.py`)
- Standardowe artefakty budowania Pythona (`__pycache__/`, `*.egg-info/`, `dist/`, itd.)

## Licencja

MIT — zobacz `LICENSE`.