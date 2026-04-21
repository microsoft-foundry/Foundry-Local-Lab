![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Část 8: Vývoj řízený hodnocením s Foundry Local

> **Cíl:** Vytvořit hodnotící rámec, který systematicky testuje a hodnotí vaše AI agenty, přičemž používá stejný lokální model jak jako testovaný agent, tak i jako soudce, abyste mohli s jistotou iterovat nad promptami před nasazením.

## Proč vývoj řízený hodnocením?

Při tvorbě AI agentů není „vypadá to tak nějak správně“ dostatečné. **Vývoj řízený hodnocením** přistupuje k výstupům agentů jako kódu: nejprve píšete testy, měříte kvalitu a nasazujete jen tehdy, když skóre dosáhne prahu.

V Zava Creative Writer (Část 7) již **Editor agent** funguje jako lehký hodnotitel; rozhoduje, zda PŘIJMOUT nebo ZREVIDOVAT. Tento lab formalizuje tento vzorec do opakovatelných hodnotících rámců, které můžete aplikovat na jakéhokoli agenta či pipeline.

| Problém | Řešení |
|---------|----------|
| Změny promptů tiše zhoršují kvalitu | **Golden dataset** zachytí regrese |
| Bias "funguje na jednom příkladu" | **Více testovacích případů** odhalí okrajové případy |
| Subjektivní hodnocení kvality | **Hodnocení na základě pravidel + LLM jako soudce** poskytuje čísla |
| Žádný způsob porovnání variant promptů | **Souběžné vyhodnocení** s celkovými skóre |

---

## Klíčové pojmy

### 1. Golden datasety

**Golden dataset** je kurátorská sada testovacích případů s známými očekávanými výstupy. Každý testovací případ obsahuje:

- **Vstup**: Prompt či otázku, kterou agentovi pošlete
- **Očekávaný výstup**: Co by měla správná nebo kvalitní odpověď obsahovat (klíčová slova, strukturu, fakta)
- **Kategorie**: Skupinu pro reportování (např. „faktická přesnost“, „tón“, „úplnost“)

### 2. Kontroly založené na pravidlech

Rychlé, deterministické kontroly, které nevyžadují LLM:

| Kontrola | Co testuje |
|----------|------------|
| **Délkové limity** | Odpověď není příliš krátká (líná) ani dlouhá (rozvláčná) |
| **Požadovaná klíčová slova** | Odpověď zmiňuje očekávané termíny nebo entity |
| **Validace formátu** | JSON je parsovatelný, jsou přítomny Markdown záhlaví |
| **Zakázaný obsah** | Žádná halucinovaná značková jména, žádné zmínky konkurence |

### 3. LLM jako soudce

Použijte **stejný lokální model** k hodnocení svých vlastních výstupů (nebo výstupů jiné varianty promptu). Soudce obdrží:

- Původní otázku
- Odpověď agenta
- Kritéria hodnocení

A vrátí strukturované skóre. To kopíruje vzorec Editor agenta z Části 7, ale systematicky aplikované napříč celou testovací sadou.

### 4. Smyčka iterace řízená hodnocením

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Požadavky

| Požadavek | Detaily |
|-----------|---------|
| **Foundry Local CLI** | Nainstalován s modelem staženým |
| **Jazykové runtime** | **Python 3.9+** a/nebo **Node.js 18+** a/nebo **.NET 9+ SDK** |
| **Ukončeno** | [Část 5: Jednotliví agenti](part5-single-agents.md) a [Část 6: Multi-agentní workflow](part6-multi-agent-workflows.md) |

---

## Cvičení v laboratoři

### Cvičení 1 – Spusťte hodnotící rámec

Workshop zahrnuje kompletní evaluační vzorek, který testuje Foundry Local agenta proti golden datasetu otázek souvisejících se Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Nastavení:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Spusťte:**
```bash
python foundry-local-eval.py
```

**Co se stane:**
1. Připojí se k Foundry Local a načte model
2. Definuje golden dataset s 5 testovacími případy (otázky týkající se Zava produktů)
3. Pro každý testovací případ spustí dvě varianty promptu
4. Ohodnotí každou odpověď pomocí **kontrol založených na pravidlech** (délka, klíčová slova, formát)
5. Ohodnotí každou odpověď pomocí **LLM jako soudce** (stejný model uděluje kvalitu 1-5)
6. Vytiskne scorecard porovnávající obě varianty promptu

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Nastavení:**
```bash
cd javascript
npm install
```

**Spusťte:**
```bash
node foundry-local-eval.mjs
```

**Stejný evaluační pipeline:** golden dataset, dvě varianty promptu, hodnocení založené na pravidlech + LLM, scorecard.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Nastavení:**
```bash
cd csharp
dotnet restore
```

**Spusťte:**
```bash
dotnet run eval
```

**Stejný evaluační pipeline:** golden dataset, dvě varianty promptu, hodnocení založené na pravidlech + LLM, scorecard.

</details>

---

### Cvičení 2 – Porozumění Golden Datasetu

Prozkoumejte testovací případy definované ve vzorku hodnocení. Každý testovací případ obsahuje:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Otázky k zamyšlení:**
1. Proč jsou očekávané hodnoty **klíčová slova** spíše než celé věty?
2. Kolik testovacích případů potřebujete pro spolehlivé hodnocení?
3. Jaké kategorie byste přidali pro vaši vlastní aplikaci?

---

### Cvičení 3 – Porozumění hodnocení na základě pravidel vs LLM jako soudce

Evaluační rámec využívá **dvě vrstvy hodnocení**:

#### Kontroly založené na pravidlech (rychlé, deterministické)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM jako soudce (nuancované, kvalitativní)

Stejný lokální model funguje jako soudce se strukturovaným rubrikem:

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

**Otázky k zamyšlení:**
1. Kdy byste raději důvěřovali kontrolám založeným na pravidlech než LLM jako soudci?
2. Může model spolehlivě hodnotit svůj vlastní výstup? Jaká jsou omezení?
3. Jak to srovnáváte se vzorcem Editor agenta z Části 7?

---

### Cvičení 4 – Porovnejte varianty promptu

Vzorek spouští **dvě varianty promptu** nad stejnými testovacími případy:

| Varianta | Styl systémového promptu |
|----------|-------------------------|
| **Základní** | Obecný: „Jsi užitečný asistent“ |
| **Specializovaný** | Detailní: „Jsi expert na Zava DIY, který doporučuje konkrétní produkty a poskytuje krok za krokem instrukce“ |

Po spuštění uvidíte scorecard jako:

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

**Cvičení:**
1. Spusťte hodnocení a zaznamenejte skóre pro každou variantu
2. Upravit specializovaný prompt tak, aby byl ještě specifičtější. Zlepší se skóre?
3. Přidejte třetí variantu promptu a porovnejte všechny tři.
4. Vyzkoušejte změnu aliasu modelu (např. `phi-4-mini` vs `phi-3.5-mini`) a porovnejte výsledky.

---

### Cvičení 5 – Aplikujte hodnocení na svého agenta

Použijte evaluační rámec jako šablonu pro své agenty:

1. **Definujte svůj golden dataset**: napište 5 až 10 testovacích případů s očekávanými klíčovými slovy.
2. **Napište systémový prompt**: instrukce pro agenta, které chcete testovat.
3. **Spusťte hodnocení**: získejte základní skóre.
4. **Iterujte**: doladěte prompt, znovu spusťte a porovnejte.
5. **Nastavte práh**: např. „nenasazujte pod kombinované skóre 0,75“.

---

### Cvičení 6 – Spojení se vzorcem Editoru v Zava

Podívejte se zpět na Editor agenta ze Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Editor je LLM-jako-rozhodčí v produkci:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Toto je **stejný koncept** jako LLM-as-judge v Části 8, ale vložený v produkční pipeline namísto offline testovací sady. Oba vzorce:

- Používají strukturovaný JSON výstup z modelu.
- Aplikují kritéria kvality definovaná v systémovém promptu.
- Dělají rozhodnutí ano/ne.

**Rozdíl:** Editor běží v produkci (na každý požadavek). Evaluační rámec běží ve vývoji (před nasazením).

---

## Klíčová shrnutí

| Pojem | Shrnutí |
|-------|---------|
| **Golden datasety** | Vyvíjejte testovací případy brzy; slouží jako regresní testy pro AI |
| **Kontroly založené na pravidlech** | Rychlé, deterministické, zachytí zjevné chyby (délka, klíčová slova, formát) |
| **LLM jako soudce** | Nuancované hodnocení kvality pomocí stejného lokálního modelu |
| **Porovnání promptů** | Spusťte více variant nad stejnou testovací sadou, abyste vybrali tu nejlepší |
| **Výhoda na zařízení** | Veškeré hodnocení běží lokálně: žádné náklady na API, žádné limity, žádný únik dat z vašeho zařízení |
| **Eval před nasazením** | Nastavte prahové hodnoty kvality a svazujte vydávání na výsledky hodnocení |

---

## Další kroky

- **Rozšiřujte**: Přidejte více testovacích případů a kategorií do svého golden datasetu.
- **Automatizujte**: Integrujte hodnocení do vašeho CI/CD pipeline.
- **Pokročilí soudci**: Použijte větší model jako soudce při testování výstupu menšího modelu.
- **Sledujte v čase**: Ukládejte výsledky hodnocení pro porovnání napříč verzemi promptů a modelů.

---

## Další lab

Pokračujte do [Části 9: Přepis hlasu pomocí Whisper](part9-whisper-voice-transcription.md) a prozkoumejte převod řeči na text lokálně pomocí Foundry Local SDK.