![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 8: Evalueringstyrt utvikling med Foundry Local

> **Mål:** Bygg et evalueringsrammeverk som systematisk tester og gir poeng til AI-agentene dine, ved å bruke samme lokale modell som både agenten under test og dommeren, slik at du kan iterere på promptene med selvtillit før levering.

## Hvorfor evalueringstyrt utvikling?

Når du bygger AI-agenter, er "det ser omtrent riktig ut" ikke godt nok. **Evalueringstyrt utvikling** behandler agentutdata som kode: du skriver tester først, måler kvalitet, og sender kun ut når poengene når en terskel.

I Zava Creative Writer (Del 7) fungerer **Editor-agenten** allerede som en lettvekts evaluator; den bestemmer AKSEPTER eller REVIDER. Dette laboratoriet formaliserer det mønsteret til et repeterbart evalueringsrammeverk du kan bruke på enhver agent eller pipeline.

| Problem | Løsning |
|---------|----------|
| Prompt-endringer bryter kvalitet stille | **Golden dataset** fanger regresjoner |
| "Fungerer på ett eksempel"-skjevhet | **Flere testtilfeller** avslører grensetilfeller |
| Subjektiv kvalitetsvurdering | **Regelbasert + LLM-som-dommer-poeng** gir tall |
| Ingen måte å sammenligne promptvarianter | **Eval-kjøringer side om side** med aggregerte poeng |

---

## Nøkkelkonsepter

### 1. Golden Datasets

Et **golden dataset** er et kuratert sett av testtilfeller med kjente forventede utdata. Hvert testtilfelle inneholder:

- **Input**: Prompten eller spørsmålet du sender til agenten
- **Forventet utdata**: Hva et korrekt eller høy-kvalitets svar bør inneholde (nøkkelord, struktur, fakta)
- **Kategori**: Gruppert for rapportering (f.eks. "faktuell nøyaktighet", "tone", "fullstendighet")

### 2. Regelbaserte sjekker

Raske, deterministiske sjekker som ikke krever en LLM:

| Sjekk | Hva den tester |
|-------|--------------|
| **Lengdegrenser** | Svar er ikke for kort (lat) eller for langt (slentrende) |
| **Nødvendige nøkkelord** | Svar nevner forventede termer eller enheter |
| **Formatvalidering** | JSON kan parses, Markdown-overskrifter er tilstede |
| **Forbudt innhold** | Ingen hallusinerte merkenavn, ingen konkurrentreferanser |

### 3. LLM-som-dommer

Bruk **samme lokale modell** til å vurdere sine egne utdata (eller utdata fra en annen promptvariant). Dommeren mottar:

- Det opprinnelige spørsmålet
- Agentens svar
- Vurderingskriterier

Og returnerer en strukturert poengsum. Dette speiler Editor-mønsteret fra Del 7, men brukes systematisk på tvers av et testsett.

### 4. Evalueringsdrevet iterasjonsløkke

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Forutsetninger

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installert med modell lastet ned |
| **Språkruntime** | **Python 3.9+** og/eller **Node.js 18+** og/eller **.NET 9+ SDK** |
| **Fullført** | [Del 5: Enkeltagenter](part5-single-agents.md) og [Del 6: Multi-Agent Workflows](part6-multi-agent-workflows.md) |

---

## Laboratorieøvelser

### Øvelse 1 - Kjør evalueringsrammeverket

Workshopen inkluderer et komplett evaluerings-eksempel som tester en Foundry Local-agent mot et golden dataset med Zava DIY-relaterte spørsmål.

<details>
<summary><strong>🐍 Python</strong></summary>

**Oppsett:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Kjør:**
```bash
python foundry-local-eval.py
```

**Hva skjer:**
1. Kobler til Foundry Local og laster modellen
2. Definerer et golden dataset med 5 testtilfeller (Zava produktspørsmål)
3. Kjører to promptvarianter mot hvert testtilfelle
4. Gir poeng for hvert svar med **regelbaserte sjekker** (lengde, nøkkelord, format)
5. Gir poeng for hvert svar med **LLM-som-dommer** (samme modell vurderer kvalitet 1-5)
6. Skriver ut et scorekort som sammenligner begge promptvariantene

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Oppsett:**
```bash
cd javascript
npm install
```

**Kjør:**
```bash
node foundry-local-eval.mjs
```

**Samme evalueringspipeline:** golden dataset, doble promptkjøringer, regelbasert + LLM-poeng, scorekort.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Oppsett:**
```bash
cd csharp
dotnet restore
```

**Kjør:**
```bash
dotnet run eval
```

**Samme evalueringspipeline:** golden dataset, doble promptkjøringer, regelbasert + LLM-poeng, scorekort.

</details>

---

### Øvelse 2 - Forstå golden dataset

Undersøk testtilfellene definert i evaluerings-eksempelet. Hvert testtilfelle har:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Spørsmål å vurdere:**
1. Hvorfor er de forventede verdiene **nøkkelord** fremfor fullstendige setninger?
2. Hvor mange testtilfeller trenger du for en pålitelig evaluering?
3. Hvilke kategorier ville du lagt til for din egen applikasjon?

---

### Øvelse 3 - Forstå regelbasert vs LLM-som-dommer-poengsetting

Evalueringsrammeverket bruker **to poengsettingslag**:

#### Regelbaserte sjekker (raske, deterministiske)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-som-dommer (nyansert, kvalitativ)

Samme lokale modell fungerer som dommer med en strukturert rubrikk:

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

**Spørsmål å vurdere:**
1. Når ville du stole på regelbaserte sjekker fremfor LLM-som-dommer?
2. Kan en modell pålitelig vurdere sitt eget utdata? Hva er begrensningene?
3. Hvordan sammenlignes dette med Editor-agent-mønsteret fra Del 7?

---

### Øvelse 4 - Sammenlign promptvarianter

Eksempelet kjører **to promptvarianter** mot de samme testtilfellene:

| Variant | System-Prompt-stil |
|---------|-------------------|
| **Baseline** | Generisk: "Du er en hjelpsom assistent" |
| **Spesialisert** | Detaljert: "Du er en Zava DIY-ekspert som anbefaler spesifikke produkter og gir trinnvis veiledning" |

Etter kjøring vil du se et scorekort som:

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
1. Kjør evalueringen og noter poengene for hver variant
2. Endre den spesialiserte prompten til å være enda mer spesifikk. Forbedres poengsummen?
3. Legg til en tredje promptvariant og sammenlign alle tre.
4. Prøv å endre modellalias (f.eks. `phi-4-mini` vs `phi-3.5-mini`) og sammenlign resultatene.

---

### Øvelse 5 - Bruk evaluering på din egen agent

Bruk evalueringsrammeverket som en mal for dine egne agenter:

1. **Definer ditt golden dataset**: skriv 5 til 10 testtilfeller med forventede nøkkelord.
2. **Skriv din systemprompt**: agentinstruksjonene du ønsker å teste.
3. **Kjør evalueringen**: få baseline-poeng.
4. **Iterer**: juster prompten, kjør på nytt, og sammenlign.
5. **Sett en terskel**: f.eks. "ikke lever under 0.75 kombinert poeng".

---

### Øvelse 6 - Kobling til Zava Editor-mønsteret

Se tilbake på Zava Creative Writer sin Editor-agent (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Redaktøren er en LLM-som-dommer i produksjon:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Dette er det **samme konseptet** som Del 8’s LLM-som-dommer, men innebygd i produksjonspipeline i stedet for i en offline testpakke. Begge mønstrene:

- Bruker strukturert JSON-utdata fra modellen.
- Benytter kvalitetskriterier definert i systemprompten.
- Tar en godkjent/ikke godkjent beslutning.

**Forskjellen:** Editor kjører i produksjon (på hver forespørsel). Evalueringsrammeverket kjører i utvikling (før levering).

---

## Viktige konklusjoner

| Konsept | Hovedpoeng |
|---------|----------|
| **Golden datasets** | Kurater testtilfeller tidlig; de er regresjonstestene dine for AI |
| **Regelbaserte sjekker** | Raske, deterministiske, og fanger åpenbare feil (lengde, nøkkelord, format) |
| **LLM-som-dommer** | Nyansert kvalitetspoengsetting med samme lokale modell |
| **Prompt-sammenligning** | Kjør flere varianter mot samme testsuite for å velge den beste |
| **Fordel på enheten** | All evaluering kjøres lokalt: ingen API-kostnader, ingen takbegrensninger, ingen data ut av maskinen |
| **Evaluer-før-levering** | Sett kvalitetsgrenser og styr utgivelser basert på evalueringspoeng |

---

## Neste steg

- **Skaler opp**: Legg til flere testtilfeller og kategorier i ditt golden dataset.
- **Automatiser**: Integrer evaluering i din CI/CD-pipeline.
- **Avanserte dommere**: Bruk en større modell som dommer mens du tester utdata fra en mindre modell.
- **Spor over tid**: Lagre evalueringsresultater for å sammenligne over prompt- og modellversjoner.

---

## Neste lab

Fortsett til [Del 9: Tale-til-tekst med Whisper](part9-whisper-voice-transcription.md) for å utforske tale-til-tekst lokalt ved bruk av Foundry Local SDK.