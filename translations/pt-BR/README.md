<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Workshop Foundry Local - Crie Aplicativos de IA no Dispositivo

Um workshop prático para executar modelos de linguagem na sua própria máquina e construir aplicações inteligentes com o [Foundry Local](https://foundrylocal.ai) e o [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **O que é Foundry Local?** Foundry Local é um ambiente de execução leve que permite baixar, gerenciar e servir modelos de linguagem inteiramente no seu hardware. Ele expõe uma **API compatível com OpenAI** para que qualquer ferramenta ou SDK que use OpenAI possa se conectar - sem necessidade de conta na nuvem.

### 🌐 Suporte Multilíngue

#### Suportado via GitHub Action (Automatizado e Sempre Atualizado)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](./README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Prefere Clonar Localmente?**
>
> Este repositório inclui mais de 50 traduções de idiomas, o que aumenta significativamente o tamanho do download. Para clonar sem traduções, use checkout esparso:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> Isso lhe dá tudo o que você precisa para completar o curso com um download muito mais rápido.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Objetivos de Aprendizagem

Ao final deste workshop, você será capaz de:

| # | Objetivo |
|---|----------|
| 1 | Instalar o Foundry Local e gerenciar modelos com o CLI |
| 2 | Dominar a API do Foundry Local SDK para gerenciamento programático de modelos |
| 3 | Conectar-se ao servidor de inferência local usando os SDKs Python, JavaScript e C# |
| 4 | Construir um pipeline de Geração Aumentada por Recuperação (RAG) que fundamenta respostas em seus próprios dados |
| 5 | Criar agentes de IA com instruções persistentes e personas |
| 6 | Orquestrar fluxos de trabalho multi-agentes com ciclos de feedback |
| 7 | Explorar um aplicativo capstone de produção - o Zava Creative Writer |
| 8 | Construir frameworks de avaliação com conjuntos de dados de referência e pontuações com LLM como juiz |
| 9 | Transcrever áudio com Whisper - reconhecimento de voz no dispositivo usando o Foundry Local SDK |
| 10 | Compilar e executar modelos customizados ou do Hugging Face com ONNX Runtime GenAI e Foundry Local |
| 11 | Permitir que modelos locais chamem funções externas com o padrão de chamadas de ferramenta |
| 12 | Construir uma interface de navegador para o Zava Creative Writer com streaming em tempo real |

---

## Pré-requisitos

| Requisito | Detalhes |
|-----------|----------|
| **Hardware** | Mínimo 8 GB RAM (16 GB recomendado); CPU com suporte a AVX2 ou GPU compatível |
| **SO** | Windows 10/11 (x64/ARM), Windows Server 2025 ou macOS 13+ |
| **Foundry Local CLI** | Instale via `winget install Microsoft.FoundryLocal` (Windows) ou `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Veja o [guia de início rápido](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) para mais detalhes. |
| **Runtime de linguagem** | **Python 3.9+** e/ou **.NET 9.0+** e/ou **Node.js 18+** |
| **Git** | Para clonar este repositório |

---

## Começando

```bash
# 1. Clone o repositório
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifique se o Foundry Local está instalado
foundry model list              # Liste os modelos disponíveis
foundry model run phi-3.5-mini  # Inicie um chat interativo

# 3. Escolha sua trilha de idioma (veja o laboratório da Parte 2 para configuração completa)
```

| Linguagem | Início Rápido |
|-----------|--------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Partes do Workshop

### Parte 1: Introdução ao Foundry Local

**Guia do laboratório:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- O que é Foundry Local e como funciona
- Instalando o CLI no Windows e macOS
- Explorando modelos - listando, baixando, executando
- Entendendo aliases de modelos e portas dinâmicas

---

### Parte 2: Mergulho Profundo no Foundry Local SDK

**Guia do laboratório:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Por que usar o SDK em vez do CLI para desenvolvimento de aplicações
- Referência completa da API do SDK para Python, JavaScript e C#
- Gerenciamento de serviço, navegação no catálogo, ciclo de vida do modelo (download, carregamento, descarregamento)
- Padrões para início rápido: bootstrap construtor Python, `init()` JavaScript, `CreateAsync()` C#
- Metadata `FoundryModelInfo`, aliases e seleção de modelo otimizada para hardware

---

### Parte 3: SDKs e APIs

**Guia do laboratório:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Conectando-se ao Foundry Local pelo Python, JavaScript e C#
- Usando o Foundry Local SDK para gerenciar o serviço programaticamente
- Transmissão de respostas de chat via API compatível com OpenAI
- Referência de métodos do SDK para cada linguagem

**Exemplos de código:**

| Linguagem | Arquivo | Descrição |
|-----------|---------|-----------|
| Python | `python/foundry-local.py` | Chat básico com streaming |
| C# | `csharp/BasicChat.cs` | Chat com streaming usando .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat com streaming usando Node.js |

---

### Parte 4: Geração Aumentada por Recuperação (RAG)

**Guia do laboratório:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- O que é RAG e por que é importante
- Construindo uma base de conhecimento em memória
- Recuperação por sobreposição de palavras-chave com pontuação
- Composição de prompts de sistema fundamentados
- Executando um pipeline completo de RAG no dispositivo

**Exemplos de código:**

| Linguagem | Arquivo |
|-----------|---------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Parte 5: Construindo Agentes de IA

**Guia do laboratório:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- O que é um agente de IA (vs. uma chamada bruta de LLM)
- O padrão `ChatAgent` e o Microsoft Agent Framework
- Instruções de sistema, personas e conversas multi-turno
- Saída estruturada (JSON) dos agentes

**Exemplos de código:**

| Linguagem | Arquivo | Descrição |
|-----------|---------|-----------|
| Python | `python/foundry-local-with-agf.py` | Agente único com Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agente único (padrão ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agente único (padrão ChatAgent) |

---

### Parte 6: Fluxos de Trabalho Multi-Agentes

**Guia do laboratório:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipelines multi-agentes: Pesquisador → Escritor → Editor
- Orquestração sequencial e ciclos de feedback
- Configuração compartilhada e transferências estruturadas
- Projetando seu próprio fluxo de trabalho multi-agente

**Exemplos de código:**

| Linguagem | Arquivo | Descrição |
|-----------|---------|-----------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline com três agentes |
| C# | `csharp/MultiAgent.cs` | Pipeline com três agentes |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline com três agentes |

---

### Parte 7: Zava Creative Writer - Aplicação Capstone

**Guia do laboratório:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Um app multi-agente estilo produção com 4 agentes especializados
- Pipeline sequencial com ciclos de feedback acionados por avaliador
- Saída por streaming, busca em catálogo de produtos, transferências JSON estruturadas
- Implementação completa em Python (FastAPI), JavaScript (Node.js CLI) e C# (console .NET)

**Exemplos de código:**

| Linguagem | Diretório | Descrição |
|-----------|-----------|-----------|
| Python | `zava-creative-writer-local/src/api/` | Serviço web FastAPI com orquestrador |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplicativo CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplicativo console .NET 9 |

---

### Parte 8: Desenvolvimento Voltado para Avaliação

**Guia do laboratório:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Construir um framework sistemático de avaliação para agentes de IA usando conjuntos de dados de referência
- Verificações baseadas em regras (comprimento, cobertura de palavras-chave, termos proibidos) + pontuação LLM como juiz
- Comparação lado a lado de variantes de prompt com scorecards agregados
- Extensão do padrão de agente Editor do Zava da Parte 7, em uma suíte de testes offline
- Trilhas para Python, JavaScript e C#

**Exemplos de código:**

| Linguagem | Arquivo | Descrição |
|-----------|---------|-----------|
| Python | `python/foundry-local-eval.py` | Framework de avaliação |
| C# | `csharp/AgentEvaluation.cs` | Framework de avaliação |
| JavaScript | `javascript/foundry-local-eval.mjs` | Framework de avaliação |

---

### Parte 9: Transcrição de Voz com Whisper

**Guia do laboratório:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Transcrição de fala para texto usando OpenAI Whisper rodando localmente
- Processamento de áudio com foco em privacidade - o áudio nunca sai do seu dispositivo
- Trilhas Python, JavaScript e C# com `client.audio.transcriptions.create()` (Python/JS) e `AudioClient.TranscribeAudioAsync()` (C#)
- Inclui arquivos de áudio de exemplo com tema Zava para prática prática

**Exemplos de código:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transcrição de voz Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transcrição de voz Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transcrição de voz Whisper |

> **Nota:** Este laboratório usa o **Foundry Local SDK** para baixar e carregar programaticamente o modelo Whisper, depois envia áudio para o endpoint local compatível com OpenAI para transcrição. O modelo Whisper (`whisper`) está listado no catálogo Foundry Local e roda inteiramente no dispositivo - nenhuma chave de API de nuvem ou acesso à rede é necessário.

---

### Parte 10: Usando Modelos Personalizados ou Hugging Face

**Guia do laboratório:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilação de modelos Hugging Face para formato ONNX otimizado usando o construtor de modelo GenAI do ONNX Runtime
- Compilação específica para hardware (CPU, GPU NVIDIA, DirectML, WebGPU) e quantização (int4, fp16, bf16)
- Criação de arquivos de configuração de template de chat para o Foundry Local
- Adição de modelos compilados ao cache do Foundry Local
- Execução de modelos personalizados via CLI, REST API e OpenAI SDK
- Exemplo de referência: compilando Qwen/Qwen3-0.6B do início ao fim

---

### Parte 11: Chamada de Ferramentas com Modelos Locais

**Guia do laboratório:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Permitir que modelos locais chamem funções externas (chamada de ferramenta/função)
- Definir esquemas de ferramentas usando o formato de chamada de função do OpenAI
- Gerenciar o fluxo de conversa de chamada de ferramenta com múltiplas interações
- Executar chamadas de ferramenta localmente e retornar resultados ao modelo
- Escolher o modelo correto para cenários de chamada de ferramenta (Qwen 2.5, Phi-4-mini)
- Usar o `ChatClient` nativo do SDK para chamadas de ferramenta (JavaScript)

**Exemplos de código:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Chamada de ferramentas com ferramentas de clima/população |
| C# | `csharp/ToolCalling.cs` | Chamada de ferramentas com .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Chamada de ferramentas com ChatClient |

---

### Parte 12: Construindo uma UI Web para o Zava Creative Writer

**Guia do laboratório:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Adicionar uma interface frontend baseada em navegador para o Zava Creative Writer
- Servir a UI compartilhada a partir de Python (FastAPI), JavaScript (Node.js HTTP) e C# (ASP.NET Core)
- Consumir NDJSON em streaming no navegador com Fetch API e ReadableStream
- Indicadores de status do agente ao vivo e streaming em tempo real do texto do artigo

**Código (UI compartilhada):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Layout da página |
| `zava-creative-writer-local/ui/style.css` | Estilização |
| `zava-creative-writer-local/ui/app.js` | Leitor de streaming e lógica de atualização do DOM |

**Adições no backend:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Atualizado para servir UI estática |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Novo servidor HTTP envolvendo o orquestrador |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Novo projeto mínimo de API ASP.NET Core |

---

### Parte 13: Workshop Completo

**Guia do laboratório:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Resumo de tudo que você construiu ao longo das 12 partes
- Ideias adicionais para expandir suas aplicações
- Links para recursos e documentação

---

## Estrutura do Projeto

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## Recursos

| Resource | Link |
|----------|------|
| Site Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Catálogo de modelos | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Guia de início rápido | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referência do SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licença

Este material do workshop é fornecido para fins educacionais.

---

**Bons projetos! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:
Este documento foi traduzido utilizando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos para garantir a precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autoritativa. Para informações críticas, recomenda-se a tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações equivocadas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->