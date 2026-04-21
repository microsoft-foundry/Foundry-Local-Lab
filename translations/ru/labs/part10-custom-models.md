![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Часть 10: Использование кастомных моделей или моделей Hugging Face с Foundry Local

> **Цель:** Скомпилировать модель Hugging Face в оптимизированный формат ONNX, который требует Foundry Local, настроить её с шаблоном чата, добавить в локальный кэш и выполнить вывод с помощью CLI, REST API и OpenAI SDK.

## Обзор

Foundry Local поставляется с курированным каталогом предварительно скомпилированных моделей, но вы не ограничены этим списком. Любая трансформерная языковая модель, доступная на [Hugging Face](https://huggingface.co/) (или хранящаяся локально в формате PyTorch / Safetensors), может быть скомпилирована в оптимизированную ONNX-модель и обслуживаться через Foundry Local.

Конвейер компиляции использует **ONNX Runtime GenAI Model Builder**, инструмент командной строки, включённый в пакет `onnxruntime-genai`. Model Builder выполняет основную работу: скачивает исходные веса, конвертирует их в формат ONNX, применяет квантизацию (int4, fp16, bf16) и генерирует конфигурационные файлы (включая шаблон чата и токенизатор), которые ожидает Foundry Local.

В этой лабораторной работе вы скомпилируете **Qwen/Qwen3-0.6B** с Hugging Face, зарегистрируете её в Foundry Local и будете общаться с моделью полностью на вашем устройстве.

---

## Цели обучения

К концу этой лабораторной работы вы сможете:

- Объяснить, зачем нужна кастомная компиляция моделей и когда она необходима
- Установить ONNX Runtime GenAI Model Builder
- Скомпилировать модель Hugging Face в оптимизированный ONNX одним командным вызовом
- Понять ключевые параметры компиляции (провайдер выполнения, точность)
- Создать файл конфигурации шаблона чата `inference_model.json`
- Добавить скомпилированную модель в локальный кэш Foundry Local
- Выполнить инференс кастомной модели с помощью CLI, REST API и OpenAI SDK

---

## Необходимые условия

| Требование | Подробности |
|-------------|-------------|
| **Foundry Local CLI** | Установлен и доступен в `PATH` ([Часть 1](part1-getting-started.md)) |
| **Python 3.10+** | Требуется для ONNX Runtime GenAI Model Builder |
| **pip** | Менеджер пакетов Python |
| **Свободное место на диске** | Не менее 5 ГБ для исходных и скомпилированных файлов модели |
| **Аккаунт Hugging Face** | Для некоторых моделей нужно принять лицензию перед скачиванием. Qwen3-0.6B использует лицензию Apache 2.0 и доступна бесплатно. |

---

## Настройка окружения

Компиляция модели требует нескольких больших Python-библиотек (PyTorch, ONNX Runtime GenAI, Transformers). Создайте отдельное виртуальное окружение, чтобы они не мешали системному Python или другим проектам.

```bash
# От корня репозитория
python -m venv .venv
```
  
Активируйте окружение:

**Windows (PowerShell):**  
```powershell
.venv\Scripts\Activate.ps1
```
  
**macOS / Linux:**  
```bash
source .venv/bin/activate
```
  
Обновите pip, чтобы избежать проблем с разрешением зависимостей:

```bash
python -m pip install --upgrade pip
```
  
> **Совет:** Если у вас уже есть `.venv` из предыдущих лабораторных, вы можете использовать его повторно. Просто убедитесь, что оно активировано перед продолжением.

---

## Концепция: Конвейер компиляции

Foundry Local требует модели в формате ONNX с конфигурацией ONNX Runtime GenAI. Большинство open-source моделей на Hugging Face распространяются в виде весов PyTorch или Safetensors, поэтому нужен этап конвертации.

![Конвейер компиляции кастомной модели](../../../images/custom-model-pipeline.svg)

### Что делает Model Builder?

1. **Скачивает** исходную модель с Hugging Face (или читает с локального пути).  
2. **Конвертирует** веса PyTorch / Safetensors в формат ONNX.  
3. **Квантизирует** модель до меньшей точности (например, int4) для снижения объёма памяти и увеличения пропускной способности.  
4. **Генерирует** конфигурацию ONNX Runtime GenAI (`genai_config.json`), шаблон чата (`chat_template.jinja`) и все файлы токенизатора для загрузки и обслуживания модели Foundry Local.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Вы можете встретить упоминания **Microsoft Olive** как альтернативного инструмента для оптимизации моделей. Оба инструмента могут производить модели ONNX, но у них разные задачи и компромиссы:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Пакет** | `onnxruntime-genai` | `olive-ai` |
| **Основная задача** | Конвертация и квантизация генеративных ИИ моделей для инференса ONNX Runtime GenAI | Комплексная система оптимизации моделей с поддержкой множества бэкендов и аппаратных целей |
| **Удобство использования** | Одна команда — конвертация и квантизация за один шаг | Построение по workflow — настраиваемые многошаговые пайплайны с YAML/JSON |
| **Формат вывода** | ONNX Runtime GenAI (готов к Foundry Local) | Общий ONNX, ONNX Runtime GenAI и др. в зависимости от workflow |
| **Поддержка аппаратуры** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN и др. |
| **Опции квантизации** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, графовые оптимизации, покадровая настройка |
| **Область применения** | Генеративные модели ИИ (LLM, SLM) | Любые модели, конвертируемые в ONNX (визуальные, NLP, аудио, мультимодальные) |
| **Идеален для** | Быстрой компиляции одной модели для локального инференса | Производственные пайплайны с тонкой настройкой |
| **Зависимости** | Средние (PyTorch, Transformers, ONNX Runtime) | Большие (включает фреймворк Olive, дополнительные опции под workflow) |
| **Интеграция с Foundry Local** | Прямая — результат сразу совместим | Требуется флаг `--use_ort_genai` и дополнительная конфигурация |

> **Почему в этой лабораторной используется Model Builder:** Для задачи компиляции одной модели Hugging Face и регистрации её в Foundry Local, Model Builder — самый простой и надёжный путь. Он выдаёт именно тот формат, который ожидает Foundry Local, одной командой. Если позже вам понадобятся более сложные функции оптимизации — например, квантизация с учётом точности, графовые модификации или многошаговая настройка — Olive станет мощным вариантом. Подробнее в [документации Microsoft Olive](https://microsoft.github.io/Olive/).

---

## Лабораторные упражнения

### Упражнение 1: Установка ONNX Runtime GenAI Model Builder

Установите пакет ONNX Runtime GenAI, который содержит утилиту model builder:

```bash
pip install onnxruntime-genai
```
  
Проверьте установку, убедившись, что model builder доступен:

```bash
python -m onnxruntime_genai.models.builder --help
```
  
Вы должны увидеть справку с параметрами, такими как `-m` (имя модели), `-o` (путь вывода), `-p` (точность), `-e` (провайдер исполнения).

> **Примечание:** Model builder зависит от PyTorch, Transformers и других пакетов. Установка может занять несколько минут.

---

### Упражнение 2: Компиляция Qwen3-0.6B для CPU

Выполните следующую команду, чтобы скачать модель Qwen3-0.6B с Hugging Face и скомпилировать её для инференса на CPU с квантизацией int4:

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
  
#### Значение параметров

| Параметр | Назначение | Используемое значение |
|-----------|------------|----------------------|
| `-m` | ID модели Hugging Face или локальный путь | `Qwen/Qwen3-0.6B` |
| `-o` | Каталог для сохранения скомпилированной ONNX-модели | `models/qwen3` |
| `-p` | Точность квантизации во время компиляции | `int4` |
| `-e` | Провайдер выполнения ONNX Runtime (целевое оборудование) | `cpu` |
| `--extra_options hf_token=false` | Пропускает аутентификацию Hugging Face (для публичных моделей) | `hf_token=false` |

> **Сколько это занимает времени?** Время компиляции зависит от вашего оборудования и размера модели. Для Qwen3-0.6B с квантизацией int4 на современном CPU — примерно 5–15 минут. Большие модели требуют пропорционально больше времени.

После завершения команды должно появиться директория `models/qwen3` с скомпилированными файлами модели. Проверьте результат:

```bash
ls models/qwen3
```
  
Вы должны увидеть файлы, включая:  
- `model.onnx` и `model.onnx.data` — веса скомпилированной модели  
- `genai_config.json` — конфигурация ONNX Runtime GenAI  
- `chat_template.jinja` — шаблон чата модели (создан автоматически)  
- `tokenizer.json`, `tokenizer_config.json` — файлы токенизатора  
- Другие файлы словарей и конфигурации

---

### Упражнение 3: Компиляция для GPU (опционально)

Если у вас есть GPU NVIDIA с поддержкой CUDA, вы можете скомпилировать оптимизированный для GPU вариант для более быстрого инференса:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```
  
> **Примечание:** Компиляция для GPU требует `onnxruntime-gpu` и корректной установки CUDA. Если этого нет, модель builder выдаст ошибку. Вы можете пропустить это упражнение и продолжить с вариантом для CPU.

#### Справка по аппаратным целям

| Цель | Execution Provider (`-e`) | Рекомендуемая точность (`-p`) |
|-------|-------------------------|-------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` или `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` или `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Компромиссы точности

| Точность | Размер | Скорость | Качество |
|----------|--------|----------|----------|
| `fp32` | Максимальный | Самый медленный | Максимальная точность |
| `fp16` | Большой | Быстрый (GPU) | Очень хорошая точность |
| `int8` | Малый | Быстрый | Небольшая потеря точности |
| `int4` | Минимальный | Самый быстрый | Умеренная потеря точности |

Для большинства локальных сценариев разработки `int4` на CPU даёт лучший баланс скорости и использования ресурсов. Для продакшен-качества лучше использовать `fp16` на CUDA GPU.

---

### Упражнение 4: Создание конфигурации шаблона чата

Model builder автоматически создаёт файл `chat_template.jinja` и `genai_config.json` в каталоге вывода. Однако Foundry Local также нужен файл `inference_model.json`, чтобы понимать, как форматировать запросы для вашей модели. Этот файл определяет имя модели и шаблон подсказки, который оборачивает сообщения пользователя в правильные специальные токены.

#### Шаг 1: Осмотр скомпилированного вывода

Просмотрите содержимое каталога скомпилированной модели:

```bash
ls models/qwen3
```
  
Вы должны увидеть файлы, такие как:  
- `model.onnx` и `model.onnx.data` — веса скомпилированной модели  
- `genai_config.json` — конфигурация ONNX Runtime GenAI (создана автоматически)  
- `chat_template.jinja` — шаблон чата модели (создан автоматически)  
- `tokenizer.json`, `tokenizer_config.json` — файлы токенизатора  
- Другие конфигурационные и словарные файлы

#### Шаг 2: Создание файла inference_model.json

Файл `inference_model.json` рассказывает Foundry Local, как форматировать запросы. Создайте Python-скрипт с именем `generate_chat_template.py` **в корне репозитория** (в той же папке, где находится каталог `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Постройте минимальный диалог для извлечения шаблона чата
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

# Постройте структуру inference_model.json
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
  
Запустите скрипт из корня репозитория:

```bash
python generate_chat_template.py
```
  
> **Примечание:** Пакет `transformers` уже был установлен как зависимость `onnxruntime-genai`. Если появится ошибка `ImportError`, запустите сначала `pip install transformers`.

Скрипт создаст файл `inference_model.json` внутри папки `models/qwen3`. Этот файл сообщает Foundry Local, как правильно оборачивать ввод пользователя в специальные токены для Qwen3.

> **Важно:** Поле `"Name"` в `inference_model.json` (в этом скрипте установлено как `qwen3-0.6b`) — это **алиас модели**, который вы будете использовать во всех последующих командах и API вызовах. Если измените это имя, обновите имя модели в упражнениях 6–10 соответственно.

#### Шаг 3: Проверка конфигурации

Откройте `models/qwen3/inference_model.json` и убедитесь, что там есть поле `Name` и объект `PromptTemplate` с ключами `assistant` и `prompt`. Шаблон подсказки должен содержать специальные токены, такие как `<|im_start|>` и `<|im_end|>` (точные токены зависят от шаблона чата модели).

> **Альтернатива вручную:** Если вы не хотите запускать скрипт, вы можете создать файл вручную. Главное требование — поле `prompt` должно содержать полный шаблон чата модели с `{Content}` в качестве заполнителя для сообщения пользователя.

---

### Упражнение 5: Проверка структуры каталога модели
Построитель моделей помещает все скомпилированные файлы непосредственно в указанный вами каталог вывода. Проверьте, что итоговая структура выглядит правильно:

```bash
ls models/qwen3
```

В каталоге должны находиться следующие файлы:

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

> **Примечание:** В отличие от некоторых других инструментов компиляции, построитель моделей не создаёт вложенные подпапки. Все файлы располагаются непосредственно в папке вывода, что именно и ожидает Foundry Local.

---

### Упражнение 6: Добавьте модель в кэш Foundry Local

Сообщите Foundry Local, где найти вашу скомпилированную модель, добавив каталог в его кэш:

```bash
foundry cache cd models/qwen3
```

Проверьте, что модель появилась в кэше:

```bash
foundry cache ls
```

Вы должны увидеть вашу кастомную модель в списке вместе с ранее закэшированными моделями (например, `phi-3.5-mini` или `phi-4-mini`).

---

### Упражнение 7: Запустите кастомную модель через CLI

Начните интерактивную сессию чата с вашей недавно скомпилированной моделью (псевдоним `qwen3-0.6b` взят из поля `Name` в файле `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Флаг `--verbose` показывает дополнительную диагностическую информацию, что полезно при первом тестировании кастомной модели. Если модель загрузится успешно, вы увидите интерактивный приглашение. Попробуйте несколько сообщений:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Введите `exit` или нажмите `Ctrl+C`, чтобы завершить сессию.

> **Решение проблем:** Если модель не загружается, проверьте следующее:
> - Файл `genai_config.json` был сгенерирован построителем моделей.
> - Файл `inference_model.json` существует и содержит корректный JSON.
> - ONNX-файлы модели находятся в правильном каталоге.
> - У вас достаточно свободной оперативной памяти (Qwen3-0.6B int4 требует примерно 1 ГБ).
> - Qwen3 — это модель рассуждений, которая генерирует теги `<think>`. Если вы видите `<think>...</think>` в начале ответов, это нормальное поведение. Шаблон подсказки в `inference_model.json` можно отредактировать, чтобы подавить вывод “мышления”.

---

### Упражнение 8: Запрос к кастомной модели через REST API

Если вы вышли из интерактивной сессии в упражнении 7, возможно, модель уже не загружена. Сначала запустите сервис Foundry Local и загрузите модель:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Проверьте, на каком порту работает сервис:

```bash
foundry service status
```

Затем отправьте запрос (замените `5273` на фактический порт, если он отличается):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Примечание для Windows:** Команда `curl` выше использует синтаксис bash. В Windows используйте вместо неё командлет PowerShell `Invoke-RestMethod`, приведённый ниже.

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

### Упражнение 9: Использование кастомной модели с OpenAI SDK

Вы можете подключиться к вашей кастомной модели точно так же, как к встроенным моделям, используя OpenAI SDK (см. [Часть 3](part3-sdk-and-apis.md)). Единственное отличие — имя модели.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local не проверяет API-ключи
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
  apiKey: "foundry-local", // Foundry Local не проверяет API ключи
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

> **Главное:** Поскольку Foundry Local предоставляет API, совместимый с OpenAI, любой код, работающий со встроенными моделями, также будет работать с вашими кастомными моделями. Нужно лишь изменить параметр `model`.

---

### Упражнение 10: Тестирование кастомной модели с Foundry Local SDK

В предыдущих лабораторных работах вы использовали Foundry Local SDK для запуска сервиса, обнаружения endpoint и автоматического управления моделями. Вы можете следовать точно такой же схеме с вашей кастомной моделью. SDK управляет запуском сервиса и обнаружением endpoint, так что в коде не нужно хардкодить `localhost:5273`.

> **Примечание:** Убедитесь, что Foundry Local SDK установлен перед запуском этих примеров:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Добавьте NuGet-пакеты `Microsoft.AI.Foundry.Local` и `OpenAI`
>
> Сохраняйте каждый скрипт **в корне репозитория** (в той же папке, что и ваша папка `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Шаг 1: Запустите локальную службу Foundry и загрузите пользовательскую модель
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Шаг 2: Проверьте кеш на наличие пользовательской модели
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Шаг 3: Загрузите модель в память
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Шаг 4: Создайте клиент OpenAI, используя обнаруженный SDK конечный пункт
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Шаг 5: Отправьте запрос на потоковое завершение чата
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

Запустите его:

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

// Шаг 1: Запустите службу Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Шаг 2: Получите пользовательскую модель из каталога
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Шаг 3: Загрузите модель в память
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Шаг 4: Создайте клиент OpenAI, используя обнаруженную SDK конечную точку
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Шаг 5: Отправьте запрос на потоковое завершение чата
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

Запустите его:

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

> **Главное:** Foundry Local SDK динамически обнаруживает endpoint, поэтому порт никогда не хардкодится. Это рекомендованный подход для продакшен-приложений. Ваша кастомная модель работает идентично моделям из каталога через SDK.

---

## Выбор модели для компиляции

Qwen3-0.6B используется в этом лабораторном примере в качестве эталонной, так как она маленькая, быстро компилируется и свободно доступна по лицензии Apache 2.0. Однако вы можете скомпилировать и другие модели. Вот несколько предложений:

| Модель | Hugging Face ID | Параметры | Лицензия | Примечания |
|--------|-----------------|-----------|----------|------------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Очень маленькая, быстрая компиляция, хороша для тестов |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Лучше качество, всё ещё быстро компилируется |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Высокое качество, требует больше RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Требует принятия лицензии на Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Высокое качество, больший размер скачивания и длительная компиляция |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Уже в каталоге Foundry Local (полезно для сравнения) |

> **Напоминание о лицензии:** Всегда проверяйте лицензию модели на Hugging Face перед использованием. Некоторые модели (например, Llama) требуют принять лицензионное соглашение и пройти аутентификацию с помощью `huggingface-cli login` перед загрузкой.

---

## Концепции: когда использовать кастомные модели

| Сценарий | Почему компилировать самостоятельно? |
|----------|-------------------------------------|
| **Модель, которой нет в каталоге** | Каталог Foundry Local отобран вручную. Если нужной модели нет в списке, скомпилируйте её сами. |
| **Тонко настроенные модели** | Если вы дообучали модель на доменных данных, нужно скомпилировать свои веса. |
| **Специфические требования к квантизации** | Возможно, вам нужна точность или стратегия квантизации, отличающаяся от дефолтной в каталоге. |
| **Новые версии моделей** | Когда модель появилась недавно на Hugging Face, она может отсутствовать в каталоге Foundry Local. Компиляция даёт немедленный доступ. |
| **Исследования и эксперименты** | Эксперименты с архитектурой, размером или настройками модели локально до выбора для продакшена. |

---

## Итог

В этой лабораторной работе вы научились:

| Шаг | Что вы сделали |
|-----|----------------|
| 1 | Установили ONNX Runtime GenAI model builder |
| 2 | Скомпилировали `Qwen/Qwen3-0.6B` из Hugging Face в оптимизированный ONNX-модель |
| 3 | Создали конфигурационный файл chat-шаблона `inference_model.json` |
| 4 | Добавили скомпилированную модель в кэш Foundry Local |
| 5 | Запустили интерактивный чат с кастомной моделью через CLI |
| 6 | Выполнили запрос к модели через REST API, совместимый с OpenAI |
| 7 | Подключились через Python, JavaScript и C# с использованием OpenAI SDK |
| 8 | Протестировали кастомную модель полностью с помощью Foundry Local SDK |

Главное — **любая модель на базе трансформеров может работать через Foundry Local**, когда её скомпилировать в формат ONNX. API, совместимый с OpenAI, обеспечивает работу вашего существующего кода без изменений; нужно только поменять имя модели.

---

## Основные выводы

| Концепция | Подробности |
|-----------|-------------|
| ONNX Runtime GenAI Model Builder | Конвертирует модели Hugging Face в ONNX с квантизацией одной командой |
| Формат ONNX | Foundry Local требует ONNX-модели с конфигурацией ONNX Runtime GenAI |
| Chat шаблоны | Файл `inference_model.json` указывает, как форматировать подсказки для данной модели |
| Аппаратные цели | Компиляция для CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) или WebGPU в зависимости от вашего оборудования |
| Квантизация | Меньшая точность (int4) уменьшает размер и ускоряет работу за счёт некоторой потери качества; fp16 сохраняет высокое качество на GPU |
| Совместимость API | Кастомные модели используют тот же OpenAI-совместимый API, что и встроенные модели |
| Foundry Local SDK | SDK автоматически запускает сервис, находит endpoint и загружает модели как из каталога, так и кастомные |

---

## Дополнительные ресурсы

| Ресурс | Ссылка |
|--------|--------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Руководство по кастомным моделям Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Семейство моделей Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Документация Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Следующие шаги

Продолжайте к [Части 11: Вызов инструментов с помощью локальных моделей](part11-tool-calling.md), чтобы узнать, как разрешить вашим локальным моделям вызывать внешние функции.

[← Часть 9: Распознавание речи Whisper](part9-whisper-voice-transcription.md) | [Часть 11: Вызов инструментов →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от ответственности**:  
Этот документ был переведен с использованием сервиса автоматического перевода [Co-op Translator](https://github.com/Azure/co-op-translator). Несмотря на наши усилия по обеспечению точности, пожалуйста, учитывайте, что автоматический перевод может содержать ошибки или неточности. Оригинальный документ на исходном языке следует считать авторитетным источником. Для критически важной информации рекомендуется профессиональный перевод человеком. Мы не несем ответственности за любые недоразумения или неправильные толкования, возникшие в результате использования данного перевода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->