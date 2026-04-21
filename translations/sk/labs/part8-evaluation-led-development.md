![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Časť 8: Vývoj riadený hodnotením s Foundry Local

> **Cieľ:** Vytvoriť evaluačný rámec, ktorý systematicky testuje a hodnotí vašich AI agentov, pričom používa rovnaký lokálny model ako testovaný agent a aj ako rozhodcu, aby ste mohli s dôverou iterovať nad promptami pred ich nasadením.

## Prečo vývoj riadený hodnotením?

Pri budovaní AI agentov nestačí, že „to vyzerá tak nejako správne“. **Vývoj riadený hodnotením** pristupuje k výstupom agenta ako ku kódu: najskôr píšete testy, meriate kvalitu a nasadzujete až vtedy, keď skóre dosiahne nastavený prah.

V Zava Creative Writerovi (časť 7) už **Editor agent** funguje ako jednoduchý hodnotiteľ; rozhoduje o PRIJATÍ alebo ÚPRAVE. Tento lab túto šablónu formálne spracováva do opakovateľného evaluačného rámca, ktorý môžete aplikovať na akéhokoľvek agenta alebo pipeline.

| Problém | Riešenie |
|---------|----------|
| Zmeny promptu neviditeľne znižujú kvalitu | **Zlatý dataset** zachytí regresie |
| Zaujatosť „funguje na jednom príklade“ | **Viacero testovacích prípadov** odhalí okrajové situácie |
| Subjektívne hodnotenie kvality | **Skórovanie na základe pravidiel + LLM ako rozhodca** prináša číselné hodnoty |
| Nemožnosť porovnať varianty promptu | **Súbežné hodnotenie** s agregovanými skóre |

---

## Kľúčové pojmy

### 1. Zlaté datasety

**Zlatý dataset** je vybraná množina testovacích prípadov so známymi očakávanými výstupmi. Každý testovací prípad obsahuje:

- **Vstup**: Prompt alebo otázka poslaná agentovi
- **Očakávaný výstup**: Čo by mal správny alebo kvalitný výstup obsahovať (kľúčové slová, štruktúra, fakty)
- **Kategória**: Zoskupenie na účely reportovania (napr. „faktická presnosť“, „tón“, „komplexnosť“)

### 2. Kontroly založené na pravidlách

Rýchle, deterministické kontroly, ktoré nevyžadujú LLM:

| Kontrola | Čo testuje |
|----------|------------|
| **Dĺžkové hranice** | Odpoveď nie je príliš krátka (línosť) ani príliš dlhá (odbočovanie) |
| **Povinné kľúčové slová** | Odpoveď spomína očakávané výrazy alebo entity |
| **Validácia formátu** | JSON je parsovateľný, prítomné sú hlavičky Markdown |
| **Zakázaný obsah** | Žiadne nezmyselné názvy značiek, žiadne zmienky o konkurencii |

### 3. LLM ako rozhodca

Použite **ten istý lokálny model** na hodnotenie vlastných výstupov (alebo výstupov inej varianty promptu). Rozhodca dostane:

- Pôvodnú otázku
- Odpoveď agenta
- Kritériá hodnotenia

A vráti štruktúrované skóre. To zodpovedá Editor vzoru z časti 7, ale aplikovanému systematicky v rámci testovacej sady.

### 4. Iteračný cyklus riadený evaluáciou

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Predpoklady

| Požiadavka | Podrobnosti |
|------------|-------------|
| **Foundry Local CLI** | Nainštalovaný s načítaným modelom |
| **Jazykové runtime** | **Python 3.9+** a/alebo **Node.js 18+** a/alebo **.NET 9+ SDK** |
| **Dokončené** | [Časť 5: Jednotliví agenti](part5-single-agents.md) a [Časť 6: Workflow s viacerými agentmi](part6-multi-agent-workflows.md) |

---

## Laboratórne cvičenia

### Cvičenie 1 - Spustenie evaluačného rámca

Workshop obsahuje kompletný evaluačný príklad, ktorý testuje agenta Foundry Local na zlatom datasete otázok súvisiacich so Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Nastavenie:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Spustenie:**
```bash
python foundry-local-eval.py
```

**Čo sa stane:**
1. Pripojí sa k Foundry Local a načíta model
2. Definuje zlatý dataset 5 testovacích prípadov (otázky na produkty Zava)
3. Spustí dve varianty promptov na každý testovací prípad
4. Skóruje každú odpoveď pomocou **kontrol podľa pravidiel** (dĺžka, kľúčové slová, formát)
5. Skóruje každú odpoveď pomocou **LLM ako rozhodca** (ten istý model hodnotí kvalitu stupnicou 1-5)
6. Vypíše skórovaciu tabuľku porovnávajúcu obe varianty promptu

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Nastavenie:**
```bash
cd javascript
npm install
```

**Spustenie:**
```bash
node foundry-local-eval.mjs
```

**Totožný evaluačný pipeline:** zlatý dataset, dvojité spustenie promptov, skórovanie podľa pravidiel + LLM, skórovacia tabuľka.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Nastavenie:**
```bash
cd csharp
dotnet restore
```

**Spustenie:**
```bash
dotnet run eval
```

**Totožný evaluačný pipeline:** zlatý dataset, dvojité spustenie promptov, skórovanie podľa pravidiel + LLM, skórovacia tabuľka.

</details>

---

### Cvičenie 2 - Pochopenie zlatého datasetu

Prezrite si testovacie prípady definované v evaluačnom príklade. Každý testovací prípad obsahuje:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Otázky na zamyslenie:**
1. Prečo sú očakávané hodnoty **kľúčové slová** namiesto celých viet?
2. Koľko testovacích prípadov potrebujete pre spoľahlivé hodnotenie?
3. Aké kategórie by ste pridali pre svoje vlastné použitie?

---

### Cvičenie 3 - Pochopenie skórovania podľa pravidiel vs LLM ako rozhodca

Evaluačný rámec používa **dve vrstvy skórovania**:

#### Kontroly podľa pravidiel (rýchle, deterministické)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM ako rozhodca (nuančné, kvalitatívne)

Ten istý lokálny model funguje ako rozhodca so štruktúrovanou rubrikou:

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

**Otázky na zamyslenie:**
1. Kedy by ste dôverovali viac kontrolám podľa pravidiel než LLM ako rozhodcovi?
2. Môže model spoľahlivo hodnotiť svoj vlastný výstup? Aké sú obmedzenia?
3. Ako to porovnávate so vzorom Editor agenta z časti 7?

---

### Cvičenie 4 - Porovnanie variant promptov

Príklad spustí **dve varianty promptov** na rovnaké testovacie prípady:

| Varianta | Štýl systémového promptu |
|----------|--------------------------|
| **Základná** | Všeobecný: „Ste nápomocný asistent“ |
| **Špecializovaná** | Detailná: „Ste expert na Zava DIY, ktorý odporúča konkrétne produkty a poskytuje krok-za-krokom návody“ |

Po spustení uvidíte skórovaciu tabuľku ako:

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

**Cvičenia:**
1. Spustite hodnotenie a zaznamenajte skóre každej varianty
2. Upravte špecializovaný prompt, aby bol ešte konkrétnejší. Zlepšilo sa skóre?
3. Pridajte tretiu variantu promptu a porovnajte všetky tri.
4. Skúste zmeniť alias modelu (napr. `phi-4-mini` vs `phi-3.5-mini`) a porovnajte výsledky.

---

### Cvičenie 5 - Použitie hodnotenia na vlastného agenta

Použite evaluačný rámec ako šablónu pre svojich agentov:

1. **Definujte svoj zlatý dataset**: napíšte 5 až 10 testovacích prípadov s očakávanými kľúčovými slovami.
2. **Napíšte systémový prompt**: inštrukcie pre agenta, ktoré chcete testovať.
3. **Spustite hodnotenie**: získajte základné skóre.
4. **Iterujte**: upravte prompt, znovu spustite a porovnajte.
5. **Nastavte prah**: napr. „nesmie byť nasadené pod 0,75 kombinované skóre“.

---

### Cvičenie 6 - Prepojenie so vzorom Zava Editora

Pozrite sa späť na Editor agenta Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Editor je LLM ako sudca v produkcii:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Toto je **rovnaký koncept** ako LLM ako rozhodca v časti 8, no zabudovaný v produkčnom pipeline namiesto offline testovacej sady. Obe šablóny:

- Používajú štruktúrovaný JSON výstup modelu.
- Aplikujú kvalitné kritériá definované v systémovom prompte.
- Robia rozhodnutie o úspechu/neúspechu.

**Rozdiel:** Editor beží v produkcii (pri každom dopyte). Evaluačný rámec beží vo vývoji (pred nasadením).

---

## Kľúčové poznatky

| Pojem | Zhrnutie |
|-------|----------|
| **Zlaté datasety** | Vytvárajte testovacie prípady skoro; sú vašimi regresnými testami pre AI |
| **Kontroly podľa pravidiel** | Rýchle, deterministické, zachytávajú očividné chyby (dĺžka, kľúčové slová, formát) |
| **LLM ako rozhodca** | Nuančné skórovanie kvality použitím rovnakého lokálneho modelu |
| **Porovnanie promptov** | Spustite viacero variant na rovnakom teste a vyberte najlepší |
| **Výhoda na zariadení** | Všetko beží lokálne: žiadne náklady na API, žiadne limity, žiadne úniky dát |
| **Eval pred nasadením** | Nastavte kvalitné prahy a povoľujte vydania iba ak skóre splní kritéria |

---

## Ďalšie kroky

- **Rozširujte**: Pridajte viac testovacích prípadov a kategórií do zlatého datasetu.
- **Automatizujte**: Integrujte hodnotenie do CI/CD pipeline.
- **Pokročilí rozhodcovia**: Použite väčší model ako rozhodcu, keď testujete výstup menšieho modelu.
- **Sledujte v čase**: Ukladajte výsledky hodnotení na porovnávanie medzi verziami promptov a modelov.

---

## Ďalší lab

Pokračujte do [Časti 9: Prepis hlasu pomocou Whisper](part9-whisper-voice-transcription.md) a preskúmajte prevod reči na text lokálne pomocou Foundry Local SDK.