# Відомі проблеми — Foundry Local Workshop

Проблеми, які виникли під час створення та тестування цього воркшопу на пристрої **Snapdragon X Elite (ARM64)** під керуванням Windows, з Foundry Local SDK v0.9.0, CLI v0.8.117 та .NET SDK 10.0.

> **Остання перевірка:** 2026-03-11

---

## 1. CPU Snapdragon X Elite не розпізнається ONNX Runtime

**Статус:** Відкрито  
**Серйозність:** Попередження (не блокує)  
**Компонент:** ONNX Runtime / cpuinfo  
**Відтворення:** Кожен запуск сервісу Foundry Local на обладнанні Snapdragon X Elite

При кожному запуску сервісу Foundry Local з'являються два попередження:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Вплив:** Попередження косметичні — інференс працює коректно. Але вони з'являються при кожному запуску і можуть заплутати учасників воркшопу. Потрібно оновити бібліотеку ONNX Runtime cpuinfo, щоб розпізнавати ядра Qualcomm Oryon CPU.

**Очікувано:** Snapdragon X Elite має розпізнаватися як підтримуваний ARM64 CPU без повідомлень рівня помилок.

---

## 2. NullReferenceException у SingleAgent під час першого запуску

**Статус:** Відкрито (переривчасто)  
**Серйозність:** Критична (завал)  
**Компонент:** Foundry Local C# SDK + Microsoft Agent Framework  
**Відтворення:** Запустити `dotnet run agent` — аварійне завершення одразу після завантаження моделі

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Контекст:** У рядку 37 викликається `model.IsCachedAsync(default)`. Помилка виникла при першому запуску агента після свіжої команди `foundry service stop`. Наступні запуски з тим самим кодом проходили успішно.

**Вплив:** Переривчаста — свідчить про умовну гонку під час ініціалізації сервісу SDK або запиту каталогу. Виклик `GetModelAsync()` може повертати результат до повної готовності сервісу.

**Очікувано:** `GetModelAsync()` має або блокуватися до готовності сервісу, або повертати чітке повідомлення про помилку у випадку, якщо сервіс ще не завершив ініціалізацію.

---

## 3. C# SDK вимагає явного RuntimeIdentifier

**Статус:** Відкрито — відслідковується у [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Серйозність:** Прогалина в документації  
**Компонент:** NuGet пакет `Microsoft.AI.Foundry.Local`  
**Відтворення:** Створити .NET 8+ проект без `<RuntimeIdentifier>` у `.csproj`

Збірка не проходить з помилкою:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Причина:** Вимога RID очікувана — SDK містить нативні бібліотеки (P/Invoke до `Microsoft.AI.Foundry.Local.Core` та ONNX Runtime), тож .NET мусить знати, яку платформо-залежну бібліотеку завантажувати.

Це документується на MS Learn ([Як використовувати нативні чат-комплекти](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), де в інструкціях запуску показано:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Проте користувачам потрібно пам’ятати про прапорець `-r` щоразу, що легко забути.

**Обхідний шлях:** Додати авто-виявлення fallback у `.csproj`, щоб `dotnet run` працював без прапорців:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` — вбудована властивість MSBuild, що автоматично визначає RID хост-машини. Тестові проєкти SDK вже використовують цей підхід. Явні прапорці `-r` також підтримуються, коли надані.

> **Примітка:** У проекті воркшопу `.csproj` включено цей fallback, щоб `dotnet run` працював напряму на будь-якій платформі.

**Очікувано:** Шаблон `.csproj` у документації MS Learn має включати цей авто-виявний патерн, щоб користувачам не доводилося пам’ятати прапорець `-r`.

---

## 4. JavaScript Whisper — Аудіотранскрипція повертає пустий/бінарний результат

**Статус:** Відкрито (регресія — погіршилася після початкової звітності)  
**Серйозність:** Важлива  
**Компонент:** JavaScript Whisper імплементація (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Відтворення:** Запустити `node foundry-local-whisper.mjs` — усі аудіофайли повертають порожній або бінарний результат замість текстового транскрипту

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Спочатку порожнім був тільки 5-й файл; наразі, починаючи з v0.9.x, усі 5 файлів повертають один байт (`\ufffd`) замість текстової транскрипції. Імплементація Whisper на Python через OpenAI SDK правильно транскрибує ці файли.

**Очікувано:** `createAudioClient()` має повертати текстовий транскрипт, що відповідає реалізаціям на Python/C#.

---

## 5. C# SDK поширюється тільки з net8.0 — немає офіційної підтримки .NET 9 або .NET 10

**Статус:** Відкрито  
**Серйозність:** Прогалина в документації  
**Компонент:** NuGet пакет `Microsoft.AI.Foundry.Local` версії 0.9.0  
**Команда встановлення:** `dotnet add package Microsoft.AI.Foundry.Local`

Пакет NuGet містить тільки одну цільову платформу:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Немає TFM `net9.0` або `net10.0`. Для порівняння, супутній пакет `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) підтримує `net8.0`, `net9.0`, `net10.0`, `net472` і `netstandard2.0`.

### Тестування сумісності

| Цільова платформа | Збірка | Запуск | Примітки |
|-------------------|--------|--------|----------|
| net8.0            | ✅     | ✅     | Офіційно підтримується |
| net9.0            | ✅     | ✅     | Збирається через forward-compat — використовується у прикладах воркшопу |
| net10.0           | ✅     | ✅     | Збирається і запускається через forward-compat із runtime .NET 10.0.3 |

Збірка net8.0 завантажується на нових runtime завдяки механізму forward-compat .NET, тому збірка проходить. Проте це не задокументовано та не протестовано командою SDK.

### Чому приклади націлені на net9.0

1. **.NET 9 — останній стабільний реліз** — більшість учасників воркшопу матимуть його встановленим  
2. **Forward compatibility працює** — збірка net8.0 у NuGet пакеті працює на runtime .NET 9 без проблем  
3. **.NET 10 (прев’ю/RC)** надто новий для воркшопу, який має працювати для всіх

**Очікувано:** Майбутні релізи SDK мають розглянути додавання TFM `net9.0` і `net10.0` поряд з `net8.0`, щоб відповідати підходу `Microsoft.Agents.AI.OpenAI` і забезпечити підтверджену підтримку новіших runtime.

---

## 6. JavaScript ChatClient Streaming використовує колбеки, а не асинхронні ітератори

**Статус:** Відкрито  
**Серйозність:** Прогалина в документації  
**Компонент:** JavaScript `foundry-local-sdk` v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient`, повернутий `model.createChatClient()`, надає метод `completeStreamingChat()`, але він використовує **callback pattern**, а не повертає асинхронний ітерований об’єкт:

```javascript
// ❌ Це НЕ працює — викликає помилку "stream is not async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Правильний шаблон — передайте зворотний виклик
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Вплив:** Розробники, знайомі з асинхронними ітераторами OpenAI SDK (`for await`), натраплять на незрозумілі помилки. Колбек має бути валідною функцією, інакше SDK викидає помилку "Callback must be a valid function."

**Очікувано:** Документувати патерн колбека у референсі SDK. Або підтримати асинхронний ітератор для послідовності з OpenAI SDK.

---

## Інформація про середовище

| Компонент             | Версія      |
|----------------------|-------------|
| ОС                   | Windows 11 ARM64 |
| Обладнання           | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI    | 0.8.117     |
| Foundry Local SDK (C#) | 0.9.0      |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK        | 2.9.0       |
| .NET SDK             | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x   |
| foundry-local-sdk (JS) | 0.9.x      |
| Node.js              | 18+         |
| ONNX Runtime         | 1.18+       |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Відмова від відповідальності**:
Цей документ було перекладено за допомогою сервісу автоматичного перекладу [Co-op Translator](https://github.com/Azure/co-op-translator). Хоча ми прагнемо до точності, будь ласка, зверніть увагу, що автоматичні переклади можуть містити помилки або неточності. Оригінальний документ у його рідній мові слід вважати авторитетним джерелом. Для критичної інформації рекомендується професійний людський переклад. Ми не несемо відповідальності за будь-які непорозуміння чи неправильні тлумачення, що виникли внаслідок використання цього перекладу.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->