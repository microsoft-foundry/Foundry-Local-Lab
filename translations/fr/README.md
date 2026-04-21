<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Atelier Foundry Local - Créez des applications IA en local

Un atelier pratique pour exécuter des modèles de langage sur votre propre machine et créer des applications intelligentes avec [Foundry Local](https://foundrylocal.ai) et le [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Qu’est-ce que Foundry Local ?** Foundry Local est un environnement d’exécution léger qui vous permet de télécharger, gérer et servir des modèles de langage entièrement sur votre matériel. Il expose une **API compatible OpenAI** afin que tout outil ou SDK compatible OpenAI puisse se connecter - aucun compte cloud requis.

---

## Objectifs d’apprentissage

À la fin de cet atelier, vous serez capable de :

| # | Objectif |
|---|-----------|
| 1 | Installer Foundry Local et gérer les modèles avec le CLI |
| 2 | Maîtriser l’API Foundry Local SDK pour la gestion programmatique des modèles |
| 3 | Se connecter au serveur d’inférence local via les SDK Python, JavaScript et C# |
| 4 | Construire un pipeline de génération augmentée par récupération (RAG) qui fonde les réponses sur vos propres données |
| 5 | Créer des agents IA avec des instructions et des personas persistants |
| 6 | Orchestrer des workflows multi-agents avec boucles de rétroaction |
| 7 | Explorer une application de synthèse en production - le Zava Creative Writer |
| 8 | Construire des cadres d’évaluation avec jeux de données golden et notation LLM-en-juge |
| 9 | Transcrire de l’audio avec Whisper - reconnaissance vocale en local via le Foundry Local SDK |
| 10 | Compiler et exécuter des modèles personnalisés ou Hugging Face avec ONNX Runtime GenAI et Foundry Local |
| 11 | Permettre aux modèles locaux d’appeler des fonctions externes avec le modèle d’appel d’outil |
| 12 | Créer une interface web pour le Zava Creative Writer avec streaming en temps réel |

---

## Prérequis

| Exigence | Détails |
|-------------|---------|
| **Matériel** | Minimum 8 Go RAM (16 Go recommandé) ; CPU compatible AVX2 ou GPU supporté |
| **Système d’exploitation** | Windows 10/11 (x64/ARM), Windows Server 2025, ou macOS 13+ |
| **Foundry Local CLI** | Installation via `winget install Microsoft.FoundryLocal` (Windows) ou `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Voir le [guide de démarrage](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) pour détails. |
| **Environnement d’exécution** | **Python 3.9+** et/ou **.NET 9.0+** et/ou **Node.js 18+** |
| **Git** | Pour cloner ce dépôt |

---

## Démarrage

```bash
# 1. Cloner le dépôt
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Vérifiez que Foundry Local est installé
foundry model list              # Lister les modèles disponibles
foundry model run phi-3.5-mini  # Démarrer une conversation interactive

# 3. Choisissez votre filière linguistique (voir le laboratoire Partie 2 pour l'installation complète)
```

| Langage | Démarrage rapide |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Parties de l’atelier

### Partie 1 : Premiers pas avec Foundry Local

**Guide lab :** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Qu’est-ce que Foundry Local et comment ça fonctionne
- Installation du CLI sur Windows et macOS
- Explorer les modèles : liste, téléchargement, exécution
- Comprendre les alias de modèles et ports dynamiques

---

### Partie 2 : Approfondissement du SDK Foundry Local

**Guide lab :** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Pourquoi utiliser le SDK plutôt que le CLI pour le développement d’applications
- Référence complète de l’API SDK pour Python, JavaScript et C#
- Gestion du service, navigation dans le catalogue, cycle de vie du modèle (téléchargement, chargement, déchargement)
- Modèles de démarrage rapide : bootstrap constructeur Python, `init()` JavaScript, `CreateAsync()` C#
- Métadonnées `FoundryModelInfo`, alias et sélection optimale du modèle selon le matériel

---

### Partie 3 : SDK et API

**Guide lab :** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Connexion à Foundry Local depuis Python, JavaScript, et C#
- Utiliser le SDK Foundry Local pour gérer le service de façon programmatique
- Streaming des complétions de chat via l’API compatible OpenAI
- Référence des méthodes SDK pour chaque langage

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat de base en streaming |
| C# | `csharp/BasicChat.cs` | Chat en streaming avec .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat en streaming avec Node.js |

---

### Partie 4 : Génération augmentée par récupération (RAG)

**Guide lab :** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Qu’est-ce que RAG et son importance
- Construire une base de connaissances en mémoire
- Récupération par chevauchement de mots-clés avec scoring
- Composer des prompts système fondés
- Exécuter un pipeline RAG complet en local

**Exemples de code :**

| Langage | Fichier |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Partie 5 : Création d’agents IA

**Guide lab :** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Qu’est-ce qu’un agent IA (vs. un appel LLM brut)
- Le pattern `ChatAgent` et Microsoft Agent Framework
- Instructions système, personas, conversations multi-tours
- Sortie structurée (JSON) des agents

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent unique avec Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agent unique (pattern ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agent unique (pattern ChatAgent) |

---

### Partie 6 : Workflows multi-agents

**Guide lab :** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipelines multi-agents : Chercheur → Rédacteur → Éditeur
- Orchestration séquentielle et boucles de feedback
- Configuration partagée et transfert structuré
- Concevoir votre propre workflow multi-agents

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline à trois agents |
| C# | `csharp/MultiAgent.cs` | Pipeline à trois agents |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline à trois agents |

---

### Partie 7 : Zava Creative Writer - Application de synthèse

**Guide lab :** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Application multi-agents de style production avec 4 agents spécialisés
- Pipeline séquentiel avec boucles de rétroaction pilotées par évaluateur
- Sortie en streaming, recherche dans catalogue produit, transferts JSON structurés
- Implémentation complète en Python (FastAPI), JavaScript (Node.js CLI), et C# (console .NET)

**Exemples de code :**

| Langage | Répertoire | Description |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Service web FastAPI avec orchestrateur |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Application CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Application console .NET 9 |

---

### Partie 8 : Développement guidé par l’évaluation

**Guide lab :** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Construire un cadre d’évaluation systématique pour agents IA avec jeux de données golden
- Vérifications basées sur règles (longueur, couverture de mots-clés, termes interdits) + notation LLM-en-juge
- Comparaison côte à côte de variantes de prompts avec tableaux de scores agrégés
- Prolonge le pattern agent Éditeur Zava de la partie 7 en suite de tests hors ligne
- Parcours Python, JavaScript, et C#

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Cadre d’évaluation |
| C# | `csharp/AgentEvaluation.cs` | Cadre d’évaluation |
| JavaScript | `javascript/foundry-local-eval.mjs` | Cadre d’évaluation |

---

### Partie 9 : Transcription vocale avec Whisper

**Guide lab :** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Transcription de la parole en texte avec OpenAI Whisper localement
- Traitement audio respectueux de la vie privée - l’audio ne quitte jamais votre appareil
- Parcours Python, JavaScript, et C# avec `client.audio.transcriptions.create()` (Python/JS) et `AudioClient.TranscribeAudioAsync()` (C#)
- Contient des fichiers audio exemples à thème Zava pour pratique

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transcription vocale Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transcription vocale Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transcription vocale Whisper |

> **Note :** Ce lab utilise le **Foundry Local SDK** pour télécharger et charger programmatiquement le modèle Whisper, puis envoie l’audio à l’endpoint local compatible OpenAI pour transcription. Le modèle Whisper (`whisper`) est listé dans le catalogue Foundry Local et s’exécute entièrement en local - aucune clé API cloud ou accès réseau requis.

---

### Partie 10 : Utilisation de modèles personnalisés ou Hugging Face

**Guide lab :** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compiler les modèles Hugging Face au format ONNX optimisé avec ONNX Runtime GenAI model builder
- Compilation spécifique au matériel (CPU, GPU NVIDIA, DirectML, WebGPU) et quantification (int4, fp16, bf16)
- Créer les fichiers de configuration de template de chat pour Foundry Local
- Ajouter les modèles compilés au cache Foundry Local
- Exécuter les modèles personnalisés via CLI, API REST, et SDK OpenAI
- Exemple de référence : compilation bout-en-bout de Qwen/Qwen3-0.6B

---

### Partie 11 : Appel d’outils avec modèles locaux

**Guide lab :** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Permettre aux modèles locaux d’appeler des fonctions externes (appel d’outil/fonction)
- Définir les schémas d’outil selon le format OpenAI function-calling
- Gérer la conversation multi-tours d’appel d’outil
- Exécuter les appels d’outil localement et retourner les résultats au modèle
- Choisir le bon modèle pour les scénarios d’appel d’outil (Qwen 2.5, Phi-4-mini)
- Utiliser le `ChatClient` natif du SDK pour appel d’outil (JavaScript)

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Appel d’outil avec outils météo/population |
| C# | `csharp/ToolCalling.cs` | Appel d’outil avec .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Appel d’outil avec ChatClient |

---

### Partie 12 : Création d’une interface web pour le Zava Creative Writer

**Guide lab :** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Ajouter une interface navigateur pour le Zava Creative Writer
- Servir l’interface partagée depuis Python (FastAPI), JavaScript (HTTP Node.js), et C# (ASP.NET Core)
- Consommer du NDJSON en streaming dans le navigateur avec Fetch API et ReadableStream
- Badges de statut agent en direct et streaming en temps réel du texte des articles

**Code (interface partagée) :**

| Fichier | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Mise en page de la page |
| `zava-creative-writer-local/ui/style.css` | Styles |
| `zava-creative-writer-local/ui/app.js` | Lecteur de flux et logique de mise à jour du DOM |

**Ajouts Backend :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Mis à jour pour servir l’interface statique |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nouveau serveur HTTP encapsulant l’orchestrateur |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nouveau projet minimal API ASP.NET Core |

---

### Partie 13 : Atelier terminé
**Guide du laboratoire :** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Résumé de tout ce que vous avez construit à travers les 12 parties
- Idées supplémentaires pour étendre vos applications
- Liens vers des ressources et documentation

---

## Structure du projet

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## Ressources

| Ressource | Lien |
|----------|------|
| Site Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Catalogue de modèles | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Guide de démarrage | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Référence SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licence

Ce matériel d'atelier est fourni à des fins éducatives.

---

**Bonne construction ! 🚀**