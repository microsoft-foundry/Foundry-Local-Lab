# Zmiany — Foundry Local Workshop

Wszystkie istotne zmiany w tym warsztacie są opisane poniżej.

---

## 2026-03-11 — Części 12 i 13, Web UI, Przepisanie Whisper, Poprawka WinML/QNN oraz Walidacja

### Dodano
- **Część 12: Budowanie Web UI dla Zava Creative Writer** — nowy przewodnik laboratoryjny (`labs/part12-zava-ui.md`) z ćwiczeniami dotyczącymi strumieniowania NDJSON, przeglądarkowego `ReadableStream`, znaczników statusu agenta na żywo oraz strumieniowania tekstu artykułu w czasie rzeczywistym
- **Część 13: Warsztat ukończony** — nowy podsumowujący przewodnik laboratoryjny (`labs/part13-workshop-complete.md`) z przeglądem wszystkich 12 części, dalszymi pomysłami i linkami do zasobów
- **Interfejs front-end Zava UI:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — wspólny przeglądarkowy interfejs HTML/CSS/JS vanilla używany przez wszystkie trzy backendy
- **Serwer HTTP w JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — nowy serwer HTTP w stylu Express opakowujący orchestrator dla dostępu z przeglądarki
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` i `ZavaCreativeWriterWeb.csproj` — nowy minimalny projekt API serwujący UI i strumieniowanie NDJSON
- **Generator próbek audio:** `samples/audio/generate_samples.py` — offline'owy skrypt TTS używający `pyttsx3` do generowania plików WAV tematyki Zava dla Części 9
- **Próbka audio:** `samples/audio/zava-full-project-walkthrough.wav` — nowa dłuższa próbka audio do testowania transkrypcji
- **Skrypt walidacyjny:** `validate-npu-workaround.ps1` — automatyczny skrypt PowerShell do weryfikacji obejścia NPU/QNN dla wszystkich próbek C#
- **Diagramy Mermaid SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Wsparcie WinML wieloplatformowe:** Wszystkie 3 pliki `.csproj` C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) teraz używają warunkowego TFM i wzajemnie wykluczających się referencji pakietów dla wsparcia wieloplatformowego. Na Windowsie: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (nadrzędny zestaw zawierający wtyczkę QNN EP). Na systemach nie-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (podstawowy SDK). Twardo zakodowany RID `win-arm64` w projektach Zava został zastąpiony automatycznym wykrywaniem. Przejściowe obejście zależności wyklucza zasoby natywne z `Microsoft.ML.OnnxRuntime.Gpu.Linux`, który ma uszkodzony odnośnik do win-arm64. Poprzednie obejście NPU przez try/catch zostało usunięte ze wszystkich 7 plików C#.

### Zmieniono
- **Część 9 (Whisper):** Znaczne przepisanie — JavaScript używa teraz wbudowanego `AudioClient` SDK (`model.createAudioClient()`) zamiast ręcznej inferencji ONNX Runtime; zaktualizowano opisy architektury, tabele porównawcze i diagramy pipeline dla podejścia JS/C# `AudioClient` vs podejścia Python ONNX Runtime
- **Część 11:** Zaktualizowano linki nawigacyjne (teraz prowadzą do Części 12); dodano wyrenderowane diagramy SVG dla przepływu wywoływania narzędzi i sekwencji
- **Część 10:** Zmieniono nawigację, która prowadzi przez Część 12 zamiast kończyć warsztat
- **Python Whisper (`foundry-local-whisper.py`):** Rozszerzony o dodatkowe próbki audio i ulepszone obsługi błędów
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Przepisany do użycia `model.createAudioClient()` z `audioClient.transcribe()` zamiast ręcznych sesji ONNX Runtime
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Zaktualizowany do serwowania statycznych plików UI wraz z API
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** Usunięto obejście NPU (teraz obsługiwane przez pakiet WinML)
- **README.md:** Dodano sekcję Część 12 z tabelami przykładów kodu i dodatkami backendu; dodano sekcję Część 13; zaktualizowano cele nauki i strukturę projektu
- **KNOWN-ISSUES.md:** Usunięto rozwiązany Problem #7 (wariant Modelu NPU w SDK C# — teraz obsługiwany przez pakiet WinML). Przenumerowano pozostałe problemy na #1–#6. Zaktualizowano szczegóły środowiska z .NET SDK 10.0.104
- **AGENTS.md:** Zaktualizowano strukturę katalogów projektu o nowe wpisy `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); zaktualizowano kluczowe pakiety C# i szczegóły warunkowego TFM
- **labs/part2-foundry-local-sdk.md:** Zaktualizowano przykład `.csproj` pokazujący pełen wzorzec wieloplatformowy z warunkowym TFM, wzajemnie wykluczającymi się referencjami pakietów i objaśniającą notatką

### Zweryfikowano
- Wszystkie 3 projekty C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) kompilują się pomyślnie na Windows ARM64
- Przykład czatu (`dotnet run chat`): model ładuje się jako `phi-3.5-mini-instruct-qnn-npu:1` przez WinML/QNN — wariant NPU ładuje się bezpośrednio bez przejścia zapasowego na CPU
- Przykład agenta (`dotnet run agent`): działa end-to-end z wielokrotną konwersacją, kod zakończenia 0
- Foundry Local CLI v0.8.117 i SDK v0.9.0 na .NET SDK 9.0.312

---

## 2026-03-11 — Poprawki kodu, oczyszczenie modelu, diagramy Mermaid i walidacja

### Naprawiono
- **Wszystkie 21 przykładów kodu (7 Python, 7 JavaScript, 7 C#):** Dodano `model.unload()` / `unload_model()` / `model.UnloadAsync()` sprzątanie przy wyjściu, aby rozwiązać ostrzeżenia o wycieku pamięci OGA (Znany Problem #4)
- **csharp/WhisperTranscription.cs:** Zastąpiono niestabilną ścieżkę względną `AppContext.BaseDirectory` funkcją `FindSamplesDirectory()`, która odszukuje `samples/audio` niezawodnie, wspinając się po katalogach (Znany Problem #7)
- **csharp/csharp.csproj:** Zastąpiono twardo zakodowany `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` automatycznym wykrywaniem zapasowym z użyciem `$(NETCoreSdkRuntimeIdentifier)`, aby `dotnet run` działał na dowolnej platformie bez flagi `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Zmieniono
- **Część 8:** Przekształcono pętlę iteracyjną opartą na eval z diagramu ASCII box na wyrenderowany obraz SVG
- **Część 10:** Przekształcono diagram pipeline kompilacji z ASCII strzałek na wyrenderowany obraz SVG
- **Część 11:** Przekształcono diagramy przepływu wywoływania narzędzi i sekwencji na wyrenderowane obrazy SVG
- **Część 10:** Przeniesiono sekcję „Warsztat ukończony!” do Części 11 (ostateczne laboratorium); zastąpiono linkiem „Kolejne kroki”
- **KNOWN-ISSUES.md:** Pełna rewalidacja wszystkich problemów względem CLI v0.8.117. Usunięto rozwiązane: wyciek pamięci OGA (dodano sprzątanie), ścieżka Whisper (FindSamplesDirectory), utrzymujący się HTTP 500 podczas inferencji (nierozpoznany, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), ograniczenia `tool_choice` (teraz działa z `"required"` i specyficznym kierowaniem funkcji na qwen2.5-0.5b). Zaktualizowano problem JS Whisper — teraz wszystkie pliki zwracają pusty/binarny wynik (regresja od v0.9.x, podniesiona powaga do Major). Zaktualizowano #4 RID C# z obejściem automatycznego wykrywania i linkiem [#497](https://github.com/microsoft/Foundry-Local/issues/497). Pozostało 7 otwartych problemów.
- **javascript/foundry-local-whisper.mjs:** Naprawiono nazwę zmiennej sprzątającej (`whisperModel` → `model`)

### Zweryfikowano
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — uruchamiają się pomyślnie ze sprzątaniem
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — uruchamiają się pomyślnie ze sprzątaniem
- C#: `dotnet build` przechodzi bez ostrzeżeń i błędów (cel net9.0)
- Wszystkie 7 plików Pythona przechodzi kontrolę składni `py_compile`
- Wszystkie 7 plików JavaScript przechodzi walidację składni `node --check`

---

## 2026-03-10 — Część 11: Wywoływanie narzędzi, Rozszerzenie SDK API i Pokrycie modelu

### Dodano
- **Część 11: Wywoływanie narzędzi z lokalnymi modelami** — nowy przewodnik laboratoryjny (`labs/part11-tool-calling.md`) z 8 ćwiczeniami dotyczącymi schematów narzędzi, przepływu wieloetapowego, wielokrotnych wywołań narzędzi, narzędzi niestandardowych, wywoływania narzędzi przez ChatClient oraz `tool_choice`
- **Przykład Python:** `python/foundry-local-tool-calling.py` — wywoływanie narzędzi `get_weather`/`get_population` przy użyciu OpenAI SDK
- **Przykład JavaScript:** `javascript/foundry-local-tool-calling.mjs` — wywoływanie narzędzi przy użyciu natywnego `ChatClient` SDK (`model.createChatClient()`)
- **Przykład C#:** `csharp/ToolCalling.cs` — wywoływanie narzędzi przy użyciu `ChatTool.CreateFunctionTool()` z OpenAI C# SDK
- **Część 2, Ćwiczenie 7:** Natywny `ChatClient` — `model.createChatClient()` (JS) i `model.GetChatClientAsync()` (C#) jako alternatywy dla OpenAI SDK
- **Część 2, Ćwiczenie 8:** Warianty modelu i wybór sprzętu — `selectVariant()`, `variants`, tabela wariantów NPU (7 modeli)
- **Część 2, Ćwiczenie 9:** Uaktualnienia modelu i odświeżanie katalogu — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Część 2, Ćwiczenie 10:** Modele rozumowania — `phi-4-mini-reasoning` z przykładami analizy tagu `<think>`
- **Część 3, Ćwiczenie 4:** `createChatClient` jako alternatywa dla OpenAI SDK, z dokumentacją wzorca zwrotnego strumieniowego
- **AGENTS.md:** Dodano konwencje kodowania dla Wywoływania narzędzi, ChatClient i modeli rozumowania

### Zmieniono
- **Część 1:** Rozszerzono katalog modeli — dodano phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Część 2:** Rozszerzono tabele referencyjne API — dodano `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Część 2:** Przenumerowano ćwiczenia 7-9 → 10-13, aby uwzględnić nowe ćwiczenia
- **Część 3:** Zaktualizowano tabelę kluczowych wniosków o natywnym ChatClient
- **README.md:** Dodano sekcję Część 11 z tabelą przykładów kodu; dodano cel nauki #11; zaktualizowano strukturę projektu
- **csharp/Program.cs:** Dodano przypadek `toolcall` w routerze CLI i zaktualizowano tekst pomocy

---

## 2026-03-09 — Aktualizacja SDK v0.9.0, brytyjski angielski i walidacja

### Zmieniono
- **Wszystkie przykłady kodu (Python, JavaScript, C#):** Zaktualizowano do Foundry Local SDK v0.9.0 API — naprawiono `await catalog.getModel()` (brakowało `await`), zaktualizowano wzorce inicjalizacji `FoundryLocalManager`, naprawiono wykrywanie punktów końcowych
- **Wszystkie przewodniki laboratoryjne (Części 1-10):** Przekonwertowano na brytyjski angielski (colour, catalogue, optimised itd.)
- **Wszystkie przewodniki laboratoryjne:** Zaktualizowano przykłady kodu SDK zgodnie z powierzchnią API v0.9.0
- **Wszystkie przewodniki laboratoryjne:** Zaktualizowano tabele referencyjne API i bloki kodu ćwiczeń
- **Krytyczna poprawka JavaScript:** Dodano brakujące `await` przy `catalog.getModel()` — zwracało `Promise`, nie obiekt `Model`, co powodowało ciche błędy dalej

### Zweryfikowano
- Wszystkie próbki Pythona działają poprawnie z usługą Foundry Local
- Wszystkie próbki JavaScript działają poprawnie (Node.js 18+)
- Projekt C# kompiluje się i działa na .NET 9.0 (kompatybilność wstecz od net8.0 SDK)
- 29 plików zmodyfikowanych i zweryfikowanych w całym warsztacie

---

## Indeks plików

| Plik | Ostatnia aktualizacja | Opis |
|------|----------------------|------|
| `labs/part1-getting-started.md` | 2026-03-10 | Rozszerzony katalog modeli |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nowe ćwiczenia 7-10, rozszerzone tabele API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Nowe ćwiczenie 4 (ChatClient), zaktualizowane wnioski |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + brytyjski angielski |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + brytyjski angielski |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + angielski brytyjski |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + angielski brytyjski |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Diagram Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + angielski brytyjski |
| `labs/part10-custom-models.md` | 2026-03-11 | Diagram Mermaid, przeniesiono Workshop Complete do części 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nowe laboratorium, diagramy Mermaid, sekcja Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Nowość: przykład wywoływania narzędzi |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Nowość: przykład wywoływania narzędzi |
| `csharp/ToolCalling.cs` | 2026-03-10 | Nowość: przykład wywoływania narzędzi |
| `csharp/Program.cs` | 2026-03-10 | Dodano polecenie CLI `toolcall` |
| `README.md` | 2026-03-10 | Część 11, struktura projektu |
| `AGENTS.md` | 2026-03-10 | Wywoływanie narzędzi + konwencje ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Usunięto rozwiązany problem #7, pozostało 6 otwartych problemów |
| `csharp/csharp.csproj` | 2026-03-11 | Uniwersalny TFM, warunkowe odwołania WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Uniwersalny TFM, automatyczne wykrywanie RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Uniwersalny TFM, automatyczne wykrywanie RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Usunięto obejście try/catch NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Usunięto obejście try/catch NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Usunięto obejście try/catch NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Usunięto obejście try/catch NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Usunięto obejście try/catch NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Przykład uniwersalnego pliku .csproj |
| `AGENTS.md` | 2026-03-11 | Zaktualizowano pakiety C# i szczegóły TFM |
| `CHANGELOG.md` | 2026-03-11 | Ten plik |