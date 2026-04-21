![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 9: Transcrição de Voz com Whisper e Foundry Local

> **Objetivo:** Usar o modelo OpenAI Whisper a correr localmente através do Foundry Local para transcrever ficheiros de áudio - completamente no dispositivo, sem necessidade de nuvem.

## Visão Geral

O Foundry Local não serve apenas para geração de texto; também suporta modelos de **fala para texto**. Neste laboratório, vai usar o modelo **OpenAI Whisper Medium** para transcrever ficheiros de áudio inteiramente na sua máquina. Isto é ideal para cenários como transcrever chamadas de serviço ao cliente da Zava, gravações de avaliações de produtos ou sessões de planeamento de workshops onde os dados de áudio nunca devem sair do seu dispositivo.


---

## Objetivos de Aprendizagem

No final deste laboratório será capaz de:

- Compreender o modelo de fala para texto Whisper e as suas capacidades
- Descarregar e executar o modelo Whisper usando o Foundry Local
- Transcrever ficheiros de áudio usando o Foundry Local SDK em Python, JavaScript e C#
- Construir um serviço simples de transcrição que corre inteiramente no dispositivo
- Compreender as diferenças entre modelos de chat/texto e modelos de áudio no Foundry Local

---

## Pré-requisitos

| Requisito | Detalhes |
|-------------|---------|
| **Foundry Local CLI** | Versão **0.8.101 ou superior** (os modelos Whisper estão disponíveis a partir da v0.8.101) |
| **SO** | Windows 10/11 (x64 ou ARM64) |
| **Ambiente de execução** | **Python 3.9+** e/ou **Node.js 18+** e/ou **.NET 9 SDK** ([Descarregar .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Concluído** | [Parte 1: Começar](part1-getting-started.md), [Parte 2: Imersão no Foundry Local SDK](part2-foundry-local-sdk.md), e [Parte 3: SDKs e APIs](part3-sdk-and-apis.md) |

> **Nota:** Os modelos Whisper devem ser descarregados via o **SDK** (não o CLI). O CLI não suporta o endpoint de transcrição de áudio. Verifique a sua versão com:
> ```bash
> foundry --version
> ```

---

## Conceito: Como o Whisper funciona com Foundry Local

O modelo OpenAI Whisper é um modelo de reconhecimento de voz de uso geral treinado com um grande conjunto de dados de áudio diversificado. Quando executado via Foundry Local:

- O modelo funciona **inteiramente no seu CPU** - não é necessário GPU
- O áudio nunca sai do seu dispositivo - **privacidade completa**
- O Foundry Local SDK gere o descarregamento e cache do modelo
- **JavaScript e C#** fornecem um `AudioClient` integrado no SDK que trata todo o processo de transcrição — sem necessidade de configurar ONNX manualmente
- **Python** usa o SDK para gestão do modelo e ONNX Runtime para inferência direta nos modelos ONNX do codificador/decodificador

### Como funciona o pipeline (JavaScript e C#) — SDK AudioClient

1. O **Foundry Local SDK** descarrega e guarda em cache o modelo Whisper
2. `model.createAudioClient()` (JS) ou `model.GetAudioClientAsync()` (C#) cria um `AudioClient`
3. `audioClient.transcribe(path)` (JS) ou `audioClient.TranscribeAudioAsync(path)` (C#) trata internamente de todo o pipeline — pré-processamento do áudio, codificador, decodificador e decodificação dos tokens
4. O `AudioClient` expõe a propriedade `settings.language` (definida para `"en"` para Inglês) para orientar uma transcrição precisa

### Como funciona o pipeline (Python) — ONNX Runtime

1. O **Foundry Local SDK** descarrega e guarda em cache os ficheiros ONNX do Whisper
2. O **pré-processamento de áudio** converte o áudio WAV numa espectrograma mel (80 bins mel x 3000 frames)
3. O **Codificador** processa o espectrograma mel e produz estados ocultos mais tensores chave/valor de atenção cruzada
4. O **Decodificador** corre de forma autoregressiva, gerando um token de cada vez até produzir o token de fim de texto
5. O **Tokenizador** decodifica os IDs dos tokens para texto legível

### Variantes do Modelo Whisper

| Alias | ID do Modelo | Dispositivo | Tamanho | Descrição |
|-------|--------------|-------------|---------|-----------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Acelerado por GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Otimizado para CPU (recomendado para a maioria dos dispositivos) |

> **Nota:** Ao contrário dos modelos de chat que aparecem por defeito, os modelos Whisper estão categorizados sob a tarefa `automatic-speech-recognition`. Use `foundry model info whisper-medium` para ver detalhes.

---

## Exercícios do Laboratório

### Exercício 0 - Obter Ficheiros de Áudio de Exemplo

Este laboratório inclui ficheiros WAV pré-construídos baseados em cenários de produto Zava DIY. Gere-os com o script incluído:

```bash
# A partir da raiz do repositório - criar e ativar um .venv primeiro
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Isto cria seis ficheiros WAV em `samples/audio/`:

| Ficheiro | Cenário |
|----------|---------|
| `zava-customer-inquiry.wav` | Cliente a perguntar sobre a **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | Cliente a fazer uma avaliação da **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | Chamada de suporte sobre o **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | DIY a planear um deck com **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Passagem por um workshop usando **os cinco produtos Zava** |
| `zava-full-project-walkthrough.wav` | Revisão extensa de renovação da garagem usando **todos os produtos Zava** (~4 min, para teste de áudio longo) |

> **Dica:** Pode também usar os seus próprios ficheiros WAV/MP3/M4A, ou gravar-se com o Gravador de Voz do Windows.

---

### Exercício 1 - Descarregar o Modelo Whisper Usando o SDK

Devido a incompatibilidades do CLI com modelos Whisper nas versões mais recentes do Foundry Local, use o **SDK** para descarregar e carregar o modelo. Escolha a sua linguagem:

<details>
<summary><b>🐍 Python</b></summary>

**Instale o SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Iniciar o serviço
manager = FoundryLocalManager()
manager.start_service()

# Verificar informação do catálogo
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Verificar se já está em cache
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Carregar o modelo para a memória
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Guarde como `download_whisper.py` e execute:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Instale o SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Criar gestor e iniciar o serviço
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Obter modelo do catálogo
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.id}`);

if (model.isCached) {
  console.log("Whisper model already downloaded.");
} else {
  console.log("Downloading Whisper model (this may take several minutes)...");
  await model.download();
  console.log("Download complete.");
}

// Carregar o modelo para a memória
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Guarde como `download-whisper.mjs` e execute:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Instale o SDK:**
```bash
dotnet add package Microsoft.AI.Foundry.Local
```

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "whisper-medium";

// Start the service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);
Console.WriteLine($"Model: {model.Id}");

// Check if already cached
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine("Whisper model already downloaded.");
}
else
{
    Console.WriteLine("Downloading Whisper model (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine("Download complete.");
}

// Load the model into memory
await model.LoadAsync(default);
Console.WriteLine($"Whisper model loaded: {model.Id}");
```

</details>

> **Porquê o SDK em vez do CLI?** O Foundry Local CLI não suporta descarregar nem fornecer modelos Whisper diretamente. O SDK fornece uma forma fiável de descarregar e gerir modelos de áudio programaticamente. Os SDKs JavaScript e C# incluem um `AudioClient` integrado que trata todo o pipeline de transcrição. O Python utiliza o ONNX Runtime para inferência direta com os ficheiros do modelo em cache.

---

### Exercício 2 - Compreender o SDK Whisper

A transcrição Whisper usa abordagens diferentes consoante a linguagem. **JavaScript e C#** fornecem um `AudioClient` integrado no Foundry Local SDK que trata todo o pipeline (pré-processamento do áudio, codificador, decodificador, decodificação dos tokens) numa única chamada de método. **Python** usa o Foundry Local SDK para gestão do modelo e ONNX Runtime para inferência direta com os modelos ONNX do codificador/decodificador.

| Componente | Python | JavaScript | C# |
|------------|--------|------------|----|
| **Pacotes SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Gestão do modelo** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catálogo |
| **Extração de características** | `WhisperFeatureExtractor` + `librosa` | Tratado pelo `AudioClient` do SDK | Tratado pelo `AudioClient` do SDK |
| **Inferência** | `ort.InferenceSession` (codificador + decodificador) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Decodificação dos tokens** | `WhisperTokenizer` | Tratado pelo `AudioClient` do SDK | Tratado pelo `AudioClient` do SDK |
| **Definição da língua** | Definida via `forced_ids` nos tokens do decodificador | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Entrada** | Caminho do ficheiro WAV | Caminho do ficheiro WAV | Caminho do ficheiro WAV |
| **Saída** | Texto descodificado | `result.text` | `result.Text` |

> **Importante:** Defina sempre a língua no `AudioClient` (ex.: `"en"` para Inglês). Sem uma definição explícita de língua, o modelo pode produzir resultados confusos porque tenta detetar automaticamente a língua.

> **Padrões do SDK:** Python usa `FoundryLocalManager(alias)` para inicializar, depois `get_cache_location()` para encontrar os ficheiros ONNX do modelo. JavaScript e C# usam o `AudioClient` integrado do SDK — obtido via `model.createAudioClient()` (JS) ou `model.GetAudioClientAsync()` (C#) — que trata todo o pipeline de transcrição. Veja [Parte 2: Imersão no Foundry Local SDK](part2-foundry-local-sdk.md) para detalhes completos.

---

### Exercício 3 - Construir uma App Simples de Transcrição

Escolha o seu track de linguagem e construa uma aplicação mínima que transcreva um ficheiro de áudio.

> **Formatos de áudio suportados:** WAV, MP3, M4A. Para melhores resultados, use ficheiros WAV com frequência de amostragem de 16kHz.

<details>
<summary><h3>Track Python</h3></summary>

#### Configuração

```bash
cd python
python -m venv venv

# Ativar o ambiente virtual:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Código de Transcrição

Crie o ficheiro `foundry-local-whisper.py`:

```python
import sys
import os
import numpy as np
import onnxruntime as ort
import librosa
from transformers import WhisperFeatureExtractor, WhisperTokenizer
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"
audio_file = sys.argv[1] if len(sys.argv) > 1 else "sample.wav"

if not os.path.exists(audio_file):
    print(f"Audio file not found: {audio_file}")
    sys.exit(1)

# Passo 1: Bootstrap - inicia o serviço, descarrega e carrega o modelo
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Construir caminho para os ficheiros ONNX em cache
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Passo 2: Carregar sessões ONNX e extrator de características
encoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_encoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
decoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_decoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
fe = WhisperFeatureExtractor.from_pretrained(model_dir)
tokenizer = WhisperTokenizer.from_pretrained(model_dir)

# Passo 3: Extrair características do espectrograma mel
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Passo 4: Executar codificador
enc_out = encoder.run(None, {"audio_features": input_features})
# A primeira saída são os estados ocultos; os restantes são pares KV de atenção cruzada
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Passo 5: Decodificação autoregressiva
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcrever, sem marcas temporais
input_ids = np.array([initial_tokens], dtype=np.int32)

# Cache KV vazio de autoatenção
self_kv = {}
for i in range(24):
    self_kv[f"past_key_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)
    self_kv[f"past_value_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)

generated = []
for _ in range(448):
    feeds = {"input_ids": input_ids, **cross_kv, **self_kv}
    outputs = decoder.run(None, feeds)
    logits = outputs[0]
    next_token = int(np.argmax(logits[0, -1, :]))

    if next_token == 50257:  # fim do texto
        break
    generated.append(next_token)

    # Atualizar cache KV de autoatenção
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Execute-o

```bash
# Transcreve um cenário de produto Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Ou experimenta outros:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Pontos-chave para Python

| Método | Propósito |
|--------|-----------|
| `FoundryLocalManager(alias)` | Inicializa: inicia o serviço, descarrega e carrega o modelo |
| `manager.get_cache_location()` | Obtém o caminho para os ficheiros ONNX em cache |
| `WhisperFeatureExtractor.from_pretrained()` | Carrega o extrator de características do espectrograma mel |
| `ort.InferenceSession()` | Cria sessões ONNX Runtime para codificador e decodificador |
| `tokenizer.decode()` | Converte os IDs dos tokens para texto |

</details>

<details>
<summary><h3>Track JavaScript</h3></summary>

#### Configuração

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Código de Transcrição

Crie o ficheiro `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Passo 1: Inicializar - criar gestor, iniciar serviço e carregar o modelo
console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log("Downloading Whisper model...");
  await model.download();
}
await model.load();

// Passo 2: Criar um cliente de áudio e transcrever
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Limpar
await model.unload();
```

> **Nota:** O Foundry Local SDK fornece um `AudioClient` integrado via `model.createAudioClient()` que trata internamente todo o pipeline de inferência ONNX — não é necessário importar `onnxruntime-node`. Defina sempre `audioClient.settings.language = "en"` para garantir uma transcrição precisa em Inglês.

#### Execute-o

```bash
# Transcreva um cenário de produto Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Ou experimente outros:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Pontos-chave para JavaScript

| Método | Propósito |
|--------|-----------|
| `FoundryLocalManager.create({ appName })` | Cria o singleton manager |
| `await catalog.getModel(alias)` | Obtém um modelo do catálogo |
| `model.download()` / `model.load()` | Descarrega e carrega o modelo Whisper |
| `model.createAudioClient()` | Cria um cliente de áudio para transcrição |
| `audioClient.settings.language = "en"` | Define a língua para transcrição (obrigatório para saída precisa) |
| `audioClient.transcribe(path)` | Transcreve um ficheiro de áudio, retorna `{ text, duration }` |

</details>

<details>
<summary><h3>Track C#</h3></summary>

#### Configuração

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Nota:** O track C# usa o pacote `Microsoft.AI.Foundry.Local` que fornece um `AudioClient` integrado via `model.GetAudioClientAsync()`. Este trata todo o pipeline de transcrição no processo — não é necessária configuração separada do ONNX Runtime.

#### Código de Transcrição

Substitua o conteúdo de `Program.cs`:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// --- Configuration ---
var modelAlias = "whisper-medium";
var audioFile = args.Length > 0 ? args[0] : "sample.wav";

if (!File.Exists(audioFile))
{
    Console.WriteLine($"Audio file not found: {audioFile}");
    Console.WriteLine("Usage: dotnet run <path-to-audio-file>");
    return;
}

// --- Step 1: Initialize Foundry Local ---
Console.WriteLine("Initializing Foundry Local...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// --- Step 2: Load the Whisper model ---
Console.WriteLine($"Loading model: {modelAlias}...");
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading model...");
    await model.DownloadAsync(null, default);
}

// Load model into memory
Console.WriteLine("Loading model into memory...");
await model.LoadAsync(default);

// --- Step 3: Transcribe audio ---
Console.WriteLine($"Transcribing: {audioFile}");

var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en";

var result = await audioClient.TranscribeAudioAsync(audioFile);

Console.WriteLine("\n--- Transcription ---");
Console.WriteLine(result.Text);
Console.WriteLine("---------------------");
```

#### Execute-o

```bash
# Transcreva um cenário de produto Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Ou experimente outros:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Pontos-chave para C#

| Método | Propósito |
|--------|-----------|
| `FoundryLocalManager.CreateAsync(config)` | Inicializa o Foundry Local com configuração |
| `catalog.GetModelAsync(alias)` | Obtém modelo do catálogo |
| `model.DownloadAsync()` | Descarrega o modelo Whisper |
| `model.GetAudioClientAsync()` | Obtém o AudioClient (não o ChatClient!) |
| `audioClient.Settings.Language = "en"` | Define a língua para transcrição (obrigatório para saída precisa) |
| `audioClient.TranscribeAudioAsync(path)` | Transcreve um ficheiro de áudio |
| `result.Text` | Texto transcrito |


> **C# vs Python/JS:** O SDK C# fornece um `AudioClient` incorporado para transcrição em processo através de `model.GetAudioClientAsync()`, semelhante ao SDK JavaScript. O Python usa diretamente o ONNX Runtime para inferência contra os modelos de codificador/descodificador em cache.

</details>

---

### Exercício 4 - Transcrição em Lote de Todas as Amostras Zava

Agora que tem uma aplicação de transcrição a funcionar, transcreva os cinco ficheiros de amostra Zava e compare os resultados.

<details>
<summary><h3>Trajeto Python</h3></summary>

O ficheiro de amostra completo `python/foundry-local-whisper.py` já suporta transcrição em lote. Quando executado sem argumentos, transcreve todos os ficheiros `zava-*.wav` na pasta `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

A amostra usa `FoundryLocalManager(alias)` para inicializar, depois executa as sessões ONNX do codificador e descodificador para cada ficheiro.

</details>

<details>
<summary><h3>Trajeto JavaScript</h3></summary>

O ficheiro de amostra completo `javascript/foundry-local-whisper.mjs` já suporta transcrição em lote. Quando executado sem argumentos, transcreve todos os ficheiros `zava-*.wav` na pasta `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

A amostra usa `FoundryLocalManager.create()` e `catalog.getModel(alias)` para inicializar o SDK, depois utiliza o `AudioClient` (com `settings.language = "en"`) para transcrever cada ficheiro.

</details>

<details>
<summary><h3>Trajeto C#</h3></summary>

O ficheiro de amostra completo `csharp/WhisperTranscription.cs` já suporta transcrição em lote. Quando executado sem um argumento de ficheiro específico, transcreve todos os ficheiros `zava-*.wav` na pasta `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

A amostra usa `FoundryLocalManager.CreateAsync()` e o `AudioClient` do SDK (com `Settings.Language = "en"`) para transcrição em processo.

</details>

**O que observar:** Compare a saída da transcrição com o texto original em `samples/audio/generate_samples.py`. Quão precisamente o Whisper captura nomes de produtos como "Zava ProGrip" e termos técnicos como "brushless motor" ou "composite decking"?

---

### Exercício 5 - Entender os Padrões-Chave de Código

Estude como a transcrição Whisper difere das respostas de chat nas três linguagens:

<details>
<summary><b>Python - Diferenças-Chave em Relação ao Chat</b></summary>

```python
# Conclusão de chat (Partes 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Transcrição de áudio (Esta parte):
# Usa diretamente o ONNX Runtime em vez do cliente OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... ciclo do decodificador autorregressivo ...
print(tokenizer.decode(generated_tokens))
```

**Insight chave:** Os modelos de chat usam a API compatível com OpenAI através de `manager.endpoint`. O Whisper usa o SDK para localizar os ficheiros ONNX em cache e depois executa inferência diretamente com ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Diferenças-Chave em Relação ao Chat</b></summary>

```javascript
// Conclusão do chat (Partes 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Transcrição de áudio (Esta Parte):
// Usa o AudioClient incorporado no SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Defina sempre o idioma para melhores resultados
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Insight chave:** Os modelos de chat usam a API compatível com OpenAI via `manager.urls[0] + "/v1"`. A transcrição Whisper usa o `AudioClient` do SDK, obtido com `model.createAudioClient()`. Defina `settings.language` para evitar saída corrompida na deteção automática.

</details>

<details>
<summary><b>C# - Diferenças-Chave em Relação ao Chat</b></summary>

A abordagem C# usa o `AudioClient` incorporado do SDK para transcrição em processo:

**Inicialização do modelo:**

```csharp
// 1. Create the manager with configuration
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// 2. Get model from catalog, download, and load
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("whisper-medium", default);
await model.DownloadAsync(null, default);
await model.LoadAsync(default);
```

**Transcrição:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Insight chave:** O C# usa `FoundryLocalManager.CreateAsync()` e obtém um `AudioClient` diretamente — sem necessidade de configuração ONNX Runtime. Defina `Settings.Language` para evitar saída corrompida na deteção automática.

</details>

> **Resumo:** O Python usa o Foundry Local SDK para gestão de modelos e ONNX Runtime para inferência direta contra os modelos de codificador/descodificador. JavaScript e C# usam o `AudioClient` incorporado do SDK para uma transcrição simplificada — crie o cliente, defina a língua e ligue para `transcribe()` / `TranscribeAudioAsync()`. Defina sempre a propriedade de língua no AudioClient para resultados precisos.

---

### Exercício 6 - Experimente

Tente estas modificações para aprofundar a sua compreensão:

1. **Experimente ficheiros áudio diferentes** – grave-se a falar usando o Gravador de Voz do Windows, guarde como WAV e transcreva

2. **Compare variantes do modelo** – se tiver uma GPU NVIDIA, experimente a variante CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Compare a velocidade da transcrição contra a variante CPU.

3. **Adicione formatação à saída** – a resposta JSON pode incluir:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Construa uma API REST** – envolva o seu código de transcrição num servidor web:

   | Linguagem | Framework | Exemplo |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` com `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` com `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` com `IFormFile` |

5. **Multi-turn com transcrição** – combine Whisper com um agente de chat da Parte 4: transcreva áudio primeiro, depois passe o texto a um agente para análise ou sumarização.

---

## Referência da API de Áudio do SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — cria uma instância `AudioClient`
> - `audioClient.settings.language` — define a língua de transcrição (ex. `"en"`)
> - `audioClient.settings.temperature` — controla a aleatoriedade (opcional)
> - `audioClient.transcribe(filePath)` — transcreve um ficheiro, retorna `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — transmite pedaços da transcrição via callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — cria uma instância `OpenAIAudioClient`
> - `audioClient.Settings.Language` — define a língua de transcrição (ex. `"en"`)
> - `audioClient.Settings.Temperature` — controla a aleatoriedade (opcional)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transcreve um ficheiro, retorna objeto com `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — retorna `IAsyncEnumerable` de pedaços da transcrição

> **Dica:** Defina sempre a propriedade da língua antes de transcrever. Sem ela, o modelo Whisper tenta a deteção automática, o que pode produzir saída corrompida (um único carácter de substituição em vez de texto).

---

## Comparação: Modelos de Chat vs. Whisper

| Aspeto | Modelos de Chat (Partes 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|-----------------------------|------------------|-------------------|
| **Tipo de tarefa** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Entrada** | Mensagens de texto (JSON) | Ficheiros áudio (WAV/MP3/M4A) | Ficheiros áudio (WAV/MP3/M4A) |
| **Saída** | Texto gerado (transmitido) | Texto transcrito (completo) | Texto transcrito (completo) |
| **Pacote SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Método API** | `client.chat.completions.create()` | ONNX Runtime direto | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Definição de língua** | N/A | Tokens do prompt do descodificador | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Sim | Não | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Benefício de privacidade** | Código/dados ficam locais | Dados áudio ficam locais | Dados áudio ficam locais |

---

## Principais Conclusões

| Conceito | O Que Aprendeu |
|---------|----------------|
| **Whisper local** | Fala para texto corre inteiramente localmente, ideal para transcrever chamadas e avaliações de clientes Zava no dispositivo |
| **SDK AudioClient** | SDKs JavaScript e C# fornecem um `AudioClient` incorporado que trata toda a pipeline de transcrição numa única chamada |
| **Definição da língua** | Defina sempre a língua no AudioClient (ex. `"en"`) — sem isto, a deteção automática pode produzir saída corrompida |
| **Python** | Usa `foundry-local-sdk` para gestão de modelos + `onnxruntime` + `transformers` + `librosa` para inferência ONNX direta |
| **JavaScript** | Usa `foundry-local-sdk` com `model.createAudioClient()` — defina `settings.language`, depois chame `transcribe()` |
| **C#** | Usa `Microsoft.AI.Foundry.Local` com `model.GetAudioClientAsync()` — defina `Settings.Language`, depois chame `TranscribeAudioAsync()` |
| **Suporte a streaming** | SDKs JS e C# também oferecem `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` para saída pedaço a pedaço |
| **Otimizado para CPU** | A variante CPU (3.05 GB) funciona em qualquer dispositivo Windows sem GPU |
| **Privacidade em primeiro lugar** | Perfeito para manter interações com clientes Zava e dados proprietários de produtos localmente |

---

## Recursos

| Recurso | Link |
|----------|------|
| Documentação Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referência SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Modelo OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Website Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Próximo Passo

Continue para [Parte 10: Usar Modelos Customizados ou Hugging Face](part10-custom-models.md) para compilar os seus próprios modelos do Hugging Face e executá-los no Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, por favor tenha em conta que traduções automáticas podem conter erros ou imprecisões. O documento original no seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se a tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->