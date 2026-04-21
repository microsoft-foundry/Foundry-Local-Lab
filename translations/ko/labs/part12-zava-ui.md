![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 파트 12: Zava Creative Writer를 위한 웹 UI 구축

> **목표:** Zava Creative Writer에 브라우저 기반 프론트엔드를 추가하여 다중 에이전트 파이프라인이 실시간으로 실행되는 것을 보고, 라이브 에이전트 상태 배지와 스트리밍되는 기사 텍스트를 단일 로컬 웹 서버에서 모두 제공할 수 있도록 합니다.

[파트 7](part7-zava-creative-writer.md)에서 Zava Creative Writer를 **CLI 애플리케이션**(JavaScript, C#)과 **헤드리스 API**(Python)로 탐색했습니다. 이번 실습에서는 공유하는 **순수 HTML/CSS/JavaScript** 프론트엔드를 각 백엔드에 연결하여 사용자가 터미널 대신 브라우저를 통해 파이프라인과 상호작용할 수 있게 합니다.

---

## 배울 내용

| 목표 | 설명 |
|-----------|-------------|
| 백엔드에서 정적 파일 서빙 | API 라우트와 함께 HTML/CSS/JS 디렉터리를 마운트하기 |
| 브라우저에서 스트리밍 NDJSON 소비 | Fetch API와 `ReadableStream`을 사용하여 개행 구분 JSON 읽기 |
| 통합 스트리밍 프로토콜 | Python, JavaScript, C# 백엔드가 동일한 메시지 형식 방출하도록 보장 |
| 점진적 UI 업데이트 | 에이전트 상태 배지 업데이트 및 기사 텍스트를 토큰 단위로 스트리밍 |
| CLI 앱에 HTTP 레이어 추가 | 기존 조율기 로직을 Express 스타일 서버(JS) 또는 ASP.NET Core 최소 API(C#)로 감싸기 |

---

## 아키텍처

UI는 세 백엔드가 공유하는 단일 정적 파일 세트(`index.html`, `style.css`, `app.js`)입니다. 각 백엔드는 동일한 두 경로를 노출합니다:

![Zava UI 아키텍처 — 세 백엔드가 공유하는 프론트엔드](../../../images/part12-architecture.svg)

| 경로 | 메서드 | 용도 |
|-------|--------|---------|
| `/` | GET | 정적 UI 제공 |
| `/api/article` | POST | 다중 에이전트 파이프라인 실행 및 NDJSON 스트리밍 |

프론트엔드는 JSON 본문을 보내고 응답을 개행 구분된 JSON 메시지 스트림으로 읽습니다. 각 메시지는 UI가 올바른 패널을 업데이트할 때 사용하는 `type` 필드를 갖습니다:

| 메시지 유형 | 의미 |
|-------------|---------|
| `message` | 상태 업데이트 (예: "연구 에이전트 작업 시작 중...") |
| `researcher` | 연구 결과 준비 완료 |
| `marketing` | 제품 검색 결과 준비 완료 |
| `writer` | 작성기 시작 또는 완료 ( `{ start: true }` 또는 `{ complete: true }` 포함) |
| `partial` | 작성기로부터 단일 스트리밍된 토큰 ( `{ text: "..." }` 포함) |
| `editor` | 편집자 판정 완료 |
| `error` | 오류 발생 |

![브라우저 내 메시지 타입 라우팅](../../../images/part12-message-types.svg)

![스트리밍 순서 — 브라우저와 백엔드 간 통신](../../../images/part12-streaming-sequence.svg)

---

## 사전 준비 사항

- [파트 7: Zava Creative Writer](part7-zava-creative-writer.md) 완료
- Foundry Local CLI 설치 및 `phi-3.5-mini` 모델 다운로드
- 최신 웹 브라우저(Chrome, Edge, Firefox, Safari)

---

## 공유 UI

백엔드 코드를 건드리기 전에, 세 언어 트랙 모두 사용할 프론트엔드 구성을 살펴보세요. 파일들은 `zava-creative-writer-local/ui/`에 있습니다:

| 파일 | 용도 |
|------|---------|
| `index.html` | 페이지 레이아웃: 입력 폼, 에이전트 상태 배지, 기사 출력 영역, 접이식 상세 패널 |
| `style.css` | 상태 배지 색상 상태(대기, 실행 중, 완료, 오류)를 가진 최소한의 스타일링 |
| `app.js` | Fetch 호출, `ReadableStream` 라인 리더, DOM 업데이트 로직 |

> **팁:** `index.html`을 브라우저에서 직접 열어 레이아웃을 미리 볼 수 있습니다. 백엔드가 없기 때문에 아직 아무것도 작동하지 않지만 구조를 확인할 수 있습니다.

### 스트림 리더 동작 방식

`app.js`의 핵심 함수는 응답 본문을 청크 단위로 읽고 개행 경계에서 분할합니다:

```javascript
async function readStream(response) {
  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = "";

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;

    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split("\n");
    buffer = lines.pop(); // 미완성된 마지막 줄을 유지합니다

    for (const line of lines) {
      const trimmed = line.trim();
      if (!trimmed) continue;
      try {
        const msg = JSON.parse(trimmed);
        if (msg && msg.type) handleMessage(msg);
      } catch { /* skip non-JSON lines */ }
    }
  }
}
```

파싱된 각 메시지는 `handleMessage()`로 보내지며, `msg.type`에 따라 관련 DOM 요소를 업데이트합니다.

---

## 실습

### 실습 1: UI와 함께 Python 백엔드 실행하기

Python(FastAPI) 변형은 이미 스트리밍 API 엔드포인트가 있습니다. 유일한 변경점은 `ui/` 폴더를 정적 파일로 마운트하는 것입니다.

**1.1** Python API 디렉터리로 이동해 종속성 설치:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** 서버 시작:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** 브라우저에서 `http://localhost:8000` 열기. 세 개의 텍스트 필드와 "Generate Article" 버튼이 있는 Zava Creative Writer UI가 보일 것입니다.

**1.4** 기본값으로 **Generate Article** 클릭. 각 에이전트가 작업을 완료함에 따라 상태 배지가 "Waiting"에서 "Running" 그리고 "Done"으로 바뀌고, 기사 텍스트가 토큰 단위로 출력 패널에 스트리밍되는 것을 확인하세요.

> **문제 해결:** UI 대신 JSON 응답이 보인다면, 정적 파일을 마운트한 최신 `main.py`를 실행 중인지 확인하세요. `/api/article` 엔드포인트는 원래 경로에서 계속 작동하며, 정적 파일 마운트가 다른 모든 경로에서 UI를 제공합니다.

**작동 원리:** 업데이트된 `main.py`는 하단에 한 줄을 추가합니다:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

이는 `zava-creative-writer-local/ui/`의 모든 파일을 정적 자산으로 제공하며, 기본 문서는 `index.html`입니다. `/api/article` POST 라우트는 정적 마운트 이전에 등록되어 우선순위를 가집니다.

---

### 실습 2: JavaScript 변형에 웹 서버 추가하기

JavaScript 변형은 현재 CLI 애플리케이션(`main.mjs`)입니다. 새 파일 `server.mjs`가 동일한 에이전트 모듈을 HTTP 서버 뒤에 래핑하고 공유 UI를 제공합니다.

**2.1** JavaScript 디렉터리로 이동해 종속성 설치:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** 웹 서버 시작:

```bash
node server.mjs
```

```powershell
node server.mjs
```

다음과 같은 메시지가 표시될 것입니다:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** 브라우저에서 `http://localhost:3000` 열고 **Generate Article** 클릭. 동일 UI가 JavaScript 백엔드에서도 똑같이 작동합니다.

**코드 공부하기:** `server.mjs`를 열어 주요 패턴을 살펴보세요:

- <strong>정적 파일 서빙</strong>은 Node.js 내장 `http`, `fs`, `path` 모듈을 사용하며 외부 프레임워크 불필요.
- <strong>경로 순회 보호</strong>는 요청 경로를 정규화하고 `ui/` 디렉터리 내에 있는지 확인.
- <strong>NDJSON 스트리밍</strong>은 각 객체를 직렬화하고 내부 개행을 제거하며, 끝에 개행 문자를 추가하는 `sendLine()` 헬퍼 함수 사용.
- <strong>에이전트 조율</strong>은 기존 `researcher.mjs`, `product.mjs`, `writer.mjs`, `editor.mjs` 모듈을 변경 없이 재사용.

<details>
<summary>server.mjs 주요 발췌</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// 연구원
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// 작가 (스트리밍)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### 실습 3: C# 변형에 최소 API 추가하기

C# 변형은 현재 콘솔 애플리케이션입니다. 새 프로젝트 `csharp-web`은 ASP.NET Core 최소 API를 사용해 동일 파이프라인을 웹 서비스로 제공합니다.

**3.1** C# 웹 프로젝트로 이동해 패키지 복원:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** 웹 서버 실행:

```bash
dotnet run
```

```powershell
dotnet run
```

다음과 같은 메시지가 표시될 것입니다:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** 브라우저에서 `http://localhost:5000` 열고 **Generate Article** 클릭.

**코드 공부하기:** `csharp-web` 디렉터리 내 `Program.cs` 열어 다음 사항을 살펴보세요:

- 프로젝트 파일은 `Microsoft.NET.Sdk` 대신 `Microsoft.NET.Sdk.Web`을 사용해 ASP.NET Core 지원 추가.
- 정적 파일은 공유 `ui/` 디렉터리를 가리키는 `UseDefaultFiles`와 `UseStaticFiles`로 제공.
- `/api/article` 엔드포인트는 NDJSON 라인을 `HttpContext.Response`에 직접 기록하고 각 줄 뒤에 플러시하여 실시간 스트리밍 보장.
- 모든 에이전트 로직(`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`)은 콘솔 버전과 동일.

<details>
<summary>csharp-web/Program.cs 주요 발췌</summary>

```csharp
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/event-stream; charset=utf-8";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    // Researcher
    await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
    var researchResult = RunResearcher(body.Research, feedback);
    await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

    // Writer (streaming)
    foreach (var update in completionUpdates)
    {
        if (update.ContentUpdate.Count > 0)
        {
            var text = update.ContentUpdate[0].Text;
            await SendLine(new { type = "partial", message = "token", data = new { text } });
        }
    }
});
```

</details>

---

### 실습 4: 에이전트 상태 배지 탐색

작동하는 UI를 갖췄으니 프론트엔드가 상태 배지를 어떻게 업데이트하는지 살펴보세요.

**4.1** 편집기에서 `zava-creative-writer-local/ui/app.js` 열기.

**4.2** `handleMessage()` 함수 찾기. 메시지 타입을 DOM 업데이트에 매핑하는 방식을 확인:

| 메시지 유형 | UI 동작 |
|-------------|-----------|
| "researcher" 포함된 `message` | 연구 에이전트 배지를 "Running"으로 설정 |
| `researcher` | 연구 에이전트 배지를 "Done"으로 설정하고 연구 결과 패널 채우기 |
| `marketing` | 제품 검색 배지를 "Done"으로 설정하고 제품 매칭 패널 채우기 |
| `writer` + `data.start` | 작성기 배지를 "Running"으로 설정하고 기사 출력 초기화 |
| `partial` | 토큰 텍스트를 기사 출력에 추가 |
| `writer` + `data.complete` | 작성기 배지를 "Done"으로 설정 |
| `editor` | 편집기 배지를 "Done"으로 설정하고 편집자 피드백 패널 채우기 |

**4.3** 기사 아래의 접이식 "Research Results", "Product Matches", "Editor Feedback" 패널을 열어 각 에이전트가 생성한 원시 JSON을 확인하세요.

---

### 실습 5: UI 사용자 지정 (확장)

다음 개선 사항 중 하나 이상을 시도해 보세요:

**5.1 단어 수 추가.** 작성기가 완료된 후 출력 패널 아래에 기사 단어 수 표시. `handleMessage`에서 `type === "writer"`이고 `data.complete`이 참일 때 계산할 수 있습니다:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 재시도 표시기 추가.** 편집자가 수정을 요청하면 파이프라인이 재실행됩니다. 상태 패널에 "Revision 1", "Revision 2" 배너를 표시하세요. "Revision"을 포함하는 `message` 타입을 듣고 새 DOM 요소를 업데이트합니다.

**5.3 다크 모드.** 전환 버튼과 `<body>`에 `.dark` 클래스를 추가하세요. `style.css`에서 `body.dark` 선택자로 배경, 텍스트, 패널 색상을 재정의합니다.

---

## 요약

| 수행 내용 | 방법 |
|-------------|-----|
| Python 백엔드에서 UI 서빙 | FastAPI의 `StaticFiles`로 `ui/` 폴더 마운트 |
| JavaScript 변형에 HTTP 서버 추가 | 내장 Node.js `http` 모듈로 `server.mjs` 생성 |
| C# 변형에 웹 API 추가 | ASP.NET Core 최소 API로 새 `csharp-web` 프로젝트 생성 |
| 브라우저에서 스트리밍 NDJSON 소비 | `fetch()`와 `ReadableStream`, 행 단위 JSON 파싱 사용 |
| UI 실시간 업데이트 | 메시지 유형을 DOM 업데이트(배지, 텍스트, 접이식 패널)에 매핑 |

---

## 핵심 내용

1. <strong>공유 정적 프론트엔드</strong>는 동일 스트리밍 프로토콜을 사용하는 모든 백엔드와 작동하며, OpenAI 호환 API 패턴의 가치를 강화합니다.
2. <strong>개행 구분 JSON(NDJSON)</strong>은 브라우저 `ReadableStream` API와 네이티브로 작동하는 간단한 스트리밍 형식입니다.
3. **Python 변형은** 이미 FastAPI 엔드포인트가 있어 변경이 가장 적었고, JavaScript와 C# 변형은 얇은 HTTP 래퍼가 필요했습니다.
4. UI를 <strong>순수 HTML/CSS/JS</strong>로 유지하여 빌드 도구, 프레임워크 의존성, 학습자용 복잡성을 줄입니다.
5. 동일한 에이전트 모듈(Researcher, Product, Writer, Editor)을 변경 없이 재사용하며, 운송 계층만 변경합니다.

---

## 추가 자료

| 자료 | 링크 |
|----------|------|
| MDN: Readable Streams 사용법 | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI 정적 파일 | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core 정적 파일 | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON 명세 | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

[파트 13: 워크숍 완료](part13-workshop-complete.md)에서 이번 워크숍 전체에서 만든 내용을 요약해 보세요.

---
[← 11부: 도구 호출](part11-tool-calling.md) | [13부: 워크숍 완료 →](part13-workshop-complete.md)