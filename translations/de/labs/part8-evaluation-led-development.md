![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Teil 8: Evaluation-gesteuerte Entwicklung mit Foundry Local

> **Ziel:** Bauen Sie ein Bewertungs-Framework, das Ihre KI-Agenten systematisch testet und bewertet, wobei dasselbe lokale Modell sowohl als zu testender Agent als auch als Richter verwendet wird, sodass Sie mit Zuversicht an Eingabeaufforderungen iterieren können, bevor Sie sie ausliefern.

## Warum Evaluation-gesteuerte Entwicklung?

Beim Aufbau von KI-Agenten reicht „es sieht ungefähr richtig aus“ nicht aus. **Evaluation-gesteuerte Entwicklung** behandelt Agentenausgaben wie Code: Sie schreiben zuerst Tests, messen die Qualität und liefern erst aus, wenn die Punktzahlen eine Schwelle erreichen.

Im Zava Creative Writer (Teil 7) fungiert der **Editor-Agent** bereits als leichter Evaluator; er entscheidet ÜBERNEHMEN oder ÜBERARBEITEN. Dieses Labor formalisiert dieses Muster zu einem wiederholbaren Bewertungs-Framework, das Sie auf jeden Agenten oder jede Pipeline anwenden können.

| Problem | Lösung |
|---------|----------|
| Änderungen im Prompt brechen stillschweigend die Qualität | **Goldener Datensatz** erkennt Regressionen |
| „Funktioniert bei einem Beispiel“-Bias | **Mehrere Testfälle** decken Randfälle auf |
| Subjektive Qualitätsbewertung | **Regelbasierte + LLM-als-Richter-Bewertung** liefert Zahlen |
| Keine Möglichkeit, Prompt-Varianten zu vergleichen | **Gegenübergestellte Auswertungen** mit aggregierten Punktzahlen |

---

## Kernkonzepte

### 1. Goldene Datensätze

Ein **goldener Datensatz** ist eine kuratierte Sammlung von Testfällen mit bekannten erwarteten Ausgaben. Jeder Testfall enthält:

- **Eingabe**: Die Eingabeaufforderung oder Frage, die an den Agenten gesendet wird
- **Erwartete Ausgabe**: Was eine korrekte oder qualitativ hochwertige Antwort enthalten sollte (Schlüsselwörter, Struktur, Fakten)
- **Kategorie**: Gruppierung für Berichte (z. B. „faktische Genauigkeit“, „Ton“, „Vollständigkeit“)

### 2. Regelbasierte Prüfungen

Schnelle, deterministische Prüfungen, die kein LLM erfordern:

| Prüfung | Was sie testet |
|---------|----------------|
| **Längenbegrenzungen** | Antwort ist nicht zu kurz (faul) oder zu lang (abschweifend) |
| **Erforderliche Schlüsselwörter** | Antwort erwähnt erwartete Begriffe oder Entitäten |
| **Formatvalidierung** | JSON ist parsbar, Markdown-Überschriften sind vorhanden |
| **Verbotener Inhalt** | Keine halluzinierten Markennamen, keine Wettbewerber-Erwähnungen |

### 3. LLM als Richter

Verwenden Sie dasselbe lokale Modell, um seine eigenen Ausgaben (oder Ausgaben einer anderen Prompt-Variante) zu bewerten. Der Richter erhält:

- Die ursprüngliche Frage
- Die Antwort des Agenten
- Bewertungskriterien

Und liefert eine strukturierte Bewertung zurück. Dies spiegelt das Editor-Muster aus Teil 7 wider, wird hier aber systematisch über eine Testsuite angewendet.

### 4. Eval-gesteuerter Iterationszyklus

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Voraussetzungen

| Anforderung | Details |
|-------------|---------|
| **Foundry Local CLI** | Installiert mit heruntergeladenem Modell |
| **Sprachlaufzeit** | **Python 3.9+** und/oder **Node.js 18+** und/oder **.NET 9+ SDK** |
| **Abgeschlossen** | [Teil 5: Einzelne Agenten](part5-single-agents.md) und [Teil 6: Multi-Agent Workflows](part6-multi-agent-workflows.md) |

---

## Laborübungen

### Übung 1 - Führen Sie das Bewertungs-Framework aus

Der Workshop enthält eine vollständige Bewertungsvorlage, die einen Foundry Local Agenten gegen einen goldenen Datensatz mit Zava DIY-bezogenen Fragen testet.

<details>
<summary><strong>🐍 Python</strong></summary>

**Setup:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Ausführen:**
```bash
python foundry-local-eval.py
```

**Was passiert:**
1. Verbindet mit Foundry Local und lädt das Modell
2. Definiert einen goldenen Datensatz von 5 Testfällen (Zava Produktfragen)
3. Führt zwei Prompt-Varianten gegen jeden Testfall aus
4. Bewertet jede Antwort mit **regelbasierten Prüfungen** (Länge, Schlüsselwörter, Format)
5. Bewertet jede Antwort mit **LLM-als-Richter** (dasselbe Modell bewertet Qualität 1-5)
6. Gibt eine Scorecard aus, die beide Prompt-Varianten vergleicht

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Setup:**
```bash
cd javascript
npm install
```

**Ausführen:**
```bash
node foundry-local-eval.mjs
```

**Gleiche Bewertungspipeline:** goldener Datensatz, zwei Prompt-Durchläufe, regelbasierte + LLM-Bewertung, Scorecard.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Setup:**
```bash
cd csharp
dotnet restore
```

**Ausführen:**
```bash
dotnet run eval
```

**Gleiche Bewertungspipeline:** goldener Datensatz, zwei Prompt-Durchläufe, regelbasierte + LLM-Bewertung, Scorecard.

</details>

---

### Übung 2 - Verstehen Sie den goldenen Datensatz

Untersuchen Sie die im Bewertungsbeispiel definierten Testfälle. Jeder Testfall enthält:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Fragen zum Nachdenken:**
1. Warum sind die erwarteten Werte **Schlüsselwörter** und keine vollständigen Sätze?
2. Wie viele Testfälle benötigt man für eine zuverlässige Bewertung?
3. Welche Kategorien würden Sie für Ihre eigene Anwendung hinzufügen?

---

### Übung 3 - Verstehen Sie regelbasierte vs. LLM-als-Richter-Bewertung

Das Bewertungs-Framework verwendet **zwei Bewertungsebenen**:

#### Regelbasierte Prüfungen (schnell, deterministisch)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM als Richter (nuanciert, qualitativ)

Dasselbe lokale Modell fungiert als Richter mit einem strukturierten Bewertungsraster:

```
Rate this response on a scale of 1-5:
- 1: Completely wrong or irrelevant
- 2: Partially correct but missing key information
- 3: Adequate but could be improved
- 4: Good response with minor issues
- 5: Excellent, comprehensive, well-structured

Score: 4
Reasoning: The response correctly identifies all necessary tools
and provides practical advice, but could include safety equipment.
```

**Fragen zum Nachdenken:**
1. Wann würden Sie regelbasierte Prüfungen gegenüber LLM-als-Richter vertrauen?
2. Kann ein Modell zuverlässig seine eigene Ausgabe bewerten? Was sind die Grenzen?
3. Wie vergleicht sich das mit dem Editor-Agent-Muster aus Teil 7?

---

### Übung 4 - Vergleichen Sie Prompt-Varianten

Das Beispiel führt **zwei Prompt-Varianten** gegen die gleichen Testfälle aus:

| Variante | System-Prompt-Stil |
|---------|-------------------|
| **Baseline** | Generisch: „Du bist ein hilfreicher Assistent“ |
| **Spezialisiert** | Detailliert: „Du bist ein Zava DIY-Experte, der spezifische Produkte empfiehlt und Schritt-für-Schritt-Anleitungen gibt“ |

Nach dem Ausführen sehen Sie eine Scorecard wie:

```
╔══════════════════════════════════════════════════════════════╗
║                    EVALUATION SCORECARD                      ║
╠══════════════════════════════════════════════════════════════╣
║ Prompt Variant    │ Rule Score │ LLM Score │ Combined       ║
╠═══════════════════╪════════════╪═══════════╪════════════════╣
║ Baseline          │ 0.62       │ 3.2 / 5   │ 0.62           ║
║ Specialised       │ 0.81       │ 4.1 / 5   │ 0.81           ║
╚══════════════════════════════════════════════════════════════╝
```

**Übungen:**
1. Führen Sie die Bewertung durch und notieren Sie die Punktzahlen für jede Variante
2. Ändern Sie den spezialisierten Prompt, um noch spezifischer zu sein. Verbessert sich der Score?
3. Fügen Sie eine dritte Prompt-Variante hinzu und vergleichen Sie alle drei.
4. Versuchen Sie, den Modellalias zu ändern (z. B. `phi-4-mini` vs. `phi-3.5-mini`) und vergleichen die Ergebnisse.

---

### Übung 5 - Anwenden der Bewertung auf Ihren eigenen Agenten

Verwenden Sie das Bewertungs-Framework als Vorlage für Ihre eigenen Agenten:

1. **Definieren Sie Ihren goldenen Datensatz**: Schreiben Sie 5 bis 10 Testfälle mit erwarteten Schlüsselwörtern.
2. **Schreiben Sie Ihren System-Prompt**: die Agentenanweisungen, die Sie testen möchten.
3. **Führen Sie die Bewertung aus**: Erhalten Sie Baseline-Punktzahlen.
4. **Iterieren**: Optimieren Sie den Prompt, führen Sie erneut aus und vergleichen Sie.
5. **Setzen Sie eine Schwelle**: z. B. „nicht unter 0,75 kombinierten Score ausliefern“.

---

### Übung 6 - Verbindung zum Zava Editor-Muster

Werfen Sie einen Blick zurück auf den Editor-Agenten des Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Der Editor ist ein LLM-als-Richter im Produktionseinsatz:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Dies ist das **gleiche Konzept** wie LLM-als-Richter in Teil 8, aber eingebettet in die Produktions-Pipeline anstatt in einer Offline-Testsuite. Beide Muster:

- Verwenden strukturierte JSON-Ausgabe des Modells.
- Wenden Qualitätskriterien an, die im System-Prompt definiert sind.
- Treffen eine Bestehen/Nicht-Bestehen-Entscheidung.

**Der Unterschied:** Der Editor läuft in der Produktion (bei jeder Anfrage). Das Bewertungs-Framework läuft in der Entwicklung (vor dem Ausliefern).

---

## Wichtige Erkenntnisse

| Konzept | Erkenntnis |
|---------|------------|
| **Goldene Datensätze** | Kuratieren Sie Testfälle früh; sie sind Ihre Regressions-Tests für KI |
| **Regelbasierte Prüfungen** | Schnell, deterministisch und fangen offensichtliche Fehler (Länge, Schlüsselwörter, Format) ein |
| **LLM-als-Richter** | Nuancierte Qualitätsbewertung mit demselben lokalen Modell |
| **Promptvergleich** | Führen Sie mehrere Varianten gegen dieselbe Testsuite aus, um die beste auszuwählen |
| **Vorteil lokal** | Alle Bewertungen laufen lokal: keine API-Kosten, keine Ratenbegrenzungen, keine Daten verlassen Ihre Maschine |
| **Bewertung vor Auslieferung** | Setzen Sie Qualitätsgrenzen und steuern Sie Veröffentlichungen durch Bewertungsergebnisse |

---

## Nächste Schritte

- **Skalieren:** Fügen Sie weitere Testfälle und Kategorien zu Ihrem goldenen Datensatz hinzu.
- **Automatisieren:** Integrieren Sie Bewertung in Ihre CI/CD-Pipeline.
- **Fortgeschrittene Richter:** Verwenden Sie ein größeres Modell als Richter beim Testen der Ausgabe eines kleineren Modells.
- **Über Zeit verfolgen:** Speichern Sie Bewertungsergebnisse zum Vergleich zwischen Prompt- und Modellversionen.

---

## Nächstes Labor

Fahren Sie fort mit [Teil 9: Sprachtranskription mit Whisper](part9-whisper-voice-transcription.md), um Sprache-zu-Text lokal mit dem Foundry Local SDK zu erkunden.