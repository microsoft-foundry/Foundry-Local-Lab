![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Dio 8: Razvoj vođen evaluacijom s Foundry Local

> **Cilj:** Izgraditi okvir za evaluaciju koji sustavno testira i ocjenjuje vaše AI agente, koristeći isti lokalni model kao i agenta pod testiranjem i kao suca, kako biste mogli pouzdano iterirati nad promptovima prije lansiranja.

## Zašto razvoj vođen evaluacijom?

Kod izrade AI agenata, “izgleda otprilike u redu” nije dovoljno. **Razvoj vođen evaluacijom** tretira rezultate agenta kao kod: prvo pišete testove, mjerite kvalitetu, i šaljete u produkciju tek kad rezultati dostignu prag.

U Zava Creative Writeru (Dio 7), **Editor agent** već djeluje kao lagani evaluator; on odlučuje PRIHVATI ili IZMJENI. Ovaj laboratorij formalizira taj obrazac u ponovljiv okvir za evaluaciju koji možete primijeniti na bilo kojeg agenta ili tijek rada.

| Problem | Rješenje |
|---------|----------|
| Promjene prompta tiho kvare kvalitetu | **Zlatni skup podataka** hvata regresije |
| Predrasuda "radi na jednom primjeru" | **Više testnih slučajeva** otkriva rubne slučajeve |
| Subjektivna procjena kvalitete | **Ocjenjivanje pomoću pravila + LLM kao sudac** daje brojke |
| Nema načina za usporedbu varijanti prompta | **Evaluacije jedna uz drugu** s agregiranim rezultatima |

---

## Ključni pojmovi

### 1. Zlatni skup podataka

**Zlatni skup podataka** je kuriran skup testnih slučajeva s poznatim očekivanim ishodima. Svaki testni slučaj uključuje:

- **Ulaz**: prompt ili pitanje koje se šalje agentu
- **Očekivani rezultat**: što točan ili kvalitetan odgovor treba sadržavati (ključne riječi, strukturu, činjenice)
- **Kategorija**: grupiranje za izvještavanje (npr. "činjenična točnost", "ton", "potpunost")

### 2. Provjere temeljene na pravilima

Brze, determinističke provjere koje ne zahtijevaju LLM:

| Provjera | Što testira |
|----------|--------------|
| **Granične duljine** | Odgovor nije prekratak (lenj) ili predug (razvučen) |
| **Potrebne ključne riječi** | Odgovor spominje očekivane termine ili entitete |
| **Validacija formata** | JSON je parsabilan, Markdown naslovi su prisutni |
| **Zabranjeni sadržaj** | Nema izmišljenih naziva brendova, nema spominjanja konkurenata |

### 3. LLM kao sudac

Koristite **isti lokalni model** da ocjenjuje vlastite rezultate (ili rezultate iz druge varijante prompta). Sudac prima:

- Izvorno pitanje
- Odgovor agenta
- Kriterije ocjenjivanja

I vraća strukturirani rezultat. Ovo je ogledni obrazac Editora iz dijela 7, ali sustavno primijenjen na cijeli testni skup.

### 4. Evaluacijom vođeni ciklus iteracije

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Preduvjeti

| Zahtjev | Detalji |
|---------|---------|
| **Foundry Local CLI** | Instaliran s preuzetim modelom |
| **Okruženje za jezik** | **Python 3.9+** i/ili **Node.js 18+** i/ili **.NET 9+ SDK** |
| **Završeno** | [Dio 5: Pojedinačni agenti](part5-single-agents.md) i [Dio 6: Višestruki agenti i tijekovi rada](part6-multi-agent-workflows.md) |

---

## Laboratorijske vježbe

### Vježba 1 - Pokrenite okvir za evaluaciju

Radionica uključuje potpun uzorak evaluacije koji testira Foundry Local agenta na temelju zlatnog skupa pitanja vezanih za Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Postavljanje:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```
  
**Pokretanje:**
```bash
python foundry-local-eval.py
```
  
**Što se događa:**
1. Povezuje se s Foundry Local i učitava model  
2. Definira zlatni skup podataka od 5 testnih slučajeva (Zava pitanja o proizvodima)  
3. Pokreću se dvije varijante prompta za svaki testni slučaj  
4. Svaki odgovor se boduje pomoću **provjera temeljenih na pravilima** (duljina, ključne riječi, format)  
5. Svaki odgovor se boduje pomoću **LLM kao sudca** (isti model ocjenjuje kvalitetu od 1 do 5)  
6. Ispisuje se rezultat s usporedbom oba prompta  

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Postavljanje:**
```bash
cd javascript
npm install
```
  
**Pokretanje:**
```bash
node foundry-local-eval.mjs
```
  
**Isti evaluacijski tijek:** zlatni skup podataka, dvostruko pokretanje prompta, bodovanje temeljeno na pravilima + LLM, tablica rezultata.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Postavljanje:**
```bash
cd csharp
dotnet restore
```
  
**Pokretanje:**
```bash
dotnet run eval
```
  
**Isti evaluacijski tijek:** zlatni skup podataka, dvostruko pokretanje prompta, bodovanje temeljeno na pravilima + LLM, tablica rezultata.

</details>

---

### Vježba 2 - Razumijevanje zlatnog skupa podataka

Pregledajte testne slučajeve definirane u evaluacijskom uzorku. Svaki testni slučaj sadrži:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```
  
**Pitanja za razmišljanje:**  
1. Zašto su očekivane vrijednosti **ključne riječi**, a ne cijele rečenice?  
2. Koliko testnih slučajeva trebate za pouzdanu evaluaciju?  
3. Koje biste kategorije dodali za svoju vlastitu primjenu?  

---

### Vježba 3 - Razumijevanje bodovanja temeljeg na pravilima vs LLM kao sudca

Okvir za evaluaciju koristi **dva sloja ocjenjivanja**:

#### Provjere temeljene na pravilima (brzo, deterministički)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```
  
#### LLM kao sudac (nijansirano, kvalitativno)

Isti lokalni model djeluje kao sudac sa strukturiranom rubrikom:

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
  
**Pitanja za razmišljanje:**  
1. Kada biste više vjerovali provjerama temeljenim na pravilima nego LLM-u kao sudcu?  
2. Može li model pouzdano ocjenjivati vlastite odgovore? Koja su ograničenja?  
3. Kako se ovo uspoređuje s Editor agent obrascem iz dijela 7?

---

### Vježba 4 - Usporedba varijanti prompta

Uzorak pokreće **dvije varijante prompta** na istim testnim slučajevima:

| Varijanta | Stil sistemskog prompta |
|-----------|------------------------|
| **Osnovna** | Generički: "Vi ste korisni asistent" |
| **Specijalizirana** | Detaljni: "Vi ste Zava DIY stručnjak koji preporučuje specifične proizvode i daje korak-po-korak upute" |

Nakon pokretanja vidjet ćete tablicu rezultata poput ove:

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
  
**Zadaci:**  
1. Pokrenite evaluaciju i zabilježite rezultate za svaku varijantu  
2. Modificirajte specijalizirani prompt da bude još specifičniji. Poboljšava li se rezultat?  
3. Dodajte treću varijantu prompta i usporedite svih triju.  
4. Pokušajte promijeniti alias modela (npr. `phi-4-mini` vs `phi-3.5-mini`) i usporedite rezultate.

---

### Vježba 5 - Primijenite evaluaciju na vlastitog agenta

Koristite okvir za evaluaciju kao predložak za vlastite agente:

1. **Definirajte vlastiti zlatni skup podataka**: napišite 5 do 10 testnih slučajeva s očekivanim ključnim riječima.  
2. **Napišite svoj sistemski prompt**: upute agenta koje želite testirati.  
3. **Pokrenite evaluaciju**: dobijte osnovne rezultate.  
4. **Iterirajte**: doradite prompt, ponovno pokrenite i usporedite.  
5. **Postavite prag**: npr. “ne šalji ako je ukupni rezultat ispod 0.75”.

---

### Vježba 6 - Povezanost s obrascem Zava Editora

Pogledajte Editor agenta iz Zava Creative Writera (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Uređivač je LLM-kao-sudac u produkciji:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```
  
Ovo je **isti koncept** kao LLM kao sudac u Dijelu 8, ali ugrađen u produkcijski tijek, a ne u offline testni skup. Oba obrasca:

- Koriste strukturirani JSON izlaz iz modela.  
- Primjenjuju kriterije kvalitete definirane u sistemskom promptu.  
- Donose odluku o prolazu/neprolazu.

**Razlika:** Editor radi u produkciji (na svakom zahtjevu). Okvir za evaluaciju radi u razvoju (prije objave).

---

## Ključne pouke

| Pojam | Pouka |
|-------|--------|
| **Zlatni skupovi podataka** | Napravite testne slučajeve rano; oni su vaši regresijski testovi za AI |
| **Provjere temeljene na pravilima** | Brze, determinističke i hvataju očite pogreške (duljina, ključne riječi, format) |
| **LLM kao sudac** | Nijansirano ocjenjivanje kvalitete pomoću istog lokalnog modela |
| **Usporedba prompta** | Pokrenite više varijanti na istom skupu da odaberete najbolju |
| **Prednost na uređaju** | Sva evaluacija se izvršava lokalno: bez troškova API-ja, bez ograničenja, bez slanja podataka iz uređaja |
| **Evaluacija prije isporuke** | Postavite kvalitetne pragove i ograničite objave prema rezultatima |

---

## Sljedeći koraci

- **Širite se:** Dodajte više testnih slučajeva i kategorija u svoj zlatni skup podataka.  
- **Automatizirajte:** Integrirajte evaluaciju u svoj CI/CD pipeline.  
- **Napredni suci:** Koristite veći model kao suca dok testirate izlaz manjeg modela.  
- **Pratite tijekom vremena:** Spremite rezultate evaluacije da biste uspoređivali verzije prompta i modela.

---

## Sljedeći laboratorij

Nastavite na [Dio 9: Prenos glasa pomoću Whisper](part9-whisper-voice-transcription.md) kako biste istražili pretvaranje govora u tekst na uređaju koristeći Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje od odgovornosti**:  
Ovaj je dokument preveden korištenjem AI usluge za prijevod [Co-op Translator](https://github.com/Azure/co-op-translator). Iako težimo točnosti, imajte na umu da automatski prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na izvornom jeziku treba smatrati službenim izvorom. Za ključne informacije preporučuje se stručni ljudski prijevod. Nismo odgovorni za bilo kakve nesporazume ili pogrešna tumačenja nastala korištenjem ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->