![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第10部分：在 Foundry Local 使用自訂或 Hugging Face 模型

> **目標：** 將 Hugging Face 模型編譯成 Foundry Local 所需的優化 ONNX 格式，配置聊天範本，將其添加到本地快取，並使用 CLI、REST API 及 OpenAI SDK 對其進行推理。

## 概觀

Foundry Local 隨附經策劃的預編譯模型目錄，但你不僅限於此清單。任何在 [Hugging Face](https://huggingface.co/) 上可用的基於 Transformer 的語言模型（或以 PyTorch / Safetensors 格式本地存儲）都可以編譯成優化的 ONNX 模型，並通過 Foundry Local 提供服務。

編譯流程使用了 **ONNX Runtime GenAI Model Builder**，這是一個包含於 `onnxruntime-genai` 套件中的命令列工具。模型建立器負責繁重工作：下載來源權重，轉換為 ONNX 格式，應用量化（int4、fp16、bf16），並產出 Foundry Local 期望的配置文件（包括聊天範本與分詞器）。

在本實驗中，你將從 Hugging Face 編譯 **Qwen/Qwen3-0.6B**，在 Foundry Local 中註冊，並完全在本地裝置上與其對話。

---

## 學習目標

完成本實驗後，你將能夠：

- 解釋為什麼自訂模型編譯有用及其使用時機
- 安裝 ONNX Runtime GenAI 模型建立器
- 透過單一命令將 Hugging Face 模型編譯為優化 ONNX 格式
- 了解關鍵編譯參數（執行提供者、精度）
- 建立 `inference_model.json` 聊天範本配置文件
- 將編譯過的模型加入 Foundry Local 快取
- 使用 CLI、REST API 及 OpenAI SDK 針對自訂模型執行推理

---

## 先決條件

| 需求 | 詳細資訊 |
|-------------|---------|
| **Foundry Local CLI** | 已安裝並加入你的 `PATH`（參見 [第1部分](part1-getting-started.md)） |
| **Python 3.10+** | ONNX Runtime GenAI 模型建立器所需 |
| **pip** | Python 套件管理工具 |
| <strong>硬碟空間</strong> | 至少 5 GB 可用空間以存放來源及編譯模型檔案 |
| **Hugging Face 帳號** | 部分模型下載前需接受授權條款。Qwen3-0.6B 採用 Apache 2.0 授權，可自由取得。 |

---

## 環境設置

模型編譯需要多個大型 Python 套件（PyTorch、ONNX Runtime GenAI、Transformers）。請建立專門的虛擬環境，避免影響系統 Python 或其他專案。

```bash
# 從儲存庫根目錄開始
python -m venv .venv
```
  
啟用虛擬環境：

**Windows (PowerShell)：**  
```powershell
.venv\Scripts\Activate.ps1
```
  
**macOS / Linux：**  
```bash
source .venv/bin/activate
```
  
升級 pip 以避免依賴解析問題：

```bash
python -m pip install --upgrade pip
```
  
> **提示：** 如果你先前實驗已有 `.venv`，可以重用。只要確保啟用後再繼續即可。

---

## 概念：編譯流程

Foundry Local 需要 ONNX 格式且帶有 ONNX Runtime GenAI 配置的模型。大多數在 Hugging Face 開源的模型以 PyTorch 或 Safetensors 權重形式發佈，因此需經過轉換步驟。

![自訂模型編譯流程](../../../images/custom-model-pipeline.svg)

### 模型建立器的功能？

1. <strong>下載</strong> 來源模型（來自 Hugging Face 或本地路徑）。
2. <strong>轉換</strong> PyTorch / Safetensors 權重至 ONNX 格式。
3. <strong>量化</strong> 模型至較低精度（例如 int4），以減少記憶體使用並提高效能。
4. <strong>產出</strong> ONNX Runtime GenAI 配置（`genai_config.json`）、聊天範本(`chat_template.jinja`)及所有分詞器檔案，讓 Foundry Local 可載入並服務模型。

### ONNX Runtime GenAI 模型建立器 vs Microsoft Olive

你或許會碰到 **Microsoft Olive** 作為模型優化的備選工具。兩者皆可輸出 ONNX 模型，但用途及權衡不同：

|  | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| <strong>套件</strong> | `onnxruntime-genai` | `olive-ai` |
| <strong>主要目標</strong> | 轉換且量化生成式 AI 模型供 ONNX Runtime GenAI 推理 | 端對端模型優化框架，支援多種後端與硬體 |
| <strong>易用性</strong> | 單一命令 — 一步完成轉換及量化 | 工作流程導向 — 可配置多重流程帶 YAML/JSON |
| <strong>輸出格式</strong> | ONNX Runtime GenAI 格式（Foundry Local 相容） | 通用 ONNX、ONNX Runtime GenAI 或依流程決定 |
| <strong>硬體目標</strong> | CPU、CUDA、DirectML、TensorRT RTX、WebGPU | CPU、CUDA、DirectML、TensorRT、Qualcomm QNN 及更多 |
| <strong>量化選項</strong> | int4、int8、fp16、fp32 | int4（AWQ、GPTQ、RTN）、int8、fp16，還有圖形優化和分層調校 |
| <strong>模型範圍</strong> | 生成式 AI 模型（LLMs、SLMs） | 任何可轉為 ONNX 的模型（視覺、NLP、語音、多模態） |
| <strong>適用情境</strong> | 本地推理快速單模型編譯 | 生產線需細粒度優化控制 |
| <strong>依賴體積</strong> | 中等（PyTorch、Transformers、ONNX Runtime） | 較大（加入 Olive 框架，並按流程附加額外） |
| **Foundry Local 整合** | 直接 — 輸出即相容 | 需 `--use_ort_genai` 旗標與額外配置 |

> **為何本實驗使用模型建立器：** 編譯單一 Hugging Face 模型並註冊至 Foundry Local，模型建立器是最簡單可靠的路徑。它以單一命令產出 Foundry Local 所需的完整輸出格式。若日後需進階優化功能（如精度感知量化、圖形手術、多輪調校），可考慮 Olive。詳情請參閱 [Microsoft Olive 文件](https://microsoft.github.io/Olive/)。

---

## 實驗練習

### 練習1：安裝 ONNX Runtime GenAI 模型建立器

安裝包含模型建立器工具的 ONNX Runtime GenAI 套件：

```bash
pip install onnxruntime-genai
```
  
透過檢查模型建立器是否可用來驗證安裝：

```bash
python -m onnxruntime_genai.models.builder --help
```
  
你應該看到列出 `-m`（模型名）、`-o`（輸出路徑）、`-p`（精度）、`-e`（執行提供者）等參數的幫助輸出。

> **注意：** 模型建立器依賴 PyTorch、Transformers 等多套件，安裝可能需幾分鐘。

---

### 練習2：為 CPU 編譯 Qwen3-0.6B

執行以下命令，從 Hugging Face 下載 Qwen3-0.6B 模型並以 int4 量化為 CPU 推理編譯：

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
  
#### 參數說明

| 參數 | 功用 | 使用值 |
|-----------|---------|------------|
| `-m` | Hugging Face 模型 ID 或本地路徑 | `Qwen/Qwen3-0.6B` |
| `-o` | 編譯後 ONNX 模型保存目錄 | `models/qwen3` |
| `-p` | 編譯時應用的量化精度 | `int4` |
| `-e` | ONNX Runtime 執行提供者（目標硬體） | `cpu` |
| `--extra_options hf_token=false` | 跳過 Hugging Face 認證（公開模型可用） | `hf_token=false` |

> **需多久時間？** 編譯時間因硬體與模型大小而異。Qwen3-0.6B 在現代 CPU 上進行 int4 量化約需 5 至 15 分鐘，大模型時間更長。

命令完成後應在 `models/qwen3` 目錄中看到編譯模型檔案。確認輸出：

```bash
ls models/qwen3
```
  
你應看到檔案包括：  
- `model.onnx` 和 `model.onnx.data` — 編譯後的模型權重  
- `genai_config.json` — ONNX Runtime GenAI 配置  
- `chat_template.jinja` — 模型聊天範本（自動生成）  
- `tokenizer.json`、`tokenizer_config.json` — 分詞器檔案  
- 其他詞彙與配置文件  

---

### 練習3：為 GPU 編譯（選擇性）

若你有帶 CUDA 支援的 NVIDIA GPU，可編譯 GPU 最優化版本以加速推理：

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```
  
> **注意：** GPU 編譯需 `onnxruntime-gpu` 及正常安裝 CUDA。如無，模型建立器將報錯。可跳過此練習，繼續使用 CPU 版本。

#### 硬體專用編譯參考

| 目標 | 執行提供者 (`-e`) | 推薦精度 (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` 或 `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` 或 `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### 精度權衡

| 精度 | 大小 | 速度 | 品質 |
|-----------|------|-------|---------|
| `fp32` | 最大 | 最慢 | 最高準確度 |
| `fp16` | 大 | 快（GPU） | 非常好準確度 |
| `int8` | 小 | 快 | 輕微準確度損失 |
| `int4` | 最小 | 最快 | 中等準確度損失 |

對大多數本地開發而言，CPU 上的 `int4` 提供最佳速度與資源平衡。生產品質建議在 CUDA GPU 使用 `fp16`。

---

### 練習4：建立聊天範本配置

模型建立器會自動在輸出目錄產生 `chat_template.jinja` 與 `genai_config.json`。但 Foundry Local 也需要 `inference_model.json`，以瞭解如何為模型格式化提示詞。此文件定義模型名稱及包裝使用者訊息的提示範本。

#### 步驟1：檢查編譯輸出

列出已編譯模型目錄內容：

```bash
ls models/qwen3
```
  
你應看到檔案如：  
- `model.onnx` 和 `model.onnx.data` — 編譯權重  
- `genai_config.json` — ONNX Runtime GenAI 配置（自動生成）  
- `chat_template.jinja` — 模型聊天範本（自動生成）  
- `tokenizer.json`、`tokenizer_config.json` — 分詞器檔案  
- 其他配置及詞彙文件  

#### 步驟2：生成 inference_model.json 檔案

`inference_model.json` 告訴 Foundry Local 如何格式化提示詞。建立 Python 腳本 `generate_chat_template.py` <strong>於專案根目錄</strong>（即包含 `models/` 目錄的同一目錄）：

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# 建立一個最簡對話以提取聊天範本
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
  
從專案根目錄執行此腳本：

```bash
python generate_chat_template.py
```
  
> **注意：** `transformers` 套件已隨 `onnxruntime-genai` 安裝。如出現 `ImportError`，請先執行 `pip install transformers`。

腳本會在 `models/qwen3` 下生成 `inference_model.json`。該文件告訴 Foundry Local 如何用 Qwen3 的特殊標記包裝使用者輸入。

> **重要：** `inference_model.json` 中的 `"Name"` 欄位（本腳本設為 `qwen3-0.6b`）是你後續所有指令及 API 調用中使用的<strong>模型別名</strong>。若你更改此名稱，請同時更新練習6至10中使用的模型名稱。

#### 步驟3：驗證配置

開啟 `models/qwen3/inference_model.json`，確認其中包含 `Name` 欄位與具有 `assistant` 和 `prompt` 鍵的 `PromptTemplate` 物件。提示範本應包含如 `<|im_start|>` 及 `<|im_end|>` 的特殊標記（具體標記視模型聊天範本而定）。

> **手動替代方案：** 若不想執行腳本，也可手動建立該文件。關鍵是 `prompt` 欄位必須包含模型完整聊天範本，且以 `{Content}` 作為使用者訊息的佔位符。

---

### 練習5：驗證模型目錄結構
模型構建器會將所有編譯好的文件直接放入您指定的輸出目錄。請確認最終結構是否正確：

```bash
ls models/qwen3
```

該目錄應包含以下文件：

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

> **注意：** 與其他某些編譯工具不同，模型構建器不會創建嵌套子目錄。所有文件都直接放在輸出資料夾中，這正是 Foundry Local 所期望的。

---

### 練習 6：將模型添加到 Foundry Local 快取中

告訴 Foundry Local 在哪裡找到您編譯好的模型，方法是將目錄添加到其快取中：

```bash
foundry cache cd models/qwen3
```

確認模型是否出現在快取中：

```bash
foundry cache ls
```

您應該會看到您的自訂模型與任何先前快取的模型（例如 `phi-3.5-mini` 或 `phi-4-mini`）一同列出。

---

### 練習 7：使用 CLI 執行自訂模型

啟動一個與您新編譯模型的互動式聊天會話（`qwen3-0.6b` 這個別名來自您在 `inference_model.json` 中設定的 `Name` 欄位）：

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` 標誌會顯示額外的診斷資訊，首次測試自訂模型時這非常有用。如果模型成功載入，您會看到互動提示。嘗試輸入幾條訊息：

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

輸入 `exit` 或按 `Ctrl+C` 結束會話。

> **故障排除：** 若模型載入失敗，請檢查以下項目：
> - `genai_config.json` 檔案由模型構建器生成。
> - `inference_model.json` 檔案存在且為有效 JSON。
> - ONNX 模型檔案位於正確目錄。
> - 您有足夠的可用 RAM（Qwen3-0.6B int4 約需 1 GB）。
> - Qwen3 是一個推理模型，會產生 `<think>` 標籤。若您看到回應前綴有 `<think>...</think>`，這是正常行為。`inference_model.json` 中的提示模板可調整以抑制思考輸出。

---

### 練習 8：透過 REST API 查詢自訂模型

如果您在練習 7 中退出了互動會話，模型可能不再已載入。請先啟動 Foundry Local 服務並載入模型：

```bash
foundry service start
foundry model load qwen3-0.6b
```

確認服務運行的埠號：

```bash
foundry service status
```

然後發送請求（如果不同，請將 `5273` 替換為您的實際埠號）：

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows 注意事項：** 上述 `curl` 命令使用 bash 語法。在 Windows 上，請改用 PowerShell 的 `Invoke-RestMethod` 指令如下。

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

### 練習 9：使用 OpenAI SDK 連接自訂模型

您可以使用與內建模型完全相同的 OpenAI SDK 程式碼連接您的自訂模型（請參閱[第 3 部分](part3-sdk-and-apis.md)）。唯一不同的是模型名稱。

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local 不會驗證 API 金鑰
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
  apiKey: "foundry-local", // Foundry 本地端不會驗證 API 金鑰
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

> **重點：** 因為 Foundry Local 提供與 OpenAI 相容的 API，任何適用於內建模型的程式碼也適用於您的自訂模型。您只需更改 `model` 參數即可。

---

### 練習 10：使用 Foundry Local SDK 測試自訂模型

在之前的實驗中，您已使用 Foundry Local SDK 啟動服務、發現端點及自動管理模型。您可以用完全相同的模式操作您的自訂編譯模型。SDK 處理服務啟動和端點發現，您的程式碼不需要硬編碼 `localhost:5273`。

> **注意：** 請確保在執行以下範例前已安裝 Foundry Local SDK：
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** 新增 `Microsoft.AI.Foundry.Local` 和 `OpenAI` NuGet 套件
>
> 請將每個腳本檔案<strong>儲存在儲存庫根目錄</strong>（與您的 `models/` 資料夾同目錄）。

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# 第一步：啟動 Foundry Local 服務並加載自訂模型
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# 第二步：檢查快取中的自訂模型
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# 第三步：將模型載入記憶體
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# 第四步：使用 SDK 探測的端點建立 OpenAI 用戶端
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# 第五步：發送串流聊天補全請求
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

// 第二步：從目錄獲取自訂模型
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// 第三步：將模型載入記憶體
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// 第四步：使用 SDK 探測到的端點建立 OpenAI 用戶端
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// 第五步：發送串流聊天完成請求
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

> **重點：** Foundry Local SDK 會動態發現端點，因此您永遠不會硬編碼端口號。這是生產環境應用程式的推薦做法。您的自訂編譯模型透過 SDK 的行為與內建目錄模型完全相同。

---

## 選擇要編譯的模型

本實驗以 Qwen3-0.6B 作為參考範例，因為它體積小、編譯快，且根據 Apache 2.0 授權免費提供。不過，您也可以編譯許多其他模型。以下是一些建議：

| 模型 | Hugging Face ID | 參數數量 | 授權 | 備註 |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | 體積非常小，編譯快速，適合測試 |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | 品質更好，仍然快速編譯 |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | 高品質，需求更多 RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | 需在 Hugging Face 接受授權條款 |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | 高品質，下載更大且編譯時間較長 |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | 已在 Foundry Local 目錄中（比對用途有用）|

> **授權提醒：** 使用前務必在 Hugging Face 上查看模型授權。有些模型（例如 Llama）需要您接受授權協議並使用 `huggingface-cli login` 進行認證後才能下載。

---

## 概念：何時使用自訂模型

| 情境 | 為何要自行編譯？ |
|----------|----------------------|
| <strong>目錄中沒有您需要的模型</strong> | Foundry Local 目錄是經過策展的。如果您想要的模型未列出，請自行編譯。 |
| <strong>微調過的模型</strong> | 如果您有在特定領域資料上微調的模型，您需要編譯您自己的權重。 |
| <strong>特定量化需求</strong> | 您可能需要與目錄預設不同的精度或量化策略。 |
| <strong>較新的模型發布</strong> | 當 Hugging Face 發佈新模型時，可能尚未在 Foundry Local 目錄中。自行編譯可立即使用。 |
| <strong>研究和試驗</strong> | 在決定生產用模型前嘗試不同的模型架構、大小或配置。 |

---

## 總結

在本實驗中，您學到了如何：

| 步驟 | 您所做的事 |
|------|-------------|
| 1 | 安裝 ONNX Runtime GenAI 模型構建器 |
| 2 | 將 Hugging Face 上的 `Qwen/Qwen3-0.6B` 編譯成優化的 ONNX 模型 |
| 3 | 建立 `inference_model.json` 聊天模板設定檔 |
| 4 | 將編譯好的模型加入 Foundry Local 快取 |
| 5 | 透過 CLI 與自訂模型進行互動聊天 |
| 6 | 透過 OpenAI 相容的 REST API 查詢模型 |
| 7 | 使用 Python、JavaScript 和 C# 的 OpenAI SDK 連接模型 |
| 8 | 透過 Foundry Local SDK 進行自訂模型的端對端測試 |

主要心得是<strong>任何基於 Transformer 的模型都能經由 Foundry Local 執行</strong>，只要它已被編譯成 ONNX 格式。OpenAI 相容的 API 表示您現有的應用程式程式碼無需更動，只需要替換模型名稱。

---

## 重要要點整理

| 概念 | 詳情 |
|---------|--------|
| ONNX Runtime GenAI 模型構建器 | 一行命令即可將 Hugging Face 模型轉換成帶量化優化的 ONNX 格式 |
| ONNX 格式 | Foundry Local 需要帶 ONNX Runtime GenAI 配置的 ONNX 模型 |
| 聊天模板 | `inference_model.json` 檔案告訴 Foundry Local 如何為特定模型格式化提示詞 |
| 硬體目標 | 可根據硬體編譯為 CPU、NVIDIA GPU（CUDA）、DirectML（Windows GPU）或 WebGPU |
| 量化 | 低精度（int4）降低大小及加快速度，代價是些許精度損失；fp16 在 GPU 上仍保有高品質 |
| API 相容性 | 自訂模型使用與內建模型相同的 OpenAI 相容 API |
| Foundry Local SDK | SDK 自動處理服務啟動、端點發現與模型載入，適用於目錄和自訂模型 |

---

## 延伸閱讀

| 資源 | 連結 |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local 自訂模型指南 | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 模型系列 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive 文件 | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## 下一步

繼續至 [第 11 部分：使用本機模型呼叫工具](part11-tool-calling.md) ，學習如何讓本機模型調用外部函式。

[← 第 9 部分：Whisper 語音轉錄](part9-whisper-voice-transcription.md) | [第 11 部分：工具呼叫 →](part11-tool-calling.md)