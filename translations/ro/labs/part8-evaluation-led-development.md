![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partea 8: Dezvoltare Condusă de Evaluare cu Foundry Local

> **Scop:** Construiește un cadru de evaluare care testează și evaluează sistematic agenții tăi AI, folosind același model local atât ca agent testat, cât și ca judecător, astfel încât să poți itera pe prompturi cu încredere înainte de lansare.

## De ce Dezvoltare Condusă de Evaluare?

Când construiești agenți AI, „arată destul de bine” nu este suficient. **Dezvoltarea condusă de evaluare** tratează output-urile agenților ca pe cod: scrii teste întâi, măsori calitatea și trimiți în producție doar când scorurile ating un prag.

În Zava Creative Writer (Partea 7), **agentul Editor** acționează deja ca un evaluator ușor; decide ACCEPTARE sau REVIZUIRE. Acest laborator formalizează acest tipar într-un cadru de evaluare repetabil pe care îl poți aplica oricărui agent sau flux de lucru.

| Problemă | Soluție |
|---------|----------|
| Schimbările în prompt rup calitatea în mod silențios | **Set de date aurit** detectează regresiile |
| Bias de „funcționează pe un singur exemplu” | **Mai multe cazuri de test** dezvăluie cazuri-limită |
| Evaluare subiectivă a calității | **Reguli + evaluare LLM ca judecător** oferă cifre |
| Nicio modalitate de a compara variante de prompt | **Evaluări paralele** cu scoruri agregate |

---

## Concepte Cheie

### 1. Seturi de Date Aurite

Un **set de date aurit** este un set selectat de cazuri de test cu output-uri așteptate cunoscute. Fiecare caz de test include:

- **Input**: Promptul sau întrebarea trimisă agentului
- **Output așteptat**: Ce trebuie să conțină un răspuns corect sau de calitate înaltă (cuvinte cheie, structură, fapte)
- **Categorie**: Grupare pentru raportare (ex. „acuratețe factuală”, „ton”, „completitudine”)

### 2. Verificări pe Bază de Reguli

Verificări rapide, deterministe, care nu necesită un LLM:

| Verificare | Ce testează |
|-------|--------------|
| **Limite de lungime** | Răspunsul nu este prea scurt (leneș) sau prea lung (a bălăcărit) |
| **Cuvinte cheie obligatorii** | Răspunsul menționează termeni sau entități așteptate |
| **Validare format** | JSON-ul este parsabil, header-ele Markdown sunt prezente |
| **Conținut interzis** | Niciun nume de brand halucinat, nicio mențiune a competitorilor |

### 3. LLM ca Judecător

Folosește **același model local** pentru a evalua propriile output-uri (sau cele generate de o variantă diferită de prompt). Judecătorul primește:

- Întrebarea originală
- Răspunsul agentului
- Criteriile de notare

Și returnează un scor structurat. Aceasta reflectă tiparul Editor din Partea 7, aplicat sistematic pe un set de teste.

### 4. Bucle Iterative Condusă de Evaluare

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Cerințe Prealabile

| Cerință | Detalii |
|-------------|---------|
| **Foundry Local CLI** | Instalată cu un model descărcat |
| **Mediu de execuție** | **Python 3.9+** și/sau **Node.js 18+** și/sau **.NET 9+ SDK** |
| **Finalizat** | [Partea 5: Agenți Unici](part5-single-agents.md) și [Partea 6: Fluxuri Multi-Agent](part6-multi-agent-workflows.md) |

---

## Exerciții de Laborator

### Exercițiul 1 - Rulează Cadrul de Evaluare

Workshop-ul include un exemplu complet de evaluare care testează un agent Foundry Local cu un set de date aurit de întrebări legate de Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Configurare:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Rulează:**
```bash
python foundry-local-eval.py
```

**Ce se întâmplă:**
1. Se conectează la Foundry Local și încarcă modelul
2. Definește un set de date aurit cu 5 cazuri de test (întrebări despre produsele Zava)
3. Rulează două variante de prompt pentru fiecare caz de test
4. Evaluează fiecare răspuns cu **verificări pe bază de reguli** (lungime, cuvinte cheie, format)
5. Evaluează fiecare răspuns cu **LLM ca judecător** (același model notează calitatea de la 1-5)
6. Afișează o fișă de scor care compară ambele variante de prompt

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Configurare:**
```bash
cd javascript
npm install
```

**Rulează:**
```bash
node foundry-local-eval.mjs
```

**Același flux de evaluare:** set de date aurit, rulări duble de prompt, scor pe baza de reguli + LLM, fișă de scor.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Configurare:**
```bash
cd csharp
dotnet restore
```

**Rulează:**
```bash
dotnet run eval
```

**Același flux de evaluare:** set de date aurit, rulări duble de prompt, scor pe baza de reguli + LLM, fișă de scor.

</details>

---

### Exercițiul 2 - Înțelege Setul de Date Aurit

Examinează cazurile de test definite în exemplul de evaluare. Fiecare caz are:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Întrebări de luat în considerare:**
1. De ce valorile așteptate sunt **cuvinte cheie** și nu fraze complete?
2. Câte cazuri de test sunt necesare pentru o evaluare fiabilă?
3. Ce categorii ai adăuga pentru propria ta aplicație?

---

### Exercițiul 3 - Înțelege Diferența dintre Verificări pe Bază de Reguli și Evaluarea LLM ca Judecător

Cadrul de evaluare folosește **două straturi de evaluare**:

#### Verificări pe Bază de Reguli (rapide, deterministe)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM ca Judecător (nuanțat, calitativ)

Același model local acționează ca judecător cu o rubrică structurată:

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

**Întrebări de luat în considerare:**
1. Când ai încredere mai mare în verificările pe bază de reguli decât în evaluarea LLM ca judecător?
2. Poate un model să evalueze fiabil propriul output? Care sunt limitările?
3. Cum se compară cu tiparul agentului Editor din Partea 7?

---

### Exercițiul 4 - Compară Variantele de Prompt

Exemplul rulează **două variante de prompt** pe aceleași cazuri de test:

| Variantă | Stilul Promptului Sistem |
|---------|-------------------|
| **Baseline** | Generic: „Ești un asistent de ajutor” |
| **Specializat** | Detaliat: „Ești un expert Zava DIY care recomandă produse specifice și oferă ghidare pas cu pas” |

După rulare, vei vedea o fișă de scor ca aceasta:

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

**Exerciții:**
1. Rulează evaluarea și notează scorurile pentru fiecare variantă
2. Modifică promptul specializat pentru a fi și mai specific. Îmbunătățește scorul?
3. Adaugă o a treia variantă de prompt și compară toate trei.
4. Încearcă să schimbi aliasul modelului (de ex. `phi-4-mini` vs `phi-3.5-mini`) și compară rezultatele.

---

### Exercițiul 5 - Aplică Evaluarea Propriului Agent

Folosește cadrul de evaluare ca șablon pentru agenții tăi:

1. **Definește propriul set de date aurit**: scrie 5-10 cazuri de test cu cuvinte cheie așteptate.
2. **Scrie promptul sistemului**: instrucțiunile agentului pe care vrei să le testezi.
3. **Rulează evaluarea**: obține scoruri de bază.
4. **Iterează**: ajustează promptul, rulează iar și compară.
5. **Setează un prag**: ex. „nu lansa dacă scorul combinat este sub 0.75”.

---

### Exercițiul 6 - Conexiune la Tiparul Editor Zava

Revezi agentul Editor din Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Editorul este un LLM-ca-judecător în producție:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Acesta este **același concept** ca LLM ca judecător din Partea 8, dar integrat în fluxul de producție, nu într-un set de teste offline. Ambele tipare:

- Utilizează output JSON structurat de la model.
- Aplică criterii de calitate definite în promptul sistem.
- Ia o decizie de trecut/respins.

**Diferența:** Editorul rulează în producție (la fiecare cerere). Cadrul de evaluare rulează în dezvoltare (înainte de lansare).

---

## Puncte Cheie de Reținut

| Concept | Esență |
|---------|----------|
| **Seturi de date aurite** | Curăță cazurile de test din timp; sunt testele tale de regresie pentru AI |
| **Verificări pe bază de reguli** | Rapid, determinist, prinde eșecurile evidente (lungime, cuvinte cheie, format) |
| **LLM ca judecător** | Evaluare nuanțată a calității folosind același model local |
| **Comparație de prompturi** | Rulează variante multiple pe același set de teste ca să alegi cea mai bună |
| **Avantaj on-device** | Toate evaluările rulează local: fără costuri de API, fără limitări de rată, fără date care părăsesc mașina |
| **Eval înainte de lansare** | Setează praguri de calitate și blochează lansările pe baza scorurilor |

---

## Pași Următori

- **Scalează**: Adaugă mai multe cazuri de test și categorii în setul tău aurit.
- **Automatizează**: Integrează evaluarea în fluxul CI/CD.
- **Judecători avansați**: Folosește un model mai mare ca judecător testând output-ul unui model mai mic.
- **Monitorizează în timp**: Salvează rezultatele pentru a compara versiunile de prompt și model.

---

## Următorul Laborator

Continuă cu [Partea 9: Transcriere Vocală cu Whisper](part9-whisper-voice-transcription.md) pentru a explora conversia vorbirii în text on-device folosind Foundry Local SDK.