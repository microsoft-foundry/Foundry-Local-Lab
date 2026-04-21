<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Workshop Foundry Local - Construir Aplicações de IA Localmente

Um workshop prático para executar modelos de linguagem na sua própria máquina e construir aplicações inteligentes com o [Foundry Local](https://foundrylocal.ai) e o [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **O que é o Foundry Local?** Foundry Local é um runtime leve que permite descarregar, gerir e servir modelos de linguagem inteiramente no seu hardware. Expõe uma **API compatível com OpenAI** para que qualquer ferramenta ou SDK que suporte OpenAI possa ligar-se — não é necessária conta na cloud.

---

## Objetivos de Aprendizagem

No final deste workshop será capaz de:

| # | Objetivo |
|---|-----------|
| 1 | Instalar o Foundry Local e gerir modelos com a CLI |
| 2 | Dominar a API do Foundry Local SDK para gestão programática de modelos |
| 3 | Ligar-se ao servidor local de inferência usando os SDKs Python, JavaScript e C# |
| 4 | Construir um pipeline Retrieval-Augmented Generation (RAG) que fundamenta respostas nos seus próprios dados |
| 5 | Criar agentes de IA com instruções e personas persistentes |
| 6 | Orquestrar fluxos de trabalho multi-agentes com ciclos de feedback |
| 7 | Explorar uma aplicação de capstone em produção — o Zava Creative Writer |
| 8 | Construir frameworks de avaliação com conjuntos de dados golden e scoring LLM-como-juiz |
| 9 | Transcrever áudio com Whisper — reconhecimento de voz local com o Foundry Local SDK |
| 10 | Compilar e executar modelos personalizados ou Hugging Face com ONNX Runtime GenAI e Foundry Local |
| 11 | Permitir que modelos locais chamem funções externas com o padrão tool-calling |
| 12 | Construir uma interface web para o Zava Creative Writer com streaming em tempo real |

---

## Pré-requisitos

| Requisito | Detalhes |
|-------------|---------|
| **Hardware** | Mínimo 8 GB de RAM (16 GB recomendado); CPU compatível com AVX2 ou GPU suportada |
| **SO** | Windows 10/11 (x64/ARM), Windows Server 2025 ou macOS 13+ |
| **Foundry Local CLI** | Instale via `winget install Microsoft.FoundryLocal` (Windows) ou `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Veja o [guia de início rápido](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) para detalhes. |
| **Runtime de linguagem** | **Python 3.9+** e/ou **.NET 9.0+** e/ou **Node.js 18+** |
| **Git** | Para clonar este repositório |

---

## Iniciar

```bash
# 1. Clone o repositório
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifique se o Foundry Local está instalado
foundry model list              # Liste os modelos disponíveis
foundry model run phi-3.5-mini  # Inicie um chat interativo

# 3. Escolha o seu percurso linguístico (consulte o laboratório da Parte 2 para configuração completa)
```

| Linguagem | Início Rápido |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Partes do Workshop

### Parte 1: Introdução ao Foundry Local

**Guia do laboratório:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- O que é o Foundry Local e como funciona
- Instalar a CLI no Windows e macOS
- Explorar modelos - listar, descarregar, executar
- Entender aliases de modelos e portas dinâmicas

---

### Parte 2: Exploração Profunda do Foundry Local SDK

**Guia do laboratório:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Por que usar o SDK em vez da CLI para desenvolvimento de aplicações
- Referência completa da API do SDK para Python, JavaScript e C#
- Gestão do serviço, navegação no catálogo, ciclo de vida do modelo (download, carregamento, descarregamento)
- Padrões de início rápido: bootstrap do construtor Python, `init()` em JavaScript, `CreateAsync()` em C#
- Metadados `FoundryModelInfo`, aliases e seleção de modelos otimizada para hardware

---

### Parte 3: SDKs e APIs

**Guia do laboratório:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Conectar-se ao Foundry Local a partir de Python, JavaScript e C#
- Usar o Foundry Local SDK para gerir o serviço programaticamente
- Streaming de chat completions via API compatível com OpenAI
- Referência dos métodos do SDK para cada linguagem

**Exemplos de código:**

| Linguagem | Ficheiro | Descrição |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat básico com streaming |
| C# | `csharp/BasicChat.cs` | Chat com streaming em .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat com streaming em Node.js |

---

### Parte 4: Retrieval-Augmented Generation (RAG)

**Guia do laboratório:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- O que é RAG e por que é importante
- Construir uma base de conhecimento em memória
- Recuperação por sobreposição de palavras-chave com scoring
- Compor prompts do sistema fundamentados
- Executar um pipeline RAG completo localmente

**Exemplos de código:**

| Linguagem | Ficheiro |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Parte 5: Construir Agentes de IA

**Guia do laboratório:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- O que é um agente de IA (vs. uma chamada direta a LLM)
- Padrão `ChatAgent` e Microsoft Agent Framework
- Instruções do sistema, personas e conversas multi-turno
- Saída estruturada (JSON) dos agentes

**Exemplos de código:**

| Linguagem | Ficheiro | Descrição |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agente único com Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agente único (padrão ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agente único (padrão ChatAgent) |

---

### Parte 6: Fluxos de Trabalho Multi-Agentes

**Guia do laboratório:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipelines multi-agentes: Investigador → Escritor → Editor
- Orquestração sequencial e ciclos de feedback
- Configuração partilhada e entregas estruturadas
- Conceber o seu próprio fluxo de trabalho multi-agente

**Exemplos de código:**

| Linguagem | Ficheiro | Descrição |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline de três agentes |
| C# | `csharp/MultiAgent.cs` | Pipeline de três agentes |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline de três agentes |

---

### Parte 7: Zava Creative Writer - Aplicação Capstone

**Guia do laboratório:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Aplicação multi-agente em estilo de produção com 4 agentes especializados
- Pipeline sequencial com ciclos de feedback orientados por avaliadores
- Saída em streaming, pesquisa no catálogo de produtos, entregas em JSON estruturado
- Implementação completa em Python (FastAPI), JavaScript (Node.js CLI) e C# (console .NET)

**Exemplos de código:**

| Linguagem | Diretório | Descrição |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Serviço web FastAPI com orquestrador |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplicação CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplicação consola .NET 9 |

---

### Parte 8: Desenvolvimento Orientado a Avaliação

**Guia do laboratório:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Construir um framework sistemático de avaliação para agentes de IA usando conjuntos de dados golden
- Verificações baseadas em regras (comprimento, cobertura de palavras-chave, termos proibidos) + scoring LLM-como-juiz
- Comparação lado a lado de variantes de prompt com scorecards agregados
- Estende o padrão do agente Editor Zava da Parte 7 para um conjunto de testes offline
- Trilhos para Python, JavaScript e C#

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
- Processamento de áudio com foco na privacidade — o áudio nunca sai do dispositivo
- Trilhos Python, JavaScript e C# com `client.audio.transcriptions.create()` (Python/JS) e `AudioClient.TranscribeAudioAsync()` (C#)
- Inclui ficheiros de áudio sample com tema Zava para prática

**Exemplos de código:**

| Linguagem | Ficheiro | Descrição |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transcrição de voz com Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transcrição de voz com Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transcrição de voz com Whisper |

> **Nota:** Este laboratório usa o **Foundry Local SDK** para descarregar e carregar programaticamente o modelo Whisper, depois envia áudio para o endpoint local compatível com OpenAI para transcrição. O modelo Whisper (`whisper`) está listado no catálogo do Foundry Local e corre inteiramente localmente — sem chaves API cloud nem acesso à rede necessário.

---

### Parte 10: Usar Modelos Personalizados ou Hugging Face

**Guia do laboratório:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilar modelos Hugging Face para formato ONNX otimizado usando o construtor de modelos ONNX Runtime GenAI
- Compilação específica para hardware (CPU, GPU NVIDIA, DirectML, WebGPU) e quantização (int4, fp16, bf16)
- Criar ficheiros de configuração de template de chat para Foundry Local
- Adicionar modelos compilados ao cache do Foundry Local
- Executar modelos personalizados via CLI, API REST e SDK OpenAI
- Exemplo de referência: compilação end-to-end do Qwen/Qwen3-0.6B

---

### Parte 11: Tool Calling com Modelos Locais

**Guia do laboratório:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Permitir que modelos locais chamem funções externas (tool/function calling)
- Definir esquemas de ferramentas usando o formato de chamadas de função OpenAI
- Gerir o fluxo da conversa multi-turno para chamadas de ferramentas
- Executar chamadas de ferramentas localmente e devolver os resultados ao modelo
- Escolher o modelo certo para cenários de chamadas de ferramenta (Qwen 2.5, Phi-4-mini)
- Usar o `ChatClient` nativo do SDK para chamadas de ferramenta (JavaScript)

**Exemplos de código:**

| Linguagem | Ficheiro | Descrição |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Chamada de ferramentas com ferramentas de tempo e população |
| C# | `csharp/ToolCalling.cs` | Chamada de ferramentas com .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Chamada de ferramentas com ChatClient |

---

### Parte 12: Construir uma Interface Web para o Zava Creative Writer

**Guia do laboratório:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Adicionar uma interface front-end baseada em browser ao Zava Creative Writer
- Servir a UI partilhada a partir de Python (FastAPI), JavaScript (Node.js HTTP) e C# (ASP.NET Core)
- Consumir NDJSON em streaming no browser com Fetch API e ReadableStream
- Insígnias de estado do agente em direto e streaming do texto do artigo em tempo real

**Código (UI partilhada):**

| Ficheiro | Descrição |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Layout da página |
| `zava-creative-writer-local/ui/style.css` | Estilo |
| `zava-creative-writer-local/ui/app.js` | Leitor de stream e lógica de atualização do DOM |

**Adições ao backend:**

| Linguagem | Ficheiro | Descrição |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Atualizado para servir UI estática |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Novo servidor HTTP a envolver o orquestrador |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Novo projeto API minimal ASP.NET Core |

---

### Parte 13: Workshop Concluído
**Guia do laboratório:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Sumário de tudo o que construiu ao longo das 12 partes
- Ideias adicionais para expandir as suas aplicações
- Ligações para recursos e documentação

---

## Estrutura do projeto

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
|----------|------|
| Website Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Catálogo de modelos | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Guia de introdução | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referência do SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licença

Este material do workshop é fornecido para fins educativos.

---

**Boa construção! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:
Este documento foi traduzido utilizando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos por garantir a precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original na sua língua nativa deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se tradução profissional feita por humanos. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações erradas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->