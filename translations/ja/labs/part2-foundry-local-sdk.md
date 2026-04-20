![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# パート 2: Foundry Local SDK 詳細解説

> **目標:** Foundry Local SDK をマスターして、プログラムからモデル、サービス、キャッシュを管理できるようになること。そして、なぜアプリ構築には CLI より SDK の使用が推奨されているのか理解すること。

## 概要

パート 1 では **Foundry Local CLI** を使ってモデルをダウンロードし、対話的に実行しました。CLI は探索に適していますが、本格的なアプリ開発では <strong>プログラム制御</strong> が必要です。Foundry Local SDK はこれを実現します。SDK は <strong>コントロールプレーン</strong>（サービスの起動、モデルの検出、ダウンロード、ロード）を管理し、アプリコードは <strong>データプレーン</strong>（プロンプトの送信、完了の受信）に集中できます。

このラボでは Python、JavaScript、C# の SDK API 全体を学びます。最後には使える全てのメソッドを理解し、適切な使い分けができるようになります。

## 学習目標

このラボを終えると以下ができるようになります：

- SDK がなぜ CLI よりアプリ開発に適しているか説明できる
- Python、JavaScript、C# 向け Foundry Local SDK のインストール方法
- `FoundryLocalManager` を使ったサービス起動、モデル管理、カタログ問い合わせ
- プログラムからモデルの一覧表示、ダウンロード、ロード、アンロードする
- `FoundryModelInfo` でモデルのメタデータを検査する
- カタログ、キャッシュ、ロード済みモデルの違いを理解する
- コンストラクタのブートストラップ（Python）と `create()` + カタログパターン（JavaScript）を使う
- C# SDK の再設計とそのオブジェクト指向 API を理解する

---

## 前提条件

| 要件 | 詳細 |
|------|------|
| **Foundry Local CLI** | インストール済みで PATH にあること ([パート 1](part1-getting-started.md)) |
| <strong>言語ランタイム</strong> | **Python 3.9+** かつ/または **Node.js 18+** かつ/または **.NET 9.0+** |

---

## コンセプト：SDK と CLI - なぜ SDK を使うのか？

| 項目 | CLI (`foundry` コマンド) | SDK (`foundry-local-sdk`) |
|-------|---------------------------|---------------------------|
| <strong>利用ケース</strong> | 探索、手動テスト | アプリ統合 |
| <strong>サービス管理</strong> | 手動操作: `foundry service start` | 自動化: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| <strong>ポート検出</strong> | CLI 出力を読む | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| <strong>モデルダウンロード</strong> | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| <strong>エラー処理</strong> | 終了コード、stderr | 例外、型付きエラー |
| <strong>自動化</strong> | シェルスクリプト | ネイティブ言語統合 |
| <strong>デプロイ</strong> | エンドユーザー端末に CLI 必須 | C# SDK は自己完結可能（CLI 不要） |

> **重要なポイント:** SDK はサービス起動、キャッシュ確認、未ダウンロードモデルの取得、ロード、エンドポイント検出を数行のコードで完全に管理します。アプリは CLI 出力解析やサブプロセス管理の必要がありません。

---

## ラボ演習

### 演習 1: SDK のインストール

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

インストールの検証：

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

インストールの検証：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

NuGet パッケージが2つあります：

| パッケージ | プラットフォーム | 説明 |
|------------|------------------|------|
| `Microsoft.AI.Foundry.Local` | クロスプラットフォーム | Windows、Linux、macOS で動作 |
| `Microsoft.AI.Foundry.Local.WinML` | Windowsのみ | WinML ハードウェアアクセラレーション追加；プラグイン実行プロバイダー（例：Qualcomm NPU 用 QNN）をダウンロードしインストール |

**Windows セットアップ:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` ファイル編集：

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

> **注意:** Windows では WinML パッケージは基本 SDK + QNN 実行プロバイダーを含むスーパーセットです。Linux/macOS は基本 SDK を使います。条件付き TFM とパッケージ参照により完全なクロスプラットフォーム対応を維持。

プロジェクトルートに `nuget.config` を作成：

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

パッケージリストア：

```bash
dotnet restore
```

</details>

---

### 演習 2: サービス起動とカタログ一覧取得

アプリケーションではまず Foundry Local サービスを起動し、利用可能なモデルを調べます。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# マネージャーを作成してサービスを開始する
manager = FoundryLocalManager()
manager.start_service()

# カタログで利用可能なすべてのモデルを一覧表示する
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - サービス管理メソッド

| メソッド | シグネチャ | 説明 |
|----------|------------|------|
| `is_service_running()` | `() -> bool` | サービスが起動中かを確認 |
| `start_service()` | `() -> None` | Foundry Local サービスを起動 |
| `service_uri` | `@property -> str` | 基本のサービス URI |
| `endpoint` | `@property -> str` | API エンドポイント（サービス URI + `/v1`） |
| `api_key` | `@property -> str` | API キー（環境変数またはデフォルトプレースホルダ） |

#### Python SDK - カタログ管理メソッド

| メソッド | シグネチャ | 説明 |
|----------|------------|------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | カタログ内の全モデル一覧を取得 |
| `refresh_catalog()` | `() -> None` | サービスからカタログをリフレッシュ |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | 特定モデルの情報を取得 |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// マネージャーを作成してサービスを開始する
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// カタログを参照する
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - マネージャーメソッド

| メソッド | シグネチャ | 説明 |
|----------|------------|------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK シングルトンを初期化 |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | シングルトンマネージャーにアクセス |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local Web サービスを起動 |
| `manager.urls` | `string[]` | サービス用基本 URL の配列 |

#### JavaScript SDK - カタログとモデルメソッド

| メソッド | シグネチャ | 説明 |
|----------|------------|------|
| `manager.catalog` | `Catalog` | モデルのカタログにアクセス |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | エイリアスでモデルオブジェクトを取得 |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ はオブジェクト指向アーキテクチャで、`Configuration`、`Catalog`、`Model` オブジェクトを持ちます：

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

#### C# SDK - キークラス

| クラス | 目的 |
|--------|-------|
| `Configuration` | アプリ名、ログレベル、キャッシュディレクトリ、Web サーバーURL 設定 |
| `FoundryLocalManager` | メインエントリポイント - `CreateAsync()` で作成し、`.Instance` でアクセス |
| `Catalog` | カタログ内のモデルの参照、検索、取得 |
| `Model` | 特定モデルを表現 - ダウンロード、ロード、クライアント取得 |

#### C# SDK - マネージャーとカタログメソッド

| メソッド | 説明 |
|----------|------|
| `FoundryLocalManager.CreateAsync(config, logger)` | マネージャー初期化 |
| `FoundryLocalManager.Instance` | シングルトンマネージャー取得 |
| `manager.GetCatalogAsync()` | モデルカタログ取得 |
| `catalog.ListModelsAsync()` | 利用可能なモデルを一覧表示 |
| `catalog.GetModelAsync(alias: "alias")` | 指定エイリアスのモデル取得 |
| `catalog.GetCachedModelsAsync()` | ダウンロード済みモデル一覧 |
| `catalog.GetLoadedModelsAsync()` | 現在ロード中のモデル一覧 |

> **C# アーキテクチャノート:** C# SDK v0.8.0+ の再設計はアプリを <strong>自己完結型</strong> にしました。エンドユーザー端末に Foundry Local CLI は不要で、SDK がモデル管理と推論をネイティブに扱います。

</details>

---

### 演習 3: モデルのダウンロードとロード

SDK はダウンロード（ディスクへの保存）とロード（メモリへの読み込み）を分離。セットアップ時にモデルを事前ダウンロードし、必要に応じてロード可能です。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# オプションA: 手動のステップバイステップ
manager = FoundryLocalManager()
manager.start_service()

# 最初にキャッシュを確認する
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

# オプションB: ワンライナーブートストラップ（推奨）
# エイリアスをコンストラクターに渡す - サービスを開始し、自動的にダウンロードして読み込みます
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - モデル管理メソッド

| メソッド | シグネチャ | 説明 |
|----------|------------|------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | モデルをローカルキャッシュにダウンロード |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | モデルを推論サーバにロード |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | サーバからモデルをアンロード |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | 現在ロード中のモデル一覧 |

#### Python - キャッシュ管理メソッド

| メソッド | シグネチャ | 説明 |
|----------|------------|------|
| `get_cache_location()` | `() -> str` | キャッシュディレクトリのパスを取得 |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | ダウンロード済みモデル一覧 |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// ステップバイステップのアプローチ
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

#### JavaScript - モデルメソッド

| メソッド | シグネチャ | 説明 |
|----------|------------|------|
| `model.isCached` | `boolean` | モデルが既にダウンロード済みか |
| `model.download()` | `() => Promise<void>` | モデルをローカルキャッシュにダウンロード |
| `model.load()` | `() => Promise<void>` | 推論サーバにロード |
| `model.unload()` | `() => Promise<void>` | 推論サーバからアンロード |
| `model.id` | `string` | モデルのユニーク ID |

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

#### C# - モデルメソッド

| メソッド | 説明 |
|----------|------|
| `model.DownloadAsync(progress?)` | 選択したバリアントをダウンロード |
| `model.LoadAsync()` | モデルをメモリにロード |
| `model.UnloadAsync()` | モデルをアンロード |
| `model.SelectVariant(variant)` | 特定バリアント（CPU/GPU/NPU）を選択 |
| `model.SelectedVariant` | 現在選択されているバリアント |
| `model.Variants` | 利用可能な全バリアント |
| `model.GetPathAsync()` | ローカルファイルパスを取得 |
| `model.GetChatClientAsync()` | ネイティブチャットクライアントを取得（OpenAI SDK 不要） |
| `model.GetAudioClientAsync()` | 音声クライアント（音声認識用）を取得 |

</details>

---

### 演習 4: モデルのメタデータを検査する

`FoundryModelInfo` オブジェクトは各モデルの詳細なメタデータを含みます。これらを理解すれば適切なモデル選択が可能です。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# 特定のモデルに関する詳細情報を取得する
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

#### FoundryModelInfo フィールド

| フィールド | 型 | 説明 |
|------------|----|------|
| `alias` | string | 短い名前（例：`phi-3.5-mini`） |
| `id` | string | ユニークなモデル識別子 |
| `version` | string | モデルのバージョン |
| `task` | string | `chat-completions` または `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU、GPU、または NPU |
| `execution_provider` | string | ランタイムバックエンド（CUDA、CPU、QNN、WebGPUなど） |
| `file_size_mb` | int | ディスク上のサイズ（MB） |
| `supports_tool_calling` | bool | 関数やツール呼び出しの対応有無 |
| `publisher` | string | モデルの公開者 |
| `license` | string | ライセンス名 |
| `uri` | string | モデルの URI |
| `prompt_template` | dict/null | プロンプトテンプレート（あれば） |

---

### 演習 5: モデルのライフサイクル管理

フルライフサイクルを実践します：一覧 → ダウンロード → ロード → 使用 → アンロード。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # クイックテスト用の小さなモデル

manager = FoundryLocalManager()
manager.start_service()

# 1. カタログに何があるか確認する
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. すでにダウンロードされているものを確認する
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. モデルをダウンロードする
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. それがキャッシュにあることを確認する
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. ロードする
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. ロードされているものを確認する
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. アンロードする
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

const alias = "qwen2.5-0.5b"; // 素早いテストのための小さなモデル

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. カタログからモデルを取得
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. 必要に応じてダウンロード
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. ロードする
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. アンロードする
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### 演習6: クイックスタートパターン

各言語は、サービスを開始しモデルを読み込むためのショートカットを1回の呼び出しで提供しています。これらはほとんどのアプリケーションにとって<strong>推奨されるパターン</strong>です。

<details>
<summary><h3>🐍 Python - コンストラクタブートストラップ</h3></summary>

```python
from foundry_local import FoundryLocalManager

# コンストラクタにエイリアスを渡します - これですべて処理されます：
# 1. サービスが起動していなければ開始します
# 2. モデルがキャッシュされていなければダウンロードします
# 3. モデルを推論サーバーに読み込みます
manager = FoundryLocalManager("phi-3.5-mini")

# すぐに使用可能です
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap`パラメータ（デフォルト`True`）はこの動作を制御します。手動で制御したい場合は`bootstrap=False`と設定してください:

```python
# マニュアルモード - 自動で何も起こりません
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + カタログ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() がすべてを処理します:
// 1. サービスを開始します
// 2. カタログからモデルを取得します
// 3. 必要に応じてダウンロードし、モデルを読み込みます
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// すぐに使用可能です
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + カタログ</h3></summary>

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

> **C# 注記:** C# SDKは`Configuration`を使用してアプリ名、ログ、キャッシュディレクトリ、さらには特定のWebサーバーポートの固定まで制御します。これにより3つのSDKの中で最も設定が柔軟です。

</details>

---

### 演習7: ネイティブChatClient（OpenAI SDK不要）

JavaScriptとC#のSDKは`createChatClient()`という便利なメソッドを提供し、ネイティブチャットクライアントを返します。OpenAI SDKを別途インストールや設定する必要はありません。

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

// モデルから直接ChatClientを作成 — OpenAIのインポートは不要
const chatClient = model.createChatClient();

// completeChatはOpenAI互換のレスポンスオブジェクトを返します
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// ストリーミングはコールバックパターンを使用します
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient`はツール呼び出しにも対応しています。ツールを第2引数として渡してください:

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

> **どのパターンを使うかの目安:**
> - **`createChatClient()`** — クイックなプロトタイプ作成、依存関係が少なくコードも簡単
> - **OpenAI SDK** — パラメータ制御（temperature、top_p、stopトークンなど）が完全、プロダクション向けに適している

---

### 演習8: モデルのバリアントとハードウェア選択

モデルには異なるハードウェア向けに最適化された複数の<strong>バリアント</strong>があります。SDKは最適なバリアントを自動的に選択しますが、手動で調べて選ぶことも可能です。

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// 利用可能なすべてのバリアントを一覧表示します
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDKはハードウェアに最適なバリアントを自動的に選択します
// 上書きするには、selectVariant()を使用してください：
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

PythonではSDKがハードウェアに基づいて最適なバリアントを自動選択します。選択されたものを確認するには`get_model_info()`を使います:

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

#### NPUバリアントを持つモデル

一部のモデルはNeural Processing Units（Qualcomm Snapdragon、Intel Core Ultra）向けに最適化されたNPUバリアントを持っています:

| モデル | NPUバリアント利用可能 |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **ヒント:** NPU対応ハードウェア上では、SDKは利用可能な場合自動的にNPUバリアントを選択します。コードの変更は不要です。WindowsのC#プロジェクトでは`Microsoft.AI.Foundry.Local.WinML` NuGetパッケージを追加してQNN実行プロバイダーを有効化してください。QNNはWinMLプラグインEPとして提供されます。

---

### 演習9: モデルのアップグレードとカタログの更新

モデルカタログは定期的に更新されます。アップデートの確認と適用にはこれらのメソッドを使用してください。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# 最新のモデルリストを取得するためにカタログを更新します
manager.refresh_catalog()

# キャッシュされたモデルに新しいバージョンがあるか確認します
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

// カタログを更新して最新のモデルリストを取得する
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// 更新後に利用可能なすべてのモデルをリストする
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### 演習10: 推論モデルの使い方

<strong>phi-4-mini-reasoning</strong>モデルはチェーンオブソート推論を含みます。最終回答を生成する前に内側の思考を`<think>...</think>`タグでラップします。これは多段階の論理、数学、問題解決タスクに便利です。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning は約4.6 GBです
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# モデルは思考を <think>...</think> タグで囲みます
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

// チェーン・オブ・ソート思考を解析する
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **推論モデルを使うべき場合:**
> - 数学や論理問題
> - 多段階計画タスク
> - 複雑なコード生成
> - 手順を示すことで精度が向上するタスク
>
> **トレードオフ:** 推論モデルはより多くのトークンを生成し（`<think>`の部分）、処理も遅くなります。単純なQ&Aにはphi-3.5-miniのような標準モデルの方が高速です。

---

### 演習11: エイリアスとハードウェア選択の理解

フルモデルIDの代わりに<strong>エイリアス</strong>（例: `phi-3.5-mini`）を渡すと、SDKが自動で最適なバリアントをハードウェアに合わせて選択します:

| ハードウェア | 選択される実行プロバイダー |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinMLプラグイン経由) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| 任意のデバイス（フォールバック） | `CPUExecutionProvider` または `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# エイリアスはあなたのハードウェアに最適なバリアントに解決されます
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **ヒント:** アプリケーションコードでは必ずエイリアスを使いましょう。ユーザーのマシンにデプロイした際、SDKが最適なバリアントを実行時に選択します — NVIDIAではCUDA、QualcommではQNN、それ以外はCPUなど。

---

### 演習12: C# 設定オプション

C# SDKの`Configuration`クラスは実行時の詳細設定を提供します:

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

| プロパティ | デフォルト | 説明 |
|----------|---------|-------------|
| `AppName` | (必須) | アプリケーションの名前 |
| `LogLevel` | `Information` | ロギングの詳細度 |
| `Web.Urls` | (動的) | Webサーバーの特定ポートを固定 |
| `AppDataDir` | OSデフォルト | アプリデータのベースディレクトリ |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | モデルを保存する場所 |
| `LogsDir` | `{AppDataDir}/logs` | ログを保存する場所 |

---

### 演習13: ブラウザでの使用（JavaScriptのみ）

JavaScript SDKにはブラウザ対応版があります。ブラウザでは手動でCLIからサービスを開始し、ホストURLを指定する必要があります:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// 最初にサービスを手動で開始してください：
//   foundry service start
// 次にCLIの出力からURLを使用します
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// カタログを閲覧してください
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **ブラウザの制限:** ブラウザ版は`startWebService()`を<strong>サポートしていません</strong>。Foundry Localサービスが既に起動している必要があります。

---

## 完全なAPIリファレンス

### Python

| カテゴリ | メソッド | 説明 |
|----------|--------|-------------|
| <strong>初期化</strong> | `FoundryLocalManager(alias?, bootstrap=True)` | マネージャーを作成；オプションでモデルをブートストラップ |
| <strong>サービス</strong> | `is_service_running()` | サービスが稼働中か確認 |
| <strong>サービス</strong> | `start_service()` | サービスを開始 |
| <strong>サービス</strong> | `endpoint` | API エンドポイントURL |
| <strong>サービス</strong> | `api_key` | API キー |
| <strong>カタログ</strong> | `list_catalog_models()` | 全利用可能モデルを一覧表示 |
| <strong>カタログ</strong> | `refresh_catalog()` | カタログを更新 |
| <strong>カタログ</strong> | `get_model_info(alias_or_model_id)` | モデルのメタデータ取得 |
| <strong>キャッシュ</strong> | `get_cache_location()` | キャッシュディレクトリパス取得 |
| <strong>キャッシュ</strong> | `list_cached_models()` | ダウンロード済モデルを一覧表示 |
| <strong>モデル</strong> | `download_model(alias_or_model_id)` | モデルをダウンロード |
| <strong>モデル</strong> | `load_model(alias_or_model_id, ttl=600)` | モデルを読み込み |
| <strong>モデル</strong> | `unload_model(alias_or_model_id)` | モデルをアンロード |
| <strong>モデル</strong> | `list_loaded_models()` | 現在読み込まれているモデル一覧 |
| <strong>モデル</strong> | `is_model_upgradeable(alias_or_model_id)` | 新しいバージョンがあるか確認 |
| <strong>モデル</strong> | `upgrade_model(alias_or_model_id)` | 最新版にアップグレード |
| <strong>サービス</strong> | `httpx_client` | API直接呼び出し用の設定済HTTPXクライアント |

### JavaScript

| カテゴリ | メソッド | 説明 |
|----------|--------|-------------|
| <strong>初期化</strong> | `FoundryLocalManager.create(options)` | SDKシングルトンを初期化 |
| <strong>初期化</strong> | `FoundryLocalManager.instance` | シングルトンマネージャーを取得 |
| <strong>サービス</strong> | `manager.startWebService()` | Webサービスを開始 |
| <strong>サービス</strong> | `manager.urls` | サービスのベースURL配列 |
| <strong>カタログ</strong> | `manager.catalog` | モデルカタログにアクセス |
| <strong>カタログ</strong> | `catalog.getModel(alias)` | エイリアスでモデルオブジェクトを取得（Promise） |
| <strong>モデル</strong> | `model.isCached` | モデルがダウンロード済か |
| <strong>モデル</strong> | `model.download()` | モデルをダウンロード |
| <strong>モデル</strong> | `model.load()` | モデルを読み込み |
| <strong>モデル</strong> | `model.unload()` | モデルをアンロード |
| <strong>モデル</strong> | `model.id` | モデルのユニークID |
| <strong>モデル</strong> | `model.alias` | モデルのエイリアス |
| <strong>モデル</strong> | `model.createChatClient()` | ネイティブチャットクライアントを取得（OpenAI SDK不要） |
| <strong>モデル</strong> | `model.createAudioClient()` | 音声認識クライアントを取得 |
| <strong>モデル</strong> | `model.removeFromCache()` | ローカルキャッシュからモデルを削除 |
| <strong>モデル</strong> | `model.selectVariant(variant)` | 特定のハードウェアバリアントを選択 |
| <strong>モデル</strong> | `model.variants` | このモデルの利用可能バリアント配列 |
| <strong>モデル</strong> | `model.isLoaded()` | モデルが現在読み込まれているか確認 |
| <strong>カタログ</strong> | `catalog.getModels()` | 全利用可能モデルを一覧表示 |
| <strong>カタログ</strong> | `catalog.getCachedModels()` | ダウンロード済モデルを一覧表示 |
| <strong>カタログ</strong> | `catalog.getLoadedModels()` | 現在読み込み中のモデル一覧 |
| <strong>カタログ</strong> | `catalog.updateModels()` | サービスからカタログを更新 |
| <strong>サービス</strong> | `manager.stopWebService()` | Foundry Local Webサービスを停止 |

### C# (v0.8.0+)

| カテゴリ | メソッド | 説明 |
|----------|--------|-------------|
| <strong>初期化</strong> | `FoundryLocalManager.CreateAsync(config, logger)` | マネージャーを初期化 |
| <strong>初期化</strong> | `FoundryLocalManager.Instance` | シングルトンを取得 |
| <strong>カタログ</strong> | `manager.GetCatalogAsync()` | カタログを取得 |
| <strong>カタログ</strong> | `catalog.ListModelsAsync()` | 全モデル一覧 |
| <strong>カタログ</strong> | `catalog.GetModelAsync(alias)` | 特定モデルを取得 |
| <strong>カタログ</strong> | `catalog.GetCachedModelsAsync()` | キャッシュ済モデル一覧 |
| <strong>カタログ</strong> | `catalog.GetLoadedModelsAsync()` | 読み込み済モデル一覧 |
| <strong>モデル</strong> | `model.DownloadAsync(progress?)` | モデルをダウンロード |
| <strong>モデル</strong> | `model.LoadAsync()` | モデルを読み込み |
| <strong>モデル</strong> | `model.UnloadAsync()` | モデルをアンロード |
| <strong>モデル</strong> | `model.SelectVariant(variant)` | ハードウェアバリアントを選択 |
| <strong>モデル</strong> | `model.GetChatClientAsync()` | ネイティブチャットクライアントを取得 |
| <strong>モデル</strong> | `model.GetAudioClientAsync()` | 音声認識クライアントを取得 |
| <strong>モデル</strong> | `model.GetPathAsync()` | ローカルファイルパスを取得 |
| <strong>カタログ</strong> | `catalog.GetModelVariantAsync(alias, variant)` | 特定ハードウェアバリアントを取得 |
| <strong>カタログ</strong> | `catalog.UpdateModelsAsync()` | カタログを更新 |
| <strong>サーバー</strong> | `manager.StartWebServerAsync()` | REST Webサーバーを開始 |
| <strong>サーバー</strong> | `manager.StopWebServerAsync()` | REST Webサーバーを停止 |
| <strong>設定</strong> | `config.ModelCacheDir` | キャッシュディレクトリ |

---

## まとめ

| コンセプト | 学んだこと |
|---------|-----------------|
| **SDKとCLIの違い** | SDKはプログラムからの制御を可能にし、アプリケーションに不可欠 |
| <strong>制御プレーン</strong> | SDKはサービス、モデル、キャッシュを管理 |
| <strong>動的ポート</strong> | Pythonでは`manager.endpoint`、JS/C#では`manager.urls[0]`を必ず使い、ポートをハードコードしない |
| <strong>エイリアス</strong> | エイリアスを使うことで自動的にハードウェア最適なモデルが選択される |
| <strong>クイックスタート</strong> | Python: `FoundryLocalManager(alias)`、JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# 再設計** | v0.8.0以降は自己完結型 - エンドユーザーのマシンにCLI不要 |
| <strong>モデルのライフサイクル</strong> | カタログ → ダウンロード → ロード → 使用 → アンロード |
| **FoundryModelInfo** | 豊富なメタデータ: タスク、デバイス、サイズ、ライセンス、ツール呼び出し対応 |
| **ChatClient** | OpenAI未使用のための `createChatClient()` (JS) / `GetChatClientAsync()` (C#) |
| <strong>バリアント</strong> | モデルにはハードウェア特有のバリアント（CPU、GPU、NPU）があり、自動選択される |
| <strong>アップグレード</strong> | Python: `is_model_upgradeable()` + `upgrade_model()` でモデルを最新に維持 |
| <strong>カタログの更新</strong> | `refresh_catalog()` (Python) / `updateModels()` (JS) で新しいモデルを検出 |

---

## リソース

| リソース | リンク |
|----------|------|
| SDKリファレンス（全言語対応） | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| 推論SDKとの統合 | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK APIリファレンス | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDKサンプル | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local公式サイト | [foundrylocal.ai](https://foundrylocal.ai) |

---

## 次のステップ

[パート3: OpenAIを使ったSDKの利用](part3-sdk-and-apis.md) に進み、SDKをOpenAIクライアントライブラリに接続して最初のチャット完了アプリケーションを構築しましょう。