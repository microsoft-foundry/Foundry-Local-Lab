![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Deel 8: Evaluatie-Geleide Ontwikkeling met Foundry Local

> **Doel:** Bouw een evaluatiekader dat systematisch je AI-agenten test en scoort, waarbij hetzelfde lokale model wordt gebruikt als zowel de geteste agent als de beoordelaar, zodat je met vertrouwen kunt itereren op prompts voordat je ze uitbrengt.

## Waarom Evaluatie-Geleide Ontwikkeling?

Bij het bouwen van AI-agenten is "het ziet er ongeveer goed uit" niet voldoende. **Evaluatie-geleide ontwikkeling** behandelt agentuitvoer als code: je schrijft eerst tests, meet kwaliteit en brengt pas uit wanneer scores aan een drempel voldoen.

In de Zava Creative Writer (Deel 7) functioneert de **Editor-agent** al als een lichte evaluator; die beslist ACCEPT of REVISE. Deze workshop formaliseert dat patroon tot een herhaalbaar evaluatiekader dat je op elke agent of pijplijn kunt toepassen.

| Probleem | Oplossing |
|---------|----------|
| Promptwijzigingen breken stilletjes de kwaliteit | **Gouden dataset** vangt regressies |
| "Werkt op één voorbeeld" vooringenomenheid | **Meerdere testgevallen** onthullen randgevallen |
| Subjectieve kwaliteitsbeoordeling | **Regelgebaseerde + LLM-als-beoordelaar scoring** levert cijfers op |
| Geen manier om promptvarianten te vergelijken | **Evaluaties naast elkaar uitvoeren** met geaggregeerde scores |

---

## Kernconcepten

### 1. Gouden Datasets

Een **gouden dataset** is een samengestelde set testgevallen met bekende verwachte uitkomsten. Elk testgeval bevat:

- **Input**: De prompt of vraag die aan de agent wordt gestuurd
- **Verwachte output**: Wat een correct of hoogwaardig antwoord moet bevatten (trefwoorden, structuur, feiten)
- **Categorie**: Groepering voor rapportage (bijv. "feitelijke nauwkeurigheid", "toon", "volledigheid")

### 2. Regelgebaseerde Controles

Snelle, deterministische controles die geen LLM vereisen:

| Controle | Wat wordt getest |
|-------|--------------|
| **Lengtegrenzen** | Antwoord is niet te kort (lui) of te lang (uitweiden) |
| **Vereiste trefwoorden** | Antwoord noemt verwachte termen of entiteiten |
| **Formaatvalidatie** | JSON is parseerbaar, Markdown-koppen zijn aanwezig |
| **Verboden inhoud** | Geen gehallucineerde merknamen, geen vermeldingen van concurrenten |

### 3. LLM-als-Beoordelaar

Gebruik hetzelfde **lokale model** om zijn eigen uitvoer (of uitvoer van een andere promptvariant) te beoordelen. De beoordelaar ontvangt:

- De originele vraag
- Het antwoord van de agent
- Beoordelingscriteria

En geeft een gestructureerde score terug. Dit weerspiegelt het Editor-patroon uit Deel 7, maar nu systematisch toegepast over een testsuite.

### 4. Evaluatie-Gestuurde Iteratielus

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Vereisten

| Vereiste | Details |
|-------------|---------|
| **Foundry Local CLI** | Geïnstalleerd met een gedownload model |
| **Programmeertaal runtime** | **Python 3.9+** en/of **Node.js 18+** en/of **.NET 9+ SDK** |
| **Voltooid** | [Deel 5: Single Agents](part5-single-agents.md) en [Deel 6: Multi-Agent Workflows](part6-multi-agent-workflows.md) |

---

## Laboratoriumoefeningen

### Oefening 1 - Draai het Evaluatiekader

De workshop bevat een volledige evaluatievoorbeeld die een Foundry Local-agent test aan de hand van een gouden dataset van Zava DIY-gerelateerde vragen.

<details>
<summary><strong>🐍 Python</strong></summary>

**Installatie:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Uitvoeren:**
```bash
python foundry-local-eval.py
```

**Wat gebeurt er:**
1. Verbindt met Foundry Local en laadt het model
2. Definieert een gouden dataset van 5 testgevallen (Zava productvragen)
3. Voert twee promptvarianten uit tegen elk testgeval
4. Scoort elk antwoord met **regelgebaseerde controles** (lengte, trefwoorden, formaat)
5. Scoort elk antwoord met **LLM-als-beoordelaar** (hetzelfde model beoordeelt kwaliteit 1-5)
6. Print een scorekaart die beide promptvarianten vergelijkt

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Installatie:**
```bash
cd javascript
npm install
```

**Uitvoeren:**
```bash
node foundry-local-eval.mjs
```

**Zelfde evaluatiepijplijn:** gouden dataset, dubbele prompt runs, regelgebaseerde + LLM scoring, scorekaart.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Installatie:**
```bash
cd csharp
dotnet restore
```

**Uitvoeren:**
```bash
dotnet run eval
```

**Zelfde evaluatiepijplijn:** gouden dataset, dubbele prompt runs, regelgebaseerde + LLM scoring, scorekaart.

</details>

---

### Oefening 2 - Begrijp de Gouden Dataset

Bekijk de testgevallen die in het evaluatievoorbeeld zijn gedefinieerd. Elk testgeval bevat:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Vragen om over na te denken:**
1. Waarom zijn de verwachte waarden **trefwoorden** in plaats van volledige zinnen?
2. Hoeveel testgevallen heb je nodig voor een betrouwbare evaluatie?
3. Welke categorieën zou je toevoegen voor je eigen toepassing?

---

### Oefening 3 - Begrijp Regelgebaseerde vs LLM-als-Beoordelaar Scoring

Het evaluatiekader gebruikt **twee scoringslagen**:

#### Regelgebaseerde controles (snel, deterministisch)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-als-Beoordelaar (nuance, kwalitatief)

Hetzelfde lokale model fungeert als beoordelaar met een gestructureerde rubric:

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

**Vragen om over na te denken:**
1. Wanneer vertrouw je regelgebaseerde controles meer dan LLM-als-beoordelaar?
2. Kan een model betrouwbaar zijn eigen output beoordelen? Wat zijn de beperkingen?
3. Hoe verhoudt dit zich tot het Editor-agent patroon uit Deel 7?

---

### Oefening 4 - Vergelijk Promptvarianten

Het voorbeeld voert **twee promptvarianten** uit tegen dezelfde testgevallen:

| Variant | Stijl Systeem Prompt |
|---------|-------------------|
| **Baseline** | Generiek: "Je bent een behulpzame assistent" |
| **Gespecialiseerd** | Gedetailleerd: "Je bent een Zava DIY-expert die specifieke producten aanbeveelt en stapsgewijze begeleiding geeft" |

Na uitvoering zie je een scorekaart zoals:

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

**Oefeningen:**
1. Voer de evaluatie uit en noteer de scores van elke variant
2. Pas de gespecialiseerde prompt aan om nog specifieker te zijn. Verbeter de score?
3. Voeg een derde promptvariant toe en vergelijk alle drie.
4. Probeer het modelalias te veranderen (bijv. `phi-4-mini` vs `phi-3.5-mini`) en vergelijk de resultaten.

---

### Oefening 5 - Pas Evaluatie Toe op Je Eigen Agent

Gebruik het evaluatiekader als sjabloon voor je eigen agenten:

1. **Definieer je gouden dataset**: schrijf 5 tot 10 testgevallen met verwachte trefwoorden.
2. **Schrijf je systeem prompt**: de agentinstructies die je wilt testen.
3. **Draai de evaluatie**: verkrijg baselinescores.
4. **Itereer**: pas de prompt aan, voer opnieuw uit en vergelijk.
5. **Stel een drempel in**: bijv. "breng niet uit onder een gecombineerde score van 0.75".

---

### Oefening 6 - Verbinding met het Zava Editor Patroon

Kijk terug naar de Zava Creative Writer's Editor-agent (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# De editor is een LLM-als-rechter in productie:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Dit is hetzelfde **concept** als LLM-als-beoordelaar in Deel 8, maar ingebed in de productie-pijplijn in plaats van in een offline testsuite. Beide patronen:

- Gebruiken gestructureerde JSON-uitvoer van het model.
- Pas kwaliteitscriteria toe die in de systeem prompt zijn gedefinieerd.
- Maken een slagen/falen-beslissing.

**Het verschil:** De editor draait in productie (bij elke aanvraag). Het evaluatiekader draait in ontwikkeling (voordat je uitbrengt).

---

## Belangrijke Leerpunten

| Concept | Leerpunt |
|---------|----------|
| **Gouden datasets** | Verzamel vroeg testgevallen; ze zijn je regressietests voor AI |
| **Regelgebaseerde controles** | Snel, deterministisch, en vangen duidelijke fouten op (lengte, trefwoorden, formaat) |
| **LLM-als-beoordelaar** | Nauwkeurige kwaliteitsbeoordeling met hetzelfde lokale model |
| **Promptvergelijking** | Voer meerdere varianten uit op dezelfde testsuite om de beste te kiezen |
| **Voordeel op apparaat** | Alle evaluaties draaien lokaal: geen API-kosten, geen snelheidslimieten, geen data-uitwisseling buiten je machine |
| **Evalueren vóór uitbrengen** | Stel kwaliteitsdrempels in en koppel releases aan evaluatiescores |

---

## Volgende Stappen

- **Opschalen**: Voeg meer testgevallen en categorieën toe aan je gouden dataset.
- **Automatiseren**: Integreer evaluatie in je CI/CD pipeline.
- **Geavanceerde beoordelaars**: Gebruik een groter model als beoordelaar terwijl je output van een kleiner model test.
- **Volg in de tijd**: Bewaar evaluatieresultaten om te vergelijken tussen prompt- en modelversies.

---

## Volgende Lab

Ga verder naar [Deel 9: Stemtranscriptie met Whisper](part9-whisper-voice-transcription.md) om spraak-naar-tekst op apparaat te verkennen met de Foundry Local SDK.