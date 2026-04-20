# Part 7 範例音訊檔案 - Whisper 語音轉錄

這些 WAV 檔案是使用 `pyttsx3`（Windows SAPI5 文字轉語音）生成，主題圍繞 Creative Writer 示範中的<strong>Zava DIY 產品</strong>。

## 產生範例

```bash
# 從倉庫根目錄 - 需要安裝了 pyttsx3 的 .venv
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## 範例檔案

| 檔案 | 情境 | 時長 |
|------|----------|----------|
| `zava-customer-inquiry.wav` | 顧客詢問 **Zava ProGrip 無繩電鑽**－扭力、電池壽命、攜帶盒 | 約 15 秒 |
| `zava-product-review.wav` | 顧客評論 **Zava UltraSmooth 室內油漆**－覆蓋性、乾燥時間、低揮發性有機化合物 | 約 22 秒 |
| `zava-support-call.wav` | 關於 **Zava TitanLock 工具箱** 的客服電話－替換鎖匙、額外抽屜襯套 | 約 20 秒 |
| `zava-project-planning.wav` | DIY 愛好者計劃後院露台，使用 <strong>Zava EcoBoard 複合材料甲板</strong>及 BrightBeam 燈具 | 約 25 秒 |
| `zava-workshop-setup.wav` | 完整工作間導覽，使用 **所有五款 Zava 產品** | 約 32 秒 |
| `zava-full-project-walkthrough.wav` | 延伸車庫翻新導覽，使用 **所有 Zava 產品**（長音訊測試請參考 [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)） | 約 4 分鐘 |

## 備註

- WAV 檔案已<strong>提交</strong>到倉庫（列於 `. 為建立新的 .wav 檔案，請執行上述指令碼以重新生成新的腳本或修改以創建新腳本。
- 指令碼使用 **Microsoft David**（美式英語）語音，語速為 160 字/分鐘，以獲得清晰的轉錄結果。
- 所有情境均引用自 [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) 的產品。