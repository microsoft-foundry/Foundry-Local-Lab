![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 8: Arviointivetoista kehitystä Foundry Localin avulla

> **Tavoite:** Luo arviointikehys, joka systemaattisesti testaa ja pisteyttää tekoälyagenttejasi käyttäen samaa paikallista mallia sekä testattavana agenttina että arvioijana, jotta voit kehittää kehotteita luottavaisin mielin ennen julkaisua.

## Miksi arviointivetoista kehitystä?

Rakentaessa tekoälyagentteja "näyttää oikealta" ei riitä. **Arviointivetoisessa kehityksessä** agenttien vastauksia käsitellään kuin koodia: ensin kirjoitetaan testit, laatu mitataan ja toimitus tehdään vasta kun pisteet ylittävät rajaarvon.

Zava Creative Writerissa (Osa 7) **Editor-agentti** toimii kevyenä arvioijana; se päättää HYVÄKSYTÄÄN vai MUOKATAAN. Tämä työpaja formalisoidaa kyseisen mallin toistettavaksi arviointikehyseksi, jota voi soveltaa mihin tahansa agenttiin tai putkeen.

| Ongelma | Ratkaisu |
|---------|----------|
| Kehotteen muutokset rikkoivat hiljaisesti laatua | **Kultainen aineisto** havaitsee regressiot |
| "Toimii yhdellä esimerkillä" -harha | **Monet testitapaukset** paljastavat reunatapaukset |
| Subjektiivinen laadunarviointi | **Sääntöpohjainen + LLM-arvioija** antaa numeerisen arvion |
| Ei tapaa verrata kehotteiden versioita | **Sivuttain ajetut arvioinnit** yhdistetyillä pisteillä |

---

## Keskeiset käsitteet

### 1. Kultaiset aineistot

**Kultainen aineisto** on huolellisesti koottu joukko testitapauksia, joiden odotetut vastaukset tunnetaan. Jokainen testitapaus sisältää:

- **Syöte**: Kehote tai kysymys agentille
- **Odotettu vastaus**: Mitä oikean tai laadukkaan vastauksen tulisi sisältää (avainsanat, rakenne, faktat)
- **Kategoria**: Raportointiryhmittely (esim. "tarkka faktatieto", "sävy", "täydellisyys")

### 2. Sääntöpohjaiset tarkastukset

Nopeat, deterministiset tarkistukset, jotka eivät vaadi LLM:ää:

| Tarkistus | Mitä se testaa |
|-----------|----------------|
| **Pituuden rajat** | Vastaus ei ole liian lyhyt (laiska) tai liian pitkä (höpiseminen) |
| **Vaaditut avainsanat** | Vastaus mainitsee odotetut termit tai entiteetit |
| **Muodon validointi** | JSON on jäsennettävissä, Markdown-otsikot ovat paikallaan |
| **Kielletty sisältö** | Ei harhailevia tuotemerkkejä, ei kilpailijoiden mainintoja |

### 3. LLM-arvioija

Käytä **samaa paikallista mallia** arvioimaan omat vastauksensa (tai eri kehotteen vastausten). Arvioija saa:

- Alkuperäisen kysymyksen
- Agentin vastauksen
- Arviointikriteerit

Ja palauttaa rakenteellisen pistemääräyksen. Tämä vastaa Editor-mallia Osa 7:ssä, mutta sovelletaan systemaattisesti koko testisettiin.

### 4. Arviointivetoisen iteraation sykli

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Vaatimukset

| Vaatimus | Yksityiskohdat |
|----------|----------------|
| **Foundry Local CLI** | Asennettuna ja malli ladattuna |
| **Kieliajoalusta** | **Python 3.9+** ja/tai **Node.js 18+** ja/tai **.NET 9+ SDK** |
| **Valmis** | [Osa 5: Yksittäiset agentit](part5-single-agents.md) ja [Osa 6: Moniagenttityönkulut](part6-multi-agent-workflows.md) |

---

## Harjoitukset

### Harjoitus 1 - Aja arviointikehys

Työpajassa mukana täydellinen arviointiesimerkki, joka testaa Foundry Local -agenttia kultaisen aineiston Zava DIY -kysymyksiä vastaan.

<details>
<summary><strong>🐍 Python</strong></summary>

**Asetus:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Aja:**
```bash
python foundry-local-eval.py
```

**Tapahtuu:**
1. Yhdistää Foundry Localiin ja lataa mallin
2. Määrittelee kultaisen aineiston, jossa 5 testitapausta (Zava-tuotekysymykset)
3. Ajaa kaksi kehotteen versiota jokaista testitapausta vastaan
4. Pisteyttää jokaisen vastauksen **sääntöpohjaisilla tarkistuksilla** (pituus, avainsanat, muoto)
5. Pisteyttää jokaisen vastauksen **LLM-arvioijalla** (sama malli arvioi laadun 1-5)
6. Tulostaa vertailutaulukon molemmista kehotteista

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Asetus:**
```bash
cd javascript
npm install
```

**Aja:**
```bash
node foundry-local-eval.mjs
```

**Sama arviointiputki:** kultainen aineisto, kaksi kehotteen ajoa, sääntöpohjainen + LLM-pisteytys, tulostaulukko.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Asetus:**
```bash
cd csharp
dotnet restore
```

**Aja:**
```bash
dotnet run eval
```

**Sama arviointiputki:** kultainen aineisto, kaksi kehotteen ajoa, sääntöpohjainen + LLM-pisteytys, tulostaulukko.

</details>

---

### Harjoitus 2 - Ymmärrä kultainen aineisto

Tutki arviointiesimerkissä määriteltyjä testitapauksia. Jokaisella testitapauksella on:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Kysymyksiä pohdittavaksi:**
1. Miksi odotetut arvot ovat **avainsanoja** eivätkä kokonaisia lauseita?
2. Kuinka monta testitapausta tarvitset luotettavaan arviointiin?
3. Mitä kategorioita lisäisit omaan sovellukseesi?

---

### Harjoitus 3 - Ymmärrä sääntöpohjaisen ja LLM-arvioijan pisteytyksen erot

Arviointikehyksessä on **kaksi pisteytyskerrosta**:

#### Sääntöpohjaiset tarkistukset (nopeat, deterministiset)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-arvioija (monisyinen, laadullinen)

Sama paikallinen malli toimii arvioijana rakenteellisen arviointikriteeristön avulla:

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

**Kysymyksiä pohdittavaksi:**
1. Milloin luottaisit enemmän sääntöpohjaisiin tarkastuksiin kuin LLM-arvioijaan?
2. Voiko malli luotettavasti arvioida omaa tuotostaan? Mitkä ovat rajoitteet?
3. Miten tämä vertautuu Editor-agentin malliin Osa 7:ssä?

---

### Harjoitus 4 - Vertaa kehotteen versioita

Esimerkki ajaa **kahta kehotteiden versiota** samoja testitapauksia vastaan:

| Versio | Järjestelmäkehote |
|--------|------------------|
| **Perus** | Yleinen: "Olet avulias avustaja" |
| **Erikoistunut** | Yksityiskohtainen: "Olet Zava DIY -asiantuntija, joka suosittelee tiettyjä tuotteita ja antaa vaiheittaiset ohjeet" |

Ajon jälkeen näet tulostaulukon:

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

**Harjoitukset:**
1. Aja arviointi ja merkitse pisteet kummallekin versiolle
2. Muokkaa erikoistunutta kehotetta vielä tarkemmaksi. Paraneeko pistemäärä?
3. Lisää kolmas kehotteen versio ja vertaa kaikkia kolmea.
4. Kokeile vaihtaa mallin alias (esim. `phi-4-mini` vs `phi-3.5-mini`) ja vertaa tuloksia.

---

### Harjoitus 5 - Käytä arviointia omalle agentillesi

Käytä arviointikehystä mallina omille agenteillesi:

1. **Määritä oma kultainen aineisto:** kirjoita 5–10 testitapausta odotetuilla avainsanoilla.
2. **Kirjoita järjestelmäkehote:** agentin ohjeistus, jota haluat testata.
3. **Aja arviointi:** saat lähtötason pisteet.
4. **Iteroi:** muokkaa kehotetta, aja uudelleen ja vertaa.
5. **Aseta kynnysarvo:** esim. "älä julkaise alle 0.75 yhdistetyn pisteen".

---

### Harjoitus 6 - Yhteys Zava Editorin malliin

Katso Zava Creative Writerin Editor-agentti (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Editori on tuotannossa toimiva LLM-tuomarina:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Tämä on **sama konsepti** kuin Osa 8:n LLM-arvioija, mutta upotettuna tuotantoputkeen eikä offline-testisarjaan. Molemmissa:

- Malli tuottaa rakenteellista JSON-vastausta.
- Sovelletaan järjestelmäkehotteessa määriteltyjä laatukriteerejä.
- Tehdään hyväksytty/hylätty -päätös.

**Ero:** Editor toimii tuotannossa (joka pyynnöllä). Arviointikehys toimii kehityksessä (ennen julkaisua).

---

## Keskeiset opit

| Käsite | Yhteenveto |
|---------|------------|
| **Kultaiset aineistot** | Kerää testitapaukset ajoissa; ne ovat AI:n regressiotestit |
| **Sääntöpohjaiset tarkistukset** | Nopeat, deterministiset ja havaitsevat ilmeiset virheet (pituus, avainsanat, muoto) |
| **LLM-arvioija** | Monisyinen laatupisteytys samalla paikallisella mallilla |
| **Kehotteen vertailu** | Aja useita versioita samassa testikokoelmassa, valitse paras |
| **Paikallinen etu** | Kaikki arvioinnit ajetaan laitteella: ei API-kuluja, ei nopeusrajoja, ei tiedon siirtymistä |
| **Arvioi ennen julkaisua** | Aseta laatukynnykset ja vapauta vain kun pisteet täyttyvät |

---

## Seuraavat askeleet

- **Skaalaa**: lisää testitapauksia ja kategorioita kultaisiin aineistoihin.
- **Automatisoi**: integroi arviointi CI/CD-putkeen.
- **Edistyneet arvioijat**: käytä isompaa mallia arvioijana, testaa pienemmän tuotosta.
- **Seuraa ajan myötä**: tallenna arviointitulokset vertailtaessa kehotteiden ja mallien versioita.

---

## Seuraava labra

Jatka [Osa 9: Puheen tekstitys Whisperillä](part9-whisper-voice-transcription.md) kokeillaksesi puheesta tekstiksi muunnosta suoraan laitteella Foundry Local SDK:n avulla.