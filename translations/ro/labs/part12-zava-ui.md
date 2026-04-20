![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partea 12: Construirea unei Interfețe Web pentru Zava Creative Writer

> **Scop:** Adăugați o interfață web bazată pe browser pentru Zava Creative Writer astfel încât să puteți urmări execuția conductei multi-agent în timp real, cu insigne de stare a agenților live și textul articolului transmis în flux, toate servite dintr-un singur server web local.

În [Partea 7](part7-zava-creative-writer.md) ați explorat Zava Creative Writer ca o **aplicație CLI** (JavaScript, C#) și o **API fara interfață** (Python). În acest laborator veți conecta o interfață comună **vanilla HTML/CSS/JavaScript** la fiecare backend astfel încât utilizatorii să poată interacționa cu conducta printr-un browser în loc de un terminal.

---

## Ce veți învăța

| Obiectiv | Descriere |
|-----------|-------------|
| Servirea fișierelor statice dintr-un backend | Montarea unui director HTML/CSS/JS alături de ruta API |
| Consumarea NDJSON în streaming în browser | Folosirea Fetch API cu `ReadableStream` pentru a citi JSON delimitat prin linii noi |
| Protocol unificat de streaming | Asigurarea că backend-urile Python, JavaScript și C# emit același format de mesaje |
| Actualizări progresive ale UI-ului | Actualizarea insignei de stare a agenților și transmiterea textului articolului token cu token |
| Adăugarea unui strat HTTP la o aplicație CLI | Înfășurarea logicii orchestratorului existent într-un server de tip Express (JS) sau API minimal ASP.NET Core (C#) |

---

## Arhitectură

UI-ul este un set unic de fișiere statice (`index.html`, `style.css`, `app.js`) partajat de toate cele trei backend-uri. Fiecare backend expune aceleași două rute:

![Zava UI architecture — front end partajat cu trei backend-uri](../../../images/part12-architecture.svg)

| Rută | Metodă | Scop |
|-------|--------|---------|
| `/` | GET | Servește UI-ul static |
| `/api/article` | POST | Rulează conducta multi-agent și transmite NDJSON în streaming |

Interfața trimite un corp JSON și citește răspunsul ca un flux de mesaje JSON delimitate prin linii noi. Fiecare mesaj are un câmp `type` pe care UI-ul îl folosește pentru a actualiza panoul corect:

| Tip mesaj | Înțeles |
|-------------|---------|
| `message` | Actualizare stare (ex. "Pornirea sarcinii agentului researcher...") |
| `researcher` | Rezultatele cercetării sunt gata |
| `marketing` | Rezultatele căutării produsului sunt gata |
| `writer` | Writer-ul a pornit sau s-a terminat (conține `{ start: true }` sau `{ complete: true }`) |
| `partial` | Un singur token transmis din Writer (conține `{ text: "..." }`) |
| `editor` | Verdictul editorului este gata |
| `error` | Ceva nu a funcționat |

![Rutare tipuri mesaje în browser](../../../images/part12-message-types.svg)

![Secvență streaming — Comunicare Browser către Backend](../../../images/part12-streaming-sequence.svg)

---

## Cerințe

- Finalizați [Partea 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI instalat și modelul `phi-3.5-mini` descărcat
- Un browser modern (Chrome, Edge, Firefox sau Safari)

---

## UI Comun

Înainte de a modifica codul backend, luați un moment să explorați interfața front-end pe care o vor folosi toate cele trei trasee de limbaj. Fișierele se găsesc în `zava-creative-writer-local/ui/`:

| Fișier | Scop |
|------|---------|
| `index.html` | Layout-ul paginii: formular de input, insigne de stare a agenților, zonă de ieșire a articolului, panouri detaliate pliabile |
| `style.css` | Stilizare minimală cu stări de culoare pentru insignele de stare (în așteptare, în execuție, gata, eroare) |
| `app.js` | Apel fetch, cititor de linii `ReadableStream`, și logică de actualizare DOM |

> **Sfaturi:** Deschideți `index.html` direct în browser pentru a previzualiza layout-ul. Nimic nu va funcționa încă pentru că nu există backend, dar puteți vedea structura.

### Cum funcționează cititorul de flux

Funcția cheie din `app.js` citește corpul răspunsului bucățică cu bucățică și împarte după delimitatoare de linie nouă:

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
    buffer = lines.pop(); // păstrează linia finală incompletă

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

Fiecare mesaj analizat este trimis către `handleMessage()`, care actualizează elementul DOM relevant bazat pe `msg.type`.

---

## Exerciții

### Exercițiul 1: Rulați Backend-ul Python cu UI-ul

Varianta Python (FastAPI) are deja un endpoint API de streaming. Singura schimbare este montarea folderului `ui/` ca fișiere statice.

**1.1** Navigați la directorul API Python și instalați dependențele:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Porniți serverul:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Deschideți browserul la `http://localhost:8000`. Ar trebui să vedeți UI-ul Zava Creative Writer cu trei câmpuri de text și un buton "Generate Article".

**1.4** Apăsați **Generate Article** folosind valorile implicite. Observați cum insignele de stare ale agenților se schimbă de la "Waiting" la "Running" la "Done" pe măsură ce fiecare agent își finalizează munca și vedeți textul articolului transmis în flux din token în token în panoul de ieșire.

> **Depanare:** Dacă pagina afișează un răspuns JSON în loc de UI, asigurați-vă că rulați `main.py` actualizat care montează fișierele statice. Endpoint-ul `/api/article` funcționează în continuare la calea inițială; montarea fișierelor statice servește UI-ul pe orice altă rută.

**Cum funcționează:** `main.py` actualizat adaugă o singură linie la final:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Aceasta servește fiecare fișier din `zava-creative-writer-local/ui/` ca asset static, cu `index.html` ca document implicit. Ruta POST `/api/article` este înregistrată înainte de montarea statică, deci are prioritate.

---

### Exercițiul 2: Adăugați un Server Web la Varianta JavaScript

Varianta JavaScript este momentan o aplicație CLI (`main.mjs`). Un fișier nou, `server.mjs`, afișează aceleași module agent în spatele unui server HTTP și servește UI-ul comun.

**2.1** Navigați în directorul JavaScript și instalați dependențele:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Porniți serverul web:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Ar trebui să vedeți:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Deschideți `http://localhost:3000` în browser și apăsați **Generate Article**. Același UI funcționează identic cu backend-ul JavaScript.

**Studiați codul:** Deschideți `server.mjs` și observați modelele cheie:

- **Servirea fișierelor statice** folosește module Node.js încorporate `http`, `fs` și `path` fără framework extern.
- **Protecție path-traversal** normalizează calea solicitată și verifică că rămâne în directorul `ui/`.
- **Streaming NDJSON** folosește un helper `sendLine()` care serializă fiecare obiect, elimină linii noi interne și adaugă o linie nouă finală.
- **Orchestrarea agenților** reutilizează modulele existente `researcher.mjs`, `product.mjs`, `writer.mjs`, și `editor.mjs` neschimbate.

<details>
<summary>Fragment cheie din server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Cercetător
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Scriitor (streaming)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Exercițiul 3: Adăugați o API Minimală la Varianta C#

Varianta C# este momentan o aplicație de consolă. Un proiect nou, `csharp-web`, folosește API-uri minimale ASP.NET Core pentru a expune aceeași conductă ca serviciu web.

**3.1** Navigați la proiectul web C# și restaurați pachetele:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Rulați serverul web:

```bash
dotnet run
```

```powershell
dotnet run
```

Ar trebui să vedeți:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Deschideți `http://localhost:5000` în browser și apăsați **Generate Article**.

**Studiați codul:** Deschideți `Program.cs` din directorul `csharp-web` și observați:

- Fișierul proiectului folosește `Microsoft.NET.Sdk.Web` în loc de `Microsoft.NET.Sdk`, care adaugă suport ASP.NET Core.
- Fișierele statice sunt servite prin `UseDefaultFiles` și `UseStaticFiles` indicând către directorul comun `ui/`.
- Endpoint-ul `/api/article` scrie linii NDJSON direct în `HttpContext.Response` și face flush după fiecare linie pentru streaming în timp real.
- Toată logica agenților (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) este aceeași ca în versiunea de consolă.

<details>
<summary>Fragment cheie din csharp-web/Program.cs</summary>

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

### Exercițiul 4: Explorați Insignele de Stare ale Agenților

Acum că aveți un UI funcțional, observați cum interfața actualizează insignele de stare.

**4.1** Deschideți `zava-creative-writer-local/ui/app.js` în editorul dvs.

**4.2** Găsiți funcția `handleMessage()`. Observați cum mapează tipurile de mesaj la actualizări în DOM:

| Tip mesaj | Acțiune UI |
|-------------|-----------|
| `message` conținând "researcher" | Setează insigna Researcher pe "Running" |
| `researcher` | Setează insigna Researcher pe "Done" și populează panoul Research Results |
| `marketing` | Setează insigna Product Search pe "Done" și populează panoul Product Matches |
| `writer` cu `data.start` | Setează insigna Writer pe "Running" și golește ieșirea articolului |
| `partial` | Adaugă textul tokenului la ieșirea articolului |
| `writer` cu `data.complete` | Setează insigna Writer pe "Done" |
| `editor` | Setează insigna Editor pe "Done" și populează panoul Editor Feedback |

**4.3** Deschideți panourile pliabile "Research Results", "Product Matches" și "Editor Feedback" sub articol pentru a inspecta JSON-ul brut produs de fiecare agent.

---

### Exercițiul 5: Personalizați UI-ul (Extensie)

Încercați una sau mai multe dintre aceste îmbunătățiri:

**5.1 Adăugați numărul de cuvinte.** După ce Writer-ul se termină, afișați numărul de cuvinte al articolului sub panoul de ieșire. Puteți calcula acest lucru în `handleMessage` când `type === "writer"` și `data.complete` este adevărat:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Adăugați un indicator de retry.** Când Editorul solicită o revizuire, conducta rulează din nou. Afișați un banner "Revision 1" sau "Revision 2" în panoul de stare. Ascultați pentru un tip `message` conținând "Revision" și actualizați un nou element DOM.

**5.3 Mod întunecat.** Adăugați un buton de comutare și o clasă `.dark` pe `<body>`. Suprascrieți culorile de fundal, text și panouri în `style.css` cu selectorul `body.dark`.

---

## Rezumat

| Ce ați făcut | Cum |
|-------------|-----|
| Ați servit UI-ul din backend-ul Python | Ați montat folderul `ui/` cu `StaticFiles` în FastAPI |
| Ați adăugat un server HTTP variantei JavaScript | Ați creat `server.mjs` folosind modulul încorporat Node.js `http` |
| Ați adăugat o API web variantei C# | Ați creat un proiect nou `csharp-web` cu API-uri minimale ASP.NET Core |
| Ați consumat NDJSON în streaming în browser | Ați folosit `fetch()` cu `ReadableStream` și parsing JSON linie cu linie |
| Ați actualizat UI-ul în timp real | Ați mapat tipurile de mesaje la actualizări DOM (insigne, text, panouri pliabile) |

---

## Concluzii Cheie

1. Un **front-end static comun** poate funcționa cu orice backend care vorbește același protocol de streaming, întărind valoarea modelului API compatibil OpenAI.
2. **JSON delimitat prin linii noi (NDJSON)** este un format simplu de streaming care funcționează nativ cu API-ul browserului `ReadableStream`.
3. Varianta **Python** a avut cea mai mică schimbare pentru că avea deja un endpoint FastAPI; variantele JavaScript și C# au necesitat un strat HTTP suplimentar subțire.
4. Păstrarea UI-ului ca **vanilla HTML/CSS/JS** evită uneltele de build, dependențe de framework și complexitate suplimentară pentru cursanți.
5. Aceleași module pentru agenți (Researcher, Product, Writer, Editor) sunt reutilizate fără modificări; doar stratul de transport se schimbă.

---

## Lecturi suplimentare

| Resursă | Link |
|----------|------|
| MDN: Utilizarea fluxurilor Readable | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Fișiere statice | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Fișiere statice | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Specificația NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Continuați la [Partea 13: Workshop Finalizat](part13-workshop-complete.md) pentru un rezumat al tot ce ați construit pe parcursul acestui workshop.

---
[← Partea 11: Apelarea uneltei](part11-tool-calling.md) | [Partea 13: Atelierul complet →](part13-workshop-complete.md)