<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local 워크숍 - 기기 내 AI 앱 구축

자신의 컴퓨터에서 언어 모델을 실행하고 [Foundry Local](https://foundrylocal.ai) 및 [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/)와 함께 지능형 애플리케이션을 구축하는 실습 워크숍입니다.

> **Foundry Local이란?** Foundry Local은 경량 런타임으로, 하드웨어에서 언어 모델을 완전히 다운로드, 관리, 제공할 수 있게 합니다. <strong>OpenAI 호환 API</strong>를 노출하여 OpenAI를 지원하는 모든 도구나 SDK가 연결할 수 있으며, 클라우드 계정이 필요 없습니다.

---

## 학습 목표

이 워크숍을 마치면 다음을 할 수 있습니다:

| # | 목표 |
|---|-----------|
| 1 | Foundry Local 설치 및 CLI로 모델 관리 |
| 2 | 프로그래밍 방식 모델 관리를 위한 Foundry Local SDK API 마스터 |
| 3 | Python, JavaScript, C# SDK를 사용해 로컬 추론 서버에 연결 |
| 4 | 자체 데이터에 근거한 답변을 생성하는 검색-증강 생성(RAG) 파이프라인 구축 |
| 5 | 지속적인 명령과 페르소나가 있는 AI 에이전트 생성 |
| 6 | 피드백 루프가 있는 다중 에이전트 워크플로우 조율 |
| 7 | 프로덕션 캡스톤 앱인 Zava Creative Writer 탐색 |
| 8 | 골든 데이터셋 및 LLM 심사 기반 평가 프레임워크 구축 |
| 9 | Whisper로 오디오 필사 - Foundry Local SDK를 사용하여 기기 내 음성-텍스트 변환 |
| 10 | ONNX Runtime GenAI 및 Foundry Local로 사용자 지정 또는 Hugging Face 모델 컴파일 및 실행 |
| 11 | 도구 호출 패턴으로 로컬 모델이 외부 함수 호출 가능하게 하기 |
| 12 | 실시간 스트리밍을 지원하는 Zava Creative Writer의 브라우저 기반 UI 구축 |

---

## 사전 준비 사항

| 요구사항 | 세부 내용 |
|-------------|---------|
| <strong>하드웨어</strong> | 최소 8GB RAM (권장 16GB); AVX2 지원 CPU 또는 지원되는 GPU |
| <strong>운영체제</strong> | Windows 10/11 (x64/ARM), Windows Server 2025, macOS 13 이상 |
| **Foundry Local CLI** | Windows: `winget install Microsoft.FoundryLocal` 또는 macOS: `brew tap microsoft/foundrylocal && brew install foundrylocal`로 설치. 자세한 내용은 [시작 가이드](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) 참조. |
| **언어 런타임** | **Python 3.9+** 및/또는 **.NET 9.0+** 및/또는 **Node.js 18+** |
| **Git** | 이 저장소 클론용 |

---

## 시작하기

```bash
# 1. 저장소를 복제하세요
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry Local이 설치되었는지 확인하세요
foundry model list              # 사용 가능한 모델 목록
foundry model run phi-3.5-mini  # 대화형 채팅 시작

# 3. 사용 언어 트랙 선택 (전체 설정은 2부 실습 참고)
```

| 언어 | 빠른 시작 |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## 워크숍 구성

### 파트 1: Foundry Local 시작하기

**실습 안내:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local이 무엇이며 어떻게 작동하는지
- Windows 및 macOS에 CLI 설치하기
- 모델 탐색 - 목록, 다운로드, 실행
- 모델 별칭 및 동적 포트 이해하기

---

### 파트 2: Foundry Local SDK 심화

**실습 안내:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- 앱 개발에 CLI 대신 SDK 사용 이유
- Python, JavaScript, C#용 전체 SDK API 참조
- 서비스 관리, 카탈로그 탐색, 모델 라이프사이클(다운로드, 로드, 언로드)
- 빠른 시작 패턴: Python 생성자 부트스트랩, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` 메타데이터, 별칭, 하드웨어 최적 모델 선택

---

### 파트 3: SDK 및 API

**실습 안내:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, C#에서 Foundry Local 연결
- Foundry Local SDK로 서비스 프로그래밍 방식 관리
- OpenAI 호환 API를 통한 채팅 스트리밍
- 각 언어별 SDK 메서드 참조

**코드 샘플:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `python/foundry-local.py` | 기본 스트리밍 채팅 |
| C# | `csharp/BasicChat.cs` | .NET 스트리밍 채팅 |
| JavaScript | `javascript/foundry-local.mjs` | Node.js 스트리밍 채팅 |

---

### 파트 4: 검색-증강 생성(RAG)

**실습 안내:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG란 무엇이며 중요한 이유
- 메모리 내 지식 기반 구축
- 키워드 중첩 검색 및 점수 매기기
- 근거 있는 시스템 프롬프트 작성
- 기기 내에서 완전한 RAG 파이프라인 실행

**코드 샘플:**

| 언어 | 파일 |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 파트 5: AI 에이전트 구축

**실습 안내:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI 에이전트란? (원시 LLM 호출과의 비교)
- `ChatAgent` 패턴 및 Microsoft Agent Framework
- 시스템 명령, 페르소나, 다중 턴 대화
- 에이전트의 구조화된 출력 (JSON)

**코드 샘플:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework 단일 에이전트 |
| C# | `csharp/SingleAgent.cs` | 단일 에이전트 (ChatAgent 패턴) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 단일 에이전트 (ChatAgent 패턴) |

---

### 파트 6: 다중 에이전트 워크플로우

**실습 안내:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- 다중 에이전트 파이프라인: Researcher → Writer → Editor
- 순차 조율 및 피드백 루프
- 공유 구성 및 구조화된 인계
- 자신만의 다중 에이전트 워크플로우 설계

**코드 샘플:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | 세 에이전트 파이프라인 |
| C# | `csharp/MultiAgent.cs` | 세 에이전트 파이프라인 |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 세 에이전트 파이프라인 |

---

### 파트 7: Zava Creative Writer - 캡스톤 애플리케이션

**실습 안내:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4개의 전문화된 에이전트가 포함된 프로덕션 스타일 다중 에이전트 앱
- 평가자 주도 피드백 루프가 있는 순차적 파이프라인
- 스트리밍 출력, 제품 카탈로그 검색, 구조화된 JSON 인계
- Python (FastAPI), JavaScript (Node.js CLI), C# (.NET 콘솔)로 완전 구현

**코드 샘플:**

| 언어 | 디렉터리 | 설명 |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | 조율자가 포함된 FastAPI 웹 서비스 |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI 애플리케이션 |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 콘솔 애플리케이션 |

---

### 파트 8: 평가 중심 개발

**실습 안내:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 골든 데이터셋을 활용한 AI 에이전트 체계적 평가 프레임워크 구축
- 규칙 기반 검사(길이, 키워드 범위, 금지어) + LLM 심사 점수
- 프롬프트 변형의 나란한 비교와 종합 점수표
- 7부의 Zava Editor 에이전트 패턴을 오프라인 테스트 스위트로 확장
- Python, JavaScript, C# 트랙 제공

**코드 샘플:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | 평가 프레임워크 |
| C# | `csharp/AgentEvaluation.cs` | 평가 프레임워크 |
| JavaScript | `javascript/foundry-local-eval.mjs` | 평가 프레임워크 |

---

### 파트 9: Whisper를 활용한 음성 필사

**실습 안내:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- 로컬에서 실행되는 OpenAI Whisper를 사용한 음성-텍스트 변환 필사
- 프라이버시를 최우선으로 하는 오디오 처리 - 오디오는 장치를 벗어나지 않음
- Python, JavaScript, C# 트랙에서 `client.audio.transcriptions.create()`(Python/JS) 및 `AudioClient.TranscribeAudioAsync()`(C#) 사용
- 실습용 Zava 테마 샘플 오디오 파일 포함

**코드 샘플:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper 음성 필사 |
| C# | `csharp/WhisperTranscription.cs` | Whisper 음성 필사 |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper 음성 필사 |

> **참고:** 이 실습은 <strong>Foundry Local SDK</strong>를 이용해 Whisper 모델을 프로그래밍 방식으로 다운로드 및 로드한 후, 로컬 OpenAI 호환 엔드포인트로 오디오를 보내 필사합니다. Whisper 모델(`whisper`)은 Foundry Local 카탈로그에 등록되어 있으며 기기 내에서 완전히 실행되어 클라우드 API 키나 네트워크 접근이 필요 없습니다.

---

### 파트 10: 사용자 지정 또는 Hugging Face 모델 사용

**실습 안내:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX Runtime GenAI 모델 빌더를 이용해 Hugging Face 모델을 최적화된 ONNX 형식으로 컴파일
- 하드웨어별 컴파일(CPU, NVIDIA GPU, DirectML, WebGPU) 및 양자화(int4, fp16, bf16)
- Foundry Local용 채팅 템플릿 구성 파일 생성
- 컴파일된 모델을 Foundry Local 캐시에 추가
- CLI, REST API, OpenAI SDK를 통해 사용자 지정 모델 실행
- 참조 예: Qwen/Qwen3-0.6B를 처음부터 끝까지 컴파일

---

### 파트 11: 로컬 모델에서 도구 호출

**실습 안내:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- 로컬 모델이 외부 함수(도구) 호출 가능하도록 활성화
- OpenAI 함수 호출 형식으로 도구 스키마 정의
- 다중 턴 도구 호출 대화 흐름 처리
- 도구 호출을 로컬에서 실행하고 결과를 모델에 반환
- 도구 호출 시나리오에 적합한 모델 선택(Qwen 2.5, Phi-4-mini)
- 도구 호출을 위해 SDK의 네이티브 `ChatClient` 사용(JavaScript)

**코드 샘플:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 날씨/인구 도구와 도구 호출 |
| C# | `csharp/ToolCalling.cs` | .NET으로 도구 호출 |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient로 도구 호출 |

---

### 파트 12: Zava Creative Writer용 웹 UI 구축

**실습 안내:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer에 브라우저 기반 프런트엔드 추가
- Python(FastAPI), JavaScript(Node.js HTTP), C#(ASP.NET Core)에서 공유 UI 제공
- Fetch API와 ReadableStream을 사용해 브라우저에서 스트리밍 NDJSON 소비
- 라이브 에이전트 상태 배지 및 실시간 기사 텍스트 스트리밍

**코드 (공유 UI):**

| 파일 | 설명 |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | 페이지 레이아웃 |
| `zava-creative-writer-local/ui/style.css` | 스타일링 |
| `zava-creative-writer-local/ui/app.js` | 스트림 리더 및 DOM 업데이트 로직 |

**백엔드 추가 사항:**

| 언어 | 파일 | 설명 |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 정적 UI 제공용 업데이트 |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | 조율기 래핑 신규 HTTP 서버 |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 새로운 ASP.NET Core 미니멀 API 프로젝트 |

---

### 파트 13: 워크숍 완료
**랩 가이드:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 12개 파트 전반에 걸쳐 구축한 모든 내용 요약
- 애플리케이션 확장을 위한 추가 아이디어
- 리소스 및 문서 링크

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

## 리소스

| 리소스 | 링크 |
|----------|------|
| Foundry Local 웹사이트 | [foundrylocal.ai](https://foundrylocal.ai) |
| 모델 카탈로그 | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| 시작 가이드 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 참조 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## 라이선스

이 워크숍 자료는 교육 목적으로 제공됩니다.

---

**행복한 개발 되세요! 🚀**