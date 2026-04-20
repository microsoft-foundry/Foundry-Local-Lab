![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 8: Hindamisega juhitud arendus Foundry Localiga

> **Eesmärk:** Luua hindamisraamistik, mis süsteemselt testib ja hindab teie tehisintellekti agente, kasutades sama kohalikku mudelit nii testitava agendi kui ka hindajana, et saaksite julgesti käske kohandada enne lõplikku kasutuselevõttu.

## Miks hindamisega juhitud arendus?

AI agentide ehitamisel ei piisa "näib sobivat". **Hindamisega juhitud arendus** käsitleb agendi väljundeid nagu koodi: esmalt kirjutate testid, mõõdate kvaliteeti ja alles siis avaldate, kui skoor vastab künnisele.

Zava Creative Writer'is (Osa 7) toimib **Toimetaja agent** juba kergekaalulise hindajana; ta otsustab KINNITA või MUUDA. See labor formaliseerib selle mustri korduvaks hindamisraamistikuks, mida võite kasutada kõigi agentide või töövoogude puhul.

| Probleem | Lahendus |
|---------|----------|
| Käsu muudatused rikuvad kvaliteeti vaikselt | **Kuldne andmekogu** tabab tagasiminekuid |
| "Töötab ühe näitega" kalduvus | **Mitmed testjuhtumid** avastavad äärealad |
| Subjektiivne kvaliteedi hindamine | **Reeglipõhine + LLM-hindaja** annab numbreid |
| Puudub viis käsuvariantide võrdlemiseks | **Kõrvuti hindamise käivitused** kogutud skooridega |

---

## Põhimõisted

### 1. Kuldne andmekogu

**Kuldne andmekogu** on kureeritud testjuhtumite kogum koos teadaolevate oodatavate tulemusega. Iga test sisaldab:

- **Sisend**: Käsu või küsimuse sõnastus, mis saadetakse agendile
- **Oodatav väljund**: Mida õige või kõrge kvaliteediga vastus peaks sisaldama (märksõnad, struktuur, faktid)
- **Kategooria**: Rühmitus aruandluseks (nt "faktitäpsus", "toon", "täielikkus")

### 2. Reeglipõhised kontrollid

Kiired, deterministlikud kontrollid, mis ei vaja LLM-i:

| Kontroll | Mida see testib |
|----------|-----------------|
| **Pikkuse piirid** | Vastus ei ole liiga lühike (laisk) ega liiga pikk (lobisev) |
| **Nõutud märksõnad** | Vastus mainib oodatud termineid või üksusi |
| **Formaadi valideerimine** | JSON on parsideatav, Markdowni päised on kohal |
| **Keelatud sisu** | Puuduvad hallutsineeritud brändinimed, puuduvad konkurendi viited |

### 3. LLM-hindaja

Kasutage **sama kohalikku mudelit** oma väljundite (või mõne teise käsuvariandi väljundite) hindamiseks. Hindaja saab:

- Originaalküsimuse
- Agendi vastuse
- Hindamiskriteeriumid

Ja tagastab struktureeritud skoori. See peegeldab osa 7 Toimetaja mustrit, kuid rakendatuna süsteemselt testikomplekti ulatuses.

### 4. Hindamisega juhitud iteratsiooni tsükkel

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Eeldused

| Nõue | Detailid |
|-------|---------|
| **Foundry Local CLI** | Paigaldatud koos mudeli allalaadimisega |
| **Keele runtime** | **Python 3.9+** ja/või **Node.js 18+** ja/või **.NET 9+ SDK** |
| **Läbitud** | [Osa 5: Üksikud agentid](part5-single-agents.md) ja [Osa 6: Mitme agendi töövood](part6-multi-agent-workflows.md) |

---

## Labori harjutused

### Harjutus 1 - Käivita hindamisraamistik

Workshop sisaldab täielikku hindamise näidet, mis testib Foundry Local agenti kuldse andmekogu Zava DIY küsimuste alusel.

<details>
<summary><strong>🐍 Python</strong></summary>

**Seadistamine:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Käivita:**
```bash
python foundry-local-eval.py
```

**Mis toimub:**
1. Ühendub Foundry Localiga ja laadib mudeli
2. Määratleb kuldse andmekogu 5 testjuhtumiks (Zava tooteküsimused)
3. Käivitab iga testi kohta kaks käsuvarianti
4. Hindab iga vastust **reeglipõhiste kontrollidega** (pikkus, märksõnad, formaat)
5. Hindab iga vastust **LLM-hindajaga** (sama mudel hindab kvaliteeti 1–5)
6. Prindib skoorikaardi, mis võrdleb mõlemaid käsuvariante

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Seadistamine:**
```bash
cd javascript
npm install
```

**Käivita:**
```bash
node foundry-local-eval.mjs
```

**Sama hindamisvool:** kuldne andmekogu, topeltkäsud, reeglipõhine + LLM skoorimine, skoorikaart.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Seadistamine:**
```bash
cd csharp
dotnet restore
```

**Käivita:**
```bash
dotnet run eval
```

**Sama hindamisvool:** kuldne andmekogu, topeltkäsud, reeglipõhine + LLM skoorimine, skoorikaart.

</details>

---

### Harjutus 2 - Mõista kuldset andmekogu

Vaadake hindamisnäites määratletud testjuhtumeid. Igal testjuhtumil on:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Küsimused kaalumiseks:**
1. Miks oodatavad väärtused on **märksõnad**, mitte täislauseid?
2. Mitu testjuhtumit on usaldusväärseks hindamiseks vaja?
3. Milliseid kategooriaid lisaksite oma rakendusele?

---

### Harjutus 3 - Mõista reeglipõhist ja LLM-hindaja skoorimist

Hindamisraamistik kasutab **kaht skoorimiskihi**:

#### Reeglipõhised kontrollid (kiired, deterministlikud)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-hindaja (peenekoeline, kvalitatiivne)

Sama kohalik mudel toimib hindajana struktureeritud hindamisjuhiste alusel:

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

**Küsimused kaalumiseks:**
1. Millal usuksite pigem reeglipõhiseid kontrollid kui LLM-hindajat?
2. Kas mudel suudab oma väljundit usaldusväärselt hinnata? Millised on piirangud?
3. Kuidas see võrreldub Osa 7 Toimetaja agent mustriga?

---

### Harjutus 4 - Võrdle käsuvariante

Näidis käivitab **kaks käsuvarianti** sama testikomplekti vastu:

| Variant | Süsteemi käsu stiil |
|---------|---------------------|
| **Algne** | Üldine: "Sa oled abivalmis assistent" |
| **Spetsialiseeritud** | Üksikasjalik: "Sa oled Zava DIY ekspert, kes soovitab konkreetseid tooteid ja annab samm-sammult juhised" |

Pärast käivitamist näete skoorikaarti nagu:

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

**Harjutused:**
1. Käivita hindamine ja märgi iga variandi skoorid üles
2. Muuda spetsialiseeritud käsku veelgi konkreetsemaks. Kas skoor paraneb?
3. Lisa kolmas käskuvariant ja võrdle kõiki kolme.
4. Proovi mudeli aliasit muuta (nt `phi-4-mini` vs `phi-3.5-mini`) ja võrdle tulemusi.

---

### Harjutus 5 - Kasuta hindamist oma agendi jaoks

Kasuta hindamisraamistikku mallina oma agentide jaoks:

1. **Määra oma kuldne andmekogu**: kirjuta 5–10 testjuhtumit koos oodatud märksõnadega.
2. **Kirjuta oma süsteemi käsk**: agenti juhised, mida testida.
3. **Käivita hindamine**: saa lähtepunkti skoorid.
4. **Itereeri**: kohanda käsku, käivita uuesti ja võrdle.
5. **Sea künnis**: nt "ära avalda, kui kombineeritud skoor on alla 0,75".

---

### Harjutus 6 - Seos Zava Toimetaja murranguga

Vaadake uuesti Zava Creative Writer'i Toimetaja agenti (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Redaktor on tootmises LLM-kohus:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

See on **sama kontseptsioon** mis Osa 8 LLM-hindajad, aga integreeritud tootmisvoogu, mitte offline testikomplekti. Mõlemad mustrid:

- Kasutavad mudelist struktureeritud JSON väljundit.
- Rakendavad süsteemi käsus määratletud kvaliteedikriteeriumid.
- Teevad läbipääsu/eittäinemise otsuse.

**Erinevus:** Toimetaja töötab tootmises (igal päringul). Hindamisraamistik töötab arenduses (enne avaldamist).

---

## Peamised õppetunnid

| Mõiste | Õppetund |
|---------|----------|
| **Kuldne andmekogu** | Koosta testjuhtumeid varakult; need on sinu AI regressioonitestid |
| **Reeglipõhised kontrollid** | Kiired, deterministlikud ja tabavad ilmselged vead (pikkus, märksõnad, formaat) |
| **LLM-hindaja** | Peenekoeline kvaliteedi hindamine sama kohaliku mudeliga |
| **Käsu võrdlus** | Käivita mitu varianti sama testikomplekti vastu parima valimiseks |
| **Seadmeline eelis** | Kõik hindamised toimuvad kohapeal: pole API kulusid, piiranguid ega andmete väljaviimist |
| **Hinda enne avaldamist** | Sea kvaliteedikünnised ja lase väljaanded läbida hindamised |

---

## Järgmised sammud

- **Skaala üles**: Lisa rohkem testjuhtumeid ja kategooriaid oma kuldsesse andmekogusse.
- **Automatiseeri**: Integreeri hindamine oma CI/CD voogu.
- **Täpsemad hindajad**: Kasuta suuremat mudelit hindajana, samal ajal testides väiksema mudeli väljundit.
- **Jälgi ajas**: Salvesta hindamistulemused, et võrrelda käsu- ja mudeliversioonide vahel.

---

## Järgmine labor

Jätka [Osa 9: Hääle transkriptsioon Whisperiga](part9-whisper-voice-transcription.md), et uurida kõne tekstiks seadmel Foundry Local SDK abil.