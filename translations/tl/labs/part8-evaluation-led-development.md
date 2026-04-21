![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagi 8: Development na Pinangunahan ng Pagsusuri gamit ang Foundry Local

> **Layunin:** Bumuo ng isang balangkas ng pagsusuri na sistematikong sumusubok at nagbibigay ng iskor sa iyong mga AI agent, gamit ang parehong lokal na modelo bilang parehong ahente na sinusubukan at hukom, upang makapag-iterate ka sa mga prompt nang may kumpiyansa bago ito ilabas.

## Bakit Development na Pinangunahan ng Pagsusuri?

Kapag bumubuo ng mga AI agent, ang "mukhang tama na" ay hindi sapat. **Development na pinangunahan ng pagsusuri** ay tinatrato ang mga output ng ahente tulad ng code: sumusulat ka muna ng mga pagsusuri, sinusukat ang kalidad, at nagpapadala lamang kapag umabot sa inaasahang iskor.

Sa Zava Creative Writer (Bahagi 7), ang **Editor agent** ay gumaganap na bilang isang magaan na tagasuri; nagdedesisyon ito ng TANGGAPIN o BAGUHIN. Ang laboratoryong ito ay pormal na ginagawa ang pattern na iyon bilang isang paulit-ulit na balangkas ng pagsusuri na maaari mong ilapat sa anumang ahente o pipeline.

| Problema | Solusyon |
|---------|----------|
| Tahimik na pagbago ng prompt na sumisira sa kalidad | Nahuhuli ng **golden dataset** ang mga regression |
| Bias dahil "gumagana lang sa isang halimbawa" | Nagpapakita ng mga edge case ang **maramihang pagsusulit** |
| Paksa ng panseguridad sa kalidad | Nagbibigay ng numero ang **pagsusuri gamit ang panuntunan + LLM bilang hukom** |
| Walang paraan para ihambing ang mga variant ng prompt | **Pagsusuri ng magkaharap na pagsusuri** na may pinagsamang iskor |

---

## Mga Pangunahing Konsepto

### 1. Golden Datasets

Ang **golden dataset** ay isang maingat na pinili na set ng mga kaso ng pagsusulit na may kilalang inaasahang output. Bawat kaso ng pagsusulit ay naglalaman ng:

- **Input**: Ang prompt o tanong na ipapadala sa ahente
- **Inaasahang output**: Ano ang dapat lamanin ng tamang o mataas na kalidad na sagot (mga keyword, istraktura, mga katotohanan)
- **Kategorya**: Grupo para sa pag-uulat (hal. "katumpakan ng katotohanan", "tono", "kompletong nilalaman")

### 2. Mga Pagsusuri Batay sa Panuntunan

Mabilis, deterministic na mga pagsusuri na hindi nangangailangan ng LLM:

| Pagsusuri | Ano ang Sinusubok |
|-------|--------------|
| **Mga hangganan ng haba** | Hindi masyadong maikli (tamad) o masyadong mahaba (paligoy-ligoy) ang sagot |
| **Mga kinakailangang keyword** | Binabanggit sa sagot ang inaasahang mga termino o entidad |
| **Pag-validate ng format** | Nai-parse ang JSON, may mga header ng Markdown |
| **Ipinagbabawal na nilalaman** | Walang hinallucinate na pangalan ng tatak, walang nabanggit na kakumpitensya |

### 3. LLM bilang Hukom

Gamitin ang **parehong lokal na modelo** para bigyan ng grado ang sarili nitong mga output (o output mula sa ibang variant ng prompt). Tinatanggap ng hukom ang:

- Ang orihinal na tanong
- Ang sagot ng ahente
- Mga pamantayan sa paggrado

At nagbabalik ng istrukturadong iskor. Ito ay sumasalamin sa pattern ng Editor mula sa Bahagi 7 ngunit sistematikong inilalapat sa buong test suite.

### 4. Loop ng Iteration na Pinapatakbo ng Pagsusuri

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Mga Kinakailangan

| Pangangailangan | Detalye |
|-------------|---------|
| **Foundry Local CLI** | Nainstall kasama ang na-download na modelo |
| **Runtime ng wika** | **Python 3.9+** at/o **Node.js 18+** at/o **.NET 9+ SDK** |
| **Naitapos na** | [Bahagi 5: Mga Single Agents](part5-single-agents.md) at [Bahagi 6: Multi-Agent Workflows](part6-multi-agent-workflows.md) |

---

## Mga Ehersisyo sa Lab

### Ehersisyo 1 - Patakbuhin ang Balangkas ng Pagsusuri

Kasama sa workshop ang isang kumpletong halimbawa ng pagsusuri na sumusubok sa Foundry Local agent laban sa isang golden dataset ng mga tanong na may kaugnayan sa Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Setup:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Patakbuhin:**
```bash
python foundry-local-eval.py
```

**Ano ang nangyayari:**
1. Kumokonekta sa Foundry Local at naglo-load ng modelo
2. Nagtatakda ng golden dataset na may 5 kaso ng pagsusulit (mga tanong sa produkto ng Zava)
3. Pinapatakbo ang dalawang variant ng prompt laban sa bawat kaso ng pagsusulit
4. Binibigyan ng iskor bawat tugon gamit ang **rule-based checks** (haba, keyword, format)
5. Binibigyan ng iskor bawat tugon gamit ang **LLM-as-judge** (ginagawaran ng parehong modelo ang kalidad mula 1-5)
6. Ipinapakita ang scorecard na naghahambing ng dalawang variant ng prompt

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Setup:**
```bash
cd javascript
npm install
```

**Patakbuhin:**
```bash
node foundry-local-eval.mjs
```

**Parehong pipeline ng pagsusuri:** golden dataset, dobleng pagpapatakbo ng prompt, rule-based + LLM na pag-iskor, scorecard.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Setup:**
```bash
cd csharp
dotnet restore
```

**Patakbuhin:**
```bash
dotnet run eval
```

**Parehong pipeline ng pagsusuri:** golden dataset, dobleng pagpapatakbo ng prompt, rule-based + LLM na pag-iskor, scorecard.

</details>

---

### Ehersisyo 2 - Unawain ang Golden Dataset

Suriin ang mga kaso ng pagsusulit na tinukoy sa halimbawa ng pagsusuri. Bawat kaso ng pagsusulit ay may:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Mga tanong upang pag-isipan:**
1. Bakit mga **keyword** ang inaasahang halaga sa halip na buong pangungusap?
2. Gaano karaming mga kaso ng pagsusulit ang kailangan para sa maaasahang pagsusuri?
3. Anong mga kategorya ang idaragdag mo para sa sarili mong aplikasyon?

---

### Ehersisyo 3 - Unawain ang Rule-Based vs LLM-as-Judge na Pag-iskor

Gumagamit ang balangkas ng pagsusuri ng **dalawang layer ng pag-iskor**:

#### Rule-Based Checks (mabilis, deterministic)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-as-Judge (mas malalim, kwalitatibo)

Ang parehong lokal na modelo ay gumaganap bilang hukom gamit ang isang istrukturadong rubric:

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

**Mga tanong upang pag-isipan:**
1. Kailan ka magtitiwala sa rule-based checks kaysa LLM-as-judge?
2. Maaari ba ng isang modelo na husgahan nang maaasahan ang sarili nitong output? Ano ang mga limitasyon?
3. Paano ito ikinukumpara sa Editor agent pattern mula sa Bahagi 7?

---

### Ehersisyo 4 - Ihambing ang Mga Variant ng Prompt

Pinapatakbo ng halimbawa ang **dalawang variant ng prompt** laban sa parehong mga kaso ng pagsusulit:

| Variant | Estilo ng System Prompt |
|---------|-------------------|
| **Baseline** | Pangkalahatan: "Ikaw ay isang matulunging katulong" |
| **Spesyal na** | Detalyado: "Ikaw ay isang eksperto sa Zava DIY na nagrerekomenda ng mga partikular na produkto at nagbibigay ng hakbang-hakbang na gabay" |

Pagkatapos patakbuhin, makikita mo ang scorecard na tulad nito:

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

**Mga Ehersisyo:**
1. Patakbuhin ang pagsusuri at tandaan ang mga iskor para sa bawat variant
2. Baguhin ang espesyal na prompt upang maging mas tiyak pa. Tumaas ba ang iskor?
3. Magdagdag ng pangatlong variant ng prompt at paghambingin ang lahat ng tatlo.
4. Subukan baguhin ang model alias (hal. `phi-4-mini` vs `phi-3.5-mini`) at paghambingin ang mga resulta.

---

### Ehersisyo 5 - Ilapat ang Pagsusuri sa Sariling Ahente

Gamitin ang balangkas ng pagsusuri bilang template para sa sarili mong mga ahente:

1. **Tukuyin ang iyong golden dataset**: magsulat ng 5 hanggang 10 kaso ng pagsusulit na may inaasahang mga keyword.
2. **Isulat ang iyong system prompt**: ang mga tagubilin sa ahente na nais mong subukan.
3. **Patakbuhin ang pagsusuri**: kumuha ng baseline na mga iskor.
4. **Mag-iterate**: ayusin ang prompt, patakbuhin muli, at paghambingin.
5. **Magtakda ng threshold**: hal. "huwag magpadala kung mababa sa 0.75 ang pinagsamang iskor".

---

### Ehersisyo 6 - Koneksyon sa Editor Pattern ng Zava

Balikan ang Editor agent ng Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Ang editor ay isang LLM-bilang-hukom sa produksyon:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Ito ay ang **kaparehong konsepto** ng LLM-as-judge ng Bahagi 8, ngunit naka-embed sa production pipeline sa halip na offline test suite. Parehong mga pattern:

- Gumagamit ng istrukturadong JSON output mula sa modelo.
- Nag-aaplay ng mga pamantayan sa kalidad na tinukoy sa system prompt.
- Nagsasagawa ng pass/fail na desisyon.

**Ang pagkakaiba:** Ang editor ay tumatakbo sa production (sa bawat kahilingan). Ang balangkas ng pagsusuri ay tumatakbo sa development (bago ka magpadala).

---

## Mga Mahahalagang Aral

| Konsepto | Aral |
|---------|----------|
| **Golden datasets** | Maagang mag-curate ng mga kaso ng pagsusulit; sila ang regression tests para sa AI |
| **Rule-based checks** | Mabilis, deterministic, at nahuhuli ang halatang pagkabigo (haba, mga keyword, format) |
| **LLM-as-judge** | Malalim na pag-iskor ng kalidad gamit ang parehong lokal na modelo |
| **Paghahambing ng prompt** | Patakbuhin ang maraming variant laban sa parehong test suite upang piliin ang pinakamahusay |
| **Bentahe sa device** | Lahat ng pagsusuri ay tumatakbo nang lokal: walang gastos sa API, walang limitasyon sa rate, walang data na lumalabas sa iyong makina |
| **Pagsusuri bago ipadala** | Magtakda ng mga threshold sa kalidad at kontrolin ang paglabas gamit ang mga iskor ng pagsusuri |

---

## Mga Susunod na Hakbang

- **Palawakin**: Magdagdag pa ng mga kaso ng pagsusulit at kategorya sa iyong golden dataset.
- **Automate**: Isama ang pagsusuri sa iyong CI/CD pipeline.
- **Mas advanced na mga hukom**: Gumamit ng mas malaking modelo bilang hukom habang sinusubukan ang output ng mas maliit na modelo.
- **Subaybayan sa paglipas ng panahon**: Itago ang mga resulta ng pagsusuri para maikumpara sa pagitan ng mga bersyon ng prompt at modelo.

---

## Susunod na Lab

Magpatuloy sa [Bahagi 9: Pag-transcribe ng Boses gamit ang Whisper](part9-whisper-voice-transcription.md) upang tuklasin ang speech-to-text on-device gamit ang Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:
Ang dokumentong ito ay isinalin gamit ang AI translation service na [Co-op Translator](https://github.com/Azure/co-op-translator). Bagamat nagsusumikap kami para sa katumpakan, pakitandaan na ang mga awtomatikong pagsasalin ay maaaring maglaman ng mga pagkakamali o kamalian. Ang orihinal na dokumento sa orihinal nitong wika ang dapat ituring na pangunahing pinagkukunan. Para sa mahahalagang impormasyon, inirerekomenda ang propesyonal na pagsasaling tao. Hindi kami mananagot sa anumang hindi pagkakaunawaan o maling interpretasyon na nagmumula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->