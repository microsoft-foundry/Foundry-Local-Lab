# Eksempel lydfiler for del 7 - Whisper tale-transkribering

Disse WAV-filene er generert ved bruk av `pyttsx3` (Windows SAPI5 tekst-til-tale) og har tema rundt **Zava DIY-produkter** fra Creative Writer-demoen.

## Generer eksemplene

```bash
# Fra repo-rot - krever .venv med pyttsx3 installert
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Eksempelfiler

| Fil | Scenario | Varighet |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Kunde som spør om **Zava ProGrip ledningsfri drill** - moment, batterilevetid, bærekoffert | ~15 sek |
| `zava-product-review.wav` | Kunde som vurderer **Zava UltraSmooth interiørmaling** - dekning, tørketid, lavt VOC | ~22 sek |
| `zava-support-call.wav` | Supportanrop om **Zava TitanLock verktøy-kiste** - reservednøkler, ekstra skuffeinnlegg | ~20 sek |
| `zava-project-planning.wav` | DIY-entusiast som planlegger en bakgårdsdekk med **Zava EcoBoard kompositt-dekk** & BrightBeam-lys | ~25 sek |
| `zava-workshop-setup.wav` | Gjennomgang av komplett verksted som bruker **alle fem Zava-produktene** | ~32 sek |
| `zava-full-project-walkthrough.wav` | Utvidet garasjerenoverings-gjennomgang med bruk av **alle Zava-produkter** (for lang lyd-testing, se [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Notater

- WAV-filene er **innsendt** i repoet (oppført i `. For å lage nye .wav-filer kjør skriptet ovenfor for å regenerere nye skript eller modifisere for å lage nye skript.
- Skriptet bruker **Microsoft David** (amerikansk engelsk) stemme på 160 ord per minutt for klare transkriberingsresultater.
- Alle scenarier refererer til produkter fra [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).