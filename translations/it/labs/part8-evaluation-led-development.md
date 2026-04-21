![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 8: Sviluppo guidato dalla valutazione con Foundry Local

> **Obiettivo:** Costruire un framework di valutazione che testa e valuta in modo sistematico i tuoi agenti AI, utilizzando lo stesso modello locale sia come agente in prova sia come giudice, così potrai iterare sui prompt con sicurezza prima di distribuire.

## Perché lo sviluppo guidato dalla valutazione?

Quando si costruiscono agenti AI, "sembra ok" non è abbastanza. **Lo sviluppo guidato dalla valutazione** tratta le risposte dell’agente come codice: si scrivono prima i test, si misura la qualità e si distribuisce solo quando i punteggi raggiungono una soglia.

Nel Zava Creative Writer (Parte 7), l’**agente Editor** agisce già come valutatore leggero; decide ACCETTA o RIFAI. Questo laboratorio formalizza quel modello in un framework di valutazione ripetibile che puoi applicare a qualsiasi agente o pipeline.

| Problema | Soluzione |
|---------|----------|
| Le modifiche al prompt rompono silenziosamente la qualità | Il **golden dataset** rileva le regressioni |
| Bias "funziona su un solo esempio" | I **molteplici casi di test** rivelano casi limite |
| Valutazione della qualità soggettiva | La valutazione **basata su regole + LLM come giudice** fornisce numeri |
| Nessun modo per confrontare varianti di prompt | **Esecuzioni di valutazione affiancate** con punteggi aggregati |

---

## Concetti chiave

### 1. Golden Dataset

Un **golden dataset** è un insieme curato di casi di test con output attesi noti. Ogni caso di test include:

- **Input**: Il prompt o la domanda da inviare all’agente
- **Output atteso**: Cosa dovrebbe contenere una risposta corretta o di alta qualità (parole chiave, struttura, fatti)
- **Categoria**: Raggruppamento per reportistica (es. "precisione fattuale", "tono", "completezza")

### 2. Verifiche Basate su Regole

Verifiche rapide, deterministiche che non richiedono un LLM:

| Verifica | Cosa testa |
|---------|------------|
| **Limiti di lunghezza** | La risposta non è troppo breve (pigra) o troppo lunga (logorroica) |
| **Parole chiave richieste** | La risposta menziona termini o entità attese |
| **Validazione del formato** | JSON è analizzabile, sono presenti intestazioni Markdown |
| **Contenuto proibito** | Nessun nome di marca allucinato, nessun riferimento a concorrenti |

### 3. LLM come Giudice

Usa lo **stesso modello locale** per valutare i propri output (o quelli di una variante diversa del prompt). Il giudice riceve:

- La domanda originale
- La risposta dell’agente
- I criteri di valutazione

E restituisce un punteggio strutturato. Questo riflette il modello Editor della Parte 7 ma applicato sistematicamente a una suite di test.

### 4. Ciclo Iterativo Guidato dalla Valutazione

![Ciclo iterativo guidato dalla valutazione](../../../images/eval-loop-flowchart.svg)

---

## Prerequisiti

| Requisito | Dettagli |
|-----------|----------|
| **Foundry Local CLI** | Installato con un modello scaricato |
| **Runtime linguaggio** | **Python 3.9+** e/o **Node.js 18+** e/o **.NET 9+ SDK** |
| **Completato** | [Parte 5: Agenti Singoli](part5-single-agents.md) e [Parte 6: Workflow Multi-Agente](part6-multi-agent-workflows.md) |

---

## Esercizi del laboratorio

### Esercizio 1 - Esegui il framework di valutazione

Il workshop include un esempio completo di valutazione che testa un agente Foundry Local contro un golden dataset di domande relative a Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Setup:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Esecuzione:**
```bash
python foundry-local-eval.py
```

**Cosa succede:**
1. Si connette a Foundry Local e carica il modello
2. Definisce un golden dataset di 5 casi di test (domande sui prodotti Zava)
3. Esegue due varianti di prompt su ogni caso di test
4. Valuta ogni risposta con **verifiche basate su regole** (lunghezza, parole chiave, formato)
5. Valuta ogni risposta con **LLM come giudice** (lo stesso modello assegna qualità da 1 a 5)
6. Stampa una scheda punteggi che confronta entrambe le varianti di prompt

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Setup:**
```bash
cd javascript
npm install
```

**Esecuzione:**
```bash
node foundry-local-eval.mjs
```

**Stessa pipeline di valutazione:** golden dataset, doppia esecuzione dei prompt, punteggi basati su regole + LLM, scheda punteggi.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Setup:**
```bash
cd csharp
dotnet restore
```

**Esecuzione:**
```bash
dotnet run eval
```

**Stessa pipeline di valutazione:** golden dataset, doppia esecuzione dei prompt, punteggi basati su regole + LLM, scheda punteggi.

</details>

---

### Esercizio 2 - Comprendere il Golden Dataset

Esamina i casi di test definiti nell’esempio di valutazione. Ogni caso di test ha:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Domande da considerare:**
1. Perché i valori attesi sono **parole chiave** e non frasi complete?
2. Quanti casi di test servono per una valutazione affidabile?
3. Quali categorie aggiungeresti per la tua applicazione?

---

### Esercizio 3 - Comprendere la valutazione basata su regole vs LLM come giudice

Il framework di valutazione utilizza **due livelli di punteggio**:

#### Verifiche basate su regole (veloci, deterministiche)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM come giudice (sfumata, qualitativa)

Lo stesso modello locale agisce come giudice con una rubrica strutturata:

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

**Domande da considerare:**
1. Quando ti fideresti di più delle verifiche basate su regole rispetto all’LLM come giudice?
2. Un modello può giudicare affidabilmente il proprio output? Quali sono i limiti?
3. Come si confronta questo con il modello Editor della Parte 7?

---

### Esercizio 4 - Confronta le varianti di prompt

L’esempio esegue **due varianti di prompt** sugli stessi casi di test:

| Variante | Stile del prompt di sistema |
|----------|-----------------------------|
| **Base** | Generico: "Sei un assistente utile" |
| **Specializzato** | Dettagliato: "Sei un esperto Zava DIY che raccomanda prodotti specifici e fornisce guide passo-passo" |

Dopo l’esecuzione, vedrai una scheda punteggi come:

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

**Esercizi:**
1. Esegui la valutazione e annota i punteggi di ogni variante
2. Modifica il prompt specializzato per renderlo ancora più specifico. Il punteggio migliora?
3. Aggiungi una terza variante di prompt e confronta tutte e tre.
4. Prova a cambiare l’alias del modello (ad es. `phi-4-mini` vs `phi-3.5-mini`) e confronta i risultati.

---

### Esercizio 5 - Applica la valutazione al tuo agente

Usa il framework di valutazione come modello per i tuoi agenti:

1. **Definisci il tuo golden dataset**: scrivi da 5 a 10 casi di test con parole chiave attese.
2. **Scrivi il tuo prompt di sistema**: le istruzioni agenti che vuoi testare.
3. **Esegui la valutazione**: ottieni i punteggi di base.
4. **Itera**: modifica il prompt, riesegui e confronta.
5. **Imposta una soglia**: ad esempio "non distribuire sotto punteggio combinato 0.75".

---

### Esercizio 6 - Connessione al modello Editor di Zava

Guarda l’agente Editor del Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# L'editor è un LLM-come-giudice in produzione:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Questo è lo **stesso concetto** dell’LLM come giudice della Parte 8, ma integrato nella pipeline di produzione anziché in una suite di test offline. Entrambi i modelli:

- Usano output JSON strutturato dal modello.
- Applicano criteri di qualità definiti nel prompt di sistema.
- Prendono una decisione di pass/fail.

**La differenza:** L’editor gira in produzione (ad ogni richiesta). Il framework di valutazione gira in sviluppo (prima della distribuzione).

---

## Punti chiave da ricordare

| Concetto | Riassunto |
|----------|-----------|
| **Golden dataset** | Cura i casi di test fin dall’inizio; sono i tuoi test di regressione per l’AI |
| **Verifiche basate su regole** | Veloci, deterministiche, individuano fallimenti evidenti (lunghezza, parole chiave, formato) |
| **LLM come giudice** | Valutazione di qualità sfumata usando lo stesso modello locale |
| **Confronto prompt** | Esegui più varianti sulla stessa suite per scegliere la migliore |
| **Vantaggio on-device** | Tutta la valutazione avviene localmente: nessun costo API, nessun limite di rate, nessun dato fuori dal computer |
| **Valuta prima della distribuzione** | Imposta soglie di qualità e controlla le versioni in base ai punteggi di valutazione |

---

## Prossimi passi

- **Scala**: aggiungi più casi di test e categorie al tuo golden dataset.
- **Automatizza**: integra la valutazione nella tua pipeline CI/CD.
- **Giudici avanzati**: usa un modello più grande come giudice mentre testi output di un modello più piccolo.
- **Monitora nel tempo**: salva i risultati delle valutazioni per confrontare prompt e versioni modello.

---

## Prossimo laboratorio

Continua a [Parte 9: Trascrizione vocale con Whisper](part9-whisper-voice-transcription.md) per esplorare il riconoscimento vocale on-device usando il Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:  
Questo documento è stato tradotto utilizzando il servizio di traduzione AI [Co-op Translator](https://github.com/Azure/co-op-translator). Sebbene ci impegniamo per l'accuratezza, si prega di notare che le traduzioni automatiche possono contenere errori o imprecisioni. Il documento originale nella sua lingua nativa deve essere considerato la fonte autorevole. Per informazioni critiche, si raccomanda una traduzione professionale effettuata da un umano. Non siamo responsabili per eventuali malintesi o interpretazioni errate derivanti dall'uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->