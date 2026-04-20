![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 11: 로컬 모델을 사용한 도구 호출

> **목표:** 로컬 모델이 외부 함수(도구)를 호출하여 실시간 데이터를 검색하거나, 계산을 수행하거나, API와 상호작용하도록 하세요 — 모두 귀하의 장치에서 사적으로 실행됩니다.

## 도구 호출이란?

도구 호출(또는 **함수 호출**)은 언어 모델이 사용자가 정의한 함수의 실행을 요청할 수 있게 합니다. 모델이 답을 추측하는 대신, 도구가 도움이 될 때 이를 인식하고 귀하의 코드가 실행할 구조화된 요청을 반환합니다. 애플리케이션이 함수를 실행하여 결과를 보내주면 모델은 이를 최종 응답에 반영합니다.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

이 패턴은 다음과 같은 에이전트를 구축하는 데 필수적입니다:

- **실시간 데이터 조회** (날씨, 주식 가격, 데이터베이스 쿼리)
- **정확한 계산 수행** (수학, 단위 변환)
- **조치 수행** (이메일 전송, 티켓 생성, 기록 업데이트)
- **개인 시스템 접근** (내부 API, 파일 시스템)

---

## 도구 호출 작동 원리

도구 호출 흐름은 네 단계로 나뉩니다:

| 단계 | 수행 내용 |
|-------|-------------|
| **1. 도구 정의** | JSON Schema를 사용해 사용 가능한 함수의 이름, 설명, 매개변수를 기술 |
| **2. 모델 결정** | 사용자의 메시지와 도구 정의를 모델에 전달. 도구가 필요하면 텍스트 답변 대신 `tool_calls` 응답 반환 |
| **3. 로컬 실행** | 코드가 도구 호출을 파싱하고 함수 실행 후 결과 수집 |
| **4. 최종 답변** | 도구 결과를 모델에 보내 최종 응답 생성 |

> **핵심:** 모델은 코드를 직접 실행하지 않습니다. 단지 도구 호출을 <em>요청</em>할 뿐이며, 이 요청을 수락할지 여부는 애플리케이션이 결정하여 완전한 제어권을 유지합니다.

---

## 도구 호출을 지원하는 모델은?

모든 모델이 도구 호출을 지원하는 것은 아닙니다. 현재 Foundry Local 카탈로그에서 도구 호출 기능이 있는 모델은 다음과 같습니다:

| 모델 | 크기 | 도구 호출 |
|-------|------|:---:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b | 6.3 GB | ✅ |
| qwen2.5-14b | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b | 6.3 GB | ✅ |
| qwen2.5-coder-14b | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4 | 10.4 GB | ❌ |

> **팁:** 이번 실습에서는 <strong>qwen2.5-0.5b</strong>를 사용합니다 — 작고(다운로드 822 MB) 빠르며 신뢰할 수 있는 도구 호출을 지원합니다.

---

## 학습 목표

이 실습을 마치면 다음을 할 수 있습니다:

- 도구 호출 패턴과 AI 에이전트에 중요한 이유 설명
- OpenAI 함수 호출 형식으로 도구 스키마 정의
- 다중 턴 도구 호출 대화 흐름 처리
- 로컬에서 도구 호출을 실행하고 결과를 모델에 반환
- 도구 호출 시나리오에 적합한 모델 선택

---

## 전제 조건

| 요구 사항 | 상세 |
|-------------|---------|
| **Foundry Local CLI** | 설치되어 있고 `PATH`에 등록 ([Part 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript 또는 C# SDK 설치 ([Part 2](part2-foundry-local-sdk.md)) |
| **도구 호출 지원 모델** | qwen2.5-0.5b (자동 다운로드) |

---

## 실습

### 실습 1 — 도구 호출 흐름 이해하기

코드 작성 전에 이 시퀀스 다이어그램을 학습하세요:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**주요 관찰점:**

1. 도구는 JSON Schema 객체로 미리 정의됨  
2. 모델의 응답에는 일반 콘텐츠가 아닌 `tool_calls` 포함  
3. 각 도구 호출에는 결과를 반환할 때 참조해야 하는 고유 `id`가 있음  
4. 모델은 이전 메시지 <em>및</em> 도구 결과를 모두 보고 최종 답변 생성  
5. 하나의 응답에서 여러 도구 호출 가능  

> **토론:** 모델이 직접 함수를 실행하지 않고 도구 호출을 반환하는 이유는? 보안상 어떤 이점이 있나?

---

### 실습 2 — 도구 스키마 정의하기

도구는 표준 OpenAI 함수 호출 형식으로 정의됩니다. 각 도구는:

- **`type`**: 항상 `"function"`  
- **`function.name`**: 설명적인 함수 이름(예: `get_weather`)  
- **`function.description`**: 도구 호출 시 모델이 참고하는 명확한 설명  
- **`function.parameters`**: 기대하는 인자를 설명하는 JSON Schema 객체  

```json
{
  "type": "function",
  "function": {
    "name": "get_weather",
    "description": "Get the current weather for a given city",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. London"
        }
      },
      "required": ["city"]
    }
  }
}
```

> **도구 설명 모범 사례:**  
> - 구체적으로: "특정 도시의 현재 날씨를 가져오기"가 "날씨 가져오기"보다 좋음  
> - 매개변수를 명확히 설명: 모델이 올바른 값을 채우는 데 도움  
> - 필수와 선택 매개변수 구분 — 모델이 무엇을 물어야 할지 결정에 도움  

---

### 실습 3 — 도구 호출 예제 실행하기

각 언어별 샘플은 두 도구(`get_weather`, `get_population`)를 정의하고, 도구 호출을 유발하는 질문을 보내고, 로컬에서 실행 후 결과를 반환하여 최종 답변을 생성합니다.

<details>
<summary><strong>🐍 Python</strong></summary>

**전제 조건:**  
```bash
cd python
python -m venv venv

# 윈도우 (파워셸):
venv\Scripts\Activate.ps1
# 맥OS / 리눅스:
source venv/bin/activate

pip install -r requirements.txt
```

**실행:**  
```bash
python foundry-local-tool-calling.py
```

**예상 출력:**  
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**코드 설명** (`python/foundry-local-tool-calling.py`):

```python
# 도구들을 함수 스키마의 목록으로 정의합니다
tools = [
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "Get the current weather for a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "The city name"}
                },
                "required": ["city"]
            }
        }
    }
]

# 도구와 함께 전송 — 모델이 콘텐츠 대신 tool_calls를 반환할 수 있습니다
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# 모델이 도구를 호출하려고 하는지 확인합니다
if response.choices[0].message.tool_calls:
    # 도구를 실행하고 결과를 다시 전송합니다
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**전제 조건:**  
```bash
cd javascript
npm install
```

**실행:**  
```bash
node foundry-local-tool-calling.mjs
```

**예상 출력:**  
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**코드 설명** (`javascript/foundry-local-tool-calling.mjs`):

이 예제는 OpenAI SDK 대신 Foundry Local SDK의 `ChatClient`를 사용하며, `createChatClient()` 메서드로 편리함을 보여줍니다:

```javascript
// 모델 객체에서 직접 ChatClient를 가져옵니다
const chatClient = model.createChatClient();

// 도구로 전송 — ChatClient가 OpenAI 호환 형식을 처리합니다
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// 도구 호출을 확인합니다
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // 도구를 실행하고 결과를 다시 전송합니다
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**전제 조건:**  
```bash
cd csharp
dotnet restore
```

**실행:**  
```bash
dotnet run toolcall
```

**예상 출력:**  
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**코드 설명** (`csharp/ToolCalling.cs`):

C#은 `ChatTool.CreateFunctionTool` 헬퍼를 사용하여 도구를 정의합니다:

```csharp
ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
    functionName: "get_weather",
    functionDescription: "Get the current weather for a given city",
    functionParameters: BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "city": { "type": "string", "description": "The city name" }
        },
        "required": ["city"]
    }
    """));

var options = new ChatCompletionOptions();
options.Tools.Add(getWeatherTool);

// Check FinishReason to see if tools were called
if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    // Execute tools and send results back
    ...
}
```

</details>

---

### 실습 4 — 도구 호출 대화 흐름

메시지 구조를 이해하는 것이 중요합니다. 각 단계의 `messages` 배열 전체 흐름입니다:

**1단계 — 초기 요청:**  
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**2단계 — 모델이 tool_calls로 응답 (콘텐츠 아님):**  
```json
{
  "role": "assistant",
  "tool_calls": [
    {
      "id": "call_abc123",
      "type": "function",
      "function": {
        "name": "get_weather",
        "arguments": "{\"city\": \"London\"}"
      }
    }
  ]
}
```

**3단계 — 조수 메시지 및 도구 결과 추가:**  
```json
[
  {"role": "system", "content": "..."},
  {"role": "user", "content": "What is the weather like in London?"},
  {"role": "assistant", "tool_calls": [...]},
  {
    "role": "tool",
    "tool_call_id": "call_abc123",
    "content": "{\"city\": \"London\", \"temperature\": \"18°C\", \"condition\": \"Partly cloudy\"}"
  }
]
```

**4단계 — 도구 결과로 최종 답변 생성**

> **중요:** 도구 메시지의 `tool_call_id`는 도구 호출의 `id`와 일치해야 합니다. 그래야 모델이 결과와 요청을 연결합니다.

---

### 실습 5 — 다중 도구 호출

모델은 한 응답에서 여러 도구 호출을 요청할 수 있습니다. 사용자 메시지를 변경하여 여러 호출을 유도하세요:

```python
# 파이썬에서 — 사용자 메시지 변경:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// JavaScript에서 — 사용자 메시지를 변경하세요:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

모델은 `get_weather`와 `get_population` 두 개의 `tool_calls`를 반환해야 합니다. 귀하의 코드는 모든 도구 호출을 반복 처리하므로 이미 지원합니다.

> **시도해보기:** 사용자 메시지 수정 후 샘플을 다시 실행해 보세요. 모델이 두 도구 모두 호출하나요?

---

### 실습 6 — 자신만의 도구 추가하기

샘플 중 하나에 새 도구를 확장해 보세요. 예: `get_time` 도구 추가:

1. 도구 스키마 정의:  
```json
{
  "type": "function",
  "function": {
    "name": "get_time",
    "description": "Get the current time in a given city's timezone",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. Tokyo"
        }
      },
      "required": ["city"]
    }
  }
}
```

2. 실행 로직 추가:  
```python
# 파이썬
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # 실제 앱에서는 타임존 라이브러리를 사용하세요
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... 기존 도구 ...
```

3. `tools` 배열에 도구 추가 후 "도쿄의 시간은?" 질문으로 테스트

> **도전 과제:** 섭씨와 화씨 간 변환하는 `convert_temperature`와 같은 계산 도구 추가. "100°F를 섭씨로 변환해줘"로 테스트하세요.

---

### 실습 7 — SDK의 ChatClient로 도구 호출 (JavaScript)

JavaScript 샘플은 이미 OpenAI SDK 대신 SDK 자체의 `ChatClient`를 사용합니다. 이 기능은 OpenAI 클라이언트를 직접 생성하지 않아도 되는 편리함을 제공합니다:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient는 모델 객체에서 직접 생성됩니다
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat은 두 번째 매개변수로 도구들을 받습니다
const response = await chatClient.completeChat(messages, tools);
```

파이썬 예제와 비교하면 OpenAI SDK를 명시적으로 사용하는 방식입니다:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

두 패턴은 모두 유효합니다. `ChatClient`가 더 편리하며 OpenAI SDK는 모든 OpenAI 매개변수에 접근할 수 있습니다.

> **시도해보기:** JavaScript 샘플을 `ChatClient` 대신 OpenAI SDK를 사용하도록 변경하세요. `import OpenAI from "openai"` 후 `manager.urls[0]`의 엔드포인트로 클라이언트를 구성해야 합니다.

---

### 실습 8 — tool_choice 이해하기

`tool_choice` 매개변수는 모델이 도구를 <em>반드시</em> 사용해야 하는지 또는 자유롭게 선택할 수 있는지 조절합니다:

| 값 | 동작 |
|-------|-----------|
| `"auto"` | 모델이 도구 호출 여부를 결정 (기본값) |
| `"none"` | 모델이 도구를 절대 호출하지 않음 |
| `"required"` | 모델이 최소 하나의 도구를 호출해야 함 |
| `{"type": "function", "function": {"name": "get_weather"}}` | 모델이 지정한 도구만 호출해야 함 |

파이썬 샘플에서 각 옵션을 시도해 보세요:

```python
# 모델이 get_weather를 호출하도록 강제합니다
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **참고:** 모든 모델이 모든 `tool_choice` 옵션을 지원하는 것은 아닙니다. `"required"`를 지원하지 않는 경우, 설정을 무시하고 `"auto"`처럼 동작할 수 있습니다.

---

## 흔한 문제점

| 문제 | 해결책 |
|---------|----------|
| 모델이 도구를 호출하지 않음 | 도구 호출 모델(qwen2.5-0.5b 등) 사용 여부 확인 |
| `tool_call_id` 불일치 | 도구 호출 응답의 `id`를 반드시 사용, 하드코딩 금지 |
| `arguments` 내 JSON 오류 반환 | 작은 모델은 비정상 JSON 생성 가능. `JSON.parse()`를 try/catch로 감싸기 |
| 존재하지 않는 도구 호출 | `execute_tool` 함수에 기본 핸들러 추가 |
| 무한 도구 호출 루프 | 최대 호출 횟수 제한 설정(e.g. 5회)으로 무한 반복 방지 |

---

## 주요 요점

1. <strong>도구 호출</strong>은 모델이 답을 추측하는 대신 함수 실행 요청을 하도록 함  
2. 모델은 **코드를 직접 실행하지 않고**, 실행 여부는 애플리케이션이 결정  
3. 도구는 OpenAI 함수 호출 형식을 따르는 **JSON Schema** 객체로 정의  
4. 대화는 <strong>다중 턴 패턴</strong>을 사용: 사용자 → 조수(tool_calls) → 도구(결과) → 조수(최종 답변)  
5. 반드시 **도구 호출 지원 모델** 사용(Qwen 2.5, Phi-4-mini)  
6. SDK의 `createChatClient()`는 OpenAI 클라이언트 생성 없이 도구 호출 요청 편리하게 지원

---

[Zava Creative Writer를 위한 웹 UI 구축 — Part 12](part12-zava-ui.md) 로 진행하여 실제 스트리밍과 다중 에이전트 파이프라인용 브라우저 프런트엔드를 추가하세요.

---

[← Part 10: Custom Models](part10-custom-models.md) | [Part 12: Zava Writer UI →](part12-zava-ui.md)