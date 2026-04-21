# File audio di esempio per la Parte 7 - Trascrizione vocale Whisper

Questi file WAV sono generati utilizzando `pyttsx3` (sintesi vocale Windows SAPI5) e sono incentrati sui **prodotti Zava DIY** dalla demo Creative Writer.

## Genera i campioni

```bash
# Dalla radice del repository - richiede il .venv con pyttsx3 installato
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## File di esempio

| File | Scenario | Durata |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Cliente che chiede informazioni sul **trapano cordless Zava ProGrip** - coppia, durata della batteria, custodia | ~15 sec |
| `zava-product-review.wav` | Cliente che recensisce la **vernice interna Zava UltraSmooth** - copertura, tempo di asciugatura, VOC basso | ~22 sec |
| `zava-support-call.wav` | Chiamata di supporto sul **baule degli attrezzi Zava TitanLock** - chiavi di ricambio, fodere extra per cassetti | ~20 sec |
| `zava-project-planning.wav` | Fai-da-te che pianifica un patio in giardino con **Zava EcoBoard decking composito** e luci BrightBeam | ~25 sec |
| `zava-workshop-setup.wav` | Panoramica di un laboratorio completo usando **tutti e cinque i prodotti Zava** | ~32 sec |
| `zava-full-project-walkthrough.wav` | Panoramica estesa della ristrutturazione del garage usando **tutti i prodotti Zava** (per test audio lunghi, vedi [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Note

- I file WAV sono **inseriti** nel repository (elencati in `. Per creare nuovi file .wav eseguire lo script sopra per rigenerare nuovi script o modificarli per crearne di nuovi.
- Lo script utilizza la voce **Microsoft David** (inglese USA) a 160 WPM per risultati di trascrizione chiari.
- Tutti gli scenari fanno riferimento ai prodotti da [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:
Questo documento è stato tradotto utilizzando il servizio di traduzione AI [Co-op Translator](https://github.com/Azure/co-op-translator). Pur impegnandoci per l'accuratezza, si prega di notare che le traduzioni automatiche possono contenere errori o inesattezze. Il documento originale nella sua lingua nativa deve essere considerato la fonte autorevole. Per informazioni critiche, si consiglia una traduzione professionale umana. Non siamo responsabili per eventuali fraintendimenti o interpretazioni errate derivanti dall'uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->