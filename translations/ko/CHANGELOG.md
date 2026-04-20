# 변경 로그 — Foundry Local 워크숍

이 워크숍의 모든 주요 변경 사항은 아래에 문서화되어 있습니다.

---

## 2026-03-11 — 파트 12 & 13, 웹 UI, Whisper 재작성, WinML/QNN 수정 및 검증

### 추가됨
- **파트 12: Zava 크리에이티브 라이터를 위한 웹 UI 구축** — 스트리밍 NDJSON, 브라우저 `ReadableStream`, 라이브 에이전트 상태 배지, 실시간 기사 텍스트 스트리밍을 다루는 새로운 실습 안내서 (`labs/part12-zava-ui.md`)
- **파트 13: 워크숍 완료** — 12개 파트의 요약, 추가 아이디어 및 리소스 링크를 포함하는 새 요약 실습 (`labs/part13-workshop-complete.md`)
- **Zava UI 프론트엔드:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — 세 가지 백엔드 모두에서 사용하는 공용 바닐라 HTML/CSS/JS 브라우저 인터페이스
- **JavaScript HTTP 서버:** `zava-creative-writer-local/src/javascript/server.mjs` — 브라우저 기반 접근을 위한 오케스트레이터를 감싸는 새 Express 스타일 HTTP 서버
- **C# ASP.NET Core 백엔드:** `zava-creative-writer-local/src/csharp-web/Program.cs` 및 `ZavaCreativeWriterWeb.csproj` — UI 및 스트리밍 NDJSON을 제공하는 새로운 최소 API 프로젝트
- **오디오 샘플 생성기:** `samples/audio/generate_samples.py` — Part 9용 Zava 테마 WAV 파일을 생성하는 오프라인 TTS 스크립트 (`pyttsx3` 사용)
- **오디오 샘플:** `samples/audio/zava-full-project-walkthrough.wav` — 전사 테스트용 새 장시간 오디오 샘플
- **검증 스크립트:** `validate-npu-workaround.ps1` — 모든 C# 샘플의 NPU/QNN 우회 작업을 자동으로 검증하는 PowerShell 스크립트
- **Mermaid 다이어그램 SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML 크로스 플랫폼 지원:** 3개의 C# `.csproj` 파일 모두(`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`)가 조건부 TFM 및 상호 배타적인 패키지 참조를 사용하여 크로스 플랫폼 지원 구현. 윈도우에서는 `net9.0-windows10.0.26100` TFM과 `Microsoft.AI.Foundry.Local.WinML` (QNN EP 플러그인을 포함하는 상위 집합), 비윈도우에서는 `net9.0` TFM과 `Microsoft.AI.Foundry.Local` (기본 SDK) 사용. 하드코딩된 `win-arm64` RID가 자동 감지로 변경됨. `Microsoft.ML.OnnxRuntime.Gpu.Linux`에서 끊어진 win-arm64 참조를 가진 네이티브 자산은 제외하는 종속성 우회가 추가됨. 이전의 try/catch NPU 우회는 7개의 모든 C# 파일에서 제거됨.

### 변경됨
- **파트 9 (Whisper):** 대대적 재작성 — JavaScript는 이제 SDK 내장 `AudioClient` (`model.createAudioClient()`)를 사용하며 수동 ONNX Runtime 추론 대신 사용; JS/C# `AudioClient` 방식과 Python ONNX Runtime 방식을 반영하도록 아키텍처 설명, 비교 테이블, 파이프라인 다이어그램을 업데이트함
- **파트 11:** 내비게이션 링크 업데이트 (현재 파트 12로 연결); 도구 호출 흐름 및 시퀀스 렌더링 SVG 다이어그램 추가
- **파트 10:** 내비게이션을 파트 12 경유로 변경하여 워크숍 종료 위치 변경
- **Python Whisper (`foundry-local-whisper.py`):** 추가 오디오 샘플 및 개선된 오류 처리 기능 확장
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** 수동 ONNX Runtime 세션 대신 `model.createAudioClient()` 및 `audioClient.transcribe()` 사용하도록 재작성
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** API와 함께 정적 UI 파일 서빙 기능으로 업데이트
- **Zava C# 콘솔 (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU 우회 제거 (WinML 패키지로 처리됨)
- **README.md:** 파트 12를 코드 샘플 테이블 및 백엔드 추가 사항과 함께 추가; 파트 13 섹션 추가; 학습 목표 및 프로젝트 구조 업데이트
- **KNOWN-ISSUES.md:** 해결된 문제 #7 (C# SDK NPU 모델 변형—이제 WinML 패키지로 처리) 제거. 나머지 문제 번호를 #1–#6으로 재정렬. .NET SDK 10.0.104 환경 세부 정보 업데이트
- **AGENTS.md:** 새 `zava-creative-writer-local` 항목(`ui/`, `csharp-web/`, `server.mjs`)으로 프로젝트 구조 트리 업데이트; C# 주요 패키지 및 조건부 TFM 세부 정보 업데이트
- **labs/part2-foundry-local-sdk.md:** 조건부 TFM, 상호 배타적 패키지 참조, 설명 노트를 포함한 완전한 크로스 플랫폼 패턴을 `.csproj` 예제로 업데이트

### 검증됨
- 3개의 C# 프로젝트(`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) 모두 Windows ARM64에서 빌드 성공
- 채팅 샘플 (`dotnet run chat`): WinML/QNN를 통해 `phi-3.5-mini-instruct-qnn-npu:1` 모델 로드 — CPU 폴백 없이 NPU 변형 직접 로드
- 에이전트 샘플 (`dotnet run agent`): 다중 회차 대화 실행, 종료 코드 0 완료
- Foundry Local CLI v0.8.117 및 SDK v0.9.0이 .NET SDK 9.0.312에서 정상 실행

---

## 2026-03-11 — 코드 수정, 모델 정리, Mermaid 다이어그램 및 검증

### 수정됨
- **모든 21개 코드 샘플(7개 Python, 7개 JavaScript, 7개 C#):** 종료 시 `model.unload()` / `unload_model()` / `model.UnloadAsync()` 정리 코드 추가하여 OGA 메모리 누수 경고 해결(알려진 문제 #4)
- **csharp/WhisperTranscription.cs:** 취약한 `AppContext.BaseDirectory` 상대 경로를 `FindSamplesDirectory()` 메서드로 교체, 이 메서드는 상위 디렉터리를 탐색하여 `samples/audio`를 신뢰성 있게 찾음 (알려진 문제 #7)
- **csharp/csharp.csproj:** 하드코딩된 `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>`를 `$(NETCoreSdkRuntimeIdentifier)` 기반 자동 감지 후속 처리로 교체하여 `dotnet run`이 플랫폼에 상관없이 `-r` 플래그 없이 작동하도록 수정 ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### 변경됨
- **파트 8:** 평가 기반 반복 루프를 ASCII 박스 다이어그램에서 렌더링된 SVG 이미지로 변환
- **파트 10:** 컴파일 파이프라인 다이어그램을 ASCII 화살표에서 렌더링된 SVG 이미지로 변환
- **파트 11:** 도구 호출 흐름 및 시퀀스 다이어그램을 렌더링된 SVG 이미지로 변환
- **파트 10:** "워크숍 완료!" 섹션을 파트 11(최종 실습)으로 이동; "다음 단계" 링크로 교체
- **KNOWN-ISSUES.md:** CLI v0.8.117에 대해 모든 문제 전면 재검증. 해결된 문제 제거: OGA 메모리 누수(정리 추가), Whisper 경로(FindSamplesDirectory), HTTP 500 지속 추론(재현 불가, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice 제한(현재 `"required"` 및 특정 함수 대상 qwen2.5-0.5b에서 작동). JS Whisper 문제 업데이트 — 현재 모든 파일이 빈/바이너리 출력 반환(v0.9.x 이후 회귀, 심각도 Major로 상승). #4 C# RID는 자동 감지 우회 및 [#497](https://github.com/microsoft/Foundry-Local/issues/497) 링크와 함께 업데이트됨. 7건 미해결 문제 유지.
- **javascript/foundry-local-whisper.mjs:** 정리 변수명 수정 (`whisperModel` → `model`)

### 검증됨
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — 정리 후 정상 실행
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — 정리 후 정상 실행
- C#: `dotnet build` 경고 0, 오류 0으로 성공 (net9.0 대상)
- 모든 7개 Python 파일 `py_compile` 구문 검사 통과
- 모든 7개 JavaScript 파일 `node --check` 구문 검사 통과

---

## 2026-03-10 — 파트 11: 도구 호출, SDK API 확장 및 모델 범위

### 추가됨
- **파트 11: 로컬 모델과 함께하는 도구 호출** — 도구 스키마, 다중 회차 흐름, 다중 도구 호출, 맞춤형 도구, ChatClient 도구 호출 및 `tool_choice`를 다루는 8개의 실습을 포함한 새로운 실습 안내서 (`labs/part11-tool-calling.md`)
- **Python 샘플:** `python/foundry-local-tool-calling.py` — OpenAI SDK를 사용한 `get_weather`/`get_population` 도구 호출
- **JavaScript 샘플:** `javascript/foundry-local-tool-calling.mjs` — SDK 네이티브 `ChatClient` (`model.createChatClient()`) 사용 도구 호출
- **C# 샘플:** `csharp/ToolCalling.cs` — OpenAI C# SDK를 사용한 `ChatTool.CreateFunctionTool()` 도구 호출
- **파트 2, 연습 7:** 네이티브 `ChatClient` — `model.createChatClient()` (JS)와 `model.GetChatClientAsync()` (C#)는 OpenAI SDK 대안
- **파트 2, 연습 8:** 모델 변형 및 하드웨어 선택 — `selectVariant()`, `variants`, NPU 변형 표(7개 모델)
- **파트 2, 연습 9:** 모델 업그레이드 및 카탈로그 갱신 — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **파트 2, 연습 10:** 추론 모델 — `<think>` 태그 파싱 예시가 포함된 `phi-4-mini-reasoning`
- **파트 3, 연습 4:** OpenAI SDK 대안으로 `createChatClient` 활용 및 스트리밍 콜백 패턴 문서화
- **AGENTS.md:** 도구 호출, ChatClient, 추론 모델 코딩 규칙 추가

### 변경됨
- **파트 1:** 모델 카탈로그 확장 — `phi-4-mini-reasoning`, `gpt-oss-20b`, `phi-4`, `qwen2.5-7b`, `qwen2.5-coder-7b`, `whisper-large-v3-turbo` 추가
- **파트 2:** API 참조 테이블 확장 — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync` 포함
- **파트 2:** 연습 7-9 → 10-13으로 번호 조정(새 연습 삽입 때문)
- **파트 3:** 주요 요약 테이블에 네이티브 ChatClient 포함 업데이트
- **README.md:** 파트 11 섹션 및 코드 샘플 테이블 추가; 학습 목표 #11 추가; 프로젝트 구조 트리 업데이트
- **csharp/Program.cs:** CLI 라우터에 `toolcall` 케이스 추가 및 도움말 텍스트 갱신

---

## 2026-03-09 — SDK v0.9.0 업데이트, 영국식 영어, 및 검증

### 변경됨
- **모든 코드 샘플(Python, JavaScript, C#):** Foundry Local SDK v0.9.0 API로 업데이트 — 누락된 `await` 추가(`await catalog.getModel()`), `FoundryLocalManager` 초기화 패턴 업데이트, 엔드포인트 검색 수정
- **모든 실습 안내서(파트 1-10):** 영국식 영어로 변환(예: colour, catalogue, optimised 등)
- **모든 실습 안내서:** v0.9.0 API 표면과 일치하도록 SDK 코드 예제 업데이트
- **모든 실습 안내서:** API 참조 테이블 및 연습 코드 블록 업데이트
- **JavaScript 중요 수정:** `catalog.getModel()` 호출에 누락된 `await` 추가 — `Model` 객체가 아닌 `Promise` 반환 문제로 하위 오류 방지

### 검증됨
- 모든 Python 샘플이 Foundry Local 서비스에서 정상 실행
- 모든 JavaScript 샘플이 Node.js 18+에서 정상 실행
- C# 프로젝트가 .NET 9.0에서 빌드 및 실행 (net8.0 SDK 조립체와의 전향 호환)
- 워크숍 전체에서 29개 파일 수정 및 검증 완료

---

## 파일 색인

| 파일 | 마지막 업데이트 | 설명 |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | 확장된 모델 카탈로그 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | 새로운 실습 7-10, 확장된 API 테이블 |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | 새 연습 4 (ChatClient), 업데이트된 요약 |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 및 영국식 영어 적용 |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 및 영국식 영어 적용 |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + 영국 영어 |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + 영국 영어 |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid 다이어그램 |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + 영국 영어 |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid 다이어그램, 워크숍 완료를 Part 11로 이동 |
| `labs/part11-tool-calling.md` | 2026-03-11 | 새로운 실습, Mermaid 다이어그램, 워크숍 완료 섹션 |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | 신규: 툴 호출 샘플 |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | 신규: 툴 호출 샘플 |
| `csharp/ToolCalling.cs` | 2026-03-10 | 신규: 툴 호출 샘플 |
| `csharp/Program.cs` | 2026-03-10 | `toolcall` CLI 명령어 추가 |
| `README.md` | 2026-03-10 | Part 11, 프로젝트 구조 |
| `AGENTS.md` | 2026-03-10 | 툴 호출 + ChatClient 규약 |
| `KNOWN-ISSUES.md` | 2026-03-11 | 해결된 이슈 #7 삭제, 6개의 열린 이슈 유지 |
| `csharp/csharp.csproj` | 2026-03-11 | 크로스 플랫폼 TFM, WinML/base SDK 조건부 참조 |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | 크로스 플랫폼 TFM, RID 자동 감지 |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | 크로스 플랫폼 TFM, RID 자동 감지 |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU try/catch 우회 제거 |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU try/catch 우회 제거 |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU try/catch 우회 제거 |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU try/catch 우회 제거 |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU try/catch 우회 제거 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | 크로스 플랫폼 .csproj 예제 |
| `AGENTS.md` | 2026-03-11 | C# 패키지 및 TFM 상세 정보 업데이트 |
| `CHANGELOG.md` | 2026-03-11 | 이 파일 |