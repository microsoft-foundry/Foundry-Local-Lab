# パート7用サンプル音声ファイル - Whisper音声文字起こし

これらのWAVファイルは `pyttsx3`（Windows SAPI5テキスト読み上げ）を使用して生成されており、Creative Writerデモの<strong>Zava DIY製品</strong>をテーマにしています。

## サンプルの生成

```bash
# リポジトリのルートから - pyttsx3がインストールされた.venvが必要です
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## サンプルファイル

| ファイル | シナリオ | 長さ |
|------|----------|----------|
| `zava-customer-inquiry.wav` | 顧客が<strong>Zava ProGrip コードレスドリル</strong>のトルク、バッテリー寿命、キャリングケースについて問い合わせ | 約15秒 |
| `zava-product-review.wav` | 顧客が<strong>Zava UltraSmooth 室内用塗料</strong>のカバー力、乾燥時間、低VOCについてレビュー | 約22秒 |
| `zava-support-call.wav` | <strong>Zava TitanLock 工具箱</strong>の交換用キー、追加の引き出しライナーに関するサポートコール | 約20秒 |
| `zava-project-planning.wav` | DIY愛好者が<strong>Zava EcoBoard コンポジットデッキ材</strong>とBrightBeamライトを使った裏庭デッキ計画 | 約25秒 |
| `zava-workshop-setup.wav` | <strong>5つのZava製品すべて</strong>を使った完全なワークショップの案内 | 約32秒 |
| `zava-full-project-walkthrough.wav` | <strong>すべてのZava製品</strong>を使ったガレージ改装の長時間ウォークスルー（長時間音声テスト用、[Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)参照） | 約4分 |

## 注意事項

- WAVファイルはリポジトリに<strong>コミット</strong>されています（一覧は `. ... 。新しい.wavファイルを作成するには、上記スクリプトを実行して新しいスクリプトを再生成するか、変更してください）。
- スクリプトは文字起こし結果をクリアにするため、160WPMの<strong>Microsoft David</strong>（米国英語）音声を使用しています。
- すべてのシナリオは [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) の製品を参照しています。