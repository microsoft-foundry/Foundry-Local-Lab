![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Част 10: Използване на собствени или Hugging Face модели с Foundry Local

> **Цел:** Компилиране на модел от Hugging Face в оптимизиран ONNX формат, необходим за Foundry Local, конфигуриране с чат шаблон, добавяне към локалния кеш и изпълнение на разпознаване с него чрез CLI, REST API и OpenAI SDK.

## Преглед

Foundry Local се доставя с куриран каталог от предварително компилирани модели, но не сте ограничени само до този списък. Всеки трансформър-базиран езиков модел, наличен на [Hugging Face](https://huggingface.co/) (или съхранен локално във формат PyTorch / Safetensors), може да бъде компилиран в оптимизиран ONNX модел и обслужван чрез Foundry Local.

Процесът на компилация използва **ONNX Runtime GenAI Model Builder**, команден инструмент включен в пакета `onnxruntime-genai`. Този конструктор поема тежката работа: изтеглянето на изходните тегла, конвертирането им в ONNX формат, прилагането на квантоване (int4, fp16, bf16) и генерирането на конфигурационните файлове (включително чат шаблона и токенизатора), които Foundry Local очаква.

В този лабораторен урок ще компилирате **Qwen/Qwen3-0.6B** от Hugging Face, ще го регистрирате в Foundry Local и ще си чатите с него изцяло на вашето устройство.

---

## Образователни цели

След края на този лабораторен урок ще можете:

- Да обясните защо компилацията на собствени модели е полезна и кога може да ви е необходима
- Да инсталирате ONNX Runtime GenAI model builder
- Да компилирате модел от Hugging Face в оптимизиран ONNX формат с една команда
- Да разберете ключовите параметри за компилация (execution provider, precision)
- Да създадете конфигурационен файл `inference_model.json` с чат шаблон
- Да добавите компилиран модел в кеша на Foundry Local
- Да изпълните разпознаване с персонализиран модел чрез CLI, REST API и OpenAI SDK

---

## Предварителни условия

| Изискване | Подробности |
|-------------|---------|
| **Foundry Local CLI** | Инсталиран и в `PATH` ([Част 1](part1-getting-started.md)) |
| **Python 3.10+** | Изисква се от ONNX Runtime GenAI model builder |
| **pip** | Python пакетен мениджър |
| **Дисково пространство** | Най-малко 5 GB свободни за изходните и компилираните файлове на модела |
| **Акаунт в Hugging Face** | Някои модели изискват приемане на лиценз преди изтегляне. Qwen3-0.6B използва лиценз Apache 2.0 и е свободно достъпен. |

---

## Настройка на околната среда

Компилацията на модел изисква няколко големи Python пакета (PyTorch, ONNX Runtime GenAI, Transformers). Създайте отделна виртуална среда, за да не се получават конфликти с вашия системен Python или други проекти.

```bash
# От корена на хранилището
python -m venv .venv
```

Активирайте средата:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Обновете pip, за да избегнете проблеми с разрешаването на зависимости:

```bash
python -m pip install --upgrade pip
```

> **Съвет:** Ако вече имате `.venv` от по-ранни уроци, можете да я използвате отново. Уверете се само, че е активирана преди да продължите.

---

## Концепция: Процесът на компилация

Foundry Local изисква модели в ONNX формат с конфигурация за ONNX Runtime GenAI. Повечето отворени модели на Hugging Face се разпространяват като PyTorch или Safetensors тегла, затова е нужна стъпка за конверсия.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Какво прави Model Builder?

1. **Изтегля** изходния модел от Hugging Face (или го чете от локална пътека).
2. **Конвертира** теглата PyTorch / Safetensors в ONNX формат.
3. **Квантова** модела в по-ниска прецизност (например int4), за да намали използването на памет и да ускори работата.
4. **Генерира** конфигурацията на ONNX Runtime GenAI (`genai_config.json`), чат шаблона (`chat_template.jinja`) и всички файлове на токенизатора, така че Foundry Local да може да зареди и обслужва модела.

### ONNX Runtime GenAI Model Builder срещу Microsoft Olive

Може да срещнете препратки към **Microsoft Olive** като алтернативен инструмент за оптимизация на модели. И двата инструмента могат да произвеждат ONNX модели, но служат за различни цели и имат различни компромиси:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Пакет** | `onnxruntime-genai` | `olive-ai` |
| **Основно предназначение** | Конверсия и квантоване на генеративни AI модели за ONNX Runtime GenAI inference | Енд-то-енд платформа за оптимизация на модели с поддръжка на множество бекендове и хардуерни цели |
| **Лесен за употреба** | Една команда — едностъпкова конверсия + квантоване | Работен процес — конфигурируеми многопроходни пайплайни с YAML/JSON |
| **Изходен формат** | ONNX Runtime GenAI формат (готов за Foundry Local) | Общ ONNX, ONNX Runtime GenAI или други формати в зависимост от работния процес |
| **Хардуерни цели** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN и други |
| **Опции за квантоване** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, плюс оптимизации на графи, насочена настройка на слоеве |
| **Обхват на модела** | Генеративни AI модели (LLMs, SLMs) | Всеки ONNX-конвертируем модел (визия, NLP, аудио, мултимодал) |
| **Най-подходящ за** | Бърза едновременна компилация на отделни модели за локален inference | Производствени пайплайни, нуждаещи се от прецизен контрол върху оптимизацията |
| **Отпечатък на зависимости** | Среден (PyTorch, Transformers, ONNX Runtime) | По-голям (добавя Olive framework, опционални екстри според workflow) |
| **Интеграция с Foundry Local** | Директна — изходът е веднага съвместим | Изисква флага `--use_ort_genai` и допълнителна конфигурация |

> **Защо тази лаборатория използва Model Builder:** За задачата да компилирате един Hugging Face модел и да го регистрирате в Foundry Local, Model Builder е най-простият и надежден път. Той произвежда точно желания от Foundry Local изходен формат с една команда. Ако по-късно имате нужда от по-усъвършенствани оптимизации — например квантоване с осъзнаване на точността, модификации на графа или многопроходно настройване — Olive е мощна опция за разглеждане. Вижте [документацията на Microsoft Olive](https://microsoft.github.io/Olive/) за повече информация.

---

## Лабораторни упражнения

### Упражнение 1: Инсталиране на ONNX Runtime GenAI Model Builder

Инсталирайте пакета ONNX Runtime GenAI, който включва инструмента model builder:

```bash
pip install onnxruntime-genai
```

Проверете инсталацията като потвърдите, че model builder е наличен:

```bash
python -m onnxruntime_genai.models.builder --help
```

Трябва да видите помощен изход, който изброява параметри като `-m` (име на модел), `-o` (път за изходни файлове), `-p` (прецизност) и `-e` (execution provider).

> **Забележка:** Model builder зависи от PyTorch, Transformers и няколко други пакета. Инсталацията може да отнеме няколко минути.

---

### Упражнение 2: Компилиране на Qwen3-0.6B за CPU

Изпълнете следната команда, за да изтеглите модела Qwen3-0.6B от Hugging Face и да го компилирате за inference на CPU с int4 квантоване:

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

#### Какво прави всеки параметър

| Параметър | Цел | Използвана стойност |
|-----------|---------|------------|
| `-m` | Hugging Face идентификатор на модела или локална директория | `Qwen/Qwen3-0.6B` |
| `-o` | Папка, където ще се запазят компилираните ONNX файлове | `models/qwen3` |
| `-p` | Квантована прецизност, прилагана по време на компилация | `int4` |
| `-e` | ONNX Runtime execution provider (целеви хардуер) | `cpu` |
| `--extra_options hf_token=false` | Пропуска аутентификацията на Hugging Face (подходящо за публични модели) | `hf_token=false` |

> **Колко време отнема?** Времето за компилация зависи от хардуера ви и размера на модела. За Qwen3-0.6B с int4 квантоване на модерен CPU очаквайте около 5 до 15 минути. По-големите модели отнемат пропорционално повече.

След завършване ще видите директория `models/qwen3` с компилираните файлове на модела. Проверете изхода:

```bash
ls models/qwen3
```

Трябва да видите файлове включително:
- `model.onnx` и `model.onnx.data` — компилираните тегла на модела
- `genai_config.json` — конфигурация за ONNX Runtime GenAI
- `chat_template.jinja` — чат шаблон на модела (автоматично генериран)
- `tokenizer.json`, `tokenizer_config.json` — файлове на токенизатора
- Други файлове с речник и конфигурация

---

### Упражнение 3: Компилация за GPU (по избор)

Ако имате NVIDIA GPU с поддръжка за CUDA, можете да компилирате GPU-оптимизиран вариант за по-бързо разпознаване:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Забележка:** Компилацията за GPU изисква `onnxruntime-gpu` и работеща инсталация на CUDA. Ако те не са налични, model builder ще съобщи грешка. Можете да пропуснете това упражнение и да продължите с CPU варианта.

#### Справка за хардуерно специфична компилация

| Целеви хардуер | Execution Provider (`-e`) | Препоръчителна прецизност (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` или `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` или `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Компромиси при прецизността

| Прецизност | Размер | Скорост | Качество |
|-----------|------|-------|---------|
| `fp32` | Най-голяма | Най-бавна | Най-висока точност |
| `fp16` | Голяма | Бърза (GPU) | Много добра точност |
| `int8` | Малка | Бърза | Леки загуби в точността |
| `int4` | Най-малка | Най-бърза | Умерени загуби в точността |

За повечето локални разработки `int4` на CPU осигурява най-добър баланс между скорост и използване на ресурси. За продукционни качества е препоръчително `fp16` на CUDA GPU.

---

### Упражнение 4: Създаване на конфигурация на чат шаблона

Model builder автоматично генерира файл `chat_template.jinja` и `genai_config.json` в изходната директория. Въпреки това, Foundry Local също изисква файл `inference_model.json`, който да определи как да се форматират подканите за вашия модел. Този файл дефинира името на модела и шаблона на подканата, която обвива потребителските съобщения с правилните специални токени.

#### Стъпка 1: Преглед на компилирания изход

Избройте съдържанието на директорията с компилирания модел:

```bash
ls models/qwen3
```

Трябва да видите файлове като:
- `model.onnx` и `model.onnx.data` — компилирани тегла на модела
- `genai_config.json` — ONNX Runtime GenAI конфигурация (автоматично генерирана)
- `chat_template.jinja` — чат шаблон на модела (автоматично генериран)
- `tokenizer.json`, `tokenizer_config.json` — файлове на токенизатора
- Различни други конфигурационни и речникови файлове

#### Стъпка 2: Генериране на файл inference_model.json

Файлът `inference_model.json` казва на Foundry Local как да форматира подканите. Създайте Python скрипт с име `generate_chat_template.py` **в главната директория на репозиторито** (същата, която съдържа папката `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Създайте минимален разговор за извличане на шаблона за чат
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

# Създайте структурата inference_model.json
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

Изпълнете скрипта от корена на репозиторито:

```bash
python generate_chat_template.py
```

> **Забележка:** Пакетът `transformers` вече беше инсталиран като зависимост на `onnxruntime-genai`. Ако получите `ImportError`, инсталирайте го с `pip install transformers`.

Скриптът ще създаде файл `inference_model.json` в папката `models/qwen3`. Този файл казва на Foundry Local как да обвива потребителския вход с правилните специални токени за Qwen3.

> **Важно:** Полето `"Name"` в `inference_model.json` (зададено на `qwen3-0.6b` в този скрипт) е **псевдонимът на модела**, който ще използвате във всички следващи команди и API повиквания. Ако промените това име, обновете името на модела в упражненията 6–10 съответно.

#### Стъпка 3: Потвърждаване на конфигурацията

Отворете `models/qwen3/inference_model.json` и се уверете, че съдържа поле `Name` и обект `PromptTemplate` с ключове `assistant` и `prompt`. Шаблонът на подканата трябва да съдържа специални токени като `<|im_start|>` и `<|im_end|>` (точните токени зависят от чат шаблона на модела).

> **Ръчен алтернативен вариант:** Ако не желаете да изпълнявате скрипта, можете да създадете файла ръчно. Основното изискване е полето `prompt` да съдържа пълния чат шаблон на модела с `{Content}` като заместител на съобщението на потребителя.

---

### Упражнение 5: Проверка на структурата на директорията на модела
Компилаторът на модели поставя всички компилирани файлове директно в указаната от вас изходна директория. Проверете дали крайната структура изглежда коректна:

```bash
ls models/qwen3
```

Директорията трябва да съдържа следните файлове:

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

> **Забележка:** За разлика от някои други инструменти за компилиране, компилаторът на модели не създава вложени поддиректории. Всички файлове се намират директно в изходната папка, което е точно това, което очаква Foundry Local.

---

### Упражнение 6: Добавяне на модела към кеша на Foundry Local

Укажете на Foundry Local къде да намери компилирания ви модел чрез добавяне на директорията в кеша:

```bash
foundry cache cd models/qwen3
```

Проверете дали моделът се появява в кеша:

```bash
foundry cache ls
```

Трябва да видите вашия персонализиран модел списъчен заедно с всички предварително кеширани модели (като `phi-3.5-mini` или `phi-4-mini`).

---

### Упражнение 7: Стартиране на персонализирания модел от CLI

Стартирайте интерактивна чат сесия с вашия новокомпилиран модел (псевдонимът `qwen3-0.6b` идва от полето `Name`, което сте задали в `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Флагът `--verbose` показва допълнителна диагностична информация, което е полезно при първоначално тестване на персонализиран модел. Ако моделът се зареди успешно, ще видите интерактивен промпт. Опитайте няколко съобщения:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Напишете `exit` или натиснете `Ctrl+C`, за да завършите сесията.

> **Отстраняване на проблеми:** Ако моделът не успее да се зареди, проверете следното:
> - Файлът `genai_config.json` е генериран от компилатора на модели.
> - Файлът `inference_model.json` съществува и е валиден JSON.
> - ONNX моделните файлове са в правилната директория.
> - Имате достатъчно налична RAM памет (Qwen3-0.6B int4 се нуждае от около 1 GB).
> - Qwen3 е модел за разсъждения, който произвежда тагове `<think>`. Ако виждате `<think>...</think>` пред отговорите, това е нормално поведение. Шаблонът за промпт в `inference_model.json` може да бъде настроен да потисне изхода с мисленето.

---

### Упражнение 8: Запитване към персонализирания модел чрез REST API

Ако сте излезли от интерактивната сесия в Упражнение 7, моделът може вече да не е зареден. Стартирайте услугата Foundry Local и заредете модела първо:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Проверете на кой порт работи услугата:

```bash
foundry service status
```

След това изпратете заявка (заменете `5273` с вашия реален порт, ако е различен):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Бележка за Windows:** Командата `curl` по-горе използва синтаксис на bash. В Windows използвайте cmdlet-а `Invoke-RestMethod` на PowerShell, както е показано по-долу.

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

### Упражнение 9: Използване на персонализирания модел с OpenAI SDK

Можете да се свържете с вашия персонализиран модел, като използвате точно същия OpenAI SDK код, който използвахте за вградените модели (вижте [Част 3](part3-sdk-and-apis.md)). Единствената разлика е името на модела.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local не проверява валидността на API ключовете
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
  apiKey: "foundry-local", // Foundry Local не валидира API ключове
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

> **Ключов момент:** Тъй като Foundry Local предоставя OpenAI-съвместим API, всеки код, който работи с вградените модели, ще работи и с вашите персонализирани модели. Просто трябва да смените параметъра `model`.

---

### Упражнение 10: Тествайте персонализирания модел с Foundry Local SDK

В предишните лаборатории използвахте Foundry Local SDK за стартиране на услугата, откриване на endpoint и управление на модели автоматично. Можете да следвате същия подход с вашия персонализиран, компилиран модел. SDK управлява стартирането на услугата и откриването на endpoint, така че вашият код не трябва да кодирате на твърдо `localhost:5273`.

> **Забележка:** Уверете се, че Foundry Local SDK е инсталиран преди да изпълните тези примери:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Добавете NuGet пакети `Microsoft.AI.Foundry.Local` и `OpenAI`
>
> Запазете всеки скрипт **в корена на репозитория** (същата директория като вашата `models/` папка).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Стъпка 1: Стартирайте услугата Foundry Local и заредете персонализирания модел
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Стъпка 2: Проверете кеша за персонализирания модел
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Стъпка 3: Заредете модела в паметта
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Стъпка 4: Създайте OpenAI клиент, използвайки крайна точка, открита чрез SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Стъпка 5: Изпратете заявка за поточно завършване на чат
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

Изпълнете го:

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

// Стъпка 1: Стартирайте локалната услуга Foundry
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Стъпка 2: Вземете персонализирания модел от каталога
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Стъпка 3: Заредете модела в паметта
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Стъпка 4: Създайте OpenAI клиент, използвайки ендпойнт, открит от SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Стъпка 5: Изпратете заявка за чат с непрекъснат поток
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

Изпълнете го:

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

> **Ключов момент:** Foundry Local SDK открива endpoint динамично, така че никога не кодирате на твърдо номер на порт. Това е препоръчителният подход за производствени приложения. Вашият персонализиран компилиран модел работи идентично с вградените модели от каталога през SDK.

---

## Избор на модел за компилиране

Qwen3-0.6B се използва като референтен пример в тази лаборатория, защото е малък, бърз за компилиране и свободно достъпен под лиценза Apache 2.0. Въпреки това, можете да компилирате много други модели. Ето някои предложения:

| Модел | Hugging Face ID | Параметри | Лиценз | Бележки |
|-------|-----------------|-----------|---------|---------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Много малък, бързо компилиране, подходящ за тестване |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | По-добро качество, все още бързо за компилиране |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Силно качество, изисква повече RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Изисква приемане на лицензионни условия в Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Високо качество, голямо теглене и по-дълго компилиране |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Вече в каталога на Foundry Local (полезно за сравнение) |

> **Напомняне за лиценза:** Винаги проверявайте лицензът на модела в Hugging Face преди да го използвате. Някои модели (като Llama) изискват да приемете лицензионно споразумение и да се удостоверите с `huggingface-cli login` преди теглене.

---

## Концепции: Кога да използвате персонализирани модели

| Сценарий | Защо да компилирате собствен? |
|----------|-------------------------------|
| **Моделът, от който имате нужда, не е в каталога** | Каталогът на Foundry Local е куриран. Ако моделът, който искате, не е в списъка, компилирайте го сами. |
| **Финно настройване на модели** | Ако сте фино настроили модел върху домейн-специфични данни, трябва да компилирате собствените си тежести. |
| **Специфични изисквания за квантизация** | Може да искате стратегия за точност или квантизация, която се различава от подразбиращата се в каталога. |
| **По-нови версии на модели** | Когато нов модел излезе в Hugging Face, може все още да не е в каталога на Foundry Local. Компилирането му ви дава незабавен достъп. |
| **Изследвания и експерименти** | Изпробване на различни архитектури, размери или конфигурации на модели локално преди да вземете решение за продукционна употреба. |

---

## Резюме

В тази лаборатория научихте как да:

| Стъпка | Какво направихте |
|--------|------------------|
| 1 | Инсталирахте ONNX Runtime GenAI компилатора на модели |
| 2 | Компилирахте `Qwen/Qwen3-0.6B` от Hugging Face в оптимизиран ONNX модел |
| 3 | Създадохте конфигурационен файл с шаблон за чат `inference_model.json` |
| 4 | Добавихте компилирания модел към кеша на Foundry Local |
| 5 | Стартирахте интерактивен чат с персонализиран модел през CLI |
| 6 | Запитахте към модела чрез OpenAI-съвместим REST API |
| 7 | Свързахте се от Python, JavaScript и C# чрез OpenAI SDK |
| 8 | Тествахте персонализирания модел от край до край с Foundry Local SDK |

Основният извод е, че **всеки трансформър-базиран модел може да работи чрез Foundry Local**, след като бъде компилиран в ONNX формат. OpenAI-съвместимият API означава, че целият съществуващ код на вашето приложение работи без промени; просто сменяте името на модела.

---

## Ключови изводи

| Концепция | Подробности |
|-----------|-------------|
| ONNX Runtime GenAI компилатор на модели | Превръща модели от Hugging Face в ONNX формат с квантизация с една команда |
| ONNX формат | Foundry Local изисква ONNX модели с ONNX Runtime GenAI конфигурация |
| Чат шаблони | Файлът `inference_model.json` указва как да се форматират промптовете за даден модел |
| Хардуерни цели | Компилиране за CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) или WebGPU в зависимост от хардуера |
| Квантизация | По-ниска точност (int4) намалява размер и увеличава скоростта за сметка на точността; fp16 запазва високо качество на GPU |
| API съвместимост | Персонализираните модели използват същия OpenAI-съвместим API като вградените модели |
| Foundry Local SDK | SDK управлява стартирането на услугата, откриването на endpoint и зареждането на модели автоматично както за каталожни, така и за персонализирани модели |

---

## Допълнително четене

| Ресурс | Връзка |
|--------|--------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Ръководство за персонализирани модели на Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Семейство модели Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive документация | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Следващи стъпки

Продължете към [Част 11: Извикване на инструменти с локални модели](part11-tool-calling.md), за да научите как да активирате вашите локални модели да извикват външни функции.

[← Част 9: Whisper гласова транскрипция](part9-whisper-voice-transcription.md) | [Част 11: Извикване на инструменти →](part11-tool-calling.md)