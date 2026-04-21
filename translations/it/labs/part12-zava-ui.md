![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 12: Costruire una Web UI per lo Zava Creative Writer

> **Obiettivo:** Aggiungere un front end basato su browser allo Zava Creative Writer così puoi osservare la pipeline multi-agente in esecuzione in tempo reale, con badge di stato agente live e testo dell'articolo in streaming, tutto servito da un unico server web locale.

In [Parte 7](part7-zava-creative-writer.md) hai esplorato lo Zava Creative Writer come **applicazione CLI** (JavaScript, C#) e come **API headless** (Python). In questo laboratorio collegherai un front end condiviso in **vanilla HTML/CSS/JavaScript** a ciascun backend in modo che gli utenti possano interagire con la pipeline tramite un browser anziché un terminale.

---

## Cosa Imparerai

| Obiettivo | Descrizione |
|-----------|-------------|
| Servire file statici da un backend | Montare una directory HTML/CSS/JS accanto alla tua route API |
| Consumare NDJSON in streaming nel browser | Usare la Fetch API con `ReadableStream` per leggere JSON delimitato da newline |
| Protocollo di streaming unificato | Garantire che i backend Python, JavaScript e C# emettano lo stesso formato di messaggio |
| Aggiornamenti UI progressivi | Aggiornare badge di stato agente e fare streaming del testo dell'articolo token per token |
| Aggiungere un livello HTTP a un’app CLI | Incapsulare la logica orchestratrice esistente in un server stile Express (JS) o in un API minimal ASP.NET Core (C#) |

---

## Architettura

L’UI è un singolo set di file statici (`index.html`, `style.css`, `app.js`) condiviso da tutti e tre i backend. Ogni backend espone le stesse due rotte:

![Architettura UI Zava — front end condiviso con tre backend](../../../images/part12-architecture.svg)

| Rotta | Metodo | Scopo |
|-------|--------|---------|
| `/` | GET | Serve l’UI statica |
| `/api/article` | POST | Esegue la pipeline multi-agente e fa streaming di NDJSON |

Il front end invia un corpo JSON e legge la risposta come uno stream di messaggi JSON delimitati da newline. Ogni messaggio ha un campo `type` che l’UI usa per aggiornare il pannello corretto:

| Tipo di messaggio | Significato |
|-------------|---------|
| `message` | Aggiornamento di stato (es. "Avvio del task agente ricercatore...") |
| `researcher` | Risultati della ricerca pronti |
| `marketing` | Risultati della ricerca prodotto pronti |
| `writer` | Scrittore avviato o terminato (contiene `{ start: true }` o `{ complete: true }`) |
| `partial` | Un singolo token trasmesso dallo Scrittore (contiene `{ text: "..." }`) |
| `editor` | Verdetto dell’editor pronto |
| `error` | Qualcosa è andato storto |

![Routing dei tipi di messaggi nel browser](../../../images/part12-message-types.svg)

![Sequenza di streaming — comunicazione Browser a Backend](../../../images/part12-streaming-sequence.svg)

---

## Prerequisiti

- Completare [Parte 7: Zava Creative Writer](part7-zava-creative-writer.md)
- CLI Foundry Local installata e modello `phi-3.5-mini` scaricato
- Un browser web moderno (Chrome, Edge, Firefox o Safari)

---

## L’UI Condivisa

Prima di toccare il codice backend, prenditi un momento per esplorare il front end che useranno tutti e tre i linguaggi. I file si trovano in `zava-creative-writer-local/ui/`:

| File | Scopo |
|------|---------|
| `index.html` | Layout della pagina: form di input, badge di stato agenti, area output articolo, pannelli dettagli espandibili |
| `style.css` | Stile minimale con stati colore per badge di stato (in attesa, in esecuzione, completato, errore) |
| `app.js` | Chiamata fetch, lettore di linee `ReadableStream` e logica di aggiornamento DOM |

> **Consiglio:** Apri `index.html` direttamente nel browser per vedere l’anteprima del layout. Nulla funzionerà ancora perché non c’è backend, ma puoi vedere la struttura.

### Come Funziona il Lettore di Stream

La funzione chiave in `app.js` legge il corpo della risposta a pezzi e divide sui confini delle newline:

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
    buffer = lines.pop(); // mantieni la linea finale incompleta

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

Ogni messaggio parsato viene inviato a `handleMessage()`, che aggiorna l’elemento DOM pertinente in base a `msg.type`.

---

## Esercizi

### Esercizio 1: Esegui il Backend Python con l’UI

La variante Python (FastAPI) ha già un endpoint API per lo streaming. L’unica modifica è montare la cartella `ui/` come file statici.

**1.1** Vai nella directory API Python e installa le dipendenze:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Avvia il server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Apri il browser su `http://localhost:8000`. Dovresti vedere l’interfaccia di Zava Creative Writer con tre campi di testo e un pulsante "Generate Article".

**1.4** Clicca su **Generate Article** usando i valori di default. Guarda i badge di stato agente che cambiano da "Waiting" a "Running" a "Done" man mano che ogni agente conclude il proprio lavoro, e vedi il testo dell’articolo trasmesso nel pannello output token per token.

> **Soluzione problemi:** Se la pagina mostra una risposta JSON invece dell’UI, assicurati di eseguire il `main.py` aggiornato che monta i file statici. L’endpoint `/api/article` funziona ancora nel percorso originale; il mounting dei file statici serve l’UI su tutte le altre rotte.

**Come funziona:** Il file `main.py` aggiornato aggiunge una sola linea in fondo:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Questo serve ogni file dalla cartella `zava-creative-writer-local/ui/` come risorsa statica, con `index.html` come documento di default. La route POST `/api/article` è registrata prima del mounting statico, quindi ha priorità.

---

### Esercizio 2: Aggiungi un Web Server alla Variante JavaScript

La variante JavaScript è attualmente un’app CLI (`main.mjs`). Un nuovo file, `server.mjs`, incapsula gli stessi moduli agenti dietro un server HTTP e serve l’UI condivisa.

**2.1** Entra nella directory JavaScript e installa le dipendenze:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Avvia il server web:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Dovresti vedere:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Apri `http://localhost:3000` nel browser e clicca su **Generate Article**. La stessa UI funziona identica contro il backend JavaScript.

**Studia il codice:** Apri `server.mjs` e osserva i pattern chiave:

- **Servizio di file statici** usa i moduli built-in Node.js `http`, `fs`, e `path` senza framework esterni.
- **Protezione da path traversal** normalizza il percorso richiesto e verifica che rimanga dentro la directory `ui/`.
- **Streaming NDJSON** usa un helper `sendLine()` che serializza ogni oggetto, rimuove newline interni e aggiunge una newline finale.
- **Orchestrazione agenti** riutilizza i moduli esistenti `researcher.mjs`, `product.mjs`, `writer.mjs` e `editor.mjs` senza modifiche.

<details>
<summary>Estratto chiave da server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Ricercatore
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Scrittore (streaming)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Esercizio 3: Aggiungi un’API Minimal alla Variante C#

La variante C# è attualmente un’app console. Un nuovo progetto, `csharp-web`, usa API minimal di ASP.NET Core per esporre la stessa pipeline come servizio web.

**3.1** Vai nella cartella del progetto web C# e ripristina i pacchetti:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Avvia il server web:

```bash
dotnet run
```

```powershell
dotnet run
```

Dovresti vedere:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Apri `http://localhost:5000` nel browser e clicca su **Generate Article**.

**Studia il codice:** Apri `Program.cs` nella directory `csharp-web` e osserva:

- Il file di progetto usa `Microsoft.NET.Sdk.Web` invece di `Microsoft.NET.Sdk`, che aggiunge supporto ASP.NET Core.
- I file statici vengono serviti tramite `UseDefaultFiles` e `UseStaticFiles` puntati alla cartella condivisa `ui/`.
- L’endpoint `/api/article` scrive linee NDJSON direttamente su `HttpContext.Response` e esegue flush dopo ogni linea per streaming in realtime.
- Tutta la logica agente (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) è la stessa della versione console.

<details>
<summary>Estratto chiave da csharp-web/Program.cs</summary>

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

### Esercizio 4: Esplora i Badge di Stato Agente

Ora che hai un’UI funzionante, guarda come il front end aggiorna i badge di stato.

**4.1** Apri `zava-creative-writer-local/ui/app.js` nel tuo editor.

**4.2** Trova la funzione `handleMessage()`. Nota come mappa i tipi di messaggi agli aggiornamenti del DOM:

| Tipo di messaggio | Azione UI |
|-------------|-----------|
| `message` contenente "researcher" | Imposta il badge Researcher su "Running" |
| `researcher` | Imposta il badge Researcher su "Done" e popola il pannello dei risultati ricerca |
| `marketing` | Imposta il badge Product Search su "Done" e popola il pannello Product Matches |
| `writer` con `data.start` | Imposta il badge Writer su "Running" e pulisce l’output dell’articolo |
| `partial` | Aggiunge il token di testo all’output articolo |
| `writer` con `data.complete` | Imposta il badge Writer su "Done" |
| `editor` | Imposta il badge Editor su "Done" e popola il pannello Editor Feedback |

**4.3** Apri i pannelli espandibili "Research Results", "Product Matches", e "Editor Feedback" sotto l’articolo per ispezionare il JSON grezzo prodotto da ogni agente.

---

### Esercizio 5: Personalizza l’UI (Stretch)

Prova uno o più di questi miglioramenti:

**5.1 Aggiungi un conteggio parole.** Dopo che lo Scrittore ha finito, mostra il conteggio parole dell’articolo sotto il pannello di output. Puoi calcolarlo in `handleMessage` quando `type === "writer"` e `data.complete` è true:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Aggiungi un indicatore di retry.** Quando l’Editor richiede una revisione, la pipeline si riavvia. Mostra un banner "Revision 1" o "Revision 2" nel pannello stato. Ascolta per un messaggio di tipo `message` contenente "Revision" e aggiorna un nuovo elemento DOM.

**5.3 Modalità scura.** Aggiungi un pulsante toggle e una classe `.dark` al `<body>`. Sovrascrivi sfondo, testo e colori dei pannelli in `style.css` con un selettore `body.dark`.

---

## Riepilogo

| Cosa hai fatto | Come |
|-------------|-----|
| Hai servito l’UI dal backend Python | Montando la cartella `ui/` con `StaticFiles` in FastAPI |
| Hai aggiunto un server HTTP alla variante JavaScript | Creato `server.mjs` usando il modulo built-in `http` di Node.js |
| Hai aggiunto un’API web alla variante C# | Creato un nuovo progetto `csharp-web` con API minimal ASP.NET Core |
| Hai consumato NDJSON in streaming nel browser | Usato `fetch()` con `ReadableStream` e parsing JSON linea per linea |
| Hai aggiornato l’UI in tempo reale | Mappato i tipi messaggi ad aggiornamenti DOM (badge, testo, pannelli espandibili) |

---

## Punti Chiave da Ricordare

1. Un **front end statico condiviso** può funzionare con qualunque backend che implementi lo stesso protocollo di streaming, rafforzando il valore del pattern API compatibile con OpenAI.
2. Il **JSON delimitato da newline (NDJSON)** è un formato streaming semplice e funziona nativamente con l’API `ReadableStream` del browser.
3. La **variante Python** ha richiesto meno modifiche perché aveva già un endpoint FastAPI; JavaScript e C# hanno richiesto un sottile wrapper HTTP.
4. Mantenere l’UI come **vanilla HTML/CSS/JS** evita strumenti di build, dipendenze da framework e complessità aggiuntive per i partecipanti al workshop.
5. Gli stessi moduli agenti (Researcher, Product, Writer, Editor) sono riutilizzati senza modifiche; cambia solo il livello di trasporto.

---

## Ulteriori Letture

| Risorsa | Link |
|----------|------|
| MDN: Usare Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Specifica NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Continua a [Parte 13: Workshop Completo](part13-workshop-complete.md) per un riepilogo di tutto ciò che hai costruito durante questo workshop.

---
[← Parte 11: Chiamata allo Strumento](part11-tool-calling.md) | [Parte 13: Officina Completa →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:
Questo documento è stato tradotto utilizzando il servizio di traduzione AI [Co-op Translator](https://github.com/Azure/co-op-translator). Pur impegnandoci per garantire l'accuratezza, si prega di notare che le traduzioni automatiche potrebbero contenere errori o inesattezze. Il documento originale nella sua lingua nativa dovrebbe essere considerato la fonte autorevole. Per informazioni critiche, si raccomanda una traduzione professionale effettuata da un essere umano. Non siamo responsabili per eventuali malintesi o interpretazioni errate derivanti dall'uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->