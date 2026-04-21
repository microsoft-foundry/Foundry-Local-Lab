![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Μέρος 12: Δημιουργία Web UI για τον Zava Creative Writer

> **Στόχος:** Προσθήκη ενός browser-based front end στον Zava Creative Writer ώστε να μπορείς να παρακολουθείς την εκτέλεση της multi-agent ροής εργασίας σε πραγματικό χρόνο, με ζωντανά σήματα κατάστασης των agents και ροή κειμένου άρθρου, όλα εξυπηρετούμενα από έναν μόνο τοπικό web server.

Στο [Μέρος 7](part7-zava-creative-writer.md) εξερεύνησες τον Zava Creative Writer ως **CLI εφαρμογή** (JavaScript, C#) και ως **headless API** (Python). Σε αυτό το εργαστήριο θα συνδέσεις ένα κοινό **vanilla HTML/CSS/JavaScript** front end σε κάθε backend έτσι ώστε οι χρήστες να μπορούν να αλληλεπιδρούν με τη ροή εργασίας μέσα από έναν φυλλομετρητή αντί με το τερματικό.

---

## Τι θα μάθετε

| Στόχος | Περιγραφή |
|---------|-----------|
| Εξυπηρέτηση στατικών αρχείων από backend | Προσαρμογή HTML/CSS/JS φακέλου δίπλα στη διαδρομή API |
| Κατανάλωση streaming NDJSON μέσα στο browser | Χρήση Fetch API με `ReadableStream` για ανάγνωση newline-delimited JSON |
| Ενοποιημένο streaming πρωτόκολλο | Διασφάλιση ότι Python, JavaScript και C# backends εκπέμπουν το ίδιο φορμά μηνυμάτων |
| Προοδευτικές ενημερώσεις UI | Ενημέρωση status badges των agents και ροή του κειμένου άρθρου token προς token |
| Προσθήκη HTTP layer σε CLI εφαρμογή | Ενσωμάτωση του υπάρχοντος orchestrator σε Express-style server (JS) ή ASP.NET Core minimal API (C#) |

---

## Αρχιτεκτονική

Το UI αποτελείται από ένα σύνολο στατικών αρχείων (`index.html`, `style.css`, `app.js`) που μοιράζονται και τα τρία backend. Κάθε backend εκθέτει τις ίδιες δύο διαδρομές:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| Διαδρομή | Μέθοδος | Σκοπός |
|----------|---------|---------|
| `/` | GET | Εξυπηρετεί το στατικό UI |
| `/api/article` | POST | Εκτελεί τη multi-agent ροή εργασίας και streamάρει NDJSON |

Το front end στέλνει σώμα JSON και διαβάζει την απάντηση ως ροή από newline-delimited JSON μηνύματα. Κάθε μήνυμα έχει πεδίο `type` που χρησιμοποιείται από το UI για να ενημερώσει τη σωστή περιοχή:

| Τύπος μηνύματος | Σημασία |
|-----------------|---------|
| `message` | Ενημέρωση κατάστασης (π.χ. "Starting researcher agent task...") |
| `researcher` | Έτοιμα αποτελέσματα έρευνας |
| `marketing` | Έτοιμα αποτελέσματα αναζήτησης προϊόντος |
| `writer` | Ο writer ξεκίνησε ή τελείωσε (περιέχει `{ start: true }` ή `{ complete: true }`) |
| `partial` | Ένα μεμονωμένο token που streamάρεται από τον Writer (περιέχει `{ text: "..." }`) |
| `editor` | Η απόφαση του editor είναι έτοιμη |
| `error` | Κάτι πήγε στραβά |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## Προαπαιτούμενα

- Ολοκληρώστε το [Μέρος 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Εγκατεστημένο Foundry Local CLI και το μοντέλο `phi-3.5-mini` κατεβασμένο
- Ένας σύγχρονος φυλλομετρητής (Chrome, Edge, Firefox, ή Safari)

---

## Το Κοινό UI

Πριν πειράξετε κάποιο backend κώδικα, αφιερώστε λίγο χρόνο να εξερευνήσετε το front end που θα χρησιμοποιήσουν και οι τρεις γλώσσες. Τα αρχεία βρίσκονται στο `zava-creative-writer-local/ui/`:

| Αρχείο | Σκοπός |
|---------|---------|
| `index.html` | Διάταξη σελίδας: φόρμα εισαγωγής, σήματα κατάστασης agents, περιοχή εξόδου άρθρου, αναδιπλούμενες λεπτομερείς καρτέλες |
| `style.css` | Ελάχιστο στυλ με χρωματιστά state badges (αναμονή, εκτέλεση, ολοκλήρωση, λάθος) |
| `app.js` | Κλήση fetch, `ReadableStream` line reader, και λογική ενημέρωσης DOM |

> **Συμβουλή:** Άνοιξε το `index.html` απευθείας στον browser σου για προεπισκόπηση της διάταξης. Δεν θα λειτουργήσει ακόμη γιατί δεν υπάρχει backend, αλλά μπορείς να δεις τη δομή.

### Πώς λειτουργεί ο Stream Reader

Η βασική συνάρτηση στο `app.js` διαβάζει το σώμα απόκρισης κομμάτι-κομμάτι και χωρίζει στα σημεία αλλαγής γραμμής:

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
    buffer = lines.pop(); // διατήρησε την ελλιπή τελευταία γραμμή

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

Κάθε ανάλυση μηνύματος στέλνεται στη `handleMessage()`, που ενημερώνει το σχετικό DOM στοιχείο με βάση το `msg.type`.

---

## Ασκήσεις

### Άσκηση 1: Εκτέλεση του Python Backend με το UI

Η παραλλαγή Python (FastAPI) έχει ήδη endpoint streaming API. Η μόνη αλλαγή είναι η προσάρτηση του φακέλου `ui/` ως στατικών αρχείων.

**1.1** Μεταβείτε στο φάκελο Python API και εγκαταστήστε τις εξαρτήσεις:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Ξεκινήστε τον server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Ανοίξτε το πρόγραμμα περιήγησης στη διεύθυνση `http://localhost:8000`. Πρέπει να δείτε το UI του Zava Creative Writer με τρία πεδία κειμένου και ένα κουμπί "Generate Article".

**1.4** Κάντε κλικ στο **Generate Article** χρησιμοποιώντας τις προεπιλεγμένες τιμές. Παρακολουθήστε τα agent status badges να αλλάζουν από "Waiting" σε "Running" και μετά σε "Done" καθώς κάθε agent ολοκληρώνει, και δείτε το κείμενο του άρθρου να εμφανίζεται token προς token στον πίνακα εξόδου.

> **Επίλυση Προβλημάτων:** Αν η σελίδα εμφανίζει JSON αντί για το UI, βεβαιωθείτε ότι τρέχετε το ενημερωμένο αρχείο `main.py` που προσαρτά τα στατικά αρχεία. Το endpoint `/api/article` λειτουργεί ακόμα στη βασική του διαδρομή, το mount των στατικών αρχείων εξυπηρετεί το UI σε όλες τις άλλες διαδρομές.

**Πώς δουλεύει:** Το ανανεωμένο `main.py` προσθέτει μία γραμμή στο τέλος:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Αυτό εξυπηρετεί όλα τα αρχεία από τον φάκελο `zava-creative-writer-local/ui/` ως στατικά assets, με το `index.html` ως το προεπιλεγμένο αρχείο. Η διαδρομή POST `/api/article` είναι πρωτύτερα εγγεγραμμένη ώστε να έχει προτεραιότητα.

---

### Άσκηση 2: Προσθήκη Web Server στην παραλλαγή JavaScript

Η παραλλαγή JavaScript είναι προς το παρόν CLI εφαρμογή (`main.mjs`). Ένα νέο αρχείο, `server.mjs`, περιτυλίγει τους ίδιους agent modules πίσω από έναν HTTP server και εξυπηρετεί το κοινό UI.

**2.1** Μεταβείτε στον φάκελο JavaScript και εγκαταστήστε τις εξαρτήσεις:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Ξεκινήστε τον web server:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Πρέπει να δείτε:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Ανοίξτε `http://localhost:3000` στον browser και κάντε κλικ στο **Generate Article**. Το ίδιο UI λειτουργεί ακριβώς το ίδιο με το JavaScript backend.

**Μελετήστε τον κώδικα:** Ανοίξτε το `server.mjs` και παρατηρήστε τα βασικά μοτίβα:

- Η εξυπηρέτηση στατικών αρχείων γίνεται με τα ενσωματωμένα modules Node.js `http`, `fs` και `path` χωρίς εξωτερικά frameworks.
- Η προστασία από path-traversal κανονικοποιεί το ζητούμενο path και ελέγχει ότι παραμένει μέσα στον φάκελο `ui/`.
- Το streaming NDJSON χρησιμοποιεί την βοηθητική συνάρτηση `sendLine()` που σειριοποιεί κάθε αντικείμενο, αφαιρεί εσωτερικά newlines και προσθέτει τελικό newline.
- Η ορχήστρωση των agents ξαναχρησιμοποιεί τους υπάρχοντες modules `researcher.mjs`, `product.mjs`, `writer.mjs`, `editor.mjs` χωρίς αλλαγές.

<details>
<summary>Αποσπάσμα κώδικα από server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Ερευνητής
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Συγγραφέας (δοχειοποίηση)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Άσκηση 3: Προσθήκη Minimal API στην παραλλαγή C#

Η παραλλαγή C# είναι προς το παρόν εφαρμογή κονσόλας. Ένα νέο project, `csharp-web`, χρησιμοποιεί ASP.NET Core minimal APIs για να εκθέσει την ίδια ροή εργασίας ως web service.

**3.1** Μεταβείτε στο C# web project και επαναφέρετε τα πακέτα:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Τρέξτε τον web server:

```bash
dotnet run
```

```powershell
dotnet run
```

Πρέπει να δείτε:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Ανοίξτε `http://localhost:5000` στον browser και κάντε κλικ στο **Generate Article**.

**Μελετήστε τον κώδικα:** Ανοίξτε το `Program.cs` στο φάκελο `csharp-web` και παρατηρήστε:

- Το project file χρησιμοποιεί `Microsoft.NET.Sdk.Web` αντί για `Microsoft.NET.Sdk` που προσθέτει υποστήριξη ASP.NET Core.
- Τα στατικά αρχεία εξυπηρετούνται με `UseDefaultFiles` και `UseStaticFiles` που δείχνουν στον κοινό φάκελο `ui/`.
- Το `/api/article` endpoint γράφει NDJSON γραμμές απευθείας σε `HttpContext.Response` και αδειάζει το buffer μετά από κάθε γραμμή για streaming σε πραγματικό χρόνο.
- Όλη η λογική των agents (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) είναι η ίδια με την έκδοση κονσόλας.

<details>
<summary>Απόσπασμα από csharp-web/Program.cs</summary>

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

### Άσκηση 4: Εξερεύνηση των Agent Status Badges

Τώρα που έχετε λειτουργικό UI, δείτε πώς το front end ενημερώνει τα status badges.

**4.1** Ανοίξτε το `zava-creative-writer-local/ui/app.js` στον επεξεργαστή σας.

**4.2** Βρείτε τη συνάρτηση `handleMessage()`. Παρατηρήστε πως αντιστοιχίζει τους τύπους μηνυμάτων σε ενημερώσεις DOM:

| Τύπος μηνύματος | Ενέργεια UI |
|-----------------|-------------|
| `message` που περιέχει "researcher" | Θέτει το badge Researcher σε "Running" |
| `researcher` | Θέτει το badge Researcher σε "Done" και εμφανίζει τα αποτελέσματα έρευνας |
| `marketing` | Θέτει το badge Product Search σε "Done" και εμφανίζει τα αποτελέσματα προϊόντων |
| `writer` με `data.start` | Θέτει το badge Writer σε "Running" και καθαρίζει την έξοδο κειμένου |
| `partial` | Προσθέτει το token κειμένου στην έξοδο άρθρου |
| `writer` με `data.complete` | Θέτει το badge Writer σε "Done" |
| `editor` | Θέτει το badge Editor σε "Done" και εμφανίζει τα σχόλια του Editor |

**4.3** Ανοίξτε τους αναδιπλούμενους πίνακες "Research Results", "Product Matches", και "Editor Feedback" κάτω από το άρθρο για να ελέγξετε το ακατέργαστο JSON που παρήγαγε κάθε agent.

---

### Άσκηση 5: Προσαρμογή UI (Επέκταση)

Δοκιμάστε μία ή περισσότερες από τις παρακάτω βελτιώσεις:

**5.1 Προσθήκη μέτρησης λέξεων.** Μετά το τέλος του Writer, εμφανίστε τον αριθμό λέξεων του άρθρου κάτω από τον πίνακα εξόδου. Μπορείτε να υπολογίσετε αυτό μέσα στη `handleMessage` όταν `type === "writer"` και `data.complete` είναι true:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Προσθήκη δείκτη επανάληψης.** Όταν ο Editor ζητά αναθεώρηση, η pipeline ξανατρέχει. Εμφανίστε ένα πλαίσιο "Revision 1" ή "Revision 2" στο πάνελ κατάστασης. Ακούστε για `message` τύπο που περιέχει "Revision" και ενημερώστε νέο στοιχείο DOM.

**5.3 Dark mode.** Προσθέστε ένα κουμπί ενεργοποίησης dark mode και κλάση `.dark` στο `<body>`. Αντικαταστήστε τα χρώματα φόντου, κειμένου και πάνελ μέσα στο `style.css` με έναν selector `body.dark`.

---

## Περίληψη

| Τι κάνατε | Πώς |
|-----------|-----|
| Εξυπηρετήσατε το UI από το Python backend | Προσαρτήσατε τον φάκελο `ui/` με το `StaticFiles` στο FastAPI |
| Προσθέσατε HTTP server στην JavaScript παραλλαγή | Δημιουργήσατε το `server.mjs` με built-in Node.js `http` module |
| Προσθέσατε web API στην C# παραλλαγή | Δημιουργήσατε νέο `csharp-web` project με ASP.NET Core minimal APIs |
| Καταναλώσατε streaming NDJSON στον browser | Χρησιμοποιήσατε `fetch()` με `ReadableStream` και γραμμή-γραμμή ανάλυση JSON |
| Ενημερώσατε το UI σε πραγματικό χρόνο | Αντιστοιχίσατε τύπους μηνυμάτων σε ενημερώσεις DOM (badges, κείμενο, πάνελ) |

---

## Σημαντικά Μαθήματα

1. Ένα **κοινό στατικό front end** μπορεί να συνεργαστεί με οποιοδήποτε backend που μιλάει το ίδιο streaming πρωτόκολλο, ενισχύοντας την αξία του OpenAI-συμβατού API pattern.
2. Το **newline-delimited JSON (NDJSON)** είναι μια απλή μορφή streaming που δουλεύει εγγενώς με το browser `ReadableStream` API.
3. Η **Python παραλλαγή** απαιτούσε τις λιγότερες αλλαγές γιατί είχε ήδη endpoint FastAPI· οι παραλλαγές JS και C# χρειάστηκαν λεπτό HTTP wrapper.
4. Η διατήρηση του UI ως **vanilla HTML/CSS/JS** αποφεύγει εργαλεία build, εξαρτήσεις framework και επιπλέον πολυπλοκότητα για τους μαθητές.
5. Τα ίδια modules agents (Researcher, Product, Writer, Editor) επαναχρησιμοποιούνται χωρίς τροποποιήσεις· μόνο το επίπεδο μεταφοράς αλλάζει.

---

## Περισσότερη Ανάγνωση

| Πόρος | Σύνδεσμος |
|--------|-----------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Συνεχίστε στο [Μέρος 13: Ολοκλήρωση Εργαστηρίου](part13-workshop-complete.md) για περίληψη όλων όσων δημιουργήσατε σε αυτό το εργαστήριο.

---
[← Μέρος 11: Κλήση Εργαλείου](part11-tool-calling.md) | [Μέρος 13: Ολοκλήρωση Εργαστηρίου →](part13-workshop-complete.md)