![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ಭಾಗ 2: Foundry Local SDK ಆಳವಾದ ಅಧ್ಯಯನ

> **ಲಕ್ಷ್ಯ:** ಮಾದರಿಗಳು, ಸೇವೆಗಳು ಮತ್ತು ಕ್ಯಾಶಿಂಗ್ ಅನ್ನು ಪ್ರೋಗ್ರಾಮ್ಯಾಟಿಕ طورದಲ್ಲಿ ನಿರ್ವಹಿಸಲು Foundry Local SDKವನ್ನು ಪೋಷಿಸುವುದು - ಮತ್ತು ಏಕೆ SDK ಅನ್ನು CLIಗೆ ಬದಲಾಗಿ ಅಪ್ಲಿಕೇಶನ್‌ಗಳನ್ನು ನಿರ್ಮಿಸಲು ಶಿಫಾರಸು ಮಾಡಲಾಗಿದೆ ಎಂದು ಅರ್ಥಮಾಡಿಕೊಳ್ಳಲು.

## ಅವಲೋಕನ

ಭಾಗ 1 ರಲ್ಲಿ ನೀವು **Foundry Local CLI**ನ್ನು ಬಳಸಿ ಮಾದರಿಗಳನ್ನು ಡೌನ್ಲೋಡ್ ಮಾಡಿ ಇಂಟರಾಕ್ಟಿವ್ ಆಗಿ ರನ್ ಮಾಡಿದ್ದೀರಿ. CLI ಅನ್ವೇಷಣೆಗೆ ಒಳ್ಳೆಯದು, ಆದರೆ ನೀವು ನೈಜ ಅಪ್ಲಿಕೇಶನ್‌ಗಳನ್ನು ನಿರ್ಮಿಸಿದಾಗ **ಪ್ರೋಗ್ರಾಮ್ಯಾಟಿಕ್ ನಿಯಂತ್ರಣ** ಬೇಕಾಗುತ್ತದೆ. Foundry Local SDK ಅದನ್ನು ನಿಮಗೆ ನೀಡುತ್ತದೆ - ಇದು **ನಿಯಂತ್ರಣ ಪದವಿ** (ಸೇವೆ ಪ್ರಾರಂಭಿಸುವುದು, ಮಾದರಿಗಳನ್ನು ಪತ್ತೆಮಾಡುವುದು, ಡೌನ್ಲೋಡ್ ಮಾಡುವುದು, ಲೋಡ್ ಮಾಡುವುದು) ನಿರ್ವಹಿಸುತ್ತದೆ ನಿಮ್ಮ ಅಪ್ಲಿಕೇಶನ್ ಕೋಡ್‌ಗೆ **ಡೇಟಾ ಪದವಿ** (ಪ್ರಾಂಪ್ಟ್ ಕಳುಹಿಸುವುದು, ಪೂರ್ಣಗೊಳಿಸುವಿಕೆಗಳು ಸ್ವೀಕರಿಸುವುದು) ಮೇಲೆ ಗಮನಹರಿಸಲು ಸಾಧ್ಯವಾಗುತ್ತದೆ.

ಈ ಲ್ಯಾಬ್ ಪೈಥಾನ್, ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C# ನಲ್ಲಿ SDK API ಪೂರ್ಣ ದೇಪ್ತಿಯನ್ನು ಕಲಿಸುತ್ತದೆ. ಕೊನೆಗೆ ನೀವು ಲಭ್ಯವಿರುವ ಪ್ರತಿಯೊಂದು ವಿಧಾನ ಮತ್ತು ಯಾವಾಗ ಯಾವದನ್ನು ಬಳಸಬೇಕೆಂದು ಅರ್ಥಮಾಡಿಕೊಳ್ಳುತ್ತೀರಿ.

## Lernning Objectives

ಈ ಲ್ಯಾಬ್ ಕೊನೆಗೆ ನೀವು ಸಹಯೋಗ ಮಾಡಬಲ್ಲಿರಿ:

- ಅಪ್ಲಿಕೇಶನ್ ಡೆವಲಪ್‌ಮೆಂಟ್‌ಗಾಗಿ SDK CLIಕ್ಕಿಂತ ಮೇಲುಗೈ ಹೊಂದಿರುವುದಕ್ಕೆ ಕಾರಣವನ್ನು ವಿವರಿಸುವುದು
- Python, JavaScript, ಅಥವಾ C# ಗಾಗಿ Foundry Local SDK ಅನ್ನು ಸ್ಥಾಪಿಸುವುದು
- `FoundryLocalManager` ಬಳಸಿ ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸುವುದು, ಮಾದರಿಗಳನ್ನು ನಿರ್ವಹಿಸುವುದು ಮತ್ತು ಕಾಗಾಲॉग್ ಅನ್ನು ಪ್ರಶ್ನಿಸುವುದು
- ಪ್ರೋಗ್ರಾಮ್ಯಾಟಿಕಾಗಿ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡುವುದು, ಡೌನ್ಲೋಡ್ ಮಾಡುವುದು, ಲೋಡ್ ಮಾಡುವುದು ಮತ್ತು ಅನ್ಲೋಡ್ ಮಾಡುವುದು
- `FoundryModelInfo` ಉಪಯೋಗಿಸಿ ಮಾದರಿ ಮೆಟಾಡೇಟಾವನ್ನು ಪರಿಶೀಲಿಸುವುದು
- ಕಾಗಾಲಾಕ್, ಕ್ಯಾಶೆ ಮತ್ತು ಲೋಡ್ ಮಾಡಿದ ಮಾದರಿಗಳ ನಡುವಿನ ವ್ಯತ್ಯಾಸವನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳುವುದು
- ಕಾಂಸ್ಟ್ರಕ್ಟರ್ ಬೂಟ್ಸ್ಟ್ರ್ಯಾಪ್ (Python) ಮತ್ತು `create()` + ಕಾಗಾಲಕ್ ಮಾದರಿಯನ್ನು (JavaScript) ಬಳಸುವುದು
- C# SDK ಮರು ವಿನ್ಯಾಸ ಮತ್ತು ಅದರ ವಸ್ತುನಿಷ್ಟ API ಅನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳುವುದು

---

## ಪೂರ್ವ ಅವಶ್ಯಕತೆಗಳು

| ಅಗತ್ಯವಿದ್ದು | ವಿವರಗಳು |
|-------------|---------|
| **Foundry Local CLI** | ನಿಮ್ಮ `PATH` ನಲ್ಲಿ ಇನ್ಸ್ಟಾಲ್ ಆಗಿದೆ ([ಭಾಗ 1](part1-getting-started.md)) |
| **ಭಾಷಾ ರನ್‌ಟೈಮ್** | **Python 3.9+** ಮತ್ತು/ಅಥವಾ **Node.js 18+** ಮತ್ತು/ಅಥವಾ **.NET 9.0+** |

---

## ಕಲ್ಪನೆ: SDK ವಿರುದ್ಧ CLI - SDK ಯನ್ನು ಏಕೆ ಬಳಸಬೇಕು?

| ಅಂಶ | CLI (`foundry` ಕಮಾಂಡ್) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **ಬಳಕೆ ಪ್ರಕರಣ** | ಅನ್ವೇಷಣೆ, ಕೈಯಿಂದ ಪರೀಕ್ಷೆ | ಅಪ್ಲಿಕೇಶನ್ ಏಕಗೂಡಿಕೆ |
| **ಸೇವೆಯ ನಿರ್ವಹಣೆ** | ಕೈಯಿಂದ: `foundry service start` | ಸ್ವಯಂಚಾಲಿತ: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **ಪೋರ್ಟ್ ಪತ್ತೆಮಾಡುವುದು** | CLI ಔಟ್‌ಪುಟ್‌ನಿಂದ ಓದು | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **ಮಾದರಿ ಡೌನ್ಲೋಡ್** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **ದೋಷ ನಿರ್ವಹಣೆ** | ಎಕ್ಸಿಟ್ ಕೋಡ್‌ಗಳು, ಸ್ಟ್ಡೆರ್ | ವಿಸ್ತಾರಗಳು, ಟೈಪ್ಡ್ ದೋಷಗಳು |
| **ಸ್ವಯಂಕ್ರಿಯತೆ** | ಶೆಲ್ ಸ್ಕ್ರಿಪ್ಟ್‌ಗಳು | ನೇಟಿವ್ ಭಾಷಾ ಏಕಗೂಡಿಕೆ |
| **ವಿತರಣಾ ವಿಧಾನ** | ಅಂತಿಮ ಬಳಕೆದಾರ ಯಂತ್ರದಲ್ಲಿ CLI ಅಗತ್ಯವಿದೆ | C# SDK ಸ್ವಯಂ-ಒಟ್ಟುಗೂಡಿಕೆಯ (CLI ಅಗತ್ಯವಿಲ್ಲ) |

> **ಮುಖ್ಯ ಅರ್ಥ:** SDK ಸಂಪೂರ್ಣ ಜೀವಚಕ್ರವನ್ನು ನಿರ್ವಹಿಸುತ್ತದೆ: ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸುವುದನ್ನು, ಕ್ಯಾಶೆ ಪರಿಶೀಲಿಸುವುದು, ಕಳವಳ ಇರುವ ಮಾದರಿಗಳನ್ನು ಡೌನ್ಲೋಡ್ ಮಾಡುವುದು, ಅವುಗಳನ್ನು ಲೋಡ್ ಮಾಡುವುದು, ಮತ್ತು ಎಂಡ್‌ಪಾಯಿಂಟ್ ಪತ್ತೆ ಮಾಡುವುದು, ಕೆಲವು ಪಂಕ್ತಿಗಳಲ್ಲಿ. ನಿಮ್ಮ ಅಪ್ಲಿಕೇಶನ್ CLI ಔಟ್‌ಪುಟ್ ಅನ್ನು ಪಾರ್ಸ್ ಮಾಡುವ ಅಗತ್ಯವಿಲ್ಲ ಅಥವಾ ಉಪಪ್ರಕ್ರಿಯೆಗಳನ್ನು ನಿರ್ವಹಿಸುವ ಅಗತ್ಯವಿಲ್ಲ.

---

## ಪ್ರಯೋಗಾಲಯ ವ್ಯಾಯಾಮಗಳು

### ವ್ಯಾಯಾಮ 1: SDK ಸ್ಥಾಪಿಸಿ

<details>
<summary><h3>🐍 ಪೈಥಾನ್</h3></summary>

```bash
pip install foundry-local-sdk
```

ಸ್ಥಾಪನೆಯನ್ನು ಪರಿಶೀಲಿಸಿ:

```python
from foundry_local import FoundryLocalManager
print("SDK installed successfully")
```

</details>

<details>
<summary><h3>📘 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್</h3></summary>

```bash
npm install foundry-local-sdk
```

ಸ್ಥಾಪನೆಯನ್ನು ಪರಿಶೀಲಿಸಿ:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

ಇಲ್ಲಿ ಎರಡು ನ್ಯೂಗಟ್ ಪ್ಯಾಕೇಜ್‌ಗಳಿವೆ:

| ಪ್ಯಾಕೇಜ್ | ವೇದಿಕೆ | ವಿವರಣೆ |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | ಕ್ರಾಸ್-ಪ್ಲಾಟ್‌ಫಾರ್ಮ್ | ವಿಂಡೋಸ್, ಲಿನಕ್ಸ, ಮ್ಯಾಕ್ಓಎಸ್ ಮೇಲೆ ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತದೆ |
| `Microsoft.AI.Foundry.Local.WinML` | ಕೇವಲ ವಿಂಡೋಸ್ | WinML ಹಾರ್ಡ್‌ವೇರ್ ವೇಗವರ್ಧನೆ ಸೇರಿಸುತ್ತದೆ; ಪ್ಲಗಿನ್ ಎಕ್ಸಿಕ್ಯೂಷನ್ ಪ್ರೊವೈಡರ್‌ಗಳನ್ನು ಡೌನ್ಲೋಡ್ ಮಾಡಿಸಿ ಮತ್ತು ಇನ್‌ಸ್ಟಾಲ್ ಮಾಡುತ್ತದೆ (ಉದಾ: Qualcomm NPU ಗಾಗಿ QNN) |

**ವಿಂಡೋಸ್ ಸೆಟಪ್:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` ಫೈಲ್ ಸಂಪಾದಿಸಿ:

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

> **ಟಿಪ್ಪಣಿ:** ವಿಂಡೋಸ್ ನಲ್ಲಿ WinML ಪ್ಯಾಕೇಜ್ ಮೂಲ SDK ಜೊತೆಗೆ QNN ಎಕ್ಸಿಕ್ಯೂಷನ್ ಪ್ರೊವೈಡರ್ ಸೇರಿಸಿಕೊಂಡಿರುವ супerset ಆಗಿದೆ. ಲಿನಕ್ಸ/ಮ್ಯಾಕ್ಓಎಸ್ ನಲ್ಲಿ ಮೂಲ SDK ಉಪಯೋಗಿಸಲಾಗುತ್ತದೆ. ಷರತ್ತುಗೊಂಡಿರುವ TFM ಮತ್ತು ಪ್ಯಾಕೇಜ್ ರೆಫರೆನ್ಸ್‌ಗಳು ಯೋಜನೆಯನ್ನು ಸಂಪೂರ್ಣವಾಗಿ ಕ್ರಾಸ್-ಪ್ಲಾಟ್‌ಫಾರ್ಮ್ ಆಗಿ ಇಟ್ಟುಕೊಳ್ಳುತ್ತವೆ.

ಯೋಜನೆಯ ಕೂಟದಲ್ಲಿ `nuget.config` ರಚಿಸಿ:

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

ಪ್ಯಾಕೇಜ್‌ಗಳನ್ನು ರಿಪೇರಿ ಮಾಡಿ:

```bash
dotnet restore
```

</details>

---

### ವ್ಯಾಯಾಮ 2: ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ ಮತ್ತು ಕಾಗಾಲಕ್ ಅನ್ನು ಪಟ್ಟಿ ಮಾಡಿ

ಯಾವುದೇ ಅಪ್ಲಿಕೇಶನ್ ಮೊದಲು Foundry Local ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ ಮತ್ತು ಲಭ್ಯವಿರುವ ಮಾದರಿಗಳನ್ನು ಪತ್ತೆಮಾಡುತ್ತದೆ.

<details>
<summary><h3>🐍 ಪೈಥಾನ್</h3></summary>

```python
from foundry_local import FoundryLocalManager

# ನಿರ್ವಾಹಕರನ್ನು ರಚಿಸಿ ಮತ್ತು ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ
manager = FoundryLocalManager()
manager.start_service()

# ಕ್ಯಾಟಲಾಗ್ನಲ್ಲಿ ಲಭ್ಯವಿರುವ ಎಲ್ಲಾ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### ಪೈಥಾನ್ SDK - ಸೇವಾ ನಿರ್ವಹಣೆಯ ವಿಧಾನಗಳು

| ವಿಧಾನ | ಸಹಿ | ವಿವರಣೆ |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | ಸೇವೆ ಓಡುತ್ತಿದ್ದೆಯೇ ಎಂದು ಪರಿಶೀಲಿಸಿ |
| `start_service()` | `() -> None` | Foundry Local ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ |
| `service_uri` | `@property -> str` | ಮೂಲ ಸೇವೆಯ URI |
| `endpoint` | `@property -> str` | API ಎಂಡ್ಪಾಯಿಂಟ್ (ಸೇವೆ URI + `/v1`) |
| `api_key` | `@property -> str` | API ಕೀ (ಪರಿಸರದಿಂದ ಅಥವಾ ಪೂರ್ವನಿಯೋಜಿತ ಪ್ಲೇಸ್‌ಹೋಲ್ಡರ್) |

#### ಪೈಥಾನ್ SDK - ಕಾಗಾಲಕ್ ನಿರ್ವಹಣೆಯ ವಿಧಾನಗಳು

| ವಿಧಾನ | ಸಹಿ | ವಿವರಣೆ |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | ಕಾಗಾಲಕ್‌ನಲ್ಲಿನ ಎಲ್ಲಾ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ |
| `refresh_catalog()` | `() -> None` | ಸೇವೆಯಿಂದ ಕಾಗಾಲಾಕ್ ರಿಫ್ರೆಶ್ ಮಾಡಿ |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | ನಿರ್ದಿಷ್ಟ ಮಾದರಿಗಾಗಿ ಮಾಹಿತಿ ಪಡೆಯಿರಿ |

</details>

<details>
<summary><h3>📘 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ವ್ಯವಸ್ಥಾಪಕನನ್ನು ರಚಿಸಿ ಮತ್ತು ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ಕ್ಯಾಟಲಾಗ್ ಅನ್ನು ಬ್ರೌಸ್ ಮಾಡಿ
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ SDK - ಮ್ಯಾನೇಜರ್ ವಿಧಾನಗಳು

| ವಿಧಾನ | ಸಹಿ | ವಿವರಣೆ |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK ಸಿಂಗಲ್ಟನ್ ಅನ್ನು ಪ್ರಾರಂಭಿಸಿ |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | ಸಿಂಗಲ್ಟನ್ ಮ್ಯಾನೇಜರ್ ಪ್ರವೇಶಿಸಿ |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local ವೆಬ್ ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ |
| `manager.urls` | `string[]` | ಸೇವೆಯ ಮೂಲ URLಗಳ ಪಟ್ಟಿ |

#### ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ SDK - ಕಾಗಾಲಕ್ ಮತ್ತು ಮಾದರಿ ವಿಧಾನಗಳು

| ವಿಧಾನ | ಸಹಿ | ವಿವರಣೆ |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | ಮಾದರಿ ಕಾಗಾಲಕ್ ಪ್ರವೇಶಿಸಿ |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` |_ALIAS ಮೂಲಕ ಮಾದರಿ ವಸ್ತುವನ್ನು ಪಡೆಯಿರಿ |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ ವಸ್ತುನಿಷ್ಠ ವಾಸ್ತುಶಿಲ್ಪವನ್ನು ಬಳಸುತ್ತದೆ - `Configuration`, `Catalog`, ಮತ್ತು `Model` ವಸ್ತುಗಳೊಂದಿಗೆ:

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

#### C# SDK - ಪ್ರಮುಖ ವರ್ಗಗಳು

| ವರ್ಗ | ಉದ್ದೇಶ |
|-------|---------|
| `Configuration` | ಅಪ್ಲಿಕೇಶನ್ ಹೆಸರು, ಲಾಗ್ ಮಟ್ಟ, ಕ್ಯಾಶೆ ಡೈರೆಕ್ಟರಿ, ವೆಬ್ ಸರ್ವರ್ URLಗಳನ್ನು ಸೆಟ್ ಮಾಡುವುದು |
| `FoundryLocalManager` | ಮುಖ್ಯ ಪ್ರವೇಶ ಬಿಂದು - `CreateAsync()` ಮೂಲಕ ಸೃಷ್ಟಿಸಲಾಗುತ್ತದೆ, `.Instance` ಮೂಲಕ ಪ್ರವೇಶ |
| `Catalog` | ಕಾಗಾಲಕ್‌ನಿಂದ ಮಾದರಿಗಳನ್ನು ತಿರುವು, ಹುಡುಕು, ಪಡೆಯಿರಿ |
| `Model` | ನಿರ್ದಿಷ್ಟ ಮಾದರಿಯನ್ನು ಪ್ರತಿನಿಧಿಸುತ್ತದೆ - ಡೌನ್ಲೋಡ್, ಲೋಡ್ ಮಾಡುವುದು, ಕ್ಲೈಂಟ್ ಗಳು ಪಡೆಯುವುದು |

#### C# SDK - ಮ್ಯಾನೇಜರ್ ಮತ್ತು ಕಾಗಾಲಕ್ ವಿಧಾನಗಳು

| ವಿಧಾನ | ವಿವರಣೆ |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | ಮ್ಯಾನೇಜರ್ ಪ್ರಾರಂಭಿಸಿ |
| `FoundryLocalManager.Instance` | ಸಿಂಗಲ್ಟನ್ ಮ್ಯಾನೇಜರ್ ಗೆ ಪ್ರವೇಶ |
| `manager.GetCatalogAsync()` | ಮಾದರಿ ಕಾಗಾಲಕ್ ಪಡೆಯಿರಿ |
| `catalog.ListModelsAsync()` | ಲಭ್ಯವಿರುವ ಎಲ್ಲಾ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ |
| `catalog.GetModelAsync(alias: "alias")` | ನಿರ್ದಿಷ್ಟ ಮಾದರಿ _ alias ಮೂಲಕ ಪಡೆಯಿರಿ |
| `catalog.GetCachedModelsAsync()` | ಡೌನ್ಲೋಡ್ ಮಾಡಲಾದ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ |
| `catalog.GetLoadedModelsAsync()` | ಪ್ರಸ್ತುತ ಲೋಡ್ ಮಾಡಿದ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ |

> **C# ವಾಸ್ತುಶಿಲ್ಪ ಟಿಪ್ಪಣಿ:** C# SDK v0.8.0+ ಮರು ವಿನ್ಯಾಸವು ಅಪ್ಲಿಕೇಶನ್ ಅನ್ನು **ಸ್ವಯಂ-ಒಟ್ಟುಗೂಡಿಕೆಯ** ಆಗಾಗಿಸುತ್ತದೆ; ಇದು ಅಂತಿಮ ಬಳಕೆದಾರ ಯಂತ್ರದಲ್ಲಿ Foundry Local CLI ಅಗತ್ಯವಿಲ್ಲ. SDK ಮಾದರಿ ನಿರ್ವಹಣೆಯನ್ನು ಮತ್ತು ಅಭಿಪ್ರಾಯವನ್ನು ನೇಟಿವ್ ರೀತಿಯಲ್ಲಿ ನಿರ್ವಹಿಸುತ್ತದೆ.

</details>

---

### ವ್ಯಾಯಾಮ 3: ಮಾದರಿಯನ್ನು ಡೌನ್ಲೋಡ್ ಮಾಡಿ ಮತ್ತು ಲೋಡ್ ಮಾಡಿ

SDK ಡೌನ್ಲೋಡ್ ಮಾಡುವುದು (ಡಿಸ್ಕ್‌ಗೆ) ಮತ್ತು ಲೋಡ್ ಮಾಡುವುದು (ಮೆಮೊರಿಯಲ್ಲಿ) ಅನ್ನು ವಿಭಜಿಸುತ್ತದೆ. ಇದರಿಂದ ನೀವು ಮೊದಲು ಮಾದರಿಗಳನ್ನು ಡೌನ್ಲೋಡ್ ಮಾಡಿ, ಅಗತ್ಯವಿದ್ದಾಗ ಲೋಡ್ ಮಾಡಬಹುದು.

<details>
<summary><h3>🐍 ಪೈಥಾನ್</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# ಆಯ್ಕೆ A: ಹಸ್ತಚಾಲಿತ ಸ್ಟೆಪ್‌ಬೈಸ್ಟೆಪ್
manager = FoundryLocalManager()
manager.start_service()

# ಮೊದಲು ಕ್ಯಾಶೆ ಪರಿಶೀಲಿಸಿ
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

# ಆಯ್ಕೆ B: ಒನ್-ಲೈನರ್ ಬೂಟ್‌ಸ್ಟ್ರಾಪ್ (ಶಿಫಾರಸು ಮಾಡಲಾಗಿದೆ)
# ನೀಗೊ alias ಕನ್‌ಸ್ಟ್ರಕ್ಟರ್‌ಗೆ ಪಾಸ್ ಮಾಡಿರಿ - ಅದು ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸುತ್ತದೆ, ಡೌನ್‌ಲೋಡ್ ಮಾಡುತ್ತದೆ ಮತ್ತು ಸ್ವಯಂಚಾಲಿತವಾಗಿ ಲೋಡ್ ಮಾಡುತ್ತದೆ
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### ಪೈಥಾನ್ - ಮಾದರಿ ನಿರ್ವಹಣೆಯ ವಿಧಾನಗಳು

| ವಿಧಾನ | ಸಹಿ | ವಿವರಣೆ |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | ಮಾದರಿಯನ್ನು ಸ್ಥಳೀಯ ಕ್ಯಾಶೆಗೆ ಡೌನ್ಲೋಡ್ ಮಾಡಿ |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | ಮಾದರಿಯನ್ನು ಇನ್ಫರೆನ್ಸ್ ಸರ್ವರ್‌ಗೆ ಲೋಡ್ ಮಾಡಿ |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | ಸರ್ವರ್‌ನಿಂದ ಮಾದರಿಯನ್ನು ಅನ್ಲೋಡ್ ಮಾಡಿ |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | ಪ್ರಸ್ತುತ ಲೋಡ್ ಮಾಡಿದ ಎಲ್ಲಾ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ |

#### ಪೈಥಾನ್ - ಕ್ಯಾಶೆ ನಿರ್ವಹಣೆಯ ವಿಧಾನಗಳು

| ವಿಧಾನ | ಸಹಿ | ವಿವರಣೆ |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | ಕ್ಯಾಶೆ ಡೈರೆಕ್ಟರಿಗೆ ಮಾರ್ಗ ಪಡೆಯಿರಿ |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | ಎಲ್ಲಾ ಡೌನ್ಲೋಡ್ ಮಾಡಿದ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ |

</details>

<details>
<summary><h3>📘 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// ಹಂತದ ಮೂಲಕ ದೃಷ್ಠಿಕೋಣ
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

#### ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ - ಮಾದರಿ ವಿಧಾನಗಳು

| ವಿಧಾನ | ಸಹಿ | ವಿವರಣೆ |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | ಮಾದರಿ ಈಗಾಗಲೇ ಡೌನ್ಲೋಡ್ ಆಗಿದೆಯೇ ಎಂಬುದು |
| `model.download()` | `() => Promise<void>` | ಮಾದರಿಯನ್ನು ಸ್ಥಳೀಯ ಕ್ಯಾಶೆಗೆ ಡೌನ್ಲೋಡ್ ಮಾಡಿ |
| `model.load()` | `() => Promise<void>` | ಇನ್ಫರೆನ್ಸ್ ಸರ್ವರ್‌ಗೆ ಲೋಡ್ ಮಾಡಿ |
| `model.unload()` | `() => Promise<void>` | ಇನ್ಫರೆನ್ಸ್ ಸರ್ವರ್‌ನಿಂದ ಅನ್ಲೋಡ್ ಮಾಡಿ |
| `model.id` | `string` | ಮಾದರಿಯ ವಿಶಿಷ್ಟ ಗುರುತು |

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

#### C# - ಮಾದರಿ ವಿಧಾನಗಳು

| ವಿಧಾನ | ವಿವರಣೆ |
|--------|-------------|
| `model.DownloadAsync(progress?)` | ಆಯ್ದ ವ್ಯತ್ಯಾಸವನ್ನು ಡೌನ್ಲೋಡ್ ಮಾಡಿ |
| `model.LoadAsync()` | ಮೆಮೊರಿಯಲ್ಲಿ ಮಾದರಿಯನ್ನು ಲೋಡ್ ಮಾಡಿ |
| `model.UnloadAsync()` | ಮಾದರಿಯನ್ನು ಅನ್ಲೋಡ್ ಮಾಡಿ |
| `model.SelectVariant(variant)` | ನಿರ್ದಿಷ್ಟ ವ್ಯತ್ಯಾಸ (CPU/GPU/NPU) ಆಯ್ಕೆಮಾಡಿ |
| `model.SelectedVariant` | ಪ್ರಸ್ತುತ ಆಯ್ಕೆಮಾಡಲಾದ ವ್ಯತ್ಯಾಸ |
| `model.Variants` | ಈ ಮಾದರಿಗಾಗಿ ಲಭ್ಯವಿರುವ ಎಲ್ಲಾ ವ್ಯತ್ಯಾಸಗಳು |
| `model.GetPathAsync()` | ಸ್ಥಳೀಯ ಫೈಲ್ ಮಾರ್ಗವನ್ನು ಪಡೆಯಿರಿ |
| `model.GetChatClientAsync()` | ಸ್ಥಳೀಯ ಚಾಟ್ ಕ್ಲೈಂಟ್ ಪಡೆಯಿರಿ (OpenAI SDK ಅಗತ್ಯವಿಲ್ಲ) |
| `model.GetAudioClientAsync()` | ಭಾಷಾಂತರಕ್ಕಾಗಿ ಆಡಿಯೋ ಕ್ಲೈಂಟ್ ಪಡೆಯಿರಿ |

</details>

---

### ವ್ಯಾಯಾಮ 4: ಮಾದರಿ ಮೆಟಾಡೇಟಾವನ್ನು ಪರಿಶೀಲಿಸಿ

`FoundryModelInfo` ವಸ್ತು ಪ್ರತಿಯೊಂದು ಮಾದರಿಗೂ ವಿಶಿಷ್ಟ ಮೆಟಾಡೇಟಾವನ್ನು ಹೊಂದಿದೆ. ಈ ಕ್ಷೇತ್ರಗಳನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳುವುದರಿಂದ ನಿಮ್ಮ ಅಪ್ಲಿಕೇಶನ್‌ಗಾಗಿ ಸರಿಯಾದ ಮಾದರಿಯನ್ನು ಆಯ್ಕೆಮಾಡಲು ಸಹಾಯವಾಗುತ್ತದೆ.

<details>
<summary><h3>🐍 ಪೈಥಾನ್</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ನಿರ್ದಿಷ್ಟ ಮಾದರಿಯ ಬಗ್ಗೆ ವಿವರವಾದ ಮಾಹಿತಿ ಪಡೆಯಿರಿ
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
<summary><h3>📘 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್</h3></summary>

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

#### FoundryModelInfo ಕ್ಷೇತ್ರಗಳು

| ಕ್ಷೇತ್ರ | ಪ್ರಕಾರ | ವಿವರಣೆ |
|-------|------|-------------|
| `alias` | string | ಸಂಕ್ಷಿಪ್ತ ಹೆಸರು (ಉದಾ. `phi-3.5-mini`) |
| `id` | string | ವಿಶಿಷ್ಟ ಮಾದರಿ ಗುರುತು |
| `version` | string | ಮಾದರಿ ಆವೃತ್ತಿ |
| `task` | string | `chat-completions` ಅಥವಾ `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, ಅಥವಾ NPU |
| `execution_provider` | string | ರನ್‌ಟೈಮ್ ಬ್ಯಾಕೆಂಡ್ (CUDA, CPU, QNN, WebGPU, ಇತ್ಯಾದಿ) |
| `file_size_mb` | int | ಡಿಸ್ಕ್‌ನ ಮೇಲೆ ಅಳತೆ MBಗಳಲ್ಲಿ |
| `supports_tool_calling` | bool | ಮಾದರಿ ಫಂಕ್ಷನ್/ಟೂಲ್ ಕರೆಯುವುದನ್ನು ಬೆಂಬಲಿಸುತ್ತದೆ ಎಂದರೆ |
| `publisher` | string | ಯಾರಾದರೂ ಮಾದರಿಯನ್ನು ಪ್ರಕಟಿಸಿದ್ದಾರೆ |
| `license` | string | ಪರವಾನಗಿ ಹೆಸರು |
| `uri` | string | ಮಾದರಿ URI |
| `prompt_template` | dict/null | ಪ್ರಾಂಪ್ಟ್ ಟೆಂಪ್ಲೇಟ್, ಇದ್ದರೆ |

---

### ವ್ಯಾಯಾಮ 5: ಮಾದರಿ ಜೀವಚಕ್ರವನ್ನು ನಿರ್ವಹಿಸಿ

ಸಂಪೂರ್ಣ ಜೀವಚಕ್ರವು ಅಭ್ಯಾಸ ಮಾಡಿ: ಪಟ್ಟಿ ಮಾಡಿ → ಡೌನ್ಲೋಡ್ ಮಾಡಿ → ಲೋಡ್ ಮಾಡಿ → ಬಳಸಿ → ಅನ್ಲೋಡ್ ಮಾಡಿ.

<details>
<summary><h3>🐍 ಪೈಥಾನ್</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # ತ್ವರಿತ ಪರೀಕ್ಷೆಗಾಗಿ ಸಣ್ಣ ಮಾದರಿ

manager = FoundryLocalManager()
manager.start_service()

# 1. ಕ್ಯಾಟಲോഗ್ ನಲ್ಲಿ ಏನು ಇದೆ ಎಂದು ಪರಿಶೀಲಿಸಿ
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. ಈಗಾಗಲೇ ಡೌನ್‌ಲೋಡ್ ಆಗಿರುವದು ಏನು ಎಂದು ಪರಿಶೀಲಿಸಿ
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. ಒಂದು ಮಾದರಿಯನ್ನು ডೌನ್‌ಲೋಡ್ ಮಾಡಿ
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. ಈಗ ಅದು ಕ್ಯಾಶ್‌ನಲ್ಲಿ ಇದೆ ಎಂದು ದೃಢಪಡಿಸಿ
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. ಅದನ್ನು ಲೋಡ್ ಮಾಡಿ
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. ಏನು ಲೋಡ್ ಆಗಿದೆ ಎಂದು ಪರಿಶೀಲಿಸಿ
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. ಅದನ್ನು ಅನ್‌ಲೋಡ್ ಮಾಡಿ
print(f"\nUnloading {alias}...")
manager.unload_model(alias)
loaded = manager.list_loaded_models()
print(f"Loaded models after unload: {len(loaded)}")
```

</details>

<details>
<summary><h3>📘 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "qwen2.5-0.5b"; // ವೆಗವಾಗಿ ಪರೀಕ್ಷಿಸಲು ಸಣ್ಣ ಮಾದರಿ

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. ಕ್ಯಾಟಲಾಗ್‌ನಿಂದ ಮಾದರಿಯನ್ನು ಪಡೆಯಿರಿ
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. ಅಗತ್ಯವಿದ್ದರೆ ಡೌನ್ಲೋಡ್ ಮಾಡಿ
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. ಅದನ್ನು ಲೋಡ್ ಮಾಡಿ
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. ಅದನ್ನು ಅನ್‌ಲೋಡ್ ಮಾಡಿ
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### ವ್ಯಾಯಾಮ 6: ಕ್ವಿಕ್-ಸ್ಟಾರ್ಟ್ ಮಾದರಿಗಳು

ಪ್ರತಿ ಭಾಷೆ ಒಂದು ಕರೆನಲ್ಲಿ ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಲು ಮತ್ತು ಮಾದರಿಯನ್ನು โหลด್ ಮಾಡಲು ಒಂದು ಶಾರ್ಟ್‌ಕಟ್ ನೀಡುತ್ತದೆ. ಇವು ಬಹುತೇಕ ಅನ್ವಯಿಕೆಗಳಿಗೆ **ಶಿಫಾರಸು ಮಾಡಲಾದ ಮಾದರಿಗಳು**.

<details>
<summary><h3>🐍 ಪೈಥಾನ್ - ಕಾಣ್ಸ್ಟ್ರಕ್ಟರ್ ಬುಟ್ಸ್ಟ್ರ್ಯಾಪ್</h3></summary>

```python
from foundry_local import FoundryLocalManager

# ನಿರ್ಮಾಪಕಕ್ಕೆ ಒಂದು ಉಪನಾಮವನ್ನು ನೀಡಿರಿ - ಅದು ಎಲ್ಲವನ್ನು ನಿರ್ವಹಿಸುತ್ತದೆ:
# 1. ಸೇವೆ ಚಾಲನೆಯಲ್ಲದಿದ್ದರೆ ಪ್ರಾರಂಭಿಸುತ್ತದೆ
# 2. ಮಾದರಿಯನ್ನು ಕ್ಯಾಶೆ ಆಗಿಲ್ಲದಿದ್ದರೆ ಡೌನ್‌ಲೋಡ್ ಮಾಡುತ್ತದೆ
# 3. ಮಾದರಿಯನ್ನು ಅನುಮಾನ ಸರ್ವರ್‌ಗೆ ಲೋಡ್ ಮಾಡುತ್ತದೆ
manager = FoundryLocalManager("phi-3.5-mini")

# ತಕ್ಷಣ ಬಳಸಲು ಸಿದ್ಧವಾಗಿದೆ
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` ಪ್ಯಾರಾಮೀಟರ್ (ಡಿಫಾಲ್ಟ್ `True`) ಈ ವರ್ತನೆಯನ್ನು ನಿಯಂತ್ರಿಸುತ್ತದೆ. ಕೈಗೊಳ್ಳುವ ನಿಯಂತ್ರಣ ಬೇಕಾದರೆ `bootstrap=False` ಎಂದು ಸೆಟ್ ಮಾಡಿ:

```python
# ಕೈಯರ್ ನಡುವೆ - ಯಾವುದೇ ಸ್ವಯಂಚಾಲಿತವಾಗಿ ಆಗುವುದಿಲ್ಲ
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ - `create()` + ಕ್ಯಾಟಲಾಗ್</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() ಎಲ್ಲವನ್ನೂ ನಿರ್ವಹಿಸುತ್ತದೆ:
// 1. ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸುತ್ತದೆ
// 2. ಕ್ಯಾಟಲೋಗ್ನಿಂದ ಮಾದರಿಯನ್ನು ಪಡೆಯುತ್ತದೆ
// 3. ಅಗತ್ಯವಿದ್ದಲ್ಲಿ ಡೌನ್‌ಲೋಡ್ ಮಾಡಿ ಮಾದರಿಯನ್ನು ಲೋಡ್ ಮಾಡುತ್ತದೆ
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// ತಕ್ಷಣ ಬಳಸಲು ಸಿದ್ಧವಾಗಿದೆ
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + ಕ್ಯಾಟಲಾಗ್</h3></summary>

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

> **C# ಗಮನಿಸಿ:** C# SDK ಅಪ್ಲಿಕೇಶನ್ ಹೆಸರು, ಲಾಗಿಂಗ್, ಕ್ಯಾಶೆ ಡೈರೆಕ್ಟರಿಗಳು ಮತ್ತು ಕ್ಕೆ ವೆಬ್ ಸರ್ವರ್ ಪೋರ್ಟ್ ಅನ್ನು `Configuration` ಮೂಲಕ ನಿಯಂತ್ರಿಸುತ್ತದೆ. ಇದು ಮೂರು SDK ಗಳಲ್ಲಿ ಅತ್ಯಂತ ಸಮನ್ವಯೋಪಯೋಗಿ ಮಾಡುತ್ತದೆ.

</details>

---

### ವ್ಯಾಯಾಮ 7: ನೆೇಟಿವ್ ChatClient (ಒಂದು OpenAI SDK ಅಗತ್ಯವಿಲ್ಲ)

ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C# SDKಗಳು `createChatClient()` ಆರಾಮದಾಯಕ ವಿಧಾನ ಒದಗಿಸುತ್ತವೆ, ಇದು ನೈಜ ಚಾಟ್ ಕ್ಲೈಂಟ್ ಒದಗಿಸುತ್ತದೆ — OpenAI SDK ಪ್ರತ್ಯೇಕವಾಗಿ ಸ್ಥಾಪನೆ ಅಥವಾ ಸಂರಚನೆ ಅವಶ್ಯಕತೆಯಾಗಿಲ್ಲ.

<details>
<summary><h3>📘 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ - <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// ಮಾದರಿ ನೇರವಾಗಿ ChatClient ರಚಿಸಿ — ಯಾವುದೇ OpenAI आयಾತ आवश्यकವಿಲ್ಲ
const chatClient = model.createChatClient();

// completeChat ಒಂದು OpenAI- ಅನುಕೂಲಕರ ಪ್ರತಿಕ್ರಿಯೆ ವಸ್ತು ನೀಡುತ್ತದೆ
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// ಸ್ಟ್ರೀಮಿಂಗ್ ಕ್ಯಾಲ್ಬ್ಯಾಕ್ ಮಾದರಿಯನ್ನು ಬಳಸುತ್ತದೆ
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` ಉಪಕರಣ ಕರೆಗೂ ಬೆಂಬಲ ನೀಡುತ್ತದೆ — ಉಪಕರಣಗಳನ್ನು ಎರಡನೇ ಆರ್ಗ್ಯೂಮೆಂಟ್ ಆಗಿ ಪಾಸ್ ಮಾಡಿ:

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

> **ಏವತ್ತು ಯಾವ ಮಾದರಿ ಅವಲಂಬಿಸಬೇಕು:**
> - **`createChatClient()`** — ವೇಗದ ಪ್ರೋಟೋಟೈಪಿಂಗ್, ಕಡಿಮೆ ಅವಲಂಬನೆಗಳು, ಸರಳ ಕೋಡ್
> - **OpenAI SDK** — ಪ್ಯಾರಾಮೀಟರುಗಳ (ತಾಪಮಾನ, top_p, ಸ್ಟಾಪ್ ಟೋಕನ್ಗಳು ಇತ್ಯಾದಿ) ಸಂಪೂರ್ಣ ನಿಯಂತ್ರಣ; ಉತ್ಪಾದನಾ ಅನ್ವಯಿಕೆಗಳಿಗೆ ಉತ್ತಮ

---

### ವ್ಯಾಯಾಮ 8: ಮಾದರಿ ಪ್ರಕಾರಗಳು ಮತ್ತು ಹಾರ್ಡ್ವೇರ್ ಆಯ್ಕೆ

ಮಾದರಿಗಳಿಗೆ ಭಿನ್ನ ಹಾರ್ದ್ವೇರ್‌ಗೆ ಅಪ್ಟಿಮೈಸ್ ಮಾಡಿದ ಬಹು **ಪ್ರಕಾರಗಳು** ಇರಬಹುದು. SDK ಸ್ವಯಂಚಾಲಿತವಾಗಿ ಅತ್ಯುತ್ತಮ ಪ್ರಕಾರವನ್ನು ಆಯ್ಕೆಮಾಡುತ್ತದೆ, ಆದರೆ ನೀವು ಕ್ರಮವಾಗಿ ಪರಿಗಣಿಸಬಹುದು ಮತ್ತು ಆಯ್ಕೆ ಮಾಡಬಹುದು.

<details>
<summary><h3>📘 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// ಎಲ್ಲಾ ಲಭ್ಯವಿರುವ ವೈವಿಧ್ಯಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// ನಿಮ್ಮ ಹಾರ್ಡ್‌ವೇರ್‌ಗಾಗಿ SDK ಸ್ವಯಂಚಾಲಿತವಾಗಿ ಉತ್ತಮ ವೈವಿಧ್ಯವನ್ನು ಆಯ್ಕೆಮಾಡುತ್ತದೆ
// ಮೀರಿಸುವುದಕ್ಕೆ, selectVariant() ಬಳಸಿ:
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
<summary><h3>🐍 ಪೈಥಾನ್</h3></summary>

ಪೈಥಾನ್ ನಲ್ಲಿ, SDK ಸ್ವಯಂಚಾಲಿತವಾಗಿ ಹಾರ್ಡ್ವೇರ್ ಆಧಾರಿತ ಅತ್ಯುತ್ತಮ ಪ್ರಕಾರವನ್ನು ಆಯ್ಕೆ ಮಾಡುತ್ತದೆ. `get_model_info()` ಪಾವತಿಸಿ ಆಯ್ಕೆಮಾಡಿದದ್ದು ಏನೆಂದು ನೋಡಿ:

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

#### NPU ಪ್ರಕಾರಗಳೊಂದಿಗೆ ಮಾದರಿಗಳು

ಕೆಲವು ಮಾದರಿಗಳಿಗೆ ನ್ಯೂರಲ್ ಪ್ರೊಸೆಸಿಂಗ್ ಯೂನಿಟ್‌ಗಳಿಗೆ (Qualcomm Snapdragon, Intel Core Ultra) NPU-ಅಪ್ಟಿಮೈಸ್ ಮಾಡಿದ ಪ್ರಕಾರಗಳು ಇವೆ:

| ಮಾದರಿ | NPU ಪ್ರಕಾರ ಲಭ್ಯವಿದೆ |
|-------|:-------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **ಸಲಹೆ:** NPU-ಸಾಮರ್ಥ್ಯಯುತ ಹಾರ್ಡ್ವೇರ್‌ನಲ್ಲಿ, SDK ಲಭ್ಯವಿದ್ದರೆ ಸ್ವಯಂಚಾಲಿತವಾಗಿ NPU ಪ್ರಕಾರವನ್ನು ಆಯ್ಕೆ ಮಾಡುತ್ತದೆ. ನಿಮ್ಮ ಕೋಡ್ ಅನ್ನು ಬದಲಾಯಿಸುವ ಅಗತ್ಯವಿಲ್ಲ. ವಿಂಡೋಸ್‌ನಲ್ಲಿ C# ಯೋಜನೆಗಳಿಗೆ, QNN ಕಾರ್ಯಗತಗೊಳಿಸುವ ಮೂಲಕ `Microsoft.AI.Foundry.Local.WinML` NuGet ಪ್ಯಾಕೇಜ್ ಸೇರಿಸಿ — QNN ಅನ್ನು WinML ಮೂಲಕ ಪ್ಲಗಿನ್ EP ಆಗಿ ವಿತರಿಸಲಾಗುತ್ತದೆ.

---

### ವ್ಯಾಯಾಮ 9: ಮಾದರಿ ನವೀಕರಣಗಳು ಮತ್ತು ಕ್ಯಾಟಲಾಗ್ ರಿಫ್ರೆಶ್

ಮಾದರಿ ಕ್ಯಾಟಲೋಗ್ ಕಾಲಕಾಲಕ್ಕೆ ನವೀಕರಿಸಲಾಗುತ್ತದೆ. ನವೀಕರಣಗಳನ್ನು ಪರಿಶೀಲಿಸಲು ಮತ್ತು ಅನ್ವಯಿಸಲು ಈ ವಿಧಾನಗಳನ್ನು ಬಳಸಿರಿ.

<details>
<summary><h3>🐍 ಪೈಥಾನ್</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# ಇತ್ತೀಚಿನ ಮಾದರಿ ಪಟ್ಟಿಯನ್ನು ಪಡೆಯಲು ಕ್ಯಾಟಲಾಗ್ ಅನ್ನು ರಿಫ್ರೆಶ್ ಮಾಡಿ
manager.refresh_catalog()

# ಕ್ಯಾಶೆ ಮಾಡಲಾದ ಮಾದರಿಯ ಹೊಸ ಆವೃತ್ತಿ ಲಭ್ಯವಿದೆಯೇ ಎಂದು ಪರಿಶೀಲಿಸಿ
if manager.is_model_upgradeable(alias):
    print(f"{alias} has a newer version available!")
    manager.upgrade_model(alias)
    print("Upgrade complete")
else:
    print(f"{alias} is up to date")
```

</details>

<details>
<summary><h3>📘 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "UpgradeDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ಇತ್ತೀಚಿನ ಮಾದರಿ ಸೇರ್ಪಡೆ ಪಡೆಯಲು ಜಾಹೀರಾತು ತಾಜಾತನಗೊಳಿಸಿ
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// ತಾಜಾತನಗೊಂಡ ನಂತರ ಎಲ್ಲಾ ಲಭ್ಯವಿರುವ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡು
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### ವ್ಯಾಯಾಮ 10: ಪರಿಗಣನೆ ಮಾಡುವ ಮಾದರಿಗಳ ಬಳಕೆ

**phi-4-mini-reasoning** ಮಾದರಿ ಚೈನ್-ಆಫ್-ಥಾಟ್ (ಚಿಂತನ ಸರಣಿ) ಪರಿಗಣನೆಯನ್ನು ಒಳಗೊಂಡಿದೆ. ಇದು ಆಡಿಯೋ ತಯಾರಿಸುವ ಮುಂಚೆ `<think>...</think>` ಟ್ಯಾಗ್‌ಗಳಲ್ಲಿ ಅದರ ಆಂತರಿಕ ಚಿಂತನೆಗಳನ್ನು ಜೋಡಿಸುತ್ತದೆ. ಇದು ಬಹು ಹಂತದ ತಾರ್ಕಿಕ, ಗಣಿತ ಅಥವಾ ಸಮಸ್ಯೆ ಪರಿಹಾರ ಕಾರ್ಯಗಳಿಗೆ ಉಪಯುಕ್ತ.

<details>
<summary><h3>🐍 ಪೈಥಾನ್</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning ಸುಮಾರು 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# ಮಾದರಿ ಅದರ ಚಿಂತನೆಯನ್ನು <think>...</think> ಟ್ಯಾಗ್‌ಗಳಲ್ಲಿ ಮುಚ್ಚುತ್ತದೆ
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
<summary><h3>📘 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್</h3></summary>

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

// ಚೈನ್ಅಫ್‌ಥಾಟ್ ಆಲೋಚನೆಯನ್ನು ವಿಡಾಯಿಸಿ
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **ಯಾವಾಗ ಪರಿಗಣನೆ ಮಾಡುವ ಮಾದರಿಗಳನ್ನು ಬಳಸಬೇಕು:**
> - ಗಣಿತ ಮತ್ತು ಲಾಜಿಕ್ ಸಮಸ್ಯೆಗಳು
> - ಪলಿಶ್ ಹಂತ ಯೋಜನೆ ಕಾರ್ಯಗಳು
> - ಸಂಕೀರ್ಣ ಕೋಡ್ ಸೃಷ್ಟಿ
> - ಕೆಲಸ ತೋರಿಸುವಿಕೆ ಖಚಿತತೆಯನ್ನು ಸುಧಾರಿಸುತ್ತದೆ
>
> **ವ್ಯತ್ಯಾಸ:** ಪರಿಗಣನೆ ಮಾದರಿಗಳು ಹೆಚ್ಚು ಟೋಕನ್ಗಳನ್ನು (ಅದೇ `<think>` ವಿಭಾಗ) ಉತ್ಪಾದಿಸುತ್ತವೆ ಮತ್ತು ನಿಧಾನಾಗಿರುತ್ತವೆ. ಸರಳ ಪ್ರಶ್ನೋತ್ತರಕ್ಕಾಗಿ, phi-3.5-mini ಮಾದರಿ ವೇಗವಾಗಿದೆ.

---

### ವ್ಯಾಯಾಮ 11: ಬದಲಾವಣೆಗಳ ಅರ್ಥಮಾಡಿಕೊಳ್ಳಿ ಮತ್ತು ಹಾರ್ಡ್ವೇರ್ ಆಯ್ಕೆ

ನೀವು ಪೂರ್ಣ ಮಾದರಿ ID ಬದಲು **ಅಲಿಯಾಸ್** (ಹೀಗೆ `phi-3.5-mini` ) ಒದಗಿಸಿದಾಗ SDK ಸ್ವಯಂಚಾಲಿತವಾಗಿ ನಿಮ್ಮ ಹಾರ್ಡ್ವೇರ್‌ಗೆ ಅತ್ಯುತ್ತಮ ಪ್ರಕಾರವನ್ನು ಆಯ್ಕೆ ಮಾಡುತ್ತದೆ:

| ಹಾರ್ಡ್ವೇರ್ | ಆಯ್ಕೆ ಮಾಡಲಾದ ಕಾರ್ಯಗತಗೊಳಿಸುವ ಒದಗಣೆದಾರ |
|------------|-------------------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML ಪ್ಲಗಿನ್ ಮೂಲಕ) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| ಯಾವುದೇ ಸಾಧನ (ಫಾಲ್ ಬ್ಯಾಕ್) | `CPUExecutionProvider` ಅಥವಾ `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ಆಲಿಯಾಸ್ ನಿಮ್ಮ ಹಾರ್ಡ್‌ವೇರ್‌ಗೆ ಉತ್ತಮಂವಾದ ರೂಪಾಂತರವನ್ನು ಪರಿಹರಿಸುತ್ತದೆ
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **ಸಲಹೆ:** ಯಾವಾಗಲೂ ನಿಮ್ಮ ಅಪ್ಲಿಕೇಶನ್ ಕೋಡ್‌ನಲ್ಲಿ ಅಲಿಯಾಸ್‌ಗಳನ್ನು ಬಳಸಿರಿ. ಬಳಕೆದಾರರ ಯಂತ್ರದಲ್ಲಿ ನಿಯೋಜಿಸಿದಾಗ, SDK ರನ್‌ಟೈಮ್‌ನಲ್ಲಿ ಅತ್ಯುತ್ತಮ ಪ್ರಕಾರವನ್ನು ಆಯ್ಕೆಮಾಡುತ್ತದೆ - NVIDIA ಗಾಗಿ CUDA, Qualcomm ಗಾಗಿ QNN, ಉಳಿದವುಗಳಿಗೆ CPU.

---

### ವ್ಯಾಯಾಮ 12: C# ಸಂರಚನಾ ಆಯ್ಕೆಗಳು

C# SDK ಯ `Configuration` ಕ್ಲಾಸ್ ರನ್‌ಟೈಮ್ ಮೇಲೆ ಸೂಕ್ಷ್ಮ ನಿಯಂತ್ರಣ ಒದಗಿಸುತ್ತದೆ:

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

| ಗುಣಲಕ್ಷಣ | ಡಿಫಾಲ್ಟ್ | ವಿವರಣೆ |
|-----------|----------|---------|
| `AppName` | (ಅಗತ್ಯ) | ನಿಮ್ಮ ಅಪ್ಲಿಕೇಶನ್ ಹೆಸರು |
| `LogLevel` | `Information` | ಲಾಗಿಂಗ್ ವಿವರತೆ |
| `Web.Urls` | (ಡೈನಾಮಿಕ್) | ವೆಬ್ ಸರ್ವರ್‌ಗಾಗಿ ನಿರ್ದಿಷ್ಟ ಪೋರ್ಟ್ ನಿಗದಿ ಮಾಡು |
| `AppDataDir` | OS ಡಿಫಾಲ್ಟ್ | ಅಪ್ಲಿಕೇಶನ್ ಡಾಟಾ ಮೂಲ ಡೈರೆಕ್ಟರಿ |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | ಮಾದರಿಗಳ ಸಂಗ್ರಹಣಾ ಸ್ಥಳ |
| `LogsDir` | `{AppDataDir}/logs` | ಲಾಗ್‌ಗಳು ಬರೆಯುವ ಸ್ಥಳ |

---

### ವ್ಯಾಯಾಮ 13: ಬ್ರೌಸರ್ ಬಳಕೆ (ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮಾತ್ರ)

ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ SDK ಬ್ರೌಸರ್-ಸಮರ್ಥಿತ ಆವೃತ್ತಿಯನ್ನು ಒಳಗೊಂಡಿದೆ. ಬ್ರೌಸರ್‌ನಲ್ಲಿ ನೀವು ಕೈಯಿಂದ CLI ಮೂಲಕ ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಬೇಕು ಮತ್ತು ಹೋಸ್ಟ್ URL ಅನ್ನು ಸೂಚಿಸಬೇಕು:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// ಮೊದಲು ಸೇವೆಯನ್ನು ಕೈಯಿಂದ ಪ್ರಾರಂಭಿಸಿ:
//   foundry ಸೇವೆ ಪ್ರಾರಂಭಿಸಿ
// ನಂತರ CLI ಔಟ್‌ಪುಟ್‌ನಿಂದ URL ಬಳಸಿ
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// ಕ್ಯಾಟಲೋಗ್ ಬ್ರೌಸ್ ಮಾಡಿ
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **ಬ್ರೌಸರ್ ಮಿತಿಗಳು:** ಬ್ರೌಸರ್ ಆವೃತ್ತಿ `startWebService()`ನ್ನು ಬೆಂಬಲಿಸದು. SDK ಬಳಕೆಮಾಡುವ ಮೊದಲು Foundry Local ಸೇವೆ ಈಗಾಗಲೆ ಚಾಲನೆಯಲ್ಲಿರುವುದನ್ನು ಖಚಿತಪಡಿಸಿಕೊಳ್ಳಬೇಕು.

---

## ಪೂರ್ಣ API ಉಲ್ಲೇಖ

### ಪೈಥಾನ್

| ವರ್ಗ | ವಿಧಾನ | ವಿವರಣೆ |
|--------|--------|-------------|
| **ಪ್ರಾರಂಭ** | `FoundryLocalManager(alias?, bootstrap=True)` | ಮ್ಯಾನೇಜರ್ ರಚಿಸಿ; ಐಚ್ಛಿಕವಾಗಿ ಮಾದರಿಯೊಂದಿಗೆ ಬುಟ್ಸ್ಟ್ರ್ಯಾಪ್ ಮಾಡಿ |
| **ಸೇವೆಯು** | `is_service_running()` | ಸೇವೆ ಚಾಲನೆಯಲ್ಲಿದೆಯೇ ಪರಿಶೀಲಿಸಿ |
| **ಸೇವೆಯು** | `start_service()` | ಸೇವೆ ಪ್ರಾರಂಭಿಸಿ |
| **ಸೇವೆಯು** | `endpoint` | API ಎಂಡ್ಪಾಯಿಂಟ್ URL |
| **ಸೇವೆಯು** | `api_key` | API ಕೀ |
| **ಕ್ಯಾಟಲಾಗ್** | `list_catalog_models()` | ಲಭ್ಯವಿರುವ ಎಲ್ಲ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `refresh_catalog()` | ಕ್ಯಾಟಲೋಗ್ ರಿಫ್ರೆಶ್ ಮಾಡಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `get_model_info(alias_or_model_id)` | ಮಾದರಿ ಮೆಟಾಡೇಟಾ ಪಡೆಯಿರಿ |
| **ಕ್ಯಾಶೆ** | `get_cache_location()` | ಕ್ಯಾಶೆ ಡೈರೆಕ್ಟರಿ ಪಥ |
| **ಕ್ಯಾಶೆ** | `list_cached_models()` | ಡೌನ್ಲೋಡ್ ಮಾಡಿದ ಮಾದರಿಗಳ ಪಟ್ಟಿ |
| **ಮಾದರಿ** | `download_model(alias_or_model_id)` | ಮಾದರಿ ಡೌನ್ಲೋಡ್ ಮಾಡಿ |
| **ಮಾದರಿ** | `load_model(alias_or_model_id, ttl=600)` | ಮಾದರಿ ಲೋಡ್ ಮಾಡಿ |
| **ಮಾದರಿ** | `unload_model(alias_or_model_id)` | ಮಾದರಿ ಅನ್ಲೋಡ್ ಮಾಡಿ |
| **ಮಾದರಿ** | `list_loaded_models()` | ಲೋಡ್ ಆಗಿರುವ ಮಾದರಿಗಳ ಪಟ್ಟಿ |
| **ಮಾದರಿ** | `is_model_upgradeable(alias_or_model_id)` | ಹೊಸ ಆವೃತ್ತಿ ಲಭ್ಯವಿದೆಯೇ ಎಂದು ಪರಿಶೀಲಿಸಿ |
| **ಮಾದರಿ** | `upgrade_model(alias_or_model_id)` | ಮಾದರಿಯನ್ನು ನವೀಕರಿಸಿ |
| **ಸೇವೆಯು** | `httpx_client` | ನೇರ API ಕರೆಗಳಿಗಾಗಿ ಪೂರ್ವ ಸಂರಚಿತ HTTPX ಕ್ಲೈಂಟ್ |

### ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್

| ವರ್ಗ | ವಿಧಾನ | ವಿವರಣೆ |
|--------|--------|-------------|
| **ಪ್ರಾರಂಭ** | `FoundryLocalManager.create(options)` | SDK ಸಿಂಗಲ್ಟನ್ ಶುರುಮಾಡಿ |
| **ಪ್ರಾರಂಭ** | `FoundryLocalManager.instance` | ಸಿಂಗಲ್ಟನ್ ಮ್ಯಾನೇಜರ್ ಪ್ರವೇಶಿಸಿ |
| **ಸೇವೆಯು** | `manager.startWebService()` | ವೆಬ್ ಸೇವೆ ಪ್ರಾರಂಭಿಸಿ |
| **ಸೇವೆಯು** | `manager.urls` | ಸೇವೆಗಾಗಿ ಮೂಲ URL ಗಳು ಸರಣಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `manager.catalog` | ಮಾದರಿ ಕ್ಯಾಟಲೋಗ್ ಪ್ರವೇಶಿಸಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.getModel(alias)` | ಅಲಿಯಾಸ್ ಮೂಲಕ ಮಾದರಿ ವಸ್ತುವನ್ನು ಪಡೆದಿರಿ (ಪ್ರಾಮಿಸ್ 반환) |
| **ಮಾದರಿ** | `model.isCached` | ಮಾದರಿ ಡೌನ್ಲೋಡ್ ಆಗಿದೆಯೇ ಎಂದು ಸೂಚಿಸು |
| **ಮಾದರಿ** | `model.download()` | ಮಾದರಿ ಡೌನ್ಲೋಡ್ ಮಾಡಿ |
| **ಮಾದರಿ** | `model.load()` | ಮಾದರಿ ಲೋಡ್ ಮಾಡಿ |
| **ಮಾದರಿ** | `model.unload()` | ಮಾದರಿ ಅನ್ಲೋಡ್ ಮಾಡಿ |
| **ಮಾದರಿ** | `model.id` | ಮಾದರಿಯ ವಿಶಿಷ್ಟ ಗುರುತು |
| **ಮಾದರಿ** | `model.alias` | ಮಾದರಿಯ ಅಲಿಯಾಸ್ |
| **ಮಾದರಿ** | `model.createChatClient()` | ಸ್ಥಳೀಯ ಚಾಟ್ ಕ್ಲೈಂಟ್ ಪಡೆಯಿರಿ (OpenAI SDK ಅಗತ್ಯವಿಲ್ಲ) |
| **ಮಾದರಿ** | `model.createAudioClient()` | ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಗೆ ಆಡಿಯೋ ಕ್ಲೈಂಟ್ ಪಡೆಯಿರಿ |
| **ಮಾದರಿ** | `model.removeFromCache()` | ಸ್ಥಳೀಯ ಕ್ಯಾಶೆಯಿಂದ ಮಾದರಿಯನ್ನು ತೆಗೆಯಿರಿ |
| **ಮಾದರಿ** | `model.selectVariant(variant)` | ನಿರ್ದಿಷ್ಟ ಹಾರ್ಡ್ವೇರ್ ಪ್ರಕಾರ ಆಯ್ಕೆಮಾಡಿ |
| **ಮಾದರಿ** | `model.variants` | ಈ ಮಾದರಿಯ ಲಭ್ಯವಿರುವ ಪ್ರಕಾರಗಳ ಸರಣಿ |
| **ಮಾದರಿ** | `model.isLoaded()` | ಮಾದರಿ ప్రస్తుతం ಲೋಡ್ ಆಗಿದೆಯೇ ಎಂದು ಪರಿಶೀಲಿಸಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.getModels()` | ಲಭ್ಯವಿರುವ ಎಲ್ಲಾ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.getCachedModels()` | ಡೌನ್ಲೋಡ್ ಮಾಡಿದ ಮಾದರಿಗಳ ಪಟ್ಟಿ ಮಾಡಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.getLoadedModels()` | ಪ್ರಸ್ತುತ ಲೋಡ್ ಆಗಿರುವ ಮಾದರಿಗಳ ಪಟ್ಟಿ ಮಾಡಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.updateModels()` | ಸೇವೆಯಿಂದ ಕ್ಯಾಟಲೋಗ್ ರಿಫ್ರೆಶ್ ಮಾಡಿ |
| **ಸೇವೆಯು** | `manager.stopWebService()` | Foundry Local ವೆಬ್ ಸೇವೆ ನಿಲ್ಲಿಸಿ |

### C# (v0.8.0+)

| ವರ್ಗ | ವಿಧಾನ | ವಿವರಣೆ |
|--------|--------|-------------|
| **ಪ್ರಾರಂಭ** | `FoundryLocalManager.CreateAsync(config, logger)` | ಮ್ಯಾನೇಜರ್ ಆರಂಭಿಸಿ |
| **ಪ್ರಾರಂಭ** | `FoundryLocalManager.Instance` | ಸಿಂಗಲ್ಟನ್ ಪ್ರವೇಶಿಸಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `manager.GetCatalogAsync()` | ಕ್ಯಾಟಲಾಗ್ ಪಡೆಯಿರಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.ListModelsAsync()` | ಎಲ್ಲ ಮಾದರಿಗಳ ಪಟ್ಟಿ ಮಾಡಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.GetModelAsync(alias)` | ನಿರ್ದಿಷ್ಟ ಮಾದರಿ ಪಡೆಯಿರಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.GetCachedModelsAsync()` | ಕ್ಯಾಶೆ ಮಾಡಲಾದ ಮಾದರಿಗಳ ಪಟ್ಟಿ ಮಾಡಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.GetLoadedModelsAsync()` | ಲೋಡ್ ಆದ ಮಾದರಿಗಳ ಪಟ್ಟಿ ಮಾಡಿ |
| **ಮಾದರಿ** | `model.DownloadAsync(progress?)` | ಮಾದರಿ ಡೌನ್ಲೋಡ್ ಮಾಡಿ |
| **ಮಾದರಿ** | `model.LoadAsync()` | ಮಾದರಿ ಲೋಡ್ ಮಾಡಿ |
| **ಮಾದರಿ** | `model.UnloadAsync()` | ಮಾದರಿ ಅನ್ಲೋಡ್ ಮಾಡಿ |
| **ಮಾದರಿ** | `model.SelectVariant(variant)` | ಹಾರ್ಡ್ವೇರ್ ಪ್ರಕಾರ ಆಯ್ಕೆಮಾಡಿ |
| **ಮಾದರಿ** | `model.GetChatClientAsync()` | ಸ್ಥಳೀಯ ಚಾಟ್ ಕ್ಲೈಂಟ್ ಪಡೆಯಿರಿ |
| **ಮಾದರಿ** | `model.GetAudioClientAsync()` | ಆಡಿಯೋ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಕ್ಲೈಂಟ್ ಪಡೆಯಿರಿ |
| **ಮಾದರಿ** | `model.GetPathAsync()` | ಸ್ಥಳೀಯ ಫೈಲ್ ಪಥ ಪಡೆಯಿರಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.GetModelVariantAsync(alias, variant)` | ನಿರ್ದಿಷ್ಟ ಹಾರ್ಡ್ವೇರ್ ಪ್ರಕಾರ ಪಡೆಯಿರಿ |
| **ಕ್ಯಾಟಲಾಗ್** | `catalog.UpdateModelsAsync()` | ಕ್ಯಾಟಲಾಗ್ ನವೀಕರಿಸಿ |
| **ಸರ್ವರ್** | `manager.StartWebServerAsync()` | REST ವೆಬ್ ಸರ್ವರ್ ಪ್ರಾರಂಭಿಸಿ |
| **ಸರ್ವರ್** | `manager.StopWebServerAsync()` | REST ವೆಬ್ ಸರ್ವರ್ ನಿಲ್ಲಿಸಿ |
| **ಕಾನ್ಫಿಗ್** | `config.ModelCacheDir` | ಕ್ಯಾಶೆ ಡೈರೆಕ್ಟರಿ |

---

## ಪ್ರಮುಖ ಧ್ಯಾನಾರ್ಹಾಂಶಗಳು

| ಸಂकल्पನೆ | ನೀವು ಕಲಿತದ್ದು |
|-----------|-----------------|
| **SDK ಮತ್ತು CLI** | SDK ಕಾರ್ಯಕ್ರಮಾತ್ಮಕ ನಿಯಂತ್ರಣ ಒದಗಿಸುತ್ತದೆ - ಅನ್ವಯಿಕೆಗಳಿಗೆ ಅವಶ್ಯಕ |
| **ನಿಯಂತ್ರಣ ಪ್ಲೇನ್** | SDK ಸೇವೆಗಳು, ಮಾದರಿಗಳು ಮತ್ತು ಕ್ಯಾಶಿಂಗ್ ಅನ್ನು ನಿರ್ವಹಿಸುತ್ತದೆ |
| **ಡೈನಾಮಿಕ್ ಪೋರ್ಟ್‌ಗಳು** | ಯಾವಾಗಲೂ `manager.endpoint` (ಪೈಥಾನ್) ಅಥವಾ `manager.urls[0]` (JS/C#) ಅನ್ನು ಬಳಸಿರಿ - ಹಾರ್ಡ್‌ಕೋಡ್ ಪೋರ್ಟ್ ನಿರ್ಬಂಧಿಸುವುದಿಲ್ಲ |
| **ಅಲಿಯಾಸ್‌ಗಳು** | ಸ್ವಯಂಚಾಲಿತ ಹಾರ್ಡ್ವೇರ್-ಉತ್ತಮ ಮಾದರಿ ಆಯ್ಕೆಗಾಗಿ ಅಲಿಯಾಸ್‌ಗಳನ್ನು ಬಳಸಿ |
| **ತ್ವರಿತ ಪ್ರಾರಂಭ** | ಪೈಥಾನ್: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# ಮರು<|vq_lbr_audio_112051|><|vq_lbr_audio_46711|><|vq_lbr_audio_18735|><|vq_lbr_audio_24366|><|vq_lbr_audio_4158|><|vq_lbr_audio_84791|><|vq_lbr_audio_58759|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_114096|><|vq_lbr_audio_61785|><|vq_lbr_audio_61785|><|vq_lbr_audio_114096|><|vq_lbr_audio_54087|><|vq_lbr_audio_62203|><|vq_lbr_audio_40825|><|vq_lbr_audio_62462|><|vq_lbr_audio_74360|><|vq_lbr_audio_34577|><|vq_lbr_audio_86954|><|vq_lbr_audio_124132|><|vq_lbr_audio_2366|><|vq_lbr_audio_72136|><|vq_lbr_audio_70882|><|vq_lbr_audio_82304|><|vq_lbr_audio_39802|><|vq_lbr_audio_53778|><|vq_lbr_audio_106267|><|vq_lbr_audio_129039|><|vq_lbr_audio_12995|><|vq_lbr_audio_57450|><|vq_lbr_audio_130764|><|vq_lbr_audio_127618|><|vq_lbr_audio_20816|><|vq_lbr_audio_129632|><|vq_lbr_audio_8562|><|vq_lbr_audio_1423|><|vq_lbr_audio_31477|><|vq_lbr_audio_12122|><|vq_lbr_audio_64219|><|vq_lbr_audio_11890|><|vq_lbr_audio_56287|><|vq_lbr_audio_44813|><|vq_lbr_audio_72759|><|vq_lbr_audio_48275|><|vq_lbr_audio_24649|><|vq_lbr_audio_53379|><|vq_lbr_audio_38116|><|vq_lbr_audio_25398|><|vq_lbr_audio_49638|><|vq_lbr_audio_84749|><|vq_lbr_audio_24169|><|vq_lbr_audio_122432|><|vq_lbr_audio_56315|><|vq_lbr_audio_72590|><|vq_lbr_audio_125567|><|vq_lbr_audio_110695|><|vq_lbr_audio_70792|><|vq_lbr_audio_62884|><|vq_lbr_audio_14878|><|vq_lbr_audio_58968|><|vq_lbr_audio_48759|><|vq_lbr_audio_33449|><|vq_lbr_audio_59858|><|vq_lbr_audio_47069|><|vq_lbr_audio_75643|><|vq_lbr_audio_118190|><|vq_lbr_audio_122616|><|vq_lbr_audio_64863|><|vq_lbr_audio_42834|><|vq_lbr_audio_7788|><|vq_lbr_audio_81434|><|vq_lbr_audio_3528|><|vq_lbr_audio_122432|><|vq_lbr_audio_108959|><|vq_lbr_audio_107929|><|vq_lbr_audio_32435|><|vq_lbr_audio_9571|><|vq_lbr_audio_37082|><|vq_lbr_audio_69213|><|vq_lbr_audio_66348|><|vq_lbr_audio_107339|><|vq_lbr_audio_62080|><|vq_lbr_audio_111203|><|vq_lbr_audio_66733|><|vq_lbr_audio_81933|><|vq_lbr_audio_30325|><|vq_lbr_audio_77833|><|vq_lbr_audio_28090|><|vq_lbr_audio_41513|><|vq_lbr_audio_30928|><|vq_lbr_audio_68461|><|vq_lbr_audio_11697|><|vq_lbr_audio_124476|><|vq_lbr_audio_48576|><|vq_lbr_audio_57060|><|vq_lbr_audio_60547|><|vq_lbr_audio_110673|><|vq_lbr_audio_35774|><|vq_lbr_audio_58402|><|vq_lbr_audio_63738|><|vq_lbr_audio_44893|><|vq_lbr_audio_122329|><|vq_lbr_audio_24366|><|vq_lbr_audio_100086|><|vq_lbr_audio_8518|><|vq_lbr_audio_24366|><|vq_lbr_audio_98091|><|vq_lbr_audio_24366|><|vq_lbr_audio_44512|><|vq_lbr_audio_55556|><|vq_lbr_audio_57556|><|vq_lbr_audio_70848|><|vq_lbr_audio_4253|><|vq_lbr_audio_61527|><|vq_lbr_audio_87049|><|vq_lbr_audio_24754|><|vq_lbr_audio_32032|><|vq_lbr_audio_66762|><|vq_lbr_audio_39837|><|vq_lbr_audio_34259|><|vq_lbr_audio_10426|><|vq_lbr_audio_86556|><|vq_lbr_audio_82338|><|vq_lbr_audio_13857|><|vq_lbr_audio_40752|><|vq_lbr_audio_34597|><|vq_lbr_audio_107020|><|vq_lbr_audio_54000|><|vq_lbr_audio_131061|><|vq_lbr_audio_58376|><|vq_lbr_audio_57017|><|vq_lbr_audio_48623|><|vq_lbr_audio_50776|><|vq_lbr_audio_6375|><|vq_lbr_audio_74598|><|vq_lbr_audio_93918|><|vq_lbr_audio_124276|><|vq_lbr_audio_18750|><|vq_lbr_audio_66537|><|vq_lbr_audio_73150|><|vq_lbr_audio_67264|><|vq_lbr_audio_54546|><|vq_lbr_audio_16275|><|vq_lbr_audio_15490|><|vq_lbr_audio_125368|><|vq_lbr_audio_64377|><|vq_lbr_audio_129036|><|vq_lbr_audio_46023|><|vq_lbr_audio_95748|><|vq_lbr_audio_57521|><|vq_lbr_audio_15228|><|vq_lbr_audio_108113|><|vq_lbr_audio_926|><|vq_lbr_audio_86745|><|vq_lbr_audio_63358|><|vq_lbr_audio_110611|><|vq_lbr_audio_83862|><|vq_lbr_audio_109525|><|vq_lbr_audio_109072|><|vq_lbr_audio_90128|><|vq_lbr_audio_127783|><|vq_lbr_audio_116556|><|vq_lbr_audio_101548|><|vq_lbr_audio_120206|><|vq_lbr_audio_26315|><|vq_lbr_audio_8057|><|vq_lbr_audio_45299|><|vq_lbr_audio_126763|><|vq_lbr_audio_81693|><|vq_lbr_audio_50695|><|vq_lbr_audio_58676|><|vq_lbr_audio_31110|><|vq_lbr_audio_19312|><|vq_lbr_audio_119481|><|vq_lbr_audio_59718|><|vq_lbr_audio_84545|><|vq_lbr_audio_40019|><|vq_lbr_audio_15633|><|vq_lbr_audio_78291|><|vq_lbr_audio_122950|><|vq_lbr_audio_105970|><|vq_lbr_audio_5760|><|vq_lbr_audio_125556|><|vq_lbr_audio_118541|><|vq_lbr_audio_101414|><|vq_lbr_audio_48943|><|vq_lbr_audio_50073|><|vq_lbr_audio_92867|><|vq_lbr_audio_109241|><|vq_lbr_audio_113568|><|vq_lbr_audio_15624|><|vq_lbr_audio_40833|><|vq_lbr_audio_52367|><|vq_lbr_audio_90107|><|vq_lbr_audio_42487|><|vq_lbr_audio_9577|><|vq_lbr_audio_65605|><|vq_lbr_audio_81212|><|vq_lbr_audio_2169|><|vq_lbr_audio_959|><|vq_lbr_audio_77601|><|vq_lbr_audio_36089|><|vq_lbr_audio_69074|><|vq_lbr_audio_40019|><|vq_lbr_audio_124642|><|vq_lbr_audio_39853|><|vq_lbr_audio_103341|><|vq_lbr_audio_33006|><|vq_lbr_audio_29955|><|vq_lbr_audio_108505|><|vq_lbr_audio_89159|><|vq_lbr_audio_95846|><|vq_lbr_audio_60022|><|vq_lbr_audio_97682|><|vq_lbr_audio_118793|><|vq_lbr_audio_120019|><|vq_lbr_audio_122430|><|vq_lbr_audio_59925|><|vq_lbr_audio_109999|><|vq_lbr_audio_123967|><|vq_lbr_audio_3074|><|vq_lbr_audio_27667|><|vq_lbr_audio_111750|><|vq_lbr_audio_40214|><|vq_lbr_audio_111809|><|vq_lbr_audio_57601|><|vq_lbr_audio_40463|><|vq_lbr_audio_27196|><|vq_lbr_audio_23798|><|vq_lbr_audio_6439|><|vq_lbr_audio_32168|><|vq_lbr_audio_127229|><|vq_lbr_audio_125301|><|vq_lbr_audio_81063|><|vq_lbr_audio_111202|><|vq_lbr_audio_19787|><|vq_lbr_audio_57317|><|vq_lbr_audio_28756|><|vq_lbr_audio_22041|><|vq_lbr_audio_70845|><|vq_lbr_audio_32518|><|vq_lbr_audio_51023|><|vq_lbr_audio_77088|><|vq_lbr_audio_33553|><|vq_lbr_audio_218|><|vq_lbr_audio_84018|><|vq_lbr_audio_102668|><|vq_lbr_audio_120409|><|vq_lbr_audio_96306|><|vq_lbr_audio_19175|><|vq_lbr_audio_67330|><|vq_lbr_audio_15250|><|vq_lbr_audio_59242|><|vq_lbr_audio_84676|><|vq_lbr_audio_68784|><|vq_lbr_audio_126943|><|vq_lbr_audio_99623|><|vq_lbr_audio_80794|><|vq_lbr_audio_78832|><|vq_lbr_audio_97732|><|vq_lbr_audio_116549|><|vq_lbr_audio_21445|><|vq_lbr_audio_117387|><|vq_lbr_audio_24366|><|vq_lbr_audio_3660|><|vq_lbr_audio_90319|><|vq_lbr_audio_29540|><|vq_lbr_audio_24366|><|vq_lbr_audio_29352|><|vq_lbr_audio_24366|><|vq_lbr_audio_11434|><|vq_lbr_audio_103343|><|vq_lbr_audio_112617|><|vq_lbr_audio_24366|><|vq_lbr_audio_94206|><|vq_lbr_audio_24366|><|vq_lbr_audio_34734|><|vq_lbr_audio_57532|><|vq_lbr_audio_24366|><|vq_lbr_audio_82943|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_11181|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_60787|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_5676|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_99574|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_38119|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_84790|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_74769|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_106868|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_121668|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_80508|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_11424|><|vq_lbr_audio_24366|><|vq_lbr_audio_8210|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_102809|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_49047|><|vq_lbr_audio_19329|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_76487|><|vq_lbr_audio_37619|><|vq_lbr_audio_122864|><|vq_lbr_audio_53781|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_110524|><|vq_lbr_audio_24366|><|vq_lbr_audio_76439|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_122650|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_38765|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_65999|><|vq_lbr_audio_52931|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_65999|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_65999|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_12497|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_38765|><|vq_lbr_audio_52931|><|vq_lbr_audio_65999|><|vq_lbr_audio_114846|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_65999|><|vq_lbr_audio_65999|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_52931|><|vq_lbr_audio_65999|><|vq_lbr_audio_87497|><|vq_lbr_audio_52931|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_65999|><|vq_lbr_audio_52931|><|vq_lbr_audio_11424|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_103343|><|vq_lbr_audio_8552|><|vq_lbr_audio_104569|><|vq_lbr_audio_38657|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_116502|><|vq_lbr_audio_25398|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_26441|><|vq_lbr_audio_56327|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_107833|><|vq_lbr_audio_114032|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_124793|><|vq_lbr_audio_41743|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_31441|><|vq_lbr_audio_76973|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_122357|><|vq_lbr_audio_129137|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_30325|><|vq_lbr_audio_120885|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_120885|><|vq_lbr_audio_120885|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_120885|><|vq_lbr_audio_18865|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_31441|><|vq_lbr_audio_120885|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_123408|><|vq_lbr_audio_120885|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_30325|><|vq_lbr_audio_120885|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_101589|><|vq_lbr_audio_18865|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_124793|><|vq_lbr_audio_28111|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_103421|><|vq_lbr_audio_120885|><|vq_lbr_audio_92056|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_28111|><|vq_lbr_audio_120885|><|vq_lbr_audio_92056|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_103421|><|vq_lbr_audio_120885|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_31366|><|vq_lbr_audio_112294|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_31366|><|vq_lbr_audio_31441|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_59738|><|vq_lbr_audio_50397|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_49384|><|vq_lbr_audio_75290|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_65999|><|vq_lbr_audio_90995|><|vq_lbr_audio_103225|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_24366|><|vq_lbr_audio_28111|><|vq_lbr_audio_18865|><|vq_lbr_audio_24366|><|vq_lbr_audio_30269|><|vq_lbr_audio_24366|><|vq_lbr_audio_69654|><|vq_lbr_audio_18865|><|vq_lBr>ನಂತೂಷ್ಠಾನ: | ಕ್ಯಾಟಲಾಗ್ → ಡೌನ್‌ಲೋಡ್ → ಲೋಡ್ → ಉಪಯೋಗಿಸಿ → ಅನ್ಲೋಡ್ |
| **FoundryModelInfo** | ಶ್ರೀಮಂತ ಮೆಟಾಡೇಟಾ: ಕಾರ್ಯ, ಸಾಧನ, ಗಾತ್ರ, ಪರವಾನಿಗೆ, ಉಪಕರಣ ಕರೆ ಬೆಂಬಲ |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) OpenAI-ರಹಿತ ಬಳಕೆಗೆ |
| **ಭೇದಗಳು** | ಮಾದರಿಗಳು ಸ್ಟ್ರುಕ್ತರ್-ನಿಂದ ವಿಭಿನ್ನ ಭೇದಗಳನ್ನು ಹೊಂದಿವೆ (CPU, GPU, NPU); ಸ್ವಯಂಚಾಲಿತವಾಗಿ ಆಯ್ಕೆ ಮಾಡಲಾಗುತ್ತದೆ |
| **ಅಪ್ಗ್ರೇಡ್ಗಳು** | ಪೈಥಾನ್: `is_model_upgradeable()` + `upgrade_model()` ಮೂಲಕ ಮಾದರಿಗಳನ್ನು ನವೀಕರಿಸಿ |
| **ಕ್ಯಾಟಲಾಗ್ ರಿಫ್ರೆಶ್** | `refresh_catalog()` (Python) / `updateModels()` (JS) ಮೂಲಕ ಹೊಸ ಮಾದರಿಗಳನ್ನು ಕಂಡುಹಿಡಿಯಿರಿ |

---

## ಸಂಪನ್ಮೂಲಗಳು

| ಸಂಪನ್ಮೂಲ | ಲಿಂಕ್ |
|----------|------|
| SDK ಸಂಚಿಕೆ (ಎಲ್ಲಾ ಭಾಷೆಗಳು) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| ಇನ್‌ಫರೆನ್ಸ್ SDK ಗಳ ಜೊತೆಗೆ ಸಂಯೋಜನೆ | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API ಸಂಚಿಕೆ | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK ಉದಾಹರಣೆಗಳು | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local ವೆಬ್ಸೈಟ್ | [foundrylocal.ai](https://foundrylocal.ai) |

---

## ಮುಂದಿನ ಹಂತಗಳು

SDK ಯನ್ನು OpenAI ಕ್ಲೈಂಟ್ ಗ್ರಂಥಾಲಯದೊಂದಿಗೆ ಸಂಪರ್ಕಿಸಿ ಮತ್ತು ನಿಮ್ಮ ಮೊದಲ ಚಾಟ್ ಪೂರ್ಣತೆ ಅನ್ನು ರಚಿಸಲು [ಭಾಗ 3: SDK ಬಳಕೆ ಮತ್ತು APIs](part3-sdk-and-apis.md) ನಲ್ಲಿ ಮುಂದುವರೆಯಿರಿ.