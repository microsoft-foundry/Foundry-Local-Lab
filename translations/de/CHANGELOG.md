# Changelog — Foundry Local Workshop

Alle bedeutenden Änderungen an diesem Workshop sind unten dokumentiert.

---

## 2026-03-11 — Teil 12 & 13, Web UI, Whisper-Neuschreibung, WinML/QNN Fix und Validierung

### Hinzugefügt
- **Teil 12: Aufbau eines Web UI für den Zava Creative Writer** — neue Laboranleitung (`labs/part12-zava-ui.md`) mit Übungen zu Streaming NDJSON, Browser `ReadableStream`, Live-Agent-Statusanzeigen und Echtzeit-Textstreaming von Artikeln
- **Teil 13: Workshop Abschluss** — neue Zusammenfassungs-Laboranleitung (`labs/part13-workshop-complete.md`) mit Rückblick auf alle 12 Teile, weiteren Ideen und Ressourcenlinks
- **Zava UI Frontend:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — gemeinsames Vanilla HTML/CSS/JS-Browser-Interface, verwendet von allen drei Backends
- **JavaScript HTTP Server:** `zava-creative-writer-local/src/javascript/server.mjs` — neuer Express-artiger HTTP-Server, der den Orchestrator für den browserbasierten Zugriff kapselt
- **C# ASP.NET Core Backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` und `ZavaCreativeWriterWeb.csproj` — neues Minimal-API-Projekt, das die UI und Streaming NDJSON bereitstellt
- **Audio-Beispielgenerator:** `samples/audio/generate_samples.py` — Offline-TTS-Skript mit `pyttsx3`, um Zava-Themen-WAV-Dateien für Teil 9 zu erzeugen
- **Audio-Beispiel:** `samples/audio/zava-full-project-walkthrough.wav` — neues längeres Audio-Beispiel zum Transkriptionstesten
- **Validierungsskript:** `validate-npu-workaround.ps1` — automatisiertes PowerShell-Skript zur Validierung des NPU/QNN-Workarounds in allen C#-Beispielen
- **Mermaid-Diagramm-SVGs:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML plattformübergreifende Unterstützung:** Alle 3 C# `.csproj`-Dateien (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) verwenden jetzt bedingte TFM und sich gegenseitig ausschließende Paketreferenzen für plattformübergreifende Unterstützung. Auf Windows: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (Superset mit QNN EP-Plugin). Auf Nicht-Windows: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (Basis-SDK). Der hardcodierte `win-arm64` RID in Zava-Projekten wurde durch automatische Erkennung ersetzt. Ein transitive Abhängigkeitsworkaround schließt native Assets von `Microsoft.ML.OnnxRuntime.Gpu.Linux` aus, das eine fehlerhafte win-arm64-Referenz enthält. Der vorherige try/catch NPU-Workaround wurde aus allen 7 C# Dateien entfernt.

### Geändert
- **Teil 9 (Whisper):** Große Neuschreibung — JavaScript verwendet jetzt den SDK-internen `AudioClient` (`model.createAudioClient()`) statt manuelle ONNX Runtime-Inferenz; aktualisierte Architektur-Beschreibungen, Vergleichstabellen und Pipeline-Diagramme, um den JS/C# `AudioClient`-Ansatz gegenüber dem Python ONNX Runtime-Ansatz darzustellen
- **Teil 11:** Aktualisierte Navigationslinks (zeigen jetzt auf Teil 12); hinzugefügte gerenderte SVG-Diagramme für Tool-Aufruf-Fluss und Sequenz
- **Teil 10:** Aktualisierte Navigation, um über Teil 12 zu führen statt den Workshop zu beenden
- **Python Whisper (`foundry-local-whisper.py`):** Erweitert um zusätzliche Audio-Beispiele und verbesserte Fehlerbehandlung
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Umgeschrieben, um `model.createAudioClient()` mit `audioClient.transcribe()` anstelle manueller ONNX Runtime-Sitzungen zu verwenden
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Aktualisiert, um statische UI-Dateien neben der API zu bedienen
- **Zava C# Konsole (`zava-creative-writer-local/src/csharp/Program.cs`):** Entfernte NPU-Workaround (jetzt vom WinML-Paket behandelt)
- **README.md:** Hinzugefügt Abschnitt Teil 12 mit Code-Beispieltabelle und Backend-Erweiterungen; hinzugefügt Abschnitt Teil 13; Lernziele und Projektstruktur aktualisiert
- **KNOWN-ISSUES.md:** Entfernte gelöstes Problem #7 (C# SDK NPU-Model-Variante — jetzt vom WinML-Paket behandelt). Verbleibende Probleme neu nummeriert auf #1–#6. Aktualisierte Umgebungsdetails mit .NET SDK 10.0.104
- **AGENTS.md:** Aktualisierte Projektstrukturbaum mit neuen `zava-creative-writer-local` Einträgen (`ui/`, `csharp-web/`, `server.mjs`); aktualisierte C# Schlüsselpakete und bedingte TFM-Details
- **labs/part2-foundry-local-sdk.md:** Aktualisiertes `.csproj`-Beispiel zeigt volles plattformübergreifendes Muster mit bedingtem TFM, sich gegenseitig ausschließenden Paketreferenzen und erläuterndem Hinweis

### Validiert
- Alle 3 C# Projekte (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) bauen erfolgreich auf Windows ARM64
- Chat-Beispiel (`dotnet run chat`): Modell lädt als `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — NPU Variante lädt direkt ohne CPU-Fallback
- Agent-Beispiel (`dotnet run agent`): läuft End-to-End mit Mehrfach-Konversation, Exit Code 0
- Foundry Local CLI v0.8.117 und SDK v0.9.0 auf .NET SDK 9.0.312

---

## 2026-03-11 — Code-Fixes, Modellbereinigung, Mermaid-Diagramme und Validierung

### Behoben
- **Alle 21 Code-Beispiele (7 Python, 7 JavaScript, 7 C#):** Hinzugefügt `model.unload()` / `unload_model()` / `model.UnloadAsync()` Aufräum-Aufrufe beim Beenden zur Behebung von OGA-Speicherleck-Warnungen (Bekanntes Problem #4)
- **csharp/WhisperTranscription.cs:** Ersetzte den anfälligen relativen Pfad über `AppContext.BaseDirectory` durch `FindSamplesDirectory()`, das Verzeichnisse nach `samples/audio` zuverlässig nach oben durchläuft (Bekanntes Problem #7)
- **csharp/csharp.csproj:** Ersetzte hardcodierte `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` mit Auto-Erkennungs-Fallback mittels `$(NETCoreSdkRuntimeIdentifier)`, sodass `dotnet run` auf jeder Plattform ohne `-r` Flag funktioniert ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Geändert
- **Teil 8:** Evalgetriebene Iterationsschleife von ASCII-Box-Diagramm zu gerendertem SVG-Bild konvertiert
- **Teil 10:** Kompilations-Pipeline-Diagramm von ASCII-Pfeilen zu gerendertem SVG-Bild konvertiert
- **Teil 11:** Tool-Aufruf-Fluss- und Sequenzdiagramme zu gerenderten SVG-Bildern konvertiert
- **Teil 10:** "Workshop Complete!" Abschnitt zu Teil 11 (letztes Labor) verschoben; ersetzt durch "Nächste Schritte"-Link
- **KNOWN-ISSUES.md:** Vollständige Revalidierung aller Probleme gegen CLI v0.8.117. Gelöschte behobene Probleme: OGA Speicherleck (Aufräumen hinzugefügt), Whisper Pfad (FindSamplesDirectory), HTTP 500 Sustained Inference (nicht reproduzierbar, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice Begrenzungen (funktioniert jetzt mit `"required"` und spezifischer Funktionsauswahl bei qwen2.5-0.5b). Aktualisiertes JS Whisper Problem — alle Dateien geben jetzt leere/binar-Ausgabe zurück (Regression seit v0.9.x, Schwere erhöht auf Major). Aktualisierte #4 C# RID mit Auto-Erkennungs-Workaround und Link [#497](https://github.com/microsoft/Foundry-Local/issues/497). 7 offene Probleme verbleiben.
- **javascript/foundry-local-whisper.mjs:** Reparierter Aufräum-Variablenname (`whisperModel` → `model`)

### Validiert
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — laufen erfolgreich mit Aufräumen
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — laufen erfolgreich mit Aufräumen
- C#: `dotnet build` erfolgreich mit 0 Warnungen, 0 Fehlern (net9.0 Ziel)
- Alle 7 Python Dateien bestehen `py_compile` Syntaxprüfung
- Alle 7 JavaScript Dateien bestehen `node --check` Syntaxvalidierung

---

## 2026-03-10 — Teil 11: Tool-Aufruf, SDK API-Erweiterung und Modellabdeckung

### Hinzugefügt
- **Teil 11: Tool-Aufruf mit lokalen Modellen** — neue Laboranleitung (`labs/part11-tool-calling.md`) mit 8 Übungen zu Tool-Schemata, Mehrfach-Durchläufen, mehreren Tool-Aufrufen, benutzerdefinierten Tools, ChatClient Tool-Aufrufen und `tool_choice`
- **Python Beispiel:** `python/foundry-local-tool-calling.py` — Tool-Aufruf mit `get_weather`/`get_population` Tools unter Verwendung des OpenAI SDK
- **JavaScript Beispiel:** `javascript/foundry-local-tool-calling.mjs` — Tool-Aufruf mit dem nativen SDK `ChatClient` (`model.createChatClient()`)
- **C# Beispiel:** `csharp/ToolCalling.cs` — Tool-Aufruf mittels `ChatTool.CreateFunctionTool()` mit dem OpenAI C# SDK
- **Teil 2, Übung 7:** Nativer `ChatClient` — `model.createChatClient()` (JS) und `model.GetChatClientAsync()` (C#) als Alternativen zum OpenAI SDK
- **Teil 2, Übung 8:** Modellvarianten und Hardwareauswahl — `selectVariant()`, `variants`, NPU-Variantentabelle (7 Modelle)
- **Teil 2, Übung 9:** Modell-Upgrades und Katalogaktualisierung — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Teil 2, Übung 10:** Reasoning-Modelle — `phi-4-mini-reasoning` mit `<think>` Tag Parsing-Beispielen
- **Teil 3, Übung 4:** `createChatClient` als Alternative zum OpenAI SDK mit Dokumentation des Streaming Callback-Patterns
- **AGENTS.md:** Hinzugefügt Konventionen für Tool Calling, ChatClient und Reasoning Models

### Geändert
- **Teil 1:** Erweiterter Modelkatalog — hinzugefügt phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Teil 2:** Erweiterte API-Referenztabellen — hinzugefügt `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Teil 2:** Übungen 7-9 auf 10-13 umnummeriert, um neue Übungen einzufügen
- **Teil 3:** Aktualisierte Key Takeaways Tabelle mit nativen ChatClient
- **README.md:** Hinzugefügt Abschnitt Teil 11 mit Code-Beispieltabelle; Lernziel #11 hinzugefügt; Projektstrukturbaum aktualisiert
- **csharp/Program.cs:** Hinzugefügt `toolcall` Fall zu CLI-Router und Hilfe-Text aktualisiert

---

## 2026-03-09 — SDK v0.9.0 Update, Britisches Englisch und Validierungsdurchlauf

### Geändert
- **Alle Code-Beispiele (Python, JavaScript, C#):** Aktualisiert auf Foundry Local SDK v0.9.0 API — gefixt `await catalog.getModel()` (fehlendes `await`), aktualisierte `FoundryLocalManager` Initialisierungsmuster, behobene Endpoint-Erkennung
- **Alle Laboranleitungen (Teile 1-10):** Konvertiert auf britisches Englisch (colour, catalogue, optimised, etc.)
- **Alle Laboranleitungen:** SDK-Codebeispiele auf v0.9.0 API-Oberfläche aktualisiert
- **Alle Laboranleitungen:** API-Referenztabellen und Übungscodeblöcke aktualisiert
- **JavaScript kritischer Fix:** Fehlendes `await` bei `catalog.getModel()` eingefügt — vorher wurde ein `Promise` und kein `Model` Objekt zurückgegeben, was stille Fehler verursachte

### Validiert
- Alle Python-Beispiele laufen erfolgreich gegen Foundry Local Service
- Alle JavaScript-Beispiele laufen erfolgreich (Node.js 18+)
- C# Projekt baut und läuft auf .NET 9.0 (Forward-Kompatibilität von net8.0 SDK Assemblierung)
- 29 Dateien workshopweit modifiziert und validiert

---

## Dateindex

| Datei | Letzte Aktualisierung | Beschreibung |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Erweiterter Modellkatalog |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Neue Übungen 7-10, erweiterte API-Tabellen |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Neue Übung 4 (ChatClient), aktualisierte Erkenntnisse |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + britisches Englisch |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + britisches Englisch |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Britisches Englisch |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Britisches Englisch |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid-Diagramm |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Britisches Englisch |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid-Diagramm, Workshop abgeschlossen zu Teil 11 verschoben |
| `labs/part11-tool-calling.md` | 2026-03-11 | Neues Labor, Mermaid-Diagramme, Abschnitt Workshop abgeschlossen |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Neu: Tool-Aufruf-Beispiel |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Neu: Tool-Aufruf-Beispiel |
| `csharp/ToolCalling.cs` | 2026-03-10 | Neu: Tool-Aufruf-Beispiel |
| `csharp/Program.cs` | 2026-03-10 | Hinzugefügter `toolcall` CLI-Befehl |
| `README.md` | 2026-03-10 | Teil 11, Projektstruktur |
| `AGENTS.md` | 2026-03-10 | Tool-Aufruf + ChatClient-Konventionen |
| `KNOWN-ISSUES.md` | 2026-03-11 | Gelöstes Problem #7 entfernt, 6 offene Probleme verbleiben |
| `csharp/csharp.csproj` | 2026-03-11 | Plattformübergreifendes TFM, WinML/base SDK bedingte Referenzen |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Plattformübergreifendes TFM, automatische RID-Erkennung |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Plattformübergreifendes TFM, automatische RID-Erkennung |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU try/catch Workaround entfernt |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU try/catch Workaround entfernt |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU try/catch Workaround entfernt |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU try/catch Workaround entfernt |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU try/catch Workaround entfernt |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Plattformübergreifendes .csproj-Beispiel |
| `AGENTS.md` | 2026-03-11 | Aktualisierte C#-Pakete und TFM-Details |
| `CHANGELOG.md` | 2026-03-11 | Diese Datei |