<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Atelier Foundry Local - Créez des applications IA sur l’appareil

Un atelier pratique pour exécuter des modèles de langage sur votre propre machine et créer des applications intelligentes avec [Foundry Local](https://foundrylocal.ai) et le [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Qu’est-ce que Foundry Local ?** Foundry Local est un runtime léger qui vous permet de télécharger, gérer et servir des modèles de langage directement sur votre matériel. Il expose une **API compatible OpenAI** afin que tout outil ou SDK utilisant OpenAI puisse se connecter - aucun compte cloud requis.

### 🌐 Support multilingue

#### Pris en charge via GitHub Action (Automatisé et toujours à jour)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabe](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgare](../bg/README.md) | [Birman (Myanmar)](../my/README.md) | [Chinois (Simplifié)](../zh-CN/README.md) | [Chinois (Traditionnel, Hong Kong)](../zh-HK/README.md) | [Chinois (Traditionnel, Macau)](../zh-MO/README.md) | [Chinois (Traditionnel, Taïwan)](../zh-TW/README.md) | [Croate](../hr/README.md) | [Tchèque](../cs/README.md) | [Danois](../da/README.md) | [Néerlandais](../nl/README.md) | [Estonien](../et/README.md) | [Finnois](../fi/README.md) | [Français](./README.md) | [Allemand](../de/README.md) | [Grec](../el/README.md) | [Hébreu](../he/README.md) | [Hindi](../hi/README.md) | [Hongrois](../hu/README.md) | [Indonésien](../id/README.md) | [Italien](../it/README.md) | [Japonais](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Coréen](../ko/README.md) | [Lituanien](../lt/README.md) | [Malais](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Népalais](../ne/README.md) | [Pidgin nigérian](../pcm/README.md) | [Norvégien](../no/README.md) | [Persan (Farsi)](../fa/README.md) | [Polonais](../pl/README.md) | [Portugais (Brésil)](../pt-BR/README.md) | [Portugais (Portugal)](../pt-PT/README.md) | [Pendjabi (Gurmukhi)](../pa/README.md) | [Roumain](../ro/README.md) | [Russe](../ru/README.md) | [Serbe (Cyrillique)](../sr/README.md) | [Slovaque](../sk/README.md) | [Slovène](../sl/README.md) | [Espagnol](../es/README.md) | [Swahili](../sw/README.md) | [Suédois](../sv/README.md) | [Tagalog (Philippin)](../tl/README.md) | [Tamoul](../ta/README.md) | [Telugu](../te/README.md) | [Thaï](../th/README.md) | [Turc](../tr/README.md) | [Ukrainien](../uk/README.md) | [Ourdou](../ur/README.md) | [Vietnamien](../vi/README.md)

> **Vous préférez cloner en local ?**
>
> Ce dépôt comprend plus de 50 traductions ce qui augmente significativement la taille du téléchargement. Pour cloner sans les traductions, utilisez le checkout sparse :
>
> **Bash / macOS / Linux :**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows) :**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> Cela vous fournit tout ce dont vous avez besoin pour suivre le cours avec un téléchargement beaucoup plus rapide.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Objectifs d’apprentissage

À la fin de cet atelier, vous serez capable de :

| # | Objectif |
|---|-----------|
| 1 | Installer Foundry Local et gérer les modèles avec l’interface CLI |
| 2 | Maîtriser l’API SDK de Foundry Local pour la gestion programmatique des modèles |
| 3 | Se connecter au serveur d’inférence local via les SDK Python, JavaScript et C# |
| 4 | Construire un pipeline de génération augmentée par récupération (RAG) qui fonde les réponses sur vos propres données |
| 5 | Créer des agents IA avec instructions et personas persistants |
| 6 | Orchestrer des workflows multi-agents avec boucles de rétroaction |
| 7 | Explorer une application de clôture en production - le Zava Creative Writer |
| 8 | Construire des cadres d’évaluation avec des jeux de données de référence et un système de notation LLM-en-arbitre |
| 9 | Transcrire de l’audio avec Whisper - reconnaissance vocale locale via le SDK Foundry Local |
| 10 | Compiler et exécuter des modèles personnalisés ou Hugging Face avec ONNX Runtime GenAI et Foundry Local |
| 11 | Permettre aux modèles locaux d’appeler des fonctions externes avec le modèle de tool-calling |
| 12 | Créer une interface utilisateur navigateur pour Zava Creative Writer avec streaming en temps réel |

---

## Prérequis

| Exigence | Détails |
|-------------|---------|
| **Matériel** | Au minimum 8 Go de RAM (16 Go recommandés) ; CPU avec AVX2 ou GPU compatible |
| **Système d’exploitation** | Windows 10/11 (x64/ARM), Windows Server 2025, ou macOS 13+ |
| **CLI Foundry Local** | Installer via `winget install Microsoft.FoundryLocal` (Windows) ou `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Voir le [guide de démarrage](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) pour les détails. |
| **Environnement d’exécution** | **Python 3.9+** et/ou **.NET 9.0+** et/ou **Node.js 18+** |
| **Git** | Pour cloner ce dépôt |

---

## Démarrage rapide

```bash
# 1. Cloner le dépôt
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Vérifier que Foundry Local est installé
foundry model list              # Lister les modèles disponibles
foundry model run phi-3.5-mini  # Démarrer une discussion interactive

# 3. Choisissez votre parcours linguistique (voir le laboratoire Partie 2 pour la configuration complète)
```

| Langage | Démarrage rapide |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Parties de l’atelier

### Partie 1 : Premiers pas avec Foundry Local

**Guide laboratoire :** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Qu’est-ce que Foundry Local et comment cela fonctionne
- Installation de la CLI sur Windows et macOS
- Exploration des modèles - liste, téléchargement, exécution
- Comprendre les alias de modèles et les ports dynamiques

---

### Partie 2 : Exploration approfondie du SDK Foundry Local

**Guide laboratoire :** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Pourquoi utiliser le SDK plutôt que la CLI pour le développement d’applications
- Référence complète de l’API SDK pour Python, JavaScript et C#
- Gestion du service, navigation dans le catalogue, cycle de vie des modèles (téléchargement, chargement, déchargement)
- Patrones de démarrage rapide : bootstrap constructeur Python, `init()` JavaScript, `CreateAsync()` C#
- Métadonnées `FoundryModelInfo`, alias et sélection des modèles optimaux selon le matériel

---

### Partie 3 : SDKs et APIs

**Guide laboratoire :** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Connexion à Foundry Local depuis Python, JavaScript et C#
- Utilisation du SDK Foundry Local pour gérer le service de manière programmatique
- Diffusion en continu des complétions de chat via l’API compatible OpenAI
- Référence des méthodes SDK pour chaque langage

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat en streaming basique |
| C# | `csharp/BasicChat.cs` | Chat en streaming avec .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat en streaming avec Node.js |

---

### Partie 4 : Génération augmentée par récupération (RAG)

**Guide laboratoire :** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Qu’est-ce que la RAG et pourquoi c’est important
- Construction d’une base de connaissances en mémoire
- Récupération par recoupement de mots-clés avec score
- Composition de prompts système ancrés
- Exécution d’un pipeline RAG complet sur appareil

**Exemples de code :**

| Langage | Fichier |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Partie 5 : Construction d’agents IA

**Guide laboratoire :** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Qu’est-ce qu’un agent IA (par rapport à un appel brut de LLM)
- Le pattern `ChatAgent` et le Microsoft Agent Framework
- Instructions système, personas et conversations à plusieurs tours
- Sortie structurée (JSON) des agents

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent unique avec Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agent unique (pattern ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agent unique (pattern ChatAgent) |

---

### Partie 6 : Workflows multi-agents

**Guide laboratoire :** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipelines multi-agents : Chercheur → Écrivain → Éditeur
- Orchestration séquentielle et boucles de rétroaction
- Configuration partagée et transferts structurés
- Conception de votre propre workflow multi-agents

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline à trois agents |
| C# | `csharp/MultiAgent.cs` | Pipeline à trois agents |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline à trois agents |

---

### Partie 7 : Zava Creative Writer - Application de clôture

**Guide laboratoire :** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Application multi-agents de style production avec 4 agents spécialisés
- Pipeline séquentiel avec boucles de rétroaction pilotées par évaluateur
- Sortie streaming, recherche dans le catalogue produit, transferts JSON structurés
- Implémentation complète en Python (FastAPI), JavaScript (Node.js CLI) et C# (console .NET)

**Exemples de code :**

| Langage | Répertoire | Description |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Service web FastAPI avec orchestrateur |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Application CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Application console .NET 9 |

---

### Partie 8 : Développement piloté par l’évaluation

**Guide laboratoire :** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Construire un cadre d’évaluation systématique pour les agents IA utilisant des jeux de données de référence
- Vérifications basées sur des règles (longueur, couverture de mots-clés, termes interdits) + notation LLM-en-arbitre
- Comparaison côte-à-côte des variantes de prompt avec fiches de score agrégées
- Étend le pattern Zava Editor agent de la Partie 7 dans une suite de tests hors ligne
- Parcours Python, JavaScript et C#

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Cadre d’évaluation |
| C# | `csharp/AgentEvaluation.cs` | Cadre d’évaluation |
| JavaScript | `javascript/foundry-local-eval.mjs` | Cadre d’évaluation |

---

### Partie 9 : Transcription vocale avec Whisper

**Guide laboratoire :** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Transcription vocale en texte avec OpenAI Whisper fonctionnant localement  
- Traitement audio axé sur la confidentialité - l'audio ne quitte jamais votre appareil  
- Parcours Python, JavaScript et C# avec `client.audio.transcriptions.create()` (Python/JS) et `AudioClient.TranscribeAudioAsync()` (C#)  
- Comprend des fichiers audio d'exemple thématiques Zava pour la pratique  

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transcription vocale Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transcription vocale Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transcription vocale Whisper |

> **Note :** Ce laboratoire utilise le **Foundry Local SDK** pour télécharger et charger programmétiquement le modèle Whisper, puis envoie l'audio au point de terminaison local compatible OpenAI pour la transcription. Le modèle Whisper (`whisper`) est listé dans le catalogue Foundry Local et fonctionne entièrement sur l'appareil - aucune clé d'API cloud ni accès réseau requis.

---

### Partie 10 : Utilisation de modèles personnalisés ou Hugging Face

**Guide du laboratoire :** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilation des modèles Hugging Face au format ONNX optimisé avec l'outil ONNX Runtime GenAI  
- Compilation spécifique au matériel (CPU, GPU NVIDIA, DirectML, WebGPU) et quantification (int4, fp16, bf16)  
- Création des fichiers de configuration de template de chat pour Foundry Local  
- Ajout des modèles compilés au cache de Foundry Local  
- Exécution des modèles personnalisés via la CLI, l'API REST et le SDK OpenAI  
- Exemple de référence : compilation bout en bout de Qwen/Qwen3-0.6B  

---

### Partie 11 : Appel d'outils avec des modèles locaux

**Guide du laboratoire :** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Permettre aux modèles locaux d'appeler des fonctions externes (appel d'outils/fonctions)  
- Définition des schémas d'outils utilisant le format d'appel de fonctions OpenAI  
- Gestion du flux de conversation multi-tours d'appel d'outils  
- Exécution locale des appels d'outils et renvoi des résultats au modèle  
- Choix du modèle adapté aux scénarios d'appel d'outils (Qwen 2.5, Phi-4-mini)  
- Utilisation du `ChatClient` natif du SDK pour l'appel d'outils (JavaScript)  

**Exemples de code :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Appel d'outils avec outils météo/population |
| C# | `csharp/ToolCalling.cs` | Appel d'outils avec .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Appel d'outils avec ChatClient |

---

### Partie 12 : Création d'une interface Web pour Zava Creative Writer

**Guide du laboratoire :** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Ajout d'une interface frontale basée sur navigateur à Zava Creative Writer  
- Service de l'interface partagée à partir de Python (FastAPI), JavaScript (Node.js HTTP), et C# (ASP.NET Core)  
- Consommation de NDJSON en streaming dans le navigateur avec Fetch API et ReadableStream  
- Badges de statut d'agent en direct et diffusion en temps réel du texte des articles  

**Code (UI partagée) :**

| Fichier | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Mise en page de la page |
| `zava-creative-writer-local/ui/style.css` | Style |
| `zava-creative-writer-local/ui/app.js` | Lecteur de flux et logique de mise à jour du DOM |

**Ajouts backend :**

| Langage | Fichier | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Mis à jour pour servir l'UI statique |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nouveau serveur HTTP enveloppant l'orchestrateur |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nouveau projet minimal API ASP.NET Core |

---

### Partie 13 : Atelier terminé

**Guide du laboratoire :** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Résumé de tout ce que vous avez construit dans les 12 parties  
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
| Site web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
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

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Avertissement** :  
Ce document a été traduit à l’aide du service de traduction IA [Co-op Translator](https://github.com/Azure/co-op-translator). Bien que nous nous efforcions d’assurer l’exactitude, veuillez noter que les traductions automatisées peuvent contenir des erreurs ou des inexactitudes. Le document original dans sa langue native doit être considéré comme la source faisant autorité. Pour les informations critiques, une traduction professionnelle humaine est recommandée. Nous ne sommes pas responsables des malentendus ou des mauvaises interprétations résultant de l’utilisation de cette traduction.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->