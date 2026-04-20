# భాగం 7 కోసం నమూనా ఆడియో ఫైళ్ల - విస్పర్ వాయిస్ ట్రాన్స్క్రిప్షన్

ఈ WAV ఫైళ్లు `pyttsx3` (విండోస్ SAPI5 టెక్స్ట్-టు-స్పీచ్) ఉపయోగించి రూపొందించబడ్డవే మరియు క్రియేటివ్ రైటర్ డెమో నుండి **Zava DIY ఉత్పత్తులు** సారాంశంగా ఉంటాయి.

## నమూనాలను సృష్టించండి

```bash
# రెపో రూట్ నుండి - .venv అవసరం, అందులో pyttsx3 ఇన్స్టాల్ చేయబడింది
.venv\Scripts\Activate.ps1          # విండోస్
python samples/audio/generate_samples.py
```

## నమూనా ఫైళ్లు

| ఫైల్ | నేపథ్యం | వ్యవధి |
|------|----------|----------|
| `zava-customer-inquiry.wav` | **Zava ProGrip Cordless Drill** - టార్క్, బ్యాటరీ లైఫ్, క్యారింగ్ కేసు గురించి కస్టమర్ అడుగుతున్నారు | ~15 సెకన్లు |
| `zava-product-review.wav` | **Zava UltraSmooth Interior Paint** - కవరేజ్, డ్రయింగ్ టైం, తక్కువ VOC గురించి కస్టమర్ సమీక్షిస్తున్నారు | ~22 సెకన్లు |
| `zava-support-call.wav` | **Zava TitanLock Tool Chest** - రీప్లేస్_KEYలు, అదనపు డ్రవర్ లైనర్లు గురించి సపోర్ట్ కాల్ | ~20 సెకన్లు |
| `zava-project-planning.wav` | DIYer **Zava EcoBoard Composite Decking** & BrightBeam లైట్స్ తో బ్యాక్యార్డ్ డెక్ ప్లాన్ చేస్తున్నారు | ~25 సెకన్లు |
| `zava-workshop-setup.wav` | **అన్ని ఐదు Zava ఉత్పత్తులతో** పూర్తి వర్క్‌షాప్ వాక్‌త్రూ | ~32 సెకన్లు |
| `zava-full-project-walkthrough.wav` | **అన్ని Zava ఉత్పత్తులతో** విస్తృత గ్యారేజ్ రీనోవేషన్ వాక్‌త్రూ (దీర్ఘ-ఆడియో టెస్టింగ్ కోసం, [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517) చూడండి) | ~4 నిమిషాలు |

## గమనికలు

- WAV ఫైళ్లు రీపోలో **కమిట్** చేయబడ్డాయి (. కొత్త .wav ఫైళ్లను సృష్టించడానికి పై స్క్రిప్ట్‌ను నడిపించి కొత్త స్క్రిప్ట్‌లను పునఃసృష్టించండి లేదా సవరించండి.
- స్క్రిప్ట్ స్పష్టమైన ట్రాన్స్క్రిప్షన్ ఫలితాల కోసం 160 WPM వద్ద **Microsoft David** (US ఇంగ్లీష్) వాయిస్ ఉపయోగిస్తుంది.
- అన్ని కథనాలు [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) నుండి ఉత్పత్తులను సూచిస్తాయి.