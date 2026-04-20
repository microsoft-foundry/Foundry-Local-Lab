# 알려진 문제 — Foundry Local 워크숍

Windows에서 실행 중인 **Snapdragon X Elite (ARM64)** 장치에서 Foundry Local SDK v0.9.0, CLI v0.8.117, .NET SDK 10.0을 사용하여 이 워크숍을 빌드하고 테스트하는 동안 발견된 문제들입니다.

> **최종 검증일:** 2026-03-11

---

## 1. Snapdragon X Elite CPU가 ONNX Runtime에서 인식되지 않음

**상태:** 오픈  
**심각도:** 경고 (비차단)  
**구성요소:** ONNX Runtime / cpuinfo  
**재현:** Snapdragon X Elite 하드웨어에서 Foundry Local 서비스가 시작될 때마다

Foundry Local 서비스가 시작될 때마다 두 개의 경고가 출력됩니다:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**영향:** 이 경고는 외관상의 문제일 뿐이며, 추론은 정상적으로 작동합니다. 그러나 모든 실행 시마다 나타나 워크숍 참가자들을 혼동시킬 수 있습니다. ONNX Runtime cpuinfo 라이브러리를 업데이트하여 Qualcomm Oryon CPU 코어를 인식하도록 해야 합니다.

**기대 결과:** Snapdragon X Elite는 오류 수준 메시지 없이 지원되는 ARM64 CPU로 인식되어야 합니다.

---

## 2. SingleAgent 첫 실행 시 NullReferenceException 발생

**상태:** 오픈 (간헐적)  
**심각도:** 치명적 (충돌)  
**구성요소:** Foundry Local C# SDK + Microsoft Agent Framework  
**재현:** `dotnet run agent` 실행 — 모델 로드 직후 즉시 충돌

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**상황:** 37번째 줄에서 `model.IsCachedAsync(default)`를 호출합니다. 이 충돌은 fresh `foundry service stop` 후 에이전트 첫 실행 시 발생하였으며, 이후 동일 코드로는 정상 실행되었습니다.

**영향:** 간헐적 문제 — SDK 서비스 초기화 또는 카탈로그 조회 과정에서 경쟁 조건(race condition)이 의심됩니다. `GetModelAsync()` 호출이 서비스 준비 완료 이전에 반환될 수 있습니다.

**기대 결과:** `GetModelAsync()`는 서비스가 준비될 때까지 대기하거나, 초기화가 완료되지 않은 경우 명확한 오류 메시지를 반환해야 합니다.

---

## 3. C# SDK는 명시적인 RuntimeIdentifier 필요

**상태:** 오픈 — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)에서 추적 중  
**심각도:** 문서 부족  
**구성요소:** `Microsoft.AI.Foundry.Local` NuGet 패키지  
**재현:** `.csproj` 파일에 `<RuntimeIdentifier>` 없이 .NET 8+ 프로젝트 생성

빌드 실패 메시지:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**근본 원인:** RID 지정 요구사항은 예상된 것입니다 — SDK는 네이티브 바이너리(P/Invoke로 `Microsoft.AI.Foundry.Local.Core`와 ONNX Runtime 사용)를 제공하므로 .NET이 플랫폼별 라이브러리를 식별할 수 있어야 합니다.

이는 MS Learn 문서([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp))에도 기술되어 있으며, 실행 지침에는 다음과 같이 나옵니다:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
그러나 사용자는 매번 `-r` 플래그를 기억해야 하며, 쉽게 잊어버릴 수 있습니다.

**해결책:** `.csproj`에 자동 감지 폴백을 추가하여 `dotnet run`이 플래그 없이도 작동하게 합니다:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)`는 호스트 머신의 RID를 자동으로 해결하는 MSBuild 내장 속성입니다. SDK의 자체 테스트 프로젝트가 이미 이 패턴을 사용합니다. 명시적인 `-r` 플래그가 제공되면 그대로 존중됩니다.

> **참고:** 워크숍 `.csproj`에는 이 폴백이 포함되어 있어서 어떤 플랫폼에서도 `dotnet run`이 즉시 작동합니다.

**기대 결과:** MS Learn 문서 내 `.csproj` 템플릿에 이 자동 감지 패턴을 포함하여 사용자가 `-r` 플래그를 기억하지 않아도 되도록 해야 합니다.

---

## 4. JavaScript Whisper — 오디오 전사 결과가 비어 있거나 바이너리 출력

**상태:** 오픈 (회귀 — 초기 보고 이후 악화됨)  
**심각도:** 주요 문제  
**구성요소:** JavaScript Whisper 구현 (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**재현:** `node foundry-local-whisper.mjs` 실행 — 모든 오디오 파일에서 텍스트 전사 대신 빈 값 또는 바이너리 출력 반환

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
초기에는 다섯 번째 오디오 파일만 빈 값을 반환했으나, v0.9.x부터는 다섯 개 모두가 텍스트 전사 대신 단일 바이트(`\ufffd`)를 반환합니다. 동일한 파일을 OpenAI SDK를 사용하는 Python Whisper 구현은 올바르게 전사합니다.

**기대 결과:** `createAudioClient()`는 Python/C# 구현과 일치하는 텍스트 전사 결과를 반환해야 합니다.

---

## 5. C# SDK는 net8.0만 포함하며 .NET 9 또는 .NET 10 공식 타깃 없음

**상태:** 오픈  
**심각도:** 문서 부족  
**구성요소:** `Microsoft.AI.Foundry.Local` NuGet 패키지 v0.9.0  
**설치 명령어:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet 패키지는 단일 타겟 프레임워크만 포함합니다:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
`net9.0`이나 `net10.0` TFM은 포함되지 않았습니다. 반면에 동반 패키지인 `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3)는 `net8.0`, `net9.0`, `net10.0`, `net472`, `netstandard2.0`을 제공합니다.

### 호환성 테스트

| 타겟 프레임워크 | 빌드 | 실행 | 비고 |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | 공식 지원됨 |
| net9.0 | ✅ | ✅ | 포워드 호환으로 빌드 — 워크숍 샘플에서 사용 |
| net10.0 | ✅ | ✅ | .NET 10.0.3 런타임으로 포워드 호환 빌드 및 실행 가능 |

net8.0 어셈블리는 .NET의 포워드 호환성 메커니즘을 통해 최신 런타임에서도 로드되므로 빌드에 성공합니다. 그러나 SDK 팀이 공식 문서화하거나 테스트하지 않았습니다.

### 샘플이 net9.0을 타깃으로 하는 이유

1. **.NET 9이 최신 안정 버전** — 대부분의 워크숍 참가자가 설치함  
2. **포워드 호환성 작동** — NuGet 패키지의 net8.0 어셈블리는 .NET 9 런타임에서 문제없이 동작  
3. **.NET 10 (프리뷰/RC)** 는 모든 사용자를 위한 워크숍 타깃으로는 너무 새로움  

**기대 결과:** 향후 SDK 릴리스에 `net9.0` 및 `net10.0` TFM을 `net8.0`과 함께 추가하여 `Microsoft.Agents.AI.OpenAI`와 유사한 패턴을 적용하고 최신 런타임에 대한 검증된 지원을 제공해야 합니다.

---

## 6. JavaScript ChatClient 스트리밍은 콜백 사용, 비동기 이터레이터 미지원

**상태:** 오픈  
**심각도:** 문서 부족  
**구성요소:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()`가 반환하는 `ChatClient`는 `completeStreamingChat()` 메서드를 제공하지만, 이는 비동기 이터러블을 반환하지 않고 <strong>콜백 패턴</strong>을 사용합니다:

```javascript
// ❌ 이 코드는 작동하지 않습니다 — "stream은 비동기 반복 가능하지 않습니다" 예외가 발생합니다
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ 올바른 패턴 — 콜백을 전달하세요
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**영향:** OpenAI SDK의 비동기 이터레이션 패턴(`for await`)에 익숙한 개발자는 혼란스러운 오류를 겪을 수 있습니다. 콜백이 유효한 함수여야 하며, 그렇지 않으면 SDK가 "Callback must be a valid function." 예외를 던집니다.

**기대 결과:** SDK 참조 문서에 콜백 패턴을 명확히 문서화하거나, 일관성을 위해 OpenAI SDK와 동일하게 비동기 이터러블 패턴을 지원해야 합니다.

---

## 환경 상세 정보

| 구성요소 | 버전 |
|-----------|---------|
| OS | Windows 11 ARM64 |
| 하드웨어 | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |