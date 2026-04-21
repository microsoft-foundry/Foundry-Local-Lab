![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 12: Construir uma UI Web para o Zava Creative Writer

> **Objetivo:** Adicionar uma interface de utilizador baseada em navegador ao Zava Creative Writer para que possa acompanhar o funcionamento da pipeline multi-agente em tempo real, com distintivos de estado dos agentes em direto e texto do artigo em streaming, tudo servido a partir de um único servidor web local.

Na [Parte 7](part7-zava-creative-writer.md) explorou o Zava Creative Writer como uma **aplicação CLI** (JavaScript, C#) e uma **API headless** (Python). Neste laboratório irá ligar uma front end partilhada em **HTML/CSS/JavaScript vanilla** a cada backend para que os utilizadores possam interagir com a pipeline através de um navegador em vez de um terminal.

---

## O Que Vai Aprender

| Objetivo | Descrição |
|-----------|-------------|
| Servir ficheiros estáticos a partir de um backend | Montar um diretório HTML/CSS/JS ao lado da sua rota API |
| Consumir NDJSON em streaming no navegador | Usar a Fetch API com `ReadableStream` para ler JSON delimitado por nova linha |
| Protocolo de streaming unificado | Assegurar que os backends Python, JavaScript e C# emitem o mesmo formato de mensagem |
| Atualizações progressivas da UI | Atualizar os distintivos de estado dos agentes e fazer streaming do texto do artigo token a token |
| Adicionar uma camada HTTP a uma app CLI | Envolver a lógica existente do orquestrador num servidor estilo Express (JS) ou API minimal ASP.NET Core (C#)|

---

## Arquitectura

A UI é um conjunto único de ficheiros estáticos (`index.html`, `style.css`, `app.js`) partilhados por todos os três backends. Cada backend expõe as mesmas duas rotas:

![Arquitectura Zava UI — front end partilhado com três backends](../../../images/part12-architecture.svg)

| Rota | Método | Finalidade |
|-------|--------|---------|
| `/` | GET | Serve a UI estática |
| `/api/article` | POST | Executa a pipeline multi-agente e faz streaming NDJSON |

A front end envia um corpo JSON e lê a resposta como um fluxo de mensagens JSON delimitadas por nova linha. Cada mensagem tem um campo `type` que a UI usa para atualizar o painel correto:

| Tipo de mensagem | Significado |
|-------------|---------|
| `message` | Atualização de estado (ex. "Iniciando tarefa do agente investigador...") |
| `researcher` | Resultados da investigação estão prontos |
| `marketing` | Resultados da pesquisa de produto estão prontos |
| `writer` | Escritor iniciou ou terminou (contém `{ start: true }` ou `{ complete: true }`) |
| `partial` | Um único token transmitido do Escritor (contém `{ text: "..." }`) |
| `editor` | Veredicto do editor está pronto |
| `error` | Ocorreu um erro |

![Roteamento de tipos de mensagem no navegador](../../../images/part12-message-types.svg)

![Sequência de streaming — Comunicação do navegador para backend](../../../images/part12-streaming-sequence.svg)

---

## Pré-requisitos

- Completar a [Parte 7: Zava Creative Writer](part7-zava-creative-writer.md)  
- Ter o Foundry Local CLI instalado e o modelo `phi-3.5-mini` descarregado  
- Um navegador web moderno (Chrome, Edge, Firefox ou Safari)

---

## A UI Partilhada

Antes de tocar em qualquer código de backend, dedique um momento para explorar a front end que todas as três implementações irão usar. Os ficheiros estão em `zava-creative-writer-local/ui/`:

| Ficheiro | Finalidade |
|------|---------|
| `index.html` | Layout da página: formulário de entrada, distintivos de estado dos agentes, área de saída do artigo, painéis de detalhes colapsáveis |
| `style.css` | Estilo minimalista com cores de estado para os distintivos (à espera, a correr, terminado, erro) |
| `app.js` | Chamada fetch, leitor de linhas `ReadableStream` e lógica de atualização do DOM |

> **Dica:** Abra `index.html` diretamente no seu navegador para pré-visualizar o layout. Nada funcionará ainda porque não há backend, mas pode ver a estrutura.

### Como funciona o Leitor de Stream

A função chave em `app.js` lê o corpo da resposta pedaço a pedaço e divide nos limites de nova linha:

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

Cada mensagem analisada é enviada para `handleMessage()`, que atualiza o elemento DOM relevante com base em `msg.type`.

---

## Exercícios

### Exercício 1: Execute o Backend Python com a UI

A variante Python (FastAPI) já tem um endpoint API de streaming. A única alteração é montar a pasta `ui/` como ficheiros estáticos.

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

**1.3** Abra o seu navegador em `http://localhost:8000`. Deve ver a UI do Zava Creative Writer com três campos de texto e um botão "Generate Article".

**1.4** Clique em **Generate Article** usando os valores por defeito. Observe os distintivos de estado dos agentes mudarem de "Waiting" para "Running" para "Done" conforme cada agente completa o seu trabalho, e veja o texto do artigo a ser transmitido token a token no painel de saída.

> **Solução de problemas:** Se a página mostrar uma resposta JSON em vez da UI, assegure-se de estar a executar o `main.py` atualizado que monta os ficheiros estáticos. O endpoint `/api/article` continua a funcionar na sua rota original; a montagem dos ficheiros estáticos serve a UI em todas as outras rotas.

**Como funciona:** O `main.py` atualizado adiciona uma linha única no final:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Isto serve todos os ficheiros de `zava-creative-writer-local/ui/` como um recurso estático, com `index.html` como documento padrão. A rota POST `/api/article` está registada antes da montagem estática, por isso tem prioridade.

---

### Exercício 2: Adicionar um Servidor Web à Variante JavaScript

A variante JavaScript é atualmente uma aplicação CLI (`main.mjs`). Um novo ficheiro, `server.mjs`, envolve os mesmos módulos de agentes atrás de um servidor HTTP e serve a UI partilhada.

**2.1** Navegue para o diretório JavaScript e instale as dependências:

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

Deve ver:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Abra `http://localhost:3000` no seu navegador e clique em **Generate Article**. A mesma UI funciona de forma idêntica contra o backend JavaScript.

**Estude o código:** Abra `server.mjs` e note os padrões chave:

- **Serviço de ficheiros estáticos** usa os módulos built-in Node.js `http`, `fs` e `path` sem necessidade de qualquer framework externa.
- **Proteção contra path-traversal** normaliza o caminho pedido e verifica que permanece dentro do diretório `ui/`.
- **Streaming NDJSON** usa uma função auxiliar `sendLine()` que serializa cada objeto, elimina novas linhas internas e adiciona uma nova linha no final.
- **Orquestração de agentes** reutiliza os módulos existentes `researcher.mjs`, `product.mjs`, `writer.mjs` e `editor.mjs` sem alterações.

<details>
<summary>Excerto chave de server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Investigador
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

### Exercício 3: Adicionar uma API Minimal à Variante C#

A variante C# é atualmente uma aplicação de consola. Um novo projeto, `csharp-web`, usa APIs minimal ASP.NET Core para expor a mesma pipeline como serviço web.

**3.1** Navegue para o projeto web C# e restaure os pacotes:

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

Deve ver:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Abra `http://localhost:5000` no seu navegador e clique em **Generate Article**.

**Estude o código:** Abra `Program.cs` no diretório `csharp-web` e note:

- O ficheiro de projeto usa `Microsoft.NET.Sdk.Web` em vez de `Microsoft.NET.Sdk`, o que adiciona suporte ao ASP.NET Core.
- Os ficheiros estáticos são servidos via `UseDefaultFiles` e `UseStaticFiles` apontados ao diretório partilhado `ui/`.
- O endpoint `/api/article` escreve linhas NDJSON diretamente para `HttpContext.Response` e faz flush após cada linha para streaming em tempo real.
- Toda a lógica dos agentes (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) é igual à versão consola.

<details>
<summary>Excerto chave de csharp-web/Program.cs</summary>

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

### Exercício 4: Explore os Distintivos de Estado dos Agentes

Agora que tem uma UI a funcionar, veja como a front end atualiza os distintivos de estado.

**4.1** Abra `zava-creative-writer-local/ui/app.js` no seu editor.

**4.2** Encontre a função `handleMessage()`. Repare como ela mapeia os tipos de mensagem para atualizações DOM:

| Tipo de mensagem | Ação na UI |
|-------------|-----------|
| `message` contendo "researcher" | Define o distintivo Researcher como "Running" |
| `researcher` | Define o distintivo Researcher como "Done" e preenche o painel Research Results |
| `marketing` | Define o distintivo Product Search como "Done" e preenche o painel Product Matches |
| `writer` com `data.start` | Define o distintivo Writer como "Running" e limpa a saída do artigo |
| `partial` | Acrescenta o texto do token à saída do artigo |
| `writer` com `data.complete` | Define o distintivo Writer como "Done" |
| `editor` | Define o distintivo Editor como "Done" e preenche o painel Editor Feedback |

**4.3** Abra os painéis colapsáveis "Research Results", "Product Matches" e "Editor Feedback" abaixo do artigo para inspecionar o JSON bruto que cada agente produziu.

---

### Exercício 5: Personalize a UI (Desafio)

Tente um ou mais destes melhoramentos:

**5.1 Adicionar contagem de palavras.** Após o Escritor terminar, mostre a contagem de palavras do artigo abaixo do painel de saída. Pode calcular isto em `handleMessage` quando `type === "writer"` e `data.complete` for verdadeiro:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Adicionar um indicador de nova revisão.** Quando o Editor solicita uma revisão, a pipeline reinicia. Mostre um banner "Revision 1" ou "Revision 2" no painel de estado. Ouça uma mensagem do tipo `message` contendo "Revision" e actualize um novo elemento DOM.

**5.3 Modo escuro.** Adicione um botão de alternância e uma classe `.dark` ao `<body>`. Sobrescreva as cores de fundo, texto e painéis em `style.css` com o selector `body.dark`.

---

## Resumo

| O que fez | Como |
|-------------|-----|
| Serviu a UI a partir do backend Python | Montou a pasta `ui/` com `StaticFiles` no FastAPI |
| Adicionou um servidor HTTP à variante JavaScript | Criou `server.mjs` usando o módulo Node.js `http` incorporado |
| Adicionou uma API web à variante C# | Criou um projeto novo `csharp-web` com APIs minimal ASP.NET Core |
| Consumiu NDJSON streaming no navegador | Usou `fetch()` com `ReadableStream` e parsing JSON linha-a-linha |
| Atualizou a UI em tempo real | Mapeou tipos de mensagem para atualizações do DOM (distintivos, texto, painéis colapsáveis) |

---

## Principais Conclusões

1. Uma **front end estática partilhada** pode funcionar com qualquer backend que use o mesmo protocolo de streaming, reforçando o valor do padrão API compatível com OpenAI.
2. O **JSON delimitado por nova linha (NDJSON)** é um formato de streaming simples que funciona nativamente com a API `ReadableStream` do navegador.
3. A **variante Python** precisou da menor alteração porque já tinha um endpoint FastAPI; as variantes JavaScript e C# precisaram de uma camada HTTP fina.
4. Manter a UI como **HTML/CSS/JS vanilla** evita ferramentas de build, dependências de frameworks e complexidade adicional para os aprendizes do workshop.
5. Os mesmos módulos de agente (Researcher, Product, Writer, Editor) são reutilizados sem modificação; só muda a camada de transporte.

---

## Leituras Adicionais

| Recurso | Link |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Continue para a [Parte 13: Workshop Completo](part13-workshop-complete.md) para um resumo de tudo o que construiu ao longo deste workshop.

---
[← Parte 11: Chamada de Ferramenta](part11-tool-calling.md) | [Parte 13: Oficina Completa →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos para garantir a precisão, tenha em atenção que traduções automáticas podem conter erros ou imprecisões. O documento original, no seu idioma nativo, deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se a tradução profissional por um humano. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações erróneas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->