![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 8: Foundry Local과 함께 하는 평가 주도 개발

> **목표:** 에이전트와 심판 모두에 동일한 로컬 모델을 사용하여 AI 에이전트를 체계적으로 테스트하고 점수를 매기는 평가 프레임워크를 구축하여, 자신 있게 프롬프트를 반복 개선하고 배포하세요.

## 평가 주도 개발이 중요한 이유

AI 에이전트를 만들 때 '대충 제대로 된 것 같다'는 것은 충분하지 않습니다. <strong>평가 주도 개발</strong>은 에이전트 출력물을 코드처럼 다룹니다: 먼저 테스트를 작성하고, 품질을 측정하며, 점수가 기준에 도달했을 때만 배포합니다.

Zava 크리에이티브 라이터(Part 7)에서, <strong>에디터 에이전트</strong>는 이미 간단한 평가자 역할을 하며, ACCEPT 또는 REVISE를 결정했습니다. 이 랩은 그 패턴을 정형화하여 어떤 에이전트나 파이프라인에도 적용 가능한 반복 가능한 평가 프레임워크로 만듭니다.

| 문제 | 해결책 |
|---------|----------|
| 프롬프트 변경이 품질을 조용히 깨뜨림 | <strong>골든 데이터셋</strong>으로 회귀 검출 |
| "한 예제에서 잘 작동" 편향 | <strong>여러 테스트 케이스</strong>로 엣지 케이스 확인 |
| 주관적 품질 평가 | <strong>규칙 기반 + LLM 심판 점수</strong>로 수치화 |
| 프롬프트 변종 비교 방법 없음 | <strong>나란히 평가 실행</strong>과 종합 점수 제공 |

---

## 주요 개념

### 1. 골든 데이터셋

<strong>골든 데이터셋</strong>은 예상 출력이 알려진 테스트 케이스 모음입니다. 각 테스트 케이스는 다음을 포함합니다:

- <strong>입력</strong>: 에이전트에 보낼 프롬프트 또는 질문
- **예상 출력**: 올바르거나 높은 품질의 응답에 포함되어야 할 내용(키워드, 구조, 사실 등)
- <strong>카테고리</strong>: 보고를 위한 그룹화(예: "사실 정확성", "톤", "완성도")

### 2. 규칙 기반 검사

빠르고 결정론적이며 LLM이 필요 없는 검사:

| 검사 | 무엇을 검사하는가 |
|-------|--------------|
| **길이 한계** | 응답이 너무 짧음(게으름) 또는 너무 길음(지루함) 아님 |
| **필수 키워드** | 응답에 예상 용어 또는 개체가 포함됨 |
| **형식 검증** | JSON 파싱 가능, 마크다운 헤더 존재 |
| **금지된 내용** | 환각된 브랜드명 없음, 경쟁사 언급 없음 |

### 3. LLM 심판

같은 로컬 모델을 사용해 자체 출력(또는 다른 프롬프트 변종의 출력)을 평가합니다. 심판에게는 다음이 전달됩니다:

- 원래 질문
- 에이전트의 응답
- 평가 기준

구조화된 점수를 반환합니다. 이는 Part 7의 에디터 패턴과 유사하지만 테스트 스위트 전반에 체계적으로 적용됩니다.

### 4. 평가 주도 반복 루프

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## 필수 사항

| 요구사항 | 세부 내용 |
|-------------|---------|
| **Foundry Local CLI** | 모델 다운로드 후 설치 완료 |
| **언어 런타임** | **Python 3.9+** 및/또는 **Node.js 18+** 및/또는 **.NET 9+ SDK** |
| **완료 여부** | [Part 5: 싱글 에이전트](part5-single-agents.md), [Part 6: 다중 에이전트 워크플로우](part6-multi-agent-workflows.md) 완료 |

---

## 실습 과제

### 과제 1 - 평가 프레임워크 실행

워크숍에는 Foundry Local 에이전트를 Zava DIY 관련 질문의 골든 데이터셋에 대해 테스트하는 완전한 평가 샘플이 포함되어 있습니다.

<details>
<summary><strong>🐍 Python</strong></summary>

**설정:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**실행:**
```bash
python foundry-local-eval.py
```

**실행 내용:**
1. Foundry Local에 연결하여 모델 로드
2. 5개의 테스트 케이스로 이루어진 골든 데이터셋 정의(Zava 제품 질문)
3. 각 테스트 케이스에 대해 2개의 프롬프트 변종 실행
4. 각 응답에 대해 **규칙 기반 검사** 수행(길이, 키워드, 형식)
5. 각 응답에 대해 **LLM 심판 점수** 매김(동일 모델이 1-5 점으로 품질 판단)
6. 두 프롬프트 변종 점수를 비교한 점수표 출력

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**설정:**
```bash
cd javascript
npm install
```

**실행:**
```bash
node foundry-local-eval.mjs
```

**동일 평가 파이프라인:** 골든 데이터셋, 이중 프롬프트 실행, 규칙 + LLM 점수, 점수표 출력.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**설정:**
```bash
cd csharp
dotnet restore
```

**실행:**
```bash
dotnet run eval
```

**동일 평가 파이프라인:** 골든 데이터셋, 이중 프롬프트 실행, 규칙 + LLM 점수, 점수표 출력.

</details>

---

### 과제 2 - 골든 데이터셋 이해하기

평가 샘플에서 정의된 테스트 케이스를 살펴보세요. 각 테스트 케이스는 다음과 같습니다:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**고려할 질문들:**
1. 왜 예상 값이 전체 문장이 아닌 <strong>키워드</strong>일까요?
2. 신뢰할 만한 평가를 위해 몇 개의 테스트 케이스가 필요할까요?
3. 자신의 애플리케이션에서는 어떤 카테고리를 추가하겠습니까?

---

### 과제 3 - 규칙 기반과 LLM 심판 점수 이해하기

평가 프레임워크는 <strong>두 가지 점수 체계</strong>를 사용합니다:

#### 규칙 기반 검사 (빠르고 결정론적)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM 심판 (미묘하고 정성적)

동일한 로컬 모델이 구조화된 루브릭과 함께 심판 역할을 합니다:

```
Rate this response on a scale of 1-5:
- 1: Completely wrong or irrelevant
- 2: Partially correct but missing key information
- 3: Adequate but could be improved
- 4: Good response with minor issues
- 5: Excellent, comprehensive, well-structured

Score: 4
Reasoning: The response correctly identifies all necessary tools
and provides practical advice, but could include safety equipment.
```

**고려할 질문들:**
1. 언제 LLM 심판보다 규칙 기반 검사를 더 신뢰하겠습니까?
2. 모델이 자신의 출력을 신뢰성 있게 판단할 수 있을까요? 한계는 무엇인가요?
3. Part 7의 에디터 에이전트 패턴과는 어떻게 비교되나요?

---

### 과제 4 - 프롬프트 변종 비교하기

샘플은 동일한 테스트 케이스에 대해 <strong>두 개의 프롬프트 변종</strong>을 실행합니다:

| 변종 | 시스템 프롬프트 스타일 |
|---------|-------------------|
| <strong>기본</strong> | 일반적: "You are a helpful assistant" |
| <strong>전문가형</strong> | 상세: "You are a Zava DIY expert who recommends specific products and provides step-by-step guidance" |

실행 후 다음과 같은 점수표를 볼 수 있습니다:

```
╔══════════════════════════════════════════════════════════════╗
║                    EVALUATION SCORECARD                      ║
╠══════════════════════════════════════════════════════════════╣
║ Prompt Variant    │ Rule Score │ LLM Score │ Combined       ║
╠═══════════════════╪════════════╪═══════════╪════════════════╣
║ Baseline          │ 0.62       │ 3.2 / 5   │ 0.62           ║
║ Specialised       │ 0.81       │ 4.1 / 5   │ 0.81           ║
╚══════════════════════════════════════════════════════════════╝
```

**과제:**
1. 평가를 실행하고 각 변종의 점수를 기록하세요.
2. 전문가형 프롬프트를 보다 구체적으로 수정해 보세요. 점수가 향상되나요?
3. 세 번째 프롬프트 변종을 추가하고 세 가지를 모두 비교하세요.
4. 모델 별칭(e.g. `phi-4-mini` vs `phi-3.5-mini`)을 변경하고 결과를 비교하세요.

---

### 과제 5 - 자신의 에이전트에 평가 적용하기

평가 프레임워크를 자신의 에이전트 적용 템플릿으로 사용하세요:

1. **골든 데이터셋 정의:** 예상 키워드와 함께 5~10개의 테스트 케이스 작성
2. **시스템 프롬프트 작성:** 테스트할 에이전트 지침 작성
3. **평가 실행:** 기본 점수 획득
4. **반복:** 프롬프트 조정, 재실행, 비교
5. **기준 설정:** 예: "총점 0.75 미만은 배포하지 않음"

---

### 과제 6 - Zava 에디터 패턴과 연결하기

Zava 크리에이티브 라이터의 에디터 에이전트 (`zava-creative-writer-local/src/api/agents/editor/editor.py`)를 다시 살펴보세요:

```python
# 편집기는 실제 운영 중인 LLM-판사입니다:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

이는 Part 8의 LLM 심판과 <strong>동일한 개념</strong>이지만 오프라인 테스트 스위트가 아닌 프로덕션 파이프라인 내에 통합된 것입니다. 두 패턴 모두:

- 모델에서 구조화된 JSON 출력을 사용
- 시스템 프롬프트에 정의된 품질 기준 적용
- 합/불합 결정 수행

**차이점:** 에디터는 프로덕션에서(모든 요청 시) 실행되고, 평가 프레임워크는 개발 단계에서(배포 전)에 실행됩니다.

---

## 주요 정리

| 개념 | 요점 |
|---------|----------|
| **골든 데이터셋** | 조기에 테스트 케이스를 선별; AI 회귀 테스트로 활용 |
| **규칙 기반 검사** | 빠르고 결정론적, 명백한 실패(길이, 키워드, 형식) 검출 |
| **LLM 심판** | 동일 로컬 모델을 활용한 미묘한 품질 점수 부여 |
| **프롬프트 비교** | 동일 테스트 스위트에 여러 변종 돌려 최적 변종 선택 |
| **온디바이스 이점** | 모든 평가가 로컬에서 실행됨: API 비용·속도 제한·데이터 유출 없음 |
| **배포 전 평가** | 품질 기준을 정하고 평가 점수로 릴리스 제한 설정 |

---

## 다음 단계

- **스케일 업:** 골든 데이터셋에 테스트 케이스와 카테고리 추가
- **자동화:** 평가를 CI/CD 파이프라인에 통합
- **고급 심판:** 더 큰 모델을 심판으로 사용하고 작은 모델 출력 평가
- **시간 추적:** 평가 결과를 저장해 프롬프트와 모델 버전 간 비교

---

## 다음 랩

Foundry Local SDK를 사용한 온디바이스 음성-텍스트 변환을 탐구하는 [Part 9: Whisper 음성 전사](part9-whisper-voice-transcription.md)로 계속 진행하세요.