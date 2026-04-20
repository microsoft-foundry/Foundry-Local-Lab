# Part 7 - Whisper 语音转录示例音频文件

这些 WAV 文件使用 `pyttsx3`（Windows SAPI5 文字转语音）生成，主题围绕 Creative Writer 演示中的 **Zava DIY 产品**。

## 生成示例

```bash
# 从仓库根目录开始 - 需要安装了 pyttsx3 的 .venv
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## 示例文件

| 文件 | 场景 | 时长 |
|------|----------|----------|
| `zava-customer-inquiry.wav` | 客户询问关于 **Zava ProGrip 无线电钻** —— 扭矩、电池寿命、携带箱 | 约 15 秒 |
| `zava-product-review.wav` | 客户评价 **Zava UltraSmooth 室内漆** —— 覆盖率、干燥时间、低 VOC | 约 22 秒 |
| `zava-support-call.wav` | 关于 **Zava TitanLock 工具箱** 的支持电话 —— 更换钥匙、额外抽屉衬垫 | 约 20 秒 |
| `zava-project-planning.wav` | DIY 爱好者规划后院甲板，使用 **Zava EcoBoard 复合甲板材料** 和 BrightBeam 灯具 | 约 25 秒 |
| `zava-workshop-setup.wav` | 讲解完整工作间，使用 **所有五款 Zava 产品** | 约 32 秒 |
| `zava-full-project-walkthrough.wav` | 延长的车库装修讲解，使用 **所有 Zava 产品** （长音频测试，见 [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)） | 约 4 分钟 |

## 注意事项

- WAV 文件已经 <strong>提交</strong> 到仓库（列在 。要创建新的 .wav 文件，请运行上述脚本重新生成新的脚本，或修改以创建新脚本。
- 脚本使用 **Microsoft David**（美式英语）语音，语速 160 WPM，以获得清晰的转录结果。
- 所有场景参考来自 [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) 的产品。