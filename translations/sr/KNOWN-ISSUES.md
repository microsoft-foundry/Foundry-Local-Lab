# Познати проблеми — Foundry Local радионица

Проблеми на које се наилази током изградње и тестирања ове радионице на уређају **Snapdragon X Elite (ARM64)** са оперативним системом Windows, уз Foundry Local SDK v0.9.0, CLI v0.8.117 и .NET SDK 10.0.

> **Последња валидација:** 2026-03-11

---

## 1. Snapdragon X Elite CPU није препознат од стране ONNX Runtime-а

**Статус:** Отворено  
**Тежина:** Упозорење (не омета рад)  
**Компонента:** ONNX Runtime / cpuinfo  
**Репродукција:** Свако покретање Foundry Local сервиса на Snapdragon X Elite хардверу

Свако покретање Foundry Local сервиса резултује са два упозорења:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Утицај:** Упозорења су козметичка — инференца ради исправно. Међутим, појављују се при сваком покретању и могу збуњивати учеснике радионице. ONNX Runtime cpuinfo библиотека мора бити ажурирана да препозна Qualcomm Oryon CPU језгре.

**Очекивано:** Snapdragon X Elite треба да буде препознат као подржани ARM64 процесор без приказивања порука о грешци.

---

## 2. SingleAgent NullReferenceException при првом покретању

**Статус:** Отворено (повремено)  
**Тежина:** Критично (приказ грешке)  
**Компонента:** Foundry Local C# SDK + Microsoft Agent Framework  
**Репродукција:** Покренути `dotnet run agent` — пад одмах након учитавања модела

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Контекст:** Линија 37 позива `model.IsCachedAsync(default)`. Пад се десио при првом покретању агента након свежег `foundry service stop`. Накнадна покретања са истим кодом су успевала.

**Утицај:** Повремено — указује на тркачку (race) условност приликом иницијализације сервиса или упита у каталогу SDK-а. Позив `GetModelAsync()` може вратити пре него што сервис буде потпуно спреман.

**Очекивано:** `GetModelAsync()` треба да блокира док сервис није спреман или да врати јасну поруку о грешци ако сервис још није завршио иницијализацију.

---

## 3. C# SDK захтева експлицитан RuntimeIdentifier

**Статус:** Отворено — праћено у [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Тежина:** Пропуст у документацији  
**Компонента:** NuGet пакет `Microsoft.AI.Foundry.Local`  
**Репродукција:** Креирати .NET 8+ пројекат без `<RuntimeIdentifier>` у `.csproj`

Градња не успева са:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Узрок:** Захтев за RID је очекиван — SDK испоручује нативне бинарне датотеке (P/Invoke у `Microsoft.AI.Foundry.Local.Core` и ONNX Runtime), тако да .NET мора знати који платформски специфичан библиотечки фајл треба да буде решен.

Ово је документовано на MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)) где упутства за покретање приказују:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Међутим, корисници морају сваки пут да се сећају `-r` параметра, што је лако заборавити.

**Решење:** Додајте аутоматско препознавање као резерву у ваш `.csproj` да `dotnet run` ради без параметара:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` је уграђена MSBuild својина која аутоматски резолуира RID домаћинске машине. Он сам SDK тест пројекти већ користе овај образац. Експлицитни `-r` параметри су и даље прихваћени ако су наведени.

> **Напомена:** `.csproj` за радионицу укључује овај fallback тако да `dotnet run` ради без додатних параметара на било којој платформи.

**Очекивано:** `.csproj` шаблон у MS Learn документацији треба да укључује овај образац аутоматског препознавања како корисници не би морали да памте `-r` параметар.

---

## 4. JavaScript Whisper — Аудио транскрипција враћа празан/бинарни излаз

**Статус:** Отворено (регресија — погоршано од почетног извештаја)  
**Тежина:** Велики проблем  
**Компонента:** JavaScript Whisper имплементација (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Репродукција:** Покренути `node foundry-local-whisper.mjs` — сви аудио фајлови враћају празан или бинарни излаз уместо текстуалне транскрипције

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
У почетку је само 5. аудио фајл враћао празан излаз; од верзије v0.9.x свих 5 фајлова враћа један бајт (`\ufffd`) уместо транскрибованог текста. Python Whisper имплементација са OpenAI SDK-ом исправно транскрибује исте фајлове.

**Очекивано:** `createAudioClient()` треба да врати текстуалну транскрипцију која одговара Python/C# имплементацијама.

---

## 5. C# SDK испоручује само net8.0 — нема званично .NET 9 или .NET 10 таргет

**Статус:** Отворено  
**Тежина:** Пропуст у документацији  
**Компонента:** NuGet пакет `Microsoft.AI.Foundry.Local` v0.9.0  
**Команда за инсталацију:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet пакет испоручује само једну таргет платформу:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Нема укључених `net9.0` или `net10.0` TFM (target framework moniker). За разлику од тога, пратећи пакет `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) испоручује `net8.0`, `net9.0`, `net10.0`, `net472` и `netstandard2.0`.

### Тестирање компатибилности

| Таргет фрејмворк | Градња | Покретање | Напомене |
|-----------------|--------|-----------|----------|
| net8.0 | ✅ | ✅ | Званично подржано |
| net9.0 | ✅ | ✅ | Гради се кроз forward-compat — коришћено у примерима радионице |
| net10.0 | ✅ | ✅ | Гради и покреће се кроз forward-compat са .NET 10.0.3 runtime-ом |

net8.0 асемблери се учитавају на новијим runtime окружењима кроз .NET forward-compat механизам, тако да градња пролази. Међутим, ово није документовано нити тестиранo од стране SDK тима.

### Зашто примери таргетују net9.0

1. **.NET 9 је најновија стабилна верзија** — већина учесника радионице ће имати инсталирану ову верзију  
2. **Forward compatibility функционише** — net8.0 асемблери у NuGet пакету раде на .NET 9 runtime-у без проблема  
3. **.NET 10 (преглед/RC)** је прескор за таргет у радионици која треба да ради за све

**Очекивано:** Будућа издања SDK-а треба да размотре додавање `net9.0` и `net10.0` TFM-а уз `net8.0` како би се ускладили са шаблоном коришћеним у `Microsoft.Agents.AI.OpenAI` и пружила валидирана подршка за новије runtime-ове.

---

## 6. JavaScript ChatClient стримовање користи callback, не async iterator

**Статус:** Отворено  
**Тежина:** Пропуст у документацији  
**Компонента:** JavaScript `foundry-local-sdk` v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` враћен из `model.createChatClient()` пружа метод `completeStreamingChat()`, али користи **callback патерн** уместо да враћа async iterable:

```javascript
// ❌ Ово НЕ ради — баца грешку "stream није асинхрони итерабл"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Правилан образац — проследити callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Утицај:** Програмери који су упознати са async iteration патерном OpenAI SDK-а (`for await`) ће имати збуњујуће грешке. Callback мора бити важећа функција или SDK баца грешку „Callback must be a valid function.“

**Очекивано:** Документовати callback патерн у SDK референци. Алтернативно, подржати async iterable патерн за усклађеност са OpenAI SDK-ом.

---

## Детаљи о окружењу

| Компонента | Верзија |
|------------|---------|
| OS | Windows 11 ARM64 |
| Хардвер | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |