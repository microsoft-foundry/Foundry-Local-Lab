![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第10部分：將自訂或 Hugging Face 模型與 Foundry Local 一起使用

> **目標：** 將 Hugging Face 模型編譯為 Foundry Local 所需的優化 ONNX 格式，配置聊天範本，將其新增至本地快取，並使用 CLI、REST API 及 OpenAI SDK 執行推論。

## 概覽

Foundry Local 出貨時附有經過精選的預編譯模型目錄，但您不必侷限於該清單。任何基於 Transformer 的語言模型，只要存在於 [Hugging Face](https://huggingface.co/)（或以本地 PyTorch / Safetensors 格式儲存），都可以編譯為優化的 ONNX 模型並透過 Foundry Local 服務。

編譯流程使用的是 **ONNX Runtime GenAI Model Builder**，這是隨 `onnxruntime-genai` 套件提供的命令列工具。該模型建構器會處理繁重的工作：下載原始權重、轉換為 ONNX 格式、套用量化（int4、fp16、bf16），並輸出 Foundry Local 所需的設定檔（包含聊天範本與分詞器）。

在本實作課程中，您將編譯來自 Hugging Face 的 **Qwen/Qwen3-0.6B**，將其註冊到 Foundry Local，並在您自己的裝置上與之對話。

---

## 學習目標

完成本實作課程後，您將能夠：

- 解釋自訂模型編譯的用途與使用時機
- 安裝 ONNX Runtime GenAI 模型建構器
- 以單一指令將 Hugging Face 模型編譯為優化的 ONNX 格式
- 理解主要的編譯參數（執行供應者、精度）
- 建立 `inference_model.json` 聊天範本設定檔
- 將編譯好的模型新增到 Foundry Local 快取
- 使用 CLI、REST API 及 OpenAI SDK 對自訂模型執行推論

---

## 前置需求

| 需求 | 詳細資料 |
|-------------|---------|
| **Foundry Local CLI** | 已安裝且在您的 `PATH` 中（參見 [第1部分](part1-getting-started.md)） |
| **Python 3.10+** | ONNX Runtime GenAI 模型建構器所需 |
| **pip** | Python 套件管理工具 |
| <strong>磁碟空間</strong> | 至少有 5 GB 可用來存放原始與編譯後的模型檔案 |
| **Hugging Face 帳戶** | 部分模型下載前需接受授權。Qwen3-0.6B 採用 Apache 2.0 授權，免費提供。 |

---

## 環境設定

模型編譯需要多個龐大的 Python 套件（PyTorch、ONNX Runtime GenAI、Transformers）。請建立獨立虛擬環境，以避免影響系統 Python 或其他專案。

```bash
# 從倉庫根目錄開始
python -m venv .venv
```

啟動環境：

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

升級 pip，以避免依賴性解析問題：

```bash
python -m pip install --upgrade pip
```

> **提示：** 如果您已擁有先前實作課程建立的 `.venv`，可重複使用。只要在繼續前確定它已被啟動即可。

---

## 概念：編譯流程

Foundry Local 要求模型為帶有 ONNX Runtime GenAI 設定的 ONNX 格式。大多數在 Hugging Face 上的開源模型是以 PyTorch 或 Safetensors 權重形式發佈，因此需要一個轉換步驟。

![自訂模型編譯流程](../../../images/custom-model-pipeline.svg)

### 模型建構器做了什麼？

1. <strong>下載</strong> Hugging Face 的原始模型（或從本地路徑讀取）。
2. <strong>轉換</strong> PyTorch / Safetensors 權重為 ONNX 格式。
3. <strong>量化</strong> 模型至更低精度（例如 int4），以減少記憶體使用和提升吞吐量。
4. <strong>輸出</strong> ONNX Runtime GenAI 配置檔（`genai_config.json`）、聊天範本（`chat_template.jinja`）及所有分詞器檔案，讓 Foundry Local 能夠載入並服務模型。

### ONNX Runtime GenAI Model Builder 與 Microsoft Olive 的比較

您可能會看到 Microsoft Olive 作為另一個模型優化工具。兩者皆可製作 ONNX 模型，但用途與取捨不同：

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| <strong>套件</strong> | `onnxruntime-genai` | `olive-ai` |
| <strong>主要目的</strong> | 將生成式 AI 模型轉換並量化為 ONNX Runtime GenAI 推論格式 | 端到端模型優化框架，支援多種後端和硬體目標 |
| <strong>易用性</strong> | 單一指令 — 一步轉換 + 量化 | 工作流程為基礎 — 可配置的多次通過管線，以 YAML/JSON 控制 |
| <strong>輸出格式</strong> | ONNX Runtime GenAI 格式（可直接用於 Foundry Local） | 通用 ONNX、ONNX Runtime GenAI 或依工作流程而定的格式 |
| <strong>硬體目標</strong> | CPU、CUDA、DirectML、TensorRT RTX、WebGPU | CPU、CUDA、DirectML、TensorRT、Qualcomm QNN 等更多 |
| <strong>量化選項</strong> | int4、int8、fp16、fp32 | int4（AWQ、GPTQ、RTN）、int8、fp16，還有圖優化、層級調整 |
| <strong>模型範圍</strong> | 生成式 AI 模型（LLM、SLM） | 任何可轉 ONNX 的模型（視覺、NLP、音訊、多模態） |
| <strong>適合情境</strong> | 快速單模型編譯用於本地推論 | 需要細緻優化控制的生產級管線 |
| <strong>依賴性規模</strong> | 中等（PyTorch、Transformers、ONNX Runtime） | 較大（包含 Olive 框架，及依工作流程選擇的額外套件） |
| **Foundry Local 整合** | 直通 — 輸出立即相容 | 需加上 `--use_ort_genai` 參數及額外設定 |

> **為何本實作課程使用 Model Builder：** 針對編譯單一 Hugging Face 模型並註冊給 Foundry Local 的任務，Model Builder 是最簡單且最可靠的方式。它以一條命令產出 Foundry Local 精確要求的格式。您日後如果需要進階優化功能 —— 如準確度感知的量化、圖形手術或多遍調整 —— Olive 是功能強大的選擇。詳見 [Microsoft Olive 文件](https://microsoft.github.io/Olive/)。

---

## 實作練習

### 練習 1：安裝 ONNX Runtime GenAI Model Builder

安裝包含模型建構器的 ONNX Runtime GenAI 套件：

```bash
pip install onnxruntime-genai
```

利用下列指令驗證模型建構器是否可用：

```bash
python -m onnxruntime_genai.models.builder --help
```

您應該會看到說明輸出，列出參數如 `-m`（模型名稱）、`-o`（輸出路徑）、`-p`（精度）、`-e`（執行供應者）等。

> **注意：** 模型建構器依賴 PyTorch、Transformers 及其他多個套件，安裝過程可能需要幾分鐘。

---

### 練習 2：為 CPU 編譯 Qwen3-0.6B

執行下列命令，從 Hugging Face 下載 Qwen3-0.6B，並為 CPU 推論編譯，使用 int4 量化：

**macOS / Linux：**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows (PowerShell)：**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### 各參數意義

| 參數 | 功能 | 使用值 |
|-----------|---------|------------|
| `-m` | Hugging Face 模型 ID 或本地目錄路徑 | `Qwen/Qwen3-0.6B` |
| `-o` | 編譯後 ONNX 模型儲存資料夾 | `models/qwen3` |
| `-p` | 編譯時套用的量化精度 | `int4` |
| `-e` | ONNX Runtime 執行供應者（目標硬體） | `cpu` |
| `--extra_options hf_token=false` | 跳過 Hugging Face 認證（公開模型適用） | `hf_token=false` |

> **需要多長時間？** 編譯時間依硬體及模型大小而異。以 Qwen3-0.6B 在現代 CPU 上 int4 量化為例，大約需 5 到 15 分鐘。模型越大，時間成比例增加。

完成後，您應會在 `models/qwen3` 目錄看到編譯好的模型檔案。確認輸出：

```bash
ls models/qwen3
```

您應可看到以下檔案包括：
- `model.onnx` 與 `model.onnx.data` — 編譯後的模型權重
- `genai_config.json` — ONNX Runtime GenAI 設定檔
- `chat_template.jinja` — 模型的聊天範本（自動產生）
- `tokenizer.json`、`tokenizer_config.json` — 分詞器檔案
- 其他詞彙及設定檔案

---

### 練習 3：為 GPU 編譯（選用）

若您有支援 CUDA 的 NVIDIA GPU，則可編譯 GPU 最佳化版本以提升推論速度：

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **注意：** GPU 編譯需要 `onnxruntime-gpu` 以及可用的 CUDA 環境。若缺少，模型建構器會報錯。您也可以跳過此練習，繼續使用 CPU 版本。

#### 硬體專用編譯參考

| 目標 | 執行供應者 (`-e`) | 推薦精度 (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` 或 `int4` |
| DirectML（Windows GPU） | `dml` | `fp16` 或 `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### 精度取捨

| 精度 | 大小 | 速度 | 清晰度 |
|-----------|------|-------|---------|
| `fp32` | 最大 | 最慢 | 最高準確度 |
| `fp16` | 大 | 快（GPU） | 非常好準確度 |
| `int8` | 小 | 快 | 輕微準確度損失 |
| `int4` | 最小 | 最快 | 中等準確度損失 |

在本地開發環境中，以 CPU 執行的 `int4` 提供速度與資源使用的最佳平衡。若需要生產級品質，建議在 CUDA GPU 上採用 `fp16`。

---

### 練習 4：建立聊天範本設定檔

模型建構器會自動在輸出目錄生成 `chat_template.jinja` 檔案與 `genai_config.json`。不過，Foundry Local 還需一個 `inference_model.json` ，用來了解如何為您的模型格式化提示。該檔案定義模型名稱及將用戶訊息包覆在正確特殊標記中的提示範本。

#### 步驟1：檢查編譯輸出

列出編譯後模型目錄內容：

```bash
ls models/qwen3
```

您應看到如下檔案：
- `model.onnx` 與 `model.onnx.data` — 編譯的模型權重
- `genai_config.json` — ONNX Runtime GenAI 設定（自動產生）
- `chat_template.jinja` — 模型聊天範本（自動產生）
- `tokenizer.json`、`tokenizer_config.json` — 分詞器檔案
- 其他各種設定與詞表檔案

#### 步驟2：產生 inference_model.json

`inference_model.json` 檔案告訴 Foundry Local 如何格式化提示。請在您的代碼庫根目錄（與 `models/` 資料夾同層）建立名為 `generate_chat_template.py` 的 Python 腳本：

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# 建立一個最小的對話以提取聊天範本
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

# 建立 inference_model.json 結構
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

從代碼庫根目錄執行該腳本：

```bash
python generate_chat_template.py
```

> **注意：** `transformers` 套件同時作為 `onnxruntime-genai` 的依賴已安裝。如遇 `ImportError`，請先執行 `pip install transformers`。

此腳本將自動在 `models/qwen3` 目錄生成 `inference_model.json` 檔案。內容告訴 Foundry Local 如何用正確特殊標記包裹 Qwen3 用戶輸入。

> **重要說明：** `inference_model.json` 中的 `"Name"` 欄位（此腳本設為 `qwen3-0.6b`）是您後續所有指令及 API 調用中使用的 <strong>模型別名</strong>。如果您更改此名稱，請相應更新練習6–10中的模型名稱。

#### 步驟3：檢查設定

打開 `models/qwen3/inference_model.json`，確認它包含 `Name` 欄位及包含 `assistant` 和 `prompt` 鍵的 `PromptTemplate` 物件。提示範本應該包括 `<|im_start|>` 和 `<|im_end|>` 等特殊標記（具體標記視模型聊天範本而定）。

> **手動替代法：** 若您不想執行腳本，也可手動建立此檔案。最重要的是 `prompt` 欄位必須包含完整的模型聊天範本，且 `{Content}` 為用戶訊息的佔位符。

---

### 練習 5：驗證模型目錄結構
模型建立器會將所有編譯好的檔案直接放入您指定的輸出目錄。請確認最終的結構是否正確：

```bash
ls models/qwen3
```

目錄中應包含以下檔案：

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

> **注意：** 與其他一些編譯工具不同，模型建立器不會建立巢狀子目錄。所有檔案都直接位於輸出資料夾中，這正是 Foundry Local 所期望的結構。

---

### 練習 6：將模型新增到 Foundry Local 快取

告訴 Foundry Local 從哪裡尋找您已編譯的模型，方法是將目錄加入它的快取：

```bash
foundry cache cd models/qwen3
```

確認模型是否出現在快取中：

```bash
foundry cache ls
```

您應該會看到自訂模型與之前已快取的模型（例如 `phi-3.5-mini` 或 `phi-4-mini`）一起列出。

---

### 練習 7：使用 CLI 執行自訂模型

啟動與您新編譯模型的互動式聊天（`qwen3-0.6b` 別名來自您在 `inference_model.json` 中設定的 `Name` 欄位）：

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` 參數顯示額外的診斷資訊，首次測試自訂模型時非常有幫助。如果模型成功載入，您會看到互動提示。試著輸入幾則訊息：

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

輸入 `exit` 或按 `Ctrl+C` 結束會話。

> **疑難排解：** 如果模型無法載入，請檢查以下項目：
> - `genai_config.json` 檔案是由模型建立器產生的。
> - `inference_model.json` 檔案存在且為有效 JSON。
> - ONNX 模型檔案位於正確目錄中。
> - 您有足夠的可用記憶體（Qwen3-0.6B int4 約需 1 GB）。
> - Qwen3 是一個會產生 `<think>` 標籤的推理模型。如果您看到回應前綴了 `<think>...</think>`，這是正常行為。您可以調整 `inference_model.json` 中的提示範本以抑制思考輸出。

---

### 練習 8：透過 REST API 查詢自訂模型

如果您在練習 7 中離開了互動會話，模型可能已不在載入狀態。請先啟動 Foundry Local 服務並載入模型：

```bash
foundry service start
foundry model load qwen3-0.6b
```

確認服務運行的埠號：

```bash
foundry service status
```

接著發送請求（若埠號不同，請替換 `5273` 為實際埠號）：

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows 注意事項：** 上述的 `curl` 指令使用 bash 語法。在 Windows 上，請改用 PowerShell 的 `Invoke-RestMethod` 指令，如下所示。

**PowerShell：**

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

### 練習 9：使用 OpenAI SDK 呼叫自訂模型

您可以使用與內建模型相同的 OpenAI SDK 程式碼連接至自訂模型（參見 [第三部分](part3-sdk-and-apis.md)）。唯一差別是模型名稱不同。

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local 不會驗證 API 密鑰
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
  apiKey: "foundry-local", // Foundry Local 不會驗證 API 金鑰
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

> **關鍵點：** 因為 Foundry Local 提供 OpenAI 兼容 API，任何對內建模型有效的程式碼，同樣能操作您的自訂模型。您只需更改 `model` 參數。

---

### 練習 10：使用 Foundry Local SDK 測試自訂模型

在先前的實驗中，您使用 Foundry Local SDK 啟動服務、發現端點並自動管理模型。對於自訂編譯的模型，您可以完全按照相同模式操作。SDK 會處理服務啟動及端點發現，因此您的程式不必硬編碼 `localhost:5273`。

> **注意：** 請確保安裝 Foundry Local SDK 才能執行以下範例：
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** 新增 `Microsoft.AI.Foundry.Local` 與 `OpenAI` NuGet 套件
>
> 請將每個指令檔案<strong>儲存在版本庫根目錄</strong>（與您的 `models/` 資料夾相同的位置）。

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# 第一步：啟動 Foundry 本地服務並載入自訂模型
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# 第二步：檢查快取中的自訂模型
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# 第三步：將模型載入記憶體
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# 第四步：使用 SDK 發現的端點建立 OpenAI 用戶端
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# 第五步：發送串流聊天完成請求
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

執行它：

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

// 第一步：啟動 Foundry Local 服務
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 第二步：從目錄中取得自訂模型
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// 第三步：將模型載入記憶體
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// 第四步：使用 SDK 自動偵測的端點建立 OpenAI 用戶端
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// 第五步：發送串流式聊天完成請求
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

執行它：

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

> **關鍵點：** Foundry Local SDK 會動態探索端點，您永遠不用硬編碼埠號。這是生產環境推薦做法。您的自訂編譯模型透過 SDK 的行為與內建目錄模型完全相同。

---

## 選擇要編譯的模型

此實驗以 Qwen3-0.6B 作為範例，因其體積小、編譯速度快，且可免費使用 Apache 2.0 許可證。不過，您可以編譯其他許多模型，這裡是一些建議：

| 模型 | Hugging Face ID | 參數數量 | 許可證 | 備註 |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | 非常小，編譯快速，適合測試 |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | 品質較佳，仍然快速編譯 |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | 品質強大，但需求更多記憶體 |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | 需在 Hugging Face 接受授權條款 |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | 高品質，下載與編譯時間較長 |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | 已在 Foundry Local 目錄中（可作比較） |

> **授權提醒：** 在使用前，請務必檢查模型在 Hugging Face 上的授權條款。某些模型（如 Llama）下載前須先接受授權協議並使用 `huggingface-cli login` 進行驗證。

---

## 概念：何時使用自訂模型

| 情境 | 為何要自行編譯？ |
|----------|----------------------|
| <strong>需要的模型不在目錄中</strong> | Foundry Local 目錄是精選的。如果您需要的模型不在其中，就自行編譯。 |
| <strong>微調模型</strong> | 若您基於領域特定資料微調了模型，就需自行編譯專屬權重。 |
| <strong>特定量化需求</strong> | 可能想要跟目錄預設不同的精度或量化策略。 |
| <strong>較新的模型釋出</strong> | Hugging Face 新發佈模型可能還沒加入 Foundry Local 目錄，編譯後即可立即使用。 |
| <strong>研究與實驗</strong> | 本地嘗試不同模型架構、大小或配置，先行評估，再決定生產環境採用哪款。 |

---

## 總結

這個實驗中您學到：

| 步驟 | 您做了什麼 |
|------|-------------|
| 1 | 安裝 ONNX Runtime GenAI 模型建立器 |
| 2 | 從 Hugging Face 編譯 `Qwen/Qwen3-0.6B` 成優化 ONNX 模型 |
| 3 | 建立 `inference_model.json` 聊天範本設定檔 |
| 4 | 把編譯模型新增到 Foundry Local 快取 |
| 5 | 透過 CLI 與自訂模型進行互動式聊天 |
| 6 | 透過 OpenAI 兼容 REST API 查詢模型 |
| 7 | 使用 Python、JavaScript 與 C# 的 OpenAI SDK 連接模型 |
| 8 | 利用 Foundry Local SDK 端到端測試自訂模型 |

重點是，**任何基於 transformer 的模型，只要編譯成 ONNX 格式，都能在 Foundry Local 上運行**。OpenAI 兼容 API 讓您現有的應用程式碼都不需更動，惟唯獨替換模型名稱。

---

## 重要要點

| 概念 | 詳細說明 |
|---------|--------|
| ONNX Runtime GenAI 模型建立器 | 一個命令即可將 Hugging Face 模型轉為 ONNX 格式並量化 |
| ONNX 格式 | Foundry Local 需求 ONNX 模型及 ONNX Runtime GenAI 配置 |
| 聊天範本 | `inference_model.json` 告訴 Foundry Local 如何為特定模型格式化提示 |
| 硬體目標 | 支援編譯為 CPU、NVIDIA GPU（CUDA）、DirectML（Windows GPU）或 WebGPU 使用 |
| 量化 | 降低精度（int4）能縮小模型大小及加速，代價是略失準確度；fp16 在 GPU 上保留高品質 |
| API 相容性 | 自訂模型使用與內建模型相同的 OpenAI 兼容 API |
| Foundry Local SDK | SDK 自動處理服務啟動、端點探索及模型載入，適用目錄及自訂模型 |

---

## 參考資料

| 資源 | 連結 |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local 自訂模型指南 | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 模型系列 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive 文件 | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## 下一步

繼續前往 [第 11 部分：本地模型的工具呼叫](part11-tool-calling.md)，瞭解如何讓本地模型呼叫外部函式。

[← 第 9 部分：Whisper 語音轉錄](part9-whisper-voice-transcription.md) | [第 11 部分：工具呼叫 →](part11-tool-calling.md)