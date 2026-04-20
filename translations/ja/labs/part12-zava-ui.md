![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 12: Zava Creative WriterのWeb UI構築

> **目標:** Zava Creative Writerにブラウザベースのフロントエンドを追加し、マルチエージェントパイプラインの実行状況をリアルタイムで確認できるようにします。エージェントのステータスバッジと記事テキストのストリーミングがライブで表示され、すべてが単一のローカルWebサーバーから提供されます。

[Part 7](part7-zava-creative-writer.md)では、Zava Creative Writerを<strong>CLIアプリケーション</strong>（JavaScript、C#）および<strong>ヘッドレスAPI</strong>（Python）として探索しました。このラボでは、共有の<strong>バニラHTML/CSS/JavaScript</strong>フロントエンドをそれぞれのバックエンドに接続し、ユーザーがターミナルではなくブラウザを介してパイプラインと対話できるようにします。

---

## 学ぶこと

| 目的 | 説明 |
|-----------|-------------|
| バックエンドから静的ファイルを提供する | APIルートと並行してHTML/CSS/JSディレクトリをマウント |
| ブラウザでストリーミングNDJSONを消費する | Fetch APIと`ReadableStream`を使って改行区切りJSONを読み取る |
| 統一されたストリーミングプロトコル | Python、JavaScript、C#バックエンドが同一のメッセージ形式を出力することを保証 |
| プログレッシブUI更新 | エージェントのステータスバッジを更新し、記事テキストをトークン単位でストリーム表示 |
| CLIアプリにHTTPレイヤーを追加 | 既存のオーケストレーションロジックをExpressスタイルのサーバー（JS）またはASP.NET CoreミニマルAPI（C#）でラップ |

---

## アーキテクチャ

UIは単一の静的ファイルセット（`index.html`、`style.css`、`app.js`）で、3つのバックエンドすべてで共有されます。各バックエンドは同じ二つのルートを公開します：

![Zava UIアーキテクチャ — 共通フロントエンドと3つのバックエンド](../../../images/part12-architecture.svg)

| ルート | メソッド | 目的 |
|-------|--------|---------|
| `/` | GET | 静的UIを提供 |
| `/api/article` | POST | マルチエージェントパイプラインを実行しNDJSONをストリーミング |

フロントエンドはJSONボディを送り、改行区切りJSONのストリームとしてレスポンスを読み取ります。各メッセージには`type`フィールドがあり、UIはそれに基づいて正しいパネルを更新します：

| メッセージタイプ | 意味 |
|-------------|---------|
| `message` | 状態更新（例："Starting researcher agent task..."） |
| `researcher` | リサーチ結果が準備完了 |
| `marketing` | 商品検索結果が準備完了 |
| `writer` | ライター開始または完了（`{ start: true }`または`{ complete: true }`を含む） |
| `partial` | ライターからの単一ストリームトークン（`{ text: "..." }`を含む） |
| `editor` | エディタの判定結果が準備完了 |
| `error` | エラー発生 |

![ブラウザ内のメッセージタイプルーティング](../../../images/part12-message-types.svg)

![ストリーミングシーケンス — ブラウザからバックエンドへの通信](../../../images/part12-streaming-sequence.svg)

---

## 前提条件

- [Part 7: Zava Creative Writer](part7-zava-creative-writer.md)を完了していること
- Foundry Local CLIがインストールされ、`phi-3.5-mini`モデルがダウンロード済み
- 最新のWebブラウザ（Chrome, Edge, Firefox, Safari）

---

## 共有されるUI

バックエンドのコードに触れる前に、3つの言語トラックすべてで使用されるフロントエンドを確認しましょう。ファイルは`zava-creative-writer-local/ui/`にあります：

| ファイル | 目的 |
|------|---------|
| `index.html` | ページレイアウト：入力フォーム、エージェントステータスバッジ、記事出力領域、折り畳み可能な詳細パネル |
| `style.css` | 最小限のスタイリング。ステータスバッジの色状態（待機中、実行中、完了、エラー） |
| `app.js` | Fetch呼び出し、`ReadableStream`によるラインリーダー、DOM更新ロジック |

> **ヒント:** ブラウザで直接`index.html`を開きレイアウトを確認できます。バックエンドが無いためまだ機能しませんが構造は見えます。

### ストリームリーダーの仕組み

`app.js`の重要な関数はレスポンスの本文をチャンクごとに読み、新しい行区切りで分割します：

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
    buffer = lines.pop(); // 不完全な末尾の行を保持する

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

解析された各メッセージは`handleMessage()`に渡され、`msg.type`に基づいて該当するDOM要素が更新されます。

---

## 演習

### 演習1：PythonバックエンドをUIで実行する

Python（FastAPI）バージョンにはすでにストリーミングAPIエンドポイントがあります。変更点は`ui/`フォルダーを静的ファイルとしてマウントすることだけです。

**1.1** Python APIディレクトリに移動し依存関係をインストール：

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** サーバーを起動：

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** ブラウザで`http://localhost:8000`を開くと、3つのテキストフィールドと「Generate Article」ボタンのあるZava Creative Writer UIが表示されます。

**1.4** デフォルト値のまま「Generate Article」をクリックします。各エージェントの作業完了に応じてステータスバッジは「Waiting」から「Running」、さらに「Done」へ変わり、記事テキストがトークンごとに出力パネルにストリーム表示されます。

> **トラブルシューティング:** ページがUIでなくJSONレスポンスを表示する場合は、静的ファイルマウントを追加した最新の`main.py`を実行しているか確認してください。`/api/article`エンドポイントは元のパスで動作し続け、静的ファイルマウントはそれ以外のルートでUIを提供します。

**動作原理:** 更新された`main.py`はファイル末尾に1行追加されています：

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

これにより`zava-creative-writer-local/ui/`のすべてのファイルを静的資産として提供し、`index.html`をデフォルトドキュメントとして使用します。`/api/article`のPOSTルートは静的マウントより先に登録されているため優先されます。

---

### 演習2：JavaScriptバージョンにWebサーバーを追加する

JavaScriptバージョンは現在CLIアプリケーション（`main.mjs`）です。新しいファイル`server.mjs`は同じエージェントモジュールをHTTPサーバーの背後にラップし、共有UIを提供します。

**2.1** JavaScriptディレクトリに移動し依存関係をインストール：

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Webサーバーを起動：

```bash
node server.mjs
```

```powershell
node server.mjs
```

以下が表示されます：

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** ブラウザで`http://localhost:3000`を開き「Generate Article」をクリック。JavaScriptバックエンドに対して同様にUIが動作します。

**コードを確認:** `server.mjs`を開き主要なパターンを確認しましょう：

- <strong>静的ファイル提供</strong>はNode.js組み込みの`http`、`fs`、`path`モジュールを使い、外部フレームワークは不要
- <strong>パストラバーサル保護</strong>で要求されたパスを正規化し`ui/`ディレクトリ外に出ないことを検証
- <strong>NDJSONストリーミング</strong>はオブジェクトをシリアライズし内部改行除去、行末に改行を追加する`sendLine()`ヘルパーを使用
- <strong>エージェントオーケストレーション</strong>は既存の`researcher.mjs`、`product.mjs`、`writer.mjs`、`editor.mjs`モジュールを変更せずに再利用

<details>
<summary>server.mjsの主要抜粋</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// 研究者
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// 作家（ストリーミング）
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### 演習3：C#版にミニマルAPIを追加する

C#バージョンは現在コンソールアプリケーションです。新規プロジェクト`csharp-web`はASP.NET CoreミニマルAPIを用いて同じパイプラインをWebサービスとして公開します。

**3.1** C# Webプロジェクトに移動しパッケージを復元：

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Webサーバーを起動：

```bash
dotnet run
```

```powershell
dotnet run
```

以下が表示されます：

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** ブラウザで`http://localhost:5000`を開き「Generate Article」をクリック。

**コードを確認:** `csharp-web`ディレクトリの`Program.cs`を開き次を確認：

- プロジェクトファイルは`Microsoft.NET.Sdk.Web`を使用し、ASP.NET Coreサポートを追加
- `UseDefaultFiles`と`UseStaticFiles`で共有`ui/`ディレクトリから静的ファイルを提供
- `/api/article`エンドポイントは`HttpContext.Response`に直接NDJSON行を書き込み、行ごとにフラッシュしてリアルタイムストリーミング
- エージェントロジック全般（`RunResearcher`、`RunProductSearch`、`RunEditor`、`BuildWriterMessages`）はコンソール版と同じ

<details>
<summary>csharp-web/Program.csの主要抜粋</summary>

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

### 演習4：エージェントステータスバッジを調べる

UIが動作したら、フロントエンドがステータスバッジをどのように更新しているかを見てみましょう。

**4.1** エディターで`zava-creative-writer-local/ui/app.js`を開く。

**4.2** `handleMessage()`関数を探します。メッセージタイプをどのようにDOM更新にマッピングしているかに注目：

| メッセージタイプ | UI動作 |
|-------------|-----------|
| `message`で"researcher"を含む | Researcherバッジを「Running」に設定 |
| `researcher` | Researcherバッジを「Done」に設定しリサーチ結果パネルを更新 |
| `marketing` | Product Searchバッジを「Done」に設定し商品マッチパネルを更新 |
| `writer`で`data.start`あり | Writerバッジを「Running」に設定し記事出力をクリア |
| `partial` | 記事出力にトークンテキストを追加 |
| `writer`で`data.complete`あり | Writerバッジを「Done」に設定 |
| `editor` | Editorバッジを「Done」に設定し編集者フィードバックパネルを更新 |

**4.3** 記事下部の折り畳み可能な「Research Results」「Product Matches」「Editor Feedback」パネルを開き、各エージェントが生成した生のJSONを確認。

---

### 演習5：UIのカスタマイズ（発展）

次のいずれか、または複数の拡張を試してください：

**5.1 単語数カウントを追加。** ライター終了後、出力パネルの下に記事の単語数を表示。`handleMessage`内で`type === "writer"`かつ`data.complete`がtrueのときに計算：

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 再試行指標の追加。** エディタからの修正要求時にパイプラインが再実行される。ステータスパネルに「Revision 1」や「Revision 2」バナーを表示。`message`タイプで"Revision"を含むメッセージを監視し、新しいDOM要素を更新。

**5.3 ダークモード。** トグルボタンと`<body>`に`.dark`クラスを追加。`style.css`の`body.dark`セレクターで背景色、文字色、パネル色を上書き。

---

## まとめ

| 実施内容 | 方法 |
|-------------|-----|
| PythonバックエンドからUIを提供 | FastAPIの`StaticFiles`で`ui/`フォルダーをマウント |
| JavaScript版にHTTPサーバー追加 | Node.js組み込み`http`モジュールで`server.mjs`を作成 |
| C#版にWeb API追加 | ASP.NET CoreミニマルAPIで新プロジェクト`csharp-web`を作成 |
| ブラウザでストリーミングNDJSONを消費 | `fetch()`と`ReadableStream`で行単位のJSON解析を使用 |
| リアルタイムにUIを更新 | メッセージタイプに応じたDOM更新（バッジ、テキスト、折り畳みパネル） |

---

## 重要ポイント

1. <strong>共有の静的フロントエンド</strong>は同一のストリーミングプロトコル対応バックエンドであればどれでも機能し、OpenAI互換APIパターンの価値を裏付ける。
2. <strong>改行区切りJSON（NDJSON）</strong>はブラウザの`ReadableStream` APIにネイティブ対応するシンプルなストリーミング形式。
3. <strong>Python版</strong>はFastAPIエンドポイントが既にあるため変更最小限。JavaScriptとC#は軽量HTTPラッパーが必要。
4. UIを<strong>バニラHTML/CSS/JSに保つ</strong>ことでビルドツールやフレームワークの依存を避け、ワークショップ参加者の負担を軽減。
5. エージェントモジュール（Researcher、Product、Writer、Editor）は内容を変えずに再利用。変更は輸送層のみ。

---

## 参考資料

| リソース | リンク |
|----------|------|
| MDN: Readable Streamsの使用法 | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPIでの静的ファイル | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core静的ファイル | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON仕様 | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

続いて[Part 13: Workshop Complete](part13-workshop-complete.md)でこのワークショップ全体のまとめを確認してください。

---
[← パート11：ツール呼び出し](part11-tool-calling.md) | [パート13：ワークショップ完了 →](part13-workshop-complete.md)