![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ਭਾਗ 2: Foundry Local SDK ਡੀਪ ਡਾਈਵ

> **ਉਦੇਸ਼:** Foundry Local SDK ਵਿੱਚ ਪ੍ਰੰਗ੍ਰਾਮੈਟਿਕ ਤਰੀਕੇ ਨਾਲ ਮਾਡਲ, ਸੇਵਾਵਾਂ, ਅਤੇ ਕੈਸ਼ਿੰਗ ਦਾ ਪ੍ਰਬੰਧਨ ਕਰਨ 'ਤੇ ਮਾਹਰ ਬਣੋ - ਅਤੇ ਸਮਝੋ ਕਿ ਐਪਲੀਕੇਸ਼ਨਾਂ ਬਣਾਉਣ ਲਈ CLI ਨਾਲੋਂ SDK ਨੂੰ ਕਿਉਂ ਸਿਫਾਰਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ।

## ਓਵਰਵਿਊ

ਭਾਗ 1 ਵਿਚ ਤੁਸੀਂ **Foundry Local CLI** ਦਾ ਇਸਤੇਮਾਲ ਕਰਕੇ ਮਾਡਲ ਡਾਊਨਲੋਡ ਅਤੇ ਇੰਟਰੈਕਟਿਵ ਤਰੀਕੇ ਨਾਲ ਚਲਾਏ। CLI ਖੋਜ ਲਈ ਬਹੁਤ ਵਧੀਆ ਹੈ, ਪਰ ਜਦੋਂ ਤੁਸੀਂ ਅਸਲ ਐਪਲੀਕੇਸ਼ਨ ਬਣਾਉਂਦੇ ਹੋ ਤਾਂ ਤੁਹਾਨੂੰ **ਪ੍ਰੋਗਰਾਮੈਟਿਕ ਕੰਟਰੋਲ** ਦੀ ਲੋੜ ਹੁੰਦੀ ਹੈ। Foundry Local SDK ਤੁਹਾਨੂੰ ਇਹ ਦਿੰਦਾ ਹੈ - ਇਹ **ਕੰਟਰੋਲ ਪਲੇਨ** (ਸੇਵਾ ਸ਼ੁਰੂ ਕਰਨਾ, ਮਾਡਲ ਖੋਜਣਾ, ਡਾਊਨਲੋਡ ਕਰਨਾ, ਲੋਡ ਕਰਨਾ) ਨੂੰ ਸੰਭਾਲਦਾ ਹੈ ਤਾਂ ਜੋ ਤੁਹਾਡੇ ਐਪਲੀਕੇਸ਼ਨ ਕੋਡ ਨੂੰ **ਡਾਟਾ ਪਲੇਨ** (ਪ੍ਰਾਂਪਟ ਭੇਜਣਾ, ਪੁੱਛ-ਪੜਤਾਲ ਪ੍ਰਾਪਤ ਕਰਨਾ) 'ਤੇ ਧਿਆਨ ਕੇਂਦਰਿਤ ਕੀਤਾ ਜਾ ਸਕੇ।

ਇਹ ਲੈਬ ਤੁਹਾਨੂੰ ਪਾਈਥਨ, ਜਾਵਾਸਕ੍ਰਿਪਟ, ਅਤੇ C# ਵਿੱਚ ਪੂਰੇ SDK API ਸਤਹ ਸਿਖਾਉਂਦਾ ਹੈ। ਆਖਿਰ ਵਿੱਚ ਤੁਸੀਂ ਹਰ ਮੈਥਡ ਨੂੰ ਸਮਝੋਗੇ ਅਤੇ ਇਹ ਕਦੋਂ ਵਰਤਣਾ ਹੈ ਇਹ ਵੀ ਜਾਣੋਗੇ।

## ਸਿੱਖਣ ਦੇ ਉਦੇਸ਼

ਇਸ ਲੈਬ ਦੇ ਆਖਰ ਵਿੱਚ ਤੁਸੀਂ ਕਰ ਸਕੋਗੇ:

- ਸਮਝਾਉਣਾ ਕਿ ਐਪਲੀਕੇਸ਼ਨ ਵਿਕਾਸ ਲਈ CLI ਨਾਲੋਂ SDK ਪ੍ਰਾਥਮਿਕ ਕਿਉਂ ਹੈ
- Foundry Local SDK ਨੂੰ ਪਾਈਥਨ, ਜਾਵਾਸਕ੍ਰਿਪਟ ਜਾਂ C# ਲਈ ਇੰਸਟਾਲ ਕਰਨਾ
- `FoundryLocalManager` ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰਨਾ, ਮਾਡਲ ਸੰਭਾਲਣ ਅਤੇ ਕੈਟਾਲੌਗ ਨੂੰ ਕਵੈਰੀ ਕਰਨਾ
- ਪ੍ਰੋਗਰਾਮੈਟਿਕ ਢੰਗ ਨਾਲ ਮਾਡਲ ਦੀ ਲਿਸਟ ਬਣਾਉਣਾ, ਡਾਊਨਲੋਡ ਕਰਨਾ, ਲੋਡ ਅਤੇ ਅਨਲੋਡ ਕਰਨਾ
- `FoundryModelInfo` ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਮਾਡਲ ਮੈਟਾਡੇਟਾ ਦੀ ਜਾਂਚ ਕਰਨਾ
- ਕੈਟਾਲੌਗ, ਕੈਸ਼, ਅਤੇ ਲੋਡ ਕੀਤੇ ਮਾਡਲਾਂ ਵਿੱਚ ਫਰਕ ਸਮਝਣਾ
- ਕੰਸਟ੍ਰਕਟਰ ਬੂਟਸਟ੍ਰੈਪ (ਪਾਈਥਨ) ਅਤੇ `create()` + ਕੈਟਾਲੌਗ ਪੈਟਰਨ (ਜਾਵਾਸਕ੍ਰਿਪਟ) ਦੀ ਵਰਤੋਂ ਕਰਨਾ
- C# SDK ਦੇ ਰੀਡਿਜ਼ਾਈਨ ਅਤੇ ਇਸਦੇ ਓਬਜੈਕਟ-ਉਰੀਐਂਟਿਡ API ਨੂੰ ਸਮਝਣਾ

---

## ਜ਼ਰੂਰੀ ਸ਼ਰਤਾਂ

| ਲੋੜ | ਵੇਰਵਾ |
|-------------|---------|
| **Foundry Local CLI** | ਇੰਸਟਾਲ ਅਤੇ ਤੁਹਾਡੇ `PATH` 'ਤੇ ([ਭਾਗ 1](part1-getting-started.md)) |
| **ਭਾਸ਼ਾ ਰਨਟਾਈਮ** | **Python 3.9+** ਅਤੇ/ਜਾਂ **Node.js 18+** ਅਤੇ/ਜਾਂ **.NET 9.0+** |

---

## ਧਾਰਣਾ: SDK ਵਿ. CLI - SDK ਕਿਉਂ ਵਰਤਣਾ?

| ਪਹਲੂ | CLI (`foundry` ਕਮਾਂਡ) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **ਵਰਤੋਂ ਦਾ ਮਾਮਲਾ** | ਖੋਜ, ਮੈਨੁਅਲ ਟੈਸਟਿੰਗ | ਐਪਲੀਕੇਸ਼ਨ ਇੰਟਿਗ੍ਰੇਸ਼ਨ |
| **ਸੇਵਾ ਪ੍ਰਬੰਧਨ** | ਮੈਨੁਅਲ: `foundry service start` | ਆਟੋਮੈਟਿਕ: `manager.start_service()` (ਪਾਈਥਨ) / `manager.startWebService()` (JS/C#) |
| **ਪੋਰਟ ਖੋਜ** | CLI ਆਉਟਪੁੱਟ ਤੋਂ ਪੜ੍ਹੋ | `manager.endpoint` (ਪਾਈਥਨ) / `manager.urls[0]` (JS/C#) |
| **ਮਾਡਲ ਡਾਊਨਲੋਡ** | `foundry model download alias` | `manager.download_model(alias)` (ਪਾਈਥਨ) / `model.download()` (JS/C#) |
| **ਏਰਰ ਹੈਂਡਲਿੰਗ** | ਐਗਜ਼ਿਟ ਕੋਡ, stderr | ਐਕਸਪਸ਼ਨ, ਟਾਈਪ ਕੀਤੇ ਐਰਰ |
| **ਆਟੋਮੇਸ਼ਨ** | ਸ਼ੈਲ ਸਕ੍ਰਿਪਟ | ਦੇਸੀ ਭਾਸ਼ਾ ਇੰਟਿਗ੍ਰੇਸ਼ਨ |
| **ਡਿਪਲੋਇਮਿੰਟ** | ਐਂਡ-ਯੂਜ਼ਰ ਮਸ਼ੀਨ ਉੱਤੇ CLI ਦੀ ਲੋੜ | C# SDK ਸਵੈ-ਨਿਰਭਰ ਹੋ ਸਕਦਾ ਹੈ (CLI ਦੀ ਲੋੜ ਨਹੀਂ) |

> **ਮੁੱਖ ਜਾਣਕਾਰੀ:** SDK ਪੂਰੇ ਲਾਈਫਸਾਈਕਲ ਨੂੰ ਸੰਭਾਲਦਾ ਹੈ: ਸੇਵਾ ਸ਼ੁਰੂ ਕਰਨਾ, ਕੈਸ਼ ਚੈੱਕ ਕਰਨਾ, ਗੁੰਮਸ਼ੁਦਾਂ ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰਨਾ, ਉਨ੍ਹਾਂ ਨੂੰ ਲੋਡ ਕਰਨਾ, ਅਤੇ ਐਂਡਪੋਇੰਟ ਦੀ ਖੋਜ ਕਰਨਾ, ਸਿਰਫ ਕੁਝ ਸਤਰਾਂ ਵਿੱਚ। ਤੁਹਾਡੀ ਐਪਲੀਕੇਸ਼ਨ ਨੂੰ CLI ਆਉਟਪੁੱਟ ਪਾਰਸ ਕਰਨ ਜਾਂ ਸਬਪ੍ਰੋਸੈਸ ਪ੍ਰਬੰਧਨ ਦੀ ਲੋੜ ਨਹੀਂ।

---

## ਲੈਬ ਅਭਿਆਸ

### ਅਭਿਆਸ 1: SDK ਨੂੰ ਇੰਸਟਾਲ ਕਰੋ

<details>
<summary><h3>🐍 ਪਾਈਥਨ</h3></summary>

```bash
pip install foundry-local-sdk
```

ਇੰਸਟਾਲੇਸ਼ਨ ਦੀ ਜਾਂਚ ਕਰੋ:

```python
from foundry_local import FoundryLocalManager
print("SDK installed successfully")
```

</details>

<details>
<summary><h3>📘 ਜਾਵਾਸਕ੍ਰਿਪਟ</h3></summary>

```bash
npm install foundry-local-sdk
```

ਇੰਸਟਾਲੇਸ਼ਨ ਦੀ ਜਾਂਚ ਕਰੋ:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

ਦੋ NuGet ਪੈਕੇਜ ਹਨ:

| ਪੈਕੇਜ | ਪਲੈਟਫਾਰਮ | ਵੇਰਵਾ |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | ਕ੍ਰਾਸ-ਪਲੈਟਫਾਰਮ | Windows, Linux, macOS 'ਤੇ ਕੰਮ ਕਰਦਾ ਹੈ |
| `Microsoft.AI.Foundry.Local.WinML` | ਸਿਰਫ Windows | WinML ਹਾਰਡਵੇਅਰ ਤੀਬਰਤਾ ਜੋੜਦਾ ਹੈ; ਪਲੱਗਇਨ ਐਕਜ਼ਿਕਿ਷ਨ ਪ੍ਰੋਵਾਈਡਰਾਂ (ਜਿਵੇਂ QNN ਫੋਰ Qualcomm NPU) ਨੂੰ ਡਾਊਨਲੋਡ ਅਤੇ ਇੰਸਟਾਲ ਕਰਦਾ ਹੈ |

**Windows ਸੈਟਅਪ:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` ਫਾਈਲ ਸੋਧੋ:

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

> **ਟਿੱਪਣੀ:** Windows 'ਤੇ WinML ਪੈਕੇਜ ਇੱਕ ਸਪਰਸੈੱਟ ਹੈ ਜੋ ਬੇਸ SDK ਅਤੇ QNN ਐਕਜ਼ਿਕਿ਷ਨ ਪ੍ਰੋਵਾਈਡਰ ਦੋਹਾਂ ਸ਼ਾਮਲ ਕਰਦਾ ਹੈ। Linux/macOS 'ਤੇ ਬੇਸ SDK ਵਰਤਿਆ ਜਾਂਦਾ ਹੈ। ਸ਼ਰਤੀ TFM ਅਤੇ ਪੈਕੇਜ ਰੈਫਰੈਂਸ ਪ੍ਰੋਜੇਕਟ ਨੂੰ ਪੂਰੀ ਤਰ੍ਹਾਂ ਕ੍ਰਾਸ-ਪਲੈਟਫਾਰਮ ਬਣਾਈ ਰੱਖਦੇ ਹਨ।

ਪ੍ਰੋਜੈਕਟ ਰੂਟ ਵਿੱਚ `nuget.config` ਬਣਾਓ:

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

ਪੈਕੇਜ ਰੀਸਟੋਰ ਕਰੋ:

```bash
dotnet restore
```

</details>

---

### ਅਭਿਆਸ 2: ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ ਅਤੇ ਕੈਟਾਲੌਗ ਦੀ ਲਿਸਟ ਬਣਾਓ

ਕੋਈ ਵੀ ਐਪਲੀਕੇਸ਼ਨ ਸਭ ਤੋਂ ਪਹਿਲਾਂ Foundry Local ਸੇਵਾ ਸ਼ੁਰੂ ਕਰਦਾ ਹੈ ਅਤੇ ਪਤਾ ਲਗਾਉਂਦਾ ਹੈ ਕਿ ਕਿਹੜੇ ਮਾਡਲ ਉਪਲਬਧ ਹਨ।

<details>
<summary><h3>🐍 ਪਾਈਥਨ</h3></summary>

```python
from foundry_local import FoundryLocalManager

# ਇੱਕ ਮੈਨੇਜਰ ਬਣਾਓ ਅਤੇ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ
manager = FoundryLocalManager()
manager.start_service()

# ਕੈਟਾਲੌਗ ਵਿੱਚ ਉਪਲੱਬਧ ਸਾਰੇ ਮਾਡਲ ਸੂਚੀਬੱਧ ਕਰੋ
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### ਪਾਈਥਨ SDK - ਸੇਵਾ ਪ੍ਰਬੰਧਨ ਮੈਥਡ

| ਮੈਥਡ | ਸਿਗਨੇਚਰ | ਵੇਰਵਾ |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | ਚੈੱਕ ਕਰੋ ਕਿ ਸੇਵਾ ਚੱਲ ਰਹੀ ਹੈ ਜਾਂ ਨਹੀਂ |
| `start_service()` | `() -> None` | Foundry Local ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ |
| `service_uri` | `@property -> str` | ਬੇਸ ਸੇਵਾ URI |
| `endpoint` | `@property -> str` | API ਐਂਡਪੋਇੰਟ (ਸੇਵਾ URI + `/v1`) |
| `api_key` | `@property -> str` | API ਕੀ (env ਜਾਂ ਡਿਫਾਲਟ ਪਲੇਸਹੋਲਡਰ ਤੋਂ) |

#### ਪਾਈਥਨ SDK - ਕੈਟਾਲੌਗ ਪ੍ਰਬੰਧਨ ਮੈਥਡ

| ਮੈਥਡ | ਸਿਗਨੇਚਰ | ਵੇਰਵਾ |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | ਕੈਟਾਲੌਗ ਵਿੱਚ ਸਾਰੇ ਮਾਡਲਾਂ ਨੂੰ ਲਿਸਟ ਕਰੋ |
| `refresh_catalog()` | `() -> None` | ਸੇਵਾ ਤੋਂ ਕੈਟਾਲੌਗ ਨੂੰ ਤਾਜ਼ਾ ਕਰੋ |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=false) -> FoundryModelInfo \| None` | ਕਿਸੇ ਖਾਸ ਮਾਡਲ ਦੀ ਜਾਣਕਾਰੀ ਲਓ |

</details>

<details>
<summary><h3>📘 ਜਾਵਾਸਕ੍ਰਿਪਟ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ਇੱਕ ਮੈਨੇਜਰ ਬਣਾਓ ਅਤੇ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ਕੈਟਾਲੌਗ ਨੂੰ ਬ੍ਰਾਊਜ਼ ਕਰੋ
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### ਜਾਵਾਸਕ੍ਰਿਪਟ SDK - ਮੈਨੇਜਰ ਮੈਥਡ

| ਮੈਥਡ | ਸਿਗਨੇਚਰ | ਵੇਰਵਾ |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK ਸਿੰਗਲਟਨ ਨੂੰ ਇਨੀਸ਼ੀਅਲਾਈਜ਼ ਕਰੋ |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | ਸਿੰਗਲਟਨ ਮੈਨੇਜਰ ਤੱਕ ਪਹੁੰਚ |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local ਵੈਬ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ |
| `manager.urls` | `string[]` | ਸੇਵਾ ਲਈ ਬੇਸ URLs ਦੀ ਐਰੇ |

#### ਜਾਵਾਸਕ੍ਰਿਪਟ SDK - ਕੈਟਾਲੌਗ ਅਤੇ ਮਾਡਲ ਮੈਥਡ

| ਮੈਥਡ | ਸਿਗਨੇਚਰ | ਵੇਰਵਾ |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | ਮਾਡਲ ਕੈਟਾਲੌਗ ਤੱਕ ਪਹੁੰਚ |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | ਅਲਾਇਸ ਨਾਲ ਮਾਡਲ ਓਬਜੈਕਟ ਲਓ |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ ਓਬਜੈਕਟ-ਉਰੀਐਂਟਿਡ ਬਣਤਰ ਦਾ ਇਸਤੇਮਾਲ ਕਰਦਾ ਹੈ ਜਿਸ ਵਿੱਚ `Configuration`, `Catalog`, ਅਤੇ `Model` ਓਬਜੈਕਟ ਸ਼ਾਮਲ ਹਨ:

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

#### C# SDK - ਮੁੱਖ ਕਲਾਸਾਂ

| ਕਲਾਸ | ਉਦੇਸ਼ |
|-------|---------|
| `Configuration` | ਐਪ ਨਾਂ, ਲੌਗ ਲੈਵਲ, ਕੈਸ਼ ਡਿਰ, ਵੈੱਬ ਸਰਵਰ URLs ਸੈੱਟ ਕਰੋ |
| `FoundryLocalManager` | ਮੁੱਖ ਦਾਖਲਾ ਬਿੰਦੂ - `CreateAsync()` ਰਾਹੀਂ ਬਣਾਇਆ ਜਾਂਦਾ ਹੈ, `.Instance` ਰਾਹੀਂ ਪਹੁੰਚ ਹੈ |
| `Catalog` | ਕੈਟਾਲੌਗ ਵਿੱਚੋਂ ਮਾਡਲ ਬ੍ਰਾਉਜ਼ ਕਰੋ, ਖੋਜੋ ਅਤੇ ਪ੍ਰਾਪਤ ਕਰੋ |
| `Model` | ਕਿਸੇ ਵਿਸ਼ੇਸ਼ ਮਾਡਲ ਨੂੰ ਦਰਸਾਉਂਦਾ ਹੈ - ਡਾਊਨਲੋਡ, ਲੋਡ, ਕਲਾਇੰਟ ਪ੍ਰਾਪਤ ਕਰੋ |

#### C# SDK - ਮੈਨੇਜਰ ਅਤੇ ਕੈਟਾਲੌਗ ਮੈਥਡ

| ਮੈਥਡ | ਵੇਰਵਾ |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | ਮੈਨੇਜਰ ਇਨੀਸ਼ੀਅਲਾਈਜ਼ ਕਰੋ |
| `FoundryLocalManager.Instance` | ਸਿੰਗਲਟਨ ਮੈਨੇਜਰ ਤੱਕ ਪਹੁੰਚ |
| `manager.GetCatalogAsync()` | ਮਾਡਲ ਕੈਟਾਲੌਗ ਪ੍ਰਾਪਤ ਕਰੋ |
| `catalog.ListModelsAsync()` | ਸਾਰੇ ਉਪਲਬਧ ਮਾਡਲਾਂ ਦੀ ਲਿਸਟ ਬਣਾਓ |
| `catalog.GetModelAsync(alias: "alias")` | ਅਲਾਇਸ ਰਾਹੀਂ ਕਿਸੇ ਖਾਸ ਮਾਡਲ ਨੂੰ ਪ੍ਰਾਪਤ ਕਰੋ |
| `catalog.GetCachedModelsAsync()` | ਡਾਊਨਲੋਡ ਕੀਤੇ ਮਾਡਲਾਂ ਦੀ ਲਿਸਟ |
| `catalog.GetLoadedModelsAsync()` | ਵਰਤਮਾਨ ਵਿੱਚ ਲੋਡ ਕੀਤੇ ਮਾਡਲਾਂ ਦੀ ਲਿਸਟ |

> **C# ਵਿਆਪਕ ਨੋਟ:** C# SDK v0.8.0+ ਦਾ ਰੀਡਿਜ਼ਾਈਨ ਐਪਲੀਕੇਸ਼ਨ ਨੂੰ **ਆਤਮਨਿਰਭਰ** ਬਣਾਂਦਾ ਹੈ; ਇਹ ਐਂਡ-ਯੂਜ਼ਰ ਦੀ ਮਸ਼ੀਨ 'ਤੇ Foundry Local CLI ਦੀ ਲੋੜ ਨਹੀਂ ਰੱਖਦਾ। SDK ਮਾਡਲ ਪ੍ਰਬੰਧਨ ਅਤੇ ਇਨਫਰੰਸ ਨੈਟਿਵ ਤਰੀਕੇ ਨਾਲ ਕਰਦਾ ਹੈ।

</details>

---

### ਅਭਿਆਸ 3: ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰੋ ਅਤੇ ਲੋਡ ਕਰੋ

SDK ਡਾਊਨਲੋਡਿੰਗ (ਡਿਸਕ ਤੇ) ਨੂੰ ਲੋਡਿੰਗ (ਮੈਮੋਰੀ ਵਿੱਚ) ਤੋਂ ਵੱਖ ਕਰਦਾ ਹੈ। ਇਹ ਤੁਹਾਨੂੰ ਸੈਟਅਪ ਦੌਰਾਨ ਮਾਡਲ ਪਹਿਲਾਂ ਹੀ ਡਾਊਨਲੋਡ ਕਰਨ ਅਤੇ ਲੋਡਿੰਗ ਮੰਗ 'ਤੇ ਕਰਨ ਦੀ ਆਗਿਆ ਦਿੰਦਾ ਹੈ।

<details>
<summary><h3>🐍 ਪਾਈਥਨ</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# ਵਿਕਲਪ ਏ: ਮੈਨੂਅਲ ਕਦਮ-ਦਰ-ਕਦਮ
manager = FoundryLocalManager()
manager.start_service()

# ਪਹਿਲਾਂ ਕੈਸ਼ ਚੈੱਕ ਕਰੋ
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

# ਵਿਕਲਪ ਬੀ: ਇੱਕ ਲਾਈਨਰ ਬੂਟਸਟ੍ਰੈਪ (ਸਿਫਾਰਸ਼ੀ)
# ਕੰਸਟ੍ਰਕਟਰ ਨੂੰ ਐਲਾਇਸ ਪਾਸ ਕਰੋ - ਇਹ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰਦਾ ਹੈ, ਡਾਊਨਲੋਡ ਕਰਦਾ ਹੈ, ਅਤੇ ਆਪਣੇ ਆਪ ਲੋਡ ਕਰਦਾ ਹੈ
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### ਪਾਈਥਨ - ਮਾਡਲ ਪ੍ਰਬੰਧਨ ਮੈਥਡ

| ਮੈਥਡ | ਸਿਗਨੇਚਰ | ਵੇਰਵਾ |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | ਮਾਡਲ ਨੂੰ ਲੋਕਲ ਕੈਸ਼ ਵਿੱਚ ਡਾਊਨਲੋਡ ਕਰੋ |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | ਮਾਡਲ ਨੂੰ ਇਨਫਰੰਸ ਸਰਵਰ ਵਿੱਚ ਲੋਡ ਕਰੋ |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | ਮਾਡਲ ਨੂੰ ਸਰਵਰ ਤੋਂ ਅਨਲੋਡ ਕਰੋ |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | ਵਰਤਮਾਨ ਵਿੱਚ ਲੋਡ ਕੀਤੇ ਸਾਰੇ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |

#### ਪਾਈਥਨ - ਕੈਸ਼ ਪ੍ਰਬੰਧਨ ਮੈਥਡ

| ਮੈਥਡ | ਸਿਗਨੇਚਰ | ਵੇਰਵਾ |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | ਕੈਸ਼ ਡਾਇਰੈਕਟਰੀ ਪਾਥ ਪ੍ਰਾਪਤ ਕਰੋ |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | ਸਾਰੇ ਡਾਊਨਲੋਡ ਕੀਤੇ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |

</details>

<details>
<summary><h3>📘 ਜਾਵਾਸਕ੍ਰਿਪਟ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// ਕਦਮ-ਦਰ-ਕਦਮ ਤਰੀਕਾ
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

#### ਜਾਵਾਸਕ੍ਰਿਪਟ - ਮਾਡਲ ਮੈਥਡ

| ਮੈਥਡ | ਸਿਗਨੇਚਰ | ਵੇਰਵਾ |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | ਕੀ ਮਾਡਲ ਪਹਿਲਾਂ ਹੀ ਡਾਊਨਲੋਡ ਕੀਤਾ ਗਿਆ ਹੈ |
| `model.download()` | `() => Promise<void>` | ਮਾਡਲ ਨੂੰ ਲੋਕਲ ਕੈਸ਼ ਵਿੱਚ ਡਾਊਨਲੋਡ ਕਰੋ |
| `model.load()` | `() => Promise<void>` | ਇਨਫਰੰਸ ਸਰਵਰ ਵਿੱਚ ਲੋਡ ਕਰੋ |
| `model.unload()` | `() => Promise<void>` | ਇਨਫਰੰਸ ਸਰਵਰ ਤੋਂ ਅਨਲੋਡ ਕਰੋ |
| `model.id` | `string` | ਮਾਡਲ ਦਾ ਯੂਨੀਕ ਪਹਚਾਣਕਰਤਾ |

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

#### C# - ਮਾਡਲ ਮੈਥਡ

| ਮੈਥਡ | ਵੇਰਵਾ |
|--------|-------------|
| `model.DownloadAsync(progress?)` | ਚੁਣੀ ਹੋਈ ਕਿਸਮ ਨੂੰ ਡਾਊਨਲੋਡ ਕਰੋ |
| `model.LoadAsync()` | ਮਾਡਲ ਨੂੰ ਮੈਮੋਰੀ ਵਿੱਚ ਲੋਡ ਕਰੋ |
| `model.UnloadAsync()` | ਮਾਡਲ ਅਨਲੋਡ ਕਰੋ |
| `model.SelectVariant(variant)` | ਕਿਸੇ ਵਿਸ਼ੇਸ਼ ਵਰਿਅਂਟ (CPU/GPU/NPU) ਨੂੰ ਚੁਣੋ |
| `model.SelectedVariant` | ਵਰਤਮਾਨ ਵਿੱਚ ਚੁਣਿਆ ਗਿਆ ਵਰਿਅਂਟ |
| `model.Variants` | ਇਸ ਮਾਡਲ ਲਈ ਸਾਰੇ ਉਪਲਬਧ ਵਰਿਅਂਟ |
| `model.GetPathAsync()` | ਲੋਕਲ ਫਾਈਲ ਮਾਰਗ ਪ੍ਰਾਪਤ ਕਰੋ |
| `model.GetChatClientAsync()` | ਦੇਸੀ ਚੈਟ ਕਲਾਇੰਟ ਪ੍ਰਾਪਤ ਕਰੋ (ਕੋਈ OpenAI SDK ਦੀ ਲੋੜ ਨਹੀਂ) |
| `model.GetAudioClientAsync()` | ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਲਈ ਆਡੀਓ ਕਲਾਇੰਟ ਪ੍ਰਾਪਤ ਕਰੋ |

</details>

---

### ਅਭਿਆਸ 4: ਮਾਡਲ ਮੈਟਾਡੇਟਾ ਦੀ ਜਾਂਚ ਕਰੋ

`FoundryModelInfo` ਓਬਜੈਕਟ ਹਰ ਮਾਡਲ ਬਾਰੇ ਵਿਸਥਾਰ ਸੂਚਨਾ ਰੱਖਦਾ ਹੈ। ਇਹ ਫੀਲਡ ਤੁਹਾਨੂੰ ਆਪਣੀ ਐਪ ਲਈ ਠੀਕ ਮਾਡਲ ਚੁਣਨ ਵਿੱਚ ਮਦਦ ਕਰਦੇ ਹਨ।

<details>
<summary><h3>🐍 ਪਾਈਥਨ</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ਕਿਸੇ ਵਿਸ਼ੇਸ਼ ਮਾਡਲ ਬਾਰੇ ਵਿਸਥਾਰਪੂਰਵਕ ਜਾਣਕਾਰੀ ਪ੍ਰਾਪਤ ਕਰੋ
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
<summary><h3>📘 ਜਾਵਾਸਕ੍ਰਿਪਟ</h3></summary>

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

#### FoundryModelInfo ਫੀਲਡ

| ਫੀਲਡ | ਕਿਸਮ | ਵੇਰਵਾ |
|-------|------|-------------|
| `alias` | string | ਛੋਟਾ ਨਾਂ (ਜਿਵੇਂ `phi-3.5-mini`) |
| `id` | string | ਯੂਨੀਕ ਮਾਡਲ ਪਹਚਾਣਕਰਤਾ |
| `version` | string | ਮਾਡਲ ਵਰਜਨ |
| `task` | string | `chat-completions` ਜਾਂ `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, ਜਾਂ NPU |
| `execution_provider` | string | ਰਨਟਾਈਮ ਬੈਕਐਂਡ (CUDA, CPU, QNN, WebGPU ਆਦਿ) |
| `file_size_mb` | int | MB ਵਿੱਚ ਡਿਸਕ ਤੇ ਸਾਈਜ਼ |
| `supports_tool_calling` | bool | ਕੀ ਮਾਡਲ ਫੰਕਸ਼ਨ/ਟੂਲ ਕਾਲਿੰਗ ਦਾ ਸਮਰਥਨ ਕਰਦਾ ਹੈ |
| `publisher` | string | ਜਿਸਨੇ ਮਾਡਲ ਪ੍ਰਕਾਸ਼ਿਤ ਕੀਤਾ |
| `license` | string | ਲਾਇਸੰਸ ਦਾ ਨਾਂ |
| `uri` | string | ਮਾਡਲ URI |
| `prompt_template` | dict/null | ਪ੍ਰਾਂਪਟ ਟੈਂਪਲੇਟ, ਜੇ ਹੋਵੇ |

---

### ਅਭਿਆਸ 5: ਮਾਡਲ ਲਾਈਫਸਾਈਕਲ ਪ੍ਰਬੰਧਨ ਕਰੋ

ਪੂਰੇ ਲਾਈਫਸਾਈਕਲ ਦਾ ਅਭਿਆਸ ਕਰੋ: ਲਿਸਟ → ਡਾਊਨਲੋਡ → ਲੋਡ → ਵਰਤੋ → ਅਨਲੋਡ ਕਰੋ।

<details>
<summary><h3>🐍 ਪਾਈਥਨ</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # ਤੇਜ਼ੀ ਨਾਲ ਟੈਸਟਿੰਗ ਲਈ ਛੋਟਾ ਮਾਡਲ

manager = FoundryLocalManager()
manager.start_service()

# 1. ਦੇਖੋ ਕਿ ਕੈਟਾਲਾਗ ਵਿੱਚ ਕੀ ਹੈ
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. ਦੇਖੋ ਕਿ ਕੀ ਪਹਿਲਾਂ ਹੀ ਡਾਊਨਲੋਡ ਕੀਤਾ ਗਿਆ ਹੈ
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. ਇੱਕ ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰੋ
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. ਪੁਸ਼ਟੀ ਕਰੋ ਕਿ ਹੁਣ ਇਹ ਕੈਸ਼ ਵਿੱਚ ਹੈ
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. ਇਸ ਨੂੰ ਲੋਡ ਕਰੋ
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. ਦੇਖੋ ਕਿ ਕੀ ਲੋਡ ਕੀਤਾ ਹੈ
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. ਇਸ ਨੂੰ ਅਨਲੋਡ ਕਰੋ
print(f"\nUnloading {alias}...")
manager.unload_model(alias)
loaded = manager.list_loaded_models()
print(f"Loaded models after unload: {len(loaded)}")
```

</details>

<details>
<summary><h3>📘 ਜਾਵਾਸਕ੍ਰਿਪਟ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "qwen2.5-0.5b"; // ਤੇਜ਼ੀ ਨਾਲ ਪਰੀਖਣ ਲਈ ਛੋਟਾ ਮਾਡਲ

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. ਕੈਟਾਲੌਗ ਤੋਂ ਮਾਡਲ ਪ੍ਰਾਪਤ ਕਰੋ
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. ਜੇ ਲੋੜ ਹੋਵੇ ਤਾਂ ਡਾਊਨਲੋਡ ਕਰੋ
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. ਇਸ ਨੂੰ ਲੋਡ ਕਰੋ
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. ਇਸ ਨੂੰ ਅਨਲੋਡ ਕਰੋ
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### ਕਸਰਤ 6: ਤੁਰੰਤ ਸ਼ੁਰੂਆਤ ਲਈ ਨਮੂਨੇ

ਹਰ ਭਾਸ਼ਾ ਇੱਕ ਕਾਲ ਵਿੱਚ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰਨ ਅਤੇ ਮਾਡਲ ਲੋਡ ਕਰਨ ਲਈ ਇੱਕ ਛੋਟੀ ਰਾਹਤ ਦਿੰਦੀ ਹੈ। ਇਹ ਬਹੁਤ ਸਾਰੀਆਂ ਐਪਲੀਕੇਸ਼ਨਾਂ ਲਈ **ਸਿਫਾਰਸ਼ੀ ਨਮੂਨੇ** ਹਨ।

<details>
<summary><h3>🐍 ਪਾਇਥਨ - ਕੰਸਟਰਕਟਰ ਬੂਟਸਟਰੈਪ</h3></summary>

```python
from foundry_local import FoundryLocalManager

# ਕਨਸਟਰਕਟਰ ਨੂੰ ਏਲੀਅਸ ਪਾਸ ਕਰੋ - ਇਹ ਸਭ ਕੁਝ ਸੰਭਾਲਦਾ ਹੈ:
# 1. ਸੇਵਾ ਚਾਲੂ ਨਹੀਂ ਹੈ ਤਾਂ ਚਾਲੂ ਕਰਦਾ ਹੈ
# 2. ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰਦਾ ਹੈ ਜੇਕਰ ਕੈਸ਼ ਵਿੱਚ ਨਹੀਂ ਹੈ
# 3. ਮਾਡਲ ਨੂੰ ਇੰਫਰੰਸ ਸਰਵਰ ਵਿੱਚ ਲੋਡ ਕਰਦਾ ਹੈ
manager = FoundryLocalManager("phi-3.5-mini")

# ਤੁਰੰਤ ਵਰਤੋਂ ਲਈ ਤਿਆਰ
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` ਪੈਰਾਮੀਟਰ (ਮੂਲ ਰੂਪ ਵਿੱਚ `True`) ਇਸ ਵਿਹੇਵਿਯਰ ਨੂੰ ਨਿਯੰਤ੍ਰਿਤ ਕਰਦਾ ਹੈ। ਜੇ ਤੁਹਾਨੂੰ ਮੈਨੂਅਲ ਕੰਟਰੋਲ ਚਾਹੀਦਾ ਹੈ ਤਾਂ `bootstrap=False` ਸੈਟ ਕਰੋ:

```python
# ਮੈਨੁਅਲ ਮੋਡ - ਕੁਝ ਵੀ ਆਪਣੇ ਆਪ ਨਹੀਂ ਹੁੰਦਾ ਹੈ
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 ਜਾਵਾਸਕ੍ਰਿਪਟ - `create()` + ਕੈਟਾਲੌਗ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() ਸਭ ਕੁਝ ਸੰਭਾਲਦੇ ਹਨ:
// 1. ਸੇਵਾ ਸ਼ੁਰੂ ਕਰਦਾ ਹੈ
// 2. ਕੈਟਲੌਗ ਵਿੱਚੋਂ ਮਾਡਲ ਪ੍ਰਾਪਤ ਕਰਦਾ ਹੈ
// 3. ਜਰੂਰਤ ਹੋਣ 'ਤੇ ਡਾਊਨਲੋਡ ਕਰਦਾ ਹੈ ਅਤੇ ਮਾਡਲ ਲੋਡ ਕਰਦਾ ਹੈ
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// ਤੁਰੰਤ ਵਰਤੋਂ ਲਈ ਤਿਆਰ ਹੈ
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + ਕੈਟਾਲੌਗ</h3></summary>

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

> **C# ਨੋਟ:** C# SDK ਐਪ ਨਾਮ, ਲਾਗਿੰਗ, ਕੈਚ ਡਾਇਰੈਕਟਰੀਜ਼ ਅਤੇ ਖਾਸ ਵੈੱਬ ਸਰਵਰ ਪੋਰਟ ਨੂੰ ਕੰਟਰੋਲ ਕਰਨ ਲਈ `Configuration` ਵਰਤਦਾ ਹੈ। ਇਹ ਤਿੰਨ SDKs ਵਿੱਚੋਂ ਸਭ ਤੋਂ ਜ਼ਿਆਦਾ ਕਨਫਿਗਰੇਬਲ ਹੈ।

</details>

---

### ਕਸਰਤ 7: ਨੈਟਿਵ ChatClient (ਕੋਈ OpenAI SDK ਲੋੜੀਂਦਾ ਨਹੀਂ)

ਜਾਵਾਸਕ੍ਰਿਪਟ ਅਤੇ C# SDK ਇੱਕ `createChatClient()` ਸੁਵਿਧਾ ਪদ্ধਤੀ ਦਿੰਦੇ ਹਨ ਜੋ ਇੱਕ ਨੈਟਿਵ ਚੈਟ ਕਲਾਇੰਟ ਦਿੰਦੀ ਹੈ — OpenAI SDK ਨੂੰ ਅਲੱਗ ਤੋਂ ਇੰਸਟਾਲ ਜਾਂ ਕਨਫਿਗਰ ਕਰਨ ਦੀ ਲੋੜ ਨਹੀਂ।

<details>
<summary><h3>📘 ਜਾਵਾਸਕ੍ਰਿਪਟ - <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// ਮਾਡਲ ਤੋਂ ਸਿੱਧਾ ChatClient ਬਣਾਓ — ਕਿਸੇ OpenAI ਇੰਪੋਰਟ ਦੀ ਲੋੜ ਨਹੀਂ
const chatClient = model.createChatClient();

// completeChat ਇੱਕ OpenAI-ਅਨੁਕੂਲ ਜਵਾਬ ਵਸਤੂ ਵਾਪਸ ਕਰਦਾ ਹੈ
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// ਸਟ੍ਰੀਮਿੰਗ ਕਾਲਬੈਕ ਪੈਟਰਨ ਦੀ ਵਰਤੋਂ ਕਰਦੀ ਹੈ
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` ਟੂਲ ਕਾਲ ਨੂੰ ਵੀ ਸਮਰਥਨ ਦਿੰਦਾ ਹੈ — ਟੂਲ ਨੂੰ ਦੂਜੇ ਆਰਗੁਮੈਂਟ ਵਜੋਂ ਪਾਸ ਕਰੋ:

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

> **ਕਿਸ ਪੈਟਰਨ ਨੂੰ ਕਦੋਂ ਵਰਤਣਾ ਹੈ:**
> - **`createChatClient()`** — ਤੇਜ਼ ਪ੍ਰੋਟੋਟਾਈਪਿੰਗ, ਘੱਟ ਡਿਪੈਂਡੇਨਸੀਜ਼, ਸੌਖਾ ਕੋਡ
> - **OpenAI SDK** — ਪੈਰਾਮੀਟਰਾਂ (ਤਰਮਿਆ, top_p, ਸਟਾਪ ਟੋਕਨ ਆਦਿ) 'ਤੇ ਪੂਰਾ ਕੰਟਰੋਲ, ਪ੍ਰੋਡਕਸ਼ਨ ਐਪਲੀਕੇਸ਼ਨਾਂ ਲਈ ਵਧੀਆ

---

### ਕਸਰਤ 8: ਮਾਡਲ ਵੈਰੀਅੰਟ ਅਤੇ ਹਾਰਡਵੇਅਰ ਚੋਣ

ਮਾਡਲਾਂ ਦੇ ਬਹੁਤ ਸਾਰੇ **ਵੈਰੀਅੰਟ** ਹੁੰਦੇ ਹਨ ਜੋ ਵੱਖ-ਵੱਖ ਹਾਰਡਵੇਅਰ ਲਈ ਉਤਮ ਹੁੰਦੇ ਹਨ। SDK ਸਭ ਤੋਂ ਵਧੀਆ ਵੈਰੀਅੰਟ ਆਪੋ-ਆਪ ਚੁਣਦਾ ਹੈ, ਪਰ ਤੁਸੀਂ ਮੈਨੂਅਲ ਵੀ ਇੰਸਪੈਕਟ ਕਰਕੇ ਚੁਣ ਸਕਦੇ ਹੋ।

<details>
<summary><h3>📘 ਜਾਵਾਸਕ੍ਰਿਪਟ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// ਸਾਰੇ ਉਪਲਬਧ ਵਿਭਿੰਨਤਾਵਾਂ ਦੀ ਸੂਚੀ ਬਣਾਓ
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK ਆਪਣੇ ਆਪ ਤੁਹਾਡੇ ਹਾਰਡਵੇਅਰ ਲਈ ਸਭ ਤੋਂ ਵਧੀਆ ਵਿਭਿੰਨਤਾ ਚੁਣਦਾ ਹੈ
// ਓਵਰਰਾਈਡ ਕਰਨ ਲਈ, selectVariant() ਵਰਤੋਂ:
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
<summary><h3>🐍 ਪਾਇਥਨ</h3></summary>

ਪਾਇਥਨ ਵਿੱਚ, SDK ਹਾਰਡਵੇਅਰ ਦੇ ਆਧਾਰ ਉਤੇ ਸਭ ਤੋਂ ਵਧੀਆ ਵੈਰੀਅੰਟ ਆਪੋ-ਆਪ ਚੁਣਦਾ ਹੈ। ਦੇਖਣ ਲਈ `get_model_info()` ਵਰਤੋ ਕਿ ਕੀ ਚੁਣਿਆ ਗਿਆ:

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

#### NPU ਵੈਰੀਅੰਟ ਵਾਲੇ ਮਾਡਲ

ਕੁਝ ਮਾਡਲ NPU-ਉਪਯੋਗ ਵੈਰੀਅੰਟ ਰੱਖਦੇ ਹਨ Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra)- ਵਾਲੇ ਡਿਵਾਈਸਾਂ ਲਈ:

| ਮਾਡਲ | NPU ਵੈਰੀਅੰਟ ਉਪਲਬਧ |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **ਟਿਪ:** NPU-ਸਮਰਥਿਤ ਹਾਰਡਵੇਅਰ 'ਤੇ, SDK ਜਦੋਂ ਵੀ ਉਪਲਬਧ ਹੁੰਦਾ ਹੈ NPU ਵੈਰੀਅੰਟ ਆਪੋ-ਆਪ ਚੁਣਦਾ ਹੈ। ਤੁਹਾਨੂੰ ਆਪਣੇ ਕੋਡ ਵਿੱਚ ਕੋਈ ਬਦਲਾਅ ਕਰਨ ਦੀ ਲੋੜ ਨਹੀਂ। ਵਿਂਡੋਜ਼ 'ਤੇ C# ਪ੍ਰੋਜੈਕਟਾਂ ਲਈ, `Microsoft.AI.Foundry.Local.WinML` NuGet ਪੈਕੇਜ ਜੋੜੋ ਤਾਂ ਜੋ QNN ਏਕਜ਼ੀਕਿਊਸ਼ਨ ਪ੍ਰੋਵਾਈਡਰ ਚਾਲੂ ਹੋ ਜਾਵੇ — QNN WinML ਦੁਆਰਾ ਪਲੱਗਇਨ ਏਪੀ ਰੂਪ ਵਿੱਚ ਦਿੱਤਾ ਜਾਂਦਾ ਹੈ।

---

### ਕਸਰਤ 9: ਮਾਡਲ ਅੱਪਗ੍ਰੇਡ ਅਤੇ ਕੈਟਾਲੌਗ ਰਿਫ੍ਰੈਸ਼

ਮਾਡਲ ਕੈਟਾਲੌਗ ਸਮੇਂ-ਸਮੇਂ ਤੇ ਅੱਪਡੇਟ ਹੁੰਦਾ ਹੈ। ਮੌਜੂਦਾ ਗੱਲਾਂ ਚੈੱਕ ਕਰਨ ਅਤੇ ਅੱਪਡੇਟ ਲਾਗੂ ਕਰਨ ਲਈ ਇਹ ਪদ্ধਤੀਆਂ ਵਰਤੋ।

<details>
<summary><h3>🐍 ਪਾਇਥਨ</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# ਨਵੀਨਤਮ ਮਾਡਲ ਸੂਚੀ ਪ੍ਰਾਪਤ ਕਰਨ ਲਈ ਕੈਟਾਲੌਗ ਨੂੰ ਤਾਜ਼ਾ ਕਰੋ
manager.refresh_catalog()

# ਜਾਂਚੋ ਕਿ ਕੀ ਸੰਗ੍ਰਹਿਤ ਮਾਡਲ ਦੇ ਕੋਲ ਨਵਾਂ ਸੰਸਕਰਣ ਉਪਲਬਧ ਹੈ
if manager.is_model_upgradeable(alias):
    print(f"{alias} has a newer version available!")
    manager.upgrade_model(alias)
    print("Upgrade complete")
else:
    print(f"{alias} is up to date")
```

</details>

<details>
<summary><h3>📘 ਜਾਵਾਸਕ੍ਰਿਪਟ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "UpgradeDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ਨਵੀਂ ਮਾਡਲ ਸੂਚੀ ਲੈਣ ਲਈ ਕੈਟਾਲਾਗ ਨੂੰ ਤਾਜ਼ਾ ਕਰੋ
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// ਤਾਜ਼ਾ ਕਰਨ ਤੋਂ ਬਾਅਦ ਸਭ ਉਪਲਬਧ ਮਾਡਲਾਂ ਦੀ ਸੂਚੀ ਦਿਖਾਓ
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### ਕਸਰਤ 10: ਤਰਕ ਸਹਿਤ ਮਾਡਲਾਂ 'ਤੇ ਕੰਮ ਕਰਨਾ

**phi-4-mini-reasoning** ਮਾਡਲ ਵਿੱਚ ਸੀਰੀ ਟਰਕ-ਵਿਚਾਰ ਸ਼ਾਮਲ ਹੈ। ਇਹ ਆਪਣੇ ਅੰਦਰੂਨੀ ਸੋਚ ਨੂੰ `<think>...</think>` ਟੈਗਾਂ ਵਿੱਚ ਲਪੇਟਦਾ ਹੈ ਫਾਈਨਲ ਜਵਾਬ ਦੇਣ ਤੋਂ ਪਹਿਲਾਂ। ਇਹ ਉਹ ਕਾਰਜ ਜੋ ਮਲਟੀ-ਸਟੈਪ ਲਾਜਿਕ, ਗਣਿਤ ਜਾਂ ਸਮੱਸਿਆ-ਹੱਲ ਲਈ ਲੋੜੀਂਦੇ ਹਨ, ਵਿੱਚ ਮੁਆਫ਼ਕਦਮ ਹੈ।

<details>
<summary><h3>🐍 ਪਾਇਥਨ</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning ਲਗਭਗ 4.6 ਜੀਬੀ ਹੈ
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# ਮਾਡਲ ਆਪਣੀ ਸੋਚ ਨੂੰ <think>...</think> ਟੈਗ ਵਿੱਚ ਲਪੇਟਦਾ ਹੈ
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
<summary><h3>📘 ਜਾਵਾਸਕ੍ਰਿਪਟ</h3></summary>

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

// ਸੋਚ ਦੀ ਲੜੀ ਨੂੰ ਵਿਵਰਣ ਕਰੋ
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **ਕਦੋਂ reasoning ਮਾਡਲ ਵਰਤਣਾ:**
> - ਗਣਿਤ ਅਤੇ ਲਾਜਿਕ ਸਮੱਸਿਆਵਾਂ
> - ਮਲਟੀ-ਸਟੈਪ ਯੋਜਨਾ ਬਣਾਉਣ ਵਾਲੇ ਕੰਮ
> - ਜਟਿਲ ਕੋਡ ਬਣਾਉਣਾ
> - ਉਹ ਕੰਮ ਜਿੱਥੇ ਠੀਕ ਢੰਗ ਨਾਲ ਕੰਮ ਕਰਨ ਨਾਲ ਨਤੀਜੇ ਬਿਹਤਰ ਹੁੰਦੇ ਹਨ
>
> **ਵਿਖੇੜਾ:** Reasoning ਮਾਡਲ ਵੱਧ ਟੋਕਨ (ਜਿਵੇਂ `<think>` ਹਿੱਸਾ) ਨਿਕਲਦੇ ਹਨ ਅਤੇ ਹੌਲੀ ਚੱਲਦੇ ਹਨ। ਸਧਾਰਨ ਸਵਾਲ/ਜਵਾਬ ਲਈ, ਜਿਵੇਂ phi-3.5-mini ਤੇਜ਼ ਹਨ।

---

### ਕਸਰਤ 11: ਅਲੀਅਸ ਅਤੇ ਹਾਰਡਵੇਅਰ ਚੋਣ ਸਮਝਣਾ

ਜਦੋਂ ਤੁਸੀਂ ਪੂਰੇ ਮਾਡਲ ID ਦੀ ਬਜਾਏ **ਅਲੀਅਸ** (ਜਿਵੇਂ `phi-3.5-mini`) ਦਿੰਦੇ ਹੋ, SDK ਆਪਣੇ ਆਪ ਤੁਹਾਡੇ ਹਾਰਡਵੇਅਰ ਲਈ ਸਭ ਤੋਂ ਵਧੀਆ ਵੈਰੀਅੰਟ ਚੁਣਦਾ ਹੈ:

| ਹਾਰਡਵੇਅਰ | ਚੁਣਿਆ ਗਿਆ ਏਕਜ਼ੀਕਿਊਸ਼ਨ ਪ੍ਰੋਵਾਈਡਰ |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML ਪਲੱਗਇਨ ਰਾਹੀਂ) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| ਕਿਸੇ ਵੀ ਡਿਵਾਈਸ (ਬੈਕਅਪ) | `CPUExecutionProvider` ਜਾਂ `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ਇਹ ਅਲੀਅਸ ਤੁਹਾਡੇ ਹਾਰਡਵੇਅਰ ਲਈ ਸਭ ਤੋਂ ਵਧੀਆ ਵਿਕਲਪ ਨੂੰ ਹੱਲ ਕਰਦਾ ਹੈ
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **ਟਿਪ:** ਸਦਾ ਆਪਣੇ ਐਪਲਿਕੇਸ਼ਨ ਕੋਡ ਵਿੱਚ ਅਲੀਅਸ ਵਰਤੋਂ। ਜਦੋਂ ਤੁਸੀਂ ਕਿਸੇ ਉਪਭੋਗਤਾ ਦੇ ਮਸ਼ੀਨ 'ਤੇ ਡਿਪਲੌਇ ਕਰੋ, SDK ਚਾਲਾਨ ਸਮੇਂ ਸਭ ਤੋਂ ਵਧੀਆ ਵੈਰੀਅੰਟ ਚੁਣਦਾ ਹੈ - NVIDIA ਲਈ CUDA, Qualcomm ਲਈ QNN, ਬਾਕੀ ਲਈ CPU।

---

### ਕਸਰਤ 12: C# ਕਨਫਿਗਰੇਸ਼ਨ ਵਿਕਲਪ

C# SDK ਦੀ `Configuration` ਕਲਾਸ ਰਨਟਾਈਮ 'ਤੇ ਬਾਰੀਕੀ ਨਿਯੰਤਰਣ ਦਿੰਦੀ ਹੈ:

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

| ਪ੍ਰਾਪਰਟੀ | ਮੂਲ ਰੂਪ | ਵੇਰਵਾ |
|----------|---------|-------------|
| `AppName` | (ਲੋੜੀਂਦਾ) | ਤੁਹਾਡੇ ਐਪ ਦਾ ਨਾਮ |
| `LogLevel` | `Information` | ਲਾਗਿੰਗ ਦੀ ਵਰਣਵਾਦਤਾ |
| `Web.Urls` | (ਡਾਇਨਾਮਿਕ) | ਵੈੱਬ ਸਰਵਰ ਲਈ ਖਾਸ ਪੋਰਟ ਨਿਯਤ ਕਰੋ |
| `AppDataDir` | OS ਦਾ ਡੀਫਾਲਟ | ਐਪ ਡੇਟਾ ਲਈ ਬੇਸ ਡਾਇਰੈਕਟਰੀ |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | ਮਾਡਲ ਕਿੱਥੇ ਸਟੋਰ ਹੁੰਦੇ ਹਨ |
| `LogsDir` | `{AppDataDir}/logs` | ਕਿੱਥੇ ਲਾਗ ਲਿਖੇ ਜਾਂਦੇ ਹਨ |

---

### ਕਸਰਤ 13: ਬਰਾਉਜ਼ਰ ਵਰਤੋਂ (ਕੇਵਲ ਜਾਵਾਸਕ੍ਰਿਪਟ)

ਜਾਵਾਸਕ੍ਰਿਪਟ SDK ਇੱਕ ਬਰਾਉਜ਼ਰ-ਅਨੁਕੂਲ ਵਰਜਨ ਸ਼ਾਮਲ ਕਰਦਾ ਹੈ। ਬਰਾਉਜ਼ਰ ਵਿੱਚ, ਸੇਵਾ ਮੈਨੂਅਲ CLI ਰਾਹੀਂ ਸ਼ੁਰੂ ਕਰਨੀ ਪੈਂਦੀ ਹੈ ਅਤੇ ਹੋਸਟ URL ਦੱਸਣਾ ਪੈਂਦਾ ਹੈ:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// ਸਭ ਤੋਂ ਪਹਿਲਾਂ ਸੇਵਾ ਨੂੰ ਹੱਥੋਂ ਚਾਲੂ ਕਰੋ:
//   ਫਾਉਂਡਰੀ ਸੇਵਾ ਸਟਾਰਟ
// ਫਿਰ CLI ਆਉਟਪੁੱਟ ਤੋਂ URL ਦੀ ਵਰਤੋਂ ਕਰੋ
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// ਕੈਟਾਲੋਗ ਨੂੰ ਬ੍ਰਾਊਜ਼ ਕਰੋ
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **ਬਰਾਉਜ਼ਰ ਸੀਮਾਵਾਂ:** ਬਰਾਉਜ਼ਰ ਵਰਜਨ `startWebService()` ਦਾ ਸਮਰਥਨ ਨਹੀਂ ਕਰਦਾ। Foundry Local ਸੇਵਾ ਪਹਿਲਾਂ ਤੋਂ ਚਲ ਰਹੀ ਹੋਣੀ ਚਾਹੀਦੀ ਹੈ ਜਦੋਂ SDK ਨੂੰ ਬਰਾਉਜ਼ਰ ਵਿੱਚ ਵਰਤਣਾ ਹੋਵੇ।

---

## ਪੂਰਾ API ਹਵਾਲਾ

### ਪਾਇਥਨ

| ਸ਼੍ਰੇਣੀ | ਵਿਧੀ | ਵੇਰਵਾ |
|----------|--------|-------------|
| **ਆਰੰਭ** | `FoundryLocalManager(alias?, bootstrap=True)` | ਮੈਨੇਜਰ ਬਣਾਓ; ਵਿਕਲਪਕ ਤੌਰ ਤੇ ਮਾਡਲ ਨਾਲ ਬੂਟਸਟਰੈਪ ਕਰੋ |
| **ਸੇਵਾ** | `is_service_running()` | ਚੈੱਕ ਕਰੋ ਕਿ ਸੇਵਾ ਚੱਲ ਰਹੀ ਹੈ ਜਾਂ ਨਹੀਂ |
| **ਸੇਵਾ** | `start_service()` | ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ |
| **ਸੇਵਾ** | `endpoint` | API ਐਂਡਪੌਇੰਟ URL |
| **ਸੇਵਾ** | `api_key` | API ਕੁੰਜੀ |
| **ਕੈਟਾਲੌਗ** | `list_catalog_models()` | ਸਾਰੇ ਉਪਲਬਧ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `refresh_catalog()` | ਕੈਟਾਲੌਗ ਰਿਫ੍ਰੈਸ਼ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `get_model_info(alias_or_model_id)` | ਮਾਡਲ ਮੈਟਾਡੇਟਾ ਪ੍ਰਾਪਤ ਕਰੋ |
| **ਕੇਸ਼** | `get_cache_location()` | ਕੈਸ਼ ਡਾਇਰੈਕਟਰੀ ਮਾਰਗ |
| **ਕੇਸ਼** | `list_cached_models()` | ਡਾਊਨਲੋਡ ਕੀਤੇ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |
| **ਮਾਡਲ** | `download_model(alias_or_model_id)` | ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰੋ |
| **ਮਾਡਲ** | `load_model(alias_or_model_id, ttl=600)` | ਮਾਡਲ ਲੋਡ ਕਰੋ |
| **ਮਾਡਲ** | `unload_model(alias_or_model_id)` | ਮਾਡਲ ਅਨਲੋਡ ਕਰੋ |
| **ਮਾਡਲ** | `list_loaded_models()` | ਲੋਡ ਕੀਤੇ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |
| **ਮਾਡਲ** | `is_model_upgradeable(alias_or_model_id)` | ਦੇਖੋ ਜੇ ਨਵਾਂ ਵਰਜ਼ਨ ਉਪਲਬਧ ਹੈ |
| **ਮਾਡੇਲ** | `upgrade_model(alias_or_model_id)` | ਮਾਡਲ ਨੂੰ ਤਾਜ਼ਾ ਵਰਜ਼ਨ 'ਤੇ ਅਪਗਰੇਡ ਕਰੋ |
| **ਸੇਵਾ** | `httpx_client` | ਸਿੱਧੇ API ਕਾਲ ਲਈ ਪ੍ਰੀ-ਕਨਫਿਗਰਡ HTTPX ਕਲਾਇੰਟ |

### ਜਾਵਾਸਕ੍ਰਿਪਟ

| ਸ਼੍ਰੇਣੀ | ਵਿਧੀ | ਵੇਰਵਾ |
|----------|--------|-------------|
| **ਆਰੰਭ** | `FoundryLocalManager.create(options)` | SDK ਸਿੰਗਲਟਨ ਸ਼ੁਰੂ ਕਰੋ |
| **ਆਰੰਭ** | `FoundryLocalManager.instance` | ਸਿੰਗਲਟਨ ਮੈਨੇਜਰ ਤੱਕ ਪਹੁੰਚ |
| **ਸੇਵਾ** | `manager.startWebService()` | ਵੈੱਬ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ |
| **ਸੇਵਾ** | `manager.urls` | ਸੇਵਾ ਲਈ ਬੇਸ URL ਦੀ ਲਿਸਟ |
| **ਕੈਟਾਲੌਗ** | `manager.catalog` | ਮਾਡਲ ਕੈਟਾਲੌਗ ਤੱਕ ਪਹੁੰਚ |
| **ਕੈਟਾਲੌਗ** | `catalog.getModel(alias)` | ਅਲੀਅਸ ਦੁਆਰਾ ਮਾਡਲ ਆਬਜੈਕਟ ਪਾਓ (Promise ਰਿਟਰਨ) |
| **ਮਾਡਲ** | `model.isCached` | ਮਾਡਲ ਡਾਊਨਲੋਡ ਹੈ ਜਾਂ ਨਹੀਂ |
| **ਮਾਡਲ** | `model.download()` | ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰੋ |
| **ਮਾਡਲ** | `model.load()` | ਮਾਡਲ ਲੋਡ ਕਰੋ |
| **ਮਾਡਲ** | `model.unload()` | ਮਾਡਲ ਅਨਲੋਡ ਕਰੋ |
| **ਮਾਡਲ** | `model.id` | ਮਾਡਲ ਦੀ ਯੂਨੀਕ ਪਹਿਚਾਣ |
| **ਮਾਡਲ** | `model.alias` | ਮਾਡਲ ਦਾ ਅਲੀਅਸ |
| **ਮਾਡਲ** | `model.createChatClient()` | ਨੈਟਿਵ ਚੈਟ ਕਲਾਇੰਟ ਪ੍ਰਾਪਤ ਕਰੋ (OpenAI SDK ਦੀ ਲੋੜ ਨਹੀਂ) |
| **ਮਾਡਲ** | `model.createAudioClient()` | ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਲਈ ਆਡੀਓ ਕਲਾਇੰਟ ਪ੍ਰਾਪਤ ਕਰੋ |
| **ਮਾਡਲ** | `model.removeFromCache()` | ਮਾਡਲ ਨੂੰ ਲੋਕਲ ਕੈਸ਼ ਤੋਂ ਹਟਾਓ |
| **ਮਾਡਲ** | `model.selectVariant(variant)` | ਕਿਸੇ ਖਾਸ ਹਾਰਡਵੇਅਰ ਵੈਰੀਅੰਟ ਚੁਣੋ |
| **ਮਾਡਲ** | `model.variants` | ਇਸ ਮਾਡਲ ਲਈ ਉਪਲਬਧ ਵੈਰੀਅੰਟਾਂ ਦੀ ਲਿਸਟ |
| **ਮਾਡਲ** | `model.isLoaded()` | ਜਾਂਚੋ ਕਿ ਮਾਡਲ ਲੋਡ ਹੈ ਜਾਂ ਨਹੀਂ |
| **ਕੈਟਾਲੌਗ** | `catalog.getModels()` | ਸਾਰੇ ਉਪਲਬਧ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `catalog.getCachedModels()` | ਡਾਊਨਲੋਡ ਕੀਤੇ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `catalog.getLoadedModels()` | ਵਰਤਮਾਨ ਵਿੱਚ ਲੋਡ ਕੀਤੇ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `catalog.updateModels()` | ਸੇਵਾ ਤੋਂ ਕੈਟਾਲੌਗ ਰਿਫ੍ਰੈਸ਼ ਕਰੋ |
| **ਸੇਵਾ** | `manager.stopWebService()` | Foundry Local ਵੈੱਬ ਸੇਵਾ ਬੰਦ ਕਰੋ |

### C# (v0.8.0+)

| ਸ਼੍ਰੇਣੀ | ਵਿਧੀ | ਵੇਰਵਾ |
|----------|--------|-------------|
| **ਆਰੰਭ** | `FoundryLocalManager.CreateAsync(config, logger)` | ਮੈਨੇਜਰ ਸ਼ੁਰੂ ਕਰੋ |
| **ਆਰੰਭ** | `FoundryLocalManager.Instance` | ਸਿੰਗਲਟਨ ਤੱਕ ਪਹੁੰਚ |
| **ਕੈਟਾਲੌਗ** | `manager.GetCatalogAsync()` | ਕੈਟਾਲੌਗ ਪ੍ਰਾਪਤ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `catalog.ListModelsAsync()` | ਸਾਰੇ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `catalog.GetModelAsync(alias)` | ਕੋਈ ਖਾਸ ਮਾਡਲ ਪ੍ਰਾਪਤ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `catalog.GetCachedModelsAsync()` | ਕੈਸ਼ ਕੀਤੇ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `catalog.GetLoadedModelsAsync()` | ਲੋਡ ਕੀਤੇ ਮਾਡਲ ਲਿਸਟ ਕਰੋ |
| **ਮਾਡਲ** | `model.DownloadAsync(progress?)` | ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰੋ |
| **ਮਾਡਲ** | `model.LoadAsync()` | ਮਾਡਲ ਲੋਡ ਕਰੋ |
| **ਮਾਡਲ** | `model.UnloadAsync()` | ਮਾਡਲ ਅਨਲੋਡ ਕਰੋ |
| **ਮਾਡਲ** | `model.SelectVariant(variant)` | ਹਾਰਡਵੇਅਰ ਵੈਰੀਅੰਟ ਚੁਣੋ |
| **ਮਾਡਲ** | `model.GetChatClientAsync()` | ਨੈਟਿਵ ਚੈਟ ਕਲਾਇੰਟ ਪ੍ਰਾਪਤ ਕਰੋ |
| **ਮਾਡਲ** | `model.GetAudioClientAsync()` | ਆਡੀਓ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕਲਾਇੰਟ ਪ੍ਰਾਪਤ ਕਰੋ |
| **ਮਾਡਲ** | `model.GetPathAsync()` | ਲੋਕਲ ਫਾਇਲ ਮਾਰਗ ਪ੍ਰਾਪਤ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `catalog.GetModelVariantAsync(alias, variant)` | ਖਾਸ ਹਾਰਡਵੇਅਰ ਵੈਰੀਅੰਟ ਪ੍ਰਾਪਤ ਕਰੋ |
| **ਕੈਟਾਲੌਗ** | `catalog.UpdateModelsAsync()` | ਕੈਟਾਲੌਗ ਰਿਫ੍ਰੈਸ਼ ਕਰੋ |
| **ਸਰਵਰ** | `manager.StartWebServerAsync()` | REST ਵੈੱਬ ਸਰਵਰ ਸ਼ੁਰੂ ਕਰੋ |
| **ਸਰਵਰ** | `manager.StopWebServerAsync()` | REST ਵੈੱਬ ਸਰਵਰ ਬੰਦ ਕਰੋ |
| **ਕੰਫਿਗ** | `config.ModelCacheDir` | ਕੈਸ਼ ਡਾਇਰੈਕਟਰੀ |

---

## ਮੁੱਖ ਕਥਨ

| ਧਾਰਨਾ | ਤੁਸੀਂ ਕੀ ਸਿੱਖਿਆ |
|---------|-----------------|
| **SDK ਵਿਰੁੱਧ CLI** | SDK ਵਰਗੀ ਪ੍ਰੋਗਰਾਮੈਟਿਕ ਕੰਟਰੋਲ ਪ੍ਰਦਾਨ ਕਰਦਾ ਹੈ - ਐਪਸ ਲਈ ਜਰੂਰੀ |
| **ਕੰਟਰੋਲ ਪਲੇਨ** | SDK ਸੇਵਾਵਾਂ, ਮਾਡਲਾਂ ਅਤੇ ਕੈਸ਼ਿੰਗ ਨੂੰ ਸੰਭਾਲਦਾ ਹੈ |
| **ਡਾਇਨਾਮਿਕ ਪੋਰਟ** | ਹਮੇਸ਼ਾ `manager.endpoint` (Python) ਜਾਂ `manager.urls[0]` (JS/C#) ਵਰਤੋ - ਕਦੇ ਵੀ ਕਠੋਰ ਪੋਰਟ ਨਾ ਦਿਓ |
| **ਅਲੀਅਸ** | ਆਪਣੇ ਐਪ ਕੋਡ ਵਿੱਚ ਅਲੀਅਸ ਵਰਤੋਂ ਤਾਂ ਜੋ ਹਾਰਡਵੇਅਰ-anੁਕੂਲ ਮਾਡਲ ਚੁਣਿਆ ਜਾਵੇ |
| **ਕੁਇੱਕ-ਸਟਾਰਟ** | ਪਾਇਥਨ: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# ਰੀਡਿਜ਼ਾਈਨ** | v0.8.0+ ਸਵੈ-ਨਿਰਭਰ - ਅੰਤ-ਉਪਭੋਗਤਾ ਮਸ਼ੀਨਾਂ ‘ਤੇ ਕੋਈ CLI ਲੋੜੀਂਦਾ ਨਹੀਂ |
| **ਮਾਡਲ ਜੀਵਨ ਚਕਰ** | ਕੈਟਲੌਗ → ਡਾਊਨਲੋਡ → ਲੋਡ → ਵਰਤੋਂ → ਅਨਲੋਡ |
| **FoundryModelInfo** | ਧਨੀ ਮੈਟਾਡੇਟਾ: ਟਾਸਕ, ਡਿਵਾਈਸ, ਆਕਾਰ, ਲਾਇਸੈਂਸ, ਟੂਲ ਕਾਲਿੰਗ ਸਹਾਇਤਾ |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) ਖੁੱਲ੍ਹੇAI-ਮੁਫ਼ਤ ਵਰਤੋਂ ਲਈ |
| **ਵੈਰੀਅੰਟਸ** | ਮਾਡਲਾਂ ਕੋਲ ਹਾਰਡਵੇਅਰ-ਵਿਸ਼ੇਸ਼ ਵੈਰੀਅੰਟਸ ਹਨ (CPU, GPU, NPU); ਆਪੋਆਪ ਚੁਣਿਆ ਜਾਂਦਾ ਹੈ |
| **ਅਪਗ੍ਰੇਡਸ** | ਪਾਇਥਨ: `is_model_upgradeable()` + `upgrade_model()` ਨਾਲ ਮਾਡਲਾਂ ਨੂੰ ਅਪ-ਟੂ-ਡੇਟ ਰੱਖੋ |
| **ਕੈਟਲੌਗ ਰਿਫ੍ਰੈਸ਼** | `refresh_catalog()` (ਪਾਇਥਨ) / `updateModels()` (JS) ਨਾਲ ਨਵੇਂ ਮਾਡਲਾਂ ਦੀ ਖੋਜ ਕਰੋ |

---

## ਸਰੋਤ

| ਸਰੋਤ | ਲਿੰਕ |
|----------|------|
| SDK ਰੈਫਰੈਂਸ (ਸਭ ਭਾਸ਼ਾਵਾਂ) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| ਇੰਫਰੈਂਸ SDKs ਨਾਲ ਇੰਟਿੱਗਰੇਸ਼ਨ | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API ਰੈਫਰੈਂਸ | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK ਨਮੂਨੇ | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local ਵੈੱਬਸਾਈਟ | [foundrylocal.ai](https://foundrylocal.ai) |

---

## ਅਗਲੇ ਕਦਮ

ਜਾਰੀ ਰੱਖੋ [ਪਾਰਟ 3: SDK ਨੂੰ OpenAI ਨਾਲ ਵਰਤਣਾ](part3-sdk-and-apis.md) ਲਈ, SDK ਨੂੰ OpenAI ਕਲਾਇੰਟ ਲਾਇਬ੍ਰੇਰੀ ਨਾਲ ਜੁੜਨ ਅਤੇ ਆਪਣੀ ਪਹਿਲੀ ਚੈਟ ਪੂਰਨਤਾ ਐਪਲੀਕੇਸ਼ਨ ਬਣਾਉਣ ਲਈ।

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਅਸਹਿਯਤਾ ਸੂਚਨਾ**:  
ਇਹ ਦਸਤਾਵੇਜ਼ AI ਅਨੁਵਾਦ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਅਨੁਵਾਦਿਤ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਕਿ ਅਸੀਂ ਸਹੀਤਾ ਲਈ ਯਤਨ ਕਰਦੇ ਹਾਂ, ਕਿਰਪਾ ਕਰਕੇ ਧਿਆਨ ਵਿੱਚ ਰੱਖੋ ਕਿ ਸਵੈਚਾਲਿਤ ਅਨੁਵਾਦਾਂ ਵਿੱਚ ਗਲਤੀਆਂ ਜਾਂ ਅਸਪੱਸ਼ਟਤਾਵਾਂ ਹੋ ਸਕਦੀਆਂ ਹਨ। ਮੂਲ ਦਸਤਾਵੇਜ਼ ਜਿਸਦੀ ਮੂਲ ਭਾਸ਼ਾ ਹੈ, ਉਸਨੂੰ ਅਧਿਕਾਰਿਕ ਸ੍ਰੋਤ ਵਜੋਂ ਲਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਜ਼ਰੂਰੀ ਜਾਣਕਾਰੀ ਲਈ, ਪੇਸ਼ੇਵਰ ਮਨੁੱਖੀ ਅਨੁਵਾਦ ਦੀ ਸਿਫਾਰਿਸ਼ ਕੀਤੀ ਜਾਦੀ ਹੈ। ਸਾਨੂੰ ਇਸ ਅਨੁਵਾਦ ਦੇ ਉਪਯੋਗ ਤੋਂ ਪੈਦਾ ਹੋਣ ਵਾਲੀਆਂ ਕਿਸੇ ਵੀ ਗਲਤ ਫਹਿਮੀਆਂ ਜਾਂ ਗਲਤ ਵਿਆਖਿਆਵਾਂ ਲਈ ਜ਼ਿੰਮੇਵਾਰ ਨਹੀਂ ਠਹਿਰਾਇਆ ਜਾ ਸਕਦਾ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->