![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 10: Foundry Local에서 사용자 지정 또는 Hugging Face 모델 사용하기

> **목표:** Hugging Face 모델을 Foundry Local에서 요구하는 최적화된 ONNX 형식으로 컴파일하고, 채팅 템플릿으로 구성한 후, 로컬 캐시에 추가하고, CLI, REST API 및 OpenAI SDK를 사용하여 추론을 실행합니다.

## 개요

Foundry Local은 사전 컴파일된 모델 카탈로그를 제공하지만, 해당 목록에 국한되지 않습니다. [Hugging Face](https://huggingface.co/)에서 제공되는 트랜스포머 기반 언어 모델(또는 로컬에 PyTorch / Safetensors 형식으로 저장된 모델)은 최적화된 ONNX 모델로 컴파일되어 Foundry Local을 통해 서비스할 수 있습니다.

컴파일 파이프라인은 `onnxruntime-genai` 패키지에 포함된 커맨드라인 도구인 <strong>ONNX Runtime GenAI Model Builder</strong>를 사용합니다. 모델 빌더는 소스 가중치를 다운로드하고, ONNX 형식으로 변환하며, 양자화를 적용(int4, fp16, bf16)하고, Foundry Local에서 기대하는 구성 파일(채팅 템플릿 및 토크나이저 포함)을 생성하는 등의 무거운 작업을 처리합니다.

이 실습에서는 Hugging Face의 <strong>Qwen/Qwen3-0.6B</strong>를 컴파일하여 Foundry Local에 등록하고, 완전히 로컬 장치에서 대화하는 방법을 배웁니다.

---

## 학습 목표

이 실습을 완료하면 다음을 수행할 수 있습니다:

- 사용자 지정 모델 컴파일이 왜 유용하며 언제 필요한지 설명하기
- ONNX Runtime GenAI 모델 빌더 설치하기
- 단일 명령으로 Hugging Face 모델을 최적화된 ONNX 형식으로 컴파일하기
- 주요 컴파일 매개변수(실행 공급자, 정밀도) 이해하기
- `inference_model.json` 채팅 템플릿 구성 파일 생성하기
- 컴파일된 모델을 Foundry Local 캐시에 추가하기
- CLI, REST API, OpenAI SDK를 사용하여 사용자 지정 모델에 대한 추론 실행하기

---

## 사전 요구 사항

| 요구사항 | 세부정보 |
|-------------|---------|
| **Foundry Local CLI** | 설치되어 있고 `PATH`에 등록되어 있어야 함 ([Part 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI 모델 빌더에 필요 |
| **pip** | Python 패키지 관리자 |
| **디스크 공간** | 소스 및 컴파일된 모델 파일을 위해 최소 5GB 여유 공간 |
| **Hugging Face 계정** | 일부 모델은 라이선스 수락 후 다운로드 가능. Qwen3-0.6B는 Apache 2.0 라이선스로 자유롭게 사용 가능. |

---

## 환경 설정

모델 컴파일에는 PyTorch, ONNX Runtime GenAI, Transformers 등 대형 Python 패키지 여러 개가 필요합니다. 시스템 Python이나 다른 프로젝트와 충돌하지 않도록 별도의 가상 환경을 만드세요.

```bash
# 저장소 루트에서
python -m venv .venv
```
  
가상 환경 활성화:

**Windows (PowerShell):**  
```powershell
.venv\Scripts\Activate.ps1
```
  
**macOS / Linux:**  
```bash
source .venv/bin/activate
```
  
의존성 문제 방지를 위해 pip 업그레이드:

```bash
python -m pip install --upgrade pip
```
  
> **팁:** 이전 실습에서 이미 `.venv`가 있다면 재사용할 수 있습니다. 다만, 계속하기 전에 반드시 활성화되어 있어야 합니다.

---

## 개념: 컴파일 파이프라인

Foundry Local은 ONNX Runtime GenAI 구성을 갖춘 ONNX 형식의 모델을 요구합니다. 대부분 Hugging Face의 오픈소스 모델은 PyTorch 또는 Safetensors 가중치로 배포되어 변환 단계가 필요합니다.

![사용자 지정 모델 컴파일 파이프라인](../../../images/custom-model-pipeline.svg)

### 모델 빌더는 무엇을 하는가?

1. Hugging Face에서 소스 모델을 다운로드(또는 로컬 경로에서 읽음)  
2. PyTorch / Safetensors 가중치를 ONNX 형식으로 변환  
3. 메모리 사용량 감소 및 처리량 향상을 위해 모델을 더 작은 정밀도(int4 등)로 양자화  
4. ONNX Runtime GenAI 구성(`genai_config.json`), 채팅 템플릿(`chat_template.jinja`), 그리고 Foundry Local에서 모델을 로드 및 제공하는 데 필요한 모든 토크나이저 파일 생성

### ONNX Runtime GenAI 모델 빌더 vs Microsoft Olive

모델 최적화를 위한 대안 도구로 <strong>Microsoft Olive</strong>가 언급될 수 있습니다. 두 도구 모두 ONNX 모델을 생성하지만, 목적과 특성에 차이가 있습니다:

|  | **ONNX Runtime GenAI 모델 빌더** | **Microsoft Olive** |
|---|---|---|
| <strong>패키지</strong> | `onnxruntime-genai` | `olive-ai` |
| <strong>주용도</strong> | ONNX Runtime GenAI 추론용 생성 AI 모델 변환 및 양자화 | 다양한 백엔드와 하드웨어 대상 지원하는 종합 모델 최적화 프레임워크 |
| **사용 편의성** | 단일 명령어 — 변환+양자화 일괄 처리 | 워크플로 기반 — YAML/JSON으로 설정하는 다중 단계 파이프라인 |
| **출력 형식** | Foundry Local에 바로 쓸 수 있는 ONNX Runtime GenAI 형식 | 일반 ONNX, ONNX Runtime GenAI 또는 워크플로에 따른 기타 형식 |
| **하드웨어 대상** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN 등 다양함 |
| **양자화 옵션** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, 그래프 최적화, 레이어별 튜닝 등 |
| **대상 모델 범위** | 생성 AI 모델(LLM, SLM 등) | ONNX로 변환 가능한 모든 모델(비전, NLP, 오디오, 멀티모달 등) |
| **적합 사례** | 로컬 추론용 단일 모델 빠른 컴파일 | 세밀한 최적화 제어가 필요한 프로덕션 파이프라인 |
| **종속성 규모** | 중간(Pytorch, Transformers, ONNX Runtime 포함) | 큼(Olive 프레임워크 및 워크플로 별 선택적 추가 종속성) |
| **Foundry Local 연동** | 직접적 — 바로 호환되는 출력 생성 | `--use_ort_genai` 플래그 및 추가 설정 필요 |

> **본 실습에서 모델 빌더 사용하는 이유:** 단일 Hugging Face 모델을 컴파일하고 Foundry Local에 등록하는 데 가장 간단하고 신뢰할 수 있는 경로입니다. 한 명령어로 Foundry Local에서 예상하는 정확한 출력 형식을 생성합니다. 향후 정확도 인지 양자화, 그래프 수술, 다중 단계 튜닝과 같은 고급 최적화가 필요하면 Olive가 강력한 대안입니다. 자세한 내용은 [Microsoft Olive 문서](https://microsoft.github.io/Olive/)를 참조하세요.

---

## 실습

### 실습 1: ONNX Runtime GenAI 모델 빌더 설치

모델 빌더 도구가 포함된 ONNX Runtime GenAI 패키지를 설치합니다:

```bash
pip install onnxruntime-genai
```
  
설치가 제대로 되었는지 모델 빌더가 실행되는지 확인:

```bash
python -m onnxruntime_genai.models.builder --help
```
  
`-m` (모델 이름), `-o` (출력 경로), `-p` (정밀도), `-e` (실행 공급자) 같은 매개변수를 나열하는 도움말 출력이 보여야 합니다.

> **참고:** 모델 빌더는 PyTorch, Transformers 등 몇 가지 패키지에 의존합니다. 설치에 몇 분 소요될 수 있습니다.

---

### 실습 2: Qwen3-0.6B CPU용 컴파일

다음 명령어로 Hugging Face에서 Qwen3-0.6B 모델을 다운로드하고 CPU용 int4 양자화 컴파일을 진행합니다:

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
  
#### 각 매개변수 설명

| 매개변수 | 용도 | 값 |
|-----------|---------|------------|
| `-m` | Hugging Face 모델 ID 또는 로컬 디렉토리 경로 | `Qwen/Qwen3-0.6B` |
| `-o` | 컴파일된 ONNX 모델이 저장될 디렉토리 | `models/qwen3` |
| `-p` | 컴파일 중 적용할 양자화 정밀도 | `int4` |
| `-e` | ONNX Runtime 실행 공급자 (타겟 하드웨어) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face 인증 건너뜀 (공개 모델에 적합) | `hf_token=false` |

> **소요 시간은?** 컴파일 시간은 하드웨어와 모델 크기에 따라 다릅니다. Qwen3-0.6B의 CPU int4 양자화는 최신 CPU 기준 대략 5~15분 소요. 더 큰 모델은 더 오래 걸립니다.

명령이 완료되면 `models/qwen3` 디렉토리에 컴파일된 모델 파일이 생성됩니다. 출력 내용을 확인:

```bash
ls models/qwen3
```
  
다음 파일들을 확인할 수 있습니다:  
- `model.onnx` 및 `model.onnx.data` — 컴파일된 모델 가중치  
- `genai_config.json` — ONNX Runtime GenAI 구성  
- `chat_template.jinja` — 모델의 채팅 템플릿 (자동 생성)  
- `tokenizer.json`, `tokenizer_config.json` — 토크나이저 파일  
- 기타 어휘 및 구성 파일들

---

### 실습 3: GPU용 컴파일 (선택 사항)

CUDA 지원 NVIDIA GPU가 있다면, 더 빠른 추론을 위해 GPU 최적화 버전을 컴파일할 수 있습니다:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```
  
> **참고:** GPU 컴파일은 `onnxruntime-gpu`와 작동하는 CUDA 설치가 필요합니다. 없으면 모델 빌더가 오류를 보고합니다. 이 경우 이 실습을 건너뛰고 CPU 버전으로 계속하세요.

#### 하드웨어 별 컴파일 참조

| 타겟 | 실행 공급자 (`-e`) | 권장 정밀도 (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` 또는 `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` 또는 `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### 정밀도에 따른 트레이드오프

| 정밀도 | 크기 | 속도 | 품질 |
|-----------|------|-------|---------|
| `fp32` | 가장 큼 | 가장 느림 | 최고 정확도 |
| `fp16` | 큼 | 빠름 (GPU) | 매우 우수 정확도 |
| `int8` | 작음 | 빠름 | 약간 정확도 손실 |
| `int4` | 가장 작음 | 가장 빠름 | 보통 정확도 손실 |

대부분 로컬 개발에서는 CPU의 `int4`가 속도와 리소스 사용의 균형에 가장 적합합니다. 프로덕션급 결과물은 CUDA GPU의 `fp16` 사용을 권장합니다.

---

### 실습 4: 채팅 템플릿 구성 생성

모델 빌더가 출력 디렉토리에 `chat_template.jinja`와 `genai_config.json` 파일을 자동 생성하지만, Foundry Local은 프롬프트를 올바른 형식으로 만들기 위해 `inference_model.json` 파일도 필요합니다. 이 파일은 모델 이름과 사용자 메시지를 올바른 특수 토큰으로 감싸는 프롬프트 템플릿을 정의합니다.

#### 1단계: 컴파일된 출력 확인

컴파일된 모델 디렉토리 내용을 나열하세요:

```bash
ls models/qwen3
```
  
다음 파일들이 보여야 합니다:  
- `model.onnx` 및 `model.onnx.data` — 컴파일된 가중치  
- `genai_config.json` — ONNX Runtime GenAI 구성 (자동 생성)  
- `chat_template.jinja` — 모델 채팅 템플릿 (자동 생성)  
- `tokenizer.json`, `tokenizer_config.json` — 토크나이저 파일  
- 기타 구성 및 어휘 파일

#### 2단계: inference_model.json 파일 생성

`inference_model.json` 파일은 Foundry Local이 프롬프트를 어떻게 포맷할지 알려줍니다. 저장소 루트(예: `models/` 폴더가 있는 같은 디렉토리)에 `generate_chat_template.py`라는 Python 스크립트를 만드세요:

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# 채팅 템플릿을 추출하기 위한 최소 대화 구성
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

# inference_model.json 구조 구축
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
  
저장소 루트에서 스크립트를 실행:

```bash
python generate_chat_template.py
```
  
> **참고:** `transformers` 패키지는 `onnxruntime-genai` 설치 시 이미 함께 설치됩니다. `ImportError`가 발생하면 먼저 `pip install transformers`를 실행하세요.

스크립트가 `models/qwen3` 디렉토리에 `inference_model.json` 파일을 생성합니다. 이 파일은 Foundry Local에서 Qwen3 사용자를 올바른 특수 토큰으로 감싸도록 지시합니다.

> **중요:** `inference_model.json`의 `"Name"` 필드(스크립트에서는 `qwen3-0.6b`로 설정됨)는 이후 모든 명령어와 API 호출에서 사용할 <strong>모델 별칭</strong>입니다. 이 이름을 바꾸면 실습 6~10에서도 해당 모델 이름을 변경해 주세요.

#### 3단계: 구성 확인

`models/qwen3/inference_model.json`을 열어 `Name` 필드 및 `assistant`와 `prompt` 키를 가진 `PromptTemplate` 객체가 있는지 확인하세요. 프롬프트 템플릿에는 `<|im_start|>`, `<|im_end|>` 같은 특수 토큰이 포함되어 있어야 합니다(정확한 토큰은 모델의 채팅 템플릿에 따라 다름).

> **수동 생성 대안:** 스크립트를 실행하지 않으려면 직접 생성할 수도 있습니다. 핵심은 `prompt` 필드에 `{Content}` 자리 표시자로 사용자 메시지를 감싸는 모델의 전체 채팅 템플릿을 넣는 것입니다.

---

### 실습 5: 모델 디렉토리 구조 확인


모델 빌더는 모든 컴파일된 파일을 지정한 출력 디렉터리에 직접 배치합니다. 최종 구조가 올바른지 확인하세요:

```bash
ls models/qwen3
```

디렉터리에는 다음 파일들이 포함되어야 합니다:

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

> **참고:** 일부 다른 컴파일 도구와 달리, 모델 빌더는 중첩된 하위 디렉터리를 생성하지 않습니다. 모든 파일은 출력 폴더에 직접 위치하며, 이는 Foundry Local이 기대하는 방식입니다.

---

### 연습 6: 모델을 Foundry Local 캐시에 추가하기

컴파일된 모델을 찾을 수 있도록 디렉터리를 Foundry Local 캐시에 추가하세요:

```bash
foundry cache cd models/qwen3
```

모델이 캐시에 나타나는지 확인하세요:

```bash
foundry cache ls
```

`phi-3.5-mini` 또는 `phi-4-mini`와 같은 이전에 캐시된 모델 옆에 사용자 지정 모델이 표시되어야 합니다.

---

### 연습 7: CLI로 사용자 지정 모델 실행하기

새로 컴파일한 모델(`qwen3-0.6b` 별칭은 `inference_model.json`의 `Name` 필드에서 설정한 값입니다)과 함께 대화형 채팅 세션을 시작하세요:

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` 플래그는 추가 진단 정보를 보여주며, 처음 사용자 지정 모델을 테스트할 때 유용합니다. 모델이 성공적으로 로드되면 대화형 프롬프트가 표시됩니다. 몇 가지 메시지를 시도해보세요:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

세션을 종료하려면 `exit`를 입력하거나 `Ctrl+C`를 누르세요.

> **문제 해결:** 모델 로드에 실패하면 다음 사항을 확인하세요:
> - `genai_config.json` 파일이 모델 빌더에 의해 생성되었는지
> - `inference_model.json` 파일이 존재하며 유효한 JSON인지
> - ONNX 모델 파일이 올바른 디렉터리에 있는지
> - 충분한 사용 가능한 RAM이 있는지(Qwen3-0.6B int4는 약 1GB 필요)
> - Qwen3는 `<think>` 태그를 생성하는 추론 모델입니다. 응답 앞에 `<think>...</think>`가 표시되면 정상 동작입니다. `inference_model.json`의 프롬프트 템플릿을 조정하여 생각 출력 억제가 가능합니다.

---

### 연습 8: REST API를 통해 사용자 지정 모델 쿼리하기

연습 7에서 대화형 세션을 종료했다면 모델이 더 이상 로드되지 않을 수 있습니다. 먼저 Foundry Local 서비스를 시작하고 모델을 로드하세요:

```bash
foundry service start
foundry model load qwen3-0.6b
```

서비스가 실행 중인 포트를 확인하세요:

```bash
foundry service status
```

그런 다음 요청을 전송하세요(`5273`을 실제 포트로 변경):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows 참고:** 위 `curl` 명령은 bash 문법입니다. Windows에서는 다음 PowerShell `Invoke-RestMethod` cmdlet을 사용하세요.

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

### 연습 9: OpenAI SDK로 사용자 지정 모델 사용하기

내장 모델에 사용한 것과 동일한 OpenAI SDK 코드를 사용해 사용자 지정 모델에 연결할 수 있습니다([3부](part3-sdk-and-apis.md) 참조). 유일한 차이는 모델 이름입니다.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local은 API 키를 검증하지 않습니다
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
  apiKey: "foundry-local", // Foundry Local은 API 키를 검증하지 않습니다
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

> **핵심:** Foundry Local이 OpenAI 호환 API를 노출하므로 내장 모델에서 작동하는 모든 코드는 사용자 지정 모델에서도 작동합니다. `model` 매개변수만 변경하면 됩니다.

---

### 연습 10: Foundry Local SDK로 사용자 지정 모델 테스트하기

이전 실습에서 Foundry Local SDK를 사용해 서비스를 시작하고, 엔드포인트를 검색하며, 모델을 자동으로 관리했습니다. 동일한 패턴을 사용자 지정 컴파일 모델에도 적용할 수 있습니다. SDK는 서비스 시작과 엔드포인트 검색을 처리하므로 코드에 `localhost:5273`을 하드코딩할 필요가 없습니다.

> **참고:** 다음 예제 실행 전에 Foundry Local SDK가 설치되어 있는지 확인하세요:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` 및 `OpenAI` NuGet 패키지를 추가하세요
>
> 각 스크립트 파일은 **저장소 루트에** 저장하세요(`models/` 폴더와 같은 디렉터리).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# 1단계: Foundry Local 서비스를 시작하고 사용자 정의 모델을 로드합니다
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# 2단계: 캐시에서 사용자 정의 모델을 확인합니다
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# 3단계: 모델을 메모리에 로드합니다
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# 4단계: SDK가 발견한 엔드포인트를 사용하여 OpenAI 클라이언트를 생성합니다
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# 5단계: 스트리밍 채팅 완료 요청을 보냅니다
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

실행하세요:

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

// 1단계: Foundry Local 서비스를 시작합니다
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 2단계: 카탈로그에서 사용자 정의 모델을 가져옵니다
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// 3단계: 모델을 메모리에 로드합니다
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// 4단계: SDK가 찾은 엔드포인트를 사용하여 OpenAI 클라이언트를 생성합니다
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// 5단계: 스트리밍 채팅 완성 요청을 전송합니다
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

실행하세요:

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

> **핵심:** Foundry Local SDK는 엔드포인트를 동적으로 검색하므로 포트 번호를 하드코딩하지 않습니다. 이는 프로덕션 애플리케이션에 권장되는 방법입니다. 사용자 지정 컴파일 모델은 SDK를 통해 내장 카탈로그 모델과 동일하게 작동합니다.

---

## 컴파일할 모델 선택하기

Qwen3-0.6B는 이 실습에서 참조 예제로 사용되었는데, 크기가 작고 컴파일 속도가 빠르며 Apache 2.0 라이선스로 자유롭게 사용할 수 있기 때문입니다. 그러나 다른 많은 모델도 컴파일할 수 있습니다. 다음은 몇 가지 제안입니다:

| 모델 | Hugging Face ID | 파라미터 수 | 라이선스 | 비고 |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | 매우 작고, 빠른 컴파일, 테스트용 적합 |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | 품질 개선, 여전히 빠른 컴파일 |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | 고품질, 더 많은 RAM 필요 |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face에서 라이선스 수락 필요 |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | 고품질, 더 큰 다운로드 및 긴 컴파일 시간 |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Foundry Local 카탈로그에 이미 포함(비교용으로 유용) |

> **라이선스 주의:** 항상 Hugging Face에서 모델의 라이선스를 확인하세요. Llama와 같은 일부 모델은 다운로드 전에 라이선스 동의와 `huggingface-cli login` 인증이 필요합니다.

---

## 개념: 사용자 지정 모델을 사용할 때

| 시나리오 | 왜 직접 컴파일해야 하는가? |
|----------|---------------------------|
| **필요한 모델이 카탈로그에 없을 때** | Foundry Local 카탈로그는 엄선되어 있습니다. 원하는 모델이 없으면 직접 컴파일하세요. |
| **미세 조정된 모델** | 도메인 특화 데이터로 미세 조정된 모델은 가중치를 직접 컴파일해야 합니다. |
| **특정 양자화 요구사항** | 카탈로그 기본값과 다른 정밀도 또는 양자화 전략을 원할 수 있습니다. |
| **최신 모델 출시** | Hugging Face에서 새 모델이 출시되면 Foundry Local 카탈로그에 아직 없을 수 있습니다. 직접 컴파일하여 즉시 사용하세요. |
| **연구 및 실험** | 프로덕션 선택 전에 로컬에서 다양한 아키텍처, 크기, 구성을 시험해볼 수 있습니다. |

---

## 요약

이 실습에서 배운 내용은 다음과 같습니다:

| 단계 | 수행 내용 |
|------|----------|
| 1 | ONNX Runtime GenAI 모델 빌더 설치 |
| 2 | Hugging Face의 `Qwen/Qwen3-0.6B`를 최적화된 ONNX 모델로 컴파일 |
| 3 | `inference_model.json` 채팅 템플릿 구성 파일 생성 |
| 4 | 컴파일된 모델을 Foundry Local 캐시에 추가 |
| 5 | CLI를 통해 사용자 지정 모델로 대화형 채팅 실행 |
| 6 | OpenAI 호환 REST API로 모델 쿼리 |
| 7 | Python, JavaScript, C#에서 OpenAI SDK를 사용해 연결 |
| 8 | Foundry Local SDK로 사용자 지정 모델을 엔드 투 엔드 테스트 |

핵심은 <strong>트랜스포머 기반 모델은 모두 ONNX 형식으로 컴파일하면 Foundry Local에서 실행 가능</strong>하다는 점입니다. OpenAI 호환 API 덕분에 기존 애플리케이션 코드는 변경 없이 작동하며, 모델 이름만 바꾸면 됩니다.

---

## 주요 요점

| 개념 | 상세 내용 |
|---------|---------|
| ONNX Runtime GenAI 모델 빌더 | Hugging Face 모델을 한 번의 명령으로 양자화 포함 ONNX 형식으로 변환 |
| ONNX 형식 | Foundry Local은 ONNX Runtime GenAI 구성과 함께 ONNX 모델을 요구 |
| 채팅 템플릿 | `inference_model.json` 파일은 특정 모델에 맞게 프롬프트 형식을 Foundry Local에 알려줌 |
| 하드웨어 대상 | CPU, NVIDIA GPU(CUDA), DirectML(Windows GPU), 또는 WebGPU용으로 컴파일 가능 |
| 양자화 | 저정밀도(int4)는 크기 축소 및 속도 향상, 일부 정확도 손실; fp16은 GPU에서 높은 품질 유지 |
| API 호환성 | 사용자 지정 모델도 내장 모델과 동일한 OpenAI 호환 API 사용 |
| Foundry Local SDK | SDK가 서비스 시작, 엔드포인트 검색, 모델 로딩을 자동으로 처리 (카탈로그 및 사용자 지정 모델 모두에 적용) |

---

## 추가 자료

| 자료 | 링크 |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local 사용자 지정 모델 가이드 | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 모델 패밀리 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive 문서 | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## 다음 단계

[Part 11: Tool Calling with Local Models](part11-tool-calling.md)에서 로컬 모델이 외부 함수를 호출하도록 활성화하는 방법을 계속 학습하세요.

[← Part 9: Whisper Voice Transcription](part9-whisper-voice-transcription.md) | [Part 11: Tool Calling →](part11-tool-calling.md)