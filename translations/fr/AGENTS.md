# Instructions pour l'agent de codage

Ce fichier fournit le contexte pour les agents de codage IA (GitHub Copilot, Copilot Workspace, Codex, etc.) travaillant dans ce dépôt.

## Présentation du projet

Il s'agit d’un **atelier pratique** pour construire des applications IA avec [Foundry Local](https://foundrylocal.ai) — un environnement d’exécution léger qui télécharge, gère et sert des modèles de langage entièrement sur l’appareil via une API compatible OpenAI. L’atelier inclut des guides de laboratoire étape par étape et des exemples de code exécutables en Python, JavaScript et C#.

## Structure du dépôt

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Détails sur les langages et frameworks

### Python
- **Emplacement :** `python/`, `zava-creative-writer-local/src/api/`
- **Dépendances :** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Packages clés :** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Version minimale :** Python 3.9+
- **Exécution :** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Emplacement :** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Dépendances :** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Packages clés :** `foundry-local-sdk`, `openai`
- **Système de module :** modules ES (`.mjs` fichiers, `"type": "module"`)
- **Version minimale :** Node.js 18+
- **Exécution :** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Emplacement :** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Fichiers projet :** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Packages clés :** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — surensemble avec QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Cible :** .NET 9.0 (TFM conditionnel : `net9.0-windows10.0.26100` sur Windows, `net9.0` ailleurs)
- **Exécution :** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Conventions de codage

### Général
- Tous les exemples de code sont des **exemples autonomes mono-fichiers** — pas de bibliothèques utilitaires partagées ou d’abstractions.
- Chaque exemple s’exécute indépendamment après installation de ses propres dépendances.
- Les clés API sont toujours définies à `"foundry-local"` — Foundry Local utilise cela comme un espace réservé.
- Les URL de base utilisent `http://localhost:<port>/v1` — le port est dynamique et découvert à l’exécution via le SDK (`manager.urls[0]` en JS, `manager.endpoint` en Python).
- Le SDK Foundry Local gère le démarrage du service et la découverte des points de terminaison ; privilégiez les patterns SDK plutôt que des ports codés en dur.

### Python
- Utiliser le SDK `openai` avec `OpenAI(base_url=..., api_key="not-required")`.
- Utiliser `FoundryLocalManager()` de `foundry_local` pour le cycle de vie du service géré par le SDK.
- Streaming : itérer sur l’objet `stream` avec `for chunk in stream:`.
- Pas d’annotations de type dans les fichiers exemples (pour garder les exemples concis pour les apprenants de l’atelier).

### JavaScript
- Syntaxe module ES : `import ... from "..."`.
- Utiliser `OpenAI` de `"openai"` et `FoundryLocalManager` de `"foundry-local-sdk"`.
- Pattern d’initialisation SDK : `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming : `for await (const chunk of stream)`.
- `await` au niveau supérieur utilisé partout.

### C#
- Nullable activé, usings implicites, .NET 9.
- Utiliser `FoundryLocalManager.StartServiceAsync()` pour le cycle de vie géré par SDK.
- Streaming : `CompleteChatStreaming()` avec `foreach (var update in completionUpdates)`.
- Le fichier principal `csharp/Program.cs` est un routeur CLI dispatchant vers des méthodes statiques `RunAsync()`.

### Appel d’outils
- Seuls certains modèles prennent en charge l’appel d’outils : famille **Qwen 2.5** (`qwen2.5-*`) et **Phi-4-mini** (`phi-4-mini`).
- Les schémas d’outils suivent le format JSON d’appel de fonction OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- La conversation suit un schéma multi-tours : utilisateur → assistant (appels d’outils) → outil (résultats) → assistant (réponse finale).
- Le `tool_call_id` dans les messages résultats des outils doit correspondre à l’`id` de l’appel d’outil du modèle.
- Python utilise directement le SDK OpenAI ; JavaScript utilise le `ChatClient` natif du SDK (`model.createChatClient()`) ; C# utilise le SDK OpenAI avec `ChatTool.CreateFunctionTool()`.

### ChatClient (Client SDK natif)
- JavaScript : `model.createChatClient()` retourne un `ChatClient` avec `completeChat(messages, tools?)` et `completeStreamingChat(messages, callback)`.
- C# : `model.GetChatClientAsync()` retourne un `ChatClient` standard utilisable sans importer le package NuGet OpenAI.
- Python n’a pas de ChatClient natif — utiliser le SDK OpenAI avec `manager.endpoint` et `manager.api_key`.
- **Important :** le `completeStreamingChat` JavaScript utilise un **pattern callback**, pas une itération asynchrone.

### Modèles de raisonnement
- `phi-4-mini-reasoning` encadre sa réflexion dans des balises `<think>...</think>` avant la réponse finale.
- Analyser les balises pour séparer le raisonnement de la réponse quand nécessaire.

## Guides de laboratoires

Les fichiers de laboratoire sont dans `labs/` au format Markdown. Ils suivent une structure cohérente :
- Image d’en-tête logo
- Titre et appel à l’objectif
- Vue d’ensemble, objectifs d’apprentissage, prérequis
- Sections d’explications conceptuelles avec diagrammes
- Exercices numérotés avec blocs de code et sorties attendues
- Tableau résumé, points clés, lectures complémentaires
- Lien de navigation vers la partie suivante

Lors de l’édition du contenu des labs :
- Conservez le style de formatage Markdown existant et la hiérarchie des sections.
- Les blocs de code doivent spécifier la langue (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Fournir les variantes bash et PowerShell pour les commandes shell lorsque le système d’exploitation importe.
- Utiliser les styles d’appel `> **Note :**`, `> **Astuce :**`, et `> **Dépannage :**`.
- Les tableaux utilisent le format pipe `| En-tête | En-tête |`.

## Commandes de construction & test

| Action | Commande |
|--------|---------|
| **Exemples Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Exemples JS** | `cd javascript && npm install && node <script>.mjs` |
| **Exemples C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **CLI Foundry Local** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Générer des diagrammes** | `npx mmdc -i <input>.mmd -o <output>.svg` (requiert root `npm install`) |

## Dépendances externes

- **CLI Foundry Local** doit être installé sur la machine du développeur (`winget install Microsoft.FoundryLocal` ou `brew install foundrylocal`).
- **Le service Foundry Local** s’exécute localement et expose une API REST compatible OpenAI sur un port dynamique.
- Aucun service cloud, clé API ni abonnement Azure requis pour exécuter les exemples.
- La partie 10 (modèles personnalisés) nécessite en plus `onnxruntime-genai` et télécharge les poids de modèle depuis Hugging Face.

## Fichiers à ne pas committer

Le fichier `.gitignore` doit exclure (et exclut pour la plupart) :
- `.venv/` — environnements virtuels Python
- `node_modules/` — dépendances npm
- `models/` — sortie de modèles ONNX compilés (gros fichiers binaires, générés par la partie 10)
- `cache_dir/` — cache de téléchargement de modèles Hugging Face
- `.olive-cache/` — répertoire de travail Microsoft Olive
- `samples/audio/*.wav` — exemples audio générés (recréés via `python samples/audio/generate_samples.py`)
- Artéfacts standards de build Python (`__pycache__/`, `*.egg-info/`, `dist/`, etc.)

## Licence

MIT — voir `LICENSE`.