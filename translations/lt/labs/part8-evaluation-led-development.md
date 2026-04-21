![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 8 dalis: Vertinimu pagrįsta vystymasis su Foundry Local

> **Tikslas:** Sukurti vertinimo sistemą, kuri sistemingai testuoja ir įvertina jūsų AI agentus, naudodama tą patį vietinį modelį tiek kaip testuojamą agentą, tiek kaip teisėją, kad galėtumėte užtikrintai tobulinti užklausas prieš jas išleidžiant.

## Kodėl vertinimu pagrįstas vystymasis?

Kuriant AI agentus, „atrodo, kad viskas gerai“ nėra pakankamai gera. **Vertinimu pagrįstas vystymasis** traktuoja agentų rezultatus kaip kodą: pirmiausia rašote testus, matuojate kokybę ir išleidžiate tik tuomet, kai balai pasiekia nustatytą ribą.

Zava kūrybinio rašytojo (7 dalis) **Redaktoriaus agentas** jau veikia kaip lengvas vertintojas; jis nusprendžia PRITRINKTI arba PERDIRBTI. Ši laboratorija formalizuoja šį modelį į kartojamą vertinimo sistemą, kurią galite taikyti bet kuriam agentui ar procesui.

| Problema | Sprendimas |
|---------|----------|
| Užklausų pakeitimai tyliai sugadina kokybę | **Auksinė duomenų bazė** pagauna regresijas |
| „Veikia vienam pavyzdžiui“ šališkumas | **Daugybė testų atvejų** atskleidžia kraštutinius atvejus |
| Subjektyvus kokybės vertinimas | **Taisyklėmis pagrįsti + LLM-teisėjas balai** pateikia skaičius |
| Nėra būdo palyginti užklausų variantus | **Šalia-ant-šono vertinimai** su bendrais balais |

---

## Pagrindinės sąvokos

### 1. Auksinės duomenų bazės

**Auksinė duomenų bazė** yra parinktas testų atvejų rinkinys su žinomais laukiamais rezultatais. Kiekvienas testas apima:

- **Įvestis:** Užklausa arba klausimas agentui
- **Laukimas:** Kas sudaro teisingą arba aukštos kokybės atsakymą (raktiniai žodžiai, struktūra, faktai)
- **Kategorija:** Grupavimas ataskaitoms (pvz., „faktinė tikslumas“, „tonas“, „išsamumas“)

### 2. Taisyklėmis pagrįsti tikrinimai

Greiti, deterministiniai tikrinimai, kurie nereikalauja LLM:

| Tikrinimas | Ką testuoja |
|------------|-------------|
| **Ilgio ribos** | Atsakymas nėra per trumpas (tingus) ar per ilgas (bekalbė) |
| **Privalomi raktiniai žodžiai** | Atsakyme yra laukiamų terminų ar subjektų |
| **Formatas** | JSON yra išanalizuojamas, yra Markdown antraštės |
| **Draudžiamas turinys** | Nėra sugalvotų prekių ženklų, nėra konkurentų paminėjimų |

### 3. LLM kaip teisėjas

Naudokite **tą patį vietinį modelį** vertinti savo ar kito užklausos varianto rezultatus. Teisėjas gauna:

- Originalų klausimą
- Agento atsakymą
- Vertinimo kriterijus

Ir grąžina struktūrizuotą įvertinimą. Tai atspindi Redaktoriaus modelį iš 7 dalies, bet pritaikyta sistemingai viso testų rinkinio mastu.

### 4. Vertinimu pagrįstas iteracijos ciklas

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Priešpriešos reikalavimai

| Reikalavimas | Detalės |
|--------------|---------|
| **Foundry Local CLI** | Įdiegta su parsisiųstu modeliu |
| **Programavimo kalba** | **Python 3.9+** ir/ar **Node.js 18+** ir/ar **.NET 9+ SDK** |
| **Įvykdyta** | [5 dalis: Vieni agentai](part5-single-agents.md) ir [6 dalis: Multi agentų darbo eiga](part6-multi-agent-workflows.md) |

---

## Laboratoriniai užsiėmimai

### Užduotis 1 – Paleisti vertinimo sistemą

Dirbtuvėse yra pilnas vertinimo pavyzdys, kuris testuoja Foundry Local agentą pagal Zava „pasidaryk pats“ klausimų auksinę duomenų bazę.

<details>
<summary><strong>🐍 Python</strong></summary>

**Paruošimas:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Paleidimas:**
```bash
python foundry-local-eval.py
```

**Kas vyksta:**
1. Prisijungia prie Foundry Local ir užkrauna modelį
2. Apibrėžia 5 testų atvejų auksinę duomenų bazę (Zava produktų klausimai)
3. Vykdo du užklausų variantus kiekvienam testui
4. Įvertina kiekvieną atsakymą pagal **taisyklėmis pagrįstus tikrinimus** (ilgį, raktinius žodžius, formatą)
5. Įvertina pagal **LLM kaip teisėjas** (tas pats modelis vertina kokybę 1–5 balais)
6. Išveda rezultatų kortelę, lyginančią abu užklausų variantus

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Paruošimas:**
```bash
cd javascript
npm install
```

**Paleidimas:**
```bash
node foundry-local-eval.mjs
```

**Ta pati vertinimo eiga:** auksinė duomenų bazė, dvigubi užklausų paleidimai, taisyklėmis pagrįsti + LLM balai, rezultatų kortelė.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Paruošimas:**
```bash
cd csharp
dotnet restore
```

**Paleidimas:**
```bash
dotnet run eval
```

**Ta pati vertinimo eiga:** auksinė duomenų bazė, dvigubi užklausų paleidimai, taisyklėmis pagrįsti + LLM balai, rezultatų kortelė.

</details>

---

### Užduotis 2 – Suprasti auksinę duomenų bazę

Peržiūrėkite pavyzdyje aprašytus testų atvejus. Kiekvienas testas turi:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Klausimai svarstymui:**
1. Kodėl laukiamieji rezultatai yra **raktiniai žodžiai**, o ne pilnos sakinių eilutės?
2. Kiek testų atvejų reikia patikimam vertinimui?
3. Kokias kategorijas pridėtumėte savo programai?

---

### Užduotis 3 – Suprasti taisyklinė vs LLM kaip teisėjas vertinimą

Vertinimo sistema naudoja **du vertinimo sluoksnius**:

#### Taisyklėmis pagrįsti tikrinimai (greiti, deterministiniai)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM kaip teisėjas (niuansuota, kokybinė)

Tas pats vietinis modelis veikia kaip teisėjas su struktūrizuota rubrika:

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

**Klausimai svarstymui:**
1. Kada labiau pasitikėtumėte taisyklių tikrinimais nei LLM teisėju?
2. Ar modelis patikimai gali įvertinti savo rezultatus? Kokios yra ribotumai?
3. Kaip tai lyginasi su Redaktoriaus modeliu 7 dalyje?

---

### Užduotis 4 – Palyginti užklausų variantus

Pavyzdys paleidžia **du užklausų variantus** tuo pačiu testų rinkiniu:

| Variantas | Sistemos užklausos stilius |
|-----------|----------------------------|
| **Pagrindinis** | Bendras: „Jūs esate naudingas asistentas“ |
| **Specializuotas** | Išsamus: „Jūs esate Zava pasidaryk pats ekspertas, rekomenduojantis specifinius produktus ir suteikiantis žingsnis po žingsnio instrukcijas“ |

Paleidus, pamatysite tokį rezultatų kortelę:

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

**Uždaviniai:**
1. Paleiskite vertinimą ir užfiksuokite kiekvieno varianto balus
2. Patobulinkite specializuotą užklausą, kad būtų dar tikslesnė. Ar balas pagerėja?
3. Pridėkite trečią užklausų variantą ir palyginkite visus tris.
4. Pabandykite pakeisti modelio slapyvardį (pvz., `phi-4-mini` prieš `phi-3.5-mini`) ir palyginkite rezultatus.

---

### Užduotis 5 – Taikyti vertinimą savo agentui

Naudokite vertinimo sistemą kaip šabloną savo agentams:

1. **Apibrėžkite savo auksinę duomenų bazę:** parašykite 5–10 testų atvejų su laukiamais raktiniais žodžiais.
2. **Parašykite sistemos užklausą:** agento instrukcijas, kurias norite testuoti.
3. **Paleiskite vertinimą:** gaukite pradinius balus.
4. **Iteruokite:** keiskite užklausą, paleiskite vėl ir palyginkite.
5. **Nustatykite ribą:** pvz., „neišleisti, jei bendra balų suma žemesnė nei 0.75“.

---

### Užduotis 6 – Ryšys su Zava redaktoriaus modeliu

Peržiūrėkite Zava kūrybinio rašytojo Redaktoriaus agentą (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Redaktorius yra LLM kaip teisėjas gamyboje:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Tai yra **tas pats koncepcinis modelis** kaip 8 dalies LLM teisėjas, bet įdėtas į gamybinį procesą, o ne vykdomas tik offline testų rinkinyje. Abi modeliai:

- Naudoja struktūrizuotą JSON išvestį iš modelio.
- Taiko kokybės kriterijus, nurodytus sistemos užklausoje.
- Priima praleidimo/arba taisymo sprendimą.

**Skirtumas:** Redaktorius veikia gamyboje (kiekvienam užklausos prašymui). Vertinimo sistema veikia vystymo metu (prieš išleidimą).

---

## Pagrindinės pamokos

| Sąvoka | Esminė mintis |
|---------|---------------|
| **Auksinės duomenų bazės** | Anksti parinkite testų atvejus; tai jūsų regresijos testai AI |
| **Taisyklėmis pagrįsti tikrinimai** | Greiti, deterministiniai ir pagauna akivaizdžias klaidas (ilgis, raktiniai žodžiai, formatas) |
| **LLM kaip teisėjas** | Niuansuotas kokybės vertinimas naudojant tą patį vietinį modelį |
| **Užklausų palyginimas** | Paleiskite skirtingus variantus viename testų rinkinyje, kad pasirinktumėte geriausią |
| **Vietinis vykdymas** | Visa vertinimo eiga vyksta vietoje: jokių API sąnaudų, jokių ribojimų, neišvyksta duomenys |
| **Vertinti prieš išleidžiant** | Nustatykite kokybės ribas ir leidimus remkite vertinimo rezultatais |

---

## Tolimesni žingsniai

- **Išplėsti**: pridėkite daugiau testų ir kategorijų savo auksinėje duomenų bazėje.
- **Automatizuoti**: integruokite vertinimą į CI/CD procesą.
- **Išplėstiniai teisėjai**: naudokite didesnį modelį kaip teisėją vertinant mažesnio modelio rezultatus.
- **Stebėti laikui bėgant**: išsaugokite vertinimo rezultatus, kad galėtumėte lyginti skirtingas užklausų ir modelių versijas.

---

## Kitas laboratorinis darbas

Tęskite prie [9 dalies: Balso transkripcija su Whisper](part9-whisper-voice-transcription.md), kad išbandytumėte balso į tekstą funkcionalumą įrenginyje naudojant Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:  
Šis dokumentas buvo išverstas naudojant dirbtinio intelekto vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors siekiame tikslumo, prašome atkreipti dėmesį, kad automatiniai vertimai gali turėti klaidų ar netikslumų. Pirminis dokumentas jo gimtąja kalba turėtų būti laikomas autoritetingu šaltiniu. Svarbiai informacijai rekomenduojamas profesionalus žmogaus vertimas. Mes neatsakome už bet kokius nesusipratimus ar neteisingus aiškinimus, kilusius iš šio vertimo naudojimo.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->