![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 8: Evaluationsstyret Udvikling med Foundry Local

> **Mål:** Byg en evalueringsramme, der systematisk tester og scorer dine AI-agenter, ved at bruge samme lokale model både som agent under test og som dommer, så du trygt kan iterere på prompts inden levering.

## Hvorfor Evaluationsstyret Udvikling?

Når du bygger AI-agenter, er "det ser ud til at være korrekt" ikke godt nok. **Evaluationsstyret udvikling** behandler agentoutput som kode: du skriver tests først, måler kvalitet og leverer kun, når score når en tærskel.

I Zava Creative Writer (Del 7) fungerer **Editor-agenten** allerede som en letvægtsevaluator; den beslutter ACCEPTER eller RET. Dette laboratorium formaliserer det mønster til en gentagelig evalueringsramme, du kan anvende på enhver agent eller pipeline.

| Problem | Løsning |
|---------|----------|
| Promptændringer ødelægger stilfærdigt kvalitet | **Golden dataset** fanger regressioner |
| "Fungerer på ét eksempel"-bias | **Flere testcases** afslører hjørnesager |
| Subjektiv kvalitetsvurdering | **Regelbaseret + LLM-som-dommer scoring** giver tal |
| Ingen måde at sammenligne promptvarianter | **Side-om-side evalueringskørsler** med samlede scorer |

---

## Nøglebegreber

### 1. Golden Datasets

Et **golden dataset** er et kurateret sæt testcases med kendte forventede output. Hver testcase indeholder:

- **Input**: Prompt eller spørgsmål til agenten
- **Forventet output**: Hvad en korrekt eller høj kvalitets respons skal indeholde (nøgleord, struktur, fakta)
- **Kategori**: Gruppering til rapportering (f.eks. "faktuel nøjagtighed", "tone", "fyldestgørelse")

### 2. Regelbaserede Checks

Hurtige, deterministiske checks, som ikke kræver en LLM:

| Check | Hvad det tester |
|-------|-----------------|
| **Længde-grænser** | Responsen er hverken for kort (dovent) eller for lang (omfattende) |
| **Påkrævede nøgleord** | Respons nævner forventede termer eller enheder |
| **Formatvalidering** | JSON kan parses, Markdown-overskrifter er til stede |
| **Forbudt indhold** | Ingen hallucinerede brandnavne, ingen konkurrenthenvisninger |

### 3. LLM-som-Dommer

Brug **samme lokale model** til at bedømme sine egne output (eller output fra en anden promptvariant). Dommeren modtager:

- Det oprindelige spørgsmål
- Agentens svar
- Bedømmelseskriterier

Og returnerer en struktureret score. Dette spejler Editor-mønstret fra Del 7 men anvendt systematisk i en testsuite.

### 4. Evalueringsdrevet Iterationsloop

![Evalueringsdrevet iterationsloop](../../../images/eval-loop-flowchart.svg)

---

## Forudsætninger

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installeret med en model downloadet |
| **Sprogmiljø** | **Python 3.9+** og/eller **Node.js 18+** og/eller **.NET 9+ SDK** |
| **Fuldført** | [Del 5: Single Agents](part5-single-agents.md) og [Del 6: Multi-Agent Workflows](part6-multi-agent-workflows.md) |

---

## Laboratorieøvelser

### Øvelse 1 - Kør Evalueringsrammen

Workshoppen inkluderer et komplet evaluerings-eksempel, der tester en Foundry Local agent mod et golden dataset med Zava DIY-relaterede spørgsmål.

<details>
<summary><strong>🐍 Python</strong></summary>

**Opsætning:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Kør:**
```bash
python foundry-local-eval.py
```

**Hvad sker der:**
1. Forbinder til Foundry Local og loader modellen
2. Definerer et golden dataset med 5 testcases (Zava produktspørgsmål)
3. Kører to promptvarianter mod hver testcase
4. Scorer hvert svar med **regelbaserede checks** (længde, nøgleord, format)
5. Scorer hvert svar med **LLM-som-dommer** (samme model bedømmer kvalitet 1-5)
6. Printer et scorekort, der sammenligner begge promptvarianter

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Opsætning:**
```bash
cd javascript
npm install
```

**Kør:**
```bash
node foundry-local-eval.mjs
```

**Samme evalueringspipeline:** golden dataset, dobbelte promptkørsler, regelbaseret + LLM-scoring, scorekort.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Opsætning:**
```bash
cd csharp
dotnet restore
```

**Kør:**
```bash
dotnet run eval
```

**Samme evalueringspipeline:** golden dataset, dobbelte promptkørsler, regelbaseret + LLM-scoring, scorekort.

</details>

---

### Øvelse 2 - Forstå Golden Dataset

Undersøg testcases defineret i evaluerings-eksemplet. Hver testcase indeholder:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Spørgsmål til overvejelse:**
1. Hvorfor er forventede værdier **nøgleord** og ikke hele sætninger?
2. Hvor mange testcases har du brug for til en pålidelig evaluering?
3. Hvilke kategorier ville du tilføje til din egen applikation?

---

### Øvelse 3 - Forstå Regelbaseret vs LLM-som-Dommer Scoring

Evalueringsrammen bruger **to scoringslag**:

#### Regelbaserede Checks (hurtige, deterministiske)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-som-Dommer (nuanceret, kvalitativt)

Samme lokale model fungerer som dommer med en struktureret rubrik:

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

**Spørgsmål til overvejelse:**
1. Hvornår ville du stole på regelbaserede checks fremfor LLM-som-dommer?
2. Kan en model pålideligt bedømme sit eget output? Hvad er begrænsningerne?
3. Hvordan sammenlignes dette med Editor-agentens mønster fra Del 7?

---

### Øvelse 4 - Sammenlign Promptvarianter

Eksemplet kører **to promptvarianter** mod de samme testcases:

| Variant | Systemprompt-stil |
|---------|-------------------|
| **Baseline** | Generisk: "Du er en hjælpsom assistent" |
| **Specialiseret** | Detaljeret: "Du er en Zava DIY ekspert, der anbefaler specifikke produkter og giver trin-for-trin vejledning" |

Efter kørsel vil du se et scorekort som:

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

**Øvelser:**
1. Kør evalueringen og noter score for hver variant
2. Gør den specialiserede prompt endnu mere specifik. Forbedres scoren?
3. Tilføj en tredje promptvariant og sammenlign alle tre.
4. Prøv at ændre modelalias (f.eks. `phi-4-mini` vs `phi-3.5-mini`) og sammenlign resultater.

---

### Øvelse 5 - Anvend Evaluering på Din Egen Agent

Brug evalueringsrammen som en skabelon til dine egne agenter:

1. **Definer dit golden dataset**: skriv 5 til 10 testcases med forventede nøgleord.
2. **Skriv dit systemprompt**: agentinstruktionerne du vil teste.
3. **Kør eval**: få baseline-scorer.
4. **Iterer**: juster prompt, kør igen og sammenlign.
5. **Sæt en tærskel**: f.eks. "lever ikke under 0,75 samlet score".

---

### Øvelse 6 - Forbindelse til Zava Editor-Mønsteret

Se tilbage på Zava Creative Writers Editor-agent (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Redaktøren er en LLM-som-dommer i produktion:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Dette er **samme koncept** som Del 8's LLM-som-dommer, men indlejret i produktionspipelineren i stedet for offline testsuite. Begge mønstre:

- Bruger struktureret JSON-output fra modellen.
- Anvender kvalitetskriterier defineret i systemprompten.
- Træffer bestået/ikke-bestået beslutning.

**Forskellen:** Editor kører i produktion (på hver anmodning). Evalueringsrammen kører i udvikling (før levering).

---

## Centrale Læringer

| Begreb | Læring |
|---------|----------|
| **Golden datasets** | Kuratér testcases tidligt; de er dine regressionstests for AI |
| **Regelbaserede checks** | Hurtige, deterministiske og fanger åbenlyse fejl (længde, nøgleord, format) |
| **LLM-som-dommer** | Nuanceret kvalitetsscore med samme lokale model |
| **Prompt-sammenligning** | Kør flere varianter mod samme testsuite for at vælge det bedste |
| **Fordel ved on-device** | Al evaluering køres lokalt: ingen API-omkostninger, ingen ratebegrænsninger, ingen data ud af maskinen |
| **Eval før levering** | Sæt kvalitetsgrænser og gated releases baseret på evalueringsscore |

---

## Næste Skridt

- **Skalér op**: Tilføj flere testcases og kategorier til dit golden dataset.
- **Automatiser**: Integrer evaluering i din CI/CD-pipeline.
- **Avancerede dommere**: Brug en større model som dommer, mens du tester output fra en mindre model.
- **Spor over tid**: Gem evalueringsresultater for at sammenligne mellem prompt- og modelversioner.

---

## Næste Laboratorium

Fortsæt til [Del 9: Tale-til-tekst med Whisper](part9-whisper-voice-transcription.md) for at udforske tale-til-tekst on-device med Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi bestræber os på nøjagtighed, bedes du være opmærksom på, at automatiske oversættelser kan indeholde fejl eller unøjagtigheder. Det oprindelige dokument på dets oprindelige sprog bør betragtes som den autoritative kilde. For kritiske oplysninger anbefales professionel menneskelig oversættelse. Vi påtager os intet ansvar for eventuelle misforståelser eller fejltolkninger, der opstår som følge af brugen af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->