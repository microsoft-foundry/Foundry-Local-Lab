<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local 워크숍 - 기기 내에서 AI 앱 구축하기

자신의 머신에서 언어 모델을 실행하고 [Foundry Local](https://foundrylocal.ai) 및 [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/)와 함께 지능형 애플리케이션을 구축하는 실습 워크숍입니다.

> **Foundry Local이란?** Foundry Local은 경량 런타임으로, 언어 모델을 완전히 자신의 하드웨어에서 다운로드, 관리 및 제공할 수 있게 해줍니다. OpenAI 호환 API를 제공하므로 OpenAI를 사용하는 도구나 SDK와 연결할 수 있으며, 클라우드 계정이 필요하지 않습니다.

### 🌐 다국어 지원

#### GitHub Action을 통해 지원 (자동화 및 항상 최신 상태 유지)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](./README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **로컬에 복제하는 것을 선호하시나요?**
>
> 이 저장소에는 50개 이상의 언어 번역본이 포함되어 있어 다운로드 크기가 크게 증가합니다. 번역 없이 복제하려면 sparse checkout을 사용하세요:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> 훨씬 더 빠른 다운로드로 과정 완료에 필요한 모든 것을 얻을 수 있습니다.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## 학습 목표

이 워크숍을 완료하면 다음을 수행할 수 있습니다:

| # | 목표 |
|---|-----------|
| 1 | Foundry Local 설치 및 CLI로 모델 관리하기 |
| 2 | 프로그래밍 방식 모델 관리를 위한 Foundry Local SDK API 마스터하기 |
| 3 | Python, JavaScript, C# SDK를 사용하여 로컬 추론 서버에 연결하기 |
| 4 | 자신의 데이터를 기반으로 답변을 생성하는 RAG(검색 증강 생성) 파이프라인 구축하기 |
| 5 | 영속적인 지침과 페르소나를 가진 AI 에이전트 생성하기 |
| 6 | 피드백 루프를 가진 다중 에이전트 워크플로우 조율하기 |
| 7 | 프로덕션 캡스톤 앱 - Zava Creative Writer 탐험하기 |
| 8 | 골든 데이터셋과 LLM 판사 채점법으로 평가 프레임워크 구축하기 |
| 9 | Foundry Local SDK를 사용하여 기기 내에서 Whisper로 음성-텍스트 전사하기 |
| 10 | ONNX Runtime GenAI 및 Foundry Local로 맞춤형 또는 Hugging Face 모델 컴파일 및 실행하기 |
| 11 | 도구 호출 패턴으로 로컬 모델이 외부 함수를 호출하도록 활성화하기 |
| 12 | 실시간 스트리밍이 가능한 브라우저 기반 UI로 Zava Creative Writer 구축하기 |

---

## 사전 준비 사항

| 요구 사항 | 세부 사항 |
|-------------|---------|
| <strong>하드웨어</strong> | 최소 8 GB RAM (권장 16 GB); AVX2 지원 CPU 또는 지원되는 GPU |
| <strong>운영체제</strong> | Windows 10/11 (x64/ARM), Windows Server 2025, 또는 macOS 13 이상 |
| **Foundry Local CLI** | Windows에서는 `winget install Microsoft.FoundryLocal`로, macOS에서는 `brew tap microsoft/foundrylocal && brew install foundrylocal`로 설치하세요. 자세한 내용은 [시작 가이드](https://learn.microsoft.com/en-us/azure/foundry-local/get-started)를 참조하십시오. |
| **언어 런타임** | **Python 3.9+** 및/또는 **.NET 9.0+** 및/또는 **Node.js 18+** |
| **Git** | 이 저장소를 복제하기 위해 필요 |

---

## 시작하기

```bash
# 1. 저장소를 복제합니다
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry Local이 설치되었는지 확인합니다
foundry model list              # 사용 가능한 모델 목록
foundry model run phi-3.5-mini  # 대화형 채팅 시작

# 3. 언어 트랙을 선택하세요 (전체 설정은 2부 실습 참조)
```

| 언어 | 빠른 시작 |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## 워크숍 구성

### 1부: Foundry Local 시작하기

**실습 가이드:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local이란 무엇이며 어떻게 동작하는가
- Windows와 macOS에 CLI 설치하기
- 모델 탐색하기 - 목록, 다운로드, 실행
- 모델 별칭과 동적 포트 이해하기

---

### 2부: Foundry Local SDK 심층 분석

**실습 가이드:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- 애플리케이션 개발에서 CLI 대신 SDK를 사용하는 이유
- Python, JavaScript, C#용 전체 SDK API 참조
- 서비스 관리, 카탈로그 탐색, 모델 라이프사이클(다운로드, 로드, 언로드)
- 빠른 시작 패턴: Python 생성자 부트스트랩, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` 메타데이터, 별칭, 하드웨어 최적 모델 선택

---

### 3부: SDK 및 API

**실습 가이드:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, C#에서 Foundry Local에 연결하기
- Foundry Local SDK로 서비스를 프로그래밍 방식으로 관리하기
- OpenAI 호환 API를 통한 스트리밍 채팅 완성
- 각 언어별 SDK 메서드 참조

**코드 샘플:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `python/foundry-local.py` | 기본 스트리밍 채팅 |
| C# | `csharp/BasicChat.cs` | .NET 스트리밍 채팅 |
| JavaScript | `javascript/foundry-local.mjs` | Node.js 스트리밍 채팅 |

---

### 4부: 검색 증강 생성 (RAG)

**실습 가이드:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG란 무엇이며 왜 중요한가
- 메모리 내 지식 기반 구축
- 점수 기반 키워드 중복 검색
- 기반이 되는 시스템 프롬프트 작성
- 기기 내에서 완전한 RAG 파이프라인 실행

**코드 샘플:**

| 언어 | 파일 |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 5부: AI 에이전트 구축

**실습 가이드:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI 에이전트란 무엇인가(LM 직접 호출과 비교)
- `ChatAgent` 패턴과 Microsoft Agent Framework
- 시스템 지침, 페르소나, 다중 턴 대화
- 에이전트에서의 구조화된 출력 (JSON)

**코드 샘플:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework를 이용한 단일 에이전트 |
| C# | `csharp/SingleAgent.cs` | 단일 에이전트 (ChatAgent 패턴) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 단일 에이전트 (ChatAgent 패턴) |

---

### 6부: 다중 에이전트 워크플로우

**실습 가이드:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- 다중 에이전트 파이프라인: 연구자 → 작가 → 편집자
- 순차적 조율과 피드백 루프
- 공유 구성 및 구조화된 인수 인계
- 자신만의 다중 에이전트 워크플로우 설계하기

**코드 샘플:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | 3 에이전트 파이프라인 |
| C# | `csharp/MultiAgent.cs` | 3 에이전트 파이프라인 |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 3 에이전트 파이프라인 |

---

### 7부: Zava Creative Writer - 캡스톤 애플리케이션

**실습 가이드:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4개의 전문화된 에이전트가 포함된 프로덕션 스타일 다중 에이전트 앱
- 평가자 주도 피드백 루프가 포함된 순차적 파이프라인
- 스트리밍 출력, 제품 카탈로그 검색, 구조화된 JSON 인수 인계
- Python (FastAPI), JavaScript (Node.js CLI), C# (.NET 콘솔)에서 완전 구현

**코드 샘플:**

| 언어 | 디렉터리 | 설명 |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | 오케스트레이터가 포함된 FastAPI 웹 서비스 |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI 애플리케이션 |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 콘솔 애플리케이션 |

---

### 8부: 평가 주도 개발

**실습 가이드:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 골든 데이터셋을 사용하여 AI 에이전트를 위한 체계적 평가 프레임워크 구축
- 규칙 기반 검사(길이, 키워드 범위, 금지 용어) + LLM 판사 채점법
- 프롬프트 변형의 나란한 비교 및 종합 점수표
- 7부 Zava Editor 에이전트 패턴을 오프라인 테스트 스위트로 확장
- Python, JavaScript, C# 트랙

**코드 샘플:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | 평가 프레임워크 |
| C# | `csharp/AgentEvaluation.cs` | 평가 프레임워크 |
| JavaScript | `javascript/foundry-local-eval.mjs` | 평가 프레임워크 |

---

### 9부: Whisper로 음성 전사

**실습 가이드:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- 로컬에서 실행되는 OpenAI Whisper를 사용한 음성-텍스트 전사
- 프라이버시 우선 오디오 처리 - 오디오가 기기를 벗어나지 않음
- Python, JavaScript, C# 트랙에서 `client.audio.transcriptions.create()` (Python/JS) 및 `AudioClient.TranscribeAudioAsync()` (C#) 사용
- 실습용 Zava 테마 샘플 오디오 파일 포함

**코드 샘플:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper 음성 전사 |
| C# | `csharp/WhisperTranscription.cs` | Whisper 음성 전사 |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper 음성 전사 |

> **Note:** 이 실습은 <strong>Foundry Local SDK</strong>를 사용하여 Whisper 모델을 프로그래밍 방식으로 다운로드하고 로드한 후, 로컬 OpenAI 호환 엔드포인트에 오디오를 전사용으로 전송합니다. Whisper 모델(`whisper`)은 Foundry Local 카탈로그에 등록되어 있으며 완전히 기기 내에서 실행되므로 클라우드 API 키나 네트워크 접근이 필요하지 않습니다.

---

### 10부: 맞춤형 또는 Hugging Face 모델 사용

**실습 안내:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face 모델을 ONNX Runtime GenAI 모델 빌더를 사용해 최적화된 ONNX 형식으로 컴파일
- 하드웨어별 컴파일(CPU, NVIDIA GPU, DirectML, WebGPU) 및 양자화(int4, fp16, bf16)
- Foundry Local용 채팅 템플릿 구성 파일 작성
- 컴파일된 모델을 Foundry Local 캐시에 추가
- CLI, REST API, OpenAI SDK를 통한 맞춤형 모델 실행
- 참고 예시: Qwen/Qwen3-0.6B 모델의 엔드 투 엔드 컴파일

---

### 11부: 로컬 모델의 도구 호출

**실습 안내:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- 로컬 모델이 외부 함수(도구/함수 호출)를 호출하도록 활성화
- OpenAI 함수 호출 포맷으로 도구 스키마 정의
- 다회차 도구 호출 대화 흐름 처리
- 도구 호출을 로컬에서 실행하고 결과를 모델에 반환
- 도구 호출 시나리오에 적합한 모델 선택(Qwen 2.5, Phi-4-mini)
- SDK의 네이티브 `ChatClient`를 사용한 도구 호출 (JavaScript)

**코드 샘플:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 날씨/인구 통계 도구 호출 |
| C# | `csharp/ToolCalling.cs` | .NET을 사용한 도구 호출 |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient를 사용한 도구 호출 |

---

### 12부: Zava 크리에이티브 작가용 웹 UI 구축

**실습 안내:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- 브라우저 기반 프런트엔드를 Zava 크리에이티브 작가에 추가
- Python(FastAPI), JavaScript(Node.js HTTP), C#(ASP.NET Core)에서 공유 UI 제공
- Fetch API와 ReadableStream을 사용하여 브라우저 내 스트리밍 NDJSON 처리
- 라이브 에이전트 상태 뱃지 및 실시간 기사 텍스트 스트리밍

**코드 (공유 UI):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | 페이지 레이아웃 |
| `zava-creative-writer-local/ui/style.css` | 스타일링 |
| `zava-creative-writer-local/ui/app.js` | 스트림 리더 및 DOM 업데이트 로직 |

**백엔드 추가사항:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 정적 UI 제공을 위해 업데이트 |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | 오케스트레이터를 래핑하는 새 HTTP 서버 |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 새 ASP.NET Core 미니멀 API 프로젝트 |

---

### 13부: 워크숍 완료

**실습 안내:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 12개 파트 전부를 아우르는 구축 요약
- 애플리케이션 확장에 관한 추가 아이디어
- 참고 자료 및 문서 링크

---

## 프로젝트 구조

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## 자료

| Resource | Link |
|----------|------|
| Foundry Local 웹사이트 | [foundrylocal.ai](https://foundrylocal.ai) |
| 모델 카탈로그 | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| 시작 가이드 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 참조 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft 에이전트 프레임워크 | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## 라이선스

이 워크숍 자료는 교육 목적으로 제공됩니다.

---

**행복한 개발 되세요! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**면책 조항**:  
이 문서는 AI 번역 서비스 [Co-op Translator](https://github.com/Azure/co-op-translator)를 사용하여 번역되었습니다. 정확성을 위해 노력하고 있으나, 자동 번역에는 오류나 부정확성이 포함될 수 있음을 알려드립니다. 원문 문서는 해당 언어의 권위 있는 출처로 간주되어야 합니다. 중요한 정보에 대해서는 전문가의 인간 번역을 권장합니다. 본 번역 사용으로 인한 오해나 잘못된 해석에 대해 당사는 책임을 지지 않습니다.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->