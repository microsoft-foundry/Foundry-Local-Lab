# Instruções para Agentes de Codificação

Este ficheiro fornece contexto para agentes de codificação AI (GitHub Copilot, Copilot Workspace, Codex, etc.) a trabalhar neste repositório.

## Visão Geral do Projeto

Este é um **workshop prático** para construir aplicações AI com [Foundry Local](https://foundrylocal.ai) — um runtime leve que descarga, gere e serve modelos de linguagem inteiramente no dispositivo através de uma API compatível com OpenAI. O workshop inclui guias laboratoriais passo a passo e exemplos de código executáveis em Python, JavaScript e C#.

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

## Detalhes das Linguagens & Frameworks

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
- **Sistema de módulo:** Módulos ES (`.mjs` files, `"type": "module"`)
- **Versão mínima:** Node.js 18+
- **Execução:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Localização:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Ficheiros do projeto:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Principais pacotes:** `Microsoft.AI.Foundry.Local` (não-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset com QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Target:** .NET 9.0 (TFM condicional: `net9.0-windows10.0.26100` no Windows, `net9.0` noutros)
- **Execução:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Convenções de Codificação

### Geral
- Todos os exemplos de código são **exemplos autónomos de ficheiro único** — sem bibliotecas utilitárias ou abstrações partilhadas.
- Cada exemplo executa independentemente após instalar as suas próprias dependências.
- As chaves API estão sempre definidas para `"foundry-local"` — Foundry Local usa isto como placeholder.
- As URLs base usam `http://localhost:<port>/v1` — a porta é dinâmica e descoberta em runtime via SDK (`manager.urls[0]` em JS, `manager.endpoint` em Python).
- O Foundry Local SDK gere o arranque dos serviços e a descoberta dos endpoints; prefira padrões do SDK em vez de portas fixas.

### Python
- Use o SDK `openai` com `OpenAI(base_url=..., api_key="not-required")`.
- Use `FoundryLocalManager()` do `foundry_local` para ciclo de vida gerido pelo SDK.
- Streaming: itere sobre o objeto `stream` com `for chunk in stream:`.
- Sem anotações de tipo nos ficheiros de exemplo (manter os exemplos concisos para aprendizes do workshop).

### JavaScript
- Sintaxe de módulo ES: `import ... from "..."`.
- Use `OpenAI` de `"openai"` e `FoundryLocalManager` de `"foundry-local-sdk"`.
- Padrão de inicialização do SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- `await` de topo é usado em todo o lado.

### C#
- Nullable ativo, usings implícitos, .NET 9.
- Use `FoundryLocalManager.StartServiceAsync()` para ciclo de vida gerido pelo SDK.
- Streaming: `CompleteChatStreaming()` com `foreach (var update in completionUpdates)`.
- O ficheiro principal `csharp/Program.cs` é um despachante CLI que chama métodos estáticos `RunAsync()`.

### Chamada de Ferramentas
- Apenas certos modelos suportam chamadas a ferramentas: família **Qwen 2.5** (`qwen2.5-*`) e **Phi-4-mini** (`phi-4-mini`).
- Esquemas de ferramentas seguem o formato JSON da OpenAI para chamadas de função (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- A conversa usa um padrão multi-turno: utilizador → assistente (tool_calls) → ferramenta (results) → assistente (resposta final).
- O `tool_call_id` nas mensagens de resultado da ferramenta tem de corresponder ao `id` da chamada de ferramenta do modelo.
- O Python usa o SDK OpenAI diretamente; o JavaScript usa o `ChatClient` nativo do SDK (`model.createChatClient()`); o C# usa o SDK OpenAI com `ChatTool.CreateFunctionTool()`.

### ChatClient (Cliente SDK Nativo)
- JavaScript: `model.createChatClient()` retorna um `ChatClient` com `completeChat(messages, tools?)` e `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` retorna um `ChatClient` standard que pode ser usado sem importar o pacote NuGet OpenAI.
- Python não tem ChatClient nativo — use o SDK OpenAI com `manager.endpoint` e `manager.api_key`.
- **Importante:** `completeStreamingChat` do JavaScript usa um **padrão callback**, não iteração assíncrona.

### Modelos de Raciocínio
- `phi-4-mini-reasoning` envolve o seu pensamento em tags `<think>...</think>` antes da resposta final.
- Faça parsing das tags para separar o raciocínio da resposta quando necessário.

## Guias Laboratoriais

Os ficheiros dos laboratórios estão em `labs/` em Markdown. Seguem uma estrutura consistente:
- Imagem de cabeçalho com logo
- Título e chamada do objetivo
- Visão geral, Objetivos de Aprendizagem, Pré-requisitos
- Secções explicativas de conceitos com diagramas
- Exercícios numerados com blocos de código e saída esperada
- Tabela resumo, Pontos-chave, Leituras Complementares
- Link de navegação para a parte seguinte

Ao editar conteúdo dos labs:
- Mantenha o estilo de formatação Markdown e hierarquia de secções.
- Blocos de código devem especificar a linguagem (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Forneça variantes bash e PowerShell para comandos shell quando o SO importar.
- Use callouts `> **Note:**`, `> **Tip:**`, e `> **Troubleshooting:**`.
- Tabelas usam o formato de pipe `| Header | Header |`.

## Comandos de Compilação & Teste

| Ação | Comando |
|--------|---------|
| **Exemplos em Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
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

- O **Foundry Local CLI** deve estar instalado na máquina do desenvolvedor (`winget install Microsoft.FoundryLocal` ou `brew install foundrylocal`).
- O **serviço Foundry Local** corre localmente e expõe uma API REST compatível com OpenAI numa porta dinâmica.
- Não são necessários serviços em nuvem, chaves API nem subscrições Azure para correr quaisquer exemplos.
- A Parte 10 (modelos customizados) requer adicionalmente `onnxruntime-genai` e descarga pesos de modelos do Hugging Face.

## Ficheiros Que Não Devem Ser Comitados

O `.gitignore` deve excluir (e exclui na maioria dos casos):
- `.venv/` — ambientes virtuais Python
- `node_modules/` — dependências npm
- `models/` — saída dos modelos ONNX compilados (ficheiros binários grandes, gerados pela Parte 10)
- `cache_dir/` — cache de downloads Hugging Face
- `.olive-cache/` — directoria de trabalho Microsoft Olive
- `samples/audio/*.wav` — amostras áudio geradas (regeneradas via `python samples/audio/generate_samples.py`)
- Artefactos padrão de build Python (`__pycache__/`, `*.egg-info/`, `dist/`, etc.)

## Licença

MIT — veja `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, por favor tenha em conta que traduções automáticas podem conter erros ou imprecisões. O documento original no seu idioma nativo deve ser considerado a fonte autoritativa. Para informações críticas, é recomendada a tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas resultantes da utilização desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->