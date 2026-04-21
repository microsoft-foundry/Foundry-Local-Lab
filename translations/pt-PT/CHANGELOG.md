# Changelog — Workshop Local Foundry

Todas as alterações notáveis a este workshop estão documentadas abaixo.

---

## 2026-03-11 — Parte 12 & 13, UI Web, Reescrita do Whisper, Correção WinML/QNN, e Validação

### Adicionado
- **Parte 12: Construção de uma UI Web para o Zava Creative Writer** — novo guia de laboratório (`labs/part12-zava-ui.md`) com exercícios sobre streaming NDJSON, `ReadableStream` do browser, badges de estado de agente em tempo real, e streaming em tempo real do texto do artigo
- **Parte 13: Workshop Completo** — novo laboratório resumo (`labs/part13-workshop-complete.md`) com uma recapitulação das 12 partes, ideias adicionais, e links de recursos
- **Front-end da UI Zava:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — interface vanilla HTML/CSS/JS partilhada consumida por todos os três backends
- **Servidor HTTP JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — novo servidor HTTP estilo Express a envolver o orquestrador para acesso via browser
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` e `ZavaCreativeWriterWeb.csproj` — novo projeto de API minimalista a servir a UI e streaming NDJSON
- **Gerador de amostras áudio:** `samples/audio/generate_samples.py` — script TTS offline usando `pyttsx3` para gerar ficheiros WAV temáticos Zava para a Parte 9
- **Amostra áudio:** `samples/audio/zava-full-project-walkthrough.wav` — nova amostra áudio mais longa para testes de transcrição
- **Script de validação:** `validate-npu-workaround.ps1` — script PowerShell automatizado para validar a solução alternativa NPU/QNN em todos os exemplos C#
- **Diagramas Mermaid em SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Suporte WinML multiplataforma:** Todos os 3 ficheiros `.csproj` C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) usam agora TFM condicional e referências de pacote mutuamente exclusivas para suporte multiplataforma. Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (superset que inclui plugin EP QNN). Não-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK base). RID `win-arm64` codificado removido nos projetos Zava, substituído por deteção automática. Solução alternativa de dependência transitiva exclui ativos nativos de `Microsoft.ML.OnnxRuntime.Gpu.Linux` que tem referência incorreta para win-arm64. A solução try/catch NPU anterior foi removida dos 7 ficheiros C#.

### Alterado
- **Parte 9 (Whisper):** Reescrita maior — JavaScript usa agora `AudioClient` embutido no SDK (`model.createAudioClient()`) em vez de inferência manual ONNX Runtime; descrições de arquitetura, tabelas comparativas, e diagramas de pipeline atualizados para refletir abordagem JS/C# `AudioClient` vs abordagem Python ONNX Runtime
- **Parte 11:** Atualização dos links de navegação (apontam agora para Parte 12); adicionado diagramas SVG renderizados para fluxo e sequência de chamadas de ferramenta
- **Parte 10:** Navegação atualizada para redirecionar via Parte 12 em vez de terminar o workshop
- **Whisper Python (`foundry-local-whisper.py`):** Expandido com amostras áudio adicionais e melhoria no tratamento de erros
- **Whisper JavaScript (`foundry-local-whisper.mjs`):** Reescrito para usar `model.createAudioClient()` com `audioClient.transcribe()` em vez de sessões manuais ONNX Runtime
- **FastAPI Python (`zava-creative-writer-local/src/api/main.py`):** Atualizado para servir ficheiros UI estáticos em paralelo com a API
- **Console Zava C# (`zava-creative-writer-local/src/csharp/Program.cs`):** Removida solução alternativa NPU (agora gerida pelo pacote WinML)
- **README.md:** Adicionada secção da Parte 12 com tabelas de exemplos de código e adições ao backend; adicionada secção da Parte 13; objetivos de aprendizagem e estrutura do projeto atualizados
- **KNOWN-ISSUES.md:** Removido o problema resolvido #7 (Variante Modelo NPU C# SDK — agora gerida pelo pacote WinML). Re-numeração dos restantes problemas para #1–#6. Detalhes de ambiente atualizados com .NET SDK 10.0.104
- **AGENTS.md:** Atualizada árvore da estrutura do projeto com novas entradas `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); atualizadas principais bibliotecas C# e detalhes de TFM condicional
- **labs/part2-foundry-local-sdk.md:** Exemplo `.csproj` atualizado para mostrar padrão total multiplataforma com TFM condicional, referências de pacote mutuamente exclusivas e nota explicativa

### Validado
- Os 3 projetos C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) compilam com sucesso em Windows ARM64
- Exemplo Chat (`dotnet run chat`): modelo carrega como `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — variante NPU carrega diretamente sem fallback para CPU
- Exemplo Agente (`dotnet run agent`): executa do início ao fim com conversa multi-turno, código de saída 0
- Foundry Local CLI v0.8.117 e SDK v0.9.0 em .NET SDK 9.0.312

---

## 2026-03-11 — Correções de Código, Limpeza de Modelos, Diagramas Mermaid, e Validação

### Corrigido
- **Todos os 21 exemplos de código (7 Python, 7 JavaScript, 7 C#):** Adicionado `model.unload()` / `unload_model()` / `model.UnloadAsync()` para limpeza na saída para resolver avisos de fuga de memória OGA (Problema Conhecido #4)
- **csharp/WhisperTranscription.cs:** Substituída a frágil referência relativa `AppContext.BaseDirectory` por `FindSamplesDirectory()` que sobe diretórios para localizar `samples/audio` de forma fiável (Problema Conhecido #7)
- **csharp/csharp.csproj:** Substituído `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` codificado por fallback de deteção automática usando `$(NETCoreSdkRuntimeIdentifier)` para que `dotnet run` funcione em qualquer plataforma sem a flag `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Alterado
- **Parte 8:** Convertido o ciclo de iteração baseado em avaliação de diagrama ASCII para imagem SVG renderizada
- **Parte 10:** Convertido diagrama pipeline de compilação de setas ASCII para imagem SVG renderizada
- **Parte 11:** Convertidos diagramas de fluxo e sequência de chamadas de ferramenta para imagens SVG renderizadas
- **Parte 10:** Seção "Workshop Completo!" movida para Parte 11 (o laboratório final); substituída por link "Próximos Passos"
- **KNOWN-ISSUES.md:** Revalidação completa de todos os problemas contra CLI v0.8.117. Problemas resolvidos removidos: Fuga de memória OGA (limpeza adicionada), caminho Whisper (FindSamplesDirectory), inferência HTTP 500 sustentada (não reproduzível, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), limitações do tool_choice (agora funciona com `"required"` e direcionamento específico de função em qwen2.5-0.5b). Atualizado problema do Whisper JS — agora todos os ficheiros retornam saída vazia/binária (regressão da v0.9.x, severidade aumentada para Major). Atualizado RID #4 C# com solução alternativa de deteção automática e link [#497](https://github.com/microsoft/Foundry-Local/issues/497). 7 problemas em aberto permanecem.
- **javascript/foundry-local-whisper.mjs:** Corrigido nome de variável de limpeza (`whisperModel` → `model`)

### Validado
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — executados com sucesso com limpeza
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — executados com sucesso com limpeza
- C#: `dotnet build` com sucesso sem avisos nem erros (alvo net9.0)
- Todos os 7 ficheiros Python passam verificação de sintaxe `py_compile`
- Todos os 7 ficheiros JavaScript passam validação de sintaxe `node --check`

---

## 2026-03-10 — Parte 11: Chamadas de Ferramentas, Expansão da API do SDK, e Cobertura de Modelos

### Adicionado
- **Parte 11: Chamada de Ferramentas com Modelos Locais** — novo guia de laboratório (`labs/part11-tool-calling.md`) com 8 exercícios sobre esquemas de ferramentas, fluxo multi-turno, múltiplas chamadas de ferramentas, ferramentas customizadas, chamada de ferramentas com ChatClient, e `tool_choice`
- **Exemplo Python:** `python/foundry-local-tool-calling.py` — chamada de ferramentas `get_weather`/`get_population` usando SDK OpenAI
- **Exemplo JavaScript:** `javascript/foundry-local-tool-calling.mjs` — chamada de ferramentas usando o nativo `ChatClient` do SDK (`model.createChatClient()`)
- **Exemplo C#:** `csharp/ToolCalling.cs` — chamada de ferramentas usando `ChatTool.CreateFunctionTool()` com o SDK OpenAI C#
- **Parte 2, Exercício 7:** ChatClient nativo — `model.createChatClient()` (JS) e `model.GetChatClientAsync()` (C#) como alternativas ao SDK OpenAI
- **Parte 2, Exercício 8:** Variantes de modelos e seleção de hardware — `selectVariant()`, `variants`, tabela variante NPU (7 modelos)
- **Parte 2, Exercício 9:** Atualizações de modelos e refrescamento do catálogo — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Parte 2, Exercício 10:** Modelos de raciocínio — `phi-4-mini-reasoning` com exemplos de parsing da tag `<think>`
- **Parte 3, Exercício 4:** `createChatClient` como alternativa ao SDK OpenAI, com documentação do padrão de callback streaming
- **AGENTS.md:** Adicionadas convenções de codificação para Chamada de Ferramentas, ChatClient, e Modelos de Raciocínio

### Alterado
- **Parte 1:** Catálogo de modelos expandido — adicionados phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Parte 2:** Tabelas de referência da API expandidas — adicionadas `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Parte 2:** Exercícios renumerados de 7-9 → 10-13 para dar espaço a novos exercícios
- **Parte 3:** Tabela de principais conclusões atualizada para incluir ChatClient nativo
- **README.md:** Adicionada secção da Parte 11 com tabela de exemplos de código; objetivo de aprendizagem #11 adicionado; árvore da estrutura do projeto atualizada
- **csharp/Program.cs:** Adicionado caso `toolcall` ao router da CLI e texto de ajuda atualizado

---

## 2026-03-09 — Atualização SDK v0.9.0, Inglês Britânico, e Passagem de Validação

### Alterado
- **Todos os exemplos de código (Python, JavaScript, C#):** Atualizados para a API Foundry Local SDK v0.9.0 — corrigido `await catalog.getModel()` (faltava `await`), atualizados padrões de inicialização do `FoundryLocalManager`, corrigida descoberta de endpoint
- **Todos os guias de laboratório (Partes 1-10):** Convertidos para Inglês britânico (colour, catalogue, optimised, etc.)
- **Todos os guias de laboratório:** Exemplos de código SDK atualizados para a superfície da API v0.9.0
- **Todos os guias de laboratório:** Atualizadas tabelas de referência da API e blocos de código de exercícios
- **Correção crítica JavaScript:** Adicionado `await` faltante em `catalog.getModel()` — retornava uma `Promise` não um objeto `Model`, causando falhas silenciosas a jusante

### Validado
- Todos os exemplos Python executam com sucesso contra o serviço Foundry Local
- Todos os exemplos JavaScript executam com sucesso (Node.js 18+)
- Projeto C# compila e executa em .NET 9.0 (compatibilidade futura a partir do assembly do SDK net8.0)
- 29 ficheiros modificados e validados em todo o workshop

---

## Índice de Ficheiros

| Ficheiro | Última Atualização | Descrição |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Catálogo de modelos expandido |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Novos exercícios 7-10, tabelas API expandidas |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Novo Exercício 4 (ChatClient), conclusões atualizadas |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Inglês Britânico |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Inglês Britânico |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Inglês Britânico |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Inglês Britânico |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Diagrama Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Inglês Britânico |
| `labs/part10-custom-models.md` | 2026-03-11 | Diagrama Mermaid, Workshop Completo movido para a Parte 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Novo laboratório, diagramas Mermaid, seção Workshop Completo |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Novo: exemplo de chamada de ferramenta |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Novo: exemplo de chamada de ferramenta |
| `csharp/ToolCalling.cs` | 2026-03-10 | Novo: exemplo de chamada de ferramenta |
| `csharp/Program.cs` | 2026-03-10 | Adicionado comando CLI `toolcall` |
| `README.md` | 2026-03-10 | Parte 11, estrutura do projeto |
| `AGENTS.md` | 2026-03-10 | Chamada de ferramenta + Convenções ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Issue #7 resolvida removida, 6 issues em aberto permanecem |
| `csharp/csharp.csproj` | 2026-03-11 | TFM multiplataforma, referências condicionais WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM multiplataforma, deteção automática de RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM multiplataforma, deteção automática de RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Removida solução alternativa try/catch NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Removida solução alternativa try/catch NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Removida solução alternativa try/catch NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Removida solução alternativa try/catch NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Removida solução alternativa try/catch NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Exemplo de .csproj multiplataforma |
| `AGENTS.md` | 2026-03-11 | Atualizado detalhes dos pacotes C# e TFM |
| `CHANGELOG.md` | 2026-03-11 | Este ficheiro |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos por garantir a precisão, esteja ciente de que traduções automatizadas podem conter erros ou imprecisões. O documento original na sua língua nativa deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se a tradução profissional feita por humanos. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações erradas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->