<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Workshop Foundry Local - Criar Aplicações de IA no Dispositivo

Um workshop prático para executar modelos de linguagem na sua própria máquina e construir aplicações inteligentes com [Foundry Local](https://foundrylocal.ai) e o [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **O que é o Foundry Local?** Foundry Local é um runtime leve que lhe permite descarregar, gerir e servir modelos de linguagem inteiramente no seu hardware. Expõe uma **API compatível com OpenAI** para que qualquer ferramenta ou SDK que utilize OpenAI possa conectar-se - sem necessidade de conta na cloud.

### 🌐 Suporte Multilíngue

#### Suportado via GitHub Action (Automatizado e Sempre Atualizado)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Árabe](../ar/README.md) | [Bengali](../bn/README.md) | [Búlgaro](../bg/README.md) | [Birmanês (Myanmar)](../my/README.md) | [Chinês (Simplificado)](../zh-CN/README.md) | [Chinês (Tradicional, Hong Kong)](../zh-HK/README.md) | [Chinês (Tradicional, Macau)](../zh-MO/README.md) | [Chinês (Tradicional, Taiwan)](../zh-TW/README.md) | [Croata](../hr/README.md) | [Checo](../cs/README.md) | [Dinamarquês](../da/README.md) | [Holandês](../nl/README.md) | [Estónio](../et/README.md) | [Finlandês](../fi/README.md) | [Francês](../fr/README.md) | [Alemão](../de/README.md) | [Grego](../el/README.md) | [Hebraico](../he/README.md) | [Hindi](../hi/README.md) | [Húngaro](../hu/README.md) | [Indonésio](../id/README.md) | [Italiano](../it/README.md) | [Japonês](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Coreano](../ko/README.md) | [Lituano](../lt/README.md) | [Malaio](../ms/README.md) | [Malaiala](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Pidgin Nigeriano](../pcm/README.md) | [Norueguês](../no/README.md) | [Persa (Farsi)](../fa/README.md) | [Polaco](../pl/README.md) | [Português (Brasil)](../pt-BR/README.md) | [Português (Portugal)](./README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romeno](../ro/README.md) | [Russo](../ru/README.md) | [Sérvio (Cirílico)](../sr/README.md) | [Eslovaco](../sk/README.md) | [Esloveno](../sl/README.md) | [Espanhol](../es/README.md) | [Suaíli](../sw/README.md) | [Sueco](../sv/README.md) | [Tagalo (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Tailandês](../th/README.md) | [Turco](../tr/README.md) | [Ucraniano](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamita](../vi/README.md)

> **Prefere Clonar Localmente?**
>
> Este repositório inclui mais de 50 traduções para línguas, o que aumenta significativamente o tamanho do download. Para clonar sem traduções, use o sparse checkout:
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
> Isto fornece tudo o que precisa para completar o curso com um download muito mais rápido.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Objetivos de Aprendizagem

No final deste workshop será capaz de:

| # | Objetivo |
|---|-----------|
| 1 | Instalar o Foundry Local e gerir modelos com o CLI |
| 2 | Dominar a API do Foundry Local SDK para gestão programática de modelos |
| 3 | Ligar-se ao servidor local de inferência usando os SDKs Python, JavaScript e C# |
| 4 | Construir um pipeline de Geração Augmentada por Recuperação (RAG) que baseia respostas nos seus próprios dados |
| 5 | Criar agentes de IA com instruções e personalidades persistentes |
| 6 | Orquestrar fluxos de trabalho multi-agente com ciclos de feedback |
| 7 | Explorar uma aplicação final de produção - o Zava Creative Writer |
| 8 | Construir frameworks de avaliação com conjuntos de dados ouro e pontuação LLM-como-juiz |
| 9 | Transcrever áudio com Whisper - fala para texto no dispositivo usando o Foundry Local SDK |
| 10 | Compilar e executar modelos personalizados ou Hugging Face com ONNX Runtime GenAI e Foundry Local |
| 11 | Permitir que modelos locais chamem funções externas com o padrão de chamada de ferramentas |
| 12 | Construir uma interface de utilizador baseada em browser para o Zava Creative Writer com streaming em tempo real |

---

## Pré-requisitos

| Requisito | Detalhes |
|-------------|---------|
| **Hardware** | Mínimo 8 GB RAM (16 GB recomendado); CPU com AVX2 ou GPU suportada |
| **SO** | Windows 10/11 (x64/ARM), Windows Server 2025, ou macOS 13+ |
| **Foundry Local CLI** | Instale via `winget install Microsoft.FoundryLocal` (Windows) ou `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Consulte o [guia de início rápido](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) para detalhes. |
| **Runtime de linguagem** | **Python 3.9+** e/ou **.NET 9.0+** e/ou **Node.js 18+** |
| **Git** | Para clonar este repositório |

---

## Começar

```bash
# 1. Clone o repositório
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifique se o Foundry Local está instalado
foundry model list              # Liste os modelos disponíveis
foundry model run phi-3.5-mini  # Inicie um chat interativo

# 3. Escolha o seu percurso de idioma (consulte o laboratório da Parte 2 para configuração completa)
```

| Linguagem | Início Rápido |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Partes do Workshop

### Parte 1: Início com Foundry Local

**Guia do laboratório:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- O que é o Foundry Local e como funciona
- Instalar o CLI no Windows e macOS
- Explorar modelos - listar, descarregar, executar
- Compreender aliases de modelos e portas dinâmicas

---

### Parte 2: Mergulho Profundo no Foundry Local SDK

**Guia do laboratório:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Por que usar o SDK em vez do CLI para desenvolvimento de aplicações
- Referência completa da API do SDK para Python, JavaScript e C#
- Gestão de serviços, exploração de catálogo, ciclo de vida do modelo (descarregar, carregar, descarregar)
- Padrões de início rápido: bootstrap do construtor em Python, `init()` em JavaScript, `CreateAsync()` em C#
- Metadados `FoundryModelInfo`, aliases, e seleção de modelo otimizado para hardware

---

### Parte 3: SDKs e APIs

**Guia do laboratório:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Ligação ao Foundry Local a partir de Python, JavaScript e C#
- Usar Foundry Local SDK para gerir o serviço programaticamente
- Streaming de conversas via API compatível com OpenAI
- Referência dos métodos do SDK para cada linguagem

**Exemplos de código:**

| Linguagem | Ficheiro | Descrição |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat básico em streaming |
| C# | `csharp/BasicChat.cs` | Chat em streaming com .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat em streaming com Node.js |

---

### Parte 4: Geração Augmentada por Recuperação (RAG)

**Guia do laboratório:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- O que é RAG e por que é importante
- Construir uma base de conhecimento em memória
- Recuperação de sobreposição de palavras-chave com pontuação
- Compor prompts do sistema fundamentados
- Executar um pipeline RAG completo no dispositivo

**Exemplos de código:**

| Linguagem | Ficheiro |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Parte 5: Construir Agentes de IA

**Guia do laboratório:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- O que é um agente de IA (vs. uma chamada bruta de LLM)
- O padrão `ChatAgent` e o Microsoft Agent Framework
- Instruções do sistema, personas, e conversas com múltiplas trocas
- Saída estruturada (JSON) dos agentes

**Exemplos de código:**

| Linguagem | Ficheiro | Descrição |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agente único com Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agente único (padrão ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agente único (padrão ChatAgent) |

---

### Parte 6: Fluxos de Trabalho Multi-Agente

**Guia do laboratório:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipelines multi-agente: Investigador → Escritor → Editor
- Orquestração sequencial e ciclos de feedback
- Configuração partilhada e entregas estruturadas
- Desenhar o seu próprio fluxo de trabalho multi-agente

**Exemplos de código:**

| Linguagem | Ficheiro | Descrição |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline de três agentes |
| C# | `csharp/MultiAgent.cs` | Pipeline de três agentes |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline de três agentes |

---

### Parte 7: Zava Creative Writer - Aplicação Final

**Guia do laboratório:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Aplicação multi-agente ao estilo de produção com 4 agentes especializados
- Pipeline sequencial com ciclos de feedback conduzidos por avaliadores
- Streaming de saída, pesquisa de catálogo de produtos, entregas estruturadas em JSON
- Implementação completa em Python (FastAPI), JavaScript (Node.js CLI) e C# (console .NET)

**Exemplos de código:**

| Linguagem | Diretório | Descrição |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Serviço web FastAPI com orquestrador |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplicação CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplicação console .NET 9 |

---

### Parte 8: Desenvolvimento Orientado por Avaliação

**Guia do laboratório:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Construir um framework sistemático de avaliação para agentes de IA usando conjuntos de dados ouro
- Verificações baseadas em regras (comprimento, cobertura de palavras-chave, termos proibidos) + pontuação LLM-como-juiz
- Comparação lado a lado de variantes de prompts com tabelas de pontuação agregada
- Extensão do padrão do agente Editor do Zava da Parte 7 para um conjunto de testes offline
- Trilhas em Python, JavaScript e C#

**Exemplos de código:**

| Linguagem | Ficheiro | Descrição |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Framework de avaliação |
| C# | `csharp/AgentEvaluation.cs` | Framework de avaliação |
| JavaScript | `javascript/foundry-local-eval.mjs` | Framework de avaliação |

---

### Parte 9: Transcrição de Voz com Whisper

**Guia do laboratório:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Transcrição de fala para texto usando OpenAI Whisper a correr localmente
- Processamento de áudio com foco na privacidade - o áudio nunca sai do seu dispositivo
- Tracks em Python, JavaScript e C# com `client.audio.transcriptions.create()` (Python/JS) e `AudioClient.TranscribeAudioAsync()` (C#)
- Inclui ficheiros áudio sampleados com tema Zava para prática prática

**Exemplos de código:**

| Língua | Ficheiro | Descrição |
|--------|----------|-----------|
| Python | `python/foundry-local-whisper.py` | Transcrição de voz Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transcrição de voz Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transcrição de voz Whisper |

> **Nota:** Este laboratório usa o **Foundry Local SDK** para descarregar programaticamente e carregar o modelo Whisper, enviando depois áudio para o endpoint local compatível com OpenAI para transcrição. O modelo Whisper (`whisper`) está listado no catálogo Foundry Local e corre inteiramente no dispositivo - não são necessárias chaves API na cloud nem acesso à rede.

---

### Parte 10: Usar Modelos Personalizados ou Hugging Face

**Guia do laboratório:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilar modelos Hugging Face para formato ONNX optimizado usando o construtor de modelos ONNX Runtime GenAI
- Compilação específica para hardware (CPU, GPU NVIDIA, DirectML, WebGPU) e quantização (int4, fp16, bf16)
- Criar ficheiros de configuração de template para chat no Foundry Local
- Adicionar modelos compilados ao cache do Foundry Local
- Executar modelos personalizados via CLI, REST API e OpenAI SDK
- Exemplo de referência: compilação completa do Qwen/Qwen3-0.6B

---

### Parte 11: Chamadas a Ferramentas com Modelos Locais

**Guia do laboratório:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Permitir que modelos locais chamem funções externas (chamada de ferramenta/função)
- Definir esquemas de ferramentas utilizando o formato de chamada de funções OpenAI
- Gerir o fluxo de conversa de chamadas de ferramentas em múltiplas interações
- Executar chamadas a ferramentas localmente e devolver resultados ao modelo
- Escolher o modelo certo para cenários de chamada de ferramenta (Qwen 2.5, Phi-4-mini)
- Usar o `ChatClient` nativo do SDK para chamadas de ferramentas (JavaScript)

**Exemplos de código:**

| Língua | Ficheiro | Descrição |
|--------|----------|-----------|
| Python | `python/foundry-local-tool-calling.py` | Chamadas de ferramenta com ferramentas de clima/população |
| C# | `csharp/ToolCalling.cs` | Chamadas de ferramenta com .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Chamadas de ferramenta com ChatClient |

---

### Parte 12: Construir uma UI Web para o Zava Creative Writer

**Guia do laboratório:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Adicionar uma interface frontal baseada no browser ao Zava Creative Writer
- Servir a UI partilhada a partir de Python (FastAPI), JavaScript (Node.js HTTP) e C# (ASP.NET Core)
- Consumir NDJSON em streaming no browser com Fetch API e ReadableStream
- Insígnias de status do agente em direto e transmissão de texto de artigo em tempo real

**Código (UI partilhada):**

| Ficheiro | Descrição |
|----------|-----------|
| `zava-creative-writer-local/ui/index.html` | Layout da página |
| `zava-creative-writer-local/ui/style.css` | Estilo |
| `zava-creative-writer-local/ui/app.js` | Lógica do leitor de stream e atualização do DOM |

**Adições ao backend:**

| Língua | Ficheiro | Descrição |
|--------|----------|-----------|
| Python | `zava-creative-writer-local/src/api/main.py` | Atualizado para servir UI estática |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Novo servidor HTTP que envolve o orquestrador |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Novo projeto API minimalista ASP.NET Core |

---

### Parte 13: Oficina Completa

**Guia do laboratório:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Resumo de tudo o que construiu ao longo das 12 partes
- Ideias adicionais para expandir as suas aplicações
- Ligações para recursos e documentação

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

| Recurso | Ligação |
|---------|---------|
| Website Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Catálogo de modelos | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Guia de início rápido | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referência SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licença

Este material do workshop é fornecido para fins educacionais.

---

**Bom desenvolvimento! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso legal**:
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos por garantir a precisão, por favor, tenha em conta que traduções automáticas podem conter erros ou imprecisões. O documento original na sua língua nativa deve ser considerado a fonte autoritativa. Para informações críticas, recomenda-se a tradução profissional feita por humanos. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->