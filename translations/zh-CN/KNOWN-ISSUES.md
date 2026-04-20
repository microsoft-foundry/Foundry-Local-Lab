# 已知问题 — Foundry Local 研讨会

在一台运行 Windows 的 **Snapdragon X Elite (ARM64)** 设备上使用 Foundry Local SDK v0.9.0、CLI v0.8.117 和 .NET SDK 10.0 构建和测试本研讨会时遇到的问题。

> **最后验证时间：** 2026-03-11

---

## 1. Snapdragon X Elite CPU 未被 ONNX Runtime 识别

**状态：** 未解决  
**严重性：** 警告（非阻塞）  
**组件：** ONNX Runtime / cpuinfo  
**复现步骤：** 每次在 Snapdragon X Elite 硬件上启动 Foundry Local 服务时

每次启动 Foundry Local 服务时，会出现两个警告：

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**影响：** 这些警告是表面问题——推理可以正常进行。但它们每次运行都会出现，可能会让研讨会参与者感到困惑。需要更新 ONNX Runtime 的 cpuinfo 库以识别高通 Oryon CPU 核心。

**预期：** Snapdragon X Elite 应该被识别为受支持的 ARM64 CPU，且不应发出错误级别消息。

---

## 2. SingleAgent 首次运行时出现 NullReferenceException

**状态：** 开放（间歇性出现）  
**严重性：** 严重（崩溃）  
**组件：** Foundry Local C# SDK + Microsoft Agent Framework  
**复现步骤：** 运行 `dotnet run agent` — 模型加载后立即崩溃

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**背景：** 第37行调用了 `model.IsCachedAsync(default)`。崩溃发生在通过 `foundry service stop` 新启动代理后的首次运行。后续使用相同代码的运行均成功。

**影响：** 间歇出现——提示 SDK 服务初始化或目录查询存在竞态条件。`GetModelAsync()` 调用可能在服务完全准备好之前返回。

**预期：** `GetModelAsync()` 应阻塞直到服务准备完成，或在服务未完成初始化时返回清晰的错误信息。

---

## 3. C# SDK 需要显式指定 RuntimeIdentifier

**状态：** 开放 — 跟踪于 [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**严重性：** 文档缺失  
**组件：** `Microsoft.AI.Foundry.Local` NuGet 包  
**复现步骤：** 创建 .NET 8+ 项目但 `.csproj` 中不包含 `<RuntimeIdentifier>`

构建失败，错误如下：

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**根因：** 需要指定 RID 是预期行为——SDK 中包含本地二进制文件（通过 P/Invoke 调用 `Microsoft.AI.Foundry.Local.Core` 和 ONNX Runtime），因此 .NET 需要知道加载哪个平台特定库。

MS Learn 文档（[如何使用本地聊天完成功能](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)）中对此有说明，运行指令示例为：

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

然而，用户每次都必须记得加上 `-r` 参数，这非常容易遗忘。

**解决方案：** 在 `.csproj` 中添加自动检测回退，这样运行 `dotnet run` 时无需任何额外参数：

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` 是 MSBuild 内置属性，会自动解析为宿主机的 RID。SDK 自己的测试项目已经采用了这一模式。若仍提供 `-r` 参数，则优先使用该参数。

> **注意：** 研讨会中的 `.csproj` 已包含该回退，因此在任何平台上运行 `dotnet run` 都可开箱即用。

**预期：** MS Learn 文档中的 `.csproj` 模板应默认包含自动检测模式，避免用户忘记 `-r` 参数。

---

## 4. JavaScript Whisper — 音频转录返回空或二进制输出

**状态：** 开放（回归——比最初报告更糟）  
**严重性：** 重要  
**组件：** JavaScript Whisper 实现（`foundry-local-whisper.mjs`）/ `model.createAudioClient()`  
**复现步骤：** 运行 `node foundry-local-whisper.mjs` — 所有音频文件返回空或二进制输出，而非文本转录内容

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

最初只有第5个音频文件返回为空；截至 v0.9.x，所有5个文件均返回单字节（`\ufffd`），未转录为文本。使用 OpenAI SDK 的 Python Whisper 实现能正确转录这些文件。

**预期：** `createAudioClient()` 应输出与 Python 和 C# 实现一致的文本转录结果。

---

## 5. C# SDK 仅发布 net8.0 — 无官方 .NET 9 或 .NET 10 目标

**状态：** 开放  
**严重性：** 文档缺失  
**组件：** `Microsoft.AI.Foundry.Local` NuGet 包 v0.9.0  
**安装命令：** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet 仅包含单一目标框架：

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

无 `net9.0` 或 `net10.0` TFM。相比之下，伴随包 `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) 提供了 `net8.0`、`net9.0`、`net10.0`、`net472` 和 `netstandard2.0`。

### 兼容性测试

| 目标框架 | 构建 | 运行 | 备注 |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | 官方支持 |
| net9.0 | ✅ | ✅ | 通过向前兼容构建 — 用于研讨会示例 |
| net10.0 | ✅ | ✅ | 通过 .NET 10.0.3 运行时向前兼容构建运行 |

net8.0 程序集可以通过 .NET 的向前兼容机制在新版运行时加载，因此构建成功。但这并未由 SDK 团队记录或测试。

### 为什么示例使用 net9.0 目标

1. **.NET 9 是最新稳定版本**——大多数研讨会参与者会安装此版本  
2. <strong>向前兼容有效</strong>——NuGet 包中 net8.0 程序集在 .NET 9 运行时无问题  
3. **.NET 10 (预览/候选版)** 对于面向所有用户的研讨会来说太新

**预期：** 未来 SDK 应考虑添加 `net9.0` 和 `net10.0` 目标框架，与 `Microsoft.Agents.AI.OpenAI` 保持一致，并为新运行时提供经过验证的支持。

---

## 6. JavaScript ChatClient 流式接口使用回调，不是异步迭代器

**状态：** 开放  
**严重性：** 文档缺失  
**组件：** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` 返回的 `ChatClient` 提供了 `completeStreamingChat()` 方法，但它采用<strong>回调模式</strong>，而非返回异步可迭代对象：

```javascript
// ❌ 这不起作用 — 抛出“stream 不是异步可迭代的”
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ 正确的模式 — 传入回调函数
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**影响：** 熟悉 OpenAI SDK 异步迭代模式（`for await`）的开发者会遇到困惑的错误。回调必须是有效函数，否则 SDK 会抛出“回调必须是有效函数”错误。

**预期：** 在 SDK 参考文档中说明回调模式。或者支持异步迭代模式，以保持与 OpenAI SDK 的一致性。

---

## 环境详情

| 组件 | 版本 |
|-----------|---------|
| 操作系统 | Windows 11 ARM64 |
| 硬件 | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |