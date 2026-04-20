# Известни проблеми — Foundry Local Workshop

Проблеми, срещнати при изграждането и тестването на този workshop на устройство **Snapdragon X Elite (ARM64)** с Windows, с Foundry Local SDK v0.9.0, CLI v0.8.117 и .NET SDK 10.0.

> **Последна проверка:** 2026-03-11

---

## 1. Процесор Snapdragon X Elite не се разпознава от ONNX Runtime

**Статус:** Отворен  
**Тежест:** Предупреждение (непречещо)  
**Компонент:** ONNX Runtime / cpuinfo  
**Възпроизвеждане:** Всеки старт на Foundry Local услуга на хардуер Snapdragon X Elite

Всеки път при стартиране на Foundry Local услугата се излъчват две предупреждения:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Въздействие:** Предупрежденията са само козметични — инференцията работи правилно. Въпреки това се появяват при всяко стартиране и могат да объркат участниците в workshop-а. Библиотеката ONNX Runtime cpuinfo трябва да бъде обновена, за да разпознава ядрата Qualcomm Oryon CPU.

**Очаквано:** Snapdragon X Elite трябва да се разпознава като поддържан ARM64 процесор без да се изпращат съобщения с ниво на грешка.

---

## 2. SingleAgent NullReferenceException при първо стартиране

**Статус:** Отворен (случаен)  
**Тежест:** Критичен (срив)  
**Компонент:** Foundry Local C# SDK + Microsoft Agent Framework  
**Възпроизвеждане:** Изпълнение на `dotnet run agent` — срив веднага след зареждане на модела

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Контекст:** Ред 37 извиква `model.IsCachedAsync(default)`. Сривът е възникнал при първото стартиране на агента след чисто `foundry service stop`. Последващите стартирания със същия код са успешни.

**Въздействие:** Случаен — подсказва за състезателно условие в инициализацията на услугата в SDK или запитването към каталога. Извикването на `GetModelAsync()` може да се върне преди услугата да е напълно готова.

**Очаквано:** `GetModelAsync()` трябва или да блокира, докато услугата е готова, или да връща ясно съобщение за грешка, ако услугата не е приключила с инициализацията.

---

## 3. C# SDK изисква изрично RuntimeIdentifier

**Статус:** Отворен — следен в [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Тежест:** Липса в документацията  
**Компонент:** NuGet пакет `Microsoft.AI.Foundry.Local`  
**Възпроизвеждане:** Създаване на .NET 8+ проект без `<RuntimeIdentifier>` в `.csproj`

Сграждането се проваля със съобщение:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Основна причина:** Изискването за RID е очаквано — SDK доставя native бинарни файлове (P/Invoke към `Microsoft.AI.Foundry.Local.Core` и ONNX Runtime), затова .NET трябва да знае коя платформа-специфична библиотека да резолвира.

Това е документирано в MS Learn ([Как да използваме native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), където в инструкциите за стартиране се показва:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Потребителите трябва обаче винаги да помнят флага `-r`, който лесно може да бъде забравен.

**Заобиколно решение:** Добавете автоматично откриваща резервна стойност в `.csproj`, за да работи `dotnet run` без флагове:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` е вграден MSBuild property, който автоматично резолвира RID на хост машината. Тест проектите на SDK вече използват този подход. Явни `-r` флагове все още се уважават при подаване.

> **Забележка:** Workshop `.csproj` въвежда тази резервна стойност, така че `dotnet run` да работи без допълнителни настройки на всяка платформа.

**Очаквано:** Шаблонът `.csproj` в MS Learn документацията трябва да включва този автоматичен патърн, за да не е необходимо потребителите да помнят флага `-r`.

---

## 4. JavaScript Whisper — Аудио транскрипция връща празен/бинарен резултат

**Статус:** Отворен (регресия — влошено състояние след първоначалния доклад)  
**Тежест:** Значителен  
**Компонент:** JavaScript Whisper имплементация (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Възпроизвеждане:** Изпълнение на `node foundry-local-whisper.mjs` — всички аудио файлове връщат празен или бинарен резултат вместо текстова транскрипция

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Първоначално само 5-тият аудио файл връщаше празен резултат; към v0.9.x всички 5 аудио файла връщат един байт (`\ufffd`) вместо транскрибирания текст. Python Whisper имплементацията с OpenAI SDK транскрибира същите файлове коректно.

**Очаквано:** `createAudioClient()` трябва да връща текстова транскрипция, съответстваща на имплементациите на Python/C#.

---

## 5. C# SDK доставя само net8.0 — няма официална цел .NET 9 или .NET 10

**Статус:** Отворен  
**Тежест:** Липса в документацията  
**Компонент:** NuGet пакет `Microsoft.AI.Foundry.Local` v0.9.0  
**Инсталационна команда:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet пакетът доставя само една целева рамка:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Не е включена `net9.0` или `net10.0` TFM. За разлика от него, спътниковият пакет `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) доставя `net8.0`, `net9.0`, `net10.0`, `net472` и `netstandard2.0`.

### Тестване на съвместимостта

| Целева рамка | Сграждане | Стартиране | Забележки |
|--------------|-----------|------------|-----------|
| net8.0       | ✅        | ✅         | Официално поддържана |
| net9.0       | ✅        | ✅         | Изграждане с forward-compat — използва се в примери от workshop-а |
| net10.0      | ✅        | ✅         | Изграждане и стартиране чрез forward-compat с .NET 10.0.3 runtime |

net8.0 сборката се зарежда на по-нови runtimes чрез .NET forward-compat механизма, затова сграждането е успешно. Това обаче не е документирано и не е тествано от SDK екипа.

### Защо примерите целят net9.0

1. **.NET 9 е най-новото стабилно издание** — повечето участници в workshop-а ще го имат инсталиран  
2. **Forward compatibility работи** — net8.0 сборката в NuGet пакета се изпълнява на .NET 9 runtime без проблеми  
3. **.NET 10 (preview/RC)** е прекалено нов за цел на workshop, предназначен за всички

**Очаквано:** Бъдещите версии на SDK да помислят за добавяне на `net9.0` и `net10.0` TFMs заедно с `net8.0`, за да съответстват на модела на `Microsoft.Agents.AI.OpenAI` и да осигурят валидирана поддръжка за по-нови runtimes.

---

## 6. JavaScript ChatClient стрийминг използва callbacks, а не async итератори

**Статус:** Отворен  
**Тежест:** Липса в документацията  
**Компонент:** JavaScript `foundry-local-sdk` v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient`, върнат от `model.createChatClient()`, предоставя метод `completeStreamingChat()`, но той използва **callback модел** вместо да връща async итерируем обект:

```javascript
// ❌ Това НЕ работи — хвърля "stream is not async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Правилен модел — предайте callback функция
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Въздействие:** Разработчици, запознати с async итеративния модел на OpenAI SDK (`for await`), ще срещнат объркващи грешки. Callback трябва да бъде валидна функция, иначе SDK хвърля "Callback must be a valid function."

**Очаквано:** Да се документира callback модела в SDK справката. Алтернативно, да се добави поддръжка за async итерируем модел за съвместимост с OpenAI SDK.

---

## Подробности за средата

| Компонент | Версия |
|-----------|---------|
| ОС | Windows 11 ARM64 |
| Хардуер | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |