![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Část 10: Použití vlastních nebo Hugging Face modelů s Foundry Local

> **Cíl:** Zkompilovat model Hugging Face do optimalizovaného formátu ONNX, který Foundry Local vyžaduje, nakonfigurovat jej pomocí chatové šablony, přidat jej do lokální cache a provést inference pomocí CLI, REST API a OpenAI SDK.

## Přehled

Foundry Local obsahuje kurátorovaný katalog předkompilovaných modelů, ale nejste omezeni pouze na tento seznam. Jakýkoliv jazykový model založený na transformerech dostupný na [Hugging Face](https://huggingface.co/) (nebo uložený lokálně v PyTorch / Safetensors formátu) lze zkompilovat do optimalizovaného ONNX modelu a obsluhovat přes Foundry Local.

Kompilační pipeline využívá **ONNX Runtime GenAI Model Builder**, nástroj příkazové řádky zahrnutý v balíčku `onnxruntime-genai`. Tento model builder obstarává veškerou náročnou práci: stahuje zdrojové váhy, převádí je do formátu ONNX, aplikuje kvantizaci (int4, fp16, bf16) a vytváří konfigurační soubory (včetně chatové šablony a tokenizéru), které Foundry Local očekává.

V tomto labu zkompilujete **Qwen/Qwen3-0.6B** z Hugging Face, zaregistrujete jej v Foundry Local a budete s ním konverzovat zcela na svém zařízení.

---

## Naučíte se

Na konci tohoto labu budete schopni:

- Vysvětlit, proč je užitečné kompilovat vlastní model a kdy je to potřeba
- Nainstalovat ONNX Runtime GenAI model builder
- Zkompilovat model Hugging Face do optimalizovaného formátu ONNX jedním příkazem
- Rozumět klíčovým parametrům kompilace (výkonový provider, přesnost)
- Vytvořit konfigurační soubor `inference_model.json` s chat šablonou
- Přidat zkompilovaný model do cache Foundry Local
- Provést inference nad vlastním modelem pomocí CLI, REST API a OpenAI SDK

---

## Požadavky

| Požadavek | Podrobnosti |
|-------------|---------|
| **Foundry Local CLI** | Nainstalován a dostupný v `PATH` ([Část 1](part1-getting-started.md)) |
| **Python 3.10+** | Vyžadováno ONNX Runtime GenAI model builderem |
| **pip** | Správce balíčků pro Python |
| **Diskový prostor** | Minimálně 5 GB volného místa pro zdrojové a zkompilované modely |
| **Účet na Hugging Face** | Některé modely vyžadují souhlas s licencí před stažením. Qwen3-0.6B používá licenci Apache 2.0 a je volně dostupný. |

---

## Nastavení prostředí

Kompilace modelu vyžaduje několik velkých Python balíčků (PyTorch, ONNX Runtime GenAI, Transformers). Vytvořte si dedikované virtuální prostředí, aby tyto balíčky nezasahovaly do systému Pythonu nebo jiných projektů.

```bash
# Ze kořene repozitáře
python -m venv .venv
```

Aktivujte prostředí:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Aktualizujte pip, abyste se vyhnuli problémům s řešením závislostí:

```bash
python -m pip install --upgrade pip
```

> **Tip:** Pokud již máte `.venv` z předchozích labů, můžete jej znovu použít. Jen se ujistěte, že je aktivováno před pokračováním.

---

## Koncept: Pipeline kompilace

Foundry Local vyžaduje modely v ONNX formátu s konfigurací ONNX Runtime GenAI. Většina open-source modelů na Hugging Face je distribuována jako PyTorch nebo Safetensors váhy, proto je potřeba převodový krok.

![Schéma pipeline kompilace vlastního modelu](../../../images/custom-model-pipeline.svg)

### Co model builder dělá?

1. **Stáhne** zdrojový model z Hugging Face (nebo jej načte z lokální cesty).
2. **Převede** PyTorch / Safetensors váhy do ONNX formátu.
3. **Kvantizuje** model do menší přesnosti (např. int4) pro snížení paměťové náročnosti a zrychlení průtoku.
4. **Vygeneruje** konfiguraci ONNX Runtime GenAI (`genai_config.json`), chatovou šablonu (`chat_template.jinja`) a všechny tokenizéry, aby Foundry Local mohl model načíst a obsluhovat.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Můžete narazit na zmínky o **Microsoft Olive** jako alternativním nástroji pro optimalizaci modelů. Oba nástroje umí vytvořit ONNX modely, ale slouží jiným účelům a mají rozdílné kompromisy:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Balíček** | `onnxruntime-genai` | `olive-ai` |
| **Hlavní účel** | Převod a kvantizace generativních AI modelů pro ONNX Runtime GenAI inference | Kompletní framework pro optimalizaci modelů podporující mnoho backendů a hardwarových cílů |
| **Jednoduchost použití** | Jediný příkaz – konverze a kvantizace v jednom kroku | Workflow založený – konfigurovatelné vícestupňové pipeline s YAML/JSON |
| **Výstupní formát** | ONNX Runtime GenAI formát (připravený pro Foundry Local) | Obecný ONNX, ONNX Runtime GenAI, nebo jiné formáty podle workflow |
| **Podporovaný hardware** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN a další |
| **Možnosti kvantizace** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus optimalizace grafu, ladění vrstev |
| **Zaměření modelů** | Generativní AI modely (LLM, SLM) | Jakýkoliv ONNX-konvertovatelný model (vision, NLP, audio, multimodální) |
| **Nejvhodnější pro** | Rychlou kompilaci jednoho modelu pro lokální inference | Produkční pipelines vyžadující pokročilou optimalizaci |
| **Závislosti** | Střední (PyTorch, Transformers, ONNX Runtime) | Větší (přidává Olive framework, volitelné extras dle workflow) |
| **Integrace s Foundry Local** | Přímá – výstup je ihned kompatibilní | Vyžaduje příznak `--use_ort_genai` a další konfiguraci |

> **Proč tento lab používá Model Builder:** Pro úkol kompilace jednoho modelu Hugging Face a jeho registraci v Foundry Local je Model Builder nejjednodušší a nejspolehlivější cesta. Vygeneruje přesně takový formát, který Foundry Local očekává, a to jedním příkazem. Pokud později budete potřebovat pokročilé funkce optimalizace — jako přesnostně orientovanou kvantizaci, editaci grafu, nebo ladění ve vícestupňových průchodech — Olive je velmi silná možnost k prozkoumání. Viz [dokumentace Microsoft Olive](https://microsoft.github.io/Olive/) pro více informací.

---

## Cvičení v labu

### Cvičení 1: Instalace ONNX Runtime GenAI Model Builderu

Nainstalujte balíček ONNX Runtime GenAI, který obsahuje model builder nástroj:

```bash
pip install onnxruntime-genai
```

Ověřte instalaci tím, že zkontrolujete dostupnost model builderu:

```bash
python -m onnxruntime_genai.models.builder --help
```

Měli byste vidět nápovědu s parametry jako `-m` (název modelu), `-o` (výstupní cesta), `-p` (přesnost) a `-e` (výkonový provider).

> **Poznámka:** Model builder závisí na PyTorch, Transformers a dalších balíčcích. Instalace může trvat několik minut.

---

### Cvičení 2: Kompilace Qwen3-0.6B pro CPU

Spusťte následující příkaz, který stáhne model Qwen3-0.6B z Hugging Face a zkompiluje jej pro CPU inference s kvantizací int4:

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

#### Co každý parametr znamená

| Parametr | Účel | Použitá hodnota |
|-----------|---------|------------|
| `-m` | ID modelu na Hugging Face nebo lokální adresář | `Qwen/Qwen3-0.6B` |
| `-o` | Adresář pro uložení zkompilovaného ONNX modelu | `models/qwen3` |
| `-p` | Přesnost kvantizace použitá při kompilaci | `int4` |
| `-e` | Výkonový provider ONNX Runtime (cílový hardware) | `cpu` |
| `--extra_options hf_token=false` | Vynechává ověření na Hugging Face (vhodné pro veřejné modely) | `hf_token=false` |

> **Jak dlouho to trvá?** Doba kompilace závisí na vašem hardwaru a velikosti modelu. Pro Qwen3-0.6B s kvantizací int4 na moderním CPU očekávejte zhruba 5 až 15 minut. Větší modely trvají úměrně déle.

Po dokončení by se měl vytvořit adresář `models/qwen3` s modelovými soubory. Ověřte obsah:

```bash
ls models/qwen3
```

Měly by zde být soubory:
- `model.onnx` a `model.onnx.data` — zkompilované váhy modelu
- `genai_config.json` — konfigurace ONNX Runtime GenAI
- `chat_template.jinja` — chatová šablona modelu (automaticky generovaná)
- `tokenizer.json`, `tokenizer_config.json` — soubory tokenizéru
- Další slovníkové a konfigurační soubory

---

### Cvičení 3: Kompilace pro GPU (volitelné)

Pokud máte NVIDIA GPU s podporou CUDA, můžete zkompilovat GPU-optimalizovanou variantu pro rychlejší inference:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Poznámka:** Kompilace pro GPU vyžaduje `onnxruntime-gpu` a funkční instalaci CUDA. Pokud tyto nejsou přítomny, model builder nahlásí chybu. Můžete toto cvičení přeskočit a pokračovat s CPU variantou.

#### Srovnání hardware-specifické kompilace

| Cíl | Výkonový provider (`-e`) | Doporučená přesnost (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` nebo `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` nebo `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Kompromisy přesnosti

| Přesnost | Velikost | Rychlost | Kvalita |
|-----------|------|-------|---------|
| `fp32` | Největší | Nejpomalejší | Nejvyšší přesnost |
| `fp16` | Velká | Rychlá (GPU) | Velmi dobrá přesnost |
| `int8` | Malá | Rychlá | Mírná ztráta přesnosti |
| `int4` | Nejmenší | Nejrychlejší | Střední ztráta přesnosti |

Pro většinu lokálního vývoje je nejlepší poměr rychlosti a využití zdrojů `int4` na CPU. Pro produkční kvalitu se doporučuje `fp16` na CUDA GPU.

---

### Cvičení 4: Vytvoření chatové šablony konfigurace

Model builder automaticky vytvoří soubory `chat_template.jinja` a `genai_config.json` ve výstupním adresáři. Foundry Local však také potřebuje soubor `inference_model.json`, aby věděl, jak formátovat výzvy pro váš model. Tento soubor definuje jméno modelu a šablonu promptu, která správně zabalí uživatelské zprávy do speciálních tokenů.

#### Krok 1: Prohlédněte obsah zkompilovaného výstupu

Vylistujte obsah adresáře se zkompilovaným modelem:

```bash
ls models/qwen3
```

Měly by tam být soubory jako:
- `model.onnx` a `model.onnx.data` — zkompilované váhy modelu
- `genai_config.json` — konfigurace ONNX Runtime GenAI (automaticky vygenerovaná)
- `chat_template.jinja` — chatová šablona modelu (automaticky vygenerovaná)
- `tokenizer.json`, `tokenizer_config.json` — soubory tokenizéru
- Různé další konfigurace a slovníkové soubory

#### Krok 2: Vygenerujte soubor inference_model.json

Soubor `inference_model.json` říká Foundry Local, jak formátovat prompt. Vytvořte Python skript s názvem `generate_chat_template.py` **v kořenovém adresáři repozitáře** (ve stejném adresáři, kde máte složku `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Vytvořte minimální konverzaci pro extrakci šablony chatu
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

# Vytvořte strukturu inference_model.json
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

Spusťte skript z kořenového adresáře repozitáře:

```bash
python generate_chat_template.py
```

> **Poznámka:** Balíček `transformers` byl už nainstalován jako závislost `onnxruntime-genai`. Pokud dostanete chybu `ImportError`, nejprve spusťte `pip install transformers`.

Skript vytvoří soubor `inference_model.json` v adresáři `models/qwen3`. Tento soubor říká Foundry Local, jak správně zabalit uživatelův vstup do speciálních tokenů pro model Qwen3.

> **Důležité:** Hodnota `"Name"` v `inference_model.json` (nastavená na `qwen3-0.6b` v tomto skriptu) je **alias modelu**, který budete používat ve všech následujících příkazech a API voláních. Pokud tento název změníte, upravte název v cvičeních 6–10 odpovídajícím způsobem.

#### Krok 3: Ověřte konfiguraci

Otevřete `models/qwen3/inference_model.json` a ujistěte se, že obsahuje pole `Name` a objekt `PromptTemplate` s klíči `assistant` a `prompt`. Šablona promptu by měla zahrnovat speciální tokeny jako `<|im_start|>` a `<|im_end|>` (konkrétní tokeny závisí na chatové šabloně modelu).

> **Manuální alternativa:** Pokud nechcete spouštět skript, můžete soubor vytvořit ručně. Klíčové je, aby pole `prompt` obsahovalo kompletní chatovou šablonu modelu s `{Content}` jako zástupcem uživatelovy zprávy.

---

### Cvičení 5: Ověření struktury adresáře modelu
Model builder umístí všechny zkompilované soubory přímo do vámi určeného výstupního adresáře. Ověřte, zda konečná struktura vypadá správně:

```bash
ls models/qwen3
```

Adresář by měl obsahovat následující soubory:

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

> **Poznámka:** Na rozdíl od některých jiných nástrojů pro kompilaci modelů model builder nevytváří vnořené podsložky. Všechny soubory jsou umístěny přímo ve výstupní složce, což je přesně to, co Foundry Local očekává.

---

### Cvičení 6: Přidání modelu do mezipaměti Foundry Local

Řekněte Foundry Local, kde najde váš zkompilovaný model, přidáním adresáře do jeho mezipaměti:

```bash
foundry cache cd models/qwen3
```

Ověřte, že se model objeví v mezipaměti:

```bash
foundry cache ls
```

Měli byste vidět svůj vlastní model mezi již uloženými modely v mezipaměti (například `phi-3.5-mini` nebo `phi-4-mini`).

---

### Cvičení 7: Spuštění vlastního modelu pomocí CLI

Spusťte interaktivní chatovací relaci s vaším nově zkompilovaným modelem (alias `qwen3-0.6b` pochází z pole `Name`, které jste nastavili v `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Přepínač `--verbose` zobrazí další diagnostické informace, což je užitečné při prvním testování vlastního modelu. Pokud se model načte úspěšně, uvidíte interaktivní výzvu. Vyzkoušejte několik zpráv:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Napište `exit` nebo stiskněte `Ctrl+C` pro ukončení relace.

> **Řešení problémů:** Pokud se model nepodaří načíst, zkontrolujte následující:
> - Soubor `genai_config.json` byl vygenerován modelem builderem.
> - Soubor `inference_model.json` existuje a je platný JSON.
> - ONNX modelové soubory jsou ve správném adresáři.
> - Máte dostatek dostupné paměti RAM (Qwen3-0.6B int4 potřebuje přibližně 1 GB).
> - Qwen3 je model pro rozumování, který produkuje značky `<think>`. Pokud vidíte odpovědi začínající `<think>...</think>`, je to normální chování. Šablona promptu v `inference_model.json` může být upravena pro potlačení výstupu myšlení.

---

### Cvičení 8: Dotazování vlastního modelu přes REST API

Pokud jste v cvičení 7 ukončili interaktivní relaci, model už nemusí být načten. Nejprve spusťte službu Foundry Local a načtěte model:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Zjistěte, na jakém portu služba běží:

```bash
foundry service status
```

Následně odešlete požadavek (nahraďte `5273` skutečným portem, pokud se liší):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Poznámka pro Windows:** Příkaz `curl` výše používá syntaxi bash. Na Windows použijte místo toho PowerShell cmdlet `Invoke-RestMethod` níže.

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

### Cvičení 9: Použití vlastního modelu s OpenAI SDK

K vlastnímu modelu se můžete připojit pomocí stejného kódu OpenAI SDK, který jste používali pro vestavěné modely (viz [Část 3](part3-sdk-and-apis.md)). Jediný rozdíl je v názvu modelu.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local neověřuje API klíče
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
  apiKey: "foundry-local", // Foundry Local neověřuje API klíče
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

> **Klíčové:** Protože Foundry Local vystavuje API kompatibilní s OpenAI, jakýkoliv kód pracující s vestavěnými modely funguje také s vašimi vlastními modely. Stačí změnit parametr `model`.

---

### Cvičení 10: Testování vlastního modelu s Foundry Local SDK

V předchozích laborkách jste používali Foundry Local SDK k spuštění služby, nalezení koncového bodu a automatické správě modelů. Stejný postup můžete následovat i s vaším vlastně zkompilovaným modelem. SDK spravuje spuštění služby a objevování koncových bodů, takže ve vašem kódu nemusíte zadávat `localhost:5273` pevně.

> **Poznámka:** Ujistěte se, že máte nainstalovaný Foundry Local SDK před spuštěním těchto příkladů:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Přidejte NuGet balíčky `Microsoft.AI.Foundry.Local` a `OpenAI`
>
> Uložte každý skript **do kořenového adresáře úložiště** (stejný adresář jako složka `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Krok 1: Spusťte službu Foundry Local a načtěte vlastní model
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Krok 2: Zkontrolujte cache pro vlastní model
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Krok 3: Načtěte model do paměti
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Krok 4: Vytvořte klienta OpenAI pomocí SDK objeveného koncového bodu
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Krok 5: Odešlete požadavek na streamované dokončení chatu
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

Spusťte:

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

// Krok 1: Spusťte službu Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Krok 2: Získejte vlastní model z katalogu
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Krok 3: Načtěte model do paměti
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Krok 4: Vytvořte klienta OpenAI pomocí SDK zjištěného koncového bodu
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Krok 5: Odeslat žádost o dokončení chatu streamováním
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

Spusťte:

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

> **Klíčové:** Foundry Local SDK dynamicky objevuje koncový bod, takže nikde nemusíte zadávat číslo portu napevno. Toto je doporučený způsob pro produkční použití. Váš vlastní zkompilovaný model funguje stejně jako vestavěné modely z katalogu přes SDK.

---

## Výběr modelu ke kompilaci

Qwen3-0.6B je v tomto cvičení použito jako referenční příklad, protože je malý, rychle se kompiluje a je volně dostupný pod licencí Apache 2.0. Nicméně můžete zkompilovat mnoho dalších modelů. Zde jsou některé tipy:

| Model | ID Hugging Face | Parametry | Licence | Poznámky |
|-------|-----------------|-----------|---------|----------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Velmi malý, rychlá kompilace, vhodný k testování |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Lepší kvalita, stále rychlá kompilace |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Silná kvalita, vyžaduje více RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Vyžaduje přijetí licence na Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Vysoká kvalita, větší stahování a delší kompilace |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Již v katalogu Foundry Local (užitečné pro srovnání) |

> **Připomenutí licence:** Vždy si před použitím zkontrolujte licenci modelu na Hugging Face. Některé modely (například Llama) vyžadují přijetí licenční smlouvy a autentizaci pomocí `huggingface-cli login` před stažením.

---

## Koncepty: Kdy používat vlastní modely

| Situace | Proč si model zkompilovat? |
|---------|----------------------------|
| **Model, který potřebujete, není v katalogu** | Katalog Foundry Local je řízený. Pokud požadovaný model není uveden, zkompilujte si ho sami. |
| **Modely s doladěním na konkrétní doménu** | Pokud jste model doladili na specifická data, musíte si zkompilovat vlastní váhy. |
| **Specifické požadavky na kvantizaci** | Možná chcete přesnost nebo strategii kvantizace odlišnou od výchozí v katalogu. |
| **Novější verze modelů** | Když je nový model zveřejněn na Hugging Face, možná ještě není v katalogu Foundry Local. Kompilace vám zajistí okamžitý přístup. |
| **Výzkum a experimentování** | Vyzkoušet různé architektury, velikosti nebo konfigurace modelů lokálně před nasazením do produkce. |

---

## Shrnutí

V tomto cvičení jste se naučili:

| Krok | Co jste udělali |
|-------|----------------|
| 1 | Nainstalovali ONNX Runtime GenAI model builder |
| 2 | Zkompilovali `Qwen/Qwen3-0.6B` z Hugging Face do optimalizovaného ONNX modelu |
| 3 | Vytvořili konfigurační soubor šablony chatu `inference_model.json` |
| 4 | Přidali zkompilovaný model do mezipaměti Foundry Local |
| 5 | Spustili interaktivní chat s vlastním modelem přes CLI |
| 6 | Dotazovali model přes REST API kompatibilní s OpenAI |
| 7 | Připojili se z Pythonu, JavaScriptu a C# pomocí OpenAI SDK |
| 8 | Testovali vlastní model end-to-end s Foundry Local SDK |

Hlavní myšlenkou je, že **jakýkoliv model založený na transformerech může běžet přes Foundry Local**, jakmile je zkompilován do formátu ONNX. API kompatibilní s OpenAI znamená, že veškerý váš stávající aplikační kód pracuje bez změn; stačí jen vyměnit název modelu.

---

## Klíčové poznatky

| Koncept | Podrobnosti |
|---------|-------------|
| ONNX Runtime GenAI Model Builder | Převádí modely z Hugging Face do formátu ONNX s kvantizací v jediném příkazu |
| ONNX formát | Foundry Local vyžaduje ONNX modely s konfigurací ONNX Runtime GenAI |
| Šablony chatu | Soubor `inference_model.json` říká Foundry Local, jak formátovat prompt pro daný model |
| Cílové hardware | Kompilace pro CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) nebo WebGPU dle vašeho hardwaru |
| Kvantizace | Nižší přesnost (int4) snižuje velikost a zvyšuje rychlost na úkor přesnosti; fp16 zachovává vysokou kvalitu na GPU |
| Kompatibilita API | Vlastní modely používají stejné API kompatibilní s OpenAI jako vestavěné modely |
| Foundry Local SDK | SDK automaticky zvládá spuštění služby, objevování koncového bodu a načítání modelů, ať už z katalogu nebo vlastní |

---

## Další zdroje

| Zdroj | Odkaz |
|-------|-------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local průvodce vlastními modely | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Rodina modelů Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Dokumentace Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Další kroky

Pokračujte na [Část 11: Volání nástrojů s lokálními modely](part11-tool-calling.md), kde se naučíte, jak povolit vašim lokálním modelům volat externí funkce.

[← Část 9: Přepis hlasu pomocí Whisper](part9-whisper-voice-transcription.md) | [Část 11: Volání nástrojů →](part11-tool-calling.md)