# Fichiers Audio d'Exemple pour la Partie 7 - Transcription Vocale Whisper

Ces fichiers WAV sont générés à l'aide de `pyttsx3` (synthèse vocale Windows SAPI5) et tournent autour des **produits Zava DIY** issus de la démo Creative Writer.

## Générer les échantillons

```bash
# Depuis la racine du dépôt - nécessite le .venv avec pyttsx3 installé
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Fichiers d'exemple

| Fichier | Scénario | Durée |
|---------|----------|-------|
| `zava-customer-inquiry.wav` | Client posant des questions sur la **Perceuse sans fil Zava ProGrip** - couple, autonomie de la batterie, mallette de transport | ~15 sec |
| `zava-product-review.wav` | Client évaluant la **Peinture intérieure Zava UltraSmooth** - couverture, temps de séchage, faible COV | ~22 sec |
| `zava-support-call.wav` | Appel de support au sujet de la **Boîte à outils Zava TitanLock** - clés de remplacement, doublures supplémentaires pour tiroirs | ~20 sec |
| `zava-project-planning.wav` | Bricoleur planifiant une terrasse extérieure avec **les lames composites Zava EcoBoard** & les lumières BrightBeam | ~25 sec |
| `zava-workshop-setup.wav` | Présentation complète d'un atelier utilisant **les cinq produits Zava** | ~32 sec |
| `zava-full-project-walkthrough.wav` | Visite prolongée de la rénovation de garage utilisant **tous les produits Zava** (pour test audio longue durée, voir [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Notes

- Les fichiers WAV sont **committés** dans le dépôt (listés dans `. Pour créer de nouveaux fichiers .wav, exécutez le script ci-dessus pour régénérer de nouveaux scripts ou modifiez-le pour créer de nouveaux scripts.
- Le script utilise la voix **Microsoft David** (anglais US) à 160 mots par minute pour des résultats de transcription clairs.
- Tous les scénarios font référence aux produits dans [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).