![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 8: Utveckling ledd av utvärdering med Foundry Local

> **Mål:** Bygg ett utvärderingsramverk som systematiskt testar och poängsätter dina AI-agenter, med samma lokala modell både som agent under test och som domare, så att du kan iterera på prompts med förtroende innan leverans.

## Varför utveckling ledd av utvärdering?

När du bygger AI-agenter är "det ser ungefär rätt ut" inte tillräckligt. **Utveckling ledd av utvärdering** behandlar agenters svar som kod: du skriver tester först, mäter kvalitet och levererar endast när poängen når en tröskel.

I Zava Creative Writer (Del 7) fungerar **Editor-agenten** redan som en lättviktsutvärderare; den beslutar GODKÄNN eller REVIDERA. Denna lab formaliserar det mönstret till ett upprepbart utvärderingsramverk som du kan applicera på vilken agent eller pipeline som helst.

| Problem | Lösning |
|---------|----------|
| Promptändringar bryter tyst kvaliteten | **Golden dataset** fångar regressioner |
| Bias "fungerar på ett exempel" | **Flera testfall** avslöjar kantfall |
| Subjektiv kvalitetsbedömning | **Regelbaserad + LLM-som-domare-poängsättning** ger siffror |
| Ingen jämförelse av promptvarianter | **Sid-vid-sid utvärderingskörningar** med aggregatpoäng |

---

## Nyckelkoncept

### 1. Golden Datasets

En **golden dataset** är en kurerad samling testfall med kända förväntade utdata. Varje testfall innehåller:

- **Indata**: Prompten eller frågan som skickas till agenten
- **Förväntat svar**: Vad ett korrekt eller högkvalitativt svar bör innehålla (nyckelord, struktur, fakta)
- **Kategori**: Gruppering för rapportering (t.ex. "faktuell korrekthet", "ton", "fullständighet")

### 2. Regelbaserade kontroller

Snabba, deterministiska kontroller som inte kräver en LLM:

| Kontroll | Vad den testar |
|-------|--------------|
| **Längdgränser** | Svar är inte för kort (slött) eller för långt (omständligt) |
| **Obligatoriska nyckelord** | Svar nämner förväntade termer eller entiteter |
| **Formatvalidering** | JSON är parsbar, Markdown-headers finns |
| **Förbjudet innehåll** | Inga hallucinerade varumärkesnamn, inga omnämnanden av konkurrenter |

### 3. LLM-som-domare

Använd **samma lokala modell** för att bedöma sina egna svar (eller svar från en annan promptvariant). Domaren får:

- Den ursprungliga frågan
- Agentens svar
- Bedömningskriterier

Och returnerar en strukturerad poäng. Detta speglar Editor-mönstret från Del 7 men tillämpat systematiskt över en testsuite.

### 4. Utvärderingsstyrd iterativ loop

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Förutsättningar

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installerad med en nedladdad modell |
| **Språkruntimes** | **Python 3.9+** och/eller **Node.js 18+** och/eller **.NET 9+ SDK** |
| **Genomförda** | [Del 5: Enstaka agenter](part5-single-agents.md) och [Del 6: Multi-agent arbetsflöden](part6-multi-agent-workflows.md) |

---

## Labövningar

### Övning 1 - Kör utvärderingsramverket

Workshopen inkluderar ett komplett utvärderingsprov som testar en Foundry Local-agent mot en golden dataset med Zava DIY-relaterade frågor.

<details>
<summary><strong>🐍 Python</strong></summary>

**Installation:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Kör:**
```bash
python foundry-local-eval.py
```

**Vad som händer:**
1. Ansluter till Foundry Local och laddar modellen
2. Definierar en golden dataset med 5 testfall (Zava produktfrågor)
3. Kör två promptvarianter mot varje testfall
4. Poängsätter varje svar med **regelbaserade kontroller** (längd, nyckelord, format)
5. Poängsätter varje svar med **LLM-som-domare** (samma modell graderar kvalitet 1-5)
6. Skriver ut en poängtabell som jämför båda promptvarianterna

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Installation:**
```bash
cd javascript
npm install
```

**Kör:**
```bash
node foundry-local-eval.mjs
```

**Samma utvärderingspipeline:** golden dataset, dubbla promptkörningar, regelbaserad + LLM-poängsättning, poängtabell.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Installation:**
```bash
cd csharp
dotnet restore
```

**Kör:**
```bash
dotnet run eval
```

**Samma utvärderingspipeline:** golden dataset, dubbla promptkörningar, regelbaserad + LLM-poängsättning, poängtabell.

</details>

---

### Övning 2 - Förstå golden dataset

Granska testfallen definierade i utvärderingsprovet. Varje testfall har:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Frågor att fundera över:**
1. Varför är de förväntade värdena **nyckelord** istället för fullständiga meningar?
2. Hur många testfall behövs för en pålitlig utvärdering?
3. Vilka kategorier skulle du lägga till för din egen applikation?

---

### Övning 3 - Förstå regelbaserad vs LLM-som-domare-poängsättning

Utvärderingsramverket använder **två poängsscikt**:

#### Regelbaserade kontroller (snabba, deterministiska)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-som-domare (nyanserad, kvalitativ)

Samma lokala modell agerar som domare med en strukturerad bedömningsmall:

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

**Frågor att fundera över:**
1. När skulle du lita på regelbaserade kontroller framför LLM-som-domare?
2. Kan en modell pålitligt bedöma sitt eget svar? Vilka begränsningar finns?
3. Hur jämför detta med Editor-agent-mönstret från Del 7?

---

### Övning 4 - Jämför promptvarianter

Exemplet kör **två promptvarianter** mot samma testfall:

| Variant | Systemprompt-stil |
|---------|-------------------|
| **Baslinje** | Generisk: "Du är en hjälpsam assistent" |
| **Specialiserad** | Detaljerad: "Du är en Zava DIY-expert som rekommenderar specifika produkter och ger steg-för-steg vägledning" |

Efter körning kommer du se en poängtabell som:

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

**Övningar:**
1. Kör utvärderingen och notera poängen för varje variant
2. Modifiera den specialiserade prompten för att vara ännu mer specifik. Förbättras poängen?
3. Lägg till en tredje promptvariant och jämför alla tre.
4. Testa att byta modellalias (t.ex. `phi-4-mini` vs `phi-3.5-mini`) och jämför resultaten.

---

### Övning 5 - Använd utvärdering på dina egna agenter

Använd utvärderingsramverket som mall för dina egna agenter:

1. **Definiera din golden dataset**: skriv 5 till 10 testfall med förväntade nyckelord.
2. **Skriv din systemprompt**: agentinstruktionerna du vill testa.
3. **Kör utvärdering**: få baslinjepoäng.
4. **Iterera**: justera prompten, kör om och jämför.
5. **Sätt en tröskel**: t.ex. "skicka inte under 0,75 kombinerad poäng".

---

### Övning 6 - Koppling till Zava Editor-mönstret

Titta tillbaka på Zava Creative Writers Editor-agent (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Editorn är en LLM-som-domare i produktion:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Detta är **samma koncept** som Del 8:s LLM-som-domare, men inbäddat i produktionspipen istället för i en offline testsuite. Båda mönstren:

- Använder strukturerad JSON-utmatning från modellen.
- Tillämpa kvalitetskriterier definierade i systemprompt.
- Gör ett godkänn/underkänn-beslut.

**Skillnaden:** Editorn körs i produktion (på varje förfrågan). Utvärderingsramverket körs i utveckling (innan leverans).

---

## Viktiga lärdomar

| Koncept | Slutsats |
|---------|----------|
| **Golden datasets** | Kura testfall tidigt; de är dina regressionstester för AI |
| **Regelbaserade kontroller** | Snabba, deterministiska och fångar upp uppenbara fel (längd, nyckelord, format) |
| **LLM-som-domare** | Nyanserad kvalitetsbedömning med samma lokala modell |
| **Promptjämförelse** | Kör flera varianter mot samma testsuite för att välja bäst |
| **Fördelar på enheten** | All utvärdering körs lokalt: inga API-kostnader, inga hastighetsbegränsningar, inga data lämnar din dator |
| **Utvärdera före leverans** | Sätt kvalitetsgränser och styr releaser baserat på utvärderingspoäng |

---

## Nästa steg

- **Skalning:** Lägg till fler testfall och kategorier i din golden dataset.
- **Automatisera:** Integrera utvärdering i din CI/CD-pipeline.
- **Avancerade domare:** Använd en större modell som domare medan du testar svar från en mindre modell.
- **Följ över tid:** Spara utvärderingsresultat för att jämföra mellan prompt- och modellversioner.

---

## Nästa labb

Fortsätt till [Del 9: Rösttranskribering med Whisper](part9-whisper-voice-transcription.md) för att utforska tal-till-text lokalt med Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfriskrivning**:
Detta dokument har översatts med hjälp av AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, vänligen var medveten om att automatiska översättningar kan innehålla fel eller felaktigheter. Det ursprungliga dokumentet på dess modersmål bör betraktas som den auktoritativa källan. För kritisk information rekommenderas professionell mänsklig översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår från användningen av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->