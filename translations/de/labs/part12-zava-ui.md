![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Teil 12: Aufbau einer Web-Benutzeroberfläche für den Zava Creative Writer

> **Ziel:** Fügen Sie dem Zava Creative Writer eine browserbasierte Benutzeroberfläche hinzu, damit Sie die Multi-Agenten-Pipeline in Echtzeit beobachten können – mit Live-Agentenstatus-Abzeichen und gestreamtem Artikeltext, alles bereitgestellt von einem einzelnen lokalen Webserver.

In [Teil 7](part7-zava-creative-writer.md) haben Sie den Zava Creative Writer als **CLI-Anwendung** (JavaScript, C#) und als **headless API** (Python) erkundet. In diesem Labor verbinden Sie eine gemeinsame **Vanilla HTML/CSS/JavaScript**-Benutzeroberfläche mit jedem Backend, sodass Nutzer über einen Browser statt über ein Terminal mit der Pipeline interagieren können.

---

## Was Sie lernen werden

| Ziel | Beschreibung |
|-----------|-------------|
| Statische Dateien vom Backend bedienen | Ein HTML/CSS/JS-Verzeichnis neben Ihrer API-Route bereitstellen |
| Streaming NDJSON im Browser konsumieren | Verwenden der Fetch-API mit `ReadableStream`, um Zeilen-getrenntes JSON zu lesen |
| Einheitliches Streaming-Protokoll | Sicherstellen, dass Python-, JavaScript- und C#-Backends dasselbe Nachrichtenformat senden |
| Fortschreitende UI-Aktualisierungen | Agentenstatus-Abzeichen aktualisieren und Artikeltext Token für Token streamen |
| HTTP-Schicht zu einer CLI-App hinzufügen | Bestehende Orchestrierungslogik in einem Express-Stil Server (JS) oder ASP.NET Core Minimal-API (C#) einbinden |

---

## Architektur

Die UI ist eine einzige Sammlung von statischen Dateien (`index.html`, `style.css`, `app.js`), die von allen drei Backends gemeinsam genutzt wird. Jedes Backend stellt dieselben zwei Routen bereit:

![Zava UI-Architektur — gemeinsame Benutzeroberfläche mit drei Backends](../../../images/part12-architecture.svg)

| Route | Methode | Zweck |
|-------|--------|---------|
| `/` | GET | Dient die statische UI aus |
| `/api/article` | POST | Führt die Multi-Agenten-Pipeline aus und streamt NDJSON |

Die Benutzeroberfläche sendet einen JSON-Body und liest die Antwort als Stream zeilengetrennter JSON-Nachrichten. Jede Nachricht hat ein `type`-Feld, das die UI nutzt, um das richtige Panel zu aktualisieren:

| Nachrichtentyp | Bedeutung |
|-------------|---------|
| `message` | Statusaktualisierung (z. B. „Starting researcher agent task...“) |
| `researcher` | Forschungsergebnisse stehen bereit |
| `marketing` | Produktsuchergebnisse stehen bereit |
| `writer` | Writer gestartet oder abgeschlossen (enthält `{ start: true }` oder `{ complete: true }`) |
| `partial` | Ein einzeln gestreamtes Token vom Writer (enthält `{ text: "..." }`) |
| `editor` | Redakteursentscheid steht bereit |
| `error` | Etwas ist schiefgelaufen |

![Nachrichtentyp-Routing im Browser](../../../images/part12-message-types.svg)

![Streaming-Sequenz — Browser zu Backend-Kommunikation](../../../images/part12-streaming-sequence.svg)

---

## Voraussetzungen

- Abgeschlossen [Teil 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI installiert und das Modell `phi-3.5-mini` heruntergeladen
- Ein moderner Webbrowser (Chrome, Edge, Firefox oder Safari)

---

## Die gemeinsame UI

Bevor Sie Backend-Code ändern, nehmen Sie sich einen Moment, um die Benutzeroberfläche zu erkunden, die alle drei Sprachvarianten nutzen werden. Die Dateien befinden sich in `zava-creative-writer-local/ui/`:

| Datei | Zweck |
|------|---------|
| `index.html` | Seiten-Layout: Eingabeformular, Agentenstatus-Abzeichen, Ausgabebereich für Artikel, klappbare Detail-Panels |
| `style.css` | Minimales Styling mit Statusabzeichen-Farben (wartend, laufend, fertig, Fehler) |
| `app.js` | Fetch-Aufruf, `ReadableStream`-Zeilenleser und DOM-Aktualisierungslogik |

> **Tipp:** Öffnen Sie `index.html` direkt im Browser, um das Layout zu sehen. Noch funktioniert nichts, da kein Backend vorhanden ist, aber Sie sehen die Struktur.

### Funktionsweise des Stream-Readers

Die Schlüssel-Funktion in `app.js` liest den Antwort-Body Stück für Stück und teilt ihn an Zeilenumbrüchen auf:

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
    buffer = lines.pop(); // die unvollständige letzte Zeile beibehalten

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

Jede geparste Nachricht wird an `handleMessage()` übergeben, die das jeweilige DOM-Element anhand von `msg.type` aktualisiert.

---

## Übungen

### Übung 1: Führen Sie das Python-Backend mit der UI aus

Die Python (FastAPI) Variante hat bereits einen Streaming-API-Endpunkt. Die einzige Änderung ist das Einbinden des `ui/` Ordners als statische Dateien.

**1.1** Wechseln Sie in das Python-API-Verzeichnis und installieren Sie Abhängigkeiten:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Starten Sie den Server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Öffnen Sie Ihren Browser unter `http://localhost:8000`. Sie sollten die Zava Creative Writer UI mit drei Textfeldern und einem „Generate Article“-Button sehen.

**1.4** Klicken Sie auf **Generate Article** mit den Standardwerten. Beobachten Sie, wie die Agentenstatus-Abzeichen sich von „Waiting“ über „Running“ zu „Done“ ändern, während jeder Agent seine Aufgaben abschließt, und wie der Artikeltext Token für Token in das Ausgabefenster gestreamt wird.

> **Fehlersuche:** Wenn die Seite eine JSON-Antwort anzeigt statt der UI, stellen Sie sicher, dass Sie die aktualisierte `main.py` ausführen, die die statischen Dateien einbindet. Der `/api/article` Endpunkt funktioniert weiterhin unter dem ursprünglichen Pfad; das statische Mounting dient der UI auf allen anderen Routen.

**Funktionsweise:** Die aktualisierte `main.py` fügt eine Zeile am Ende hinzu:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Dies bedient jede Datei aus `zava-creative-writer-local/ui/` als statisches Asset, wobei `index.html` als Standarddokument dient. Die `/api/article` POST-Route ist vor dem statischen Mount registriert und hat Vorrang.

---

### Übung 2: Fügen Sie dem JavaScript-Variant einen Webserver hinzu

Die JavaScript-Variante ist derzeit eine CLI-Anwendung (`main.mjs`). Eine neue Datei `server.mjs` umgibt dieselben Agentenmodule mit einem HTTP-Server und dient der gemeinsamen UI.

**2.1** Wechseln Sie in das JavaScript-Verzeichnis und installieren Sie Abhängigkeiten:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Starten Sie den Webserver:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Sie sollten sehen:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Öffnen Sie `http://localhost:3000` in Ihrem Browser und klicken Sie auf **Generate Article**. Dieselbe UI funktioniert identisch gegen das JavaScript-Backend.

**Studieren Sie den Code:** Öffnen Sie `server.mjs` und beachten Sie die wichtigsten Muster:

- **Statische Dateien bedienen** verwendet die eingebauten Node.js-Module `http`, `fs` und `path` ohne externes Framework.
- **Pfad-Traversal-Schutz** normalisiert den angeforderten Pfad und prüft, dass er im `ui/` Verzeichnis bleibt.
- **NDJSON-Streaming** nutzt eine Hilfsfunktion `sendLine()`, die jedes Objekt serialisiert, interne Zeilenumbrüche entfernt und eine abschließende neue Zeile anhängt.
- **Agentenorchestrierung** nutzt unverändert die bestehenden Module `researcher.mjs`, `product.mjs`, `writer.mjs` und `editor.mjs`.

<details>
<summary>Wichtiger Auszug aus server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Forscher
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Schriftsteller (Streaming)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Übung 3: Fügen Sie der C#-Variante eine Minimal-API hinzu

Die C#-Variante ist derzeit eine Konsolenanwendung. Ein neues Projekt `csharp-web` verwendet ASP.NET Core Minimal-APIs, um dieselbe Pipeline als Webservice anzubieten.

**3.1** Wechseln Sie in das C#-Web-Projekt und stellen Sie Pakete wieder her:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Starten Sie den Webserver:

```bash
dotnet run
```

```powershell
dotnet run
```

Sie sollten sehen:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Öffnen Sie `http://localhost:5000` in Ihrem Browser und klicken Sie auf **Generate Article**.

**Studieren Sie den Code:** Öffnen Sie `Program.cs` im Verzeichnis `csharp-web` und beachten Sie:

- Die Projektdatei verwendet `Microsoft.NET.Sdk.Web` statt `Microsoft.NET.Sdk`, was ASP.NET Core unterstützt.
- Statische Dateien werden über `UseDefaultFiles` und `UseStaticFiles` angeboten, die auf das gemeinsame `ui/` Verzeichnis zeigen.
- Der `/api/article` Endpunkt schreibt NDJSON-Zeilen direkt in `HttpContext.Response` und spült nach jeder Zeile für Echtzeit-Streaming.
- Die gesamte Agenten-Logik (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) ist identisch zur Konsolenversion.

<details>
<summary>Wichtiger Auszug aus csharp-web/Program.cs</summary>

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

### Übung 4: Erkunden Sie die Agentenstatus-Abzeichen

Nun, da Sie eine funktionierende UI haben, sehen Sie sich an, wie das Frontend die Status-Abzeichen aktualisiert.

**4.1** Öffnen Sie `zava-creative-writer-local/ui/app.js` in Ihrem Editor.

**4.2** Finden Sie die Funktion `handleMessage()`. Beachten Sie, wie sie Nachrichtentypen DOM-Aktualisierungen zuordnet:

| Nachrichtentyp | UI-Aktion |
|-------------|-----------|
| `message` mit "researcher" | Setzt das Researcher-Abzeichen auf „Running“ |
| `researcher` | Setzt das Researcher-Abzeichen auf „Done“ und füllt das Research Results Panel |
| `marketing` | Setzt das Product Search Abzeichen auf „Done“ und füllt das Product Matches Panel |
| `writer` mit `data.start` | Setzt das Writer-Abzeichen auf „Running“ und leert die Artikelausgabe |
| `partial` | Fügt den Token-Text zur Artikelausgabe hinzu |
| `writer` mit `data.complete` | Setzt das Writer-Abzeichen auf „Done“ |
| `editor` | Setzt das Editor-Abzeichen auf „Done“ und füllt das Editor Feedback Panel |

**4.3** Öffnen Sie die aufklappbaren Panels „Research Results“, „Product Matches“ und „Editor Feedback“ unter dem Artikel, um die rohen JSON-Daten jedes Agenten zu inspizieren.

---

### Übung 5: UI anpassen (Stretch)

Probieren Sie eine oder mehrere der folgenden Verbesserungen:

**5.1 Zählen Sie Wörter.** Nach Abschluss des Writers zeigen Sie die Wortanzahl unterhalb des Ausgabefensters an. Sie können dies in `handleMessage` berechnen, wenn `type === "writer"` und `data.complete` wahr ist:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Fügen Sie einen Wiederholungsindikator hinzu.** Wenn der Editor eine Überarbeitung anfordert, läuft die Pipeline erneut. Zeigen Sie im Statuspanel ein Banner mit „Revision 1“ oder „Revision 2“ an. Hören Sie auf einen `message`-Typ mit „Revision“ und aktualisieren Sie ein neues DOM-Element.

**5.3 Dunkelmodus.** Fügen Sie einen Umschalter und eine `.dark`-Klasse zum `<body>` hinzu. Überschreiben Sie Hintergrund-, Text- und Panelfarben in `style.css` mit einem `body.dark`-Selektor.

---

## Zusammenfassung

| Was Sie gemacht haben | Wie |
|-------------|-----|
| UI wurde vom Python-Backend bereitgestellt | `ui/` Ordner mit `StaticFiles` in FastAPI eingebunden |
| HTTP-Server zur JavaScript-Variante hinzugefügt | `server.mjs` mit eingebautem Node.js `http`-Modul erstellt |
| Web-API zur C#-Variante hinzugefügt | Neues `csharp-web`-Projekt mit ASP.NET Core Minimal-APIs erstellt |
| Streaming NDJSON im Browser konsumiert | `fetch()` mit `ReadableStream` und zeilenweiser JSON-Zerlegung verwendet |
| UI in Echtzeit aktualisiert | Nachrichtentypen zu DOM-Aktualisierungen gemappt (Abzeichen, Text, klappbare Panels) |

---

## Wichtige Erkenntnisse

1. Eine **gemeinsame statische Benutzeroberfläche** kann mit jedem Backend funktionieren, das dasselbe Streaming-Protokoll spricht, was den Wert des OpenAI-kompatiblen API-Musters bestätigt.
2. **Zeilengetrenntes JSON (NDJSON)** ist ein einfaches Streaming-Format, das nativ mit der Browser-`ReadableStream` API funktioniert.
3. Die **Python-Variante** benötigte die wenigsten Änderungen, da sie bereits einen FastAPI-Endpunkt hatte; JavaScript und C# benötigten eine dünne HTTP-Hülle.
4. Die Benutzeroberfläche als **Vanilla HTML/CSS/JS** zu belassen, vermeidet Build-Tools, Framework-Abhängigkeiten und zusätzliche Komplexität für Workshop-Teilnehmer.
5. Dieselben Agentenmodule (Researcher, Product, Writer, Editor) werden unverändert wiederverwendet; nur die Transportschicht ändert sich.

---

## Weiterführende Literatur

| Ressource | Link |
|----------|------|
| MDN: Verwendung von Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI statische Dateien | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core statische Dateien | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Spezifikation | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Fahren Sie fort zu [Teil 13: Workshop abgeschlossen](part13-workshop-complete.md) für eine Zusammenfassung von allem, was Sie im Workshop aufgebaut haben.

---
[← Teil 11: Werkzeugaufruf](part11-tool-calling.md) | [Teil 13: Workshop abgeschlossen →](part13-workshop-complete.md)