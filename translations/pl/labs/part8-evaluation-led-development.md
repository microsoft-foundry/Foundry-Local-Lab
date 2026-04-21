![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Część 8: Rozwój sterowany ewaluacją z Foundry Local

> **Cel:** Zbudować ramy ewaluacyjne, które systematycznie testują i oceniają twoich agentów AI, używając tego samego lokalnego modelu zarówno jako agenta testowanego, jak i sędziego, abyś mógł z przekonaniem iterować nad promptami przed wdrożeniem.

## Dlaczego rozwój sterowany ewaluacją?

Podczas tworzenia agentów AI „wygląda dobrze” nie wystarcza. **Rozwój sterowany ewaluacją** traktuje wyniki agenta jak kod: najpierw piszesz testy, mierzysz jakość i wysyłasz dopiero, gdy wyniki osiągną próg.

W Zava Creative Writer (Część 7) **Agent Edytor** działa już jako lekki ewaluator; decyduje AKCEPTUJ lub POPRAW. Ten warsztat formalizuje ten wzorzec do powtarzalnych ram ewaluacji, które można zastosować do dowolnego agenta lub pipeline'u.

| Problem | Rozwiązanie |
|---------|-------------|
| Zmiany promptów cicho psują jakość | **Złoty zbiór danych** wykrywa regresje |
| Stronniczość "działa na jednym przykładzie" | **Wiele przypadków testowych** ujawnia przypadki brzegowe |
| Subiektywna ocena jakości | **Ocenianie regułowe + LLM jako sędzia** dostarcza liczb |
| Brak możliwości porównania wariantów promptów | **Ewaluacje obok siebie** z ocenami zbiorczymi |

---

## Kluczowe pojęcia

### 1. Złote zbiory danych

**Złoty zbiór danych** to wyselekcjonowany zestaw przypadków testowych z znanymi spodziewanymi wynikami. Każdy przypadek testowy zawiera:

- **Wejście**: Prompt lub pytanie wysyłane do agenta
- **Oczekiwany wynik**: Co powinna zawierać poprawna lub wysokiej jakości odpowiedź (słowa kluczowe, struktura, fakty)
- **Kategoria**: Grupowanie do raportowania (np. „dokładność faktualna”, „ton”, „kompletność”)

### 2. Kontrole regułowe

Szybkie, deterministyczne kontrole, które nie wymagają LLM:

| Kontrola | Co testuje |
|----------|------------|
| **Granice długości** | Odpowiedź nie jest zbyt krótka (leniwa) ani zbyt długa (rozprawna) |
| **Wymagane słowa kluczowe** | Odpowiedź zawiera oczekiwane terminy lub byty |
| **Walidacja formatu** | JSON jest parsowalny, obecne są nagłówki Markdown |
| **Zabronione treści** | Brak zmyślonych nazw marek, brak wzmiankowań konkurencji |

### 3. LLM jako sędzia

Użyj **tego samego lokalnego modelu** do oceniania własnych wyników (lub wyników innego wariantu promptu). Sędzia otrzymuje:

- Oryginalne pytanie
- Odpowiedź agenta
- Kryteria oceniania

I zwraca ustrukturyzowaną ocenę. To odzwierciedla wzorzec Edytora z Części 7, ale stosowane systematycznie w całym zestawie testowym.

### 4. Pętla iteracji sterowana ewaluacją

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Wymagania wstępne

| Wymaganie | Szczegóły |
|-----------|-----------|
| **Foundry Local CLI** | Zainstalowany z pobranym modelem |
| **Środowisko uruchomieniowe** | **Python 3.9+** i/lub **Node.js 18+** i/lub **.NET 9+ SDK** |
| **Ukończone** | [Część 5: Pojedyncze Agenty](part5-single-agents.md) oraz [Część 6: Wieloagentowe Workflow](part6-multi-agent-workflows.md) |

---

## Ćwiczenia warsztatowe

### Ćwiczenie 1 - Uruchom ramy ewaluacyjne

Warsztat zawiera kompletny przykład ewaluacji, który testuje agenta Foundry Local na podstawie złotego zbioru danych z pytaniami związanymi z Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Konfiguracja:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Uruchom:**
```bash
python foundry-local-eval.py
```

**Co się dzieje:**
1. Łączy się z Foundry Local i ładuje model
2. Definiuje złoty zbiór danych z 5 przypadkami testowymi (pytania produktowe Zava)
3. Uruchamia dwa warianty promptów dla każdego przypadku testowego
4. Ocena każdej odpowiedzi za pomocą **kontroli regułowych** (długość, słowa kluczowe, format)
5. Ocena każdej odpowiedzi za pomocą **LLM jako sędzia** (ten sam model ocenia jakość w skali 1-5)
6. Drukuje kartę wyników porównującą oba warianty promptów

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Konfiguracja:**
```bash
cd javascript
npm install
```

**Uruchom:**
```bash
node foundry-local-eval.mjs
```

**Ta sama ścieżka ewaluacyjna:** złoty zbiór danych, podwójne uruchomienia promptów, oceny regułowe + LLM, karta wyników.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Konfiguracja:**
```bash
cd csharp
dotnet restore
```

**Uruchom:**
```bash
dotnet run eval
```

**Ta sama ścieżka ewaluacyjna:** złoty zbiór danych, podwójne uruchomienia promptów, oceny regułowe + LLM, karta wyników.

</details>

---

### Ćwiczenie 2 - Poznaj złoty zbiór danych

Przeanalizuj przypadki testowe zdefiniowane w przykładzie ewaluacyjnym. Każdy przypadek testowy zawiera:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Pytania do rozważenia:**
1. Dlaczego oczekiwane wartości to **słowa kluczowe** zamiast pełnych zdań?
2. Ile przypadków testowych potrzebujesz do wiarygodnej ewaluacji?
3. Jakie kategorie dodałbyś do własnej aplikacji?

---

### Ćwiczenie 3 - Poznaj różnicę między oceną regułową a LLM jako sędzią

Ramy ewaluacji wykorzystują **dwie warstwy oceniania**:

#### Kontrole regułowe (szybkie, deterministyczne)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM jako sędzia (subtelne, jakościowe)

Ten sam lokalny model działa jako sędzia z ustrukturyzowanym rubrykiem:

```
Rate this response on a scale of 1-5:
- 1: Completely wrong or irrelevant
- 2: Partially correct but missing key information
- 3: Adequate but could be improved
- 4: Good response with minor issues
- 5: Excellent, comprehensive, well-structured

Score: 4
Reasoning: The response correctly identifies all necessary tools
and provides practical advice, but could include safety equipment.
```

**Pytania do rozważenia:**
1. Kiedy bardziej ufałbyś kontrolom regułowym niż LLM jako sędziemu?
2. Czy model może rzetelnie ocenić własne wyjście? Jakie są ograniczenia?
3. Jak to porównać z wzorcem Agenta Edytora z Części 7?

---

### Ćwiczenie 4 - Porównaj warianty promptów

Przykład uruchamia **dwa warianty promptów** na tych samych przypadkach testowych:

| Wariant | Styl systemowego promptu |
|---------|--------------------------|
| **Bazowy** | Ogólny: „Jesteś pomocnym asystentem” |
| **Specjalistyczny** | Szczegółowy: „Jesteś ekspertem Zava DIY, który poleca konkretne produkty i udziela krok po kroku wskazówek” |

Po uruchomieniu zobaczysz kartę wyników podobną do:

```
╔══════════════════════════════════════════════════════════════╗
║                    EVALUATION SCORECARD                      ║
╠══════════════════════════════════════════════════════════════╣
║ Prompt Variant    │ Rule Score │ LLM Score │ Combined       ║
╠═══════════════════╪════════════╪═══════════╪════════════════╣
║ Baseline          │ 0.62       │ 3.2 / 5   │ 0.62           ║
║ Specialised       │ 0.81       │ 4.1 / 5   │ 0.81           ║
╚══════════════════════════════════════════════════════════════╝
```

**Ćwiczenia:**
1. Uruchom ewaluację i zanotuj wyniki obu wariantów
2. Zmień specjalistyczny prompt, aby był jeszcze bardziej konkretny. Czy wynik się poprawia?
3. Dodaj trzeci wariant promptu i porównaj wszystkie trzy.
4. Spróbuj zmienić alias modelu (np. `phi-4-mini` vs `phi-3.5-mini`) i porównaj wyniki.

---

### Ćwiczenie 5 - Zastosuj ewaluację do własnego agenta

Użyj ram ewaluacyjnych jako szablonu dla własnych agentów:

1. **Zdefiniuj swój złoty zbiór danych**: napisz 5 do 10 przypadków testowych ze spodziewanymi słowami kluczowymi.
2. **Napisz swój prompt systemowy**: instrukcje dla agenta, które chcesz przetestować.
3. **Uruchom ewaluację**: uzyskaj wyniki bazowe.
4. **Iteruj**: dostosuj prompt, uruchom ponownie i porównuj.
5. **Ustaw próg**: np. „nie wysyłaj poniżej 0.75 łącznej oceny”.

---

### Ćwiczenie 6 - Powiązanie ze wzorcem Zava Editor

Spójrz ponownie na agenta Edytora w Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Edytor jest LLM-jurym w produkcji:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

To jest **ten sam koncept** co LLM jako sędzia z Części 8, ale osadzony w produkcyjnym pipeline zamiast w offline zestawie testowym. Oba wzorce:

- Używają ustrukturyzowanego wyjścia JSON z modelu.
- Stosują kryteria jakości zdefiniowane w promptcie systemowym.
- Podejmują decyzję zaliczenia/niezaliczenia.

**Różnica:** Edytor działa w produkcji (na każde żądanie). Ramy ewaluacyjne działają w fazie rozwoju (przed wdrożeniem).

---

## Kluczowe wnioski

| Pojęcie | Wniosek |
|---------|---------|
| **Złote zbiory danych** | Wczesne przygotuj przypadki testowe; są testami regresji dla AI |
| **Kontrole regułowe** | Szybkie, deterministyczne, wyłapują oczywiste błędy (długość, słowa kluczowe, format) |
| **LLM jako sędzia** | Subtelna ocena jakości z wykorzystaniem tego samego lokalnego modelu |
| **Porównanie promptów** | Uruchom wiele wariantów na tym samym zestawie testowym, by wybrać najlepszy |
| **Zaleta działania lokalnego** | Wszystkie ewaluacje działają lokalnie: brak kosztów API, limitów i wycieku danych |
| **Ewaluacja przed wdrożeniem** | Ustaw progi jakości i blokuj wydania, jeśli jakości nie osiągnięto |

---

## Kolejne kroki

- **Skaluj:** Dodaj więcej przypadków testowych i kategorii do swojego złotego zbioru.
- **Automatyzuj:** Włącz ewaluację do swojego pipeline'u CI/CD.
- **Zaawansowani sędziowie:** Użyj większego modelu jako sędzia podczas testowania wyników mniejszego modelu.
- **Śledź w czasie:** Zapisuj wyniki ewaluacji, by porównywać wersje promptów i modeli.

---

## Następne laboratorium

Kontynuuj do [Część 9: Transkrypcja mowy z Whisper](part9-whisper-voice-transcription.md), aby poznać działanie konwersji mowy na tekst na urządzeniu za pomocą Foundry Local SDK.