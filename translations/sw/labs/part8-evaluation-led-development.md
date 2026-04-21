![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Sehemu ya 8: Maendelezaji Yanayozingatia Tathmini na Foundry Local

> **Lengo:** Jenga mfumo wa tathmini unaojaribu na kutoa alama kwa mawakala wako wa AI kwa mfumo ulioratibiwa, ukitumia mfano huo huo wa ndani kama wakala anayejaribiwa na kama hakimu, ili uweze kurekebisha maelekezo kwa ujasiri kabla ya kusafirisha.

## Kwa Nini Maendeleo Yanayozingatia Tathmini?

Unapojenga mawakala wa AI, "inaonekana sawa" si ya kutosha. **Maendeleo yanayozingatia tathmini** hutilia maanani matokeo ya wakala kama vile ni msimbo: unaandika majaribio kwanza, hupima ubora, na hufanya usafirishaji pale alama zitakapofikia kikomo.

Katika Mwandishi wa Ubunifu wa Zava (Sehemu 7), **wakala Mhariri** awali hufanya kazi kama mtathmini mdogo; huamua KUBALI au KUBORESHA. Maabara hii inafanya mtindo huo kuwa mfumo wa tathmini unaoweza kurudiwa ambao unaweza kutumia kwa wakala au mchakato wowote.

| Tatizo | Suluhisho |
|---------|----------|
| Mabadiliko ya maelekezo yanavuruga ubora kimya kimya | **Muhtasari wa dhahabu** unakamata mabadiliko yaliyopungua |
| Upendeleo wa "inafanya kazi kwa mfano mmoja" | **Mifano mingi ya majaribio** huonyesha matukio magumu |
| Tathmini ya ubora unaotegemea maoni | **Alama kwa kutumia kanuni + LLM kama hakimu** hutoa nambari |
| Hakuna njia ya kulinganisha aina za maelekezo | **Kuendesha tathmini sambamba** kwa alama jumla |

---

## Dhahania Muhimu

### 1. Kundi la Dhahabu

**Kundi la dhahabu** ni seti iliyochaguliwa ya kesi za mtihani zenye matokeo yanayotarajiwa yanayojulikana. Kila kesi ya mtihani ina:

- **Ingizo**: Maelekezo au swali la kumtumia wakala
- **Matokeo yanayotarajiwa**: Majibu sahihi au ya ubora wa juu yanayotakiwa kuwa nayo (maneno muhimu, muundo, ukweli)
- **Kundi**: Ugawaji kwa ajili ya ripoti (mfano "usahihi wa kiuhalisia", "lugha", "ukuridhifu kamili")

### 2. Ukaguzi wa Kwa Kanuni

Ukaguzi wa haraka, unaoamuliwa, usiohitaji LLM:

| Ukaguzi | Kinasisitiza nini |
|-------|--------------|
| **Mipaka ya urefu** | Jibu halifupi sana (kupuuza) wala halizidi (kulizunguka) |
| **Maneno muhimu yanayohitajika** | Jibu linataja maneno au vitu vilivyotarajiwa |
| **Thibitisho la muundo** | JSON inaweza kusomeka, vichwa vya Markdown vipo |
| **Maudhui yaliyoruhusiwa** | Hakuna majina ya bidhaa yaliyobuniwa, hakuna kutajwa washindani |

### 3. LLM Kama Hakimu

Tumia **mfano wa ndani ule ule** kupima matokeo yake (au matokeo ya aina tofauti ya maelekezo). Hakimu hupokea:

- Swali la awali
- Jibu la wakala
- Vigezo vya kupima

Na hurudisha alama iliyo na muundo. Hii ni kama mtindo wa Mhariri kutoka Sehemu 7 lakini ikiendeshwa kwa mfumo wa mtihani kwa kupanga.

### 4. Mzunguko wa Marekebisho unaoongozwa na Tathmini

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Mahitaji

| Hitaji | Maelezo |
|-------------|---------|
| **CLI ya Foundry Local** | Imesakinishwa na mfano umepakuliwa |
| **Mazingira ya lugha** | **Python 3.9+** na/au **Node.js 18+** na/au **.NET 9+ SDK** |
| **Imekamilika** | [Sehemu 5: Wakala Peke Yake](part5-single-agents.md) na [Sehemu 6: Mchakato wa Wakala Wengi](part6-multi-agent-workflows.md) |

---

## Mazoezi ya Maabara

### Zoefisho 1 - Endesha Mfumo wa Tathmini

Warsha hii ina mfano kamili wa tathmini unaojaribu wakala wa Foundry Local dhidi ya kundi la dhahabu la maswali yanayohusiana na Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Kuweka Maandalizi:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Endesha:**
```bash
python foundry-local-eval.py
```

**Kinachotokea:**
1. Inajiunga na Foundry Local na kupakia mfano
2. Inaainisha kundi la dhahabu lenye kesi 5 za majaribio (maswali ya bidhaa za Zava)
3. Inaendesha aina mbili za maelekezo dhidi ya kesi zote za majaribio
4. Inatoa alama kwa kila jibu kwa kutumia **ukaguzi wa kanuni** (urefu, maneno muhimu, muundo)
5. Inatoa alama kwa kila jibu kwa kutumia **LLM kama hakimu** (mfano ule ule unatoa alama ubora 1-5)
6. Inachapisha karatasi ya alama ikilinganisha aina zote mbili za maelekezo

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Kuweka Maandalizi:**
```bash
cd javascript
npm install
```

**Endesha:**
```bash
node foundry-local-eval.mjs
```

**Mchakato wa tathmini ule ule:** kundi la dhahabu, kuendesha maelekezo mara mbili, alama kwa kanuni + LLM, karatasi ya alama.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Kuweka Maandalizi:**
```bash
cd csharp
dotnet restore
```

**Endesha:**
```bash
dotnet run eval
```

**Mchakato wa tathmini ule ule:** kundi la dhahabu, kuendesha maelekezo mara mbili, alama kwa kanuni + LLM, karatasi ya alama.

</details>

---

### Zoefisho 2 - Elewa Kundi la Dhahabu

Chunguza kesi za majaribio zilizoainishwa kwenye mfano wa tathmini. Kila kesi ya mtihani ina:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Maswali ya kuzingatia:**
1. Kwa nini thamani zinatarajiwa ni **maneno muhimu** badala ya sentensi kamili?
2. Unahitaji kesi ngapi za majaribio kwa tathmini ya kuaminika?
3. Ni makundi gani ungeongeza kwa matumizi yako binafsi?

---

### Zoefisho 3 - Elewa Tofauti ya Alama za Kanuni vs LLM Kama Hakimu

Mfumo wa tathmini hutumia **tabaka mbili za alama**:

#### Ukaguzi wa Kanuni (haraka, unaoamuliwa)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM kama Hakimu (zaidi ubora, wa maana)

Mfano huo huo wa ndani hufanya kama hakimu kwa kutumia rubiki yenye muundo:

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

**Maswali ya kuzingatia:**
1. Ni lini ungeamini ukaguzi wa kanuni zaidi kuliko LLM kama hakimu?
2. Je, mfano unaweza kuhakiki matokeo yake kwa uhakika? Ni vikwazo gani?
3. Ni vipi hii inalinganishwa na mtindo wa mhariri kutoka Sehemu 7?

---

### Zoefisho 4 - Linganisha Aina za Maelekezo

Mfano unaendesha **aina mbili za maelekezo** dhidi ya kesi mbalimbali za majaribio:

| Aina | Mtindo wa Maelekezo wa Mfumo |
|---------|-------------------|
| **Asili** | Kawaida: "Wewe ni msaidizi mwema" |
| **Mtaalamu** | Maelezo: "Wewe ni mtaalamu wa Zava DIY anayependekeza bidhaa maalum na kutoa mwongozo hatua kwa hatua" |

Baada ya kuendesha, utaona karatasi ya alama kama hii:

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

**Mazoezi:**
1. Endesha tathmini na kumbuka alama za kila aina
2. Badilisha maelekezo maalum zaidi. Alama inaongezeka?
3. Ongeza aina ya tatu ya maelekezo na linganisha zote tatu.
4. Jaribu kubadilisha jina la mfano (mfano `phi-4-mini` dhidi ya `phi-3.5-mini`) na linganisha matokeo.

---

### Zoefisho 5 - Tumia Tathmini kwa Wakala Wako

Tumia mfumo wa tathmini kama mfano kwa mawakala wako:

1. **Eleza kundi lako la dhahabu**: andika kesi 5 hadi 10 za majaribio zilizo na maneno muhimu yanayotarajiwa.
2. **Andika maelekezo ya mfumo**: maelekezo ya wakala unayotaka kujaribu.
3. **Endesha tathmini**: pata alama za msingi.
4. **Rudia**: rekebisha maelekezo, endesha tena, linganisha.
5. **Weka kizingiti**: mfano "usisafirisha chini ya alama 0.75 ya jumla".

---

### Zoefisho 6 - Unganishi na Mtindo wa Mhariri wa Zava

Rudi nyuma kwa wakala Mhariri wa Mwandishi wa Ubunifu wa Zava (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Mhariri ni LLM-kama-mhukumu katika uzalishaji:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Hii ni **dhahania ile ile** kama LLM kama hakimu ya Sehemu 8, lakini imejumuishwa kwenye mchakato wa uzalishaji badala ya mfumo wa majaribio nje ya mtandao. Mitindo yote miwili:

- Hutumia toleo la JSON lenye muundo kutoka kwa mfano.
- Hutumia vigezo vya ubora vilivyoainishwa kwenye maelekezo ya mfumo.
- Hutoa uamuzi wa kupitisha/kushindwa.

**Tofauti:** Mhariri hufanya kazi uzalishaji (kila ombi). Mfumo wa tathmini hufanya kazi maendeleo (kabla ya usafirishaji).

---

## Muhimu wa Kujifunza

| Dhahania | Muhimu |
|---------|----------|
| **Kundi la dhahabu** | Chagua kesi za mtihani mapema; ni majaribio ya mabadiliko ya AI |
| **Ukaguzi wa kanuni** | Haraka, unaoamuliwa, huchota makosa dhahiri (urefu, maneno muhimu, muundo) |
| **LLM kama hakimu** | Alama ya ubora yenye maana kwa kutumia mfano wa ndani ule ule |
| **Kulinganisha maelekezo** | Endesha aina nyingi ya maelekezo dhidi ya seti moja kuchagua bora |
| **Faida ya kifaa** | Tathmini zote zinafanyika ndani: hakuna gharama za API, hakuna mipaka ya kasi, hakuna data kuondoka kwa kifaa chako |
| **Tathmini kabla ya usafirishaji** | Weka kizingiti cha ubora na thibitisha uzinduzi kwa alama za tathmini |

---

## Hatua Zifuatazo

- **Ongeza wingi**: Ongeza kesi za mtihani na makundi katika kundi lako la dhahabu.
- **Jumlisha**: Jumuisha tathmini ndani ya mchakato wako wa CI/CD.
- **Hakimu wa hali ya juu**: Tumia mfano mkubwa kama hakimu wakati ukijaribu matokeo ya mfano mdogo.
- **Fuatilia kwa muda**: Hifadhi matokeo ya tathmini ili kulinganisha kati ya toleo la maelekezo na mfano.

---

## Maabara Inayofuata

Endelea na [Sehemu 9: Ubadilishaji sauti kuwa maandishi kwa Whisper](part9-whisper-voice-transcription.md) kuchunguza sauti-kuwa-maandishi moja kwa moja kwa kutumia Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Kiamshi**:  
Hati hii imetafsiriwa kwa kutumia huduma ya utafsiri wa AI [Co-op Translator](https://github.com/Azure/co-op-translator). Ingawa tunajitahidi kuwa sahihi, tafadhali fahamu kuwa tafsiri za kiotomatiki zinaweza kuwa na makosa au kutokubaliana. Hati ya asili katika lugha yake ya asili inapaswa kuzingatiwa kama chanzo cha mamlaka. Kwa taarifa muhimu, inapendekezwa kutumia utafsiri wa kitaalamu wa binadamu. Hatuhusiki kwa kutoelewana au tafsiri potofu zinazotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->