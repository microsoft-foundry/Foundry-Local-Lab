# 코딩 에이전트 지침

이 파일은 이 저장소에서 작업하는 AI 코딩 에이전트(GitHub Copilot, Copilot Workspace, Codex 등)를 위한 컨텍스트를 제공합니다.

## 프로젝트 개요

이것은 [Foundry Local](https://foundrylocal.ai)을 사용하여 AI 애플리케이션을 구축하는 <strong>실습 워크숍</strong>입니다 — 경량 런타임으로, OpenAI 호환 API를 통해 기기 내에서 언어 모델을 다운로드, 관리, 제공하는 기능을 갖추고 있습니다. 워크숍에는 단계별 실습 가이드와 Python, JavaScript, C# 코드 샘플이 포함되어 있습니다.

## 저장소 구조

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## 언어 및 프레임워크 세부 정보

### Python
- **위치:** `python/`, `zava-creative-writer-local/src/api/`
- **종속성:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **주요 패키지:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **최소 버전:** Python 3.9+
- **실행:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **위치:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **종속성:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **주요 패키지:** `foundry-local-sdk`, `openai`
- **모듈 시스템:** ES 모듈(`.mjs` 파일, `"type": "module"`)
- **최소 버전:** Node.js 18+
- **실행:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **위치:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **프로젝트 파일:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **주요 패키지:** `Microsoft.AI.Foundry.Local` (비-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — QNN EP 포함 상위 집합), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **대상:** .NET 9.0 (조건부 TFM: Windows에서는 `net9.0-windows10.0.26100`, 그 외는 `net9.0`)
- **실행:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## 코딩 규칙

### 일반
- 모든 코드 샘플은 <strong>자체 포함 단일 파일 예제</strong>입니다 — 공유 유틸리티 라이브러리나 추상화는 없습니다.
- 각 샘플은 자체 종속성 설치 후 독립적으로 실행됩니다.
- API 키는 항상 `"foundry-local"`로 설정됩니다 — Foundry Local에서 자리표시자로 사용합니다.
- 기본 URL은 `http://localhost:<port>/v1`이며, 포트는 런타임에 SDK를 통해 동적으로 발견됩니다(`manager.urls[0]` in JS, `manager.endpoint` in Python).
- Foundry Local SDK가 서비스 시작과 엔드포인트 발견을 처리하므로 하드코딩된 포트보다 SDK 패턴을 우선시합니다.

### Python
- `OpenAI(base_url=..., api_key="not-required")`와 함께 `openai` SDK 사용.
- `foundry_local`의 `FoundryLocalManager()`로 SDK 관리 서비스 라이프사이클 사용.
- 스트리밍: `for chunk in stream:`으로 `stream` 객체를 반복.
- 샘플 파일에는 타입 주석 없음 (워크숍 학습자용으로 샘플 간결 유지).

### JavaScript
- ES 모듈 문법: `import ... from "..."`.
- `"openai"`에서 `OpenAI`, `"foundry-local-sdk"`에서 `FoundryLocalManager` 사용.
- SDK 초기화 패턴: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- 스트리밍: `for await (const chunk of stream)`.
- 최상위 `await`를 전역적으로 사용.

### C#
- Nullable 활성화, 암시적 using, .NET 9 사용.
- SDK 관리 라이프사이클에 `FoundryLocalManager.StartServiceAsync()` 사용.
- 스트리밍: `CompleteChatStreaming()`에서 `foreach (var update in completionUpdates)`.
- `csharp/Program.cs`는 CLI 라우터로, 정적 `RunAsync()` 메서드를 호출.

### 도구 호출
- 도구 호출을 지원하는 모델은 **Qwen 2.5** 패밀리 (`qwen2.5-*`)와 **Phi-4-mini** (`phi-4-mini`)뿐입니다.
- 도구 스키마는 OpenAI 함수 호출 JSON 형식을 따릅니다 (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- 대화는 다중 턴 패턴: 사용자 → 어시스턴트 (tool_calls) → 도구 (결과) → 어시스턴트 (최종 답변).
- 도구 결과 메시지의 `tool_call_id`는 모델의 도구 호출 `id`와 일치해야 합니다.
- Python은 OpenAI SDK를 직접 사용; JavaScript는 SDK 네이티브 `ChatClient` (`model.createChatClient()`); C#은 OpenAI SDK와 `ChatTool.CreateFunctionTool()` 사용.

### ChatClient (네이티브 SDK 클라이언트)
- JavaScript: `model.createChatClient()`는 `completeChat(messages, tools?)` 및 `completeStreamingChat(messages, callback)` 메서드가 있는 `ChatClient` 반환.
- C#: `model.GetChatClientAsync()`는 OpenAI NuGet 패키지 없이 사용할 수 있는 표준 `ChatClient` 반환.
- Python에는 네이티브 ChatClient 없으므로, OpenAI SDK와 `manager.endpoint`, `manager.api_key` 사용.
- **중요:** JavaScript `completeStreamingChat`는 async 반복이 아닌 **콜백 패턴** 사용.

### 추론 모델
- `phi-4-mini-reasoning`은 최종 답변 전에 `<think>...</think>` 태그로 추론을 감쌉니다.
- 필요한 경우 태그를 파싱하여 추론과 답변을 분리합니다.

## 실습 가이드

실습 파일은 `labs/`에 마크다운으로 있습니다. 일관된 구조를 따릅니다:
- 로고 헤더 이미지
- 제목과 목표 콜아웃
- 개요, 학습 목표, 사전 조건
- 다이어그램이 포함된 개념 설명 섹션
- 코드 블록과 예상 출력이 포함된 번호 매겨진 연습문제
- 요약 표, 핵심 내용, 추가 읽을거리
- 다음 파트로 넘어가는 탐색 링크

실습 내용 편집 시:
- 기존 마크다운 형식 스타일과 섹션 계층 구조 유지.
- 코드 블록에 언어 지정 (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- OS에 따라 bash와 PowerShell 명령 모두 제공.
- `> **Note:**`, `> **Tip:**`, `> **Troubleshooting:**` 콜아웃 스타일 사용.
- 표는 `| 헤더 | 헤더 |` 파이프 형식으로 사용.

## 빌드 및 테스트 명령

| 작업 | 명령어 |
|--------|---------|
| **Python 샘플** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS 샘플** | `cd javascript && npm install && node <script>.mjs` |
| **C# 샘플** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (웹)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (웹)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **다이어그램 생성** | `npx mmdc -i <input>.mmd -o <output>.svg` (최상위 `npm install` 필요) |

## 외부 종속성

- <strong>Foundry Local CLI</strong>는 개발자의 머신에 설치되어 있어야 합니다 (`winget install Microsoft.FoundryLocal` 또는 `brew install foundrylocal`).
- <strong>Foundry Local 서비스</strong>는 로컬에서 실행되며, 동적 포트의 OpenAI 호환 REST API를 노출합니다.
- 모든 샘플 실행에 클라우드 서비스, API 키, Azure 구독은 필요 없습니다.
- 10부(커스텀 모델)는 추가로 `onnxruntime-genai` 패키지와 Hugging Face에서 모델 가중치를 다운로드해야 합니다.

## 커밋해서는 안 될 파일들

`.gitignore`가 제외(대부분 포함)하는 항목:
- `.venv/` — Python 가상 환경
- `node_modules/` — npm 종속성
- `models/` — 컴파일된 ONNX 모델 출력 (대용량 바이너리 파일, 10부에서 생성)
- `cache_dir/` — Hugging Face 모델 다운로드 캐시
- `.olive-cache/` — Microsoft Olive 작업 디렉터리
- `samples/audio/*.wav` — 생성된 오디오 샘플 (`python samples/audio/generate_samples.py`로 재생성 가능)
- 표준 Python 빌드 산출물 (`__pycache__/`, `*.egg-info/`, `dist/` 등)

## 라이선스

MIT — `LICENSE` 참조.