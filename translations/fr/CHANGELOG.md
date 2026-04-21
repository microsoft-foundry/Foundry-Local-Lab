# Changelog — Atelier Foundry Local

Tous les changements notables de cet atelier sont documentés ci-dessous.

---

## 2026-03-11 — Partie 12 & 13, Interface Web, Réécriture Whisper, Correction WinML/QNN, et Validation

### Ajouté
- **Partie 12 : Création d'une interface Web pour le Zava Creative Writer** — nouveau guide de labo (`labs/part12-zava-ui.md`) avec des exercices couvrant le streaming NDJSON, `ReadableStream` du navigateur, des badges de statut d’agent en direct, et le streaming en temps réel du texte de l’article
- **Partie 13 : Atelier complété** — nouveau labo résumé (`labs/part13-workshop-complete.md`) avec un récapitulatif des 12 parties, d’autres idées, et des liens vers des ressources
- **Interface front-end de Zava UI :** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — interface partagée en vanilla HTML/CSS/JS consommée par les trois backends
- **Serveur HTTP JavaScript :** `zava-creative-writer-local/src/javascript/server.mjs` — nouveau serveur HTTP de style Express enroulant l’orchestrateur pour accès via navigateur
- **Backend C# ASP.NET Core :** `zava-creative-writer-local/src/csharp-web/Program.cs` et `ZavaCreativeWriterWeb.csproj` — nouveau projet API minimal servant l’UI et le streaming NDJSON
- **Générateur d’exemples audio :** `samples/audio/generate_samples.py` — script TTS hors ligne utilisant `pyttsx3` pour générer des fichiers WAV à thème Zava pour la Partie 9
- **Exemple audio :** `samples/audio/zava-full-project-walkthrough.wav` — nouvel exemple audio plus long pour les tests de transcription
- **Script de validation :** `validate-npu-workaround.ps1` — script automatisé PowerShell pour valider la solution de contournement NPU/QNN sur tous les exemples C#
- **Diagrammes Mermaid SVG :** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Support multiplateforme WinML :** Les 3 fichiers `.csproj` C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) utilisent désormais une TFM conditionnelle et des références paquet mutuellement exclusives pour le support multiplateforme. Sous Windows : TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (surensemble incluant le plugin QNN EP). Hors Windows : TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK de base). Le RID codé en dur `win-arm64` dans les projets Zava a été remplacé par une détection automatique. Une solution de contournement de dépendance transitive exclut les assets natifs de `Microsoft.ML.OnnxRuntime.Gpu.Linux` qui possède une référence win-arm64 cassée. L’ancienne solution try/catch NPU a été retirée des 7 fichiers C#.

### Modifié
- **Partie 9 (Whisper) :** Réécriture majeure — JavaScript utilise maintenant le `AudioClient` intégré du SDK (`model.createAudioClient()`) au lieu d’une inférence manuelle ONNX Runtime ; mises à jour des descriptions d’architecture, tableaux comparatifs, et diagrammes de pipeline reflétant l’approche `AudioClient` JS/C# contre l’approche Python ONNX Runtime
- **Partie 11 :** Liens de navigation mis à jour (pointeurs vers Partie 12) ; ajout de diagrammes SVG rendus pour le flux d’appel d’outils et la séquence
- **Partie 10 :** Navigation mise à jour pour passer par Partie 12 au lieu de terminer l’atelier
- **Whisper Python (`foundry-local-whisper.py`) :** Élargi avec des samples audio supplémentaires et une meilleure gestion des erreurs
- **Whisper JavaScript (`foundry-local-whisper.mjs`) :** Réécrit pour utiliser `model.createAudioClient()` avec `audioClient.transcribe()` au lieu des sessions ONNX Runtime manuelles
- **FastAPI Python (`zava-creative-writer-local/src/api/main.py`) :** Mis à jour pour servir aussi les fichiers UI statiques en plus de l’API
- **Console C# Zava (`zava-creative-writer-local/src/csharp/Program.cs`) :** Suppression de la solution de contournement NPU (prise en charge par le paquet WinML)
- **README.md :** Ajout de la section Partie 12 avec tableaux d’exemples de code et ajouts backend ; ajout de la section Partie 13 ; mise à jour des objectifs d’apprentissage et de la structure du projet
- **KNOWN-ISSUES.md :** Suppression du problème résolu #7 (variante modèle NPU SDK C# — prise en charge désormais par paquet WinML). Renumérotation des problèmes restants #1–#6. Mise à jour des détails d’environnement avec .NET SDK 10.0.104
- **AGENTS.md :** Mise à jour de l’arbre de structure projet avec nouvelles entrées `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`) ; mise à jour des principaux paquets C# et détails TFM conditionnel
- **labs/part2-foundry-local-sdk.md :** Mise à jour de l’exemple `.csproj` pour montrer le modèle multiplateforme complet avec TFM conditionnelle, références mutuellement exclusives et note explicative

### Validé
- Les 3 projets C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) compilent avec succès sur Windows ARM64
- Exemple chat (`dotnet run chat`) : modèle charge comme `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — variante NPU charge directement sans fallback CPU
- Exemple agent (`dotnet run agent`) : exécution complète avec conversation multi-tours, code de sortie 0
- Foundry Local CLI v0.8.117 et SDK v0.9.0 sur .NET SDK 9.0.312

---

## 2026-03-11 — Corrections de Code, Nettoyage de Modèle, Diagrammes Mermaid, et Validation

### Corrigé
- **Les 21 exemples de code (7 Python, 7 JavaScript, 7 C#) :** Ajout de `model.unload()` / `unload_model()` / `model.UnloadAsync()` au moment de la sortie pour résoudre les avertissements de fuite mémoire OGA (Problème connu #4)
- **csharp/WhisperTranscription.cs :** Remplacement du chemin relatif fragile via `AppContext.BaseDirectory` par `FindSamplesDirectory()` qui remonte les répertoires pour localiser fiablement `samples/audio` (Problème connu #7)
- **csharp/csharp.csproj :** Remplacement du `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` codé en dur par une détection avec fallback utilisant `$(NETCoreSdkRuntimeIdentifier)` pour que `dotnet run` fonctionne sur toute plate-forme sans flag `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Modifié
- **Partie 8 :** Conversion de la boucle itérative pilotée par évaluation du diagramme en boîte ASCII vers image SVG rendue
- **Partie 10 :** Conversion du diagramme pipeline compilation des flèches ASCII vers image SVG rendue
- **Partie 11 :** Conversion des diagrammes de flux d’appel d’outils et de séquence en images SVG rendues
- **Partie 10 :** Déplacement de la section "Atelier complété !" vers Partie 11 (le labo final) ; remplacée par un lien "Étapes suivantes"
- **KNOWN-ISSUES.md :** Revalidation complète de tous les problèmes contre la CLI v0.8.117. Suppression des problèmes résolus : fuite mémoire OGA (nettoyage ajouté), chemin Whisper (FindSamplesDirectory), inférence HTTP 500 durable (non reproduisible, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), limitations de tool_choice (fonctionne désormais avec `"required"` et ciblage fonction spécifique sur qwen2.5-0.5b). Mise à jour problème JS Whisper — tous les fichiers renvoient maintenant une sortie vide/binaire (régression depuis v0.9.x, gravité montée à Majeure). Mise à jour #4 RID C# avec solution de contournement auto-détectée et lien [#497](https://github.com/microsoft/Foundry-Local/issues/497). 7 problèmes ouverts restent.
- **javascript/foundry-local-whisper.mjs :** Correction du nom de variable de nettoyage (`whisperModel` → `model`)

### Validé
- Python : `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — exécution réussie avec nettoyage
- JavaScript : `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — exécution réussie avec nettoyage
- C# : `dotnet build` réussit sans avertissement ni erreur (cible net9.0)
- Les 7 fichiers Python passent la vérification syntaxique `py_compile`
- Les 7 fichiers JavaScript passent la validation syntaxique `node --check`

---

## 2026-03-10 — Partie 11 : Appel d’Outils, Extension API SDK, et Couverture Modèle

### Ajouté
- **Partie 11 : Appel d’Outils avec Modèles Locaux** — nouveau guide de labo (`labs/part11-tool-calling.md`) avec 8 exercices couvrant schémas d’outils, flux multi-tours, appels multiples d’outils, outils personnalisés, appel d’outils via ChatClient, et `tool_choice`
- **Exemple Python :** `python/foundry-local-tool-calling.py` — appel d’outils avec `get_weather`/`get_population` utilisant OpenAI SDK
- **Exemple JavaScript :** `javascript/foundry-local-tool-calling.mjs` — appel d’outils via `ChatClient` natif du SDK (`model.createChatClient()`)
- **Exemple C# :** `csharp/ToolCalling.cs` — appel d’outils avec `ChatTool.CreateFunctionTool()` et SDK OpenAI C#
- **Partie 2, Exercice 7 :** ChatClient natif — `model.createChatClient()` (JS) et `model.GetChatClientAsync()` (C#) comme alternatives au SDK OpenAI
- **Partie 2, Exercice 8 :** Variantes de modèle et sélection hardware — `selectVariant()`, `variants`, tableau variante NPU (7 modèles)
- **Partie 2, Exercice 9 :** Mises à jour et rafraîchissement catalogue — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Partie 2, Exercice 10 :** Modèles de raisonnement — `phi-4-mini-reasoning` avec exemples d’analyse des balises `<think>`
- **Partie 3, Exercice 4 :** `createChatClient` comme alternative au SDK OpenAI, documentation sur le pattern callback streaming
- **AGENTS.md :** Ajout des conventions de codage Appel d’Outils, ChatClient, et Modèles de Raisonnement

### Modifié
- **Partie 1 :** Catalogue modèle étendu — ajout de phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Partie 2 :** Tableaux de référence API étendus — ajout de `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Partie 2 :** Renumérotation exercices 7-9 → 10-13 pour intégrer les nouveaux exercices
- **Partie 3 :** Tableau Points Clés mis à jour pour inclure ChatClient natif
- **README.md :** Ajout de la section Partie 11 avec tableau d’exemples de code ; ajout de l’objectif d’apprentissage #11 ; mise à jour de la structure de projet
- **csharp/Program.cs :** Ajout du cas `toolcall` au routeur CLI et mise à jour de l’aide

---

## 2026-03-09 — Mise à jour SDK v0.9.0, Anglais britannique, et Validation globale

### Modifié
- **Tous les exemples de code (Python, JavaScript, C#) :** Mise à jour vers Foundry Local SDK v0.9.0 — correction du `await catalog.getModel()` (manquait `await`), mise à jour des patterns d’init `FoundryLocalManager`, correction de découverte d’endpoint
- **Tous les guides de labo (Parties 1-10) :** Conversion en anglais britannique (colour, catalogue, optimised, etc.)
- **Tous les guides de labo :** Mise à jour des exemples SDK pour correspondre à l’API v0.9.0
- **Tous les guides de labo :** Mise à jour des tableaux de référence API et des blocs de code des exercices
- **Correction critique JavaScript :** Ajout du `await` manquant sur `catalog.getModel()` — renvoyait une `Promise` et non un objet `Model`, provoquant des échecs silencieux

### Validé
- Tous les exemples Python s’exécutent avec succès contre le service Foundry Local
- Tous les exemples JavaScript s’exécutent avec succès (Node.js 18+)
- Projet C# compile et s’exécute sous .NET 9.0 (compatibilité ascendante depuis assemblage net8.0 SDK)
- 29 fichiers modifiés et validés dans l’atelier

---

## Index des fichiers

| Fichier | Dernière mise à jour | Description |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Catalogue modèle étendu |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nouveaux exercices 7-10, tableaux API étendus |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Nouvel exercice 4 (ChatClient), points clés mis à jour |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + anglais britannique |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + anglais britannique |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Anglais britannique |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Anglais britannique |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Diagramme Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Anglais britannique |
| `labs/part10-custom-models.md` | 2026-03-11 | Diagramme Mermaid, atelier terminé déplacé à la partie 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nouveau laboratoire, diagrammes Mermaid, section atelier terminé |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Nouveau : exemple d’appel d’outil |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Nouveau : exemple d’appel d’outil |
| `csharp/ToolCalling.cs` | 2026-03-10 | Nouveau : exemple d’appel d’outil |
| `csharp/Program.cs` | 2026-03-10 | Commande CLI `toolcall` ajoutée |
| `README.md` | 2026-03-10 | Partie 11, structure du projet |
| `AGENTS.md` | 2026-03-10 | Appel d’outil + conventions ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Problème #7 résolu supprimé, 6 problèmes ouverts restent |
| `csharp/csharp.csproj` | 2026-03-11 | TFM multiplateforme, références conditionnelles WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM multiplateforme, détection automatique du RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM multiplateforme, détection automatique du RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Contournement try/catch NPU supprimé |
| `csharp/SingleAgent.cs` | 2026-03-11 | Contournement try/catch NPU supprimé |
| `csharp/MultiAgent.cs` | 2026-03-11 | Contournement try/catch NPU supprimé |
| `csharp/RagPipeline.cs` | 2026-03-11 | Contournement try/catch NPU supprimé |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Contournement try/catch NPU supprimé |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Exemple de .csproj multiplateforme |
| `AGENTS.md` | 2026-03-11 | Mise à jour des packages C# et détails TFM |
| `CHANGELOG.md` | 2026-03-11 | Ce fichier |