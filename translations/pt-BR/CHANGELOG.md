# Changelog — Foundry Local Workshop

Todas as mudanças notáveis deste workshop estão documentadas abaixo.

---

## 2026-03-11 — Partes 12 & 13, UI Web, Reescrita do Whisper, Correção WinML/QNN e Validação

### Adicionado
- **Parte 12: Construindo uma UI Web para o Escritor Criativo Zava** — novo guia de laboratório (`labs/part12-zava-ui.md`) com exercícios cobrindo streaming NDJSON, `ReadableStream` no navegador, distintivos de status ao vivo do agente e streaming de texto do artigo em tempo real
- **Parte 13: Workshop Completo** — novo laboratório resumido (`labs/part13-workshop-complete.md`) com recapitulação de todas as 12 partes, novas ideias e links de recursos
- **Interface front end Zava UI:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — interface de navegador vanilla HTML/CSS/JS compartilhada consumida pelos três backends
- **Servidor HTTP JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — novo servidor HTTP estilo Express encapsulando o orquestrador para acesso no navegador
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` e `ZavaCreativeWriterWeb.csproj` — novo projeto API minimal responsável pela UI e streaming NDJSON
- **Gerador de amostras de áudio:** `samples/audio/generate_samples.py` — script TTS offline usando `pyttsx3` para gerar arquivos WAV temáticos Zava para a Parte 9
- **Amostra de áudio:** `samples/audio/zava-full-project-walkthrough.wav` — nova amostra de áudio mais longa para testes de transcrição
- **Script de validação:** `validate-npu-workaround.ps1` — script PowerShell automatizado para validar o workaround NPU/QNN em todas as amostras C#
- **Diagramas Mermaid em SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Suporte multiplataforma WinML:** Todos os 3 arquivos `.csproj` C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) agora usam TFM condicional e referências de pacotes mutuamente exclusivas para suporte multiplataforma. No Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (superconjunto que inclui plugin QNN EP). No não-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK base). O RID hardcoded `win-arm64` nos projetos Zava foi substituído por detecção automática. Uma solução para dependência transitiva exclui ativos nativos do `Microsoft.ML.OnnxRuntime.Gpu.Linux`, que possui uma referência win-arm64 quebrada. O workaround NPU try/catch anterior foi removido dos 7 arquivos C#.

### Alterado
- **Parte 9 (Whisper):** Reescrita significativa — o JavaScript agora usa o `AudioClient` embutido do SDK (`model.createAudioClient()`) ao invés da inferência manual com ONNX Runtime; descrições de arquitetura, tabelas comparativas e diagramas de pipelines atualizados para refletir a abordagem JS/C# `AudioClient` versus ONNX Runtime em Python
- **Parte 11:** Links de navegação atualizados (agora apontam para a Parte 12); adicionados diagramas SVG renderizados para fluxo de chamadas de ferramentas e sequência
- **Parte 10:** Navegação atualizada para redirecionar via Parte 12 ao invés de encerrar o workshop
- **Whisper Python (`foundry-local-whisper.py`):** Expandido com amostras de áudio adicionais e tratamento de erros melhorado
- **Whisper JavaScript (`foundry-local-whisper.mjs`):** Reescrito para usar `model.createAudioClient()` com `audioClient.transcribe()` no lugar das sessões ONNX Runtime manuais
- **FastAPI Python (`zava-creative-writer-local/src/api/main.py`):** Atualizado para servir arquivos UI estáticos junto com a API
- **Console C# Zava (`zava-creative-writer-local/src/csharp/Program.cs`):** Removido workaround NPU (agora tratado pelo pacote WinML)
- **README.md:** Adicionada seção Parte 12 com tabelas de exemplos de código e adições no backend; adicionada seção Parte 13; objetivos de aprendizado e estrutura do projeto atualizados
- **KNOWN-ISSUES.md:** Removida a Issue #7 resolvida (Variantes do Modelo NPU no SDK C# — agora tratada pelo pacote WinML). Reenumeradas as questões restantes para #1–#6. Detalhes de ambiente atualizados com .NET SDK 10.0.104
- **AGENTS.md:** Estrutura do projeto atualizada com novas entradas `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); atualizadas as principais referências de pacotes C# e detalhes do TFM condicional
- **labs/part2-foundry-local-sdk.md:** Exemplo `.csproj` atualizado para mostrar padrão multiplataforma completo com TFM condicional, referências de pacote mutuamente exclusivas e nota explicativa

### Validado
- Todos os 3 projetos C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) compilam com sucesso no Windows ARM64
- Exemplo chat (`dotnet run chat`): modelo carrega como `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — variante NPU carrega diretamente sem fallback para CPU
- Exemplo agente (`dotnet run agent`): executa fim a fim com conversa multi-turno, código de saída 0
- Foundry Local CLI v0.8.117 e SDK v0.9.0 no .NET SDK 9.0.312

---

## 2026-03-11 — Correções de Código, Limpeza de Modelos, Diagramas Mermaid e Validação

### Corrigido
- **Todos os 21 exemplos de código (7 Python, 7 JavaScript, 7 C#):** Adicionado `model.unload()` / `unload_model()` / `model.UnloadAsync()` na saída para resolver avisos de vazamento de memória OGA (Issue Conhecida #4)
- **csharp/WhisperTranscription.cs:** Substituído caminho frágil relativo `AppContext.BaseDirectory` por `FindSamplesDirectory()` que percorre diretórios para localizar `samples/audio` de forma confiável (Issue Conhecida #7)
- **csharp/csharp.csproj:** Substituído `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` hardcoded por fallback de detecção automática usando `$(NETCoreSdkRuntimeIdentifier)` para que `dotnet run` funcione em qualquer plataforma sem a flag `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Alterado
- **Parte 8:** Convertido loop iterativo dirigido por avaliação de diagrama em caixa ASCII para imagem SVG renderizada
- **Parte 10:** Convertido diagrama de pipeline de compilação de setas ASCII para imagem SVG renderizada
- **Parte 11:** Convertidos fluxos de chamada de ferramenta e diagramas de sequência para imagens SVG renderizadas
- **Parte 10:** Movida seção "Workshop Completo!" para Parte 11 (o laboratório final); substituída por link "Próximos Passos"
- **KNOWN-ISSUES.md:** Revalidação completa de todas as questões contra CLI v0.8.117. Removidas as resolvidas: Vazamento de Memória OGA (limpeza adicionada), caminho Whisper (FindSamplesDirectory), inferência HTTP 500 sustentada (não reproduzível, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), limitações `tool_choice` (agora funciona com `"required"` e direcionamento específico em qwen2.5-0.5b). Atualizado problema JS Whisper — agora todos os arquivos retornam saída vazia/binária (regressão da v0.9.x, gravidade elevada para Major). Atualizado #4 RID C# com workaround de detecção automática e link [#497](https://github.com/microsoft/Foundry-Local/issues/497). Restam 7 issues abertas.
- **javascript/foundry-local-whisper.mjs:** Nome da variável de limpeza corrigido (`whisperModel` → `model`)

### Validado
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — executam com sucesso com limpeza
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — executam com sucesso com limpeza
- C#: `dotnet build` funciona com 0 avisos, 0 erros (target net9.0)
- Todos os 7 arquivos Python passam na verificação de sintaxe `py_compile`
- Todos os 7 arquivos JavaScript passam na validação de sintaxe `node --check`

---

## 2026-03-10 — Parte 11: Chamada de Ferramentas, Expansão da API SDK e Cobertura de Modelos

### Adicionado
- **Parte 11: Chamada de Ferramentas com Modelos Locais** — novo guia de laboratório (`labs/part11-tool-calling.md`) com 8 exercícios cobrindo esquemas de ferramentas, fluxo multi-turno, múltiplas chamadas de ferramentas, ferramentas customizadas, chamada de ferramentas com ChatClient e `tool_choice`
- **Exemplo Python:** `python/foundry-local-tool-calling.py` — chamada de ferramentas com `get_weather`/`get_population` usando SDK OpenAI
- **Exemplo JavaScript:** `javascript/foundry-local-tool-calling.mjs` — chamada de ferramentas usando o `ChatClient` nativo do SDK (`model.createChatClient()`)
- **Exemplo C#:** `csharp/ToolCalling.cs` — chamada de ferramentas usando `ChatTool.CreateFunctionTool()` com SDK OpenAI C#
- **Parte 2, Exercício 7:** `ChatClient` nativo — `model.createChatClient()` (JS) e `model.GetChatClientAsync()` (C#) como alternativas ao SDK OpenAI
- **Parte 2, Exercício 8:** Variantes de modelo e seleção de hardware — `selectVariant()`, `variants`, tabela de variante NPU (7 modelos)
- **Parte 2, Exercício 9:** Atualizações de modelos e atualização do catálogo — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Parte 2, Exercício 10:** Modelos de raciocínio — `phi-4-mini-reasoning` com exemplos de parsing da tag `<think>`
- **Parte 3, Exercício 4:** `createChatClient` como alternativa ao SDK OpenAI, com documentação de padrão callback de streaming
- **AGENTS.md:** Convenções de codificação para Chamada de Ferramentas, ChatClient e Modelos de Raciocínio adicionadas

### Alterado
- **Parte 1:** Catálogo de modelos expandido — adicionado phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Parte 2:** Tabelas de referência API expandidas — adicionados `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Parte 2:** Reenumerados exercícios 7-9 para 10-13 para acomodar novos exercícios
- **Parte 3:** Tabela de principais aprendizados atualizada para incluir ChatClient nativo
- **README.md:** Adicionada seção Parte 11 com tabela de exemplos de código; objetivo de aprendizado #11 adicionado; árvore de estrutura do projeto atualizada
- **csharp/Program.cs:** Caso `toolcall` adicionado ao roteador CLI e texto de ajuda atualizado

---

## 2026-03-09 — Atualização SDK v0.9.0, Inglês Britânico e Validação

### Alterado
- **Todos exemplos de código (Python, JavaScript, C#):** Atualizados para API Foundry Local SDK v0.9.0 — corrigido `await catalog.getModel()` (faltava `await`), atualizados padrões de inicialização `FoundryLocalManager`, correção na descoberta de endpoints
- **Todos guias de laboratório (Partes 1-10):** Convertidos para Inglês Britânico (colour, catalogue, optimised, etc.)
- **Todos guias de laboratório:** Exemplos de código SDK atualizados para a API v0.9.0
- **Todos guias de laboratório:** Tabelas de referência API e blocos de código dos exercícios atualizados
- **Correção crítica JavaScript:** Adicionado `await` faltando em `catalog.getModel()` — retornava um `Promise` e não um objeto `Model`, causando falhas silenciosas depois

### Validado
- Todos os exemplos Python executam com sucesso no serviço Foundry Local
- Todos os exemplos JavaScript executam com sucesso (Node.js 18+)
- Projeto C# compila e executa no .NET 9.0 (compatível para frente a partir do SDK net8.0)
- 29 arquivos modificados e validados ao longo do workshop

---

## Índice de Arquivos

| Arquivo | Última Atualização | Descrição |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Catálogo de modelos expandido |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Novos exercícios 7-10, tabelas API expandidas |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Novo Exercício 4 (ChatClient), principais aprendizados atualizados |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Inglês Britânico |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Inglês Britânico |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Inglês Britânico |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Inglês Britânico |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Diagrama Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Inglês Britânico |
| `labs/part10-custom-models.md` | 2026-03-11 | Diagrama Mermaid, Workshop Completo movido para a Parte 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Novo laboratório, diagramas Mermaid, seção Workshop Completo |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Novo: exemplo de chamadas de ferramenta |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Novo: exemplo de chamadas de ferramenta |
| `csharp/ToolCalling.cs` | 2026-03-10 | Novo: exemplo de chamadas de ferramenta |
| `csharp/Program.cs` | 2026-03-10 | Adicionado comando CLI `toolcall` |
| `README.md` | 2026-03-10 | Parte 11, estrutura do projeto |
| `AGENTS.md` | 2026-03-10 | Chamadas de ferramenta + convenções ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Issue #7 resolvida removida, 6 issues abertas permanecem |
| `csharp/csharp.csproj` | 2026-03-11 | TFM multiplataforma, referências condicionais WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM multiplataforma, detecção automática de RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM multiplataforma, detecção automática de RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Removida solução de contorno try/catch NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Removida solução de contorno try/catch NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Removida solução de contorno try/catch NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Removida solução de contorno try/catch NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Removida solução de contorno try/catch NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Exemplo .csproj multiplataforma |
| `AGENTS.md` | 2026-03-11 | Atualizados pacotes C# e detalhes do TFM |
| `CHANGELOG.md` | 2026-03-11 | Este arquivo |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido usando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos para garantir a precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte oficial. Para informações críticas, recomenda-se a tradução profissional por humanos. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->