![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Część 12: Tworzenie interfejsu Web UI dla Zava Creative Writer

> **Cel:** Dodaj front end działający w przeglądarce do Zava Creative Writer, aby móc na bieżąco obserwować działanie potoków wieloagentowych, ze statusami agentów na żywo i przesyłanym strumieniowo tekstem artykułu, serwowanymi z jednego lokalnego serwera.

W [Części 7](part7-zava-creative-writer.md) zapoznałeś się z Zava Creative Writer jako **aplikacją CLI** (JavaScript, C#) oraz jako **bezgłowym API** (Python). W tym laboratorium połączysz wspólny frontend **vanilla HTML/CSS/JavaScript** z każdym backendem, aby użytkownicy mogli wchodzić w interakcję z pipeline’em przez przeglądarkę, a nie terminal.

---

## Czego się nauczysz

| Cel | Opis |
|-----------|-------------|
| Serwowanie plików statycznych z backendu | Zamontowanie katalogu HTML/CSS/JS obok trasy API |
| Konsumpcja strumieniowego NDJSON w przeglądarce | Wykorzystanie Fetch API z `ReadableStream` do czytania JSON rozdzielonego znakami nowej linii |
| Ujednolicony protokół strumieniowy | Zapewnienie, że backendy Python, JavaScript i C# emitują ten sam format wiadomości |
| Postępowe aktualizacje UI | Aktualizowanie statusów agentów i strumieniowe przesyłanie tekstu artykułu token po tokenie |
| Dodanie warstwy HTTP do aplikacji CLI | Opakowanie istniejącej logiki orkiestracji w serwer w stylu Express (JS) lub minimalne API ASP.NET Core (C#) |

---

## Architektura

UI to pojedynczy zestaw plików statycznych (`index.html`, `style.css`, `app.js`) współdzielonych przez wszystkie trzy backendy. Każdy backend udostępnia te same dwie trasy:

![Architektura UI Zava — wspólny front end z trzema backendami](../../../images/part12-architecture.svg)

| Trasa | Metoda | Cel |
|-------|--------|---------|
| `/` | GET | Serwuje statyczne UI |
| `/api/article` | POST | Uruchamia potok wieloagentowy i strumieniuje NDJSON |

Frontend wysyła ciało JSON i odczytuje odpowiedź jako strumień rozdzielonych znakami nowej linii wiadomości JSON. Każda wiadomość ma pole `type`, które UI używa do aktualizacji odpowiedniego panelu:

| Typ wiadomości | Znaczenie |
|-------------|---------|
| `message` | Aktualizacja statusu (np. "Rozpoczynam zadanie agenta badacza...") |
| `researcher` | Wyniki badania są gotowe |
| `marketing` | Wyniki wyszukiwania produktów są gotowe |
| `writer` | Pisarz rozpoczął lub zakończył pracę (zawiera `{ start: true }` lub `{ complete: true }`) |
| `partial` | Pojedynczy przepływający token z Pisarza (zawiera `{ text: "..." }`) |
| `editor` | Werdykt redaktora jest gotowy |
| `error` | Coś poszło nie tak |

![Routing typów wiadomości w przeglądarce](../../../images/part12-message-types.svg)

![Sekwencja strumieniowania — komunikacja przeglądarki z backendem](../../../images/part12-streaming-sequence.svg)

---

## Wymagania wstępne

- Ukończ [Część 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Zainstalowany Foundry Local CLI i pobrany model `phi-3.5-mini`
- Nowoczesna przeglądarka internetowa (Chrome, Edge, Firefox lub Safari)

---

## Wspólne UI

Zanim dotkniesz backendu, poświęć chwilę na zapoznanie się z frontendem, którego używają wszystkie trzy wersje językowe. Pliki znajdują się w `zava-creative-writer-local/ui/`:

| Plik | Cel |
|------|---------|
| `index.html` | Układ strony: formularz wejściowy, odznaki statusu agentów, obszar wyjścia artykułu, rozkładane panele szczegółów |
| `style.css` | Minimalne style z kolorami statusów odznak (oczekiwanie, działanie, zakończone, błąd) |
| `app.js` | Wywołanie Fetch, czytnik linii `ReadableStream`, logika aktualizacji DOM |

> **Wskazówka:** Otwórz `index.html` bezpośrednio w przeglądarce, aby zobaczyć podgląd układu. Nic jeszcze nie zadziała, bo brak backendu, ale możesz sprawdzić strukturę.

### Jak działa czytnik strumienia

Kluczowa funkcja w `app.js` czyta ciało odpowiedzi kawałek po kawałku i dzieli na linie:

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
    buffer = lines.pop(); // zachowaj niekompletną kończąca linię

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

Każda przetworzona wiadomość jest przekazywana do `handleMessage()`, która aktualizuje odpowiedni element DOM na podstawie `msg.type`.

---

## Ćwiczenia

### Ćwiczenie 1: Uruchom backend Pythona z UI

Wariant Python (FastAPI) ma już endpoint API ze strumieniem. Jedyną zmianą jest zamontowanie folderu `ui/` jako plików statycznych.

**1.1** Przejdź do katalogu API Pythona i zainstaluj zależności:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Uruchom serwer:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Otwórz przeglądarkę pod adresem `http://localhost:8000`. Powinieneś zobaczyć UI Zava Creative Writer z trzema polami tekstowymi i przyciskiem "Generate Article".

**1.4** Kliknij **Generate Article** używając domyślnych wartości. Obserwuj, jak statusy agentów zmieniają się z "Waiting" na "Running" i "Done", gdy każdy agent kończy swoją pracę, a tekst artykułu strumieniowo pojawia się w panelu wyjściowym token po tokenie.

> **Rozwiązywanie problemów:** Jeśli zamiast UI strona pokazuje odpowiedź JSON, upewnij się, że uruchamiasz zaktualizowany `main.py`, który montuje pliki statyczne. Endpoint `/api/article` działa nadal pod oryginalną ścieżką; montowanie plików statycznych serwuje UI pod wszystkimi innymi trasami.

**Jak to działa:** Zaktualizowany `main.py` dodaje jeden wiersz na dole:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

To serwuje wszystkie pliki z `zava-creative-writer-local/ui/` jako zasoby statyczne, z `index.html` jako domyślnym dokumentem. Trasa POST `/api/article` jest zarejestrowana przed montowaniem plików statycznych, więc ma wyższy priorytet.

---

### Ćwiczenie 2: Dodaj serwer WWW do wariantu JavaScript

Wariant JavaScript jest aktualnie aplikacją CLI (`main.mjs`). Nowy plik, `server.mjs`, opakowuje te same moduły agentów za HTTP i serwuje współdzielone UI.

**2.1** Przejdź do katalogu JavaScript i zainstaluj zależności:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Uruchom serwer webowy:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Powinieneś zobaczyć:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Otwórz `http://localhost:3000` w przeglądarce i kliknij **Generate Article**. To samo UI działa identycznie z backendem JavaScript.

**Zbadaj kod:** Otwórz `server.mjs` i zwróć uwagę na kluczowe wzorce:

- **Serwowanie plików statycznych** używa natywnych modułów Node.js `http`, `fs` i `path` bez zewnętrznych frameworków.
- **Ochrona przed path-traversal** normalizuje żądaną ścieżkę i sprawdza czy pozostaje w katalogu `ui/`.
- **Strumieniowanie NDJSON** korzysta z pomocniczej funkcji `sendLine()`, która serializuje każdy obiekt, usuwa wewnętrzne znaki nowej linii i dopisuje znak nowej linii na końcu.
- **Orkiestracja agentów** ponownie wykorzystuje istniejące moduły `researcher.mjs`, `product.mjs`, `writer.mjs` i `editor.mjs` bez zmian.

<details>
<summary>Kluczowy fragment z server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Badacz
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Pisarz (streaming)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Ćwiczenie 3: Dodaj minimalne API do wariantu C#

Wariant C# jest obecnie aplikacją konsolową. Nowy projekt `csharp-web` używa minimalnych API ASP.NET Core, aby udostępnić ten sam pipeline jako usługę webową.

**3.1** Przejdź do projektu webowego C# i przywróć pakiety:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Uruchom serwer webowy:

```bash
dotnet run
```

```powershell
dotnet run
```

Powinieneś zobaczyć:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Otwórz `http://localhost:5000` w przeglądarce i kliknij **Generate Article**.

**Zbadaj kod:** Otwórz `Program.cs` w katalogu `csharp-web` i zwróć uwagę na:

- Plik projektu używa `Microsoft.NET.Sdk.Web` zamiast `Microsoft.NET.Sdk`, co dodaje wsparcie dla ASP.NET Core.
- Pliki statyczne są serwowane przez `UseDefaultFiles` i `UseStaticFiles` wskazujące na wspólny katalog `ui/`.
- Endpoint `/api/article` bezpośrednio zapisuje linie NDJSON do `HttpContext.Response` i po każdej linii wywołuje flush dla strumieniowania w czasie rzeczywistym.
- Cała logika agentów (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) jest taka sama jak w wersji konsolowej.

<details>
<summary>Kluczowy fragment z csharp-web/Program.cs</summary>

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

### Ćwiczenie 4: Poznaj odznaki statusu agentów

Mając już działające UI, zobacz jak frontend aktualizuje odznaki statusu.

**4.1** Otwórz `zava-creative-writer-local/ui/app.js` w edytorze.

**4.2** Znajdź funkcję `handleMessage()`. Zwróć uwagę, jak mapuje typy wiadomości na aktualizacje DOM:

| Typ wiadomości | Akcja w UI |
|-------------|-----------|
| `message` zawierający "researcher" | Ustawia odznakę Researcher na "Running" |
| `researcher` | Ustawia odznakę Researcher na "Done" i wypełnia panel wyników badań |
| `marketing` | Ustawia odznakę Product Search na "Done" i wypełnia panel dopasowań produktów |
| `writer` z `data.start` | Ustawia odznakę Writer na "Running" oraz czyści wyjście artykułu |
| `partial` | Dopisuje tekst tokenu do wyjścia artykułu |
| `writer` z `data.complete` | Ustawia odznakę Writer na "Done" |
| `editor` | Ustawia odznakę Editor na "Done" i wypełnia panel opinii redaktora |

**4.3** Otwórz rozkładane panele "Research Results", "Product Matches" i "Editor Feedback" poniżej artykułu, by przejrzeć surowe JSON-y każdego agenta.

---

### Ćwiczenie 5: Dostosuj UI (Dodatkowo)

Wypróbuj jedno lub więcej z poniższych usprawnień:

**5.1 Dodaj licznik słów.** Po zakończeniu pracy Pisarza wyświetl liczbę słów artykułu poniżej panelu wyjściowego. Możesz to obliczyć w `handleMessage`, gdy `type === "writer"` oraz `data.complete` jest prawdziwe:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Dodaj wskaźnik ponowienia.** Gdy Redaktor żąda poprawki, pipeline jest uruchamiany ponownie. Pokaż baner "Revision 1" lub "Revision 2" na panelu statusu. Nasłuchuj wiadomości typu `message` zawierającej "Revision" i aktualizuj nowy element DOM.

**5.3 Tryb ciemny.** Dodaj przycisk przełączania i klasę `.dark` do `<body>`. Nadpisz kolory tła, tekstu i paneli w `style.css` selektorem `body.dark`.

---

## Podsumowanie

| Co zrobiłeś | Jak |
|-------------|-----|
| Serwowałeś UI z backendu Pythona | Zamontowałeś folder `ui/` używając `StaticFiles` w FastAPI |
| Dodałeś serwer HTTP do wariantu JavaScript | Utworzyłeś `server.mjs` używając natywnego modułu Node.js `http` |
| Dodałeś web API do wariantu C# | Utworzyłeś nowy projekt `csharp-web` z minimalnymi API ASP.NET Core |
| Skonsumowałeś strumieniowe NDJSON w przeglądarce | Użyłeś `fetch()` z `ReadableStream` i parsowaniem JSON linia po linii |
| Aktualizowałeś UI w czasie rzeczywistym | Mapowałeś typy wiadomości na aktualizacje DOM (odznaki, tekst, rozkładane panele) |

---

## Kluczowe wnioski

1. **Wspólny statyczny frontend** może działać z dowolnym backendem, który stosuje ten sam protokół strumieniowy, co podkreśla wartość wzorca API kompatybilnego z OpenAI.
2. **JSON rozdzielony znakami nowej linii (NDJSON)** to prosty format strumieniowy, który działa natywnie z interfejsem `ReadableStream` w przeglądarce.
3. Wariant **Python** wymagał najmniejszej zmiany, bo miał już endpoint FastAPI; warianty JavaScript i C# potrzebowały cienkiej warstwy HTTP.
4. Utrzymywanie UI jako **vanilla HTML/CSS/JS** eliminuje potrzebę użycia narzędzi buildujących, zależności frameworków oraz dodatkowej złożoności dla uczestników warsztatów.
5. Te same moduły agentów (Researcher, Product, Writer, Editor) są używane bez modyfikacji; zmienia się tylko warstwa transportowa.

---

## Dalsza lektura

| Zasób | Link |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Specyfikacja NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Kontynuuj do [Część 13: Warsztat zakończony](part13-workshop-complete.md) po podsumowanie wszystkiego, co zbudowałeś podczas tych warsztatów.

---
[← Część 11: Wywoływanie narzędzi](part11-tool-calling.md) | [Część 13: Warsztat ukończony →](part13-workshop-complete.md)