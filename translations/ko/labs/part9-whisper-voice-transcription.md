![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 9부: Whisper와 Foundry Local을 사용한 음성 전사

> **목표:** Foundry Local을 통해 로컬에서 실행되는 OpenAI Whisper 모델을 사용하여 오디오 파일을 전사 - 완전한 온디바이스 처리, 클라우드 불필요.

## 개요

Foundry Local은 텍스트 생성뿐만 아니라 **음성-텍스트 변환** 모델도 지원합니다. 이 실습에서는 **OpenAI Whisper Medium** 모델을 사용하여 오디오 파일을 완전히 내장된 장치에서 전사합니다. 이는 Zava 고객 서비스 통화, 제품 리뷰 녹음, 작업장 기획 세션 등 오디오 데이터가 장치를 벗어나지 않아야 하는 시나리오에 이상적입니다.


---

## 학습 목표

이 실습을 마치면 다음을 할 수 있습니다:

- Whisper 음성-텍스트 변환 모델과 그 기능 이해
- Foundry Local을 사용하여 Whisper 모델 다운로드 및 실행
- Foundry Local SDK를 통한 Python, JavaScript 및 C#에서 오디오 파일 전사
- 온전히 온디바이스에서 실행되는 간단한 전사 서비스 구축
- Foundry Local에서 채팅/텍스트 모델과 오디오 모델 간 차이점 이해

---

## 사전 요구 사항

| 요구 사항 | 상세 내용 |
|-------------|---------|
| **Foundry Local CLI** | 버전 **0.8.101 이상** (Whisper 모델은 v0.8.101 이상에서 사용 가능) |
| <strong>운영체제</strong> | Windows 10/11 (x64 또는 ARM64) |
| **언어 런타임** | **Python 3.9+** 및/또는 **Node.js 18+** 및/또는 **.NET 9 SDK** ([.NET 다운로드](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **완료된 학습** | [1부: 시작하기](part1-getting-started.md), [2부: Foundry Local SDK 심층 탐구](part2-foundry-local-sdk.md), [3부: SDK 및 API](part3-sdk-and-apis.md) |

> **참고:** Whisper 모델은 <strong>SDK</strong>를 통해 다운로드해야 합니다 (CLI가 아닙니다). CLI는 오디오 전사 엔드포인트를 지원하지 않습니다. 버전을 확인하려면:
> ```bash
> foundry --version
> ```

---

## 개념: Whisper가 Foundry Local과 작동하는 방식

OpenAI Whisper 모델은 다양한 오디오 데이터셋으로 훈련된 범용 음성 인식 모델입니다. Foundry Local을 통해 실행할 때:

- 모델이 <strong>전적으로 CPU에서 실행</strong>되며 GPU는 필요 없음
- 오디오는 장치를 벗어나지 않음 - **완전한 프라이버시**
- Foundry Local SDK가 모델 다운로드 및 캐시 관리를 처리
- **JavaScript 및 C#** SDK 내장 `AudioClient`는 전사 파이프라인 전체를 자동으로 처리 — 수동 ONNX 설정 불필요
- <strong>Python</strong>은 SDK로 모델 관리를, ONNX Runtime으로 인코더/디코더 ONNX 모델에 직접 추론 수행

### 파이프라인 작동 방식 (JavaScript 및 C#) — SDK AudioClient

1. <strong>Foundry Local SDK</strong>가 Whisper 모델 다운로드 및 캐시 저장
2. `model.createAudioClient()` (JS) 또는 `model.GetAudioClientAsync()` (C#)가 `AudioClient` 생성
3. `audioClient.transcribe(path)` (JS) 또는 `audioClient.TranscribeAudioAsync(path)` (C#)가 내부적으로 전체 파이프라인 처리 — 오디오 전처리, 인코더, 디코더, 토큰 디코딩
4. `AudioClient`는 정확한 전사를 위한 `"en"`(영어) 설정이 가능한 `settings.language` 속성 제공

### 파이프라인 작동 방식 (Python) — ONNX Runtime

1. <strong>Foundry Local SDK</strong>가 Whisper ONNX 모델 파일 다운로드 및 캐시 저장
2. **오디오 전처리**: WAV 오디오를 멜 스펙트로그램(80 멜 빈 x 3000 프레임)으로 변환
3. <strong>인코더</strong>: 멜 스펙트로그램을 처리해 히든 스테이트와 크로스 어텐션 키/값 텐서 생성
4. <strong>디코더</strong>: 오토리그레시브 방식으로 한 번에 한 토큰씩 생성, 텍스트 종료 토큰까지 반복
5. <strong>토크나이저</strong>: 출력 토큰 ID를 읽을 수 있는 텍스트로 디코딩

### Whisper 모델 종류

| 별칭 | 모델 ID | 장치 | 크기 | 설명 |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU 가속(CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU 최적화(대부분 장치에 권장) |

> **참고:** 기본적으로 목록에 표시되는 채팅 모델과 달리, Whisper 모델은 `automatic-speech-recognition` 작업 아래에 분류됩니다. 세부 정보는 `foundry model info whisper-medium` 명령어로 확인하세요.

---

## 실습 문제

### 연습 0 - 샘플 오디오 파일 받기

이 실습은 Zava DIY 제품 시나리오 기반의 미리 작성된 WAV 파일을 포함합니다. 포함된 스크립트로 생성하세요:

```bash
# 저장소 루트에서 - 먼저 .venv를 생성하고 활성화하세요
python -m venv .venv

# 윈도우 (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

다음 6개의 WAV 파일이 `samples/audio/`에 생성됩니다:

| 파일 | 시나리오 |
|------|----------|
| `zava-customer-inquiry.wav` | 고객이 <strong>Zava ProGrip 무선 드릴</strong>에 대해 문의 |
| `zava-product-review.wav` | 고객이 **Zava UltraSmooth 인테리어 페인트** 리뷰 |
| `zava-support-call.wav` | **Zava TitanLock 공구함** 관련 지원 통화 |
| `zava-project-planning.wav` | DIY 사용자가 <strong>Zava EcoBoard 합성 데크재</strong>로 데크 계획 |
| `zava-workshop-setup.wav` | <strong>다섯 개 Zava 제품 모두</strong>를 이용한 작업장 둘러보기 |
| `zava-full-project-walkthrough.wav` | <strong>모든 Zava 제품</strong>을 활용한 확장된 차고 리노베이션 체험 (~4분, 장시간 오디오 테스트용) |

> **팁:** 직접 WAV/MP3/M4A 파일을 사용하거나 Windows 음성 녹음기로 녹음해도 됩니다.

---

### 연습 1 - SDK를 사용해 Whisper 모델 다운로드

최신 Foundry Local 버전에서 Whisper 모델과 CLI 간 호환성 문제 때문에, 모델 다운로드 및 로드는 <strong>SDK</strong>를 사용하세요. 언어를 선택하세요:

<details>
<summary><b>🐍 Python</b></summary>

**SDK 설치:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# 서비스를 시작합니다
manager = FoundryLocalManager()
manager.start_service()

# 카탈로그 정보를 확인합니다
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# 이미 캐시되었는지 확인합니다
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# 모델을 메모리에 로드합니다
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py`로 저장 후 실행:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK 설치:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// 매니저를 생성하고 서비스를 시작합니다
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 카탈로그에서 모델을 가져옵니다
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.id}`);

if (model.isCached) {
  console.log("Whisper model already downloaded.");
} else {
  console.log("Downloading Whisper model (this may take several minutes)...");
  await model.download();
  console.log("Download complete.");
}

// 모델을 메모리에 로드합니다
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs`로 저장 후 실행:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK 설치:**
```bash
dotnet add package Microsoft.AI.Foundry.Local
```

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "whisper-medium";

// Start the service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);
Console.WriteLine($"Model: {model.Id}");

// Check if already cached
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine("Whisper model already downloaded.");
}
else
{
    Console.WriteLine("Downloading Whisper model (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine("Download complete.");
}

// Load the model into memory
await model.LoadAsync(default);
Console.WriteLine($"Whisper model loaded: {model.Id}");
```

</details>

> **왜 CLI가 아닌 SDK인가요?** Foundry Local CLI는 Whisper 모델 직접 다운로드 또는 서빙을 지원하지 않습니다. SDK는 오디오 모델을 프로그래밍 방식으로 안정적으로 다운로드하고 관리할 수 있는 방법을 제공합니다. JavaScript와 C# SDK는 내장된 `AudioClient`를 제공하여 전체 전사 파이프라인을 처리합니다. Python은 캐시된 모델 파일에 대해 ONNX Runtime을 사용해 직접 추론합니다.

---

### 연습 2 - Whisper SDK 이해하기

Whisper 전사는 언어에 따라 접근 방식이 다릅니다. <strong>JavaScript 및 C#</strong>는 Foundry Local SDK 내장 `AudioClient`를 통해 오디오 전처리, 인코더, 디코더, 토크 디코딩을 단일 메서드 호출로 처리합니다. <strong>Python</strong>은 SDK로 모델을 관리하고 ONNX Runtime을 사용해 인코더/디코더 ONNX 모델에 직접 추론합니다.

| 구성요소 | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK 패키지** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **모델 관리** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **특징 추출** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` 내장 처리 | SDK `AudioClient` 내장 처리 |
| <strong>추론</strong> | `ort.InferenceSession` (인코더 + 디코더) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **토큰 디코딩** | `WhisperTokenizer` | SDK `AudioClient` 내장 처리 | SDK `AudioClient` 내장 처리 |
| **언어 설정** | 디코더 토큰의 `forced_ids`로 설정 | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| <strong>입력</strong> | WAV 파일 경로 | WAV 파일 경로 | WAV 파일 경로 |
| <strong>출력</strong> | 디코딩된 텍스트 문자열 | `result.text` | `result.Text` |

> **중요:** 항상 `AudioClient`에서 언어를 설정하세요(예: 영어는 `"en"`). 명시적 언어 설정 없이는 모델이 언어를 자동 감지하려 시도해 출력이 엉망이 될 수 있습니다.

> **SDK 패턴:** Python은 `FoundryLocalManager(alias)`로 부트스트랩 후 `get_cache_location()`으로 ONNX 모델 위치 확인. JavaScript와 C#은 SDK 내장 `AudioClient` — `model.createAudioClient()`(JS) 또는 `model.GetAudioClientAsync()`(C#) — 를 사용해 전체 전사 파이프라인 처리. 자세한 내용은 [2부: Foundry Local SDK 심층 탐구](part2-foundry-local-sdk.md) 참조.

---

### 연습 3 - 간단한 전사 앱 만들기

언어 트랙을 선택해 오디오 파일을 전사하는 최소 애플리케이션을 만드세요.

> **지원 오디오 포맷:** WAV, MP3, M4A. 최상의 결과를 위해 16kHz 샘플링 WAV 파일 사용 권장.

<details>
<summary><h3>Python 트랙</h3></summary>

#### 설정

```bash
cd python
python -m venv venv

# 가상 환경 활성화:
# 윈도우 (파워셸):
venv\Scripts\Activate.ps1
# 맥OS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### 전사 코드

`foundry-local-whisper.py` 파일 생성:

```python
import sys
import os
import numpy as np
import onnxruntime as ort
import librosa
from transformers import WhisperFeatureExtractor, WhisperTokenizer
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"
audio_file = sys.argv[1] if len(sys.argv) > 1 else "sample.wav"

if not os.path.exists(audio_file):
    print(f"Audio file not found: {audio_file}")
    sys.exit(1)

# 1단계: 부트스트랩 - 서비스 시작, 모델 다운로드 및 로드
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# 캐시된 ONNX 모델 파일 경로 생성
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# 2단계: ONNX 세션 및 특징 추출기 로드
encoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_encoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
decoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_decoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
fe = WhisperFeatureExtractor.from_pretrained(model_dir)
tokenizer = WhisperTokenizer.from_pretrained(model_dir)

# 3단계: 멜 스펙트로그램 특징 추출
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# 4단계: 인코더 실행
enc_out = encoder.run(None, {"audio_features": input_features})
# 첫 번째 출력은 히든 상태이며, 나머지는 크로스-어텐션 KV 쌍입니다
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# 5단계: 자기회귀 디코딩
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, 필사, 타임스탬프 없음
input_ids = np.array([initial_tokens], dtype=np.int32)

# 빈 자기어텐션 KV 캐시
self_kv = {}
for i in range(24):
    self_kv[f"past_key_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)
    self_kv[f"past_value_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)

generated = []
for _ in range(448):
    feeds = {"input_ids": input_ids, **cross_kv, **self_kv}
    outputs = decoder.run(None, feeds)
    logits = outputs[0]
    next_token = int(np.argmax(logits[0, -1, :]))

    if next_token == 50257:  # 텍스트 끝
        break
    generated.append(next_token)

    # 자기어텐션 KV 캐시 업데이트
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### 실행

```bash
# Zava 제품 시나리오 필기
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# 또는 다른 것을 시도해보세요:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### 주요 Python 포인트

| 메서드 | 용도 |
|--------|---------|
| `FoundryLocalManager(alias)` | 부트스트랩: 서비스 시작, 모델 다운로드 및 로드 |
| `manager.get_cache_location()` | 캐시된 ONNX 모델 파일 경로 가져오기 |
| `WhisperFeatureExtractor.from_pretrained()` | 멜 스펙트로그램 특징 추출기 로드 |
| `ort.InferenceSession()` | 인코더 및 디코더용 ONNX Runtime 세션 생성 |
| `tokenizer.decode()` | 출력 토큰 ID를 텍스트로 변환 |

</details>

<details>
<summary><h3>JavaScript 트랙</h3></summary>

#### 설정

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### 전사 코드

`foundry-local-whisper.mjs` 파일 생성:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// 1단계: 부트스트랩 - 매니저 생성, 서비스 시작 및 모델 로드
console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log("Downloading Whisper model...");
  await model.download();
}
await model.load();

// 2단계: 오디오 클라이언트를 생성하고 텍스트로 변환
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// 정리
await model.unload();
```

> **참고:** Foundry Local SDK는 `model.createAudioClient()`를 통해 내장된 `AudioClient`를 제공하며, ONNX 추론 파이프라인 전체를 내부적으로 처리하므로 `onnxruntime-node` 임포트가 필요 없습니다. 항상 `audioClient.settings.language = "en"`을 설정해 정확한 영어 전사를 보장하세요.

#### 실행

```bash
# Zava 제품 시나리오를 전사하기
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# 또는 다른 것을 시도해 보세요:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### 주요 JavaScript 포인트

| 메서드 | 용도 |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | 매니저 싱글톤 생성 |
| `await catalog.getModel(alias)` | 카탈로그에서 모델 가져오기 |
| `model.download()` / `model.load()` | Whisper 모델 다운로드 및 로드 |
| `model.createAudioClient()` | 전사용 오디오 클라이언트 생성 |
| `audioClient.settings.language = "en"` | 전사 언어 설정 (정확한 출력 위해 필수) |
| `audioClient.transcribe(path)` | 오디오 파일 전사, `{ text, duration }` 반환 |

</details>

<details>
<summary><h3>C# 트랙</h3></summary>

#### 설정

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **참고:** C# 트랙은 `Microsoft.AI.Foundry.Local` 패키지 사용, `model.GetAudioClientAsync()`로 내장 `AudioClient` 제공. 별도의 ONNX Runtime 설정 없이도 전체 전사 파이프라인 처리.

#### 전사 코드

`Program.cs` 내용을 교체:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// --- Configuration ---
var modelAlias = "whisper-medium";
var audioFile = args.Length > 0 ? args[0] : "sample.wav";

if (!File.Exists(audioFile))
{
    Console.WriteLine($"Audio file not found: {audioFile}");
    Console.WriteLine("Usage: dotnet run <path-to-audio-file>");
    return;
}

// --- Step 1: Initialize Foundry Local ---
Console.WriteLine("Initializing Foundry Local...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// --- Step 2: Load the Whisper model ---
Console.WriteLine($"Loading model: {modelAlias}...");
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading model...");
    await model.DownloadAsync(null, default);
}

// Load model into memory
Console.WriteLine("Loading model into memory...");
await model.LoadAsync(default);

// --- Step 3: Transcribe audio ---
Console.WriteLine($"Transcribing: {audioFile}");

var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en";

var result = await audioClient.TranscribeAudioAsync(audioFile);

Console.WriteLine("\n--- Transcription ---");
Console.WriteLine(result.Text);
Console.WriteLine("---------------------");
```

#### 실행

```bash
# Zava 제품 시나리오를 필기하세요
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# 또는 다른 것을 시도해 보세요:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### 주요 C# 포인트

| 메서드 | 용도 |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Local 초기화 및 설정 적용 |
| `catalog.GetModelAsync(alias)` | 카탈로그에서 모델 가져오기 |
| `model.DownloadAsync()` | Whisper 모델 다운로드 |
| `model.GetAudioClientAsync()` | AudioClient 가져오기 (ChatClient 아님!) |
| `audioClient.Settings.Language = "en"` | 전사 언어 설정 (정확한 출력에 필수) |
| `audioClient.TranscribeAudioAsync(path)` | 오디오 파일 전사 |
| `result.Text` | 전사된 텍스트 |
> **C# vs Python/JS:** C# SDK는 JavaScript SDK와 유사하게 `model.GetAudioClientAsync()`를 통해 프로세스 내 전사 기능을 제공하는 내장 `AudioClient`를 제공합니다. Python은 ONNX Runtime을 직접 사용하여 캐시된 인코더/디코더 모델에 대해 추론을 수행합니다.

</details>

---

### Exercise 4 - 모든 Zava 샘플 일괄 전사하기

작동하는 전사 앱이 준비되었으니, 다섯 개의 Zava 샘플 파일을 모두 전사한 후 결과를 비교해 보세요.

<details>
<summary><h3>Python 트랙</h3></summary>

전체 샘플 `python/foundry-local-whisper.py`는 이미 일괄 전사를 지원합니다. 인수 없이 실행하면 `samples/audio/` 내의 모든 `zava-*.wav` 파일을 전사합니다:

```bash
cd python
python foundry-local-whisper.py
```

이 샘플은 `FoundryLocalManager(alias)`로 부트스트랩한 후 각 파일에 대해 인코더와 디코더 ONNX 세션을 실행합니다.

</details>

<details>
<summary><h3>JavaScript 트랙</h3></summary>

전체 샘플 `javascript/foundry-local-whisper.mjs`는 이미 일괄 전사를 지원합니다. 인수 없이 실행하면 `samples/audio/` 내의 모든 `zava-*.wav` 파일을 전사합니다:

```bash
cd javascript
node foundry-local-whisper.mjs
```

이 샘플은 `FoundryLocalManager.create()`와 `catalog.getModel(alias)`를 사용해 SDK를 초기화한 후, `AudioClient`(설정에서 `language = "en"`)를 사용하여 각 파일을 전사합니다.

</details>

<details>
<summary><h3>C# 트랙</h3></summary>

전체 샘플 `csharp/WhisperTranscription.cs`는 이미 일괄 전사를 지원합니다. 특정 파일 인수 없이 실행하면 `samples/audio/` 내의 모든 `zava-*.wav` 파일을 전사합니다:

```bash
cd csharp
dotnet run whisper
```

이 샘플은 `FoundryLocalManager.CreateAsync()`와 SDK의 `AudioClient`(설정에서 `Settings.Language = "en"`)를 사용해 프로세스 내 전사를 수행합니다.

</details>

**포인트:** `samples/audio/generate_samples.py`의 원문과 전사 결과를 비교해 보세요. "Zava ProGrip" 같은 제품명과 "brushless motor", "composite decking" 같은 기술 용어를 Whisper가 얼마나 정확하게 포착하는지 확인하세요.

---

### Exercise 5 - 주요 코드 패턴 이해하기

Whisper 전사와 채팅 완성(Chat Completion)이 세 언어에서 어떻게 다른지 공부해 보세요:

<details>
<summary><b>Python - 채팅과의 주요 차이점</b></summary>

```python
# 채팅 완료 (2-6부):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# 오디오 전사 (이번 부):
# OpenAI 클라이언트 대신 ONNX Runtime 직접 사용
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... 자기 회귀 디코더 루프 ...
print(tokenizer.decode(generated_tokens))
```

**핵심 인사이트:** 채팅 모델은 `manager.endpoint`를 통해 OpenAI 호환 API를 사용합니다. Whisper는 SDK를 통해 캐시된 ONNX 모델 파일을 위치시키고 ONNX Runtime으로 직접 추론을 수행합니다.

</details>

<details>
<summary><b>JavaScript - 채팅과의 주요 차이점</b></summary>

```javascript
// 채팅 완료 (2-6부):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// 오디오 전사 (이 부):
// SDK의 내장 AudioClient 사용
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // 최상의 결과를 위해 항상 언어 설정
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**핵심 인사이트:** 채팅 모델은 `manager.urls[0] + "/v1"`를 통해 OpenAI 호환 API를 사용합니다. Whisper 전사는 SDK의 `AudioClient`를 통해 수행하며, `model.createAudioClient()`에서 얻습니다. 자동 감지로 인한 깨진 출력을 막으려면 `settings.language`를 설정하세요.

</details>

<details>
<summary><b>C# - 채팅과의 주요 차이점</b></summary>

C# 접근법은 SDK 내장 `AudioClient`를 사용하여 프로세스 내 전사를 수행합니다:

**모델 초기화:**

```csharp
// 1. Create the manager with configuration
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// 2. Get model from catalog, download, and load
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("whisper-medium", default);
await model.DownloadAsync(null, default);
await model.LoadAsync(default);
```

**전사:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**핵심 인사이트:** C#는 `FoundryLocalManager.CreateAsync()`를 사용하고 `AudioClient`를 직접 획득하므로 ONNX Runtime 설정이 필요 없습니다. 자동 감지로 인한 깨진 출력을 막으려면 `Settings.Language`를 설정하세요.

</details>

> **요약:** Python은 모델 관리를 위해 Foundry Local SDK를, 캐시된 인코더/디코더 모델에 대해 직접 추론하려 ONNX Runtime을 사용합니다. JavaScript와 C#은 둘 다 SDK 내장 `AudioClient`를 이용해 간소화된 전사를 지원합니다 — 클라이언트를 생성하고, 언어를 설정한 뒤 `transcribe()` / `TranscribeAudioAsync()`를 호출하세요. 항상 정확한 결과를 위해 AudioClient의 언어 속성을 설정하세요.

---

### Exercise 6 - 실험해 보기

다음 변경을 시도해 보며 이해를 깊게 하세요:

1. **다른 오디오 파일 시도하기** - Windows 음성 녹음기를 사용해 자신의 음성을 녹음하고 WAV로 저장한 뒤 전사해 보세요

2. **모델 변형 비교하기** - NVIDIA GPU가 있다면 CUDA 변형을 시도해 보세요:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   CPU 변형과 전사 속도를 비교하세요.

3. **출력 포맷 추가하기** - JSON 응답에 포함할 수 있습니다:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API 구축하기** - 전사 코드를 웹 서버에 래핑하세요:

   | 언어 | 프레임워크 | 예시 |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")`와 함께 `UploadFile` 사용 |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")`와 함께 `multer` 사용 |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")`와 함께 `IFormFile` 사용 |

5. **다중 턴 전사 연동** - Part 4의 채팅 에이전트와 Whisper를 결합하세요: 먼저 오디오를 전사한 뒤 텍스트를 에이전트로 전달해 분석 또는 요약을 수행합니다.

---

## SDK Audio API 참조

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — `AudioClient` 인스턴스 생성
> - `audioClient.settings.language` — 전사 언어 설정 (예: `"en"`)
> - `audioClient.settings.temperature` — 랜덤성 조절 (선택 사항)
> - `audioClient.transcribe(filePath)` — 파일 전사, `{ text, duration }` 반환
> - `audioClient.transcribeStreaming(filePath, callback)` — 콜백으로 전사 청크 스트리밍
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — `OpenAIAudioClient` 인스턴스 생성
> - `audioClient.Settings.Language` — 전사 언어 설정 (예: `"en"`)
> - `audioClient.Settings.Temperature` — 랜덤성 조절 (선택 사항)
> - `await audioClient.TranscribeAudioAsync(filePath)` — 파일 전사, `.Text` 포함 객체 반환
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — 전사 청크의 `IAsyncEnumerable` 반환

> **팁:** 전사 전 항상 언어 속성을 설정하세요. 설정하지 않으면 Whisper 모델이 자동 감지를 시도하며, 텍스트 대신 단일 대체 문자로 출력이 깨질 수 있습니다.

---

## 비교: 채팅 모델 vs Whisper

| 항목 | 채팅 모델 (파트 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **작업 유형** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| <strong>입력</strong> | 텍스트 메시지 (JSON) | 오디오 파일 (WAV/MP3/M4A) | 오디오 파일 (WAV/MP3/M4A) |
| <strong>출력</strong> | 생성된 텍스트 (스트리밍) | 전사된 텍스트 (완료) | 전사된 텍스트 (완료) |
| **SDK 패키지** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API 메서드** | `client.chat.completions.create()` | ONNX Runtime 직접 호출 | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **언어 설정** | 해당 없음 | 디코더 프롬프트 토큰 | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| <strong>스트리밍</strong> | 가능 | 불가능 | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **프라이버시 이점** | 코드/데이터가 로컬 유지 | 오디오 데이터가 로컬 유지 | 오디오 데이터가 로컬 유지 |

---

## 주요 요점

| 개념 | 학습 내용 |
|---------|-----------------|
| **Whisper 온디바이스** | 음성-텍스트 변환이 완전히 로컬에서 실행되어 Zava 고객 통화 및 제품 리뷰를 온디바이스 전사에 적합함 |
| **SDK AudioClient** | JavaScript 및 C# SDK는 단일 호출로 전체 전사 파이프라인을 처리하는 내장 `AudioClient`를 제공함 |
| **언어 설정** | 항상 AudioClient의 언어(예: `"en"`)를 설정해야 하며, 설정하지 않으면 자동 감지로 깨진 출력이 생길 수 있음 |
| **Python** | `foundry-local-sdk`로 모델 관리, `onnxruntime` + `transformers` + `librosa`로 ONNX 직접 추론 수행 |
| **JavaScript** | `foundry-local-sdk`에서 `model.createAudioClient()` 사용 — `settings.language` 설정 후 `transcribe()` 호출 |
| **C#** | `Microsoft.AI.Foundry.Local`에서 `model.GetAudioClientAsync()` 사용 — `Settings.Language` 설정 후 `TranscribeAudioAsync()` 호출 |
| **스트리밍 지원** | JS 및 C# SDK는 청크 단위 출력용 `transcribeStreaming()` / `TranscribeAudioStreamingAsync()`도 제공 |
| **CPU 최적화** | CPU 버전(3.05 GB)은 GPU 없이도 모든 Windows 기기에서 작동함 |
| **프라이버시 우선** | Zava 고객 상호작용 및 독점 제품 데이터를 온디바이스에 안전하게 보관하기에 적합함 |

---

## 참고 자료

| 자료 | 링크 |
|----------|------|
| Foundry Local 문서 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 참조 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper 모델 | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local 웹사이트 | [foundrylocal.ai](https://foundrylocal.ai) |

---

## 다음 단계

[Part 10: Using Custom or Hugging Face Models](part10-custom-models.md)로 진행하여 Hugging Face에서 직접 모델을 컴파일하고 Foundry Local에서 실행해 보세요.