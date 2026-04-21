# Instruções para Agentes de Codificação

Este arquivo fornece contexto para agentes de codificação de IA (GitHub Copilot, Copilot Workspace, Codex, etc.) que trabalham neste repositório.

## Visão Geral do Projeto

Este é um **workshop prático** para construir aplicações de IA com o [Foundry Local](https://foundrylocal.ai) — um runtime leve que faz download, gerencia e serve modelos de linguagem inteiramente no dispositivo via uma API compatível com OpenAI. O workshop inclui guias de laboratório passo a passo e exemplos de código executáveis em Python, JavaScript e C#.

## Estrutura do Repositório

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Detalhes de Linguagem e Framework

### Python
- **Localização:** `python/`, `zava-creative-writer-local/src/api/`
- **Dependências:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Principais pacotes:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Versão mínima:** Python 3.9+
- **Execução:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Localização:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Dependências:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Principais pacotes:** `foundry-local-sdk`, `openai`
- **Sistema de módulos:** Módulos ES (`.mjs` arquivos, `"type": "module"`)
- **Versão mínima:** Node.js 18+
- **Execução:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Localização:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Arquivos do projeto:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Principais pacotes:** `Microsoft.AI.Foundry.Local` (não Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superconjunto com QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Alvo:** .NET 9.0 (TFM condicional: `net9.0-windows10.0.26100` no Windows, `net9.0` em outros)
- **Execução:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Convenções de Codificação

### Geral
- Todos os exemplos de código são **exemplos autossuficientes em arquivo único** — sem bibliotecas utilitárias compartilhadas ou abstrações.
- Cada exemplo roda independentemente após instalar suas próprias dependências.
- As chaves de API são sempre configuradas para `"foundry-local"` — Foundry Local usa isso como placeholder.
- URLs base usam `http://localhost:<port>/v1` — a porta é dinâmica e descoberta em tempo de execução via SDK (`manager.urls[0]` no JS, `manager.endpoint` no Python).
- O Foundry Local SDK gerencia a inicialização do serviço e descoberta do endpoint; prefira padrões do SDK ao invés de portas fixas.

### Python
- Use o SDK `openai` com `OpenAI(base_url=..., api_key="not-required")`.
- Use `FoundryLocalManager()` de `foundry_local` para ciclo de vida gerenciado pelo SDK.
- Streaming: itere sobre o objeto `stream` com `for chunk in stream:`.
- Sem anotações de tipo nos arquivos de exemplo (mantenha os exemplos concisos para aprendizes do workshop).

### JavaScript
- Sintaxe de módulo ES: `import ... from "..."`.
- Use `OpenAI` de `"openai"` e `FoundryLocalManager` de `"foundry-local-sdk"`.
- Padrão de inicialização do SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- `await` de topo é usado amplamente.

### C#
- Nullable ativado, usings implícitos, .NET 9.
- Use `FoundryLocalManager.StartServiceAsync()` para ciclo de vida gerenciado pelo SDK.
- Streaming: `CompleteChatStreaming()` com `foreach (var update in completionUpdates)`.
- O arquivo principal `csharp/Program.cs` é um roteador CLI que despacha para métodos estáticos `RunAsync()`.

### Chamada de Ferramentas
- Apenas certos modelos suportam chamada de ferramentas: família **Qwen 2.5** (`qwen2.5-*`) e **Phi-4-mini** (`phi-4-mini`).
- Esquemas de ferramentas seguem o formato JSON de chamadas de função da OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- A conversação usa um padrão multi-turno: usuário → assistente (tool_calls) → ferramenta (resultados) → assistente (resposta final).
- O `tool_call_id` nas mensagens de resultado da ferramenta deve corresponder ao `id` da chamada de ferramenta do modelo.
- Python usa o SDK OpenAI diretamente; JavaScript usa o `ChatClient` nativo do SDK (`model.createChatClient()`); C# usa o SDK OpenAI com `ChatTool.CreateFunctionTool()`.

### ChatClient (Cliente Nativo do SDK)
- JavaScript: `model.createChatClient()` retorna um `ChatClient` com `completeChat(messages, tools?)` e `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` retorna um `ChatClient` padrão que pode ser usado sem importar o pacote NuGet do OpenAI.
- Python não tem um ChatClient nativo — use o SDK OpenAI com `manager.endpoint` e `manager.api_key`.
- **Importante:** `completeStreamingChat` em JavaScript usa um **padrão callback**, não iteração assíncrona.

### Modelos de Raciocínio
- `phi-4-mini-reasoning` envolve seu raciocínio em tags `<think>...</think>` antes da resposta final.
- Analise essas tags para separar raciocínio da resposta quando necessário.

## Guias de Laboratório

Arquivos de laboratório estão em `labs/` como Markdown. Seguem uma estrutura consistente:
- Imagem de cabeçalho com logo
- Título e chamada de objetivo
- Visão geral, Objetivos de Aprendizagem, Pré-requisitos
- Seções explicativas de conceitos com diagramas
- Exercícios numerados com blocos de código e saída esperada
- Tabela resumo, Principais conclusões, Leitura complementar
- Link de navegação para a próxima parte

Ao editar conteúdo do laboratório:
- Mantenha o estilo de formatação Markdown existente e hierarquia das seções.
- Blocos de código devem especificar a linguagem (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Forneça variantes bash e PowerShell para comandos shell quando o SO importar.
- Use estilos de chamada `> **Note:**`, `> **Tip:**` e `> **Troubleshooting:**`.
- Tabelas usam o formato com pipes `| Header | Header |`.

## Comandos de Build & Teste

| Ação | Comando |
|--------|---------|
| **Exemplos Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Exemplos JS** | `cd javascript && npm install && node <script>.mjs` |
| **Exemplos C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Gerar diagramas** | `npx mmdc -i <input>.mmd -o <output>.svg` (requer `npm install` global) |

## Dependências Externas

- **Foundry Local CLI** deve estar instalado na máquina do desenvolvedor (`winget install Microsoft.FoundryLocal` ou `brew install foundrylocal`).
- **Serviço Foundry Local** roda localmente e expõe uma API REST compatível com OpenAI em uma porta dinâmica.
- Nenhum serviço em nuvem, chave API ou assinatura Azure é necessária para rodar qualquer exemplo.
- A Parte 10 (modelos customizados) requer adicionalmente `onnxruntime-genai` e faz download dos pesos de modelo do Hugging Face.

## Arquivos Que Não Devem Ser Comitados

O `.gitignore` deve excluir (e exclui na maior parte):
- `.venv/` — ambientes virtuais Python
- `node_modules/` — dependências npm
- `models/` — saída de modelos ONNX compilados (arquivos binários grandes, gerados pela Parte 10)
- `cache_dir/` — cache de download de modelo do Hugging Face
- `.olive-cache/` — diretório de trabalho Microsoft Olive
- `samples/audio/*.wav` — amostras de áudio geradas (regeneradas via `python samples/audio/generate_samples.py`)
- Artefatos padrão de build Python (`__pycache__/`, `*.egg-info/`, `dist/`, etc.)

## Licença

MIT — ver `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se tradução profissional feita por humanos. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->