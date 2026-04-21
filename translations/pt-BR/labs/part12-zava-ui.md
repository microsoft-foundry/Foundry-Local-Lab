![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 12: Construindo uma Interface Web para o Zava Creative Writer

> **Objetivo:** Adicionar uma interface web baseada em navegador ao Zava Creative Writer para que você possa acompanhar a execução do pipeline multiagente em tempo real, com distintivos de status dos agentes ao vivo e texto do artigo transmitido, tudo servido a partir de um único servidor web local.

Na [Parte 7](part7-zava-creative-writer.md) você explorou o Zava Creative Writer como uma **aplicação CLI** (JavaScript, C#) e uma **API headless** (Python). Neste laboratório, você conectará uma interface compartilhada em **vanilla HTML/CSS/JavaScript** a cada backend para que os usuários possam interagir com o pipeline através de um navegador em vez de um terminal.

---

## O Que Você Vai Aprender

| Objetivo | Descrição |
|-----------|-------------|
| Servir arquivos estáticos de um backend | Montar um diretório HTML/CSS/JS ao lado da sua rota API |
| Consumir NDJSON em streaming no navegador | Usar a Fetch API com `ReadableStream` para ler JSON delimitado por nova linha |
| Protocolo de streaming unificado | Garantir que os backends Python, JavaScript e C# emitam o mesmo formato de mensagem |
| Atualizações progressivas da interface | Atualizar distintivos de status dos agentes e transmitir texto do artigo token por token |
| Adicionar uma camada HTTP a um app CLI | Encapsular a lógica do orquestrador existente em um servidor estilo Express (JS) ou API minimalista ASP.NET Core (C#) |

---

## Arquitetura

A UI é um conjunto único de arquivos estáticos (`index.html`, `style.css`, `app.js`) compartilhado por todos os três backends. Cada backend expõe as mesmas duas rotas:

![Arquitetura da UI Zava — front end compartilhado com três backends](../../../images/part12-architecture.svg)

| Rota | Método | Propósito |
|-------|--------|---------|
| `/` | GET | Serve a UI estática |
| `/api/article` | POST | Executa o pipeline multiagente e transmite NDJSON |

A interface envia um corpo JSON e lê a resposta como um fluxo de mensagens JSON delimitadas por nova linha. Cada mensagem possui um campo `type` que a UI usa para atualizar o painel correto:

| Tipo de mensagem | Significado |
|-------------|---------|
| `message` | Atualização de status (ex: "Iniciando tarefa do agente pesquisador...") |
| `researcher` | Resultados da pesquisa estão prontos |
| `marketing` | Resultados da busca de produto estão prontos |
| `writer` | Escritor iniciado ou finalizado (contém `{ start: true }` ou `{ complete: true }`) |
| `partial` | Um único token transmitido do Escritor (contém `{ text: "..." }`) |
| `editor` | Veredito do editor está pronto |
| `error` | Algo deu errado |

![Roteamento do tipo de mensagem no navegador](../../../images/part12-message-types.svg)

![Sequência de streaming — Comunicação do Navegador para o Backend](../../../images/part12-streaming-sequence.svg)

---

## Pré-requisitos

- Completar a [Parte 7: Zava Creative Writer](part7-zava-creative-writer.md)
- CLI Foundry Local instalada e o modelo `phi-3.5-mini` baixado
- Um navegador moderno (Chrome, Edge, Firefox ou Safari)

---

## A UI Compartilhada

Antes de tocar em qualquer código backend, reserve um momento para explorar a interface que todos os três tracks de linguagem usarão. Os arquivos ficam em `zava-creative-writer-local/ui/`:

| Arquivo | Propósito |
|------|---------|
| `index.html` | Layout da página: formulário de entrada, distintivos de status dos agentes, área de saída do artigo, painéis detalhados recolhíveis |
| `style.css` | Estilização mínima com estados de cor nos distintivos de status (aguardando, em execução, concluído, erro) |
| `app.js` | Chamada fetch, leitor de linhas `ReadableStream` e lógica de atualização do DOM |

> **Dica:** Abra `index.html` diretamente no seu navegador para visualizar o layout. Nada funcionará ainda porque não há backend, mas você pode ver a estrutura.

### Como o Leitor de Stream Funciona

A função chave em `app.js` lê o corpo da resposta pedaço por pedaço e divide nas quebras de linha:

```javascript
async function readStream(response) {
  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = "";

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;

    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split("\n");
    buffer = lines.pop(); // manter a linha final incompleta

    for (const line of lines) {
      const trimmed = line.trim();
      if (!trimmed) continue;
      try {
        const msg = JSON.parse(trimmed);
        if (msg && msg.type) handleMessage(msg);
      } catch { /* skip non-JSON lines */ }
    }
  }
}
```

Cada mensagem analisada é enviada para `handleMessage()`, que atualiza o elemento DOM relevante baseado em `msg.type`.

---

## Exercícios

### Exercício 1: Executar o Backend Python com a UI

A variante Python (FastAPI) já possui um endpoint API de streaming. A única mudança é montar a pasta `ui/` como arquivos estáticos.

**1.1** Navegue para o diretório da API Python e instale as dependências:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Inicie o servidor:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Abra seu navegador em `http://localhost:8000`. Você deve ver a UI do Zava Creative Writer com três campos de texto e um botão "Generate Article".

**1.4** Clique em **Generate Article** usando os valores padrão. Observe os distintivos de status dos agentes mudarem de "Waiting" para "Running" para "Done" conforme cada agente completa seu trabalho, e veja o texto do artigo sendo transmitido para o painel de saída token por token.

> **Resolução de problemas:** Se a página mostrar uma resposta JSON em vez da UI, verifique se você está rodando o `main.py` atualizado que monta os arquivos estáticos. O endpoint `/api/article` ainda funciona no caminho original; a montagem dos arquivos estáticos serve a UI em todas as outras rotas.

**Como funciona:** O `main.py` atualizado adiciona uma única linha no fim:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Isso serve cada arquivo dentro de `zava-creative-writer-local/ui/` como um recurso estático, com `index.html` como o documento padrão. A rota POST `/api/article` é registrada antes da montagem estática, então tem prioridade.

---

### Exercício 2: Adicionar um Servidor Web à Variante JavaScript

A variante JavaScript é atualmente uma aplicação CLI (`main.mjs`). Um novo arquivo, `server.mjs`, encapsula os mesmos módulos de agente por trás de um servidor HTTP e serve a UI compartilhada.

**2.1** Navegue até o diretório JavaScript e instale as dependências:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Inicie o servidor web:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Você deve ver:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Abra `http://localhost:3000` em seu navegador e clique em **Generate Article**. A mesma UI funciona idêntica contra o backend JavaScript.

**Estude o código:** Abra `server.mjs` e observe os padrões chave:

- Servir arquivos estáticos usa os módulos internos Node.js `http`, `fs` e `path` sem framework externo.
- Proteção contra path-traversal normaliza o caminho solicitado e verifica se ele permanece dentro do diretório `ui/`.
- Streaming NDJSON usa um helper `sendLine()` que serializa cada objeto, remove quebras de linha internas e acrescenta uma quebra de linha final.
- Orquestração de agentes reutiliza os módulos existentes `researcher.mjs`, `product.mjs`, `writer.mjs` e `editor.mjs` sem modificações.

<details>
<summary>Trecho chave de server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Pesquisador
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Escritor (streaming)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Exercício 3: Adicionar uma API Minimalista à Variante C#

A variante C# é atualmente um aplicativo console. Um novo projeto, `csharp-web`, usa APIs minimalistas ASP.NET Core para expor o mesmo pipeline como serviço web.

**3.1** Navegue até o projeto web C# e restaure os pacotes:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Execute o servidor web:

```bash
dotnet run
```

```powershell
dotnet run
```

Você deve ver:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Abra `http://localhost:5000` em seu navegador e clique em **Generate Article**.

**Estude o código:** Abra `Program.cs` no diretório `csharp-web` e observe:

- O arquivo do projeto usa `Microsoft.NET.Sdk.Web` em vez de `Microsoft.NET.Sdk` para adicionar suporte ao ASP.NET Core.
- Arquivos estáticos são servidos via `UseDefaultFiles` e `UseStaticFiles` apontando para o diretório compartilhado `ui/`.
- O endpoint `/api/article` escreve linhas NDJSON diretamente em `HttpContext.Response` e faz flush após cada linha para streaming em tempo real.
- Toda a lógica dos agentes (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) é a mesma da versão console.

<details>
<summary>Trecho chave de csharp-web/Program.cs</summary>

```csharp
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/event-stream; charset=utf-8";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    // Researcher
    await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
    var researchResult = RunResearcher(body.Research, feedback);
    await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

    // Writer (streaming)
    foreach (var update in completionUpdates)
    {
        if (update.ContentUpdate.Count > 0)
        {
            var text = update.ContentUpdate[0].Text;
            await SendLine(new { type = "partial", message = "token", data = new { text } });
        }
    }
});
```

</details>

---

### Exercício 4: Explore os Distintivos de Status dos Agentes

Agora que você tem uma UI funcional, veja como o front end atualiza os distintivos de status.

**4.1** Abra `zava-creative-writer-local/ui/app.js` no seu editor.

**4.2** Encontre a função `handleMessage()`. Note como ela mapeia tipos de mensagem para atualizações no DOM:

| Tipo de mensagem | Ação na UI |
|-------------|-----------|
| `message` contendo "researcher" | Define o distintivo Researcher como "Running" |
| `researcher` | Define o distintivo Researcher como "Done" e popula o painel de Resultados da Pesquisa |
| `marketing` | Define o distintivo Product Search como "Done" e popula o painel de Correspondências de Produto |
| `writer` com `data.start` | Define o distintivo Writer como "Running" e limpa a saída do artigo |
| `partial` | Anexa o texto do token à saída do artigo |
| `writer` com `data.complete` | Define o distintivo Writer como "Done" |
| `editor` | Define o distintivo Editor como "Done" e popula o painel de Feedback do Editor |

**4.3** Abra os painéis recolhíveis "Research Results", "Product Matches" e "Editor Feedback" abaixo do artigo para inspecionar o JSON bruto produzido por cada agente.

---

### Exercício 5: Personalize a UI (Extra)

Tente uma ou mais destas melhorias:

**5.1 Adicione uma contagem de palavras.** Após o Writer finalizar, exiba a contagem de palavras do artigo abaixo do painel de saída. Você pode calcular isso em `handleMessage` quando `type === "writer"` e `data.complete` for verdadeiro:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Adicione um indicador de tentativa.** Quando o Editor solicitar uma revisão, o pipeline será reexecutado. Mostre um banner "Revision 1" ou "Revision 2" no painel de status. Ouça um tipo `message` contendo "Revision" e atualize um novo elemento no DOM.

**5.3 Modo escuro.** Adicione um botão de alternância e uma classe `.dark` ao `<body>`. Sobrescreva as cores de fundo, texto e painéis em `style.css` com um seletor `body.dark`.

---

## Resumo

| O que você fez | Como |
|-------------|-----|
| Serviu a UI do backend Python | Montou a pasta `ui/` com `StaticFiles` no FastAPI |
| Adicionou um servidor HTTP à variante JavaScript | Criou `server.mjs` usando o módulo interno Node.js `http` |
| Adicionou uma API web à variante C# | Criou um projeto `csharp-web` com APIs minimalistas ASP.NET Core |
| Consumiu NDJSON em streaming no navegador | Usou `fetch()` com `ReadableStream` e análise JSON linha a linha |
| Atualizou a UI em tempo real | Mapeou tipos de mensagem para atualizações no DOM (distintivos, texto, painéis recolhíveis) |

---

## Principais Lições

1. Um **front end estático compartilhado** pode funcionar com qualquer backend que fale o mesmo protocolo de streaming, reforçando o valor do padrão de API compatível com OpenAI.
2. O **JSON delimitado por nova linha (NDJSON)** é um formato simples de streaming que funciona nativamente com a API `ReadableStream` do navegador.
3. A **variante Python** precisou da menor mudança porque já tinha um endpoint FastAPI; as variantes JavaScript e C# precisaram de um wrapper HTTP leve.
4. Manter a UI como **HTML/CSS/JS vanilla** evita ferramentas de build, dependências de frameworks e complexidade adicional para os participantes do workshop.
5. Os mesmos módulos dos agentes (Researcher, Product, Writer, Editor) são reutilizados sem modificação; apenas a camada de transporte muda.

---

## Leituras Complementares

| Recurso | Link |
|----------|------|
| MDN: Usando Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Arquivos Estáticos | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Arquivos Estáticos | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Especificação NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Continue para a [Parte 13: Workshop Completo](part13-workshop-complete.md) para um resumo de tudo o que você construiu ao longo deste workshop.

---
[← Parte 11: Chamada de Ferramenta](part11-tool-calling.md) | [Parte 13: Oficina Completa →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos para garantir a precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se a tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->