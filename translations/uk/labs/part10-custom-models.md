![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Частина 10: Використання кастомних моделей або моделей Hugging Face з Foundry Local

> **Мета:** Скомпілювати модель Hugging Face у оптимізований формат ONNX, необхідний Foundry Local, налаштувати її з шаблоном чату, додати до локального кешу та виконати інференс за допомогою CLI, REST API та OpenAI SDK.

## Огляд

Foundry Local постачається з ретельно відібраним каталогом попередньо скомпільованих моделей, але ви не обмежені цим списком. Будь-яка модель трансформера, доступна на [Hugging Face](https://huggingface.co/) (або збережена локально у форматі PyTorch / Safetensors), може бути скомпільована в оптимізований ONNX-модель і обслуговуватися через Foundry Local.

Процес компіляції використовує **ONNX Runtime GenAI Model Builder**, інструмент командного рядка, який входить у пакет `onnxruntime-genai`. Цей билдер виконує основну роботу: завантажує початкові ваги, конвертує їх у формат ONNX, застосовує квантизацію (int4, fp16, bf16) та генерує конфігураційні файли (включно із шаблоном чату та токенізатором), які очікує Foundry Local.

У цій лабораторній роботі ви скомпілюєте модель **Qwen/Qwen3-0.6B** з Hugging Face, зареєструєте її у Foundry Local та повністю поспілкуєтесь з нею на вашому пристрої.

---

## Цілі навчання

До кінця цієї лабораторії ви зможете:

- Пояснити, навіщо потрібна кастомна компіляція моделей і коли вона потрібна
- Встановити ONNX Runtime GenAI model builder
- Скомпілювати модель Hugging Face в оптимізований ONNX формат однією командою
- Розуміти ключові параметри компіляції (execution provider, precision)
- Створити конфігураційний файл шаблону чату `inference_model.json`
- Додати скомпільовану модель до кешу Foundry Local
- Виконати інференс кастомної моделі за допомогою CLI, REST API та OpenAI SDK

---

## Вимоги

| Вимога | Деталі |
|-------------|---------|
| **Foundry Local CLI** | Встановлено та додано в `PATH` ([Частина 1](part1-getting-started.md)) |
| **Python 3.10+** | Необхідний для ONNX Runtime GenAI model builder |
| **pip** | Менеджер пакетів Python |
| **Вільне місце на диску** | Принаймні 5 ГБ для початкових та скомпільованих моделей |
| **Обліковий запис Hugging Face** | Деякі моделі вимагають прийняти ліцензію перед завантаженням. Qwen3-0.6B використовує ліцензію Apache 2.0 і є вільно доступною. |

---

## Налаштування середовища

Для компіляції моделі потрібні великі пакети Python (PyTorch, ONNX Runtime GenAI, Transformers). Створіть окреме віртуальне середовище, щоб уникнути конфліктів з системним Python або іншими проєктами.

```bash
# З кореня репозиторію
python -m venv .venv
```

Активуйте середовище:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Оновіть pip, щоб уникнути проблем із розв’язанням залежностей:

```bash
python -m pip install --upgrade pip
```

> **Підказка:** Якщо у вас вже є `.venv` з попередніх лабораторій, можна повторно його використовувати. Просто переконайтеся, що воно активоване перед продовженням.

---

## Концепція: конвеєр компіляції

Foundry Local вимагає моделі у форматі ONNX з конфігурацією ONNX Runtime GenAI. Більшість моделей з відкритим кодом на Hugging Face розповсюджуються як ваги у форматах PyTorch або Safetensors, тому потрібен крок конвертації.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Що робить Model Builder?

1. **Завантажує** початкову модель з Hugging Face (або зчитує її з локального шляху).
2. **Конвертує** ваги PyTorch / Safetensors у формат ONNX.
3. **Квантує** модель до меншої точності (наприклад, int4) для зменшення використання пам’яті та підвищення продуктивності.
4. **Генерує** конфігурацію ONNX Runtime GenAI (`genai_config.json`), шаблон чату (`chat_template.jinja`) та всі файли токенізатора для можливості завантаження та обслуговування моделі Foundry Local.

### ONNX Runtime GenAI Model Builder порівняно з Microsoft Olive

Ви можете зустріти згадки про **Microsoft Olive** як альтернативний інструмент для оптимізації моделей. Обидва інструменти можуть створювати ONNX-моделі, але вони мають різне призначення та компроміси:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Пакет** | `onnxruntime-genai` | `olive-ai` |
| **Основне призначення** | Конвертація та квантизація генеративних AI моделей для ONNX Runtime GenAI inference | Повноцінна платформа оптимізації моделей з підтримкою багатьох бекендів і апаратних цілей |
| **Зручність використання** | Одна команда — конвертація і квантизація одним кроком | Побудова workflow — налаштовувані багатопрохідні конвеєри з YAML/JSON |
| **Формат вихідних даних** | Формат ONNX Runtime GenAI (готовий для Foundry Local) | Узагальнений ONNX, ONNX Runtime GenAI або інші формати залежно від workflow |
| **Апаратні цілі** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN та інші |
| **Опції квантизації** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, а також оптимізації графа, тонке налаштування по шарах |
| **Область застосування** | Генеративні AI моделі (LLMs, SLMs) | Будь-які моделі, що конвертуються в ONNX (зображення, NLP, аудіо, мультимодальні) |
| **Найкраще для** | Швидка однократна компіляція моделі для локального інференсу | Продуктові конвеєри з точним контролем оптимізації |
| **Залежності** | Помірні (PyTorch, Transformers, ONNX Runtime) | Великі (додатково фреймворк Olive, опційні компоненти під кожен workflow) |
| **Інтеграція з Foundry Local** | Пряма — вихідний формат сумісний відразу | Потрібен прапорець `--use_ort_genai` та додаткова конфігурація |

> **Чому в цій лабораторії використовується Model Builder:** Для задачі компіляції однієї моделі Hugging Face і реєстрації її у Foundry Local, Model Builder — це найпростіший і найнадійніший шлях. Він генерує саме той формат, який очікує Foundry Local, за допомогою однієї команди. Якщо згодом вам потрібні розширені функції оптимізації — як-то квантизація з урахуванням точності, редагування графа або багатопрохідне тонке налаштування — Olive стане потужним інструментом для дослідження. Докладніше дивіться в [документації Microsoft Olive](https://microsoft.github.io/Olive/).

---

## Лабораторні вправи

### Вправа 1: Встановлення ONNX Runtime GenAI Model Builder

Встановіть пакет ONNX Runtime GenAI, який містить інструмент model builder:

```bash
pip install onnxruntime-genai
```

Перевірте встановлення, переконавшись, що інструмент model builder доступний:

```bash
python -m onnxruntime_genai.models.builder --help
```

Ви побачите довідковий вивід з параметрами, такими як `-m` (ім’я моделі), `-o` (шлях виводу), `-p` (точність), `-e` (execution provider).

> **Примітка:** Model builder залежить від PyTorch, Transformers та кількох інших пакетів. Встановлення може зайняти декілька хвилин.

---

### Вправа 2: Компіляція Qwen3-0.6B для CPU

Виконайте команду, яка завантажить Qwen3-0.6B модель з Hugging Face та скомпілює її для інференсу на CPU з квантизацією int4:

**macOS / Linux:**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows (PowerShell):**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### Призначення параметрів

| Параметр | Призначення | Використане значення |
|-----------|---------|------------|
| `-m` | Ідентифікатор моделі Hugging Face або локальний шлях | `Qwen/Qwen3-0.6B` |
| `-o` | Каталог для збереження скомпільованої моделі ONNX | `models/qwen3` |
| `-p` | Точність квантизації під час компіляції | `int4` |
| `-e` | Провайдер виконання ONNX Runtime (цільове обладнання) | `cpu` |
| `--extra_options hf_token=false` | Пропускає автентифікацію Hugging Face (підходить для публічних моделей) | `hf_token=false` |

> **Скільки часу це займе?** Час компіляції залежить від вашого заліза та розміру моделі. Для Qwen3-0.6B з квантизацією int4 на сучасному CPU це близько 5–15 хвилин. Великі моделі компілюються пропорційно довше.

Після завершення команди ви повинні побачити каталог `models/qwen3` з файлами скомпільованої моделі. Перевірте вміст:

```bash
ls models/qwen3
```

Ви побачите файли, зокрема:
- `model.onnx` та `model.onnx.data` — ваги скомпільованої моделі
- `genai_config.json` — конфігурація ONNX Runtime GenAI
- `chat_template.jinja` — шаблон чату моделі (згенеровано автоматично)
- `tokenizer.json`, `tokenizer_config.json` — файли токенізатора
- Інші файли словника та конфігурації

---

### Вправа 3: Компіляція для GPU (Необов’язково)

Якщо у вас є NVIDIA GPU з підтримкою CUDA, ви можете скомпілювати варіант, оптимізований для GPU, для швидшого інференсу:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Примітка:** Компіляція для GPU вимагає `onnxruntime-gpu` та працездатної інсталяції CUDA. Якщо їх немає, model builder повідомить про помилку. Ви можете пропустити цю вправу і продовжити з варіантом для CPU.

#### Аппаратно-залежні рекомендації по компіляції

| Ціль | Провайдер виконання (`-e`) | Рекомендована точність (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` або `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` або `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Компроміси точності

| Точність | Розмір | Швидкість | Якість |
|-----------|------|-------|---------|
| `fp32` | Найбільший | Найповільніший | Найвища точність |
| `fp16` | Великий | Швидко (GPU) | Дуже хороша точність |
| `int8` | Малий | Швидко | Невелика втрата точності |
| `int4` | Найменший | Найшвидший | Помірна втрата точності |

Для більшості локальних розробок `int4` на CPU дає найкращій баланс швидкості і використання ресурсів. Для виробничої якості рекомендується `fp16` на CUDA GPU.

---

### Вправа 4: Створення конфігурації шаблону чату

Model builder автоматично генерує файл `chat_template.jinja` і файл `genai_config.json` у каталозі виводу. Проте Foundry Local також потрібен файл `inference_model.json`, щоб знати, як форматувати запити для вашої моделі. Цей файл визначає ім’я моделі та шаблон підказки, що обгортає повідомлення користувача у правильні спеціальні токени.

#### Крок 1: Перегляд скомпільованих файлів

Перелічіть вміст каталогу з скомпільованою моделлю:

```bash
ls models/qwen3
```

Ви побачите файли, такі як:
- `model.onnx` та `model.onnx.data` — ваги скомпільованої моделі
- `genai_config.json` — конфігурація ONNX Runtime GenAI (згенерована автоматично)
- `chat_template.jinja` — шаблон чату моделі (згенерований автоматично)
- `tokenizer.json`, `tokenizer_config.json` — файли токенізатора
- Різні інші конфігураційні та словникові файли

#### Крок 2: Генерація файлу inference_model.json

Файл `inference_model.json` повідомляє Foundry Local, як форматувати підказки. Створіть скрипт Python з ім’ям `generate_chat_template.py` **у корені репозиторію** (той самий каталог, де знаходиться папка `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Побудувати мінімальну розмову для вилучення шаблону чату
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# Створити структуру inference_model.json
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```

Запустіть скрипт з кореневого каталогу репозиторію:

```bash
python generate_chat_template.py
```

> **Примітка:** Пакет `transformers` вже був встановлений як залежність `onnxruntime-genai`. Якщо побачите `ImportError`, спочатку виконайте `pip install transformers`.

Скрипт створить файл `inference_model.json` у каталозі `models/qwen3`. Цей файл повідомляє Foundry Local, як обгорнути вхід користувача у правильні спеціальні токени для Qwen3.

> **Важливо:** Поле `"Name"` у `inference_model.json` (у цьому скрипті встановлено як `qwen3-0.6b`) — це **псевдонім моделі**, який ви будете використовувати у всіх наступних командах та викликах API. Якщо зміните це ім’я, оновіть його у вправах 6–10 відповідно.

#### Крок 3: Перевірка конфігурації

Відкрийте файл `models/qwen3/inference_model.json` і переконайтеся, що він містить поле `Name` та об’єкт `PromptTemplate` з ключами `assistant` і `prompt`. Шаблон підказки повинен містити спеціальні токени, наприклад `<|im_start|>` і `<|im_end|>` (точні токени залежать від шаблону чату моделі).

> **Альтернатива вручну:** Якщо не хочете запускати скрипт, можете створити файл вручну. Головне, щоб поле `prompt` містило повний шаблон чат-моделі з `{Content}` як заповнювачем для повідомлення користувача.

---

### Вправа 5: Перевірка структури каталогу моделі
Конструктор моделей розміщує всі скомпільовані файли безпосередньо у вказаній вами вихідній теці. Переконайтеся, що кінцева структура виглядає правильно:

```bash
ls models/qwen3
```

Каталог повинен містити наступні файли:

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **Примітка:** На відміну від деяких інших інструментів компіляції, конструктор моделей не створює вкладені підтеки. Всі файли знаходяться безпосередньо у вихідній теці, і це саме те, що очікує Foundry Local.

---

### Вправа 6: Додайте модель до кешу Foundry Local

Повідомте Foundry Local, де знайти вашу скомпільовану модель, додавши каталог до його кешу:

```bash
foundry cache cd models/qwen3
```

Перевірте, чи з'явилася модель у кеші:

```bash
foundry cache ls
```

Ви повинні побачити вашу кастомну модель у списку разом із раніше доданими моделями (наприклад, `phi-3.5-mini` або `phi-4-mini`).

---

### Вправа 7: Запустіть кастомну модель через CLI

Запустіть інтерактивний чат із вашою новою скомпільованою моделлю (псевдонім `qwen3-0.6b` походить із поля `Name` у файлі `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Прапорець `--verbose` показує додаткову діагностичну інформацію, яка корисна при першому тестуванні кастомної моделі. Якщо модель завантажилася успішно, ви побачите інтерактивний запит. Спробуйте кілька повідомлень:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Введіть `exit` або натисніть `Ctrl+C`, щоб завершити сеанс.

> **Вирішення проблем:** Якщо модель не завантажується, перевірте наступне:
> - Файл `genai_config.json` був згенерований конструктором моделей.
> - Файл `inference_model.json` існує та містить дійсний JSON.
> - Файли моделі ONNX знаходяться у правильному каталозі.
> - У вас достатньо вільної оперативної пам’яті (Qwen3-0.6B int4 потребує близько 1 ГБ).
> - Qwen3 — це модель для логічного виведення, яка генерує теги `<think>`. Якщо на відповідь передує `<think>...</think>`, це нормальна поведінка. Шаблон запиту у `inference_model.json` можна налаштувати, щоб подавити вивід роздумів.

---

### Вправа 8: Отримайте запит до кастомної моделі через REST API

Якщо ви вийшли з інтерактивного сеансу у вправі 7, модель може більше не бути завантаженою. Спочатку запустіть службу Foundry Local і завантажте модель:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Перевірте, на якому порту запущено службу:

```bash
foundry service status
```

Потім надішліть запит (якщо порт відрізняється, замініть `5273` на ваш):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Примітка для Windows:** Наведена вище команда `curl` використовує синтаксис bash. Для Windows використовуйте PowerShell cmdlet `Invoke-RestMethod` нижче.

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### Вправа 9: Використовуйте кастомну модель з OpenAI SDK

Ви можете підключитися до вашої кастомної моделі, використовуючи точно той самий код OpenAI SDK, який застосовували для вбудованих моделей (див. [Частина 3](part3-sdk-and-apis.md)). Єдина різниця — ім’я моделі.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local не перевіряє ключі API
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local не перевіряє ключі API
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **Головна думка:** Оскільки Foundry Local надає API сумісний з OpenAI, будь-який код, який працює з вбудованими моделями, також працюватиме з вашими кастомними. Потрібно лише змінити параметр `model`.

---

### Вправа 10: Перевірте кастомну модель за допомогою Foundry Local SDK

У попередніх лабораторіях ви використовували Foundry Local SDK для запуску служби, пошуку кінцевої точки та автоматичного керування моделями. Ви можете застосувати той самий підхід для вашої кастомно скомпільованої моделі. SDK обробляє запуск служби та пошук кінцевої точки, тому вам не потрібно жорстко задавати `localhost:5273` у коді.

> **Примітка:** Переконайтеся, що Foundry Local SDK встановлений перед запуском цих прикладів:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Додайте NuGet-пакети `Microsoft.AI.Foundry.Local` і `OpenAI`
>
> Збережіть кожен скрипт **у корені репозиторію** (у тій же теці, що й ваша папка `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Крок 1: Запустіть службу Foundry Local і завантажте користувацьку модель
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Крок 2: Перевірте кеш на наявність користувацької моделі
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Крок 3: Завантажте модель у пам’ять
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Крок 4: Створіть клієнта OpenAI, використовуючи виявлену SDK кінцеву точку
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Крок 5: Надішліть запит на потокове завершення чату
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

Запустіть його:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// Крок 1: Запустіть локальну службу Foundry
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Крок 2: Отримайте власну модель з каталогу
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Крок 3: Завантажте модель у пам’ять
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Крок 4: Створіть клієнта OpenAI, використовуючи кінцеву точку, знайдену за допомогою SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Крок 5: Надішліть запит на потокове завершення чату
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

Запустіть його:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **Головна думка:** Foundry Local SDK динамічно виявляє кінцеву точку, тому ви ніколи не жорстко прописуєте номер порту. Це рекомендований підхід для виробничих додатків. Ваша кастомно скомпільована модель працює ідентично до вбудованих моделей каталогу через SDK.

---

## Вибір моделі для компіляції

Qwen3-0.6B використовується як приклад у цій лабораторії, бо вона невелика, швидко компілюється та безкоштовно доступна за ліцензією Apache 2.0. Однак ви можете скомпілювати багато інших моделей. Ось деякі пропозиції:

| Модель | Ідентифікатор Hugging Face | Параметри | Ліцензія | Примітки |
|-------|-----------------------------|------------|----------|----------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Дуже маленька, швидка компіляція, добре для тестування |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Краща якість, все ще швидко компілюється |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Висока якість, вимагає більше ОЗУ |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Потрібно погодитись із ліцензією на Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Висока якість, більший розмір завантаження й триваліша компіляція |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Вже в каталозі Foundry Local (корисно для порівняння) |

> **Нагадування про ліцензію:** Завжди перевіряйте ліцензію моделі на Hugging Face перед використанням. Деякі моделі (наприклад, Llama) вимагають прийняття ліцензії та автентифікації через `huggingface-cli login` перед завантаженням.

---

## Концепції: коли варто використовувати кастомні моделі

| Сценарій | Чому компілювати свою модель? |
|----------|-------------------------------|
| **Потрібна модель відсутня в каталозі** | Каталог Foundry Local підтримуваний. Якщо потрібної моделі немає, скомпілюйте її самостійно. |
| **Фінетюнінг моделей** | Якщо ви донавчили модель на доменних даних, потрібно скомпілювати власні ваги. |
| **Особливі вимоги до квантизації** | Можливо, ви хочете іншу точність або стратегію квантизації, відмінну від дефолтної в каталозі. |
| **Нові релізи моделей** | Коли виходить нова модель на Hugging Face, вона може ще не бути в каталозі Foundry Local. Компіляція дає миттєвий доступ. |
| **Дослідження та експерименти** | Перевірка різних архітектур, розмірів чи налаштувань локально перед вибором для продакшну. |

---

## Підсумок

У цій лабораторії ви навчилися:

| Крок | Що ви зробили |
|------|---------------|
| 1 | Встановили ONNX Runtime GenAI model builder |
| 2 | Скомпілювали `Qwen/Qwen3-0.6B` з Hugging Face в оптимізовану ONNX модель |
| 3 | Створили конфігураційний файл шаблону чату `inference_model.json` |
| 4 | Додали скомпільовану модель у кеш Foundry Local |
| 5 | Запустили інтерактивний чат із кастомною моделлю через CLI |
| 6 | Зробили запит до моделі через OpenAI-сумісний REST API |
| 7 | Підключилися з Python, JavaScript і C# через OpenAI SDK |
| 8 | Перевірили кастомну модель повністю з Foundry Local SDK |

Головний висновок: **будь-яка модель на основі трансформерів може працювати у Foundry Local** після компіляції в формат ONNX. Сумісний з OpenAI API дозволяє використовувати весь ваш існуючий код без змін; потрібно лише поміняти ім’я моделі.

---

## Основні висновки

| Концепція | Деталі |
|-----------|---------|
| ONNX Runtime GenAI Model Builder | Перетворює моделі Hugging Face у ONNX формат із квантизацією однією командою |
| ONNX формат | Foundry Local потребує моделей ONNX із конфігурацією ONNX Runtime GenAI |
| Шаблони чату | Файл `inference_model.json` повідомляє Foundry Local, як форматувати запити для конкретної моделі |
| Цільове обладнання | Компіліруйте для CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) або WebGPU залежно від обладнання |
| Квантизація | Нижча точність (int4) зменшує розмір і прискорює, але трохи знижує якість; fp16 зберігає високу якість на GPU |
| Сумісність API | Кастомні моделі використовують той самий OpenAI-сумісний API, що й вбудовані моделі |
| Foundry Local SDK | SDK автоматично запускає службу, знаходить кінцеву точку і завантажує моделі для каталогу й кастомних моделей |

---

## Додаткові матеріали

| Ресурс | Посилання |
|--------|-----------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Посібник із кастомних моделей Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Родина моделей Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Документація Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Наступні кроки

Продовжуйте до [Частина 11: Виклики інструментів із локальними моделями](part11-tool-calling.md), щоб дізнатися, як дозволити вашим локальним моделям викликати зовнішні функції.

[← Частина 9: Розпізнавання голосу Whisper](part9-whisper-voice-transcription.md) | [Частина 11: Виклики інструментів →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Відмова від відповідальності**:
Цей документ був перекладений за допомогою сервісу автоматичного перекладу [Co-op Translator](https://github.com/Azure/co-op-translator). Хоча ми прагнемо до точності, будь ласка, майте на увазі, що автоматичні переклади можуть містити помилки або неточності. Оригінальний документ рідною мовою має вважатися авторитетним джерелом. Для критично важливої інформації рекомендується професійний переклад людиною. Ми не несемо відповідальності за будь-які непорозуміння чи неправильні тлумачення, що виникли внаслідок використання цього перекладу.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->