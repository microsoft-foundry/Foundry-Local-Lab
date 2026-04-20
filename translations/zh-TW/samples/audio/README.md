# 第7部分的範例音訊檔案 - Whisper 語音轉錄

這些 WAV 檔案是使用 `pyttsx3`（Windows SAPI5 文字轉語音）產生，主題圍繞 Creative Writer 範例中的 **Zava DIY 產品**。

## 產生範例檔案

```bash
# 從倉庫根目錄執行 - 需要安裝 pyttsx3 的 .venv
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## 範例檔案

| 檔案 | 情境 | 時長 |
|------|----------|----------|
| `zava-customer-inquiry.wav` | 客戶詢問 **Zava ProGrip 無線電鑽** - 扭力、電池壽命、攜帶盒 | 約15秒 |
| `zava-product-review.wav` | 客戶評論 **Zava UltraSmooth 室內油漆** - 覆蓋率、乾燥時間、低揮發性有機化合物 | 約22秒 |
| `zava-support-call.wav` | 支援電話關於 **Zava TitanLock 工具箱** - 替換鑰匙、額外抽屜襯墊 | 約20秒 |
| `zava-project-planning.wav` | DIY者規劃後院甲板，使用 **Zava EcoBoard 複合甲板** 與 BrightBeam 照明 | 約25秒 |
| `zava-workshop-setup.wav` | 使用 **五款 Zava 產品** 完整工作坊導覽 | 約32秒 |
| `zava-full-project-walkthrough.wav` | 使用 **所有 Zava 產品** 的車庫翻新完整導覽（長音訊測試，詳見 [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)） | 約4分鐘 |

## 備註

- WAV 檔已<strong>提交</strong>至儲存庫中（列於 `. 為產生新的 .wav 檔，請執行上方腳本以重新產生腳本或修改以建立新腳本。
- 腳本使用 Microsoft David（美國英語）聲音，速率為每分鐘160字，以取得清晰的轉錄結果。
- 所有情境均參考 [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) 中的產品。