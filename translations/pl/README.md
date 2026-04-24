<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Warsztat Foundry Local – Twórz aplikacje AI na urządzeniu

Praktyczny warsztat uruchamiania modeli językowych na własnym komputerze i tworzenia inteligentnych aplikacji za pomocą [Foundry Local](https://foundrylocal.ai) oraz [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Czym jest Foundry Local?** Foundry Local to lekki runtime, który pozwala pobierać, zarządzać i udostępniać modele językowe całkowicie na twoim sprzęcie. Udostępnia **kompatybilne z OpenAI API**, dzięki czemu dowolne narzędzie lub SDK obsługujące OpenAI może się połączyć - nie jest wymagane konto w chmurze.

### 🌐 Wielojęzyczne wsparcie

#### Wspierane przez GitHub Action (Automatyczne i zawsze aktualne)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabski](../ar/README.md) | [Bengalski](../bn/README.md) | [Bułgarski](../bg/README.md) | [Birmański (Myanmar)](../my/README.md) | [Chiński (uproszczony)](../zh-CN/README.md) | [Chiński (tradycyjny, Hongkong)](../zh-HK/README.md) | [Chiński (tradycyjny, Makau)](../zh-MO/README.md) | [Chiński (tradycyjny, Tajwan)](../zh-TW/README.md) | [Chorwacki](../hr/README.md) | [Czeski](../cs/README.md) | [Duński](../da/README.md) | [Niderlandzki](../nl/README.md) | [Estoński](../et/README.md) | [Fiński](../fi/README.md) | [Francuski](../fr/README.md) | [Niemiecki](../de/README.md) | [Grecki](../el/README.md) | [Hebrajski](../he/README.md) | [Hindi](../hi/README.md) | [Węgierski](../hu/README.md) | [Indonezyjski](../id/README.md) | [Włoski](../it/README.md) | [Japoński](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Koreański](../ko/README.md) | [Litewski](../lt/README.md) | [Malajski](../ms/README.md) | [Malajalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepalski](../ne/README.md) | [Nigeryjski pidgin](../pcm/README.md) | [Norweski](../no/README.md) | [Perski (Farsi)](../fa/README.md) | [Polski](./README.md) | [Portugalski (Brazylia)](../pt-BR/README.md) | [Portugalski (Portugalia)](../pt-PT/README.md) | [Pendżabski (Gurmukhi)](../pa/README.md) | [Rumuński](../ro/README.md) | [Rosyjski](../ru/README.md) | [Serbski (cyrylica)](../sr/README.md) | [Słowacki](../sk/README.md) | [Słoweński](../sl/README.md) | [Hiszpański](../es/README.md) | [Suahili](../sw/README.md) | [Szwedzki](../sv/README.md) | [Tagalog (Filipiński)](../tl/README.md) | [Tamilski](../ta/README.md) | [Telugu](../te/README.md) | [Tajski](../th/README.md) | [Turecki](../tr/README.md) | [Ukraiński](../uk/README.md) | [Urdu](../ur/README.md) | [Wietnamski](../vi/README.md)

> **Wolisz klonować lokalnie?**
>
> To repozytorium zawiera ponad 50 tłumaczeń językowych, co znacznie zwiększa wielkość pobierania. Aby sklonować bez tłumaczeń, użyj sparse checkout:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> Dzięki temu masz wszystko, co potrzebne do ukończenia kursu, przy znacznie szybszym pobieraniu.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Cele nauki

Pod koniec tego warsztatu będziesz w stanie:

| # | Cel |
|---|-----------|
| 1 | Zainstalować Foundry Local i zarządzać modelami za pomocą CLI |
| 2 | Opanować API Foundry Local SDK do programowego zarządzania modelami |
| 3 | Łączyć się z lokalnym serwerem inferencji używając SDK Python, JavaScript i C# |
| 4 | Zbudować pipeline Retrieval-Augmented Generation (RAG), który bazuje odpowiedzi na własnych danych |
| 5 | Tworzyć agentów AI z trwałymi instrukcjami i personami |
| 6 | Orkiestracja workflow wieloagentowych z pętlami zwrotnymi |
| 7 | Poznać produkcyjną aplikację końcową – Zava Creative Writer |
| 8 | Budować frameworki ewaluacyjne ze złotymi zbiorami danych oraz scoringiem LLM jako sędzia |
| 9 | Transkrybować audio za pomocą Whisper – zamiana mowy na tekst na urządzeniu z użyciem Foundry Local SDK |
| 10 | Kompilować i uruchamiać własne lub Hugging Face modele z ONNX Runtime GenAI i Foundry Local |
| 11 | Umożliwić lokalnym modelom wywoływanie funkcji zewnętrznych za pomocą wzorca tool-calling |
| 12 | Tworzyć UI w przeglądarce dla Zava Creative Writer z transmisją w czasie rzeczywistym |

---

## Wymagania wstępne

| Wymaganie | Szczegóły |
|-------------|---------|
| **Sprzęt** | Minimum 8 GB RAM (zalecane 16 GB); procesor obsługujący AVX2 lub obsługiwana GPU |
| **System operacyjny** | Windows 10/11 (x64/ARM), Windows Server 2025 lub macOS 13+ |
| **Foundry Local CLI** | Instalacja przez `winget install Microsoft.FoundryLocal` (Windows) lub `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Szczegóły w [przewodniku startowym](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Środowisko uruchomieniowe** | **Python 3.9+** i/lub **.NET 9.0+** i/lub **Node.js 18+** |
| **Git** | Do klonowania tego repozytorium |

---

## Rozpoczęcie pracy

```bash
# 1. Sklonuj repozytorium
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Sprawdź, czy Foundry Local jest zainstalowany
foundry model list              # Wyświetl dostępne modele
foundry model run phi-3.5-mini  # Rozpocznij interaktywną rozmowę

# 3. Wybierz swój język (zobacz laboratorium Część 2, aby uzyskać pełną konfigurację)
```

| Język | Szybki start |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Części warsztatu

### Część 1: Rozpoczęcie pracy z Foundry Local

**Przewodnik laboraotryjny:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Czym jest Foundry Local i jak działa
- Instalacja CLI w Windows i macOS
- Eksploracja modeli – lista, pobieranie, uruchamianie
- Zrozumienie aliasów modeli i dynamicznych portów

---

### Część 2: Dogłębne poznanie Foundry Local SDK

**Przewodnik laboraotryjny:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Dlaczego używać SDK zamiast CLI do tworzenia aplikacji
- Pełna referencja API SDK dla Pythona, JavaScript i C#
- Zarządzanie usługą, przeglądanie katalogu, cykl życia modelu (pobieranie, ładowanie, zwalnianie)
- Szybkie wzorce startowe: Python konstruktor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- Metadane `FoundryModelInfo`, aliasy i wybór modeli optymalnych dla sprzętu

---

### Część 3: SDK i API

**Przewodnik laboraotryjny:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Łączenie się z Foundry Local za pomocą Python, JavaScript i C#
- Programowe zarządzanie serwisem poprzez Foundry Local SDK
- Transmisja na żywo uzupełnień czatu przez OpenAI-kompatybilne API
- Referencja metod SDK dla każdego języka

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Podstawowy streaming czatu |
| C# | `csharp/BasicChat.cs` | Streaming czatu w .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming czatu w Node.js |

---

### Część 4: Retrieval-Augmented Generation (RAG)

**Przewodnik laboraotryjny:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Czym jest RAG i dlaczego jest ważne
- Budowanie w pamięci podręcznej bazy wiedzy
- Pobieranie na podstawie słów kluczowych z punktacją
- Komponowanie ugruntowanych promptów systemowych
- Uruchamianie kompletnego pipeline RAG na urządzeniu

**Przykłady kodu:**

| Język | Plik |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Część 5: Tworzenie agentów AI

**Przewodnik laboraotryjny:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Czym jest agent AI (w przeciwieństwie do prostego wywołania LLM)
- Wzorzec `ChatAgent` i Microsoft Agent Framework
- Instrukcje systemowe, persony oraz rozmowy wieloturnowe
- Strukturalne dane wyjściowe (JSON) z agentów

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Pojedynczy agent z Agent Framework |
| C# | `csharp/SingleAgent.cs` | Pojedynczy agent (wzorzec ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Pojedynczy agent (wzorzec ChatAgent) |

---

### Część 6: Workflow wieloagentowe

**Przewodnik laboraotryjny:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline wieloagentowy: Researcher → Writer → Editor
- Sekwencyjna orkiestracja i pętle zwrotne
- Wspólna konfiguracja i strukturalne przekazania
- Projektowanie własnego workflow wieloagentowego

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline trzyeagentowy |
| C# | `csharp/MultiAgent.cs` | Pipeline trzech agentów |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline trzech agentów |

---

### Część 7: Zava Creative Writer – aplikacja końcowa

**Przewodnik laboraotryjny:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Produkcyjna aplikacja wieloagentowa z 4 wyspecjalizowanymi agentami
- Sekwencyjny pipeline z pętlami zwrotnymi prowadzonymi przez evaluatorów
- Transmisja na żywo, wyszukiwanie w katalogu produktów, strukturalne przekazania JSON
- Pełna implementacja w Python (FastAPI), JavaScript (Node.js CLI) i C# (.NET konsola)

**Przykłady kodu:**

| Język | Katalog | Opis |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Serwis FastAPI z orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplikacja CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Konsolowa aplikacja .NET 9 |

---

### Część 8: Rozwój prowadzony przez ewaluację

**Przewodnik laboraotryjny:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Budowa systematycznych frameworków ewaluacyjnych dla agentów AI z użyciem złotych danych
- Kontrole oparte na regułach (długość, pokrycie słów kluczowych, zakazane terminy) + scoring LLM jako sędzia
- Porównanie wariantów promptów obok siebie z agregowanymi scorecardami
- Rozszerzenie wzorca agenta Zava Editor z części 7 w zestaw testowy offline
- Ścieżki dla Pythona, JavaScript i C#

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Framework ewaluacyjny |
| C# | `csharp/AgentEvaluation.cs` | Framework ewaluacyjny |
| JavaScript | `javascript/foundry-local-eval.mjs` | Framework ewaluacyjny |

---

### Część 9: Transkrypcja głosu z Whisper

**Przewodnik laboraotryjny:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Transkrypcja mowy na tekst za pomocą OpenAI Whisper działającego lokalnie
- Przetwarzanie dźwięku z naciskiem na prywatność – dźwięk nigdy nie opuszcza Twojego urządzenia
- Ścieżki Python, JavaScript i C# z `client.audio.transcriptions.create()` (Python/JS) oraz `AudioClient.TranscribeAudioAsync()` (C#)
- Zawiera przykładowe pliki dźwiękowe tematyczne Zava do praktyki

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transkrypcja głosu Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transkrypcja głosu Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transkrypcja głosu Whisper |

> **Uwaga:** To laboratorium używa **Foundry Local SDK** do programowego pobrania i załadowania modelu Whisper, a następnie wysyła dźwięk do lokalnego punktu końcowego zgodnego z OpenAI w celu transkrypcji. Model Whisper (`whisper`) jest wymieniony w katalogu Foundry Local i działa całkowicie na urządzeniu – nie jest wymagany żaden klucz API chmury ani dostęp do sieci.

---

### Część 10: Używanie własnych lub modeli Hugging Face

**Przewodnik po laboratorium:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilowanie modeli Hugging Face do zoptymalizowanego formatu ONNX z wykorzystaniem ONNX Runtime GenAI model builder
- Kompilacja sprzętowa specyficzna dla CPU, NVIDIA GPU, DirectML, WebGPU oraz kwantyzacja (int4, fp16, bf16)
- Tworzenie plików konfiguracyjnych szablonów czatu dla Foundry Local
- Dodawanie skompilowanych modeli do cache Foundry Local
- Uruchamianie własnych modeli przez CLI, REST API oraz OpenAI SDK
- Przykład referencyjny: kompletna kompilacja Qwen/Qwen3-0.6B

---

### Część 11: Wywoływanie narzędzi z lokalnymi modelami

**Przewodnik po laboratorium:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Umożliwienie lokalnym modelom wywoływania zewnętrznych funkcji (wywoływanie narzędzi/funkcji)
- Definiowanie schematów narzędzi zgodnie z formatem wywoływania funkcji OpenAI
- Obsługa konwersacji z wieloma turami wywoływania narzędzi
- Wykonywanie wywołań narzędzi lokalnie i zwracanie wyników do modelu
- Dobór właściwego modelu do scenariuszy wywoływania narzędzi (Qwen 2.5, Phi-4-mini)
- Wykorzystanie natywnego `ChatClient` SDK do wywoływania narzędzi (JavaScript)

**Przykłady kodu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Wywoływanie narzędzi pogodowych/populacyjnych |
| C# | `csharp/ToolCalling.cs` | Wywoływanie narzędzi w .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Wywoływanie narzędzi za pomocą ChatClient |

---

### Część 12: Budowa interfejsu webowego dla Zava Creative Writer

**Przewodnik po laboratorium:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Dodanie przeglądarkowego front-endu do Zava Creative Writer
- Serwowanie wspólnego interfejsu z Pythona (FastAPI), JavaScript (Node.js HTTP) i C# (ASP.NET Core)
- Konsumowanie strumieniowego NDJSON w przeglądarce z użyciem Fetch API i ReadableStream
- Statusy agenta na żywo i strumieniowe wyświetlanie tekstu artykułów w czasie rzeczywistym

**Kod (wspólny UI):**

| Plik | Opis |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Układ strony |
| `zava-creative-writer-local/ui/style.css` | Stylizacja |
| `zava-creative-writer-local/ui/app.js` | Logika czytania strumienia i aktualizacji DOM |

**Dodatki do backendu:**

| Język | Plik | Opis |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Zaktualizowany do serwowania statycznego UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nowy serwer HTTP opakowujący orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nowy minimalny projekt API ASP.NET Core |

---

### Część 13: Zakończenie warsztatów

**Przewodnik po laboratorium:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Podsumowanie wszystkiego, co zbudowałeś przez 12 części
- Dalsze pomysły na rozwijanie Twoich aplikacji
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
| Przewodnik startowy | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referencje SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licencja

Materiał warsztatowy udostępniony jest do celów edukacyjnych.

---

**Powodzenia w tworzeniu! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Zastrzeżenie**:  
Niniejszy dokument został przetłumaczony za pomocą usługi tłumaczeń AI [Co-op Translator](https://github.com/Azure/co-op-translator). Choć dążymy do dokładności, prosimy pamiętać, że automatyczne tłumaczenia mogą zawierać błędy lub nieścisłości. Oryginalny dokument w języku źródłowym należy traktować jako źródło wiarygodne. W przypadku informacji krytycznych zalecane jest skorzystanie z profesjonalnego tłumaczenia wykonanego przez człowieka. Nie ponosimy odpowiedzialności za wszelkie nieporozumienia lub błędne interpretacje wynikające z korzystania z tego tłumaczenia.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->