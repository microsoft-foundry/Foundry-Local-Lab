![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 7: Zava Creative Writer - Aplicação Final

> **Objetivo:** Explorar uma aplicação multi-agente estilo produção onde quatro agentes especializados colaboram para produzir artigos com qualidade de revista para a Zava Retail DIY - a funcionar inteiramente no seu dispositivo com o Foundry Local.

Este é o **laboratório final** do workshop. Reúne tudo o que aprendeu - integração do SDK (Parte 3), recuperação a partir de dados locais (Parte 4), personas de agentes (Parte 5), e orquestração multi-agente (Parte 6) - numa aplicação completa disponível em **Python**, **JavaScript** e **C#**.

---

## O que vai explorar

| Conceito | Onde na Zava Writer |
|---------|---------------------|
| Carregamento do modelo em 4 passos | Módulo de configuração partilhada inicia o Foundry Local |
| Recuperação estilo RAG | Agente de produto pesquisa num catálogo local |
| Especialização de Agentes | 4 agentes com prompts de sistema distintos |
| Saída em fluxo | Escritor devolve tokens em tempo real |
| Entregas estruturadas | Researcher → JSON, Editor → decisão JSON |
| Ciclos de feedback | Editor pode disparar nova execução (máx. 2 tentativas) |

---

## Arquitectura

O Zava Creative Writer usa um **pipeline sequencial com feedback conduzido por avaliador**. As três implementações linguísticas seguem a mesma arquitetura:

![Zava Creative Writer Architecture](../../../translated_images/pt-PT/part7-zava-architecture.6abd0d700b667bf6.webp)

### Os Quatro Agentes

| Agente | Entrada | Saída | Propósito |
|--------|---------|-------|-----------|
| **Researcher** | Tema + feedback opcional | `{"web": [{url, name, description}, ...]}` | Recolhe pesquisa de fundo via LLM |
| **Product Search** | String contexto do produto | Lista de produtos correspondentes | Queries geradas por LLM + pesquisa por palavras-chave no catálogo local |
| **Writer** | Pesquisa + produtos + tarefa + feedback | Texto do artigo em streaming (separado por `---`) | Redige um artigo de qualidade de revista em tempo real |
| **Editor** | Artigo + auto-feedback do escritor | `{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}` | Revisa qualidade, dispara nova tentativa se necessário |

### Fluxo do Pipeline

1. **Researcher** recebe o tema e gera notas de pesquisa estruturadas (JSON)
2. **Product Search** consulta o catálogo local usando termos de busca gerados por LLM
3. **Writer** combina pesquisa + produtos + tarefa num artigo em streaming, adicionando auto-feedback após o separador `---`
4. **Editor** revisa o artigo e devolve um veredicto em JSON:
   - `"accept"` → pipeline termina
   - `"revise"` → feedback é enviado de volta para Researcher e Writer (máx. 2 tentativas)

---

## Pré-requisitos

- Completar [Parte 6: Fluxos de Trabalho Multi-Agente](part6-multi-agent-workflows.md)
- Foundry Local CLI instalado e modelo `phi-3.5-mini` descarregado

---

## Exercícios

### Exercício 1 - Execute o Zava Creative Writer

Escolha a sua linguagem e execute a aplicação:

<details>
<summary><strong>🐍 Python - Serviço Web FastAPI</strong></summary>

A versão Python corre como um **serviço web** com uma API REST, demonstrando como construir um backend de produção.

**Configurar:**
```bash
cd zava-creative-writer-local/src/api
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Executar:**
```bash
uvicorn main:app --reload
```

**Testar:**
```bash
curl -X POST http://localhost:8000/api/article \
  -H "Content-Type: application/json" \
  -d '{
    "research": "DIY home improvement trends",
    "products": "power tools and paints",
    "assignment": "Write an article about weekend renovation projects for DIY enthusiasts"
  }'
```

A resposta é transmitida em mensagens JSON delimitadas por nova linha que mostram o progresso de cada agente.

</details>

<details>
<summary><strong>📦 JavaScript - CLI Node.js</strong></summary>

A versão JavaScript corre como uma **aplicação CLI**, imprimindo o progresso dos agentes e o artigo diretamente no terminal.

**Configurar:**
```bash
cd zava-creative-writer-local/src/javascript
npm install
```

**Executar:**
```bash
node main.mjs
```

Verá:
1. Carregamento do modelo Foundry Local (com barra de progresso se estiver a descarregar)
2. Cada agente a executar em sequência com mensagens de estado
3. O artigo transmitido para o console em tempo real
4. Decisão de aceitar/revisar do editor

</details>

<details>
<summary><strong>💜 C# - Aplicação Console .NET</strong></summary>

A versão C# corre como uma **aplicação consola .NET** com o mesmo pipeline e saída em streaming.

**Configurar:**
```bash
cd zava-creative-writer-local/src/csharp
dotnet restore
```

**Executar:**
```bash
dotnet run
```

Mesmo padrão de saída que a versão JavaScript - mensagens de estado dos agentes, artigo transmitido, e veredicto do editor.

</details>

---

### Exercício 2 - Estude a Estrutura do Código

Cada implementação linguística tem os mesmos componentes lógicos. Compare as estruturas:

**Python** (`src/api/`):
| Ficheiro | Propósito |
|----------|-----------|
| `foundry_config.py` | Gestor, modelo e cliente partilhados do Foundry Local (inicialização em 4 passos) |
| `orchestrator.py` | Coordenação do pipeline com ciclo de feedback |
| `main.py` | Endpoints FastAPI (`POST /api/article`) |
| `agents/researcher/researcher.py` | Pesquisa LLM com saída JSON |
| `agents/product/product.py` | Querys LLM + pesquisa por palavra-chave |
| `agents/writer/writer.py` | Geração do artigo em streaming |
| `agents/editor/editor.py` | Decisão JSON aceitar/revisar |

**JavaScript** (`src/javascript/`):
| Ficheiro | Propósito |
|----------|-----------|
| `foundryConfig.mjs` | Configuração partilhada Foundry Local (inicialização em 4 passos com barra de progresso) |
| `main.mjs` | Orquestrador + ponto de entrada CLI |
| `researcher.mjs` | Agente de investigação baseado em LLM |
| `product.mjs` | Geração de queries LLM + pesquisa de palavras-chave |
| `writer.mjs` | Geração do artigo em streaming (gerador async) |
| `editor.mjs` | Decisão JSON aceitar/revisar |
| `products.mjs` | Dados do catálogo de produtos |

**C#** (`src/csharp/`):
| Ficheiro | Propósito |
|----------|-----------|
| `Program.cs` | Pipeline completo: carregamento de modelo, agentes, orquestrador, ciclo de feedback |
| `ZavaCreativeWriter.csproj` | Projeto .NET 9 com Foundry Local + pacotes OpenAI |

> **Nota de design:** Python separa cada agente em ficheiro/diretório próprio (bom para equipas maiores). JavaScript usa um módulo por agente (bom para projectos médios). C# mantém tudo num único ficheiro com funções locais (bom para exemplos autónomos). Em produção, escolha o padrão que melhor se adequa à equipa.

---

### Exercício 3 - Acompanhe a Configuração Partilhada

Cada agente no pipeline partilha um único cliente do modelo Foundry Local. Estude como isso é configurado em cada linguagem:

<details>
<summary><strong>🐍 Python - foundry_config.py</strong></summary>

```python
from foundry_local import FoundryLocalManager

MODEL_ALIAS = "phi-3.5-mini"

# Passo 1: Criar gestor e iniciar o serviço Foundry Local
manager = FoundryLocalManager()
manager.start_service()

# Passo 2: Verificar se o modelo já está descarregado
cached = manager.list_cached_models()
catalog_info = manager.get_model_info(MODEL_ALIAS)
is_cached = any(m.id == catalog_info.id for m in cached) if catalog_info else False

if not is_cached:
    manager.download_model(MODEL_ALIAS)

# Passo 3: Carregar o modelo para a memória
manager.load_model(MODEL_ALIAS)
model_id = manager.get_model_info(MODEL_ALIAS).id

# Cliente OpenAI partilhado
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
```

Todos os agentes importam `from foundry_config import client, model_id`.

</details>

<details>
<summary><strong>📦 JavaScript - foundryConfig.mjs</strong></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import { OpenAI } from "openai";

FoundryLocalManager.create({ appName: "ZavaCreativeWriter" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Verificar cache → descarregar → carregar (novo padrão do SDK)
const catalog = manager.catalog;
const model = await catalog.getModel(MODEL_ALIAS);
if (!model.isCached) {
  console.log(`Downloading model: ${MODEL_ALIAS}...`);
  await model.download();
}
await model.load();

const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const modelId = model.id;
export { client, modelId };
```

Todos os agentes importam `{ client, modelId } from "./foundryConfig.mjs"`.

</details>

<details>
<summary><strong>💜 C# - topo de Program.cs</strong></summary>

```csharp
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "ZavaCreativeWriter",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

var catalog = await manager.GetCatalogAsync(default);
var catalogModel = await catalog.GetModelAsync(alias, default);
var isCached = await catalogModel.IsCachedAsync(default);
if (!isCached)
    await catalogModel.DownloadAsync(null, default);

await catalogModel.LoadAsync(default);
var key = new ApiKeyCredential("foundry-local");
var chatClient = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls[0] + "/v1")
}).GetChatClient(catalogModel.Id);
```

O `chatClient` é passado para todas as funções dos agentes no mesmo ficheiro.

</details>

> **Padrão chave:** O padrão de carregamento do modelo (iniciar serviço → verificar cache → descarregar → carregar) garante que o utilizador vê progresso claro e o modelo é descarregado apenas uma vez. É uma boa prática para qualquer aplicação Foundry Local.

---

### Exercício 4 - Compreenda o Ciclo de Feedback

O ciclo de feedback é o que torna este pipeline "inteligente" - o Editor pode enviar trabalho de volta para revisão. Acompanhe a lógica:

```
Orchestrator:
  1. researcher.research(topic, "No Feedback")    ← first pass
  2. product.findProducts(productContext)
  3. writer.write(research, products, assignment)  ← streams article
  4. Split article at "---" → article + writerFeedback
  5. editor.edit(article, writerFeedback)

  WHILE editor says "revise" AND retryCount < 2:
    6. researcher.research(topic, editor.researchFeedback)  ← refined
    7. writer.write(research, products, editor.editorFeedback)
    8. editor.edit(newArticle, newWriterFeedback)
    9. retryCount++
```

**Questões para considerar:**
- Porque é que o limite de tentativas está definido para 2? O que acontece se aumentar?
- Porque é que o researcher recebe `researchFeedback` mas o writer recebe `editorFeedback`?
- O que aconteceria se o editor dissesse sempre "revise"?

---

### Exercício 5 - Modifique um Agente

Tente alterar o comportamento de um agente e observe como isso afeta o pipeline:

| Modificação | O que mudar |
|-------------|-------------|
| **Editor mais rigoroso** | Alterar o prompt de sistema do editor para pedir sempre pelo menos uma revisão |
| **Artigos mais longos** | Mudar o prompt do writer de "800-1000 palavras" para "1500-2000 palavras" |
| **Produtos diferentes** | Adicionar ou modificar produtos no catálogo de produtos |
| **Novo tópico de investigação** | Alterar o `researchContext` padrão para um assunto diferente |
| **Researcher só em JSON** | Fazer o researcher devolver 10 itens em vez de 3-5 |

> **Dica:** Como as três linguagens implementam a mesma arquitetura, pode fazer a mesma modificação na linguagem com que se sente mais confortável.

---

### Exercício 6 - Adicione um Quinto Agente

Estenda o pipeline com um novo agente. Algumas ideias:

| Agente | Onde no pipeline | Propósito |
|--------|------------------|-----------|
| **Fact-Checker** | Depois do Writer, antes do Editor | Verificar afirmações contra os dados de investigação |
| **SEO Optimiser** | Depois do Editor aceitar | Adicionar meta descrição, palavras-chave, slug |
| **Illustrator** | Depois do Editor aceitar | Gerar prompts de imagem para o artigo |
| **Translator** | Depois do Editor aceitar | Traduzir o artigo para outro idioma |

**Passos:**
1. Escrever o prompt de sistema do agente
2. Criar a função do agente (a condizer com o padrão existente na sua linguagem)
3. Inserir no orquestrador no ponto certo
4. Atualizar a saída/log para mostrar a contribuição do novo agente

---

## Como o Foundry Local e o Agent Framework Funcionam Juntos

Esta aplicação demonstra o padrão recomendado para construir sistemas multi-agente com Foundry Local:

| Camada | Componente | Papel |
|--------|------------|-------|
| **Runtime** | Foundry Local | Descarrega, gere e serve o modelo localmente |
| **Cliente** | OpenAI SDK | Envia completions de chat para o endpoint local |
| **Agente** | Prompt de sistema + chamada de chat | comportamento especializado por instruções focadas |
| **Orquestrador** | Coordenador do pipeline | Gere fluxo de dados, sequenciamento e ciclos de feedback |
| **Framework** | Microsoft Agent Framework | Disponibiliza a abstração e padrões `ChatAgent` |

O insight chave: **Foundry Local substitui o backend na cloud, não a arquitectura da aplicação.** Os mesmos padrões de agente, estratégias de orquestração e entregas estruturadas que funcionam com modelos hospedados na cloud funcionam idênticos com modelos locais — apenas aponta o cliente para o endpoint local em vez de um endpoint Azure.

---

## Principais Conclusões

| Conceito | O que Aprendeu |
|----------|----------------|
| Arquitectura de produção | Como estruturar uma app multi-agente com configuração partilhada e agentes separados |
| Carregamento do modelo em 4 passos | Boa prática para inicializar Foundry Local com progresso visível ao utilizador |
| Especialização de agentes | Cada um dos 4 agentes tem instruções focadas e formato de saída específico |
| Geração em streaming | Escritor produz tokens em tempo real, permitindo UIs responsivas |
| Ciclos de feedback | Tentativas disparadas pelo editor melhoram a qualidade sem intervenção humana |
| Padrões multi-linguagem | Mesma arquitetura funciona em Python, JavaScript e C# |
| Local = pronto para produção | Foundry Local serve a mesma API compatível com OpenAI usada na cloud |

---

## Próximo Passo

Continue para [Parte 8: Desenvolvimento Guiado por Avaliação](part8-evaluation-led-development.md) para construir um framework sistemático de avaliação para os seus agentes, usando conjuntos de dados golden, verificações baseadas em regras, e pontuação com LLM como juiz.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:
Este documento foi traduzido utilizando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, por favor esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original na sua língua nativa deve ser considerado a fonte autorizada. Para informações críticas, é recomendada a tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->