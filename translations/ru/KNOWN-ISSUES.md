# Известные проблемы — Foundry Local Workshop

Проблемы, возникшие при сборке и тестировании этого воркшопа на устройстве **Snapdragon X Elite (ARM64)** с Windows, используя Foundry Local SDK v0.9.0, CLI v0.8.117 и .NET SDK 10.0.

> **Последняя проверка:** 2026-03-11

---

## 1. Процессор Snapdragon X Elite не распознаётся ONNX Runtime

**Статус:** Открыта  
**Серьёзность:** Предупреждение (неблокирующая)  
**Компонент:** ONNX Runtime / cpuinfo  
**Воспроизведение:** Каждый запуск сервиса Foundry Local на оборудовании Snapdragon X Elite  

При каждом запуске сервиса Foundry Local выводятся два предупреждения:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Влияние:** Предупреждения носят косметический характер — инференс работает корректно. Однако они появляются при каждом запуске и могут сбивать с толку участников воркшопа. Библиотеку cpuinfo ONNX Runtime необходимо обновить, чтобы распознавались ядра CPU Qualcomm Oryon.

**Ожидаемо:** Snapdragon X Elite должен распознаваться как поддерживаемый ARM64 процессор без сообщений уровня ошибки.

---

## 2. Исключение NullReferenceException в SingleAgent при первом запуске

**Статус:** Открыта (непостоянно)  
**Серьёзность:** Критическая (сбой)  
**Компонент:** Foundry Local C# SDK + Microsoft Agent Framework  
**Воспроизведение:** Запуск `dotnet run agent` — сбой немедленно после загрузки модели  

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Контекст:** В строке 37 вызывается `model.IsCachedAsync(default)`. Сбой произошёл при первом запуске агента после свежей команды `foundry service stop`. Последующие запуски с тем же кодом были успешными.

**Влияние:** Непостоянно — указывает на состояние гонки при инициализации сервиса SDK или запросе каталога. Вызов `GetModelAsync()` может выполняться до полной готовности сервиса.

**Ожидаемо:** `GetModelAsync()` должен либо блокировать выполнение до готовности сервиса, либо возвращать явное сообщение об ошибке, если сервис ещё не завершил инициализацию.

---

## 3. C# SDK требует явного RuntimeIdentifier

**Статус:** Открыта — отслеживается в [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Серьёзность:** Пробел в документации  
**Компонент:** NuGet-пакет `Microsoft.AI.Foundry.Local`  
**Воспроизведение:** Создать проект .NET 8+ без `<RuntimeIdentifier>` в `.csproj`

Сборка завершается с ошибкой:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Причина:** Требование RID ожидаемо — SDK поставляет нативные бинарные файлы (P/Invoke в `Microsoft.AI.Foundry.Local.Core` и ONNX Runtime), поэтому .NET должен знать, какую платформо-зависимую библиотеку разрешать.

Это описано в MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), где в инструкциях запуска показано:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Однако пользователям каждый раз нужно не забывать флаг `-r`, что легко забыть.

**Обходной путь:** Добавьте в `.csproj` автоподстановку, чтобы `dotnet run` работал без дополнительных флагов:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` — встроенное свойство MSBuild, которое автоматически разрешает RID хост-машины. Тестовые проекты SDK уже используют этот подход. Явные флаги `-r` по-прежнему учитываются, если они переданы.

> **Примечание:** Воркшоп `.csproj` включает этот fallback, так что `dotnet run` работает сразу на любой платформе.

**Ожидаемо:** Шаблон `.csproj` в документации MS Learn должен включать этот автоматический распознаваемый паттерн, чтобы пользователям не нужно было помнить про флаг `-r`.

---

## 4. JavaScript Whisper — транскрипция аудио возвращает пустой или бинарный вывод

**Статус:** Открыта (регрессия — ухудшилось с момента первого сообщения)  
**Серьёзность:** Важная  
**Компонент:** Реализация JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Воспроизведение:** Запустить `node foundry-local-whisper.mjs` — все аудиофайлы возвращают пустой или бинарный вывод вместо текстовой транскрипции

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Изначально пустым был только пятый аудиофайл; начиная с версии v0.9.x все 5 файлов возвращают один байт (`\ufffd`) вместо текста. Python-реализация Whisper через OpenAI SDK транскрибирует эти файлы корректно.

**Ожидаемо:** `createAudioClient()` должен возвращать текстовую транскрипцию, совпадающую с реализациями на Python/C#.

---

## 5. C# SDK поставляется только с net8.0 — нет официальной поддержки .NET 9 или .NET 10

**Статус:** Открыта  
**Серьёзность:** Пробел в документации  
**Компонент:** NuGet-пакет `Microsoft.AI.Foundry.Local` v0.9.0  
**Команда установки:** `dotnet add package Microsoft.AI.Foundry.Local`

Пакет NuGet содержит только одну целевую платформу:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Отсутствуют TFM `net9.0` или `net10.0`. Для сравнения, сопутствующий пакет `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) поставляется с `net8.0`, `net9.0`, `net10.0`, `net472` и `netstandard2.0`.

### Тестирование совместимости

| Целевая платформа | Сборка | Запуск | Примечания |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | Официально поддерживается |
| net9.0 | ✅ | ✅ | Сборка через forward-compat — используется в примерах воркшопа |
| net10.0 | ✅ | ✅ | Сборка и запуск через forward-compat с рантаймом .NET 10.0.3 |

Сборка net8.0 загружается на новых рантаймах благодаря механизму forward-compat .NET, поэтому сборка проходит успешно. Однако это не документировано и не тестируется командой SDK.

### Почему примеры нацелены на net9.0

1. **.NET 9 — последняя стабильная версия** — большинство участников воркшопа будет именно с ней  
2. **Forward-совместимость работает** — сборка net8.0 из NuGet-пакета без проблем работает на рантайме .NET 9  
3. **.NET 10 (превью/RC)** слишком нова для воркшопа, который должен работать у всех

**Ожидаемо:** В будущих релизах SDK следует рассмотреть возможность добавления TFM `net9.0` и `net10.0` наряду с `net8.0`, чтобы соответствовать паттерну `Microsoft.Agents.AI.OpenAI` и обеспечить проверенную поддержку новых рантаймов.

---

## 6. JavaScript ChatClient Streaming использует callbacks, а не async итераторы

**Статус:** Открыта  
**Серьёзность:** Пробел в документации  
**Компонент:** JavaScript foundry-local-sdk v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient`, возвращаемый `model.createChatClient()`, имеет метод `completeStreamingChat()`, но он использует **паттерн с callback** и не возвращает async iterable:

```javascript
// ❌ Это НЕ работает — выдает ошибку "stream is not async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Правильный шаблон — передайте колбек
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Влияние:** Разработчики, знакомые с паттерном async итераций OpenAI SDK (`for await`), столкнутся с ошибками. Callback должен быть валидной функцией, иначе SDK выдаст "Callback must be a valid function".

**Ожидаемо:** Задокументировать паттерн callback в справке SDK. В качестве альтернативы поддерживать паттерн async iterable для согласованности с OpenAI SDK.

---

## Детали среды

| Компонент | Версия |
|-----------|---------|
| ОС | Windows 11 ARM64 |
| Аппаратное обеспечение | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от ответственности**:  
Этот документ был переведен с помощью сервиса машинного перевода [Co-op Translator](https://github.com/Azure/co-op-translator). Несмотря на наши усилия по обеспечению точности, имейте в виду, что автоматический перевод может содержать ошибки или неточности. Оригинальный документ на исходном языке следует считать авторитетным источником. Для получения критически важной информации рекомендуется использовать профессиональный человеческий перевод. Мы не несем ответственности за любые недоразумения или неправильные толкования, возникшие в результате использования данного перевода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->