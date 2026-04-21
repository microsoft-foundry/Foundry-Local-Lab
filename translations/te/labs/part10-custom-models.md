![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# భాగం 10: Foundry Local తో కస్టమ్ లేదా Hugging Face మోడల్స్ ఉపయోగించడం

> **లక్ష్యం:** Foundry Local కోరుకునే ఆప్టిమైజ్ చేసిన ONNX ఫార్మాట్‌లో Hugging Face మోడల్‌ను కంపైల్ చేసి, దానిని చాట్ టెంప్లేట్‌తో కాన్ఫిగర్ చేసి, స్థానిక క్యాష్‌లో చేర్చండి, CLI, REST API, మరియు OpenAI SDK ఉపయోగించి దాని మీద ఇన్ఫరెన్స్ నడపండి.

## అవలోకనం

Foundry Local ముందుగా కంపైల్ చేసిన మోడల్స్ సంకలనం తో వస్తుంది, కానీ మీరు ఆ జాబితా పరిమితం కావలసిన అవసరం లేదు. [Hugging Face](https://huggingface.co/) (లేదా PyTorch / Safetensors ఫార్మాట్‌లో స్థానికంగా భద్రపరచబడిన) లో లభ్యమయ్యే ఏదైనా ట్రాన్స్‌ఫార్మర్-ఆధారిత భాషా మోడల్ ఆప్టిమైజ్ చేసిన ONNX మోడల్‌గా కంపైల్ చేసి Foundry Local ద్వారా సర్వ్ చేయవచ్చు.

కంపైలేషన్ పైప్‌లైన్ ONNX Runtime GenAI Model Builder అనే కమాండ్-లైన్ టూల్‌ను ఉపయోగిస్తుంది, ఇది `onnxruntime-genai` ప్యాకేజీలో ఉంది. ఈ మోడల్ బిల్డర్ మూల వేట్స్ డౌన్లోడ్ చేయడం, వాటిని ONNX ఫార్మాట్‌లోకి మార్చడం, క్వాంటైజేషన్ (int4, fp16, bf16) వర్తించడం, మరియు Foundry Local కోరుకునే కాన్ఫిగరేషన్ ఫైళ్ళను (చాట్ టెంప్లేట్, టోకనైజర్ సహా) ఉత్పత్తి చేస్తుంది.

ఈ లోగడ మీరు Hugging Face నుండి **Qwen/Qwen3-0.6B** ను కంపైల్ చేసి, దాన్ని Foundry Localలో నమోదు చేసి, అంతా మీ సరికొత్త పరికరంలోనే చాట్ చేయవచ్చు.

---

## అభ్యాస లక్ష్యాలు

ఈ లాబ్ ముగింపు వరకు మీరు చేయగలుగుతారు:

- కస్టమ్ మోడల్ కంపైలేషన్ ఎందుకు అవసరం మరియు ఎప్పుడు అవసరం అవుతుంది అవగాహన పొందండి
- ONNX Runtime GenAI మోడల్ బిల్డర్ ఇన్‌స్టాల్ చేయండి
- ఒక Hugging Face మోడల్‌ను ఒకే కమాండ్‌తో ఆప్టిమైజ్ చేసిన ONNX ఫార్మాట్‌లో కంపైల్ చేయండి
- ప్రధాన కంపైలేషన్ పరిమాణాలు (ఎగ్జిక్యూషన్ ప్రొవైడర్, ప్రిసిషన్) అర్థం చేసుకోండి
- `inference_model.json` చాట్-టెంప్లేట్ కాన్ఫిగరేషన్ ఫైల్ సృష్టించండి
- కంపైల్ చేసిన మోడల్‌ను Foundry Local క్యాష్‌లో చేర్చండి
- CLI, REST API, మరియు OpenAI SDK ఉపయోగించి కస్టమ్ మోడల్‌పై ఇన్ఫరెన్స్ నడపండి

---

## ముందస్తు అవసరాలు

| అవసరం | వివరాలు |
|-------------|---------|
| **Foundry Local CLI** | ఇన్‌స్టాల్ చేయబడినది మరియు మీ `PATH` లో ఉంటుంది ([Part 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI మోడల్ బిల్డర్ కోసం అవసరం |
| **pip** | Python ప్యాకేజీ మేనేజర్ |
| **డిస్క్ స్థలం** | మూల మరియు కంపైల్ చేసిన మోడల్ ఫైళ్ల కోసం కనీసం 5 GB ఖాళీ స్థలం |
| **Hugging Face ఖాతా** | కొన్నికొన్ని మోడల్స్ డౌన్లోడ్ ముందు లైసెన్స్ అంగీకరించాలని కోరుకుంటాయి. Qwen3-0.6B Apache 2.0 లైసెన్స్ తో ఉంది మరియు ఉచితం. |

---

## వాతావరణ సెటప్

మోడల్ కంపైలేషన్ కోసం పెద్ద Python ప్యాకేజీలు (PyTorch, ONNX Runtime GenAI, Transformers) అవసరం. వీటితో మీ సిస్టమ్ Python లేదా ఇతర ప్రాజెక్ట్స్‌లకు అంతరాయం కలగకుండా వర్చువల్ ఎన్విరాన్‌మెంట్ సృష్టించండి.

```bash
# నిల్వ మూలం నుండి
python -m venv .venv
```
  
ఎన్విరాన్‌మెంట్‌ని యాక్టివేట్ చేయండి:

**Windows (PowerShell):**  
```powershell
.venv\Scripts\Activate.ps1
```
  
**macOS / Linux:**  
```bash
source .venv/bin/activate
```
  
డిపెండెన్సీ పరిష్కారం సమస్యలు నివారించడానికి pipని అప్‌గ్రేడ్ చేయండి:

```bash
python -m pip install --upgrade pip
```
  
> **సూక్తి:** మీరు ముందే ఉన్న `.venv`ని ఈ లాబ్‌లలో నుండి సైతం ఉపయోగించవచ్చు. పకకం, కొనసాగడం కంటే ముందుగా అది యాక్టివేట్ అయి ఉన్నట్లు చూసుకోండి.

---

## కాన్సెప్ట్: కంపైల్ పైప్‌లైన్

Foundry Local ONNX ఫార్మాట్‌లో మరియు ONNX Runtime GenAI కాన్ఫిగరేషన్‌తో మోడల్స్ కోరుకుంటుంది. ఎక్కువ భాగం Hugging Face లోని ఓపెన్-సోర్స్ మోడల్స్ PyTorch లేదా Safetensors వేట్స్ రూపంలో వస్తాయి, కాబట్టి కన్వర్షన్ స్టెప్ అవసరం.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### మోడల్ బిల్డర్ ఏమి చేస్తుంది?

1. Hugging Face నుండి మూల మోడల్ డౌన్లోడ్ చేస్తుంది (లేదా స్థానిక మార్గం నుండి చదువుతుంది).  
2. PyTorch / Safetensors వేట్స్‌ను ONNX ఫార్మాట్‌లోకి మార్చుతుంది.  
3. మెమరీ వినియోగం తగ్గించడానికి మరియు అంశం వేగం పెంచడానికి మోడల్‌ను చిన్న ప్రిసిషన్ (ఉదా: int4)కి క్వాంటైజ్ చేస్తుంది.  
4. ONNX Runtime GenAI కాన్ఫిగరేషన్ (`genai_config.json`), చాట్ టెంప్లేట్ (`chat_template.jinja`), మరియు అన్ని టోకనైజర్ ఫైళ్ళను ఉత్పత్తి చేస్తుంది, తద్వారా Foundry Local మోడల్‌ను లోడ్ చేసి సర్వ్ చేయగలదు.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

మోడల్ ఆప్టిమైజేషన్ కి మీరు **Microsoft Olive** పేరుతో మరో టూల్ గురించి వినవచ్చు. వీరిని వాటి ప్రయోజనాలు మరియు వైపు మార్పులతో పోల్చండి:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **ప్యాకేజీ** | `onnxruntime-genai` | `olive-ai` |
| **ప్రధాన ఉద్దేశ్యం** | ONNX Runtime GenAI ఇన్ఫరెన్స్ కోసం జనరేటివ్ AI మోడల్స్ మార్చడం, క్వాంటైజ్ చేయడం | అనేక బ్యాక్‌ఎండ్లు, హార్డ్‌వేర్ టార్గెట్లకు సరిపోయే ముగింపు-టు-ముగింపు మోడల్ ఆప్టిమైజేషన్ ఫ్రేమ్‌వర్క్ |
| **వినియోగ సౌలభ్యం** | ఒకే కమాండ్ — ఒక దిగువ కన్వర్షన్ + క్వాంటైజేషన్ | వర్క్‌ఫ్లో ఆధారిత — YAML/JSON తో అనేక దశల పైప్లైన్లు |
| **ఫలిత ఫార్మాట్** | ONNX Runtime GenAI ఫార్మాట్ (Foundry Localకి సిద్ధంగా) | సాధారణ ONNX, ONNX Runtime GenAI, లేదా వర్క్‌ఫ్లో ఆధారంగా ఇతర ఫార్మాట్లు |
| **హార్డ్‌వేర్ టార్గెట్లు** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, మరిన్ని |
| **క్వాంటైజేషన్ ఎంపికలు** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, గ్రాఫ్ ఆప్టిమైజేషన్లు, లేయర్-వైజ్ ట్యూనింగ్ |
| **మోడల్ పరిధి** | జనరేటివ్ AI మోడల్స్ (LLMs, SLMs) | ఏదైనా ONNX-మార్పిడి చేయగలిగే మోడల్ (దృశ్యం, NLP, ఆడియో, మల్టీమోడ్) |
| **ఉత్తమమైన ఉపయోగం** | స్థలీయ ఇన్ఫరెన్స్ కోసం ఒకే మోడల్ కంపైలేషన్ | ఖచ్చితమైన ఆప్టిమైజేషన్ నియంత్రణ అవసరమయ్యే ప్రొడక్షన్ పైప్‌లైన్లకు |
| **డిపెండెన్సీ పరిధి** | మధ్యం (PyTorch, Transformers, ONNX Runtime) | పెద్దది (Olive ఫ్రేమ్‌వర్క్, ఎంపికలపై అదనపు ప్యాకేజీలు) |
| **Foundry Local ఇంటిగ్రేషన్** | ప్రత్యక్షంగా — ఫలితాలు ఖచ్చితంగా సరిపోతాయి | `--use_ort_genai` ఫ్లాగ్ మరియు అదనపు కాన్ఫిగరేషన్ అవసరం |

> **ఏమి వలన ఈ లాబ్ మోడల్ బిల్డర్ ఉపయోగిస్తుంది:** ఒక్క Hugging Face మోడల్ కంపైల్ చేసి Foundry Localలో నమోదు చేయడం కోసం, మోడల్ బిల్డర్ తేలికగా మరియు నమ్మదగ్గ మార్గం. ఇది Foundry Local ఆశించే ఖచ్చిత ఫార్మాట్‌ను ఒకే కమాండ్‌లో ఉత్పత్తి చేస్తుంది. మీరు తరువాత ఖచ్చితమైన ఆప్టిమైజేషన్ లక్షణాల (Accuracy-aware quantisation, గ్రాఫ్ శస్త్ర చికిత్స, బహుళ దశల ట్యూనింగ్) అవసరం ఉంటే, Olive ఒక శక్తివంతమైన ఎంపిక. మరిన్ని వివరాలకు [Microsoft Olive డాక్యుమెంటేషన్](https://microsoft.github.io/Olive/) చూడండి.

---

## ప్రయోగశాల వ్యాయామాలు

### వ్యాయామం 1: ONNX Runtime GenAI Model Builder ఇన్‌స్టాల్ చేయండి

ONNX Runtime GenAI ప్యాకేజ్ (మోడల్ బిల్డర్ టూల్‌తో పాటు) ను ఇన్‌స్టాల్ చేయండి:

```bash
pip install onnxruntime-genai
```
  
మోడల్ బిల్డర్ అందుబాటులో ఉందో లేదో చూసేందుకు చెక్ చేయండి:

```bash
python -m onnxruntime_genai.models.builder --help
```
  
`-m` (మోడల్ పేరు), `-o` (ఫలిత మార్గం), `-p` (ప్రిసిషన్), `-e` (ఎగ్జిక్యూషన్ ప్రొవైడర్) వంటి ప్యారామీటర్ల సహా సహాయక అవుట్‌పుట్ కనిపిస్తుందని చూసుకోండి.

> **గమనిక:** మోడల్ బిల్డర్ PyTorch, Transformers, ఇతర ప్యాకేజీలపై ఆధారపడి ఉంటుంది. ఇన్‌స్టాలేషన్ కొంత సమయం తీసుకోవచ్చు.

---

### వ్యాయామం 2: CPU కోసం Qwen3-0.6B కంపైల్ చేయండి

క్రింది కమాండ్ ద్వారా Hugging Face నుండి Qwen3-0.6B మోడల్ డౌన్లోడ్ చేసి int4 క్వాంటైజేషన్‌తో CPU ఇన్ఫరెన్స్ కోసం కంపైల్ చేయండి:

**macOS / Linux:**  
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```
  
**Windows (PowerShell):**  
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```
  
#### ప్రతి ప్యారామీటర్ పనితీరు

| ప్యారామీటర్ | ఉద్దేశ్యం | ఉపయోగించిన విలువ |
|-----------|---------|------------|
| `-m` | Hugging Face మోడల్ ID లేక స్థానిక డైరెక్టరీ మార్గం | `Qwen/Qwen3-0.6B` |
| `-o` | కంపైల్ చేసిన ONNX మోడల్ సేవ్ చేసే డైరెక్టరీ | `models/qwen3` |
| `-p` | కంపైల్ సమయంలో వర్తించే క్వాంటైజేషన్ ప్రిసిషన్ | `int4` |
| `-e` | ONNX Runtime ఎగ్జిక్యూషన్ ప్రొవైడర్ (లక్ష్య హార్డ్‌వేర్) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face ఆటోరైజేషన్ తప్పించు (పబ్లిక్ మోడల్స్‌కి సరిపోతుంది) | `hf_token=false` |

> **ఏరవేళ ఎంత సమయం తీసుకుందో?** కంపైల్ సమయం మీ హార్డ్‌వేర్, మోడల్ పరిమాణంపై ఆధారపడి ఉంటుంది. Qwen3-0.6B యొక్క int4 క్వాంటైజేషన్‌తో ఆధునిక CPU మీద సుమారు 5 నుండి 15 నిమిషాలు పడవచ్చు. పెద్ద మోడల్స్ ఎక్కువ సమయం పడతాయి.

కమాండ్ పూర్తయిన తర్వాత `models/qwen3` డైరెక్టరీలో కంపైల్ చేసిన ఫైళ్లు ఉంటాయని చూడండి. అవుట్‌పుట్ पुष्टि చేయండి:

```bash
ls models/qwen3
```
  
ఫైళ్ళు ఉండాలి:  
- `model.onnx` మరియు `model.onnx.data` — కంపైల్ చేసిన మోడల్ వేట్స్  
- `genai_config.json` — ONNX Runtime GenAI కాన్ఫిగరేషన్  
- `chat_template.jinja` — మోడల్ చాట్ టెంప్లేట్ (ఆటో-జనరేటెడ్)  
- `tokenizer.json`, `tokenizer_config.json` — టోకనైజర్ ఫైళ్లు  
- ఇతర శబ్దకోశ, కాన్ఫిగరేషన్ ఫైళ్లు  

---

### వ్యాయామం 3: GPU కోసం కంపైల్ చేయండి (ఐಚ్ఛికం)

మీ వద్ద CUDA మద్దతు ఉన్న NVIDIA GPU ఉంటే, వేగవంతమైన ఇన్ఫరెన్స్ కోసం GPU-ఆప్టిమైజ్ చేసిన వేరియంట్ కంపైల్ చేయవచ్చు:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```
  
> **గమనిక:** GPU కంపైల్ `onnxruntime-gpu` మరియు CUDA ఇన్‌స్టాలేషన్ అవసరం. లేనట్లయితే ఎర్రర్ వస్తుంది. ఈ వ్యాయామం ఎగువేసి CPU వేరియంట్ తో కొనసాగవచ్చు.

#### హార్డ్‌వేర్ స్పెసిఫిక్ కంపైలేషన్ మార్గనిర్దేశకం

| లక్ష్యం | ఎగ్జిక్యూషన్ ప్రొవైడర్ (`-e`) | సిఫార్సు చేసిన ప్రిసిషన్ (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` లేదా `int4` |
| DirectML (విండోస్ GPU) | `dml` | `fp16` లేదా `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### ప్రిసిషన్ తులనాత్మకాలు

| ప్రిసిషన్ | పరిమాణం | వేగం | నాణ్యత |
|-----------|----------|-------|---------|
| `fp32` | పెద్దది | మెల్లగా | అత్యధిక ఖచ్చితత్వం |
| `fp16` | పెద్దటి | వేగంగా (GPU పై) | చాలా మంచి ఖచ్చితత్వం |
| `int8` | చిన్నది | వేగంగా | కొంచెం ఖచ్చితత్వం తగ్గడం |
| `int4` | అతి చిన్నది | అత్యంత వేగంగా | మోస్తరు ఖచ్చితత్వం తగ్గడం |

బహుళ స్థానిక అభివృద్ధికి, CPU మీద `int4` వేగం మరియు వనరుల వినియోగం గొప్ప సమతుల్యం. ప్రొడక్షన్-గుణాత్మక అవుట్‌పుట్ కోసం CUDA GPUపై `fp16` అనుకూలంగా ఉంటుంది.

---

### వ్యాయామం 4: చాట్ టెంప్లేట్ కాన్ఫిగరేషన్ సృష్టించండి

మోడల్ బిల్డర్ ఆటోమేటిక్‌గా `chat_template.jinja` ఫైల్ మరియు `genai_config.json` ఫైల్‌ను అవుట్‌పుట్ డైరెక్టరీలో ఉత్పత్తి చేస్తుంది. అయినప్పటికీ Foundry Localకి కూడా `inference_model.json` అనే ఫైల్ అవసరం, ఇది ఎలా మోడల్ కోసం ప్రాంప్లెట్ ఫార్మాట్ చేయాలో తెలియజేస్తుంది. ఈ ఫైల్ మోడల్ పేరును మరియు యూజర్ సందేశాలను సరైన ప్రత్యేక టోకెన్ల్లో చుట్టుకొనే ప్రాంప్ట్ టెంప్లేట్‌ను నిర్వచిస్తుంది.

#### 1వ దశ: కంపైల్ చేసిన అవుట్‌పుట్‌ను పరిశీలించండి

కంపైల్ చేసిన మోడల్ డైరెక్టరీలోని ఫైల్స్‌ని చూపించండి:

```bash
ls models/qwen3
```
  
ఫైళ్లు ఉండాలి:  
- `model.onnx` మరియు `model.onnx.data` — కంపైల్ చేసిన మోడల్ వేట్స్  
- `genai_config.json` — ONNX Runtime GenAI కాన్ఫిగరేషన్ (ఆటో-జనరేటెడ్)  
- `chat_template.jinja` — మోడల్ చాట్ టెంప్లేట్ (ఆటో-జనరేటెడ్)  
- `tokenizer.json`, `tokenizer_config.json` — టోకనైజర్ ఫైళ్లు  
- వివిధ ఇతర కాన్ఫిగరేషన్ మరియు శబ్దకోశ ఫైళ్లు  

#### 2వ దశ: inference_model.json ఫైల్ తయారుచేయండి

`inference_model.json` ఫైల్ Foundry Localకి ప్రాంప్ట్లను ఎలా ఫార్మాట్ చేయాలో చెప్పుతుంది. దీనికి ఒక Python స్క్రిప్ట్ `generate_chat_template.py` అని మీ రిపాజిటరీ రూట్‌లో (మీకు `models/` ఫోల్డర్ ఉన్న అదే డైరెక్టరీ) సృష్టించండి:

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# చాట్ టెంప్లేట్‌ను తీసుకోవడానికి కనిష్ట సంభాషణను నిర్మించండి
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# inference_model.json నిర్మాణాన్ని రూపొందించండి
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```
  
రిపాజిటరీ రూట్ నుండి స్క్రిప్ట్ నడిపించండి:

```bash
python generate_chat_template.py
```
  
> **గమనిక:** `transformers` ప్యాకేజ్ ఇప్పటికే `onnxruntime-genai`లో డిపెండెన్సీగా ఇన్‌స్టాల్ అయి ఉంటుంది. `ImportError` వస్తే `pip install transformers` ముందుగా చేయండి.

స్క్రిప్ట్ `models/qwen3` డైరెక్టరీలో `inference_model.json` ఫైల్ ఉత్పత్తి చేస్తుంది. ఇది Foundry Localకి Qwen3 కోసం యూజర్ ఇన్‌పుట్‌ను సరైన ప్రత్యేక టోకెన్లతో ఎలా చుట్టుకోవాలో చెప్తుంది.

> **ముఖ్యమైనది:** `inference_model.json` లో `"Name"` ఫీల్డ్ (ఈ స్క్రిప్ట్‌లో `qwen3-0.6b` గా సెట్ అయింది) మీరు అన్ని తదుపరి కమాండ్లు మరియు API కాల్స్‌లో ఉపయోగించే మోడల్ తాత్కాలిక పేరు. మీరు పేరు మార్చితే, Exercises 6–10లో కూడా అప్‌డేట్ చేయండి.

#### 3వ దశ: కాన్ఫిగరేషన్ ధృవీకరించండి

`models/qwen3/inference_model.json` తెరుచుకుని, అందులో `Name` ఫీల్డ్ మరియు `assistant` మరియు `prompt` గల `PromptTemplate` ఆబ్జెక్టు ఉన్నదో లేదో చూడండి. ప్రాంప్ట్ టెంప్లేట్‌లో `<|im_start|>` మరియు `<|im_end|>` వంటి ప్రత్యేక టోకెన్లు ఉండాలి (ఖచ్చిత టోకన్లు మోడల్ చాట్ టెంప్లేట్‌పై ఆధారపడి ఉంటాయి).

> **మానవీయ ప్రత్యామ్నాయం:** స్క్రిప్ట్ నడపకుండా మీరు మానవీయంగా ఆ ఫైల్ సృష్టించవచ్చు. ముఖ్యమైనది `prompt` ఫీల్డ్‌లో `{Content}` అనే ప్లేస్‌హోల్డర్ ఉండి, మోడల్ పూర్తి చాట్ టెంప్లేట్ ఉండాలి.

---

### వ్యాయామం 5: మోడల్ డైరెక్టరీ నిర్మాణాన్ని ధృవీకరించండి


మోడల్ బిల్డర్ మీరు సూచించిన అవుట్‌పుట్ డైరెక్టరీలో అందరు కంపైల్ ఫైల్స్ నేరుగా ఉంచుతుంది. చివరి నిర్మాణం సరిగా ఉందని నిర్ధారించుకోండి:

```bash
ls models/qwen3
```

డైరెక్టరీ ఈ క్రింది ఫైళ్లను కలిగి ఉండాలి:

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **గమనిక:** కొన్ని ఇతర కంపైలేషన్ టూల్స్‌తో తేడా గానే, మోడల్ బిల్డర్ nested subdirectories సృష్టించదు. అన్ని ఫైళ్లు నేరుగా అవుట్‌పుట్ ఫోల్డర్‌లో ఉంటాయి, ఇది Foundry Local నువ్వు ఆకాంక్షించిన విధంగా ఉంది.

---

### వ్యాయామం 6: మోడల్‌ను Foundry Local క్యాశేలో జోడించండి

మీ కంపైల్ చేయబడిన మోడల్ ఎక్కడ ఉందో Foundry Local కి చెబుతూ ఆ డైరెక్టరీని క్యాశేలో జోడించండి:

```bash
foundry cache cd models/qwen3
```

మోడల్ క్యాశేలో కనిపిస్తుందో లేదో నిర్ధారించుకోండి:

```bash
foundry cache ls
```

మీ కస్టమ్ మోడల్ మీరు ముందు క్యాశ్ చేసిన మోడల్స్ (ఉదా: `phi-3.5-mini` లేదా `phi-4-mini`) తో పాటు జాబితాలో కనిపించాలి.

---

### వ్యాయామం 7: CLI తో కస్టమ్ మోడల్‌ను నడపండి

మీ తాజాగా కంపైల్ చేసిన మోడల్‌తో (`qwen3-0.6b` అలియాస్ `inference_model.json` లో మీరు సెట్ చేసిన `Name` ఫీల్డ్ నుండి వస్తుంది) ఇంటరాక్టివ్ చాట్ సెషన్ ప్రారంభించండి:

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` జెండా అదనపు డయాగ్నొస్టిక్ సమాచారాన్ని చూపుతుంది, ఇది మొదటిసారి కస్టమ్ మోడల్‌ను పరీక్షించే సమయంలో సహాయపడుతుంది. మోడల్ విజయవంతంగా లోడ్ అయితే మీకు ఇంటరాక్టివ్ ప్రాంప్ట్ కనిపిస్తుంది. కొన్ని సందేశాలు ప్రయత్నించండి:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

`exit` టైప్ చేయండి లేదా సెషన్ ముగించడానికి `Ctrl+C` నొక్కండి.

> **త్రుటి పరిష్కారం:** మోడల్ లోడ్ కావడం విఫలమైతే, ఈ క్రింది విషయాలు జాగ్రత్తగా తనిఖీ చేయండి:
> - `genai_config.json` ఫైల్ మోడల్ బిల్డర్ ద్వారా ఉత్పత్తి చేయబడింది.
> - `inference_model.json` ఫైల్ ఉంది మరియు చెల్లుబాటు అయ్యే JSON.
> - ONNX మోడల్ ఫైళ్లు సరైన డైరెక్టరీలో ఉన్నాయి.
> - మీ వద్ద తగినంత RAM ఉంది (Qwen3-0.6B int4 కి సుమారు 1 GB అవసరం).
> - Qwen3 ఒక reasoning మోడల్, ఇది `<think>` ట్యాగ్‌లను ఉత్పత్తి చేస్తుంది. మీరు `<think>...</think>` ట్యాగ్లు జవాబులకు ముందు కనిపిస్తే, ఇది సాధారణ ప్రవర్తన. `inference_model.json`లోని ప్రాంప్ట్ టెంప్లెట్‌ను ఆలోచన అవుట్‌పుట్‌ను దమన చేయేందుకు సర్దుబాటు చేయవచ్చు.

---

### వ్యాయామం 8: REST API ద్వారా కస్టమ్ మోడల్‌ను ప్రశ్నించండి

వీడియట్ సెషన్‌ను Exercise 7లో ముగించి ఉంటే, మోడల్ ఇప్పుడు లోడ్ కాకపోవచ్చు. మొదట Foundry Local సర్వీస్ ప్రారంభించి మోడల్‌ను లోడ్ చేయండి:

```bash
foundry service start
foundry model load qwen3-0.6b
```

సర్వీస్ ఏ పోర్టులో నడుస్తుందో తనిఖీ చేయండి:

```bash
foundry service status
```

తర్వాత ఒక అభ్యర్థన పంపండి (`5273` స్థానంలో మీ నిజమైన పోర్టును భర్తీ చేయండి):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows గమనిక:** పై `curl` కమాండ్ బాష్ సింటాక్స్ ఉపయోగిస్తుంది. Windowsలో PowerShell `Invoke-RestMethod` cmdlet ఉపయోగించండి.

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### వ్యాయామం 9: OpenAI SDKతో కస్టమ్ మోడల్‌ను ఉపయోగించండి

మీ కస్టమ్ మోడల్‌ను, మీరు బిల్ట్-ఇన్ మోడల్స్‌కి ఉపయోగించిన OpenAI SDK కోడ్‌తో backed-connect చేయవచ్చు ([Part 3](part3-sdk-and-apis.md) చూడండి). ఒక్క తేడా మోడల్ పేరు.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local API కీలు సరిచూసుకోవడం లేదు
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local API కీలు যাচింపు చేయదు
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **ముఖ్య విషయం:** Foundry Local ఓపెన్‌ఎఐ-తెలిసిన APIను ఎక్స్‌పోజ్‌ చేస్తుంది; అందువల్ల బిల్ట్-ఇన్ మోడల్స్‌తో పని చేసే ఏ కోడ్ అయినా మీ కస్టమ్ మోడల్స్‌తో కూడా పనిచేస్తుంది. కేవలం `model` ప్యారామీటర్ మార్చడం సరిపోతుంది.

---

### వ్యాయామం 10: Foundry Local SDKతో కస్టమ్ మోడల్ పరీక్షించండి

మునుపటి ప్రయోగాల్లో మీరు Foundry Local SDK ఉపయోగించి సర్వీస్ ప్రారంభించడం, ఎండ్‌పాయింట్ కనుగొనడం మరియు మోడల్స్‌ను ఆటోమాటిక్‌గా నిర్వహించడం చేసారు. కస్టమ్-కంపైల్ మోడల్‌తో కూడా ఇదే విధానాన్ని అనుసరించవచ్చు. SDK సర్వీస్ ప్రారంభకరణ మరియు ఎండ్‌పాయింట్ కనుగొనడం నిర్వహిస్తుంది, కాబట్టి మీ కోడ్ `localhost:5273` అనే పోర్ట్ కఠినంగా ఇవ్వవలసి లేదు.

> **గమనిక:** ఈ ఉదాహరణలు చేయడం ముందు Foundry Local SDK ఇన్‌స్టాల్ చేసినట్టు చూసుకోండి:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` మరియు `OpenAI` NuGet ప్యాకేజిలని యాడ్ చేయండి
>
> ప్రతి స్క్రిప్ట్ ఫైల్ **రిపాజిటరీ రూట్‌లో** సేవ్ చేయండి (మీ `models/` ఫోల్డర్ వెనుకట).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# దశ 1: Foundry Local సేవ ప్రారంభించి కస్టమ్ మోడల్‌ను లోడ్ చేయండి
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# దశ 2: కస్టమ్ మోడల్ కోసం కేశ్‌ను తనిఖీ చేయండి
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# దశ 3: మోడల్‌ను మెమరీలో లోడ్ చేయండి
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# దశ 4: SDK ద్వారా కనుగొన్న ఎండ్పాయింట్ ఉపయోగించి OpenAI క్లయింట్‌ను సృష్టించండి
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# దశ 5: స్ట్రీమింగ్ చాట్ పూర్తిచెయ్యడం కోసం అభ్యర్థన పంపించండి
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

రన్ చేయండి:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// దశ 1: ఫౌండ్రి లొకల్ సర్వీసును ప్రారంభించండి
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// దశ 2: క్యాటలాగ్ నుండి కస్టమ్ మోడల్‌ను పొందండి
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// దశ 3: మోడల్‌ను మెమరీలో లోడ్ చేయండి
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// దశ 4: SDK ద్వారా కనుగొన్న ఎండ్‌పాయింట్‌ని ఉపయోగించి OpenAI క్లయింట్‌ను సృష్టించండి
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// దశ 5: స్ట్రీమింగ్ చాట్ పూర్తి అభ్యర్థనను పంపండి
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

రన్ చేయండి:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **ముఖ్య విషయం:** Foundry Local SDK డైనమిక్‌గా ఎండ్‌పాయింట్ కనుగొంటుంది, కాబట్టి మీరు ఎప్పుడూ పోర్ట్ నంబర్‌ను కఠినంగా ఇవ్వరు. ఇది ప్రొడక్షన్ అనువర్తనాల కోసం సిఫార్సు చేయబడిన పద్ధతి. మీ కస్టమ్-కంపైల్ మోడల్ SDK ద్వారా బిల్ట్-ఇన్ క్యాటలాగ్ మోడల్స్ లాగే పని చేస్తుంది.

---

## ఒక మోడల్‌ని కంపైల్ చేయడానికి ఎంచుకోవడం

Qwen3-0.6B ఈ ల్యాబ్‌లో ఉదాహరణగా ఉపయోగించబడింది ఎందుకంటే ఇది చిన్నది, వేగంగా కంపైల్ అవుతుంది, మరియు Apache 2.0 లైసెన్స్ కింద ఉచితంగా అందుబాటులో ఉంది. మీరు అనేక ఇతర మోడల్స్ కంపైల్ చేయవచ్చు. కొన్ని సూచనలు ఇక్కడ ఉన్నాయి:

| మోడల్ | Hugging Face ID | పరిమాణాలు | లైసెన్స్ | గమనికలు |
|-------|-----------------|------------|---------|----------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | చాలా చిన్నది, వేగంగా కంపైల్ అవుతుంది, పరీక్షలకు మంచిది |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | మెరుగైన నాణ్యత, ఇంకా వేగంగా కంపైల్ అవుతుంది |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | బలమైన నాణ్యత, ఎక్కువ RAM అవసరం |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Faceలో లైసెన్స్ అంగీకారం అవసరం |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | హై క్వాలిటీ, పెద్ద డౌన్లోడ్ మరియు ఎక్కువ కంపైల్ సమయం |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | ఇప్పటికే Foundry Local క్యాటలాగ్‌లో ఉంది (తులనాత్మకంగా ఉపయోగపడుతుంది) |

> **లైసెన్స్ గుర్తు చేసుకోండి:** Hugging Faceలో మోడల్ యొక్క లైసెన్స్ తప్పకుండా తనిఖీ చేయండి. కొన్ని మోడల్స్ (ఉదా: Llama) ను ఉపయోగించేందుకు లైసెన్స్ ఒప్పందాన్ని అంగీకరించి `huggingface-cli login` ద్వారా ఆథెంటికేషన్ అవసరం.

---

## కాన్సెప్ట్‌లు: కస్టమ్ మోడల్స్ ఎప్పుడు ఉపయోగించాలో

| పరిస్థితి | మీరు ఎందుకు స్వయంగా కంపైల్ చేయాలి? |
|----------|---------------------------------------|
| **మీకు కావలసింది క్యాటలాగ్‌లో లేదు** | Foundry Local క్యాటలాగ్ కస్టమైజ్ చేస్తారు. మీరు కోరుకున్న మోడల్ జాబితాలో లేకపోతే, మీరు స్వయంగా కంపైల్ చేయండి. |
| **ఫైన్-ట్యూన్ చేసిన మోడల్స్** | మీరు డొమెయిన్-గురించి స్పెషలైజ్డ్ డేటాకి ఫైన్-ట్యూన్ చేసిన మోడల్ ఉంటే, మీ స్వయంగా వెయిట్స్ కంపైల్ చేయాలి. |
| **ఖచ్చితమైన క్వాంటైజేషన్ అవసరాలు** | క్యాటలాగ్ డిఫాల్ట్‌తో తేడా ఉన్న ప్రిసిషన్ లేదా క్వాంటైజేషన్ వ్యూహం కావాలంటే. |
| **కొత్త మోడల్ విడుదలలు** | Hugging Faceలో కొత్త మోడల్ వచ్చినప్పుడు అది ఇప్పటికీ Foundry Local క్యాటలాగ్‌లో ఉండకపోవచ్చు. మీరు స్వయంగా కంపైల్ చేసుకోవడం తక్షణ అందుబాటును ఇస్తుంది. |
| **అన్వేషణ మరియు ఎక్స్‌పెరిమెంటేషన్** | ఉత్పత్తిలోకి తీసుకునే ముందు వివిధ మోడల్ వాస్థవాలను, పరిమాణాలను, కాన్ఫిగరేషన్లను లోకల్‌గా ప్రయత్నించడం. |

---

## సారాంశం

ఈ ల్యాబ్‌లో మీరు నేర్చుకున్నట్టు:

| దశ | మీరు చేసినది |
|-----|---------------|
| 1 | ONNX Runtime GenAI మోడల్ బిల్డర్ ఇన్‌స్టాల్ చేశారు |
| 2 | Hugging Face నుండి `Qwen/Qwen3-0.6B` ను ఆప్టిమైజ్డ్ ONNX మోడల్‌గా కంపైల్ చేశారు |
| 3 | `inference_model.json` చాట్-టెంప్లేట్ కాన్ఫిగరేషన్ ఫైల్ రూపొందించారు |
| 4 | కంపైల్ చేసిన మోడల్‌ను Foundry Local క్యాశేలో జోడించారు |
| 5 | CLI ద్వారా కస్టమ్ మోడల్‌తో ఇంటరాక్టివ్ చాట్ నడిపించారు |
| 6 | OpenAI-పరిచిత REST API ద్వారా మోడల్‌ను ప్రశ్నించారు |
| 7 | Python, JavaScript, మరియు C# ద్వారా OpenAI SDK ఉపయోగించి కనెక్ట్ అయ్యారు |
| 8 | Foundry Local SDKతో కస్టమ్ మోడల్ పూర్తి పరీక్ష నిర్వహించారు |

ప్రధాన విషయం ఏమిటంటే: **ఏదైనా ట్రాన్స్ఫార్మర్-ఆధారిత మోడల్ ONNX ఫార్మాట్‌లో కంపైల్ అయినప్పుడే Foundry Local ద్వారా నడుస్తుంది**. OpenAI-పరిచిత API అంటే మీ ఉండే అప్‌లికేషన్ కోడ్ మార్పు అవసరం లేకుండానే పనిచేస్తుంది; మీరు కేవలం మోడల్ పేరు మార్చాలి.

---

## ముఖ్యమైన పాఠాలు

| కాన్సెప్ట్ | వివరాలు |
|-----------|----------|
| ONNX Runtime GenAI Model Builder | Hugging Face మోడల్స్‌ను ONNX ఫార్మాట్ లో క్వాంటైజేషన్ తో ఒక కమాండ్ ద్వారా మార్చు |
| ONNX ఫార్మాట్ | Foundry Local ONNX మోడల్స్ మరియు ONNX Runtime GenAI కాన్ఫిగరేషన్ అవసరం |
| చాట్ టెంప్లేట్స్ | `inference_model.json` ఫైల్ ఏ మోడల్ కోసం ప్రాంప్ట్‌లను ఎలా రూపొందించాలో Foundry Local కి చెప్పును |
| హార్డ్‌వేర్ టార్గెట్లు | CPU, NVIDIA GPU (CUDA), DirectML (విండోస్ GPU), లేదా WebGPU కోసం కంపైల్ చేయవచ్చు, మీ హార్డ్‌వేర్ ఆధారంగా |
| క్వాంటైజేషన్ | తక్కువ ప్రిసిషన్ (int4) ఆ కొలెడ్ సైజ్ తగ్గించి వేగం పెంచుతుంది, fp16 GPUలపై అధిక నాణ్యత దখలుస్తుంది |
| API అనుకూలత | కస్టమ్ మోడల్స్ బిల్ట్-ఇన్ మోడల్స్ వలె OpenAI-పరిచిత API ఉపయోగిస్తాయి |
| Foundry Local SDK | ఈ SDK సర్వీస్ స్టార్ట్, ఎండ్‌పాయింట్ కనుగొనడం, మరియు మోడల్ లోడింగ్ ఆటోమేటిక్‌గా నిర్వహిస్తుంది, క్యాటలాగ్ మరియు కస్టమ్ రెండింటి కోసం |

---

## మరింత చదవడం

| వనరు | లింక్ |
|---------|---------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local కస్టమ్ మోడల్ గైడ్ | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 మోడల్ కుటుంబం | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive డాక్యుమెంటేషన్ | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## తదుపరి దశలు

మీ లోకల్ మోడల్స్ బాహ్య ఫంక్షన్లను కాల్ చేయడానికి ఎలా అనుమతించాలి అనేది నేర్చుకోడానికి [Part 11: Tool Calling with Local Models](part11-tool-calling.md) వద్ద కొనసాగండి.

[← Part 9: Whisper Voice Transcription](part9-whisper-voice-transcription.md) | [Part 11: Tool Calling →](part11-tool-calling.md)