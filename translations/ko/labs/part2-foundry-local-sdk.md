![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 파트 2: Foundry Local SDK 심층 탐구

> **목표:** Foundry Local SDK를 마스터하여 모델, 서비스 및 캐싱을 프로그래밍 방식으로 관리하고, SDK가 애플리케이션 빌드에 CLI보다 권장되는 이유를 이해합니다.

## 개요

파트 1에서는 <strong>Foundry Local CLI</strong>를 사용하여 모델을 다운로드하고 대화식으로 실행했습니다. CLI는 탐색에 좋지만 실제 애플리케이션을 구축할 때는 <strong>프로그래밍 제어</strong>가 필요합니다. Foundry Local SDK는 이를 제공합니다 - 애플리케이션 코드는 **데이터 플레인**(프롬프트 전송, 완료 수신)에 집중할 수 있도록 **제어 플레인**(서비스 시작, 모델 검색, 다운로드, 로드)을 관리합니다.

이 랩에서는 Python, JavaScript, C#에서 SDK API 전 영역을 다룹니다. 끝나면 사용 가능한 모든 메소드와 사용 시기를 이해하게 될 것입니다.

## 학습 목표

이 랩을 마치면 다음을 할 수 있습니다:

- SDK가 애플리케이션 개발에 CLI보다 선호되는 이유 설명
- Python, JavaScript, C#용 Foundry Local SDK 설치
- `FoundryLocalManager`를 사용해 서비스 시작, 모델 관리, 카탈로그 조회
- 모델을 프로그래밍 방식으로 나열, 다운로드, 로드, 언로드
- `FoundryModelInfo`를 사용해 모델 메타데이터 검사
- 카탈로그, 캐시, 로드된 모델의 차이 이해
- 생성자 부트스트랩(Python)과 `create()` + 카탈로그 패턴(JavaScript) 사용
- C# SDK 재설계 및 객체지향 API 이해

---

## 사전 준비 사항

| 요구 사항 | 상세 내용 |
|-----------|------------|
| **Foundry Local CLI** | 설치되어 `PATH`에 등록 ([파트 1](part1-getting-started.md)) |
| **언어 런타임** | **Python 3.9+**, 및/또는 **Node.js 18+**, 및/또는 **.NET 9.0+** |

---

## 개념: SDK 대 CLI - 왜 SDK를 사용할까?

| 항목 | CLI (`foundry` 명령) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **사용 사례** | 탐색, 수동 테스트 | 애플리케이션 통합 |
| **서비스 관리** | 수동: `foundry service start` | 자동: `manager.start_service()`(Python) / `manager.startWebService()`(JS/C#) |
| **포트 탐색** | CLI 출력에서 읽음 | `manager.endpoint`(Python) / `manager.urls[0]`(JS/C#) |
| **모델 다운로드** | `foundry model download alias` | `manager.download_model(alias)`(Python) / `model.download()`(JS/C#) |
| **오류 처리** | 종료 코드, stderr | 예외, 타입화된 오류 |
| <strong>자동화</strong> | 셸 스크립트 | 네이티브 언어 통합 |
| <strong>배포</strong> | 최종 사용자 기기에 CLI 필요 | C# SDK는 자체 포함 가능(CLI 불필요) |

> **핵심 인사이트:** SDK는 서비스 시작, 캐시 확인, 누락 모델 다운로드, 로드, 엔드포인트 발견 등 전체 수명 주기를 몇 줄 코드로 처리합니다. 애플리케이션에서 CLI 출력 파싱이나 서브프로세스 관리는 필요 없습니다.

---

## 랩 실습

### 실습 1: SDK 설치

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

설치 확인:

```python
from foundry_local import FoundryLocalManager
print("SDK installed successfully")
```

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```bash
npm install foundry-local-sdk
```

설치 확인:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

두 가지 NuGet 패키지가 있습니다:

| 패키지 | 플랫폼 | 설명 |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | 크로스플랫폼 | Windows, Linux, macOS에서 작동 |
| `Microsoft.AI.Foundry.Local.WinML` | Windows 전용 | WinML 하드웨어 가속 추가; 플러그인 실행 공급자(예: Qualcomm NPU용 QNN) 다운로드 및 설치 |

**Windows 설정:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` 파일 수정:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <!-- Windows: windows-specific TFM so WinML (QNN EP plugin) can load -->
    <TargetFramework Condition="$([MSBuild]::IsOSPlatform('Windows'))">net9.0-windows10.0.26100</TargetFramework>
    <!-- Non-Windows: plain TFM (WinML is not available) -->
    <TargetFramework Condition="!$([MSBuild]::IsOSPlatform('Windows'))">net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <!-- WinML requires these properties on Windows -->
  <PropertyGroup Condition="$([MSBuild]::IsOSPlatform('Windows'))">
    <WindowsAppSDKSelfContained>false</WindowsAppSDKSelfContained>
    <WindowsPackageType>None</WindowsPackageType>
    <EnableCoreMrtTooling>false</EnableCoreMrtTooling>
  </PropertyGroup>

  <!-- Windows: WinML is a superset (base SDK + QNN EP plugin) -->
  <ItemGroup Condition="$([MSBuild]::IsOSPlatform('Windows'))">
    <PackageReference Include="Microsoft.AI.Foundry.Local.WinML" Version="[0.9.0,1.0.0)" />
  </ItemGroup>

  <!-- Non-Windows: base SDK only -->
  <ItemGroup Condition="!$([MSBuild]::IsOSPlatform('Windows'))">
    <PackageReference Include="Microsoft.AI.Foundry.Local" Version="[0.9.0,1.0.0)" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Logging" Version="9.0.10" />
  </ItemGroup>
</Project>
```

> **참고:** Windows에서는 WinML 패키지가 기본 SDK와 QNN 실행 공급자를 포함하는 상위 집합입니다. Linux/macOS는 기본 SDK 사용. 조건부 TFM 및 패키지 참조로 프로젝트는 완전한 크로스플랫폼 유지.

프로젝트 루트에 `nuget.config` 생성:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="ORT" value="https://aiinfra.pkgs.visualstudio.com/PublicPackages/_packaging/ORT/nuget/v3/index.json" />
  </packageSources>
  <packageSourceMapping>
    <packageSource key="nuget.org">
      <package pattern="*" />
    </packageSource>
    <packageSource key="ORT">
      <package pattern="*Foundry*" />
    </packageSource>
  </packageSourceMapping>
</configuration>
```

패키지 복원:

```bash
dotnet restore
```

</details>

---

### 실습 2: 서비스 시작 및 카탈로그 나열

애플리케이션이 처음 하는 일은 Foundry Local 서비스를 시작하고 어떤 모델들이 사용 가능한지 확인하는 것입니다.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# 매니저를 생성하고 서비스를 시작합니다
manager = FoundryLocalManager()
manager.start_service()

# 카탈로그에 있는 모든 모델을 나열합니다
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - 서비스 관리 메소드

| 메소드 | 시그니처 | 설명 |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | 서비스 실행 여부 확인 |
| `start_service()` | `() -> None` | Foundry Local 서비스 시작 |
| `service_uri` | `@property -> str` | 기본 서비스 URI |
| `endpoint` | `@property -> str` | API 엔드포인트 (서비스 URI + `/v1`) |
| `api_key` | `@property -> str` | API 키 (환경변수 또는 기본 플레이스홀더) |

#### Python SDK - 카탈로그 관리 메소드

| 메소드 | 시그니처 | 설명 |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | 카탈로그 내 모든 모델 나열 |
| `refresh_catalog()` | `() -> None` | 서비스에서 카탈로그 갱신 |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | 특정 모델 정보 획득 |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// 매니저를 생성하고 서비스를 시작합니다
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 카탈로그를 탐색합니다
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - 매니저 메소드

| 메소드 | 시그니처 | 설명 |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK 싱글톤 초기화 |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | 싱글톤 매니저 접근 |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local 웹 서비스 시작 |
| `manager.urls` | `string[]` | 서비스 기본 URL 배열 |

#### JavaScript SDK - 카탈로그 및 모델 메소드

| 메소드 | 시그니처 | 설명 |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | 모델 카탈로그 접근 |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | 별칭으로 모델 객체 얻기 |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+는 `Configuration`, `Catalog`, `Model` 객체를 갖는 객체지향 구조를 사용합니다:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// Step 1: Configure
var config = new Configuration
{
    AppName = "SDKDemo",
    Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
};

// Step 2: Create the manager
await FoundryLocalManager.CreateAsync(config, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 3: Browse the catalog
var catalog = await manager.GetCatalogAsync(default);
var models = await catalog.ListModelsAsync(default);

Console.WriteLine($"Models available in catalog: {models.Count()}");

foreach (var model in models)
{
    Console.WriteLine($"  - {model.Alias} ({model.ModelId})");
}
```

#### C# SDK - 주요 클래스

| 클래스 | 용도 |
|-------|---------|
| `Configuration` | 앱 이름, 로그 레벨, 캐시 디렉토리, 웹 서버 URL 설정 |
| `FoundryLocalManager` | 메인 진입점 - `CreateAsync()`로 생성, `.Instance`로 접근 |
| `Catalog` | 카탈로그에서 모델 탐색, 검색, 획득 |
| `Model` | 특정 모델 표현 - 다운로드, 로드, 클라이언트 획득 |

#### C# SDK - 매니저 및 카탈로그 메소드

| 메소드 | 설명 |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | 매니저 초기화 |
| `FoundryLocalManager.Instance` | 싱글톤 매니저 접근 |
| `manager.GetCatalogAsync()` | 모델 카탈로그 획득 |
| `catalog.ListModelsAsync()` | 사용 가능한 모든 모델 나열 |
| `catalog.GetModelAsync(alias: "alias")` | 별칭으로 특정 모델 획득 |
| `catalog.GetCachedModelsAsync()` | 다운로드된 모델 목록 |
| `catalog.GetLoadedModelsAsync()` | 현재 로드된 모델 목록 |

> **C# 아키텍처 노트:** C# SDK v0.8.0+는 애플리케이션이 <strong>자체 포함형</strong>이 되도록 재설계되어 최종 사용자 PC에 Foundry Local CLI가 필요 없습니다. SDK가 모델 관리와 추론을 네이티브로 처리합니다.

</details>

---

### 실습 3: 모델 다운로드 및 로드

SDK는 디스크로 다운로드하는 것과 메모리에 로드하는 것을 분리합니다. 이를 통해 미리 모델을 다운로드해 두고 필요 시 로드할 수 있습니다.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# 옵션 A: 수동 단계별 진행
manager = FoundryLocalManager()
manager.start_service()

# 먼저 캐시 확인
cached = manager.list_cached_models()
model_info = manager.get_model_info(alias)
is_cached = any(m.id == model_info.id for m in cached) if model_info else False

if not is_cached:
    print(f"Downloading {alias}...")
    manager.download_model(alias)

print(f"Loading {alias}...")
loaded = manager.load_model(alias)
print(f"Loaded: {loaded.id}")
print(f"Endpoint: {manager.endpoint}")

# 옵션 B: 원라이너 부트스트랩 (권장)
# 별칭을 생성자에 전달 - 서비스가 시작되고, 자동으로 다운로드 및 로드됩니다
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - 모델 관리 메소드

| 메소드 | 시그니처 | 설명 |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | 모델을 로컬 캐시에 다운로드 |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | 추론 서버에 모델 로드 |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | 서버에서 모델 언로드 |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | 현재 로드된 모든 모델 나열 |

#### Python - 캐시 관리 메소드

| 메소드 | 시그니처 | 설명 |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | 캐시 디렉토리 경로 반환 |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | 다운로드된 모든 모델 나열 |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// 단계별 접근법
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(alias);

if (!model.isCached) {
  console.log(`Downloading ${alias}...`);
  await model.download();
}

console.log(`Loading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);
console.log(`Endpoint: ${manager.urls[0]}/v1`);
```

#### JavaScript - 모델 메소드

| 메소드 | 시그니처 | 설명 |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | 모델이 이미 다운로드되었는지 여부 |
| `model.download()` | `() => Promise<void>` | 모델을 로컬 캐시에 다운로드 |
| `model.load()` | `() => Promise<void>` | 추론 서버에 로드 |
| `model.unload()` | `() => Promise<void>` | 추론 서버에서 언로드 |
| `model.id` | `string` | 모델 고유 식별자 |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "phi-3.5-mini";

var config = new Configuration
{
    AppName = "SDKDemo",
    Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
};

await FoundryLocalManager.CreateAsync(config, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);

// View available variants
Console.WriteLine($"Model: {model.Alias}");
foreach (var variant in model.Variants)
{
    Console.WriteLine($"  Variant: {variant.Info.ModelId}");
    Console.WriteLine($"    Device: {variant.Info.Runtime?.DeviceType}");
}

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading...");
    await model.DownloadAsync(null, default);
}

// Load into memory
Console.WriteLine("Loading...");
await model.LoadAsync(default);
Console.WriteLine($"Model loaded: {model.Id}");
```

#### C# - 모델 메소드

| 메소드 | 설명 |
|--------|-------------|
| `model.DownloadAsync(progress?)` | 선택된 변형 다운로드 |
| `model.LoadAsync()` | 모델을 메모리에 로드 |
| `model.UnloadAsync()` | 모델 언로드 |
| `model.SelectVariant(variant)` | 특정 변형(CPU/GPU/NPU) 선택 |
| `model.SelectedVariant` | 현재 선택된 변형 |
| `model.Variants` | 이 모델의 모든 사용 가능한 변형 목록 |
| `model.GetPathAsync()` | 로컬 파일 경로 획득 |
| `model.GetChatClientAsync()` | 네이티브 챗 클라이언트 획득 (OpenAI SDK 불필요) |
| `model.GetAudioClientAsync()` | 전사(transcription)를 위한 오디오 클라이언트 획득 |

</details>

---

### 실습 4: 모델 메타데이터 검사

`FoundryModelInfo` 객체는 각 모델에 대한 풍부한 메타데이터를 포함합니다. 이 필드를 이해하면 애플리케이션에 적합한 모델을 선택하는 데 도움이 됩니다.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# 특정 모델에 대한 자세한 정보를 가져옵니다
info = manager.get_model_info("phi-3.5-mini")
if info:
    print(f"Alias:              {info.alias}")
    print(f"Model ID:           {info.id}")
    print(f"Version:            {info.version}")
    print(f"Task:               {info.task}")
    print(f"Device Type:        {info.device_type}")
    print(f"Execution Provider: {info.execution_provider}")
    print(f"File Size (MB):     {info.file_size_mb}")
    print(f"Publisher:          {info.publisher}")
    print(f"License:            {info.license}")
    print(f"Tool Calling:       {info.supports_tool_calling}")
```

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Alias:              ${model.alias}`);
console.log(`Model ID:           ${model.id}`);
console.log(`Cached:             ${model.isCached}`);
```

</details>

#### FoundryModelInfo 필드

| 필드 | 타입 | 설명 |
|-------|------|-------------|
| `alias` | string | 짧은 이름 (예: `phi-3.5-mini`) |
| `id` | string | 고유 모델 식별자 |
| `version` | string | 모델 버전 |
| `task` | string | `chat-completions` 또는 `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, 또는 NPU |
| `execution_provider` | string | 런타임 백엔드(CUDA, CPU, QNN, WebGPU 등) |
| `file_size_mb` | int | 디스크 상 크기(MB) |
| `supports_tool_calling` | bool | 함수/툴 호출 지원 여부 |
| `publisher` | string | 모델 게시자 |
| `license` | string | 라이센스 이름 |
| `uri` | string | 모델 URI |
| `prompt_template` | dict/null | 프롬프트 템플릿 (있다면) |

---

### 실습 5: 모델 수명주기 관리

전체 수명주기 실습: 나열 → 다운로드 → 로드 → 사용 → 언로드.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # 빠른 테스트를 위한 작은 모델

manager = FoundryLocalManager()
manager.start_service()

# 1. 카탈로그에 무엇이 있는지 확인
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. 이미 다운로드된 것이 무엇인지 확인
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. 모델 다운로드
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. 현재 캐시에 있는지 확인
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. 모델 불러오기
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. 불러온 것을 확인
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. 언로드 하기
print(f"\nUnloading {alias}...")
manager.unload_model(alias)
loaded = manager.list_loaded_models()
print(f"Loaded models after unload: {len(loaded)}")
```

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "qwen2.5-0.5b"; // 빠른 테스트를 위한 소형 모델

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. 카탈로그에서 모델 가져오기
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. 필요하면 다운로드하기
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. 모델 로드하기
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. 모델 언로드하기
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### 연습 6: 빠른 시작 패턴

각 언어는 서비스를 시작하고 모델을 한 번에 로드하는 단축키를 제공합니다. 이는 대부분의 응용 프로그램에 권장되는 <strong>패턴</strong>입니다.

<details>
<summary><h3>🐍 Python - 생성자 부트스트랩</h3></summary>

```python
from foundry_local import FoundryLocalManager

# 생성자에 별칭을 전달하세요 - 모든 것을 처리합니다:
# 1. 서비스가 실행 중이지 않으면 시작합니다
# 2. 모델이 캐시되어 있지 않으면 다운로드합니다
# 3. 추론 서버에 모델을 로드합니다
manager = FoundryLocalManager("phi-3.5-mini")

# 즉시 사용할 준비가 되었습니다
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` 매개변수(기본값 `True`)는 이 동작을 제어합니다. 수동 제어가 필요하면 `bootstrap=False`로 설정하세요:

```python
# 수동 모드 - 아무 작업도 자동으로 실행되지 않습니다
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + 카탈로그</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel()가 모든 것을 처리합니다:
// 1. 서비스를 시작합니다
// 2. 카탈로그에서 모델을 가져옵니다
// 3. 필요할 경우 다운로드하고 모델을 로드합니다
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// 즉시 사용할 준비가 되었습니다
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + 카탈로그</h3></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var config = new Configuration
{
    AppName = "QuickStart",
    Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
};

await FoundryLocalManager.CreateAsync(config, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("phi-3.5-mini", default);

var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);

Console.WriteLine($"Model loaded: {model.Id}");
```

> **C# 참고:** C# SDK는 `Configuration`을 사용하여 앱 이름, 로깅, 캐시 디렉터리, 특정 웹 서버 포트 고정 등을 제어합니다. 이는 세 가지 SDK 중 가장 구성 가능성이 높습니다.

</details>

---

### 연습 7: 네이티브 ChatClient (OpenAI SDK 필요 없음)

JavaScript와 C# SDK는 네이티브 채팅 클라이언트를 반환하는 `createChatClient()` 편의 메서드를 제공합니다 — OpenAI SDK를 별도로 설치하거나 구성할 필요가 없습니다.

<details>
<summary><h3>📘 JavaScript - <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// 모델에서 직접 ChatClient를 생성 — OpenAI 가져오기가 필요하지 않음
const chatClient = model.createChatClient();

// completeChat은 OpenAI 호환 응답 객체를 반환함
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// 스트리밍은 콜백 패턴을 사용함
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient`는 도구 호출도 지원합니다 — 두 번째 인수로 도구를 전달하세요:

```javascript
const response = await chatClient.completeChat(messages, tools);
```

</details>

<details>
<summary><h3>💜 C# - <code>model.GetChatClientAsync()</code></h3></summary>

```csharp
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("phi-3.5-mini", default);
if (!await model.IsCachedAsync(default))
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);

// Get a native chat client — no OpenAI NuGet package needed
var chatClient = await model.GetChatClientAsync(default);

// Use it exactly like the OpenAI ChatClient
var response = chatClient.CompleteChat("What is the golden ratio?");
Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **언제 어떤 패턴을 쓸까:**
> - **`createChatClient()`** — 빠른 프로토타이핑, 종속성 적음, 간단한 코드
> - **OpenAI SDK** — 매개변수(온도, top_p, 중단 토큰 등)를 완벽히 제어, 프로덕션 앱에 적합

---

### 연습 8: 모델 변종 및 하드웨어 선택

모델은 다양한 하드웨어에 최적화된 여러 <strong>변종(variants)</strong>을 가질 수 있습니다. SDK는 자동으로 최적 변종을 선택하지만 수동으로 검사하거나 선택할 수도 있습니다.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// 사용 가능한 모든 변형 목록
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK가 자동으로 하드웨어에 가장 적합한 변형을 선택합니다
// 재정의하려면 selectVariant()를 사용하세요:
// model.selectVariant(model.variants[0]);
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

```csharp
var model = await catalog.GetModelAsync("phi-3.5-mini", default);

Console.WriteLine($"Model: {model.Alias}");
Console.WriteLine($"Selected variant: {model.SelectedVariant?.Info.ModelId}");
Console.WriteLine($"All variants:");
foreach (var variant in model.Variants)
{
    Console.WriteLine($"  - {variant.Info.ModelId}");
    Console.WriteLine($"    Device: {variant.Info.Runtime?.DeviceType}");
}

// To select a specific variant:
// model.SelectVariant(model.Variants.First());
```

</details>

<details>
<summary><h3>🐍 Python</h3></summary>

Python에서는 SDK가 하드웨어 기반으로 최적 변종을 자동 선택합니다. 선택된 변종을 확인하려면 `get_model_info()`를 사용하세요:

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

info = manager.get_model_info("phi-3.5-mini")
print(f"Selected model: {info.id}")
print(f"Device: {info.device_type}")
print(f"Provider: {info.execution_provider}")
```

</details>

#### NPU 변종이 있는 모델

일부 모델은 Neural Processing Unit(네트워크 처리 장치, Qualcomm Snapdragon, Intel Core Ultra 포함)을 위한 NPU-최적화 변종을 제공합니다:

| 모델 | NPU 변종 가능 여부 |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **팁:** NPU 지원 하드웨어에서는 SDK가 자동으로 NPU 변종을 선택합니다. 코드를 변경할 필요가 없습니다. Windows에서 C# 프로젝트는 `Microsoft.AI.Foundry.Local.WinML` NuGet 패키지를 추가하여 QNN 실행 공급자를 활성화하세요 — QNN은 WinML을 통해 플러그인 EP로 제공됩니다.

---

### 연습 9: 모델 업그레이드 및 카탈로그 갱신

모델 카탈로그는 주기적으로 업데이트됩니다. 업데이트를 확인하고 적용하려면 다음 메서드를 사용하세요.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# 최신 모델 목록을 가져오기 위해 카탈로그를 새로 고침하세요
manager.refresh_catalog()

# 캐시된 모델에 더 최신 버전이 있는지 확인하세요
if manager.is_model_upgradeable(alias):
    print(f"{alias} has a newer version available!")
    manager.upgrade_model(alias)
    print("Upgrade complete")
else:
    print(f"{alias} is up to date")
```

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "UpgradeDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 최신 모델 목록을 가져오기 위해 카탈로그를 새로 고침하십시오
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// 새로 고침 후 사용 가능한 모든 모델 나열
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### 연습 10: 추론 모델 사용하기

**phi-4-mini-reasoning** 모델은 체인 오브 생잇 추론을 포함합니다. 최종 답변을 생성하기 전에 내부 사고 과정을 `<think>...</think>` 태그로 감쌉니다. 이는 다단계 논리, 수학 또는 문제 해결이 필요한 작업에 유용합니다.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning은 약 4.6GB입니다
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# 모델은 <think>...</think> 태그 안에 생각을 감쌉니다
if "<think>" in content and "</think>" in content:
    think_start = content.index("<think>") + len("<think>")
    think_end = content.index("</think>")
    thinking = content[think_start:think_end].strip()
    answer = content[think_end + len("</think>"):].strip()
    print(f"Thinking: {thinking}")
    print(f"Answer: {answer}")
else:
    print(content)
```

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ReasoningDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-4-mini-reasoning");
if (!model.isCached) await model.download();
await model.load();

const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

const response = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "What is 17 × 23?" }],
});

const content = response.choices[0].message.content;

// 사고의 연쇄를 분석하다
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **추론 모델 사용 시기:**
> - 수학 및 논리 문제
> - 다단계 계획 작업
> - 복잡한 코드 생성
> - 작업 과정을 보여줘야 정확도가 올라가는 경우
>
> **단점:** 추론 모델은 토큰 수가 더 많고(`<think>` 부분), 속도가 느립니다. 간단한 Q&A에는 phi-3.5-mini 같은 표준 모델이 더 빠릅니다.

---

### 연습 11: 별칭과 하드웨어 선택 이해하기

전체 모델 ID 대신 <strong>별칭</strong>(예: `phi-3.5-mini`)을 전달하면 SDK가 하드웨어에 최적화된 변종을 자동으로 선택합니다:

| 하드웨어 | 선택된 실행 공급자 |
|----------|--------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML 플러그인 통해) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| 모든 장치 (대체) | `CPUExecutionProvider` 또는 `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# 이 별칭은 사용자의 하드웨어에 가장 적합한 변종으로 해석됩니다
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **팁:** 애플리케이션 코드에서는 항상 별칭을 사용하세요. 사용자 머신에 배포 시 SDK가 실행 시점에 최적 변종을 선택합니다 — NVIDIA에는 CUDA, Qualcomm에는 QNN, 그 외에는 CPU를 사용합니다.

---

### 연습 12: C# 구성 옵션

C# SDK의 `Configuration` 클래스는 런타임 제어를 세밀하게 설정할 수 있습니다:

```csharp
var config = new Configuration
{
    AppName = "MyApp",
    LogLevel = Microsoft.AI.Foundry.Local.LogLevel.Information,

    // Pin a specific port (useful for debugging)
    Web = new Configuration.WebService
    {
        Urls = "http://127.0.0.1:55588"
    },

    // Custom directories
    AppDataDir = "./foundry_local_data",
    ModelCacheDir = "{AppDataDir}/model_cache",
    LogsDir = "{AppDataDir}/logs"
};
```

| 속성 | 기본값 | 설명 |
|----------|---------|-------------|
| `AppName` | (필수) | 애플리케이션 이름 |
| `LogLevel` | `Information` | 로깅 상세 수준 |
| `Web.Urls` | (동적) | 웹 서버 특정 포트 지정 |
| `AppDataDir` | 운영체제 기본값 | 앱 데이터 기본 디렉터리 |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | 모델 저장 위치 |
| `LogsDir` | `{AppDataDir}/logs` | 로그 저장 위치 |

---

### 연습 13: 브라우저 사용 (JavaScript 전용)

JavaScript SDK는 브라우저 호환 버전을 제공합니다. 브라우저에서는 CLI를 통해 직접 서비스를 시작하고 호스트 URL을 명시해야 합니다:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// 먼저 서비스를 수동으로 시작하세요:
//   foundry service start
// 그런 다음 CLI 출력에서 URL을 사용하세요
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// 카탈로그를 탐색하세요
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **브라우저 제한 사항:** 브라우저 버전은 `startWebService()`를 지원하지 않습니다. SDK를 브라우저에서 사용하기 전에 Foundry Local 서비스가 이미 실행 중이어야 합니다.

---

## 완전한 API 참조

### Python

| 카테고리 | 메서드 | 설명 |
|----------|--------|-------------|
| <strong>초기화</strong> | `FoundryLocalManager(alias?, bootstrap=True)` | 매니저 생성; 선택적으로 모델 부트스트랩 |
| <strong>서비스</strong> | `is_service_running()` | 서비스 실행 여부 확인 |
| <strong>서비스</strong> | `start_service()` | 서비스 시작 |
| <strong>서비스</strong> | `endpoint` | API 엔드포인트 URL |
| <strong>서비스</strong> | `api_key` | API 키 |
| <strong>카탈로그</strong> | `list_catalog_models()` | 사용 가능한 모든 모델 목록 |
| <strong>카탈로그</strong> | `refresh_catalog()` | 카탈로그 갱신 |
| <strong>카탈로그</strong> | `get_model_info(alias_or_model_id)` | 모델 메타데이터 조회 |
| <strong>캐시</strong> | `get_cache_location()` | 캐시 디렉터리 경로 |
| <strong>캐시</strong> | `list_cached_models()` | 다운로드된 모델 목록 |
| <strong>모델</strong> | `download_model(alias_or_model_id)` | 모델 다운로드 |
| <strong>모델</strong> | `load_model(alias_or_model_id, ttl=600)` | 모델 로드 |
| <strong>모델</strong> | `unload_model(alias_or_model_id)` | 모델 언로드 |
| <strong>모델</strong> | `list_loaded_models()` | 로드된 모델 목록 |
| <strong>모델</strong> | `is_model_upgradeable(alias_or_model_id)` | 새 버전 업데이트 가능 여부 확인 |
| <strong>모델</strong> | `upgrade_model(alias_or_model_id)` | 최신 버전으로 업그레이드 |
| <strong>서비스</strong> | `httpx_client` | 직접 API 호출용 사전 구성된 HTTPX 클라이언트 |

### JavaScript

| 카테고리 | 메서드 | 설명 |
|----------|--------|-------------|
| <strong>초기화</strong> | `FoundryLocalManager.create(options)` | SDK 싱글톤 초기화 |
| <strong>초기화</strong> | `FoundryLocalManager.instance` | 싱글톤 매니저 접근 |
| <strong>서비스</strong> | `manager.startWebService()` | 웹 서비스 시작 |
| <strong>서비스</strong> | `manager.urls` | 서비스 기본 URL 배열 |
| <strong>카탈로그</strong> | `manager.catalog` | 모델 카탈로그 접근 |
| <strong>카탈로그</strong> | `catalog.getModel(alias)` | 별칭으로 모델 객체 가져오기 (Promise 반환) |
| <strong>모델</strong> | `model.isCached` | 모델 다운로드 여부 |
| <strong>모델</strong> | `model.download()` | 모델 다운로드 |
| <strong>모델</strong> | `model.load()` | 모델 로드 |
| <strong>모델</strong> | `model.unload()` | 모델 언로드 |
| <strong>모델</strong> | `model.id` | 모델 고유 식별자 |
| <strong>모델</strong> | `model.alias` | 모델 별칭 |
| <strong>모델</strong> | `model.createChatClient()` | 네이티브 채팅 클라이언트 얻기 (OpenAI SDK 불필요) |
| <strong>모델</strong> | `model.createAudioClient()` | 음성 인식 클라이언트 얻기 |
| <strong>모델</strong> | `model.removeFromCache()` | 로컬 캐시에서 모델 제거 |
| <strong>모델</strong> | `model.selectVariant(variant)` | 특정 하드웨어 변종 선택 |
| <strong>모델</strong> | `model.variants` | 모델의 사용 가능한 변종 배열 |
| <strong>모델</strong> | `model.isLoaded()` | 모델이 현재 로드되었는지 확인 |
| <strong>카탈로그</strong> | `catalog.getModels()` | 사용 가능한 모든 모델 목록 |
| <strong>카탈로그</strong> | `catalog.getCachedModels()` | 다운로드된 모델 목록 |
| <strong>카탈로그</strong> | `catalog.getLoadedModels()` | 현재 로드된 모델 목록 |
| <strong>카탈로그</strong> | `catalog.updateModels()` | 서비스에서 카탈로그 갱신 |
| <strong>서비스</strong> | `manager.stopWebService()` | Foundry Local 웹 서비스 중지 |

### C# (v0.8.0+)

| 카테고리 | 메서드 | 설명 |
|----------|--------|-------------|
| <strong>초기화</strong> | `FoundryLocalManager.CreateAsync(config, logger)` | 매니저 초기화 |
| <strong>초기화</strong> | `FoundryLocalManager.Instance` | 싱글톤 접근 |
| <strong>카탈로그</strong> | `manager.GetCatalogAsync()` | 카탈로그 가져오기 |
| <strong>카탈로그</strong> | `catalog.ListModelsAsync()` | 모든 모델 목록 |
| <strong>카탈로그</strong> | `catalog.GetModelAsync(alias)` | 특정 모델 가져오기 |
| <strong>카탈로그</strong> | `catalog.GetCachedModelsAsync()` | 캐시된 모델 목록 |
| <strong>카탈로그</strong> | `catalog.GetLoadedModelsAsync()` | 로드된 모델 목록 |
| <strong>모델</strong> | `model.DownloadAsync(progress?)` | 모델 다운로드 |
| <strong>모델</strong> | `model.LoadAsync()` | 모델 로드 |
| <strong>모델</strong> | `model.UnloadAsync()` | 모델 언로드 |
| <strong>모델</strong> | `model.SelectVariant(variant)` | 하드웨어 변종 선택 |
| <strong>모델</strong> | `model.GetChatClientAsync()` | 네이티브 채팅 클라이언트 얻기 |
| <strong>모델</strong> | `model.GetAudioClientAsync()` | 음성 인식 클라이언트 얻기 |
| <strong>모델</strong> | `model.GetPathAsync()` | 로컬 파일 경로 얻기 |
| <strong>카탈로그</strong> | `catalog.GetModelVariantAsync(alias, variant)` | 특정 하드웨어 변종 가져오기 |
| <strong>카탈로그</strong> | `catalog.UpdateModelsAsync()` | 카탈로그 갱신 |
| <strong>서버</strong> | `manager.StartWebServerAsync()` | REST 웹 서버 시작 |
| <strong>서버</strong> | `manager.StopWebServerAsync()` | REST 웹 서버 중지 |
| <strong>설정</strong> | `config.ModelCacheDir` | 캐시 디렉터리 |

---

## 주요 시사점

| 개념 | 배운 점 |
|---------|----------|
| **SDK vs CLI** | SDK는 프로그래밍 방식 제어를 제공 — 애플리케이션에 필수 |
| **제어 평면** | SDK는 서비스, 모델, 캐시를 관리 |
| **동적 포트** | 항상 `manager.endpoint` (Python) 또는 `manager.urls[0]` (JS/C#) 사용 — 포트를 하드코딩하지 말 것 |
| <strong>별칭</strong> | 하드웨어 최적 모델 자동 선택을 위해 별칭 사용 |
| **빠른 시작** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# 재설계** | v0.8.0 이상은 독립 실행형 - 최종 사용자 기기에 CLI 필요 없음 |
| **모델 수명 주기** | 카탈로그 → 다운로드 → 로드 → 사용 → 언로드 |
| **FoundryModelInfo** | 풍부한 메타데이터: 작업, 장치, 크기, 라이선스, 도구 호출 지원 |
| **ChatClient** | OpenAI 비사용용 `createChatClient()` (JS) / `GetChatClientAsync()` (C#) |
| <strong>변형</strong> | 모델은 하드웨어별 변형( CPU, GPU, NPU )을 가지며 자동 선택됨 |
| <strong>업그레이드</strong> | Python: `is_model_upgradeable()` + `upgrade_model()` 로 모델 최신 상태 유지 |
| **카탈로그 새로 고침** | `refresh_catalog()` (Python) / `updateModels()` (JS) 를 사용해 새 모델 발견 |

---

## 리소스

| 리소스 | 링크 |
|----------|------|
| SDK 참고문서 (모든 언어) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| 추론 SDK 통합 | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API 참고문서 | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK 샘플 | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local 웹사이트 | [foundrylocal.ai](https://foundrylocal.ai) |

---

## 다음 단계

[Part 3: Using the SDK with OpenAI](part3-sdk-and-apis.md)로 계속 진행하여 SDK를 OpenAI 클라이언트 라이브러리에 연결하고 첫 번째 채팅 완성 애플리케이션을 만드세요.