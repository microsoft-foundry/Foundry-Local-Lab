<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Warsztaty Foundry Local - Twórz aplikacje AI na urządzeniu

Warsztaty praktyczne dotyczące uruchamiania modeli językowych na własnym komputerze i tworzenia inteligentnych aplikacji z użyciem [Foundry Local](https://foundrylocal.ai) oraz [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Czym jest Foundry Local?** Foundry Local to lekki runtime, który umożliwia pobieranie, zarządzanie i udostępnianie modeli językowych bezpośrednio na Twoim sprzęcie. Udostępnia **API kompatybilne z OpenAI**, dzięki czemu każde narzędzie lub SDK zgodne z OpenAI może się połączyć – nie jest wymagane konto w chmurze.

---

## Cele nauki

Po ukończeniu warsztatów będziesz potrafił:

| # | Cel |
|---|-----------|
| 1 | Zainstalować Foundry Local i zarządzać modelami za pomocą CLI |
| 2 | Opanować API Foundry Local SDK do programowego zarządzania modelami |
| 3 | Połączyć się z lokalnym serwerem inferencji korzystając z SDK dla Pythona, JavaScript i C# |
| 4 | Zbudować pipeline Retrieval-Augmented Generation (RAG) wykorzystujący Twoje dane jako podstawę odpowiedzi |
| 5 | Tworzyć agentów AI z utrwalonymi instrukcjami i personami |
| 6 | Organizować wieloagentowe workflow z pętlami zwrotnymi |
| 7 | Poznać produkcyjną aplikację końcową – Zava Creative Writer |
| 8 | Budować ramy ewaluacji z użyciem złotych zbiorów danych i oceny LLM-jako-sędzia |
| 9 | Transkrybować audio za pomocą Whisper - rozpoznawanie mowy na urządzeniu z użyciem Foundry Local SDK |
| 10 | Kompilować i uruchamiać niestandardowe lub Hugging Face modele z ONNX Runtime GenAI i Foundry Local |
| 11 | Umożliwiać lokalnym modelom wywoływanie zewnętrznych funkcji wzorcem tool-calling |
| 12 | Zbudować interfejs przeglądarkowy dla Zava Creative Writer z transmisją strumieniową w czasie rzeczywistym |

---

## Wymagania wstępne

| Wymaganie | Szczegóły |
|-------------|---------|
| **Sprzęt** | Minimum 8 GB RAM (zalecane 16 GB); procesor z obsługą AVX2 lub obsługiwana karta GPU |
| **System operacyjny** | Windows 10/11 (x64/ARM), Windows Server 2025 lub macOS 13+ |
| **Foundry Local CLI** | Instalacja przez `winget install Microsoft.FoundryLocal` (Windows) lub `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Szczegóły w [przewodniku rozpoczęcia](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| **Środowisko językowe** | **Python 3.9+** i/lub **.NET 9.0+** i/lub **Node.js 18+** |
| **Git** | Do sklonowania tego repozytorium |

---

## Rozpoczęcie pracy

```bash
# 1. Sklonuj repozytorium
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Sprawdź, czy Foundry Local jest zainstalowany
foundry model list              # Wyświetl dostępne modele
foundry model run phi-3.5-mini  # Rozpocznij interaktywną rozmowę

# 3. Wybierz swój językowy ścieżkę (zobacz laboratorium część 2, aby uzyskać pełną konfigurację)
```

| Język | Szybki start |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Części warsztatu

### Część 1: Rozpoczęcie pracy z Foundry Local

**Przewodnik laboratorium:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Czym jest Foundry Local i jak działa
- Instalacja CLI na Windows i macOS
- Eksploracja modeli – wyświetlanie, pobieranie, uruchamianie
- Rozumienie aliasów modeli i dynamicznych portów

---

### Część 2: Głębokie zanurzenie w Foundry Local SDK

**Przewodnik laboratorium:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Dlaczego używać SDK zamiast CLI do tworzenia aplikacji
- Pełna referencja API SDK dla Pythona, JavaScript oraz C#
- Zarządzanie usługą, przegląd katalogu, cykl życia modelu (pobieranie, ładowanie, zwalnianie)
- Wzorce szybkiego startu: bootstrap konstruktora w Pythonie, `init()` w JavaScript, `CreateAsync()` w C#
- Metadane `FoundryModelInfo`, aliasy i wybór modelu optymalnego dla sprzętu

---

### Część 3: SDK i API

**Przewodnik laboratorium:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Łączenie się z Foundry Local z Pythona, JavaScript i C#
- Programowe zarządzanie usługą przy pomocy Foundry Local SDK
- Strumieniowanie uzupełnień czatu przez API kompatybilne z OpenAI
- Referencja metod SDK dla każdego języka

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Podstawowy czat strumieniowy |
| C# | `csharp/BasicChat.cs` | Czat strumieniowy w .NET |
| JavaScript | `javascript/foundry-local.mjs` | Czat strumieniowy w Node.js |

---

### Część 4: Retrieval-Augmented Generation (RAG)

**Przewodnik laboratorium:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Czym jest RAG i dlaczego jest ważny
- Budowanie bazy wiedzy w pamięci
- Pobieranie na podstawie nakładania się słów kluczowych z ocenianiem
- Komponowanie ugruntowanych promptów systemowych
- Uruchomienie pełnego pipeline RAG na urządzeniu

**Przykłady kodu:**

| Język | Plik |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Część 5: Tworzenie Agentów AI

**Przewodnik laboratorium:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Czym jest agent AI (w porównaniu do surowego wywołania LLM)
- Wzorzec `ChatAgent` i Microsoft Agent Framework
- Instrukcje systemowe, persony i wieloetapowe konwersacje
- Strukturacja wyjścia (JSON) od agentów

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Jeden agent z Agent Framework |
| C# | `csharp/SingleAgent.cs` | Jeden agent (wzorzec ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Jeden agent (wzorzec ChatAgent) |

---

### Część 6: Wieloagentowe Workflow

**Przewodnik laboratorium:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline wieloagentowy: Badacz → Pisarz → Redaktor
- Sekwencyjna orkiestracja i pętle zwrotne
- Wspólna konfiguracja i uporządkowane przekazywanie danych
- Projektowanie własnego workflow wieloagentowego

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline z trzema agentami |
| C# | `csharp/MultiAgent.cs` | Pipeline z trzema agentami |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline z trzema agentami |

---

### Część 7: Zava Creative Writer - Aplikacja Końcowa

**Przewodnik laboratorium:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Produkcyjna aplikacja z wieloma agentami i 4 wyspecjalizowanymi agentami
- Pipeline sekwencyjny z pętlami zwrotnymi kierowanymi przez ewaluatora
- Strumieniowe wyjście, wyszukiwanie w katalogu produktów, przekazywanie uporządkowanego JSON
- Pełna implementacja w Pythonie (FastAPI), JavaScript (Node.js CLI) i C# (konsola .NET)

**Przykłady kodu:**

| Język | Folder | Opis |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Serwis WWW FastAPI z orkiestratorem |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplikacja CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplikacja konsolowa .NET 9 |

---

### Część 8: Rozwój Kierowany Ewaluacją

**Przewodnik laboratorium:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Buduj systematyczne ramy ewaluacyjne agentów AI z użyciem złotych zbiorów danych
- Reguły oparte na sprawdzeniach (długość, zasięg słów kluczowych, zabronione terminy) + oceny LLM-jako-sędzia
- Porównanie wariantów promptów z agregowanymi kartami wyników
- Rozszerza wzorzec agenta Zava Editor z części 7 do testów offline
- Trasy w Pythonie, JavaScript i C#

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Rama ewaluacji |
| C# | `csharp/AgentEvaluation.cs` | Rama ewaluacji |
| JavaScript | `javascript/foundry-local-eval.mjs` | Rama ewaluacji |

---

### Część 9: Transkrypcja Głosu za pomocą Whisper

**Przewodnik laboratorium:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Transkrypcja mowy na tekst z wykorzystaniem lokalnej wersji OpenAI Whisper
- Przetwarzanie audio z dbałością o prywatność - audio nigdy nie opuszcza urządzenia
- Trasy Python, JavaScript i C# z `client.audio.transcriptions.create()` (Python/JS) i `AudioClient.TranscribeAudioAsync()` (C#)
- Zawiera próbki audio o tematyce Zava do ćwiczeń praktycznych

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transkrypcja głosu Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transkrypcja głosu Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transkrypcja głosu Whisper |

> **Uwaga:** To laboratorium korzysta z **Foundry Local SDK** do programowego pobierania i ładowania modelu Whisper, a następnie wysyła audio do lokalnego punktu końcowego kompatybilnego z OpenAI w celu transkrypcji. Model Whisper (`whisper`) jest wymieniony w katalogu Foundry Local i działa całkowicie na urządzeniu – nie są potrzebne klucze API chmury ani dostęp do sieci.

---

### Część 10: Używanie Niestandardowych lub Hugging Face Modeli

**Przewodnik laboratorium:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilacja modeli Hugging Face do zoptymalizowanego formatu ONNX za pomocą narzędzia ONNX Runtime GenAI
- Kompilacja sprzętowo-specyficzna (CPU, NVIDIA GPU, DirectML, WebGPU) oraz kwantyzacja (int4, fp16, bf16)
- Tworzenie plików konfiguracji szablonów czatu dla Foundry Local
- Dodawanie skompilowanych modeli do pamięci podręcznej Foundry Local
- Uruchamianie niestandardowych modeli przez CLI, REST API i OpenAI SDK
- Przykład referencyjny: kompilacja Qwen/Qwen3-0.6B end-to-end

---

### Część 11: Wywoływanie Narzędzi przez Lokalne Modele

**Przewodnik laboratorium:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Umożliwienie lokalnym modelom wywoływania zewnętrznych funkcji (tool/function calling)
- Definiowanie schematów narzędzi zgodnie z formatem OpenAI function-calling
- Obsługa konwersacji tool-calling wielokrotnego kroku
- Wykonywanie wywołań narzędzi lokalnie i zwracanie wyników modelowi
- Dobór odpowiedniego modelu do scenariuszy tool-calling (Qwen 2.5, Phi-4-mini)
- Korzystanie z natywnego `ChatClient` SDK do wywoływania narzędzi (JavaScript)

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Wywoływanie narzędzi pogodowych/populacyjnych |
| C# | `csharp/ToolCalling.cs` | Wywoływanie narzędzi w .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Wywoływanie narzędzi z ChatClient |

---

### Część 12: Budowanie Interfejsu WWW dla Zava Creative Writer

**Przewodnik laboratorium:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Dodanie frontendu przeglądarkowego do Zava Creative Writer
- Serwowanie wspólnego UI z Pythona (FastAPI), JavaScript (Node.js HTTP) i C# (ASP.NET Core)
- Konsumpcja strumienia NDJSON w przeglądarce za pomocą Fetch API i ReadableStream
- Statusy agentów na żywo i transmisja tekstu artykułu w czasie rzeczywistym

**Kod (wspólny interfejs):**

| Plik | Opis |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Układ strony |
| `zava-creative-writer-local/ui/style.css` | Stylizacja |
| `zava-creative-writer-local/ui/app.js` | Czytnik strumienia i logika aktualizacji DOM |

**Dodatki backendowe:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Zaktualizowany do serwowania statycznego UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nowy serwer HTTP opakowujący orkiestratora |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nowy projekt minimalnego API ASP.NET Core |

---

### Część 13: Warsztaty zakończone
**Przewodnik po laboratorium:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Podsumowanie wszystkiego, co zbudowałeś we wszystkich 12 częściach
- Dalsze pomysły na rozszerzenie Twoich aplikacji
- Linki do zasobów i dokumentacji

---

## Struktura projektu

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## Zasoby

| Zasób | Link |
|----------|------|
| Strona Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Katalog modeli | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local na GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Przewodnik rozpoczęcia pracy | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referencje SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licencja

Ten materiał warsztatowy jest udostępniony do celów edukacyjnych.

---

**Szczęśliwego budowania! 🚀**