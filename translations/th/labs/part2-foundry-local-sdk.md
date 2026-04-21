![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ส่วนที่ 2: เจาะลึก Foundry Local SDK

> **เป้าหมาย:** ครอบครอง Foundry Local SDK เพื่อจัดการโมเดล บริการ และการแคชอย่างเป็นโปรแกรม - และเข้าใจว่าทำไม SDK ถึงเป็นวิธีที่แนะนำมากกว่า CLI สำหรับการสร้างแอปพลิเคชัน

## ภาพรวม

ในส่วนที่ 1 คุณใช้ **Foundry Local CLI** เพื่อดาวน์โหลดและรันโมเดลอย่างโต้ตอบ CLI เหมาะสำหรับการสำรวจ แต่เมื่อคุณสร้างแอปพลิเคชันจริง คุณจำเป็นต้องมี **การควบคุมแบบโปรแกรม** Foundry Local SDK ให้คุณนั่น - มันจัดการ **control plane** (เริ่มบริการ, ค้นหาโมเดล, ดาวน์โหลด, โหลด) เพื่อให้โค้ดแอปของคุณโฟกัสที่ **data plane** (ส่งคำสั่ง, รับผลลัพธ์)

แลปนี้สอนคุณครอบคลุม API ของ SDK ทั้ง Python, JavaScript และ C# เมื่อจบคุณจะเข้าใจเมธอดทุกตัวและรู้ว่าเมื่อไหร่นำไปใช้

## วัตถุประสงค์การเรียนรู้

เมื่อจบแลปนี้คุณจะสามารถ:

- อธิบายว่าทำไม SDK ถึงได้รับความนิยมมากกว่า CLI สำหรับการพัฒนาแอปพลิเคชัน
- ติดตั้ง Foundry Local SDK สำหรับ Python, JavaScript หรือ C#
- ใช้ `FoundryLocalManager` เพื่อเริ่มบริการ, จัดการโมเดล และสอบถามแค็ตตาล็อก
- แสดงรายการ, ดาวน์โหลด, โหลด และปลดโหลดโมเดลอย่างโปรแกรม
- ตรวจสอบข้อมูลเมตาโมเดลด้วย `FoundryModelInfo`
- เข้าใจความแตกต่างระหว่างแค็ตตาล็อก, แคช และโมเดลที่โหลดแล้ว
- ใช้ตัวสร้าง bootstrap (Python) และรูปแบบ `create()` + แค็ตตาล็อก (JavaScript)
- เข้าใจการออกแบบใหม่ของ C# SDK และ API แบบวัตถุ

---

## สิ่งที่ต้องเตรียม

| ความต้องการ | รายละเอียด |
|-------------|------------|
| **Foundry Local CLI** | ติดตั้งและอยู่ใน `PATH` ของคุณ ([ส่วนที่ 1](part1-getting-started.md)) |
| **รันไทม์ภาษา** | **Python 3.9+** และ/หรือ **Node.js 18+** และ/หรือ **.NET 9.0+** |

---

## แนวคิด: SDK vs CLI - ทำไมต้องใช้ SDK?

| ด้าน | CLI (`foundry` คำสั่ง) | SDK (`foundry-local-sdk`) |
|-------|----------------------|-------------------------|
| **กรณีการใช้งาน** | สำรวจ, ทดสอบด้วยมือ | ผนวกรวมแอปพลิเคชัน |
| **จัดการบริการ** | ด้วยมือ: `foundry service start` | อัตโนมัติ: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **ค้นหาพอร์ต** | อ่านจากผลลัพธ์ CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **ดาวน์โหลดโมเดล** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **จัดการข้อผิดพลาด** | รหัสออก, stderr | ข้อยกเว้น, ข้อผิดพลาดแบบพิมพ์ |
| **อัตโนมัติ** | สคริปต์เชลล์ | ผนวกรวมภาษาเนทีฟ |
| **ดีพลอยเมนต์** | ต้องใช้ CLI บนเครื่องผู้ใช้ | C# SDK เป็นแบบรวมหรือแยกได้ไม่ต้องใช้ CLI |

> **ข้อมูลสำคัญ:** SDK ดูแลวงจรชีวิตทั้งหมด: เริ่มบริการ, ตรวจสอบแคช, ดาวน์โหลดโมเดลที่ขาด, โหลดโมเดล, และค้นหาจุดเชื่อมต่อ, ในไม่กี่บรรทัดโค้ด แอปของคุณไม่ต้องแยกวิเคราะห์ผลลัพธ์ CLI หรือจัดการ subprocesses

---

## แบบฝึกหัดแลป

### แบบฝึกหัด 1: ติดตั้ง SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

ตรวจสอบการติดตั้ง:

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

ตรวจสอบการติดตั้ง:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

มีสองแพ็กเกจ NuGet:

| แพ็กเกจ | แพลตฟอร์ม | คำอธิบาย |
|---------|-----------|----------|
| `Microsoft.AI.Foundry.Local` | ข้ามแพลตฟอร์ม | ใช้งานบน Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | เฉพาะ Windows | เพิ่มการเร่งฮาร์ดแวร์ WinML; ดาวน์โหลดและติดตั้งผู้ให้บริการรันไทม์ปลั๊กอิน (เช่น QNN สำหรับ Qualcomm NPU) |

**การตั้งค่า Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

แก้ไขไฟล์ `.csproj`:

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

> **หมายเหตุ:** บน Windows แพ็กเกจ WinML คือซูเปอร์เซ็ตที่รวม SDK พื้นฐานกับผู้ให้บริการรันไทม์ QNN บน Linux/macOS จะใช้ SDK พื้นฐาน แพลตฟอร์มเป้าหมายเงื่อนไขและการอ้างอิงแพ็กเกจช่วยให้โปรเจกต์รองรับทุกแพลตฟอร์มเต็มที่

สร้างไฟล์ `nuget.config` ที่โฟลเดอร์โปรเจกต์หลัก:

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

กู้คืนแพ็กเกจ:

```bash
dotnet restore
```

</details>

---

### แบบฝึกหัด 2: เริ่มบริการและแสดงรายชื่อแค็ตตาล็อก

สิ่งแรกที่แอปพลิเคชันทำคือเริ่มบริการ Foundry Local และค้นหาว่าโมเดลอะไรบ้างที่มี

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# สร้างผู้จัดการและเริ่มต้นบริการ
manager = FoundryLocalManager()
manager.start_service()

# แสดงรายการโมเดลทั้งหมดที่มีในแคตตาล็อก
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - เมธอดจัดการบริการ

| เมธอด | ซิกเนเจอร์ | คำอธิบาย |
|--------|-------------|-----------|
| `is_service_running()` | `() -> bool` | ตรวจสอบว่าบริการกำลังรันอยู่หรือไม่ |
| `start_service()` | `() -> None` | เริ่มบริการ Foundry Local |
| `service_uri` | `@property -> str` | URI ฐานของบริการ |
| `endpoint` | `@property -> str` | จุดเชื่อมต่อ API (service URI + `/v1`) |
| `api_key` | `@property -> str` | คีย์ API (จาก env หรือค่าเริ่มต้น) |

#### Python SDK - เมธอดจัดการแค็ตตาล็อก

| เมธอด | ซิกเนเจอร์ | คำอธิบาย |
|--------|------------|-----------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | แสดงรายการโมเดลทั้งหมดในแค็ตตาล็อก |
| `refresh_catalog()` | `() -> None` | รีเฟรชแค็ตตาล็อกจากบริการ |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | รับข้อมูลของโมเดลเฉพาะ |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// สร้างผู้จัดการและเริ่มต้นบริการ
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// เรียกดูแคตตาล็อก
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - เมธอดของ Manager

| เมธอด | ซิกเนเจอร์ | คำอธิบาย |
|--------|------------|-----------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | เริ่มต้น singleton SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | เข้าถึง manager singleton |
| `manager.startWebService()` | `() => Promise<void>` | เริ่มบริการเว็บ Foundry Local |
| `manager.urls` | `string[]` | อาร์เรย์ URL ฐานของบริการ |

#### JavaScript SDK - เมธอดแค็ตตาล็อกและโมเดล

| เมธอด | ซิกเนเจอร์ | คำอธิบาย |
|--------|------------|-----------|
| `manager.catalog` | `Catalog` | เข้าถึงแค็ตตาล็อกโมเดล |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | รับวัตถุโมเดลตาม alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ ใช้สถาปัตยกรรมเชิงวัตถุกับวัตถุ `Configuration`, `Catalog`, และ `Model`:

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

#### C# SDK - คลาสสำคัญ

| คลาส | วัตถุประสงค์ |
|-------|--------------|
| `Configuration` | ตั้งชื่อแอป ระดับล็อก โฟลเดอร์แคช URL เว็บเซิร์ฟเวอร์ |
| `FoundryLocalManager` | จุดเริ่มต้นหลัก - สร้างผ่าน `CreateAsync()`, เข้าถึงผ่าน `.Instance` |
| `Catalog` | เรียกดู ค้นหา และรับโมเดลจากแค็ตตาล็อก |
| `Model` | แทนโมเดลเฉพาะ - ดาวน์โหลด โหลด รับไคลเอนต์ |

#### C# SDK - เมธอดของ Manager และ Catalog

| เมธอด | คำอธิบาย |
|--------|-----------|
| `FoundryLocalManager.CreateAsync(config, logger)` | เริ่มต้น manager |
| `FoundryLocalManager.Instance` | เข้าถึง manager singleton |
| `manager.GetCatalogAsync()` | รับแค็ตตาล็อกโมเดล |
| `catalog.ListModelsAsync()` | แสดงรายการโมเดลทั้งหมด |
| `catalog.GetModelAsync(alias: "alias")` | รับโมเดลเฉพาะตาม alias |
| `catalog.GetCachedModelsAsync()` | แสดงรายการโมเดลที่ดาวน์โหลดแล้ว |
| `catalog.GetLoadedModelsAsync()` | แสดงรายการโมเดลที่กำลังโหลด |

> **บันทึกสถาปัตยกรรม C#:** C# SDK v0.8.0+ ออกแบบใหม่เพื่อให้แอปพลิเคชัน **ตัวเดียวจบ**; ไม่ต้องใช้ Foundry Local CLI บนเครื่องผู้ใช้ SDK จัดการโมเดลและการอนุมานอย่างเนทีฟ

</details>

---

### แบบฝึกหัด 3: ดาวน์โหลดและโหลดโมเดล

SDK แยกความแตกต่างระหว่างดาวน์โหลด (ไปยังดิสก์) กับโหลด (เข้าเมมโมรี่) เพื่อให้คุณดาวน์โหลดโมเดลล่วงหน้าตอนตั้งค่าและโหลดเมื่อขอใช้งาน

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# ตัวเลือก ก: ทำทีละขั้นตอนด้วยมือ
manager = FoundryLocalManager()
manager.start_service()

# ตรวจสอบแคชก่อน
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

# ตัวเลือก ข: บูตสแตรปแบบบรรทัดเดียว (แนะนำ)
# ส่ง alias ให้ constructor - มันจะเริ่มบริการ ดาวน์โหลด และโหลดโดยอัตโนมัติ
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - เมธอดจัดการโมเดล

| เมธอด | ซิกเนเจอร์ | คำอธิบาย |
|--------|-------------|----------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | ดาวน์โหลดโมเดลไปยังแคชท้องถิ่น |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | โหลดโมเดลเข้าสู่เซิร์ฟเวอร์อนุมาน |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | ปลดโหลดโมเดลออกจากเซิร์ฟเวอร์ |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | แสดงรายการโมเดลที่กำลังโหลด |

#### Python - เมธอดจัดการแคช

| เมธอด | ซิกเนเจอร์ | คำอธิบาย |
|--------|------------|----------|
| `get_cache_location()` | `() -> str` | รับเส้นทางโฟลเดอร์แคช |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | แสดงรายการโมเดลที่ดาวน์โหลดแล้ว |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// แนวทางเป็นขั้นตอน
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

#### JavaScript - เมธอดโมเดล

| เมธอด | ซิกเนเจอร์ | คำอธิบาย |
|--------|------------|----------|
| `model.isCached` | `boolean` | โมเดลดาวน์โหลดแล้วหรือยัง |
| `model.download()` | `() => Promise<void>` | ดาวน์โหลดโมเดลไปยังแคชท้องถิ่น |
| `model.load()` | `() => Promise<void>` | โหลดเข้าสู่เซิร์ฟเวอร์อนุมาน |
| `model.unload()` | `() => Promise<void>` | ปลดโหลดออกจากเซิร์ฟเวอร์ |
| `model.id` | `string` | ตัวระบุโมเดลเฉพาะ |

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

#### C# - เมธอดโมเดล

| เมธอด | คำอธิบาย |
|--------|----------|
| `model.DownloadAsync(progress?)` | ดาวน์โหลดชนิดที่เลือก |
| `model.LoadAsync()` | โหลดโมเดลเข้าสู่เมมโมรี่ |
| `model.UnloadAsync()` | ปลดโหลดโมเดล |
| `model.SelectVariant(variant)` | เลือกชนิดเฉพาะ (CPU/GPU/NPU) |
| `model.SelectedVariant` | ชนิดที่เลือกปัจจุบัน |
| `model.Variants` | ทุกชนิดที่มีสำหรับโมเดลนี้ |
| `model.GetPathAsync()` | รับเส้นทางไฟล์ท้องถิ่น |
| `model.GetChatClientAsync()` | รับไคลเอนต์แชทเนทีฟ (ไม่ต้องใช้ OpenAI SDK) |
| `model.GetAudioClientAsync()` | รับไคลเอนต์เสียงสำหรับถอดเสียง |

</details>

---

### แบบฝึกหัด 4: ตรวจสอบข้อมูลเมตาโมเดล

วัตถุ `FoundryModelInfo` มีข้อมูลเมตาละเอียดของแต่ละโมเดล การเข้าใจฟิลด์เหล่านี้ช่วยให้คุณเลือกโมเดลที่ใช่สำหรับแอปของคุณ

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# รับข้อมูลโดยละเอียดเกี่ยวกับโมเดลเฉพาะเจาะจง
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

#### ฟิลด์ของ FoundryModelInfo

| ฟิลด์ | ชนิด | คำอธิบาย |
|-------|------|-----------|
| `alias` | string | ชื่อย่อ (เช่น `phi-3.5-mini`) |
| `id` | string | ตัวระบุโมเดลเฉพาะ |
| `version` | string | เวอร์ชันโมเดล |
| `task` | string | `chat-completions` หรือ `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU หรือ NPU |
| `execution_provider` | string | รันไทม์แบ็คเอนด์ (CUDA, CPU, QNN, WebGPU ฯลฯ) |
| `file_size_mb` | int | ขนาดไฟล์บนดิสก์ (MB) |
| `supports_tool_calling` | bool | รองรับการเรียกฟังก์ชัน/เครื่องมือหรือไม่ |
| `publisher` | string | ผู้เผยแพร่โมเดล |
| `license` | string | ชื่อไลเซนส์ |
| `uri` | string | URI โมเดล |
| `prompt_template` | dict/null | แม่แบบพรอมต์ (ถ้ามี) |

---

### แบบฝึกหัด 5: จัดการวงจรชีวิตโมเดล

ฝึกการใช้งานครบวงจร: แสดงรายการ → ดาวน์โหลด → โหลด → ใช้งาน → ปลดโหลด

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # โมเดลขนาดเล็กสำหรับการทดสอบอย่างรวดเร็ว

manager = FoundryLocalManager()
manager.start_service()

# 1. ตรวจสอบว่าสิ่งใดอยู่ในแคตตาล็อก
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. ตรวจสอบว่าสิ่งใดถูกดาวน์โหลดไปแล้ว
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. ดาวน์โหลดโมเดล
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. ยืนยันว่าโมเดลนั้นอยู่ในแคชตอนนี้
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. โหลดมัน
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. ตรวจสอบว่าสิ่งใดถูกโหลด
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. ยกเลิกการโหลดมัน
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

const alias = "qwen2.5-0.5b"; // โมเดลขนาดเล็กสำหรับทดสอบอย่างรวดเร็ว

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. รับโมเดลจากแคตตาล็อก
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. ดาวน์โหลดหากจำเป็น
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. โหลดมัน
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. ปลดออกจากหน่วยความจำ
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### แบบฝึกหัดที่ 6: รูปแบบเริ่มต้นอย่างรวดเร็ว

แต่ละภาษาให้ทางลัดเพื่อเริ่มบริการและโหลดโมเดลในคำสั่งเดียว นี่คือ **รูปแบบที่แนะนำ** สำหรับแอปพลิเคชันส่วนใหญ่

<details>
<summary><h3>🐍 Python - การบูตสแตรปในตัวสร้าง</h3></summary>

```python
from foundry_local import FoundryLocalManager

# ส่ง alias ไปยังตัวสร้าง - มันจัดการทุกอย่าง:
# 1. เริ่มต้นบริการถ้ายังไม่ทำงาน
# 2. ดาวน์โหลดโมเดลถ้ายังไม่มีในแคช
# 3. โหลดโมเดลเข้าสู่เซิร์ฟเวอร์การอนุมาน
manager = FoundryLocalManager("phi-3.5-mini")

# พร้อมใช้งานทันที
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

พารามิเตอร์ `bootstrap` (ค่าเริ่มต้น `True`) ควบคุมพฤติกรรมนี้ ตั้งค่า `bootstrap=False` หากคุณต้องการควบคุมด้วยตนเอง:

```python
# โหมดแมนนวล - ไม่มีอะไรเกิดขึ้นโดยอัตโนมัติ
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + แคตตาล็อก</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() จัดการทุกอย่าง:
// 1. เริ่มต้นบริการ
// 2. รับโมเดลจากแคตาล็อก
// 3. ดาวน์โหลดถ้าจำเป็นและโหลดโมเดล
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// พร้อมใช้งานทันที
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + แคตตาล็อก</h3></summary>

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

> **หมายเหตุ C#:** SDK ของ C# ใช้ `Configuration` เพื่อควบคุมชื่อแอป, การบันทึก, ไดเร็กทอรีแคช และแม้แต่กำหนดพอร์ตเซิร์ฟเวอร์เว็บเฉพาะ ซึ่งทำให้เป็น SDK ที่ปรับแต่งได้มากที่สุดในสาม SDK

</details>

---

### แบบฝึกหัดที่ 7: Native ChatClient (ไม่ต้องใช้ OpenAI SDK)

SDK ของ JavaScript และ C# ให้เมธอดสะดวก `createChatClient()` ที่ส่งคืนไคลเอนต์แชทเนทีฟ — ไม่จำเป็นต้องติดตั้งหรือกำหนดค่า OpenAI SDK แยกต่างหาก

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

// สร้าง ChatClient โดยตรงจากโมเดล — ไม่จำเป็นต้องนำเข้า OpenAI
const chatClient = model.createChatClient();

// completeChat ส่งคืนวัตถุตอบสนองที่เข้ากันได้กับ OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// การสตรีมใช้รูปแบบ callback
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` ยังรองรับการเรียกใช้เครื่องมือ — ส่งผ่านเครื่องมือเป็นอาร์กิวเมนต์ที่สอง:

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

> **เมื่อใดควรใช้รูปแบบใด:**
> - **`createChatClient()`** — การสร้างต้นแบบอย่างรวดเร็ว, พึ่งพาน้อยกว่า, โค้ดง่ายกว่า
> - **OpenAI SDK** — ควบคุมพารามิเตอร์เต็มรูปแบบ (อุณหภูมิ, top_p, สต็อปโทเค็น ฯลฯ), เหมาะกับแอปพลิเคชันในผลิต

---

### แบบฝึกหัดที่ 8: ตัวแปรโมเดลและการเลือกฮาร์ดแวร์

โมเดลสามารถมี **ตัวแปร** หลายแบบที่ปรับแต่งสำหรับฮาร์ดแวร์ต่าง ๆ SDK จะเลือกตัวแปรที่ดีที่สุดโดยอัตโนมัติ แต่คุณก็สามารถตรวจสอบและเลือกเองได้

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// แสดงรายการตัวแปรทั้งหมดที่มี
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK จะเลือกตัวแปรที่ดีที่สุดสำหรับฮาร์ดแวร์ของคุณโดยอัตโนมัติ
// หากต้องการแทนที่ ให้ใช้ selectVariant():
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

ใน Python SDK จะเลือกตัวแปรที่ดีที่สุดโดยอัตโนมัติขึ้นอยู่กับฮาร์ดแวร์ ใช้ `get_model_info()` เพื่อดูว่าตัวแปรใดถูกเลือก:

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

#### โมเดลที่มีตัวแปร NPU

บางโมเดลมีตัวแปรที่ปรับแต่งสำหรับ NPU (Qualcomm Snapdragon, Intel Core Ultra):

| โมเดล | มีตัวแปร NPU |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **เคล็ดลับ:** บนฮาร์ดแวร์ที่รองรับ NPU SDK จะเลือกตัวแปร NPU โดยอัตโนมัติเมื่อมี คุณไม่จำเป็นต้องเปลี่ยนโค้ด สำหรับโปรเจกต์ C# บน Windows ให้เพิ่มแพ็กเกจ `Microsoft.AI.Foundry.Local.WinML` จาก NuGet เพื่อเปิดใช้งานตัวจัดการการประมวลผล QNN — QNN ถูกส่งผ่านเป็นปลั๊กอิน EP ผ่าน WinML

---

### แบบฝึกหัดที่ 9: อัปเกรดโมเดลและรีเฟรชแคตตาล็อก

แคตตาล็อกโมเดลจะได้รับการอัปเดตเป็นระยะ ใช้วิธีเหล่านี้เพื่อตรวจสอบและใช้การอัปเดต

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# รีเฟรชแคตตาล็อกเพื่อรับรายชื่อรุ่นล่าสุด
manager.refresh_catalog()

# ตรวจสอบว่ารุ่นที่แคชมีเวอร์ชันใหม่กว่าหรือไม่
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

// รีเฟรชแคตตาล็อกเพื่อดึงรายการโมเดลล่าสุด
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// แสดงรายการโมเดลทั้งหมดที่มีหลังจากรีเฟรช
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### แบบฝึกหัดที่ 10: การทำงานกับโมเดลสำหรับเหตุผล

โมเดล **phi-4-mini-reasoning** มีการคิดเชิงตรรกะแบบเชน อัลกอริทึมจะห่อความคิดภายในด้วยแท็ก `<think>...</think>` ก่อนให้คำตอบสุดท้าย วิธีนี้มีประโยชน์สำหรับงานที่ต้องใช้ตรรกะหลายขั้นตอน, คณิตศาสตร์ หรือการแก้ปัญหา

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning มีขนาดประมาณ 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# โมเดลจะห่อการคิดของมันด้วยแท็ก <think>...</think>
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

// วิเคราะห์การคิดแบบโซ่ต่อเนื่อง
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **เมื่อใดควรใช้โมเดลเหตุผล:**
> - ปัญหาคณิตศาสตร์และตรรกะ
> - งานวางแผนหลายขั้นตอน
> - การสร้างโค้ดที่ซับซ้อน
> - งานที่การแสดงการทำงานช่วยเพิ่มความแม่นยำ
>
> **แลกเปลี่ยน:** โมเดลเหตุผลสร้างโทเค็นมากขึ้น (ส่วน `<think>`) และช้ากว่า สำหรับคำถาม-คำตอบธรรมดา โมเดลมาตรฐานเช่น phi-3.5-mini จะเร็วกว่ามาก

---

### แบบฝึกหัดที่ 11: การเข้าใจนามแฝงและการเลือกฮาร์ดแวร์

เมื่อคุณส่ง **นามแฝง** (เช่น `phi-3.5-mini`) แทนที่จะเป็นรหัสโมเดลเต็ม SDK จะเลือกตัวแปรที่เหมาะสมกับฮาร์ดแวร์ของคุณโดยอัตโนมัติ:

| ฮาร์ดแวร์ | ผู้ให้บริการการประมวลผลที่ถูกเลือก |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (ผ่านปลั๊กอิน WinML) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| อุปกรณ์ใด ๆ (สำรอง) | `CPUExecutionProvider` หรือ `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# นามแฝงนี้สลายเป็นตัวแปรที่ดีที่สุดสำหรับฮาร์ดแวร์ของคุณ
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **เคล็ดลับ:** ใช้นามแฝงในโค้ดแอปพลิเคชันเสมอ เมื่อคุณดีพลอยลงเครื่องผู้ใช้ SDK จะเลือกตัวแปรที่เหมาะสมที่สุดเวลาใช้งาน — CUDA บน NVIDIA, QNN บน Qualcomm, CPU ที่อื่น

---

### แบบฝึกหัดที่ 12: ตัวเลือกการกำหนดค่า C#

คลาส `Configuration` ของ SDK C# ให้การควบคุมที่ละเอียดมากเหนือเวลารัน:

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

| คุณสมบัติ | ค่าเริ่มต้น | คำอธิบาย |
|----------|---------|-------------|
| `AppName` | (จำเป็น) | ชื่อแอปพลิเคชันของคุณ |
| `LogLevel` | `Information` | ความละเอียดของการบันทึก |
| `Web.Urls` | (แบบไดนามิก) | กำหนดพอร์ตเฉพาะสำหรับเว็บเซิร์ฟเวอร์ |
| `AppDataDir` | ค่าเริ่มต้น OS | ไดเร็กทอรีฐานสำหรับข้อมูลแอป |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | ที่เก็บโมเดล |
| `LogsDir` | `{AppDataDir}/logs` | ที่เก็บบันทึก |

---

### แบบฝึกหัดที่ 13: การใช้งานเบราว์เซอร์ (เฉพาะ JavaScript)

SDK ของ JavaScript มีเวอร์ชันที่ใช้งานในเบราว์เซอร์ได้ ในเบราว์เซอร์คุณต้องเริ่มบริการด้วย CLI ด้วยตัวเองและระบุ URL โฮสต์:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// เริ่มบริการด้วยตนเองก่อน:
//   เริ่มบริการ foundry
// จากนั้นใช้ URL จากผลลัพธ์ของ CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// เรียกดูแคตตาล็อก
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **ข้อจำกัดของเบราว์เซอร์:** เวอร์ชันเบราว์เซอร์ไม่รองรับ `startWebService()` คุณต้องแน่ใจว่าบริการ Foundry Local กำลังรันก่อนใช้ SDK ในเบราว์เซอร์

---

## เอกสารอ้างอิง API แบบสมบูรณ์

### Python

| หมวดหมู่ | เมธอด | คำอธิบาย |
|----------|--------|-------------|
| **เริ่มต้น** | `FoundryLocalManager(alias?, bootstrap=True)` | สร้างผู้จัดการ; ตัวเลือกบูตสแตรปด้วยโมเดล |
| **บริการ** | `is_service_running()` | ตรวจสอบว่าบริการกำลังทำงาน |
| **บริการ** | `start_service()` | เริ่มบริการ |
| **บริการ** | `endpoint` | URL จุดปลาย API |
| **บริการ** | `api_key` | คีย์ API |
| **แคตตาล็อก** | `list_catalog_models()` | รายการโมเดลทั้งหมดที่มี |
| **แคตตาล็อก** | `refresh_catalog()` | รีเฟรชแคตตาล็อก |
| **แคตตาล็อก** | `get_model_info(alias_or_model_id)` | รับเมตาดาต้าโมเดล |
| **แคช** | `get_cache_location()` | เส้นทางไดเร็กทอรีแคช |
| **แคช** | `list_cached_models()` | รายการโมเดลที่ดาวน์โหลดแล้ว |
| **โมเดล** | `download_model(alias_or_model_id)` | ดาวน์โหลดโมเดล |
| **โมเดล** | `load_model(alias_or_model_id, ttl=600)` | โหลดโมเดล |
| **โมเดล** | `unload_model(alias_or_model_id)` | ปลดโหลดโมเดล |
| **โมเดล** | `list_loaded_models()` | รายการโมเดลที่โหลดแล้ว |
| **โมเดล** | `is_model_upgradeable(alias_or_model_id)` | ตรวจสอบว่ามีเวอร์ชันใหม่กว่าหรือไม่ |
| **โมเดล** | `upgrade_model(alias_or_model_id)` | อัปเกรดโมเดลเป็นเวอร์ชันล่าสุด |
| **บริการ** | `httpx_client` | HTTPX client ที่ตั้งค่าล่วงหน้าสำหรับเรียก API โดยตรง |

### JavaScript

| หมวดหมู่ | เมธอด | คำอธิบาย |
|----------|--------|-------------|
| **เริ่มต้น** | `FoundryLocalManager.create(options)` | เตรียม SDK ให้เป็น singleton |
| **เริ่มต้น** | `FoundryLocalManager.instance` | เข้าถึง singleton manager |
| **บริการ** | `manager.startWebService()` | เริ่มเว็บเซอร์วิส |
| **บริการ** | `manager.urls` | อาเรย์ URL หลักของบริการ |
| **แคตตาล็อก** | `manager.catalog` | เข้าถึงแคตตาล็อกโมเดล |
| **แคตตาล็อก** | `catalog.getModel(alias)` | ดึงวัตถุโมเดลตามนามแฝง (คืน Promise) |
| **โมเดล** | `model.isCached` | โมเดลดาวน์โหลดแล้วหรือไม่ |
| **โมเดล** | `model.download()` | ดาวน์โหลดโมเดล |
| **โมเดล** | `model.load()` | โหลดโมเดล |
| **โมเดล** | `model.unload()` | ปลดโหลดโมเดล |
| **โมเดล** | `model.id` | รหัสประจำตัวโมเดล |
| **โมเดล** | `model.alias` | นามแฝงของโมเดล |
| **โมเดล** | `model.createChatClient()` | รับไคลเอนต์แชทเนทีฟ (ไม่ต้องใช้ OpenAI SDK) |
| **โมเดล** | `model.createAudioClient()` | รับไคลเอนต์เสียงสำหรับถอดเสียง |
| **โมเดล** | `model.removeFromCache()` | เอาโมเดลออกจากแคชท้องถิ่น |
| **โมเดล** | `model.selectVariant(variant)` | เลือกตัวแปรฮาร์ดแวร์เฉพาะ |
| **โมเดล** | `model.variants` | อาเรย์ตัวแปรที่มีสำหรับโมเดลนี้ |
| **โมเดล** | `model.isLoaded()` | ตรวจสอบว่าโมเดลถูกโหลดอยู่หรือไม่ |
| **แคตตาล็อก** | `catalog.getModels()` | รายการโมเดลทั้งหมดที่มี |
| **แคตตาล็อก** | `catalog.getCachedModels()` | รายการโมเดลที่ดาวน์โหลดแล้ว |
| **แคตตาล็อก** | `catalog.getLoadedModels()` | รายการโมเดลที่โหลดแล้ว |
| **แคตตาล็อก** | `catalog.updateModels()` | รีเฟรชแคตตาล็อกจากบริการ |
| **บริการ** | `manager.stopWebService()` | หยุดเว็บเซอร์วิส Foundry Local |

### C# (v0.8.0+)

| หมวดหมู่ | เมธอด | คำอธิบาย |
|----------|--------|-------------|
| **เริ่มต้น** | `FoundryLocalManager.CreateAsync(config, logger)` | เตรียม manager |
| **เริ่มต้น** | `FoundryLocalManager.Instance` | เข้าถึง singleton |
| **แคตตาล็อก** | `manager.GetCatalogAsync()` | รับแคตตาล็อก |
| **แคตตาล็อก** | `catalog.ListModelsAsync()` | รายการโมเดลทั้งหมด |
| **แคตตาล็อก** | `catalog.GetModelAsync(alias)` | รับโมเดลเฉพาะ |
| **แคตตาล็อก** | `catalog.GetCachedModelsAsync()` | รายการโมเดลที่ถูกแคช |
| **แคตตาล็อก** | `catalog.GetLoadedModelsAsync()` | รายการโมเดลที่โหลดแล้ว |
| **โมเดล** | `model.DownloadAsync(progress?)` | ดาวน์โหลดโมเดล |
| **โมเดล** | `model.LoadAsync()` | โหลดโมเดล |
| **โมเดล** | `model.UnloadAsync()` | ปลดโหลดโมเดล |
| **โมเดล** | `model.SelectVariant(variant)` | เลือกตัวแปรฮาร์ดแวร์ |
| **โมเดล** | `model.GetChatClientAsync()` | รับไคลเอนต์แชทเนทีฟ |
| **โมเดล** | `model.GetAudioClientAsync()` | รับไคลเอนต์ถอดเสียง |
| **โมเดล** | `model.GetPathAsync()` | รับเส้นทางไฟล์ท้องถิ่น |
| **แคตตาล็อก** | `catalog.GetModelVariantAsync(alias, variant)` | รับตัวแปรฮาร์ดแวร์เฉพาะ |
| **แคตตาล็อก** | `catalog.UpdateModelsAsync()` | รีเฟรชแคตตาล็อก |
| **เซิร์ฟเวอร์** | `manager.StartWebServerAsync()` | เริ่มเว็บเซิร์ฟเวอร์ REST |
| **เซิร์ฟเวอร์** | `manager.StopWebServerAsync()` | หยุดเว็บเซิร์ฟเวอร์ REST |
| **กำหนดค่า** | `config.ModelCacheDir` | ไดเร็กทอรีแคช |

---

## ประเด็นสำคัญที่ควรจำ

| แนวคิด | สิ่งที่คุณเรียนรู้ |
|---------|-----------------|
| **SDK กับ CLI** | SDK ให้การควบคุมผ่านโปรแกรม — จำเป็นสำหรับแอปพลิเคชัน |
| **ควบคุมการทำงาน** | SDK จัดการบริการ, โมเดล และการแคช |
| **พอร์ตไดนามิก** | ใช้ `manager.endpoint` (Python) หรือ `manager.urls[0]` (JS/C#) เสมอ — อย่ากำหนดพอร์ตตายตัว |
| **นามแฝง** | ใช้นามแฝงเพื่อให้ SDK เลือกโมเดลที่เหมาะกับฮาร์ดแวร์โดยอัตโนมัติ |
| **เริ่มต้นอย่างรวดเร็ว** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **การออกแบบใหม่ใน C#** | v0.8.0+ เป็นแบบอิสระ - ไม่ต้องใช้ CLI บนเครื่องของผู้ใช้ |
| **วงจรชีวิตของโมเดล** | Catalog → ดาวน์โหลด → โหลด → ใช้งาน → ปลดโหลด |
| **FoundryModelInfo** | เมตาดาต้าที่หลากหลาย: งาน, อุปกรณ์, ขนาด, ใบอนุญาต, การสนับสนุนเครื่องมือเรียกใช้งาน |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) สำหรับใช้งาน OpenAI ฟรี |
| **ตัวแปร** | โมเดลมีตัวแปรเฉพาะฮาร์ดแวร์ (CPU, GPU, NPU); เลือกโดยอัตโนมัติ |
| **การอัปเกรด** | Python: `is_model_upgradeable()` + `upgrade_model()` เพื่อรักษาโมเดลให้ทันสมัย |
| **รีเฟรช Catalog** | `refresh_catalog()` (Python) / `updateModels()` (JS) เพื่อค้นหาโมเดลใหม่ |

---

## แหล่งข้อมูล

| แหล่งข้อมูล | ลิงก์ |
|----------|------|
| เอกสารอ้างอิง SDK (ทุกภาษา) | [Microsoft Learn - เอกสารอ้างอิง Foundry Local SDK](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| การรวมกับ SDK การอนุมาน | [Microsoft Learn - การรวมกับ Inference SDK](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| เอกสารอ้างอิง API C# SDK | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| ตัวอย่าง C# SDK | [GitHub - ตัวอย่าง Foundry Local SDK](https://aka.ms/foundrylocalSDK) |
| เว็บไซต์ Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## ขั้นตอนถัดไป

ดำเนินการต่อไปที่ [ส่วนที่ 3: การใช้ SDK กับ OpenAI](part3-sdk-and-apis.md) เพื่อเชื่อมต่อ SDK กับไลบรารีลูกค้า OpenAI และสร้างแอปพลิเคชันเติมข้อความแชทตัวแรกของคุณ.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ข้อจำกัดความรับผิดชอบ**:  
เอกสารนี้ได้รับการแปลโดยใช้บริการแปลภาษาด้วย AI [Co-op Translator](https://github.com/Azure/co-op-translator) แม้ว่าเราจะมุ่งมั่นเพื่อความถูกต้อง โปรดทราบว่าการแปลอัตโนมัติอาจมีข้อผิดพลาดหรือความไม่ถูกต้องได้ เอกสารต้นฉบับในภาษาดั้งเดิมควรถือเป็นแหล่งข้อมูลที่ถูกต้องสำหรับข้อมูลสำคัญ แนะนำให้ใช้การแปลโดยมืออาชีพสำหรับข้อมูลที่สำคัญ เราไม่รับผิดชอบต่อความเข้าใจผิดหรือการตีความผิดใดๆ ที่เกิดจากการใช้การแปลนี้
<!-- CO-OP TRANSLATOR DISCLAIMER END -->