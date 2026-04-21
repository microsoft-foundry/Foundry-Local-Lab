![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Część 10: Używanie własnych modeli lub modeli z Hugging Face z Foundry Local

> **Cel:** Skompilować model Hugging Face do zoptymalizowanego formatu ONNX wymaganego przez Foundry Local, skonfigurować go z szablonem czatu, dodać do lokalnego cache i uruchomić inferencję za pomocą CLI, REST API oraz OpenAI SDK.

## Przegląd

Foundry Local dostarczany jest z wyselekcjonowanym katalogiem prekompilowanych modeli, ale nie jesteś ograniczony do tej listy. Każdy model językowy oparty na transformatorach dostępny na [Hugging Face](https://huggingface.co/) (lub przechowywany lokalnie w formacie PyTorch / Safetensors) może zostać skompilowany do zoptymalizowanego modelu ONNX i udostępniany przez Foundry Local.

Proces kompilacji wykorzystuje **ONNX Runtime GenAI Model Builder**, narzędzie wiersza poleceń dołączone do pakietu `onnxruntime-genai`. Model builder wykonuje ciężką pracę: pobiera wagi źródłowe, konwertuje je do formatu ONNX, stosuje kwantyzację (int4, fp16, bf16) oraz generuje pliki konfiguracyjne (w tym szablon czatu i tokenizator) oczekiwane przez Foundry Local.

W tym laboratorium skompilujesz **Qwen/Qwen3-0.6B** z Hugging Face, zarejestrujesz go w Foundry Local i będziesz z nim rozmawiać całkowicie na swoim urządzeniu.

---

## Cele nauki

Po ukończeniu tego laboratorium będziesz potrafił:

- Wyjaśnić, dlaczego kompilacja własnych modeli jest przydatna i kiedy może być potrzebna
- Zainstalować ONNX Runtime GenAI model builder
- Skompilować model Hugging Face do zoptymalizowanego formatu ONNX jednym poleceniem
- Zrozumieć kluczowe parametry kompilacji (execution provider, precyzja)
- Stworzyć plik konfiguracyjny `inference_model.json` ze szablonem czatu
- Dodać skompilowany model do cache Foundry Local
- Uruchomić inferencję na własnym modelu przy użyciu CLI, REST API i OpenAI SDK

---

## Wymagania wstępne

| Wymaganie | Szczegóły |
|-------------|---------|
| **Foundry Local CLI** | Zainstalowany i dostępny w `PATH` ([Część 1](part1-getting-started.md)) |
| **Python 3.10+** | Wymagany przez ONNX Runtime GenAI model builder |
| **pip** | Menedżer pakietów Pythona |
| **Miejsce na dysku** | Co najmniej 5 GB wolnego na pliki źródłowe i skompilowane modele |
| **Konto Hugging Face** | Niektóre modele wymagają akceptacji licencji przed pobraniem. Qwen3-0.6B wykorzystuje licencję Apache 2.0 i jest dostępny bezpłatnie. |

---

## Konfiguracja środowiska

Kompilacja modeli wymaga kilku dużych pakietów Pythona (PyTorch, ONNX Runtime GenAI, Transformers). Utwórz dedykowane środowisko wirtualne, aby nie kolidowało z systemowym Pythonem lub innymi projektami.

```bash
# Z katalogu głównego repozytorium
python -m venv .venv
```

Aktywuj środowisko:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Zaktualizuj pip, aby uniknąć problemów z rozwiązywaniem zależności:

```bash
python -m pip install --upgrade pip
```

> **Wskazówka:** Jeśli masz już `.venv` z poprzednich laboratoriów, możesz go ponownie użyć. Po prostu upewnij się, że jest aktywowany przed kontynuacją.

---

## Koncepcja: Pipeline kompilacji

Foundry Local wymaga modeli w formacie ONNX z konfiguracją ONNX Runtime GenAI. Większość otwartoźródłowych modeli na Hugging Face dystrybuowana jest jako wagi PyTorch lub Safetensors, więc potrzebny jest krok konwersji.

![Proces kompilacji własnego modelu](../../../images/custom-model-pipeline.svg)

### Co robi Model Builder?

1. **Pobiera** model źródłowy z Hugging Face (lub odczytuje go z lokalnej ścieżki).
2. **Konwertuje** wagi PyTorch / Safetensors do formatu ONNX.
3. **Kwantyzuje** model do mniejszej precyzji (np. int4), aby zmniejszyć użycie pamięci i poprawić przepustowość.
4. **Generuje** konfigurację ONNX Runtime GenAI (`genai_config.json`), szablon czatu (`chat_template.jinja`) oraz wszystkie pliki tokenizatora, aby Foundry Local mógł załadować i udostępniać model.

### ONNX Runtime GenAI Model Builder a Microsoft Olive

Możesz napotkać odniesienia do **Microsoft Olive** jako alternatywnego narzędzia do optymalizacji modeli. Oba narzędzia mogą produkować modele ONNX, ale służą różnym celom i mają odmienne kompromisy:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Pakiet** | `onnxruntime-genai` | `olive-ai` |
| **Główny cel** | Konwersja i kwantyzacja modeli generatywnych AI dla ONNX Runtime GenAI inference | Kompleksowe środowisko optymalizacji modeli wspierające wiele backendów i celów sprzętowych |
| **Łatwość użycia** | Jedno polecenie — konwersja i kwantyzacja w jednej operacji | Oparte na workflow — konfigurowalne wieloetapowe pipeline’y z YAML/JSON |
| **Format wyjściowy** | Format ONNX Runtime GenAI (gotowy dla Foundry Local) | ONNX ogólne, ONNX Runtime GenAI lub inne, zależnie od workflow |
| **Cele sprzętowe** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN i inne |
| **Opcje kwantyzacji** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus optymalizacje grafu, strojenie warstwa po warstwie |
| **Zakres modeli** | Modele generatywne AI (LLM, SLM) | Każdy model konwertowalny do ONNX (wizja, NLP, audio, multimodalny) |
| **Najlepsze zastosowanie** | Szybka kompilacja pojedynczego modelu do lokalnej inferencji | Produkcyjne pipeline’y wymagające precyzyjnej kontroli optymalizacji |
| **Zależności** | Średnie (PyTorch, Transformers, ONNX Runtime) | Większe (dodaje framework Olive, opcjonalne dodatki dla workflow) |
| **Integracja z Foundry Local** | Bezpośrednia — wynik od razu kompatybilny | Wymaga flagi `--use_ort_genai` i dodatkowej konfiguracji |

> **Dlaczego to laboratorium używa Model Buildera:** Do zadania kompilacji pojedynczego modelu Hugging Face i rejestracji w Foundry Local, Model Builder jest najprostszym i najpewniejszym rozwiązaniem. Produkuje dokładny format wyjściowy wymagany przez Foundry Local w jednym poleceniu. Jeśli później potrzebujesz zaawansowanych funkcji optymalizacji — takich jak kwantyzacja świadoma dokładności, modyfikacje grafu czy strojenie wieloetapowe — Olive jest potężną opcją do zbadania. Zobacz [dokumentację Microsoft Olive](https://microsoft.github.io/Olive/) po więcej informacji.

---

## Ćwiczenia laboratoryjne

### Ćwiczenie 1: Instalacja ONNX Runtime GenAI Model Builder

Zainstaluj pakiet ONNX Runtime GenAI, zawierający narzędzie model builder:

```bash
pip install onnxruntime-genai
```

Zweryfikuj instalację, sprawdzając dostępność model buildera:

```bash
python -m onnxruntime_genai.models.builder --help
```

Powinieneś zobaczyć pomoc zawierającą parametry takie jak `-m` (nazwa modelu), `-o` (ścieżka wyjściowa), `-p` (precyzja), `-e` (execution provider).

> **Uwaga:** Model builder zależy od PyTorch, Transformers oraz kilku innych pakietów. Instalacja może potrwać kilka minut.

---

### Ćwiczenie 2: Kompilacja Qwen3-0.6B dla CPU

Uruchom następujące polecenie, aby pobrać model Qwen3-0.6B z Hugging Face i skompilować go do inferencji na CPU z kwantyzacją int4:

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

#### Znaczenie poszczególnych parametrów

| Parametr | Cel | Użyta wartość |
|-----------|---------|------------|
| `-m` | ID modelu Hugging Face lub lokalna ścieżka katalogu | `Qwen/Qwen3-0.6B` |
| `-o` | Katalog, w którym zostanie zapisany skompilowany model ONNX | `models/qwen3` |
| `-p` | Precyzja kwantyzacji stosowana podczas kompilacji | `int4` |
| `-e` | Execution provider ONNX Runtime (docelowy sprzęt) | `cpu` |
| `--extra_options hf_token=false` | Pomija uwierzytelnianie Hugging Face (dobrze dla modeli publicznych) | `hf_token=false` |

> **Ile to trwa?** Czas kompilacji zależy od sprzętu i wielkości modelu. Dla Qwen3-0.6B z kwantyzacją int4 na nowoczesnym CPU, spodziewaj się około 5 do 15 minut. Większe modele zajmują proporcjonalnie więcej czasu.

Po ukończeniu polecenia powinien pojawić się katalog `models/qwen3` zawierający pliki skompilowanego modelu. Zweryfikuj zawartość:

```bash
ls models/qwen3
```

Powinieneś zobaczyć pliki, m.in.:
- `model.onnx` i `model.onnx.data` — skompilowane wagi modelu
- `genai_config.json` — konfiguracja ONNX Runtime GenAI
- `chat_template.jinja` — szablon czatu modelu (wygenerowany automatycznie)
- `tokenizer.json`, `tokenizer_config.json` — pliki tokenizatora
- Inne pliki słownika i konfiguracyjne

---

### Ćwiczenie 3: Kompilacja dla GPU (Opcjonalnie)

Jeśli masz GPU NVIDIA z obsługą CUDA, możesz skompilować wariant zoptymalizowany pod GPU dla szybszej inferencji:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Uwaga:** Kompilacja GPU wymaga `onnxruntime-gpu` i działającej instalacji CUDA. Jeśli czegoś brakuje, model builder zgłosi błąd. Możesz pominąć to ćwiczenie i kontynuować z wariantem CPU.

#### Odniesienie do kompilacji sprzętowo-specyficznej

| Cel sprzętowy | Execution Provider (`-e`) | Zalecana precyzja (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` lub `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` lub `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Kompromisy precyzji

| Precyzja | Rozmiar | Szybkość | Jakość |
|-----------|------|-------|---------|
| `fp32` | Największy | Najwolniejszy | Najwyższa dokładność |
| `fp16` | Duży | Szybki (GPU) | Bardzo dobra dokładność |
| `int8` | Mały | Szybki | Delikatna utrata dokładności |
| `int4` | Najmniejszy | Najszybszy | Umiarkowana utrata dokładności |

Dla większości lokalnych zastosowań `int4` na CPU zapewnia najlepszą równowagę między szybkością a zużyciem zasobów. Do produkcyjnej jakości zaleca się `fp16` na GPU CUDA.

---

### Ćwiczenie 4: Utworzenie konfiguracji szablonu czatu

Model builder automatycznie generuje pliki `chat_template.jinja` i `genai_config.json` w katalogu wyjściowym. Jednak Foundry Local potrzebuje także pliku `inference_model.json`, który określa sposób formatowania promptów dla Twojego modelu. Plik ten definiuje nazwę modelu oraz szablon promptu, który otacza wiadomości użytkownika odpowiednimi specjalnymi tokenami.

#### Krok 1: Sprawdzenie plików skompilowanych

Wylistuj zawartość katalogu skompilowanego modelu:

```bash
ls models/qwen3
```

Powinieneś zobaczyć pliki takie jak:
- `model.onnx` i `model.onnx.data` — skompilowane wagi modelu
- `genai_config.json` — konfiguracja ONNX Runtime GenAI (wygenerowana automatycznie)
- `chat_template.jinja` — szablon czatu modelu (automatycznie wygenerowany)
- `tokenizer.json`, `tokenizer_config.json` — pliki tokenizatora
- Różne inne pliki konfiguracyjne i słownikowe

#### Krok 2: Wygeneruj plik inference_model.json

Plik `inference_model.json` mówi Foundry Local, jak formatować prompt. Stwórz skrypt Pythona o nazwie `generate_chat_template.py` **w katalogu głównym repozytorium** (tym samym, w którym jest folder `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Zbuduj minimalną rozmowę do wyodrębnienia szablonu czatu
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

# Zbuduj strukturę pliku inference_model.json
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

Uruchom skrypt z katalogu głównego repozytorium:

```bash
python generate_chat_template.py
```

> **Uwaga:** Pakiet `transformers` był już zainstalowany jako zależność `onnxruntime-genai`. Jeśli pojawi się `ImportError`, uruchom najpierw `pip install transformers`.

Skrypt utworzy plik `inference_model.json` wewnątrz katalogu `models/qwen3`. Plik ten mówi Foundry Local, jak otoczyć wejście użytkownika odpowiednimi specjalnymi tokenami dla Qwen3.

> **Ważne:** Pole `"Name"` w `inference_model.json` (ustawione na `qwen3-0.6b` w tym skrypcie) jest **aliasem modelu**, którego będziesz używać w kolejnych poleceniach i wywołaniach API. Jeśli zmienisz tę nazwę, dostosuj nazwę modelu w Ćwiczeniach 6–10.

#### Krok 3: Zweryfikuj konfigurację

Otwórz `models/qwen3/inference_model.json` i sprawdź, czy zawiera pole `Name` oraz obiekt `PromptTemplate` z kluczami `assistant` i `prompt`. Szablon promptu powinien zawierać specjalne tokeny, takie jak `<|im_start|>` i `<|im_end|>` (dokładne tokeny zależą od szablonu czatu modelu).

> **Alternatywa ręczna:** Jeśli nie chcesz uruchamiać skryptu, możesz utworzyć plik ręcznie. Kluczowe jest, aby pole `prompt` zawierało pełny szablon czatu modelu z `{Content}` jako miejscem na wiadomość użytkownika.

---

### Ćwiczenie 5: Zweryfikuj strukturę katalogu modelu
Kreator modelu umieszcza wszystkie skompilowane pliki bezpośrednio w określonym katalogu wyjściowym. Zweryfikuj, czy ostateczna struktura wygląda poprawnie:

```bash
ls models/qwen3
```

Katalog powinien zawierać następujące pliki:

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

> **Uwaga:** W przeciwieństwie do niektórych innych narzędzi do kompilacji, kreator modelu nie tworzy zagnieżdżonych podkatalogów. Wszystkie pliki znajdują się bezpośrednio w folderze wyjściowym, co jest dokładnie tym, czego oczekuje Foundry Local.

---

### Ćwiczenie 6: Dodaj model do pamięci podręcznej Foundry Local

Powiedz Foundry Local, gdzie znaleźć skompilowany model, dodając katalog do jego pamięci podręcznej:

```bash
foundry cache cd models/qwen3
```

Sprawdź, czy model pojawił się w pamięci podręcznej:

```bash
foundry cache ls
```

Powinieneś zobaczyć swój niestandardowy model wyświetlony obok wcześniej zapisanych modeli w pamięci podręcznej (takich jak `phi-3.5-mini` lub `phi-4-mini`).

---

### Ćwiczenie 7: Uruchom niestandardowy model za pomocą CLI

Uruchom interaktywną sesję czatu z nowo skompilowanym modelem (alias `qwen3-0.6b` pochodzi z pola `Name` ustawionego w `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Flaga `--verbose` wyświetla dodatkowe informacje diagnostyczne, co jest pomocne podczas pierwszego testowania niestandardowego modelu. Jeśli model zostanie pomyślnie załadowany, zobaczysz interaktywny prompt. Wypróbuj kilka wiadomości:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Wpisz `exit` lub naciśnij `Ctrl+C`, aby zakończyć sesję.

> **Rozwiązywanie problemów:** Jeśli model nie ładuje się, sprawdź następujące kwestie:
> - Plik `genai_config.json` został wygenerowany przez kreatora modelu.
> - Plik `inference_model.json` istnieje i jest poprawnym plikiem JSON.
> - Pliki modelu ONNX znajdują się w odpowiednim katalogu.
> - Masz wystarczającą ilość dostępnej pamięci RAM (Qwen3-0.6B int4 potrzebuje około 1 GB).
> - Qwen3 to model rozumujący, który generuje tagi `<think>`. Jeśli widzisz `<think>...</think>` na początku odpowiedzi, to normalne zachowanie. Szablon promptu w `inference_model.json` można dostosować, aby wyciszyć wyjście „myślenia”.

---

### Ćwiczenie 8: Zadawaj zapytania do niestandardowego modelu za pomocą REST API

Jeśli opuściłeś interaktywną sesję w Ćwiczeniu 7, model może nie być już załadowany. Najpierw uruchom usługę Foundry Local i załaduj model:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Sprawdź, na jakim porcie działa usługa:

```bash
foundry service status
```

Następnie wyślij zapytanie (zamień `5273` na swój faktyczny port, jeśli jest inny):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Uwaga Windows:** Powyższe polecenie `curl` używa składni bash. Na Windows użyj zamiast tego cmdletu PowerShell `Invoke-RestMethod` poniżej.

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

### Ćwiczenie 9: Użyj niestandardowego modelu z OpenAI SDK

Możesz połączyć się ze swoim niestandardowym modelem, używając dokładnie tego samego kodu OpenAI SDK, którego używałeś dla wbudowanych modeli (zobacz [Część 3](part3-sdk-and-apis.md)). Jedyne co się zmienia, to nazwa modelu.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local nie weryfikuje kluczy API
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
  apiKey: "foundry-local", // Foundry Local nie weryfikuje kluczy API
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

> **Kluczowa uwaga:** Ponieważ Foundry Local udostępnia kompatybilne z OpenAI API, każdy kod działający z wbudowanymi modelami działa również z Twoimi niestandardowymi modelami. Wystarczy zmienić parametr `model`.

---

### Ćwiczenie 10: Przetestuj niestandardowy model za pomocą Foundry Local SDK

We wcześniejszych laboratoriach używałeś Foundry Local SDK, aby uruchomić usługę, wykryć punkt końcowy oraz automatycznie zarządzać modelami. Możesz zastosować dokładnie ten sam wzorzec ze swoim niestandardowo skompilowanym modelem. SDK obsługuje uruchamianie usługi i wykrywanie punktu końcowego, więc Twój kod nie musi na sztywno wpisywać `localhost:5273`.

> **Uwaga:** Upewnij się, że Foundry Local SDK jest zainstalowane przed uruchomieniem poniższych przykładów:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Dodaj pakiety NuGet `Microsoft.AI.Foundry.Local` i `OpenAI`
>
> Zapisz każdy plik skryptu **w katalogu głównym repozytorium** (tym samym, w którym znajduje się folder `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Krok 1: Uruchom usługę Foundry Local i załaduj niestandardowy model
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Krok 2: Sprawdź pamięć podręczną dla niestandardowego modelu
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Krok 3: Załaduj model do pamięci
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Krok 4: Utwórz klienta OpenAI używając punktu końcowego wykrytego przez SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Krok 5: Wyślij żądanie uzupełniania czatu w trybie strumieniowym
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

Uruchom:

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

// Krok 1: Uruchom lokalną usługę Foundry
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Krok 2: Pobierz niestandardowy model z katalogu
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Krok 3: Załaduj model do pamięci
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Krok 4: Utwórz klienta OpenAI, używając punktu końcowego wykrytego przez SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Krok 5: Wyślij żądanie strumieniowego uzupełnienia czatu
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

Uruchom:

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

> **Kluczowa uwaga:** Foundry Local SDK dynamicznie wykrywa punkt końcowy, więc nigdy nie wpisujesz na sztywno numeru portu. To zalecane podejście dla aplikacji produkcyjnych. Twój niestandardowo skompilowany model działa identycznie jak modele z katalogu przez SDK.

---

## Wybór modelu do kompilacji

Qwen3-0.6B używany jest jako przykładowy model w tym laboratorium, ponieważ jest mały, szybko się kompiluje i jest dostępny na licencji Apache 2.0. Możesz jednak skompilować wiele innych modeli. Oto kilka sugestii:

| Model | ID na Hugging Face | Parametry | Licencja | Uwagi |
|-------|--------------------|-----------|----------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Bardzo mały, szybka kompilacja, dobry do testów |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Lepsza jakość, nadal szybka kompilacja |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Mocna jakość, wymaga więcej RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Wymaga akceptacji licencji na Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Wysoka jakość, większe pobieranie i dłuższa kompilacja |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Już w katalogu Foundry Local (przydatne do porównania) |

> **Przypomnienie o licencji:** Zawsze sprawdzaj licencję modelu na Hugging Face przed jego użyciem. Niektóre modele (np. Llama) wymagają zaakceptowania umowy licencyjnej i uwierzytelnienia przez `huggingface-cli login` przed pobraniem.

---

## Koncepcje: Kiedy używać niestandardowych modeli

| Scenariusz | Dlaczego kompilować własny model? |
|------------|----------------------------------|
| **Model, którego potrzebujesz, nie ma w katalogu** | Katalog Foundry Local jest kuratorowany. Jeśli model, którego chcesz, nie jest wymieniony, skompiluj go samodzielnie. |
| **Modele dostrojone (fine-tuned)** | Jeśli dostroiłeś model na danych specyficznych dla domeny, musisz skompilować własne wagi. |
| **Specyficzne wymagania dotyczące kwantyzacji** | Możesz chcieć precyzji lub strategii kwantyzacji innej niż domyślna w katalogu. |
| **Nowsze wydania modeli** | Gdy nowy model jest wydany na Hugging Face, może jeszcze nie być w katalogu Foundry Local. Skompilowanie go daje natychmiastowy dostęp. |
| **Badania i eksperymenty** | Próba różnych architektur modeli, rozmiarów lub konfiguracji lokalnie zanim podejmiesz decyzję produkcyjną. |

---

## Podsumowanie

W tym laboratorium nauczyłeś się, jak:

| Krok | Co zrobiłeś |
|------|-------------|
| 1 | Zainstalowałeś kreator modelu ONNX Runtime GenAI |
| 2 | Skompilowałeś `Qwen/Qwen3-0.6B` z Hugging Face do zoptymalizowanego modelu ONNX |
| 3 | Utworzyłeś plik konfiguracyjny szablonu czatu `inference_model.json` |
| 4 | Dodałeś skompilowany model do pamięci podręcznej Foundry Local |
| 5 | Uruchomiłeś interaktywny czat z niestandardowym modelem przez CLI |
| 6 | Zapytania do modelu przez kompatybilne z OpenAI REST API |
| 7 | Połączyłeś się z poziomu Pythona, JavaScript i C# używając OpenAI SDK |
| 8 | Przetestowałeś niestandardowy model end-to-end używając Foundry Local SDK |

Główny wniosek to, że **dowolny model oparty na transformerach może działać przez Foundry Local** po skompilowaniu do formatu ONNX. Kompatybilne API OpenAI oznacza, że cały Twój istniejący kod aplikacji działa bez zmian; wystarczy tylko wymienić nazwę modelu.

---

## Kluczowe wnioski

| Koncepcja | Szczegóły |
|-----------|-----------|
| Kreator modeli ONNX Runtime GenAI | Konwertuje modele z Hugging Face do formatu ONNX z kwantyzacją w pojedynczym poleceniu |
| Format ONNX | Foundry Local wymaga modeli ONNX z konfiguracją ONNX Runtime GenAI |
| Szablony czatów | Plik `inference_model.json` mówi Foundry Local, jak formatować prompt dla danego modelu |
| Docelowy sprzęt | Kompiluj na CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) lub WebGPU w zależności od sprzętu |
| Kwantyzacja | Niższa precyzja (int4) zmniejsza rozmiar i przyspiesza kosztem dokładności; fp16 zachowuje wysoką jakość na GPU |
| Kompatybilność API | Niestandardowe modele korzystają z tego samego kompatybilnego z OpenAI API co modele wbudowane |
| Foundry Local SDK | SDK automatycznie obsługuje uruchamianie usługi, wykrywanie punktu końcowego i ładowanie modeli zarówno katalogowych, jak i niestandardowych |

---

## Dalsza literatura

| Zasób | Link |
|-------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Przewodnik po modelach Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Rodzina modeli Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Dokumentacja Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Kolejne kroki

Kontynuuj do [Części 11: Wywoływanie narzędzi z lokalnymi modelami](part11-tool-calling.md), aby dowiedzieć się, jak umożliwić Twoim lokalnym modelom wywoływanie zewnętrznych funkcji.

[← Część 9: Transkrypcja głosu Whisper](part9-whisper-voice-transcription.md) | [Część 11: Wywoływanie narzędzi →](part11-tool-calling.md)