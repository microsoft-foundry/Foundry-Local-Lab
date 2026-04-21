![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 8. rész: Értékelés-alapú fejlesztés a Foundry Local segítségével

> **Cél:** Építs fel egy értékelési keretrendszert, amely rendszerezetten teszteli és pontozza az AI ügynökeidet, ugyanazt a helyi modellt használva mind a tesztelt ügynökhöz, mind a bíróhoz, így magabiztosan iterálhatsz a promptokon, mielőtt kiadod azokat.

## Miért az Értékelés-alapú fejlesztés?

AI ügynökök építésekor a „nagyjából rendben van” nem elég. Az **értékelés-alapú fejlesztés** kódhoz hasonlóan kezeli az ügynökök kimenetét: először teszteket írsz, méred a minőséget, és csak akkor adod ki, amikor a pontszám eléri a küszöböt.

A Zava Kreatív Íróban (7. rész) a **Szerkesztő ügynök** már egy könnyű értékelőként működik; eldönti, hogy ELFOGADJA vagy JAVÍTJA a választ. Ez a labor formálisan is keretrendszerré fejleszti ezt a mintát, melyet bármely ügynökre vagy folyamatra alkalmazhatsz.

| Probléma | Megoldás |
|---------|----------|
| A prompt változtatások néma minőségromlást okoznak | A **golden dataset** felfedi a regressziókat |
| „Egy példa alapján működik” torzítás | Több **teszteset** feltárja a szélsőséges eseteket |
| Szubjektív minőségértékelés | A **szabályalapú + LLM-bíró általi pontozás** számadatokat ad |
| Nincs eszköz a prompt variánsok összehasonlítására | **Oldal-by-oldali értékelés** összesített pontszámokkal |

---

## Kulcsfogalmak

### 1. Golden datasetek

A **golden dataset** egy gondosan összeválogatott teszteset-lista ismert, várt kimenetekkel. Minden teszteset tartalmaz:

- **Bemenet**: A prompt vagy kérdés, amit az ügynöknek küldesz
- **Várt kimenet**: Mit kell tartalmaznia egy helyes vagy magas minőségű válasznak (kulcsszavak, struktúra, tények)
- **Kategória**: Jelentési csoportosítás (pl. „tények pontossága”, „hangnem”, „teljesség”)

### 2. Szabályalapú ellenőrzések

Gyors, determinisztikus ellenőrzések, amelyek nem igényelnek LLM-et:

| Ellenőrzés | Mit vizsgál |
|-------|--------------|
| **Hossz korlátok** | A válasz nem túl rövid (lustaság) vagy hosszú (körmönfont) |
| **Kötelező kulcsszavak** | A válasz említi a várt kifejezéseket vagy entitásokat |
| **Formátum ellenőrzés** | JSON értelmezhető, Markdown fejléc jelen van |
| **Tiltott tartalom** | Nincs hallucinált márkanév, nincs versenytárs említés |

### 3. LLM-bíró

Ugyanazt a **helyi modellt** használjuk, hogy pontozza a saját kimenetét (vagy más prompt variáns kimenetét). A bíró megkapja:

- Az eredeti kérdést
- Az ügynök válaszát
- Értékelési szempontokat

És visszaad egy strukturált pontszámot. Ez tükrözi a 7. rész Szerkesztő minta koncepcióját, de rendszerszintű tesztcsomagban.

### 4. Értékelés-vezérelt iterációs ciklus

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Előfeltételek

| Követelmény | Részletek |
|-------------|---------|
| **Foundry Local CLI** | Telepítve, modell letöltve |
| **Programozási környezet** | **Python 3.9+** és/vagy **Node.js 18+** és/vagy **.NET 9+ SDK** |
| **Elvégezve** | [5. rész: Egyszerű ügynökök](part5-single-agents.md) és [6. rész: Több ügynökös munkafolyamatok](part6-multi-agent-workflows.md) |

---

## Gyakorlati feladatok

### 1. feladat – Értékelési keretrendszer futtatása

A workshop teljes értékelési mintát tartalmaz, amely egy Foundry Local ügynököt tesztel egy golden dataset Zava DIY témájú kérdéseivel szemben.

<details>
<summary><strong>🐍 Python</strong></summary>

**Beállítás:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Futtatás:**
```bash
python foundry-local-eval.py
```

**Mi történik:**
1. Kapcsolódik a Foundry Localhoz és betölti a modellt
2. Definiál egy 5 kérdésből álló golden datasetet (Zava termékkérdések)
3. Két prompt variánst futtat minden teszteseten
4. **Szabályalapú ellenőrzést** végez a válaszokra (hossz, kulcsszavak, formátum)
5. **LLM-bíró pontozást** alkalmaz (ugyanaz a modell pontozza minőséget 1-5 között)
6. Kinyomtat egy pontozó táblát, amely összehasonlítja a prompt variánsokat

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Beállítás:**
```bash
cd javascript
npm install
```

**Futtatás:**
```bash
node foundry-local-eval.mjs
```

**Ugyanaz az értékelési folyamat:** golden dataset, két prompt futtatás, szabályalapú + LLM pontozás, pontozó tábla.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Beállítás:**
```bash
cd csharp
dotnet restore
```

**Futtatás:**
```bash
dotnet run eval
```

**Ugyanaz az értékelési folyamat:** golden dataset, két prompt futtatás, szabályalapú + LLM pontozás, pontozó tábla.

</details>

---

### 2. feladat – A Golden Dataset megértése

Vizsgáld meg az értékelési mintában definiált teszteseteket. Minden teszteset tartalmaz:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Megfontolandó kérdések:**
1. Miért **kulcsszavak** a várt értékek, nem pedig teljes mondatok?
2. Hány tesztesetre van szükség egy megbízható értékeléshez?
3. Milyen kategóriákat adnál hozzá a saját alkalmazásodhoz?

---

### 3. feladat – Szabályalapú vs LLM-bíró pontozás megértése

Az értékelési keretrendszer **két pontozási réteget** használ:

#### Szabályalapú ellenőrzések (gyors, determinisztikus)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-bíró (finomabb, minőségi)

Ugyanaz a helyi modell bíróként működik strukturált pontozási móddal:

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

**Megfontolandó kérdések:**
1. Mikor bíznál inkább a szabályalapú ellenőrzésekben, mint az LLM-bíróban?
2. Tud egy modell megbízhatóan pontozni a saját kimenetét? Mik a korlátai?
3. Hogyan viszonyul ez a 7. rész Szerkesztő ügynök mintájához?

---

### 4. feladat – Prompt variánsok összehasonlítása

A minta **két prompt variánst** futtat ugyanazokon a teszteseteken:

| Variáns | Rendszer prompt stílusa |
|---------|-------------------|
| **Alap** | Általános: „Segítőkész asszisztens vagy” |
| **Speciális** | Részletes: „Te vagy egy Zava DIY szakértő, aki konkrét termékeket ajánl és lépésről-lépésre útmutatást ad” |

A futtatás után egy ilyen pontozó táblát fogsz látni:

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

**Feladatok:**
1. Futtasd az értékelést, és jegyezd meg a pontokat mindkét variánsra
2. Módosítsd a speciális promptot még specifikusabbra. Javul-e a pontszám?
3. Adj hozzá egy harmadik prompt variánst, és hasonlítsd össze mindhármat.
4. Próbálj meg modell alias-t váltani (pl. `phi-4-mini` vs `phi-3.5-mini`) és hasonlítsd az eredményeket.

---

### 5. feladat – Értékelés alkalmazása a saját ügynöködre

Használd az értékelési keretrendszert saját ügynökeidhez sablonként:

1. **Határozd meg a golden dataseted**: írj 5-10 tesztesetet várt kulcsszavakkal.
2. **Írd meg a rendszermutató promptot**: az ügynök utasításait, amelyeket tesztelni akarsz.
3. **Futtasd az értékelést**: kapj alap pontszámokat.
4. **Iterálj**: módosítsd a promptot, futtasd újra, és hasonlítsd össze.
5. **Állíts be küszöböt**: pl. „nem adod ki 0,75 alatti összpontszám mellett”.

---

### 6. feladat – Kapcsolat a Zava Szerkesztő mintával

Nézd meg a Zava Kreatív Író Szerkesztő ügynököt (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# A szerkesztő egy LLM-bíróság termelési környezetben:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Ez ugyanaz a **koncepció**, mint a 8. rész LLM-bírója, csak beágyazva a termelési folyamatba, nem pedig offline tesztcsomagban. Mindkét minta:

- Strukturált JSON kimenetet használ a modelltől.
- Minőségi kritériumokat alkalmaz a rendszer promptból.
- Átengedési/elutasítási döntést hoz.

**A különbség:** A szerkesztő fut élesben (minden kérésnél). Az értékelési keretrendszer fejlesztés alatt fut (mielőtt kiadod).

---

## Főbb tanulságok

| Fogalom | Tanulság |
|---------|----------|
| **Golden datasetek** | Korán válogass teszteseteket; ezek az AI regressziótesztjeid |
| **Szabályalapú ellenőrzések** | Gyorsak, determinisztikusak, és az egyszerű hibákat elkapják (hossz, kulcsszavak, formátum) |
| **LLM-bíró** | Finomabb, minőségi pontozás ugyanazzal a helyi modellel |
| **Prompt-összehasonlítás** | Több változatot futtass ugyanazon tesztcsomagon, hogy megtaláld a legjobbat |
| **Helyi futtatás előnye** | Minden futás helyben: nincs API költség, nincs sebességkorlát, nem hagy ki adatokat |
| **Értékelés a kiadás előtt** | Állíts be minőségi küszöböket és feltételhez kösd a kiadásokat |

---

## Következő lépések

- **Skálázás**: Adj hozzá több tesztesetet és kategóriát a golden datasethez.
- **Automatizálás**: Integráld az értékelést a CI/CD folyamatodba.
- **Fejlettebb bírók**: Használj nagyobb modellt bíróként, miközben kisebb modellt tesztelsz.
- **Kövesd nyomon időben**: Mentsd el az értékelési eredményeket a prompt- és modellverziók összehasonlításához.

---

## Következő labor

Folytasd a [9. rész: Hangátírás Whisper-rel](part9-whisper-voice-transcription.md) témával, hogy felfedezd az on-device beszéd-szöveg átalakítást a Foundry Local SDK-val.