![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 8: Razvoj voden z evalvacijo s Foundry Local

> **Cilj:** Zgraditi evalvacijski okvir, ki sistematično testira in ocenjuje vaše AI agente, z uporabo istega lokalnega modela tako kot preizkušani agent kot tudi kot sodnik, da lahko z zaupanjem iterirate pozive pred izdajo.

## Zakaj razvoj voden z evalvacijo?

Pri gradnji AI agentov "izgleda nekako prav" ni dovolj. **Razvoj voden z evalvacijo** obravnava izhode agenta kot kodo: najprej napišete teste, merite kakovost in pošljete le, ko rezultate dosegajo prag.

V Zava Creative Writer (Del 7) **agent urednik** že deluje kot lahek ocenjevalec; odloča za SPREJMI ali POPRAVI. Ta laboratorij formalizira ta vzorec v ponovljiv evalvacijski okvir, ki ga lahko uporabite za katerega koli agenta ali potek dela.

| Problem | Rešitev |
|---------|---------|
| Tihih sprememb poziva, ki znižajo kakovost | **Zlata zbirka podatkov** ujame regresije |
| Pristranskost "deluje na enem primeru" | **Več testnih primerov** razkrije robne primere |
| Subjektivna ocena kakovosti | **Ocena na osnovi pravil + LLM kot sodnik** zagotavlja številke |
| Ni načina za primerjavo variant pozivov | **Evalvacije vzporedno** z združenimi ocenami |

---

## Ključni pojmi

### 1. Zlate zbirke podatkov

**Zlata zbirka podatkov** je kurirani nabor testnih primerov z znanimi pričakovanimi izhodi. Vsak testni primer vsebuje:

- **Vhod:** Poziv ali vprašanje, poslano agentu
- **Pričakovani izhod:** Kaj naj bi pravilni ali visoko kakovosten odgovor vseboval (ključne besede, strukturo, dejstva)
- **Kategorija:** Razvrstitev za poročanje (npr. "dejanska točnost", "ton", "popolnost")

### 2. Preverjanja na osnovi pravil

Hitri, deterministični pregledi, ki ne zahtevajo LLM:

| Preverjanje | Kaj testira |
|-------------|-------------|
| **Meje dolžine** | Odgovor ni prekratek (len) ali predolg (zajeten) |
| **Zahtevane ključne besede** | Odgovor omenja pričakovane izraze ali entitete |
| **Preverjanje oblike** | JSON je razčlenljiv, so prisotni Markdown naslovi |
| **Prepovedana vsebina** | Brez zmotnih imen blagovnih znamk, brez omen konkurenčnih podjetij |

### 3. LLM kot sodnik

Uporabite **isti lokalni model** za ocenjevanje svojih izhodov (ali izhodov z druge različice poziva). Sodnik prejme:

- Izvirno vprašanje
- Odgovor agenta
- Merila za ocenjevanje

In vrne strukturirano oceno. To odraža vzorec urednika iz Del 7, a sistematično uporabljeno po celotni zbirki testov.

### 4. Zanka iteracije voden z evalvacijo

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Predpogoji

| Zahteva | Podrobnosti |
|---------|-------------|
| **Foundry Local CLI** | Nameščen z naloženim modelom |
| **Runtime jezik** | **Python 3.9+** in/ali **Node.js 18+** in/ali **.NET 9+ SDK** |
| **Dokončano** | [Del 5: Enotni agenti](part5-single-agents.md) in [Del 6: Večagentni poteki](part6-multi-agent-workflows.md) |

---

## Laboratorijske vaje

### Vaja 1 - Zaženi evalvacijski okvir

Delavnica vključuje popoln evalvacijski primer, ki testira Foundry Local agenta na zlati zbirki vprašanj, povezanih z Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Nastavitev:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Zaženi:**
```bash
python foundry-local-eval.py
```

**Kaj se zgodi:**
1. Poveže se s Foundry Local in naloži model
2. Določi zlato zbirko 5 testnih primerov (vprašanja o Zava izdelkih)
3. Izvede dva pozivna varianta za vsak testni primer
4. Oceni vsak odgovor z **preverjanji na osnovi pravil** (dolžina, ključne besede, oblika)
5. Oceni vsak odgovor z **LLM kot sodnikom** (isti model oceni kakovost 1-5)
6. Izpiše kartico z ocenami, ki primerja oba pozivna varianta

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Nastavitev:**
```bash
cd javascript
npm install
```

**Zaženi:**
```bash
node foundry-local-eval.mjs
```

**Isti evalvacijski potek:** zlata zbirka, dvojni pozivi, ocenjevanje na osnovi pravil + LLM, kartica z ocenami.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Nastavitev:**
```bash
cd csharp
dotnet restore
```

**Zaženi:**
```bash
dotnet run eval
```

**Isti evalvacijski potek:** zlata zbirka, dvojni pozivi, ocenjevanje na osnovi pravil + LLM, kartica z ocenami.

</details>

---

### Vaja 2 - Razumeti zlato zbirko podatkov

Preglejte testne primere, definirane v evalvacijskem primeru. Vsak ima:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Vprašanja za premislek:**
1. Zakaj so pričakovane vrednosti **ključne besede** in ne celotni stavki?
2. Koliko testnih primerov potrebujete za zanesljivo evalvacijo?
3. Katere kategorije bi dodali za svojo lastno aplikacijo?

---

### Vaja 3 - Razumeti ocenjevanje na osnovi pravil proti LLM kot sodnik

Evalvacijski okvir uporablja **dva nivoja ocenjevanja**:

#### Preverjanja na osnovi pravil (hitro, deterministično)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM kot sodnik (natančno, kvalitativno)

Isti lokalni model deluje kot sodnik z urejeno rubriko:

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

**Vprašanja za premislek:**
1. Kdaj bi zaupali preverjanjem na osnovi pravil bolj kot LLM kot sodniku?
2. Ali lahko model zanesljivo ocenjuje svoj izhod? Kakšne so omejitve?
3. Kako se to primerja z vzorcem urednika iz Dela 7?

---

### Vaja 4 - Primerjaj variante poziva

Vzorec izvaja **dve varianti poziva** za iste testne primere:

| Varianta | Stil sistemskega poziva |
|----------|------------------------|
| **Osnovna** | Splošno: "Si prijazen pomočnik" |
| **Specializirana** | Podrobno: "Si strokovnjak za Zava DIY, ki priporoča specifične izdelke in nudi korak-po-korak navodila" |

Po zagonu boste videli kartico z ocenami, kot je:

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

**Vaje:**
1. Zaženi evalvacijo in zabeleži ocene za vsako varianto
2. Spremeni specializirani poziv, da bo še bolj specifičen. Ali se ocena izboljša?
3. Dodaj tretjo varianto poziva in primerjaj vse tri.
4. Poskusi zamenjati ime modela (npr. `phi-4-mini` proti `phi-3.5-mini`) in primerjaj rezultate.

---

### Vaja 5 - Uporabi evalvacijo na svojem agentu

Uporabite evalvacijski okvir kot predlogo za svoje agente:

1. **Določi svojo zlato zbirko podatkov:** napiši 5 do 10 testnih primerov s pričakovanimi ključnimi besedami.
2. **Napiši svoj sistemski poziv:** navodila za agenta, ki jih želiš testirati.
3. **Zaženi evalvacijo:** pridobi osnovne ocene.
4. **Iteriraj:** prilagodi poziv, ponovno zaženi in primerjaj.
5. **Nastavi prag:** npr. "ne pošiljaj, če skupna ocena ni nad 0,75".

---

### Vaja 6 - Povezava z vzorcem urednika Zava

Oglej si uredniškega agenta iz Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Urejevalnik je LLM kot sodnik v produkciji:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

To je **isti koncept** kot LLM kot sodnik v Deli 8, a vgrajen v produkcijski potek namesto v offline testno zbirko. Oba vzorca:

- Uporabita strukturiran JSON izhod modela.
- Uporabita merila kakovosti, določena v sistemskem pozivu.
- Naredita odločitev za uspeh/napako.

**Razlika:** Urednik teče v produkciji (pri vsakem zahtevku). Evalvacijski okvir teče pri razvoju (pred izdajo).

---

## Ključne ugotovitve

| Pojem | Ugotovitev |
|-------|------------|
| **Zlate zbirke podatkov** | Zgodaj pripravljajte testne primere; so vaši regresijski testi za AI |
| **Preverjanja na osnovi pravil** | Hitro, deterministično in zaznava očitne napake (dolžina, ključne besede, oblika) |
| **LLM kot sodnik** | Natančna kakovostna ocena s pomočjo istega lokalnega modela |
| **Primerjava pozivov** | Poženite več variant z isto zbirko za izbiro najboljše |
| **Prednost na napravi** | Vse evalvacije tečejo lokalno: brez stroškov API, brez omejitev hitrosti, brez izhoda podatkov s stroja |
| **Evalviraj pred izdajo** | Nastavite pragove kakovosti in omejite izdaje glede na ocene evalvacije |

---

## Naslednji koraki

- **Razširi:** Dodaj več testnih primerov in kategorij v svojo zlato zbirko.
- **Avtomatiziraj:** Integriraj evalvacijo v svoj CI/CD potek.
- **Napredni sodniki:** Uporabi večji model kot sodnika medtem ko testiraš izhod manjšega modela.
- **Sledi skozi čas:** Shrani rezultate evalvacije za primerjavo med verzijami pozivov in modelov.

---

## Naslednja vaja

Nadaljuj na [Del 9: Pretvorba glasu s Whisper](part9-whisper-voice-transcription.md) za raziskovanje pretvorbe govora v besedilo na napravi z uporabo Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Omejitev odgovornosti**:  
Ta dokument je bil preveden z uporabo AI prevajalske storitve [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, vas prosimo, da upoštevate, da lahko avtomatski prevodi vsebujejo napake ali netočnosti. Izvirni dokument v njegovem domačem jeziku velja za avtoritativni vir. Za kritične informacije priporočamo strokovni človeški prevod. Za morebitne nesporazume ali napačne interpretacije, ki izhajajo iz uporabe tega prevoda, ne prevzemamo odgovornosti.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->