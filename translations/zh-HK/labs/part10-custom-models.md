![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第 10 部分：使用自訂或 Hugging Face 模型於 Foundry Local

> **目標：** 將 Hugging Face 模型編譯為 Foundry Local 所需的優化 ONNX 格式，配置聊天模板，將其新增到本地快取，並透過 CLI、REST API 及 OpenAI SDK 進行推論。

## 概覽

Foundry Local 隨附經挑選的預編譯模型目錄，但您並不限於該清單。任何基於 transformer 的語言模型，只要在 [Hugging Face](https://huggingface.co/) 上可用（或以本地 PyTorch / Safetensors 格式儲存），皆可編譯成優化的 ONNX 模型並透過 Foundry Local 提供服務。

編譯流程使用 **ONNX Runtime GenAI Model Builder**，這是 `onnxruntime-genai` 套件內含的命令列工具。模型建構器會完成繁瑣工作：下載原始權重、將其轉換為 ONNX 格式、應用量化（int4、fp16、bf16），以及產生 Foundry Local 所需的配置檔（包括聊天模板和分詞器）。

在本實驗室中，您將編譯 Hugging Face 的 **Qwen/Qwen3-0.6B**，將其註冊到 Foundry Local，並全程在本地設備上與之聊天。

---

## 學習目標

完成本實驗室後，您將能夠：

- 解釋自訂模型編譯的重要性及使用時機
- 安裝 ONNX Runtime GenAI 模型建構器
- 透過單一指令將 Hugging Face 模型編譯為優化的 ONNX 格式
- 理解主要編譯參數（執行提供者、精度）
- 建立 `inference_model.json` 聊天模板配置檔
- 將編譯後模型新增至 Foundry Local 快取
- 使用 CLI、REST API 及 OpenAI SDK 執行自訂模型推論

---

## 先決條件

| 條件 | 詳細說明 |
|-------------|---------|
| **Foundry Local CLI** | 已安裝且加入您的 `PATH` ([第 1 部分](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI 模型建構器所需 |
| **pip** | Python 套件管理器 |
| <strong>磁碟空間</strong> | 來源及編譯模型檔案至少需 5 GB 可用空間 |
| **Hugging Face 帳戶** | 部分模型需先接受授權後方能下載。Qwen3-0.6B 採用 Apache 2.0 授權，開放自由取得。 |

---

## 環境設定

模型編譯需使用多個大型 Python 套件（PyTorch、ONNX Runtime GenAI、Transformers）。建議建立獨立虛擬環境，避免影響系統 Python 或其他專案。

```bash
# 從代碼庫根目錄開始
python -m venv .venv
```

啟用環境：

**Windows (PowerShell)：**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux：**
```bash
source .venv/bin/activate
```

升級 pip 避免相依性解析問題：

```bash
python -m pip install --upgrade pip
```

> **提示：** 若您已有先前實驗室建立的 `.venv`，可直接重用，啟用後即可繼續。

---

## 概念：編譯流程

Foundry Local 需要 ONNX 格式並搭配 ONNX Runtime GenAI 配置的模型。大多數 Hugging Face 開源模型皆以 PyTorch 或 Safetensors 權重形式發行，因此需先進行轉換。

![自訂模型編譯流程](../../../images/custom-model-pipeline.svg)

### 模型建構器會做什麼？

1. <strong>下載</strong> 來自 Hugging Face 的源模型（或從本地路徑讀取）。
2. <strong>轉換</strong> PyTorch / Safetensors 權重為 ONNX 格式。
3. <strong>量化</strong> 模型至較低精度（例如 int4），以減少記憶體占用並提升吞吐量。
4. <strong>產生</strong> ONNX Runtime GenAI 配置檔（`genai_config.json`）、聊天模板（`chat_template.jinja`）及所有分詞器檔案，方便 Foundry Local 載入與提供服務。

### ONNX Runtime GenAI Model Builder 與 Microsoft Olive 差異

您可能會見到 **Microsoft Olive** 作為另一模型優化工具的參考。兩者均能產生 ONNX 模型，但用途及取捨不同：

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| <strong>套件</strong> | `onnxruntime-genai` | `olive-ai` |
| <strong>主要目標</strong> | 針對 ONNX Runtime GenAI 推論，轉換並量化生成式 AI 模型 | 支援多後端及硬體目標的端對端模型優化框架 |
| <strong>易用性</strong> | 單指令完成轉換與量化 | 工作流程導向，可透過 YAML/JSON 配置多階段管線 |
| <strong>輸出格式</strong> | ONNX Runtime GenAI 格式（適用 Foundry Local） | 通用 ONNX、ONNX Runtime GenAI 或依流程變換格式 |
| <strong>支援硬體</strong> | CPU、CUDA、DirectML、TensorRT RTX、WebGPU | CPU、CUDA、DirectML、TensorRT、Qualcomm QNN 等 |
| <strong>量化選項</strong> | int4、int8、fp16、fp32 | int4 (AWQ, GPTQ, RTN)、int8、fp16，還有圖優化、分層調優 |
| <strong>模型範圍</strong> | 生成式 AI 模型（大型及小型語言模型） | 任何 ONNX 可轉模型（影像、NLP、音訊、多模態） |
| <strong>最佳用途</strong> | 快速、單模型編譯以供本地推論 | 生產用管線需精細調校與最佳化 |
| <strong>相依性大小</strong> | 中等（PyTorch、Transformers、ONNX Runtime） | 較大（包含 Olive 框架及可選流程附加套件） |
| **Foundry Local 整合** | 直接相容，可立即使用 | 需 `--use_ort_genai` 參數及額外配置 |

> **為何本實驗室使用 Model Builder：** 若只單純對一個 Hugging Face 模型做編譯並註冊 Foundry Local，Model Builder 是最簡便且穩定的方案，一個指令即可產出 Foundry Local 需要的格式。如未來需精準的準確度量化、圖形變更或多階段調整，Olive 是強大選擇。詳情請參閱 [Microsoft Olive 文件](https://microsoft.github.io/Olive/)。

---

## 實驗練習

### 練習 1：安裝 ONNX Runtime GenAI Model Builder

安裝包含模型建構器工具的 ONNX Runtime GenAI 套件：

```bash
pip install onnxruntime-genai
```

驗證安裝是否成功，確認模型建構器可用：

```bash
python -m onnxruntime_genai.models.builder --help
```

您應會看到參數說明，例如 `-m`（模型名稱）、`-o`（輸出路徑）、`-p`（精度）、`-e`（執行提供者）等。

> **注意：** 模型建構器會依賴 PyTorch、Transformers 及其他多個套件，安裝過程可能需數分鐘。

---

### 練習 2：為 CPU 編譯 Qwen3-0.6B

執行下列指令，下載並編譯 Qwen3-0.6B 模型，使用 CPU 及 int4 量化：

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

| 參數 | 作用 | 使用值 |
|-----------|---------|------------|
| `-m` | Hugging Face 模型 ID 或本地目錄路徑 | `Qwen/Qwen3-0.6B` |
| `-o` | 編譯後 ONNX 模型儲存目錄 | `models/qwen3` |
| `-p` | 編譯時應用的量化精度 | `int4` |
| `-e` | ONNX Runtime 執行提供者（目標硬體） | `cpu` |
| `--extra_options hf_token=false` | 跳過 Hugging Face 認證（公開模型可用） | `hf_token=false` |

> **需要花多長時間？** 編譯時間依硬體與模型大小而異。Qwen3-0.6B 於現代 CPU 使用 int4 量化約需 5 至 15 分鐘。模型越大時間越長。

指令完成後，目錄 `models/qwen3` 應會出現編譯模型檔案。檢查輸出：

```bash
ls models/qwen3
```

您應會見到以下檔案：
- `model.onnx` 和 `model.onnx.data` — 編譯後的模型權重
- `genai_config.json` — ONNX Runtime GenAI 配置
- `chat_template.jinja` — 模型聊天模板（自動生成）
- `tokenizer.json`、`tokenizer_config.json` — 分詞器檔案
- 其他語彙與配置相關檔案

---

### 練習 3：為 GPU 編譯（選擇性）

若您有 NVIDIA GPU 且支援 CUDA，可編譯針對 GPU 優化的版本以加快推論速度：

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **注意：** GPU 編譯需安裝 `onnxruntime-gpu` 且 CUDA 環境正常。若缺少其中，模型建構器會報錯。您可跳過此步，繼續使用 CPU 版本。

#### 硬體特定編譯參考

| 目標 | 執行提供者 (`-e`) | 建議精度 (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` 或 `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` 或 `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### 精度權衡

| 精度 | 體積 | 速度 | 品質 |
|-----------|------|-------|---------|
| `fp32` | 最大 | 最慢 | 最高準確度 |
| `fp16` | 大 | 快（GPU） | 非常好準確度 |
| `int8` | 小 | 快 | 輕微準確度流失 |
| `int4` | 最小 | 最快 | 中度準確度流失 |

對多數本地開發，CPU 上的 `int4` 是速度與資源使用的最佳平衡。若需求生產等級品質，建議 CUDA GPU 上使用 `fp16`。

---

### 練習 4：建立聊天模板配置

模型建構器會在輸出目錄自動生產 `chat_template.jinja` 與 `genai_config.json` 檔案，但 Foundry Local 仍需 `inference_model.json` 檔案，告知如何格式化您的模型提示詞。此檔定義模型名稱與將使用者訊息包裹於正確特殊標記的提示模板。

#### 第 1 步：檢查編譯輸出

列出編譯模型目錄內容：

```bash
ls models/qwen3
```

您應會見到例如：
- `model.onnx` 與 `model.onnx.data` — 編譯模型權重
- `genai_config.json` — ONNX Runtime GenAI 配置（自動生成）
- `chat_template.jinja` — 模型聊天模板（自動生成）
- `tokenizer.json`、`tokenizer_config.json` — 分詞器檔案
- 其他配置與詞彙檔案

#### 第 2 步：產生 inference_model.json 檔案

建立 Python 腳本 `generate_chat_template.py`，放在專案根目錄（與含有 `models/` 的資料夾同層）：

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

於專案根目錄執行腳本：

```bash
python generate_chat_template.py
```

> **注意：** `transformers` 套件為 `onnxruntime-genai` 相依項目已安裝。如出現 `ImportError`，請先執行 `pip install transformers`。

腳本會在 `models/qwen3` 目錄下產生 `inference_model.json` 檔，告知 Foundry Local 對 Qwen3 如何將使用者輸入包裹成適當特殊標記。

> **重要提示：** `inference_model.json` 內的 `"Name"` 欄位（本腳本為 `qwen3-0.6b`）為模型別名，您在後續所有指令及 API 呼叫中都會用到。若更改名稱，請同步更新練習 6 至 10 中的模型名稱。

#### 第 3 步：確認配置

打開 `models/qwen3/inference_model.json`，確認其中包含 `Name` 欄位及 `PromptTemplate` 物件，裡面有 `assistant` 與 `prompt` 鍵。提示模板應包含類似 `<|im_start|>` 與 `<|im_end|>` 等特殊標記（具體內容依模型聊天模板而異）。

> **手動替代方法：** 若不想執行腳本，也可自行建立檔案。關鍵是 `prompt` 欄位需包含完整聊天模板，並以 `{Content}` 占位符表示使用者訊息。

---

### 練習 5：確認模型目錄結構
模型構建器會將所有編譯後的檔案直接放入你指定的輸出目錄中。請確認最終結構是否正確：

```bash
ls models/qwen3
```

該目錄應包含以下檔案：

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

> **注意：** 與其他某些編譯工具不同，模型構建器不會建立巢狀子目錄。所有檔案直接置於輸出資料夾中，這正是 Foundry Local 預期的結構。

---

### 練習 6：將模型加入 Foundry Local 快取

告訴 Foundry Local 在哪裡找到你編譯過的模型，方法是將目錄新增至其快取中：

```bash
foundry cache cd models/qwen3
```

確認模型已顯示在快取中：

```bash
foundry cache ls
```

你應該會看到你的自訂模型列在任何先前快取的模型旁邊（例如 `phi-3.5-mini` 或 `phi-4-mini`）。

---

### 練習 7：使用 CLI 執行自訂模型

啟動一個與你新編譯模型的互動聊天會話（`qwen3-0.6b` 別名來自你在 `inference_model.json` 中設定的 `Name` 欄位）：

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` 參數會顯示更多診斷資訊，對第一次測試自訂模型非常有用。如果模型成功載入，你將看到一個互動提示。試著輸入幾則訊息：

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

輸入 `exit` 或按下 `Ctrl+C` 結束會話。

> **疑難排解：** 如果模型載入失敗，請檢查以下項目：
> - `genai_config.json` 檔案確實由模型構建器產生。
> - `inference_model.json` 檔案存在且為有效的 JSON。
> - ONNX 模型檔位於正確目錄。
> - 你有足夠的可用 RAM（Qwen3-0.6B int4 約需 1 GB）。
> - Qwen3 是一個會產生 `<think>` 標籤的推理模型。如果你看到回應前綴有 `<think>...</think>`，這是正常現象。你可以調整 `inference_model.json` 中的提示模板，以抑制思考輸出。

---

### 練習 8：透過 REST API 查詢自訂模型

如果你在練習 7 中已結束互動會話，模型可能已不在載入狀態。請先啟動 Foundry Local 服務並載入模型：

```bash
foundry service start
foundry model load qwen3-0.6b
```

查看服務正在使用的埠號：

```bash
foundry service status
```

然後發送請求（若埠號不同，請替換 `5273`）：

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows 注意事項：** 上述 `curl` 指令使用 bash 語法。在 Windows 上，請改用 PowerShell `Invoke-RestMethod` 指令，如下所示。

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

你可以使用與內建模型完全相同的 OpenAI SDK 程式碼連接你的自訂模型（請參閱[第 3 部分](part3-sdk-and-apis.md)）。唯一的差別是模型名稱。

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
  apiKey: "foundry-local", // Foundry Local 不會驗證 API 密鑰
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

> **重點提示：** 因為 Foundry Local 提供與 OpenAI 相容的 API，任何能用於內建模型的程式碼也同樣適用於你的自訂模型。你只需更改 `model` 參數即可。

---

### 練習 10：使用 Foundry Local SDK 測試自訂模型

在前面的實驗中，你已使用 Foundry Local SDK 啟動服務、發現端點、並自動管理模型。你可以用完全相同的方式處理自訂編譯模型。SDK 負責服務啟動和端點發現，因此你的程式碼無需硬編碼 `localhost:5273`。

> **注意：** 在執行這些範例前，請確保已安裝 Foundry Local SDK：
> - **Python：** `pip install foundry-local openai`
> - **JavaScript：** `npm install foundry-local-sdk openai`
> - **C#：** 安裝 `Microsoft.AI.Foundry.Local` 與 `OpenAI` NuGet 套件
>
> 請將每個腳本檔案 <strong>保存在儲存庫根目錄</strong>（與你的 `models/` 資料夾同目錄）下。

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

# 第二步：檢查緩存中的自訂模型
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# 第三步：將模型加載到記憶體中
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

// 第四步：使用 SDK 發現的端點建立 OpenAI 客戶端
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

> **重點提示：** Foundry Local SDK 動態發現端點，因此你永遠不需硬編碼埠號。這是生產環境的建議作法。你的自訂編譯模型透過 SDK 的行為與內建目錄模型完全相同。

---

## 選擇編譯的模型

本實驗以 Qwen3-0.6B 作為參考範例，因為它體積小、編譯速度快，且在 Apache 2.0 授權下免費提供。不過你也可以編譯許多其他模型，以下是一些建議：

| 模型 | Hugging Face ID | 參數數量 | 授權 | 備註 |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | 體積非常小，編譯快速，適合測試 |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | 品質更佳，仍快速編譯 |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | 品質強勁，需要更多 RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | 需於 Hugging Face 接受授權條款 |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | 高品質，下載較大、編譯較久 |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | 已包含在 Foundry Local 目錄中（便於比較） |

> **授權提醒：** 使用前請務必在 Hugging Face 查閱模型授權。部分模型（如 Llama）需先同意授權協議並使用 `huggingface-cli login` 驗證後才能下載。

---

## 概念：何時使用自訂模型

| 情境 | 為什麼要自編譯？ |
|----------|----------------------|
| <strong>你需要的模型不在目錄中</strong> | Foundry Local 目錄是經過策劃的。如果你想要的模型未列出，就自行編譯。 |
| <strong>微調模型</strong> | 若你已針對特定領域數據微調模型，需要編譯自己的權重。 |
| <strong>特定量化需求</strong> | 你可能想要不同於目錄預設的精度或量化策略。 |
| <strong>新版模型發佈</strong> | 新模型在 Hugging Face 釋出時，可能尚未納入 Foundry Local 目錄。自行編譯即可立即使用。 |
| <strong>研究與實驗</strong> | 在做生產決策前，先在本地嘗試不同模型架構、規模或設定。 |

---

## 小結

本實驗你學會了：

| 步驟 | 內容 |
|------|-------------|
| 1 | 安裝 ONNX Runtime GenAI 模型構建器 |
| 2 | 將 Hugging Face 上的 `Qwen/Qwen3-0.6B` 編譯成最佳化 ONNX 模型 |
| 3 | 建立 `inference_model.json` 聊天模板配置檔 |
| 4 | 將編譯完成的模型加入 Foundry Local 快取 |
| 5 | 透過 CLI 執行自訂模型的互動聊天 |
| 6 | 透過 OpenAI 相容的 REST API 查詢模型 |
| 7 | 從 Python、JavaScript 及 C# 以 OpenAI SDK 連接 |
| 8 | 利用 Foundry Local SDK 進行自訂模型的端對端測試 |

關鍵重點是，一旦轉換成 ONNX 格式，**任何基於 Transformer 的模型都能在 Foundry Local 上運行**。OpenAI 相容的 API 意味著你現有的應用程式碼不需改動，只需替換模型名稱。

---

## 重要重點整理

| 概念 | 詳情 |
|---------|--------|
| ONNX Runtime GenAI 模型構建器 | 一個命令即可將 Hugging Face 模型轉為帶量化的 ONNX 格式 |
| ONNX 格式 | Foundry Local 需要帶有 ONNX Runtime GenAI 配置的 ONNX 模型 |
| 聊天模板 | `inference_model.json` 檔告訴 Foundry Local 如何為該模型格式化提示 |
| 硬體目標 | 可為 CPU、NVIDIA GPU（CUDA）、DirectML（Windows GPU）或 WebGPU 編譯，視硬體而定 |
| 量化 | 較低精度（int4）減少大小並提高速度，但會犧牲部分準確度；fp16 在 GPU 上保有高品質 |
| API 相容性 | 自訂模型與內建模型使用相同的 OpenAI 相容 API |
| Foundry Local SDK | SDK 自動管理服務啟動、端點發現及模型載入，適用目錄和自訂模型 |

---

## 延伸閱讀

| 資源 | 連結 |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local 自訂模型指南 | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 模型系列 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive 文件 | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## 後續步驟

繼續閱讀[第 11 部分：使用本地模型調用工具](part11-tool-calling.md)，了解如何啟用你的本地模型調用外部函式。

[← 第 9 部分：Whisper 語音轉錄](part9-whisper-voice-transcription.md) | [第 11 部分：調用工具 →](part11-tool-calling.md)