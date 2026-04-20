![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Teil 10: Verwendung von benutzerdefinierten oder Hugging Face Modellen mit Foundry Local

> **Ziel:** Ein Hugging Face Modell in das optimierte ONNX-Format kompilieren, das Foundry Local benötigt, es mit einer Chat-Vorlage konfigurieren, in den lokalen Cache hinzufügen und die Inferenz mithilfe der CLI, REST API und OpenAI SDK ausführen.

## Überblick

Foundry Local wird mit einem kuratierten Katalog vor-kompilierter Modelle ausgeliefert, aber Sie sind nicht auf diese Liste beschränkt. Jedes transformerbasierte Sprachmodell, das auf [Hugging Face](https://huggingface.co/) verfügbar ist (oder lokal im PyTorch / Safetensors-Format gespeichert wird), kann in ein optimiertes ONNX-Modell kompiliert und über Foundry Local bereitgestellt werden.

Die Kompilierungspipeline verwendet den **ONNX Runtime GenAI Model Builder**, ein Kommandozeilenwerkzeug, das im Paket `onnxruntime-genai` enthalten ist. Der Model Builder erledigt die Hauptarbeit: Er lädt die Quellgewichte herunter, konvertiert sie in das ONNX-Format, wendet Quantisierung (int4, fp16, bf16) an und erzeugt die Konfigurationsdateien (einschließlich Chat-Vorlage und Tokenizer), die Foundry Local erwartet.

In diesem Lab kompilieren Sie **Qwen/Qwen3-0.6B** von Hugging Face, registrieren es bei Foundry Local und chatten komplett auf Ihrem Gerät mit dem Modell.

---

## Lernziele

Am Ende dieses Labs können Sie:

- Erklären, warum die Kompilierung eigener Modelle nützlich ist und wann Sie sie benötigen
- Den ONNX Runtime GenAI Model Builder installieren
- Ein Hugging Face Modell mit einem einzigen Befehl in das optimierte ONNX-Format kompilieren
- Die wichtigsten Kompilierungsparameter verstehen (Execution Provider, Präzision)
- Die Konfigurationsdatei `inference_model.json` für die Chat-Vorlage erstellen
- Ein kompiliertes Modell zum Foundry Local Cache hinzufügen
- Inferenz auf dem benutzerdefinierten Modell per CLI, REST API und OpenAI SDK ausführen

---

## Voraussetzungen

| Anforderung | Details |
|-------------|---------|
| **Foundry Local CLI** | Installiert und im `PATH` ([Teil 1](part1-getting-started.md)) |
| **Python 3.10+** | Wird vom ONNX Runtime GenAI Model Builder benötigt |
| **pip** | Python Paketverwaltung |
| **Festplattenspeicher** | Mindestens 5 GB frei für Quell- und kompilierte Modellausgaben |
| **Hugging Face Account** | Einige Modelle erfordern die Lizenzannahme vor dem Download. Qwen3-0.6B nutzt die Apache 2.0 Lizenz und ist frei verfügbar. |

---

## Umgebungssetup

Für die Modellkompilierung werden mehrere große Python-Pakete benötigt (PyTorch, ONNX Runtime GenAI, Transformers). Erstellen Sie eine eigene virtuelle Umgebung, damit diese nicht Ihre System-Python-Installation oder andere Projekte beeinflussen.

```bash
# Vom Repository-Stamm
python -m venv .venv
```
  
Aktivieren Sie die Umgebung:

**Windows (PowerShell):**  
```powershell
.venv\Scripts\Activate.ps1
```
  
**macOS / Linux:**  
```bash
source .venv/bin/activate
```
  
Aktualisieren Sie pip, um Abhängigkeitsprobleme zu vermeiden:

```bash
python -m pip install --upgrade pip
```
  
> **Tipp:** Falls Sie bereits eine `.venv` aus früheren Labs besitzen, können Sie diese wiederverwenden. Stellen Sie nur sicher, dass sie aktiviert ist, bevor Sie fortfahren.

---

## Konzept: Die Kompilierungspipeline

Foundry Local verlangt Modelle im ONNX-Format mit ONNX Runtime GenAI-Konfiguration. Die meisten Open-Source-Modelle auf Hugging Face werden als PyTorch- oder Safetensors-Gewichte verteilt, daher ist ein Konvertierungsschritt nötig.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Was macht der Model Builder?

1. **Lädt** das Quellmodell von Hugging Face herunter (oder liest es von einem lokalen Pfad).
2. **Konvertiert** die PyTorch- / Safetensors-Gewichte ins ONNX-Format.
3. **Quantisiert** das Modell auf eine niedrigere Präzision (z.B. int4), um Speicherverbrauch zu verringern und Durchsatz zu verbessern.
4. **Erzeugt** die ONNX Runtime GenAI Konfiguration (`genai_config.json`), die Chat-Vorlage (`chat_template.jinja`) und alle Tokenizer-Dateien, damit Foundry Local das Modell laden und bedienen kann.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Sie könnten auf **Microsoft Olive** stoßen als alternatives Werkzeug für Modelloptimierung. Beide Tools können ONNX-Modelle erzeugen, verfolgen aber unterschiedliche Ziele und haben unterschiedliche Vor- und Nachteile:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Paket** | `onnxruntime-genai` | `olive-ai` |
| **Hauptzweck** | Konvertierung und Quantisierung generativer KI-Modelle für ONNX Runtime GenAI Inferenz | End-to-End Modelloptimierungs-Framework mit vielen Backends und Hardware-Zielen |
| **Benutzerfreundlichkeit** | Ein Befehl — einstufige Konvertierung + Quantisierung | Workflow-basiert — konfigurierbare mehrstufige Pipelines mit YAML/JSON |
| **Ausgabeformat** | ONNX Runtime GenAI Format (bereit für Foundry Local) | Generisches ONNX, ONNX Runtime GenAI oder andere abhängig vom Workflow |
| **Hardwareziele** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN und mehr |
| **Quantisierungsoptionen** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus Graph-Optimierungen, schichtweise Feinabstimmung |
| **Modellumfang** | Generative KI-Modelle (LLMs, SLMs) | Jedes ONNX-konvertierbare Modell (Vision, NLP, Audio, Multimodal) |
| **Ideal für** | Schnelle Einzelmodell-Kompilierung für lokale Inferenz | Produktionspipelines mit feinkörniger Optimierungskontrolle |
| **Abhängigkeiten** | Mittel (PyTorch, Transformers, ONNX Runtime) | Größer (inkl. Olive Framework, optionale Zusätze je nach Workflow) |
| **Foundry Local Integration** | Direkt — Ausgabe ist direkt kompatibel | Benötigt `--use_ort_genai` Flag und zusätzliche Konfiguration |

> **Warum dieses Lab den Model Builder nutzt:** Für die Aufgabe, ein einziges Hugging Face Modell zu kompilieren und bei Foundry Local zu registrieren, ist der Model Builder der einfachste und verlässlichste Weg. Er erzeugt im einzigen Befehl exakt das von Foundry Local erwartete Ausgabeformat. Falls Sie später erweiterte Optimierungsfeatures wie genauigkeitsbewusste Quantisierung, Graph Surgery oder mehrstufige Feinabstimmung brauchen, ist Olive eine leistungsfähige Option. Details finden Sie in der [Microsoft Olive Dokumentation](https://microsoft.github.io/Olive/).

---

## Lab Übungen

### Übung 1: Installation des ONNX Runtime GenAI Model Builder

Installieren Sie das ONNX Runtime GenAI Paket, welches das Model Builder Tool enthält:

```bash
pip install onnxruntime-genai
```
  
Überprüfen Sie die Installation, indem Sie sicherstellen, dass der Model Builder verfügbar ist:

```bash
python -m onnxruntime_genai.models.builder --help
```
  
Sie sollten eine Hilfeausgabe sehen, die Parameter wie `-m` (Modellname), `-o` (Ausgabepfad), `-p` (Präzision) und `-e` (Execution Provider) listet.

> **Hinweis:** Der Model Builder ist von PyTorch, Transformers und weiteren Paketen abhängig. Die Installation kann ein paar Minuten dauern.

---

### Übung 2: Kompilieren von Qwen3-0.6B für CPU

Führen Sie den folgenden Befehl aus, um das Qwen3-0.6B Modell von Hugging Face herunterzuladen und für CPU-Inferenz mit int4 Quantisierung zu kompilieren:

**macOS / Linux:**  
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```
  
**Windows (PowerShell):**  
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```
  
#### Was bewirkt jeder Parameter?

| Parameter | Zweck | Verwendeter Wert |
|-----------|-------|------------------|
| `-m` | Hugging Face Modell-ID oder lokaler Verzeichnis-Pfad | `Qwen/Qwen3-0.6B` |
| `-o` | Verzeichnis, in dem das kompilierte ONNX-Modell gespeichert wird | `models/qwen3` |
| `-p` | Bei der Kompilierung angewandte Quantisierungs-Präzision | `int4` |
| `-e` | ONNX Runtime Execution Provider (Zielhardware) | `cpu` |
| `--extra_options hf_token=false` | Überspringt Hugging Face Authentifizierung (ok für öffentliche Modelle) | `hf_token=false` |

> **Wie lange dauert das?** Die Kompilierungsdauer hängt von Hardware und Modellgröße ab. Für Qwen3-0.6B mit int4 Quantisierung auf einer modernen CPU rechnen Sie mit etwa 5 bis 15 Minuten. Größere Modelle brauchen proportional länger.

Nach Abschluss sehen Sie ein Verzeichnis `models/qwen3` mit kompilierten Modell-Dateien. Überprüfen Sie die Ausgabe:

```bash
ls models/qwen3
```
  
Sie sollten Dateien sehen wie:
- `model.onnx` und `model.onnx.data` — die kompilierten Modellgewichte
- `genai_config.json` — ONNX Runtime GenAI Konfiguration
- `chat_template.jinja` — die Chat-Vorlage des Modells (automatisch generiert)
- `tokenizer.json`, `tokenizer_config.json` — Tokenizer-Dateien
- Weitere Vokabel- und Konfigurationsdateien

---

### Übung 3: Kompilieren für GPU (Optional)

Falls Sie eine NVIDIA GPU mit CUDA-Unterstützung besitzen, können Sie eine GPU-optimierte Variante für schnellere Inferenz kompilieren:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```
  
> **Hinweis:** Die GPU-Kompilierung erfordert `onnxruntime-gpu` und eine funktionierende CUDA-Installation. Sind diese nicht vorhanden, meldet der Model Builder einen Fehler. Sie können diese Übung überspringen und mit der CPU-Variante fortfahren.

#### Hardware-spezifische Kompilierungsreferenz

| Ziel | Execution Provider (`-e`) | Empfohlene Präzision (`-p`) |
|-------|---------------------------|-----------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` oder `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` oder `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Präzisionskompromisse

| Präzision | Größe | Geschwindigkeit | Qualität |
|-----------|--------|-----------------|----------|
| `fp32` | Größte | Langsamste | Höchste Genauigkeit |
| `fp16` | Groß | Schnell (GPU) | Sehr gute Genauigkeit |
| `int8` | Klein | Schnell | Leichter Genauigkeitsverlust |
| `int4` | Kleinste | Schnellste | Moderate Genauigkeitsverluste |

Für die meisten lokalen Entwicklungen bietet `int4` auf CPU das beste Verhältnis aus Geschwindigkeit und Ressourcenverbrauch. Für Produktion empfiehlt sich `fp16` auf CUDA-GPU.

---

### Übung 4: Erstellen der Chat-Vorlagenkonfiguration

Der Model Builder erstellt automatisch eine Datei `chat_template.jinja` und `genai_config.json` im Ausgabeordner. Foundry Local benötigt aber auch eine `inference_model.json` Datei, um zu verstehen, wie Eingabeaufforderungen für Ihr Modell formatiert werden. Diese Datei definiert den Modellnamen und die Prompt-Vorlage, die Benutzer-Nachrichten in die korrekten Sondertokens hüllt.

#### Schritt 1: Ausgabe des kompilierten Modells inspizieren

Listen Sie den Inhalt des komprimierten Modells auf:

```bash
ls models/qwen3
```
  
Sie sollten Dateien wie sehen:
- `model.onnx` und `model.onnx.data` — kompiliertes Modellgewicht
- `genai_config.json` — ONNX Runtime GenAI Konfiguration (automatisch generiert)
- `chat_template.jinja` — Chat-Vorlage des Modells (automatisch generiert)
- `tokenizer.json`, `tokenizer_config.json` — Tokenizer-Dateien
- Verschiedene weitere Vokabel- und Konfigurationsdateien

#### Schritt 2: Die Datei inference_model.json erzeugen

Die `inference_model.json` Datei sagt Foundry Local, wie Prompts formatiert werden. Erstellen Sie ein Python-Skript namens `generate_chat_template.py` **im Repository-Root** (im gleichen Verzeichnis, das Ihren `models/` Ordner enthält):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Erstelle ein minimales Gespräch, um die Chat-Vorlage zu extrahieren
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# Erstelle die Struktur von inference_model.json
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```
  
Führen Sie das Skript vom Repository-Root aus:

```bash
python generate_chat_template.py
```
  
> **Hinweis:** Das Paket `transformers` wurde bereits als Abhängigkeit von `onnxruntime-genai` installiert. Falls ein `ImportError` auftritt, installieren Sie es mit `pip install transformers` nach.

Das Skript erzeugt eine `inference_model.json` Datei im Verzeichnis `models/qwen3`. Diese Datei teilt Foundry Local mit, wie Benutzereingaben mit den korrekten Sondertokens für Qwen3 umschlossen werden.

> **Wichtig:** Das Feld `"Name"` in `inference_model.json` (im Skript auf `qwen3-0.6b` gesetzt) ist der **Modell-Alias**, den Sie in allen folgenden Befehlen und API-Aufrufen verwenden. Ändern Sie den Modellnamen hier, passen Sie ihn entsprechend in den Übungen 6–10 an.

#### Schritt 3: Konfiguration überprüfen

Öffnen Sie `models/qwen3/inference_model.json` und prüfen Sie, ob ein Feld `Name` und ein Objekt `PromptTemplate` mit den Schlüsseln `assistant` und `prompt` enthalten sind. Die Prompt-Vorlage sollte Sondertokens wie `<|im_start|>` und `<|im_end|>` enthalten (die genauen Tokens hängen von der Chat-Vorlage des Modells ab).

> **Manuelle Alternative:** Falls Sie das Skript nicht ausführen möchten, können Sie die Datei auch manuell erstellen. Die wichtigste Voraussetzung ist, dass das Feld `prompt` die vollständige Chat-Vorlage des Modells enthält mit `{Content}` als Platzhalter für die Benutzer-Nachricht.

---

### Übung 5: Überprüfen der Modell-Verzeichnisstruktur
Der Modell-Builder legt alle kompilierten Dateien direkt in das von Ihnen angegebene Ausgabeverzeichnis ab. Überprüfen Sie, ob die endgültige Struktur korrekt aussieht:

```bash
ls models/qwen3
```

Das Verzeichnis sollte die folgenden Dateien enthalten:

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **Hinweis:** Im Gegensatz zu einigen anderen Kompilierungstools erstellt der Modell-Builder keine verschachtelten Unterverzeichnisse. Alle Dateien liegen direkt im Ausgabeordner, genau wie es Foundry Local erwartet.

---

### Übung 6: Fügen Sie das Modell dem Foundry Local Cache hinzu

Sagen Sie Foundry Local, wo es Ihr kompiliertes Modell finden kann, indem Sie das Verzeichnis zu seinem Cache hinzufügen:

```bash
foundry cache cd models/qwen3
```

Überprüfen Sie, ob das Modell im Cache erscheint:

```bash
foundry cache ls
```

Sie sollten Ihr benutzerdefiniertes Modell neben zuvor gecachten Modellen (wie `phi-3.5-mini` oder `phi-4-mini`) sehen.

---

### Übung 7: Führen Sie das benutzerdefinierte Modell über die CLI aus

Starten Sie eine interaktive Chatsitzung mit Ihrem neu kompilierten Modell (der Alias `qwen3-0.6b` stammt aus dem `Name`-Feld in `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Das Flag `--verbose` zeigt zusätzliche Diagnoseinformationen an, was beim erstmaligen Testen eines benutzerdefinierten Modells hilfreich ist. Wenn das Modell erfolgreich geladen wurde, sehen Sie eine interaktive Eingabeaufforderung. Probieren Sie ein paar Nachrichten aus:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Geben Sie `exit` ein oder drücken Sie `Strg+C`, um die Sitzung zu beenden.

> **Fehlerbehebung:** Falls das Modell nicht geladen wird, prüfen Sie Folgendes:
> - Die Datei `genai_config.json` wurde vom Modell-Builder generiert.
> - Die Datei `inference_model.json` existiert und ist gültiges JSON.
> - Die ONNX-Modell-Dateien befinden sich im richtigen Verzeichnis.
> - Es steht ausreichend RAM zur Verfügung (Qwen3-0.6B int4 benötigt etwa 1 GB).
> - Qwen3 ist ein Reasoning-Modell, das `<think>`-Tags erzeugt. Wenn Sie `<think>...</think>` vor den Antworten sehen, ist dies normales Verhalten. Die Prompt-Vorlage in `inference_model.json` kann angepasst werden, um die Denk-Ausgabe zu unterdrücken.

---

### Übung 8: Fragen Sie das benutzerdefinierte Modell über die REST-API ab

Wenn Sie die interaktive Sitzung in Übung 7 beendet haben, ist das Modell möglicherweise nicht mehr geladen. Starten Sie zuerst den Foundry Local-Dienst und laden Sie das Modell:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Überprüfen Sie, auf welchem Port der Dienst läuft:

```bash
foundry service status
```

Senden Sie dann eine Anfrage (ersetzen Sie `5273` durch Ihren tatsächlichen Port, falls er abweicht):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows-Hinweis:** Der obige `curl`-Befehl verwendet Bash-Syntax. Unter Windows verwenden Sie stattdessen das PowerShell-Cmdlet `Invoke-RestMethod`.

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### Übung 9: Verwenden Sie das benutzerdefinierte Modell mit dem OpenAI SDK

Sie können sich mit Ihrem benutzerdefinierten Modell genau mit demselben OpenAI SDK verbinden, das Sie auch für die integrierten Modelle verwendet haben (siehe [Teil 3](part3-sdk-and-apis.md)). Der einzige Unterschied ist der Modellname.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local überprüft keine API-Schlüssel
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local überprüft API-Schlüssel nicht
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **Wichtig:** Da Foundry Local eine OpenAI-kompatible API bereitstellt, funktioniert jeder Code, der mit den integrierten Modellen arbeitet, auch mit Ihren benutzerdefinierten Modellen. Sie müssen nur den Parameter `model` ändern.

---

### Übung 10: Testen Sie das benutzerdefinierte Modell mit dem Foundry Local SDK

In früheren Labs haben Sie das Foundry Local SDK verwendet, um den Dienst zu starten, den Endpunkt zu entdecken und Modelle automatisch zu verwalten. Sie können exakt dasselbe Muster mit Ihrem benutzerdefiniert kompilierten Modell anwenden. Das SDK übernimmt den Dienststart und die Endpunktentdeckung, so dass Ihr Code nicht `localhost:5273` hart kodieren muss.

> **Hinweis:** Stellen Sie sicher, dass das Foundry Local SDK installiert ist, bevor Sie diese Beispiele ausführen:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Fügen Sie die NuGet-Pakete `Microsoft.AI.Foundry.Local` und `OpenAI` hinzu
>
> Speichern Sie jedes Skript **im Repository-Stammverzeichnis** (also im gleichen Verzeichnis wie Ihr `models/`-Ordner).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Schritt 1: Starten Sie den Foundry Local-Dienst und laden Sie das benutzerdefinierte Modell
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Schritt 2: Überprüfen Sie den Cache auf das benutzerdefinierte Modell
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Schritt 3: Laden Sie das Modell in den Speicher
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Schritt 4: Erstellen Sie einen OpenAI-Client mit dem vom SDK entdeckten Endpunkt
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Schritt 5: Senden Sie eine Streaming-Chat-Vervollständigungsanfrage
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

Führen Sie es aus:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// Schritt 1: Starten Sie den Foundry Local-Dienst
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Schritt 2: Holen Sie das benutzerdefinierte Modell aus dem Katalog
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Schritt 3: Laden Sie das Modell in den Speicher
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Schritt 4: Erstellen Sie einen OpenAI-Client mit dem vom SDK ermittelten Endpunkt
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Schritt 5: Senden Sie eine chatbasierte Streaming-Komplettierungsanfrage
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

Führen Sie es aus:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **Wichtig:** Das Foundry Local SDK entdeckt den Endpunkt dynamisch, sodass Sie niemals eine Portnummer hart kodieren müssen. Dies ist der empfohlene Ansatz für Produktionsanwendungen. Ihr benutzerdefiniert kompiliertes Modell funktioniert identisch mit den integrierten Katalogmodellen über das SDK.

---

## Auswahl eines Modells zum Kompilieren

Qwen3-0.6B wird in diesem Lab als Referenzbeispiel verwendet, da es klein, schnell zu kompilieren und unter der Apache 2.0-Lizenz frei verfügbar ist. Sie können jedoch viele andere Modelle kompilieren. Hier einige Vorschläge:

| Modell | Hugging Face ID | Parameter | Lizenz | Hinweise |
|--------|-----------------|-----------|--------|----------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Sehr klein, schnelle Kompilierung, gut zum Testen |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Bessere Qualität, dennoch schnell zu kompilieren |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Hohe Qualität, benötigt mehr RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Lizenzannahme auf Hugging Face erforderlich |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Hochwertig, größerer Download und längere Kompilierung |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Bereits im Foundry Local-Katalog (nützlich zum Vergleich) |

> **Lizenz-Erinnerung:** Prüfen Sie stets die Lizenz des Modells auf Hugging Face, bevor Sie es verwenden. Einige Modelle (wie Llama) erfordern, dass Sie eine Lizenzvereinbarung akzeptieren und sich mit `huggingface-cli login` authentifizieren, bevor Sie herunterladen.

---

## Konzepte: Wann benutzerdefinierte Modelle verwenden

| Szenario | Warum selbst kompilieren? |
|----------|--------------------------|
| **Ein benötigtes Modell ist nicht im Katalog** | Der Foundry Local-Katalog ist kuratiert. Wenn das gewünschte Modell nicht gelistet ist, kompilieren Sie es selbst. |
| **Fine-tuned Modelle** | Haben Sie ein Modell auf domänenspezifischen Daten feinabgestimmt, müssen Sie Ihre eigenen Gewichte kompilieren. |
| **Spezifische Quantisierungsanforderungen** | Sie möchten möglicherweise eine Präzision oder Quantisierungsstrategie verwenden, die vom Katalogstandard abweicht. |
| **Neuere Modellversionen** | Wenn ein neues Modell auf Hugging Face veröffentlicht wird, ist es möglicherweise noch nicht im Foundry Local-Katalog. Eigenkompilierung verschafft sofortigen Zugriff. |
| **Forschung und Experimente** | Verschiedene Modellarchitekturen, -größen oder -konfigurationen lokal ausprobieren, bevor Sie sich für eine Produktionsvariante entscheiden. |

---

## Zusammenfassung

In diesem Lab haben Sie gelernt, wie Sie:

| Schritt | Was Sie gemacht haben |
|---------|-----------------------|
| 1 | Den ONNX Runtime GenAI Modell-Builder installiert haben |
| 2 | `Qwen/Qwen3-0.6B` von Hugging Face in ein optimiertes ONNX-Modell kompiliert haben |
| 3 | Eine `inference_model.json` Chat-Template-Konfigurationsdatei erstellt haben |
| 4 | Das kompilierte Modell dem Foundry Local Cache hinzugefügt haben |
| 5 | Interaktiven Chat mit dem benutzerdefinierten Modell über die CLI geführt haben |
| 6 | Das Modell über die OpenAI-kompatible REST-API abgefragt haben |
| 7 | Sich von Python, JavaScript und C# mit dem OpenAI SDK verbunden haben |
| 8 | Das benutzerdefinierte Modell mit dem Foundry Local SDK durchgängig getestet haben |

Die wichtigste Erkenntnis ist, dass **jedes Transformator-basierte Modell durch Foundry Local laufen kann**, sobald es in ONNX-Format kompiliert wurde. Die OpenAI-kompatible API bedeutet, dass Ihr bestehender Anwendungscode ohne Änderungen funktioniert; Sie müssen nur den Modellnamen austauschen.

---

## Wichtige Erkenntnisse

| Konzept | Details |
|---------|---------|
| ONNX Runtime GenAI Modell-Builder | Wandelt Hugging Face-Modelle per Einzelbefehl mit Quantisierung in ONNX um |
| ONNX-Format | Foundry Local benötigt ONNX-Modelle mit ONNX Runtime GenAI-Konfiguration |
| Chat-Vorlagen | Die Datei `inference_model.json` sagt Foundry Local, wie Prompts für ein Modell formatiert werden |
| Hardware-Ziele | Kompilieren für CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) oder WebGPU je nach Hardware |
| Quantisierung | Niedrigere Präzision (int4) reduziert Größe und erhöht Geschwindigkeit zulasten der Genauigkeit; fp16 behält hohe Qualität auf GPUs |
| API-Kompatibilität | Benutzerdefinierte Modelle nutzen dieselbe OpenAI-kompatible API wie integrierte Modelle |
| Foundry Local SDK | Das SDK steuert Dienststart, Endpunktentdeckung und Modell-Laden automatisch für Katalog- und benutzerdefinierte Modelle |

---

## Weiterführende Literatur

| Ressource | Link |
|-----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local Anleitung für benutzerdefinierte Modelle | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 Modellfamilie | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive Dokumentation | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Nächste Schritte

Fahren Sie fort mit [Teil 11: Tool-Aufrufe mit lokalen Modellen](part11-tool-calling.md), um zu lernen, wie Sie Ihre lokalen Modelle zur Ausführung externer Funktionen befähigen.

[← Teil 9: Whisper Sprachtranskription](part9-whisper-voice-transcription.md) | [Teil 11: Tool-Aufrufe →](part11-tool-calling.md)