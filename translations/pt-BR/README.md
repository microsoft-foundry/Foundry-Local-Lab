<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Workshop Foundry Local - Crie Apps de IA no Dispositivo

Um workshop prático para rodar modelos de linguagem na sua própria máquina e construir aplicações inteligentes com [Foundry Local](https://foundrylocal.ai) e o [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **O que é o Foundry Local?** Foundry Local é um runtime leve que permite baixar, gerenciar e servir modelos de linguagem inteiramente no seu hardware. Ele expõe uma **API compatível com OpenAI** para que qualquer ferramenta ou SDK que utilize OpenAI possa se conectar - não é necessária conta na nuvem.

---

## Objetivos de Aprendizagem

Ao final deste workshop você será capaz de:

| # | Objetivo |
|---|----------|
| 1 | Instalar o Foundry Local e gerenciar modelos com o CLI |
| 2 | Dominar a API do Foundry Local SDK para gerenciamento programático de modelos |
| 3 | Conectar ao servidor de inferência local usando os SDKs Python, JavaScript e C# |
| 4 | Construir um pipeline de Geração Aumentada por Recuperação (RAG) que fundamenta respostas em seus próprios dados |
| 5 | Criar agentes de IA com instruções e personas persistentes |
| 6 | Orquestrar fluxos de trabalho multi-agentes com ciclos de feedback |
| 7 | Explorar um app capstone de produção - o Zava Creative Writer |
| 8 | Construir frameworks de avaliação com conjuntos de dados dourados e pontuação tipo LLM-como-juiz |
| 9 | Transcrever áudio com Whisper - reconhecimento de fala no dispositivo usando o Foundry Local SDK |
| 10 | Compilar e rodar modelos customizados ou do Hugging Face com ONNX Runtime GenAI e Foundry Local |
| 11 | Habilitar modelos locais para chamar funções externas com o padrão de ferramenta-chamada |
| 12 | Construir uma interface web para o Zava Creative Writer com streaming em tempo real |

---

## Pré-requisitos

| Requisito | Detalhes |
|-----------|----------|
| **Hardware** | Mínimo 8 GB RAM (16 GB recomendado); CPU com AVX2 ou GPU suportada |
| **SO** | Windows 10/11 (x64/ARM), Windows Server 2025 ou macOS 13+ |
| **Foundry Local CLI** | Instale via `winget install Microsoft.FoundryLocal` (Windows) ou `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Veja o [guia de início rápido](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) para detalhes. |
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

# 3. Escolha sua trilha de idioma (consulte o laboratório da Parte 2 para configuração completa)
```

| Linguagem | Início Rápido |
|-----------|--------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Partes do Workshop

### Parte 1: Começando com Foundry Local

**Guia do laboratório:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- O que é Foundry Local e como funciona
- Instalando o CLI no Windows e macOS
- Explorando modelos - listagem, download, execução
- Entendendo aliases de modelo e portas dinâmicas

---

### Parte 2: Imersão no Foundry Local SDK

**Guia do laboratório:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Por que usar o SDK em vez do CLI para desenvolvimento de aplicações
- Referência completa da API do SDK para Python, JavaScript e C#
- Gerenciamento do serviço, navegação no catálogo, ciclo de vida do modelo (download, carregar, descarregar)
- Padrões de início rápido: bootstrap no construtor Python, `init()` em JavaScript, `CreateAsync()` em C#
- Metadados FoundryModelInfo, aliases e seleção de modelo otimizada para hardware

---

### Parte 3: SDKs e APIs

**Guia do laboratório:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Conectando-se ao Foundry Local via Python, JavaScript e C#
- Usando o Foundry Local SDK para gerenciar o serviço programaticamente
- Completions de chat em streaming via API compatível com OpenAI
- Referência dos métodos do SDK para cada linguagem

**Exemplos de código:**

| Linguagem | Arquivo | Descrição |
|-----------|---------|-----------|
| Python | `python/foundry-local.py` | Chat em streaming básico |
| C# | `csharp/BasicChat.cs` | Chat em streaming com .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat em streaming com Node.js |

---

### Parte 4: Geração Aumentada por Recuperação (RAG)

**Guia do laboratório:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- O que é RAG e por que importa
- Construindo uma base de conhecimento na memória
- Recuperação por sobreposição de palavras-chave com pontuação
- Composição de prompts de sistema fundamentados
- Rodando um pipeline completo RAG no dispositivo

**Exemplos de código:**

| Linguagem | Arquivo |
|-----------|---------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Parte 5: Construindo Agentes de IA

**Guia do laboratório:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- O que é um agente de IA (vs. chamada direta de LLM)
- O padrão `ChatAgent` e o Microsoft Agent Framework
- Instruções do sistema, personas e conversas multi-turno
- Saída estruturada (JSON) gerada por agentes

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
- Configuração compartilhada e transições estruturadas
- Design do seu próprio fluxo de trabalho multi-agente

**Exemplos de código:**

| Linguagem | Arquivo | Descrição |
|-----------|---------|-----------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline com três agentes |
| C# | `csharp/MultiAgent.cs` | Pipeline com três agentes |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline com três agentes |

---

### Parte 7: Zava Creative Writer - Aplicação Capstone

**Guia do laboratório:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Aplicação multi-agente estilo produção com 4 agentes especializados
- Pipeline sequencial com feedback orientado por avaliadores
- Saída em streaming, busca em catálogo de produtos e transições JSON estruturadas
- Implementação completa em Python (FastAPI), JavaScript (Node.js CLI) e C# (console .NET)

**Exemplos de código:**

| Linguagem | Diretório | Descrição |
|-----------|-----------|-----------|
| Python | `zava-creative-writer-local/src/api/` | Serviço web FastAPI com orquestrador |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplicação CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplicação console .NET 9 |

---

### Parte 8: Desenvolvimento Guiado por Avaliação

**Guia do laboratório:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Construir um framework sistemático de avaliação para agentes de IA usando datasets dourados
- Checagens baseadas em regras (tamanho, cobertura por palavras-chave, termos proibidos) + pontuação tipo LLM-como-juiz
- Comparação lado a lado de variantes de prompts com scorecards agregados
- Extende o padrão do agente Editor do Zava da Parte 7 para uma suíte de testes offline
- Trilhas em Python, JavaScript e C#

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
- Processamento de áudio focado em privacidade - o áudio nunca sai do seu dispositivo
- Trilhas em Python, JavaScript e C# com `client.audio.transcriptions.create()` (Python/JS) e `AudioClient.TranscribeAudioAsync()` (C#)
- Inclui arquivos de áudio temáticos Zava para prática prática

**Exemplos de código:**

| Linguagem | Arquivo | Descrição |
|-----------|---------|-----------|
| Python | `python/foundry-local-whisper.py` | Transcrição de voz Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transcrição de voz Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transcrição de voz Whisper |

> **Nota:** Este laboratório usa o **Foundry Local SDK** para baixar e carregar programaticamente o modelo Whisper, depois envia o áudio para o endpoint local compatível com OpenAI para transcrição. O modelo Whisper (`whisper`) está listado no catálogo do Foundry Local e roda inteiramente no dispositivo - não são necessárias chaves de API da nuvem ou acesso à rede.

---

### Parte 10: Usando Modelos Customizados ou do Hugging Face

**Guia do laboratório:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilar modelos do Hugging Face para formato ONNX otimizado usando o construtor de modelos ONNX Runtime GenAI
- Compilação específica para hardware (CPU, GPU NVIDIA, DirectML, WebGPU) e quantização (int4, fp16, bf16)
- Criar arquivos de configuração de template de chat para Foundry Local
- Adicionar modelos compilados ao cache Foundry Local
- Rodar modelos customizados via CLI, REST API e SDK OpenAI
- Exemplo de referência: compilação completa do Qwen/Qwen3-0.6B

---

### Parte 11: Chamadas de Ferramenta com Modelos Locais

**Guia do laboratório:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Permitir que modelos locais chamem funções externas (chamada de ferramenta/função)
- Definir esquemas de ferramenta usando o formato OpenAI de chamada de função
- Gerenciar fluxo de conversa multi-turno com chamadas de ferramenta
- Executar chamadas de ferramenta localmente e retornar resultados ao modelo
- Escolher o modelo certo para cenários de chamada de ferramenta (Qwen 2.5, Phi-4-mini)
- Usar o `ChatClient` nativo do SDK para chamada de ferramentas (JavaScript)

**Exemplos de código:**

| Linguagem | Arquivo | Descrição |
|-----------|---------|-----------|
| Python | `python/foundry-local-tool-calling.py` | Chamada de ferramenta com ferramentas de clima/população |
| C# | `csharp/ToolCalling.cs` | Chamada de ferramenta com .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Chamada de ferramenta com ChatClient |

---

### Parte 12: Construindo uma UI Web para o Zava Creative Writer

**Guia do laboratório:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Adicionar uma interface baseada em navegador para o Zava Creative Writer
- Servir a UI compartilhada a partir de Python (FastAPI), JavaScript (Node.js HTTP) e C# (ASP.NET Core)
- Consumir NDJSON em streaming no navegador com Fetch API e ReadableStream
- Badges de status em tempo real do agente e streaming do texto do artigo ao vivo

**Código (UI compartilhada):**

| Arquivo | Descrição |
|---------|-----------|
| `zava-creative-writer-local/ui/index.html` | Layout da página |
| `zava-creative-writer-local/ui/style.css` | Estilo |
| `zava-creative-writer-local/ui/app.js` | Lógica do leitor de stream e atualização do DOM |

**Adições no backend:**

| Linguagem | Arquivo | Descrição |
|-----------|---------|-----------|
| Python | `zava-creative-writer-local/src/api/main.py` | Atualizado para servir UI estática |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Novo servidor HTTP envolvendo o orquestrador |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Novo projeto API minimal ASP.NET Core |

---

### Parte 13: Workshop Completo
**Guia do laboratório:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Resumo de tudo o que você construiu ao longo das 12 partes
- Ideias adicionais para estender suas aplicações
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

| Recurso | Link |
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

**Feliz construção! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido usando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos empenhemos para garantir a precisão, por favor, esteja ciente de que traduções automatizadas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->