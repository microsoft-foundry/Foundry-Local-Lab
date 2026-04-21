![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 9: Transcrição de Voz com Whisper e Foundry Local

> **Objetivo:** Usar o modelo OpenAI Whisper rodando localmente através do Foundry Local para transcrever arquivos de áudio - completamente no dispositivo, sem necessidade de nuvem.

## Visão Geral

Foundry Local não é só para geração de texto; ele também suporta modelos de **fala para texto**. Neste laboratório você utilizará o modelo **OpenAI Whisper Medium** para transcrever arquivos de áudio inteiramente na sua máquina. Isso é ideal para cenários como transcrição de chamadas de atendimento ao cliente da Zava, gravações de avaliações de produtos ou sessões de planejamento de workshops onde os dados de áudio jamais devem sair do seu dispositivo.


---

## Objetivos de Aprendizagem

Ao final deste laboratório você será capaz de:

- Compreender o modelo Whisper de fala para texto e suas capacidades
- Baixar e executar o modelo Whisper usando o Foundry Local
- Transcrever arquivos de áudio usando o Foundry Local SDK em Python, JavaScript e C#
- Construir um serviço simples de transcrição que roda inteiramente no dispositivo
- Entender as diferenças entre modelos de chat/texto e modelos de áudio no Foundry Local

---

## Pré-requisitos

| Requisito | Detalhes |
|-------------|---------|
| **Foundry Local CLI** | Versão **0.8.101 ou superior** (Modelos Whisper estão disponíveis a partir da v0.8.101) |
| **SO** | Windows 10/11 (x64 ou ARM64) |
| **Runtime da linguagem** | **Python 3.9+** e/ou **Node.js 18+** e/ou **.NET 9 SDK** ([Download .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Concluído** | [Parte 1: Introdução](part1-getting-started.md), [Parte 2: Imersão no Foundry Local SDK](part2-foundry-local-sdk.md), e [Parte 3: SDKs e APIs](part3-sdk-and-apis.md) |

> **Nota:** Modelos Whisper devem ser baixados via **SDK** (não pela CLI). A CLI não suporta o endpoint de transcrição de áudio. Verifique sua versão com:
> ```bash
> foundry --version
> ```

---

## Conceito: Como o Whisper Funciona com Foundry Local

O modelo OpenAI Whisper é um modelo geral de reconhecimento de fala treinado em um grande conjunto de dados diversificados em áudio. Quando executado através do Foundry Local:

- O modelo roda **inteiramente na sua CPU** - nenhuma GPU é necessária
- O áudio nunca sai do seu dispositivo - **privacidade completa**
- O Foundry Local SDK gerencia o download e cache do modelo
- **JavaScript e C#** oferecem um `AudioClient` embutido no SDK que gerencia todo o pipeline de transcrição — sem necessidade de configuração manual do ONNX
- **Python** usa o SDK para gerenciamento do modelo e ONNX Runtime para inferência direta contra os modelos ONNX do codificador/decodificador

### Como o Pipeline Funciona (JavaScript e C#) — AudioClient do SDK

1. **Foundry Local SDK** baixa e armazena em cache o modelo Whisper
2. `model.createAudioClient()` (JS) ou `model.GetAudioClientAsync()` (C#) cria um `AudioClient`
3. `audioClient.transcribe(path)` (JS) ou `audioClient.TranscribeAudioAsync(path)` (C#) executa internamente todo o pipeline — pré-processamento de áudio, codificador, decodificador e decodificação dos tokens
4. O `AudioClient` expõe uma propriedade `settings.language` (definida como `"en"` para Inglês) para guiar a transcrição precisa

### Como o Pipeline Funciona (Python) — ONNX Runtime

1. **Foundry Local SDK** baixa e armazena em cache os arquivos ONNX do modelo Whisper
2. **Pré-processamento de áudio** converte áudio WAV em um espectrograma mel (80 bins mel x 3000 frames)
3. **Codificador** processa o espectrograma mel e produz estados ocultos além de tensores chave/valor de atenção cruzada
4. **Decodificador** funciona autoregressivamente, gerando um token por vez até produzir um token de fim de texto
5. **Tokenizador** decodifica os IDs dos tokens de saída novamente em texto legível

### Variantes do Modelo Whisper

| Alias | ID do Modelo | Dispositivo | Tamanho | Descrição |
|-------|--------------|-------------|---------|-----------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1,53 GB | Acelerado por GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3,05 GB | Otimizado para CPU (recomendado para a maioria dos dispositivos) |

> **Nota:** Ao contrário dos modelos de chat listados por padrão, os modelos Whisper são categorizados na tarefa `automatic-speech-recognition`. Use `foundry model info whisper-medium` para ver detalhes.

---

## Exercícios do Laboratório

### Exercício 0 - Obter Arquivos de Áudio de Exemplo

Este laboratório inclui arquivos WAV pré-criados baseados em cenários de produtos DIY da Zava. Gere-os com o script incluído:

```bash
# A partir da raiz do repositório - crie e ative um .venv primeiro
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Isso cria seis arquivos WAV em `samples/audio/`:

| Arquivo | Cenário |
|---------|---------|
| `zava-customer-inquiry.wav` | Cliente perguntando sobre a **Parafusadeira Sem Fio Zava ProGrip** |
| `zava-product-review.wav` | Cliente avaliando a **Tinta de Interior UltraSmooth Zava** |
| `zava-support-call.wav` | Chamada de suporte sobre o **Baú de Ferramentas Zava TitanLock** |
| `zava-project-planning.wav` | DIYer planejando um deck com **Deck Composto EcoBoard Zava** |
| `zava-workshop-setup.wav` | Tour por uma oficina usando **os cinco produtos Zava** |
| `zava-full-project-walkthrough.wav` | Tour prolongado pela reforma da garagem usando **todos os produtos Zava** (~4 min, para teste de áudio longo) |

> **Dica:** Você também pode usar seus próprios arquivos WAV/MP3/M4A ou gravar com o Gravador de Voz do Windows.

---

### Exercício 1 - Baixar o Modelo Whisper Usando o SDK

Devido a incompatibilidades da CLI com modelos Whisper em versões mais recentes do Foundry Local, use o **SDK** para baixar e carregar o modelo. Escolha sua linguagem:

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

# Verificar informações do catálogo
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

# Carregar o modelo na memória
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Salve como `download_whisper.py` e execute:
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

// Criar gerente e iniciar o serviço
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

// Carregar o modelo na memória
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Salve como `download-whisper.mjs` e execute:
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

> **Por que SDK em vez da CLI?** A Foundry Local CLI não suporta baixar ou servir modelos Whisper diretamente. O SDK fornece uma forma confiável para baixar e gerenciar modelos de áudio programaticamente. Os SDKs JavaScript e C# incluem um `AudioClient` embutido que gerencia todo o pipeline de transcrição. O Python usa ONNX Runtime para inferência direta contra os arquivos de modelo em cache.

---

### Exercício 2 - Entender o SDK Whisper

A transcrição com Whisper usa abordagens diferentes conforme a linguagem. **JavaScript e C#** oferecem um `AudioClient` embutido no Foundry Local SDK que processa todo o pipeline (pré-processamento de áudio, codificador, decodificador, decodificação dos tokens) em uma única chamada. **Python** usa o Foundry Local SDK para gerenciamento do modelo e ONNX Runtime para inferência direta contra os modelos ONNX do codificador/decodificador.

| Componente | Python | JavaScript | C# |
|------------|--------|------------|----|
| **Pacotes SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Gerenciamento do modelo** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catálogo |
| **Extração de características** | `WhisperFeatureExtractor` + `librosa` | Gerenciado pelo `AudioClient` do SDK | Gerenciado pelo `AudioClient` do SDK |
| **Inferência** | `ort.InferenceSession` (codificador + decodificador) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Decodificação dos tokens** | `WhisperTokenizer` | Gerenciado pelo `AudioClient` do SDK | Gerenciado pelo `AudioClient` do SDK |
| **Configuração de idioma** | Definido via `forced_ids` nos tokens do decodificador | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Entrada** | Caminho do arquivo WAV | Caminho do arquivo WAV | Caminho do arquivo WAV |
| **Saída** | String de texto decodificado | `result.text` | `result.Text` |

> **Importante:** Sempre defina o idioma no `AudioClient` (ex.: `"en"` para Inglês). Sem uma configuração explícita de idioma, o modelo pode produzir resultados confusos tentando detectar automaticamente a língua.

> **Padrões do SDK:** Python usa `FoundryLocalManager(alias)` para inicializar, depois `get_cache_location()` para localizar os arquivos ONNX. JavaScript e C# usam o `AudioClient` embutido do SDK — obtido via `model.createAudioClient()` (JS) ou `model.GetAudioClientAsync()` (C#) — que gerencia todo o pipeline de transcrição. Veja [Parte 2: Imersão no Foundry Local SDK](part2-foundry-local-sdk.md) para detalhes completos.

---

### Exercício 3 - Construir um App Simples de Transcrição

Escolha sua linguagem e construa um aplicativo mínimo que transcreva um arquivo de áudio.

> **Formatos de áudio suportados:** WAV, MP3, M4A. Para melhores resultados, use arquivos WAV com taxa de amostragem 16kHz.

<details>
<summary><h3>Pista Python</h3></summary>

#### Configuração

```bash
cd python
python -m venv venv

# Ative o ambiente virtual:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Código de Transcrição

Crie o arquivo `foundry-local-whisper.py`:

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

# Passo 1: Bootstrap - inicia o serviço, baixa e carrega o modelo
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Construir caminho para os arquivos de modelo ONNX em cache
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
# A primeira saída são estados ocultos; os restantes são pares KV de atenção cruzada
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Passo 5: Decodificação autoregressiva
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcrever, sem timestamps
input_ids = np.array([initial_tokens], dtype=np.int32)

# Cache KV de autoatenção vazio
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
# Transcrever um cenário de produto da Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Ou tente outros:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Pontos Chave em Python

| Método | Objetivo |
|--------|----------|
| `FoundryLocalManager(alias)` | Inicializa: inicia serviço, baixa e carrega o modelo |
| `manager.get_cache_location()` | Obtém o caminho dos arquivos ONNX em cache |
| `WhisperFeatureExtractor.from_pretrained()` | Carrega o extrator de características do espectrograma mel |
| `ort.InferenceSession()` | Cria sessões ONNX Runtime para codificador e decodificador |
| `tokenizer.decode()` | Converte os IDs dos tokens de saída de volta para texto |

</details>

<details>
<summary><h3>Pista JavaScript</h3></summary>

#### Configuração

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Código de Transcrição

Crie o arquivo `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Etapa 1: Bootstrap - criar gerente, iniciar serviço e carregar o modelo
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

// Etapa 2: Criar um cliente de áudio e transcrever
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Limpeza
await model.unload();
```

> **Nota:** O Foundry Local SDK fornece um `AudioClient` embutido via `model.createAudioClient()` que gerencia internamente todo o pipeline de inferência ONNX — não é necessário importar `onnxruntime-node`. Sempre defina `audioClient.settings.language = "en"` para garantir uma transcrição precisa em inglês.

#### Execute-o

```bash
# Transcrever um cenário de produto Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Ou tentar outros:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Pontos Chave em JavaScript

| Método | Objetivo |
|--------|----------|
| `FoundryLocalManager.create({ appName })` | Cria o singleton do gerenciador |
| `await catalog.getModel(alias)` | Obtém um modelo do catálogo |
| `model.download()` / `model.load()` | Baixa e carrega o modelo Whisper |
| `model.createAudioClient()` | Cria um cliente de áudio para transcrição |
| `audioClient.settings.language = "en"` | Define o idioma da transcrição (necessário para saída precisa) |
| `audioClient.transcribe(path)` | Transcreve um arquivo de áudio, retorna `{ text, duration }` |

</details>

<details>
<summary><h3>Pista C#</h3></summary>

#### Configuração

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Nota:** A pista C# usa o pacote `Microsoft.AI.Foundry.Local`, que fornece um `AudioClient` embutido via `model.GetAudioClientAsync()`. Isso gerencia todo o pipeline de transcrição no processo — não é necessária configuração separada do ONNX Runtime.

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

#### Pontos Chave em C#

| Método | Objetivo |
|--------|----------|
| `FoundryLocalManager.CreateAsync(config)` | Inicializa o Foundry Local com configuração |
| `catalog.GetModelAsync(alias)` | Obtém modelo do catálogo |
| `model.DownloadAsync()` | Baixa o modelo Whisper |
| `model.GetAudioClientAsync()` | Obtém o AudioClient (não ChatClient!) |
| `audioClient.Settings.Language = "en"` | Define idioma da transcrição (necessário para saída precisa) |
| `audioClient.TranscribeAudioAsync(path)` | Transcreve um arquivo de áudio |
| `result.Text` | O texto transcrito |


> **C# vs Python/JS:** O SDK C# fornece um `AudioClient` embutido para transcrição in-process via `model.GetAudioClientAsync()`, semelhante ao SDK JavaScript. Python usa diretamente o ONNX Runtime para inferência contra os modelos encoder/decoder em cache.

</details>

---

### Exercício 4 - Transcrever em Lote Todas as Amostras Zava

Agora que você tem um aplicativo de transcrição funcionando, transcreva todos os cinco arquivos de amostra Zava e compare os resultados.

<details>
<summary><h3>Trilha Python</h3></summary>

O exemplo completo `python/foundry-local-whisper.py` já suporta transcrição em lote. Quando executado sem argumentos, ele transcreve todos os arquivos `zava-*.wav` em `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

O exemplo usa `FoundryLocalManager(alias)` para inicializar, depois executa as sessões ONNX do encoder e decoder para cada arquivo.

</details>

<details>
<summary><h3>Trilha JavaScript</h3></summary>

O exemplo completo `javascript/foundry-local-whisper.mjs` já suporta transcrição em lote. Quando executado sem argumentos, ele transcreve todos os arquivos `zava-*.wav` em `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

O exemplo usa `FoundryLocalManager.create()` e `catalog.getModel(alias)` para inicializar o SDK, depois usa o `AudioClient` (com `settings.language = "en"`) para transcrever cada arquivo.

</details>

<details>
<summary><h3>Trilha C#</h3></summary>

O exemplo completo `csharp/WhisperTranscription.cs` já suporta transcrição em lote. Quando executado sem um argumento específico de arquivo, ele transcreve todos os arquivos `zava-*.wav` em `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

O exemplo usa `FoundryLocalManager.CreateAsync()` e o `AudioClient` do SDK (com `Settings.Language = "en"`) para transcrição in-process.

</details>

**O que observar:** Compare a saída da transcrição com o texto original em `samples/audio/generate_samples.py`. Quão precisamente o Whisper captura nomes de produtos como "Zava ProGrip" e termos técnicos como "brushless motor" ou "composite decking"?

---

### Exercício 5 - Entenda os Padrões-chave do Código

Estude como a transcrição Whisper difere das conclusões de chat nas três linguagens:

<details>
<summary><b>Python - Diferenças-chave do Chat</b></summary>

```python
# Conclusão de chat (Partes 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Transcrição de áudio (Esta Parte):
# Usa ONNX Runtime diretamente em vez do cliente OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... loop do decodificador autorregressivo ...
print(tokenizer.decode(generated_tokens))
```

**Insight chave:** Modelos de chat usam a API compatível com OpenAI via `manager.endpoint`. Whisper usa o SDK para localizar os arquivos de modelo ONNX em cache, depois executa inferência diretamente com ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Diferenças-chave do Chat</b></summary>

```javascript
// Conclusão do chat (Partes 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Transcrição de áudio (Esta parte):
// Usa o AudioClient integrado do SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Sempre defina o idioma para melhores resultados
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Insight chave:** Modelos de chat usam a API compatível com OpenAI via `manager.urls[0] + "/v1"`. A transcrição Whisper usa o `AudioClient` do SDK, obtido por `model.createAudioClient()`. Defina `settings.language` para evitar saída corrompida da auto-detecção.

</details>

<details>
<summary><b>C# - Diferenças-chave do Chat</b></summary>

A abordagem C# usa o `AudioClient` embutido do SDK para transcrição in-process:

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

**Insight chave:** C# usa `FoundryLocalManager.CreateAsync()` e obtém um `AudioClient` diretamente — não precisa configurar ONNX Runtime. Defina `Settings.Language` para evitar saída corrompida da auto-detecção.

</details>

> **Resumo:** Python usa o Foundry Local SDK para gerenciamento de modelos e ONNX Runtime para inferência direta contra os modelos encoder/decoder. JavaScript e C# usam o `AudioClient` embutido para transcrição simplificada — crie o cliente, defina a linguagem, e chame `transcribe()` / `TranscribeAudioAsync()`. Sempre defina a propriedade de linguagem no AudioClient para resultados precisos.

---

### Exercício 6 - Experimente

Tente essas modificações para aprofundar seu entendimento:

1. **Tente arquivos de áudio diferentes** - grave você mesmo usando o Gravador de Voz do Windows, salve como WAV, e transcreva

2. **Compare variantes do modelo** - se você tem uma GPU NVIDIA, experimente a variante CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Compare a velocidade da transcrição em relação à variante CPU.

3. **Adicione formatação de saída** - a resposta JSON pode incluir:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Construa uma API REST** - embrulhe seu código de transcrição em um servidor web:

   | Linguagem | Framework | Exemplo |
   |----------|-----------|---------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` com `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` com `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` com `IFormFile` |

5. **Multi-turn com transcrição** - combine o Whisper com um agente de chat da Parte 4: transcreva o áudio primeiro e então passe o texto para o agente para análise ou resumo.

---

## Referência da API de Áudio do SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — cria uma instância de `AudioClient`
> - `audioClient.settings.language` — define a língua da transcrição (ex.: `"en"`)
> - `audioClient.settings.temperature` — controla a aleatoriedade (opcional)
> - `audioClient.transcribe(filePath)` — transcreve um arquivo, retorna `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — transmite partes da transcrição via callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — cria uma instância de `OpenAIAudioClient`
> - `audioClient.Settings.Language` — define a língua da transcrição (ex.: `"en"`)
> - `audioClient.Settings.Temperature` — controla a aleatoriedade (opcional)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transcreve um arquivo, retorna objeto com `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — retorna `IAsyncEnumerable` de partes da transcrição

> **Dica:** Sempre defina a propriedade de linguagem antes de transcrever. Sem isso, o modelo Whisper tenta auto-detecção, o que pode produzir saída corrompida (um único caractere de substituição em vez de texto).

---

## Comparação: Modelos de Chat vs. Whisper

| Aspecto | Modelos de Chat (Partes 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------------|------------------|-------------------|
| **Tipo de tarefa** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Entrada** | Mensagens de texto (JSON) | Arquivos de áudio (WAV/MP3/M4A) | Arquivos de áudio (WAV/MP3/M4A) |
| **Saída** | Texto gerado (streaming) | Texto transcrito (completo) | Texto transcrito (completo) |
| **Pacote SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Método API** | `client.chat.completions.create()` | ONNX Runtime direto | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Configuração de língua** | N/A | Tokens prompt do decoder | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Sim | Não | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Benefício de privacidade** | Código/dados ficam locais | Dados de áudio ficam locais | Dados de áudio ficam locais |

---

## Principais Conclusões

| Conceito | O que Você Aprendeu |
|---------|---------------------|
| **Whisper on-device** | Reconhecimento de fala para texto roda inteiramente local, ideal para transcrever chamadas de clientes Zava e avaliações de produto no dispositivo |
| **SDK AudioClient** | Os SDKs JavaScript e C# fornecem um `AudioClient` embutido que gerencia toda a pipeline de transcrição em uma única chamada |
| **Configuração de língua** | Sempre defina a língua do AudioClient (ex.: `"en"`) — sem isso, a auto-detecção pode produzir saída corrompida |
| **Python** | Usa `foundry-local-sdk` para gerenciamento de modelo + `onnxruntime` + `transformers` + `librosa` para inferência direta ONNX |
| **JavaScript** | Usa `foundry-local-sdk` com `model.createAudioClient()` — defina `settings.language` e chame `transcribe()` |
| **C#** | Usa `Microsoft.AI.Foundry.Local` com `model.GetAudioClientAsync()` — defina `Settings.Language` e chame `TranscribeAudioAsync()` |
| **Suporte a streaming** | SDKs JS e C# também oferecem `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` para saída em partes |
| **Otimizado para CPU** | A variante CPU (3,05 GB) funciona em qualquer dispositivo Windows sem GPU |
| **Privacidade em primeiro lugar** | Perfeito para manter interações com clientes Zava e dados proprietários no dispositivo |

---

## Recursos

| Recurso | Link |
|----------|------|
| Documentação Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referência SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Modelo OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Site Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Próximo Passo

Continue para [Parte 10: Usando Modelos Customizados ou Hugging Face](part10-custom-models.md) para compilar seus próprios modelos da Hugging Face e executá-los pelo Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:
Este documento foi traduzido utilizando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos empenhemos para garantir a precisão, esteja ciente de que traduções automatizadas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se tradução profissional realizada por humanos. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->