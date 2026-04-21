![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partie 10 : Utiliser des modèles personnalisés ou Hugging Face avec Foundry Local

> **Objectif :** Compiler un modèle Hugging Face au format ONNX optimisé requis par Foundry Local, le configurer avec un modèle de chat, l'ajouter au cache local, et exécuter des inférences via CLI, API REST et SDK OpenAI.

## Vue d'ensemble

Foundry Local est livré avec un catalogue sélectionné de modèles précompilés, mais vous n'êtes pas limité à cette liste. Tout modèle de langage basé sur les transformers disponible sur [Hugging Face](https://huggingface.co/) (ou stocké localement au format PyTorch / Safetensors) peut être compilé en modèle ONNX optimisé et servi via Foundry Local.

La chaîne de compilation utilise le **ONNX Runtime GenAI Model Builder**, un outil en ligne de commande inclus dans le package `onnxruntime-genai`. Le compilateur gère les étapes lourdes : téléchargement des poids sources, conversion au format ONNX, application de quantification (int4, fp16, bf16) et génération des fichiers de configuration (y compris le modèle de chat et le tokeniseur) attendus par Foundry Local.

Dans ce laboratoire, vous compilerez **Qwen/Qwen3-0.6B** depuis Hugging Face, l’enregistrerez avec Foundry Local, et discuterez avec lui entièrement sur votre dispositif.

---

## Objectifs d’apprentissage

À la fin de ce laboratoire, vous serez capable de :

- Expliquer pourquoi la compilation de modèles personnalisés est utile et dans quels cas elle est nécessaire
- Installer le model builder ONNX Runtime GenAI
- Compiler un modèle Hugging Face au format ONNX optimisé avec une seule commande
- Comprendre les paramètres clés de compilation (fournisseur d’exécution, précision)
- Créer le fichier de configuration `inference_model.json` pour le modèle de chat
- Ajouter un modèle compilé au cache de Foundry Local
- Exécuter des inférences sur le modèle personnalisé via CLI, API REST et SDK OpenAI

---

## Pré-requis

| Exigence | Détails |
|-------------|---------|
| **Foundry Local CLI** | Installé et dans votre `PATH` ([Partie 1](part1-getting-started.md)) |
| **Python 3.10+** | Requis par le model builder ONNX Runtime GenAI |
| **pip** | Gestionnaire de paquets Python |
| **Espace disque** | Au moins 5 Go libres pour les fichiers sources et compilés |
| **Compte Hugging Face** | Certains modèles requièrent l’acceptation d’une licence avant téléchargement. Qwen3-0.6B utilise la licence Apache 2.0 et est librement disponible. |

---

## Configuration de l’environnement

La compilation du modèle nécessite plusieurs gros paquets Python (PyTorch, ONNX Runtime GenAI, Transformers). Créez un environnement virtuel dédié pour éviter d’interférer avec votre Python système ou d’autres projets.

```bash
# Depuis la racine du dépôt
python -m venv .venv
```

Activez l’environnement :

**Windows (PowerShell) :**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux :**
```bash
source .venv/bin/activate
```

Mettez à jour pip pour éviter les problèmes de résolution de dépendances :

```bash
python -m pip install --upgrade pip
```

> **Astuce :** Si vous avez déjà un `.venv` des laboratoires précédents, vous pouvez le réutiliser. Veillez simplement à l’activer avant de continuer.

---

## Concept : La chaîne de compilation

Foundry Local exige des modèles au format ONNX avec une configuration ONNX Runtime GenAI. La plupart des modèles open source sur Hugging Face sont distribués sous forme de poids PyTorch ou Safetensors, donc une étape de conversion est nécessaire.

![Pipeline de compilation de modèle personnalisé](../../../images/custom-model-pipeline.svg)

### Que fait le Model Builder ?

1. **Télécharge** le modèle source depuis Hugging Face (ou le lit depuis un chemin local).
2. **Convertit** les poids PyTorch / Safetensors au format ONNX.
3. **Quantifie** le modèle dans une précision plus petite (par exemple int4) pour réduire la mémoire et améliorer le débit.
4. **Génère** la configuration ONNX Runtime GenAI (`genai_config.json`), le modèle de chat (`chat_template.jinja`) et tous les fichiers du tokeniseur pour que Foundry Local puisse charger et servir le modèle.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Vous pouvez rencontrer des références à **Microsoft Olive** comme un outil alternatif pour l’optimisation de modèles. Les deux outils produisent des modèles ONNX, mais ils servent à des fins différentes avec des compromis distincts :

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Package** | `onnxruntime-genai` | `olive-ai` |
| **Objectif principal** | Conversion et quantification de modèles d’IA générative pour ONNX Runtime GenAI | Cadre d’optimisation de modèle complet supportant plusieurs backends et cibles matérielles |
| **Facilité d’utilisation** | Commande unique — conversion + quantification en une étape | Basé sur workflow — pipelines multi-pass configurables avec YAML/JSON |
| **Format de sortie** | Format ONNX Runtime GenAI (prêt pour Foundry Local) | ONNX générique, ONNX Runtime GenAI, ou autres selon le workflow |
| **Cibles matérielles** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, et plus |
| **Options de quantification** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus optimisations de graph, réglages couche par couche |
| **Portée du modèle** | Modèles d’IA générative (LLMs, SLMs) | Tout modèle convertible en ONNX (vision, NLP, audio, multimodal) |
| **Mieux adapté pour** | Compilation rapide d’un seul modèle pour inférence locale | Pipelines de production nécessitant un contrôle fin de l’optimisation |
| **Empreinte de dépendances** | Modérée (PyTorch, Transformers, ONNX Runtime) | Plus lourde (ajoute le cadre Olive, extras optionnels par workflow) |
| **Intégration Foundry Local** | Directe — sortie immédiatement compatible | Nécessite l’option `--use_ort_genai` et une configuration supplémentaire |

> **Pourquoi ce laboratoire utilise le Model Builder :** Pour la tâche de compiler un seul modèle Hugging Face et l’enregistrer dans Foundry Local, le Model Builder est la voie la plus simple et fiable. Il produit le format exact attendu par Foundry Local en une seule commande. Si vous avez besoin plus tard de fonctions avancées d’optimisation — telles que la quantification consciente de la précision, la chirurgie de graph, ou le réglage multi-pass — Olive est une option puissante à explorer. Voir la [documentation Microsoft Olive](https://microsoft.github.io/Olive/) pour plus de détails.

---

## Exercices du laboratoire

### Exercice 1 : Installer le ONNX Runtime GenAI Model Builder

Installez le package ONNX Runtime GenAI, qui inclut l’outil model builder :

```bash
pip install onnxruntime-genai
```

Vérifiez l’installation en confirmant que le model builder est disponible :

```bash
python -m onnxruntime_genai.models.builder --help
```

Vous devriez voir une aide listant les paramètres tels que `-m` (nom du modèle), `-o` (chemin de sortie), `-p` (précision), et `-e` (fournisseur d’exécution).

> **Note :** Le model builder dépend de PyTorch, Transformers, et plusieurs autres paquets. L’installation peut prendre quelques minutes.

---

### Exercice 2 : Compiler Qwen3-0.6B pour CPU

Exécutez la commande suivante pour télécharger le modèle Qwen3-0.6B depuis Hugging Face et le compiler pour inférence CPU avec quantification int4 :

**macOS / Linux :**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows (PowerShell) :**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### Fonction de chaque paramètre

| Paramètre | But | Valeur utilisée |
|-----------|-----|-----------------|
| `-m` | ID du modèle Hugging Face ou chemin local | `Qwen/Qwen3-0.6B` |
| `-o` | Répertoire où le modèle ONNX compilé sera sauvegardé | `models/qwen3` |
| `-p` | Précision de quantification appliquée lors de la compilation | `int4` |
| `-e` | Fournisseur d’exécution ONNX Runtime (matériel cible) | `cpu` |
| `--extra_options hf_token=false` | Ignore l’authentification Hugging Face (ok pour modèles publics) | `hf_token=false` |

> **Combien de temps cela prend-il ?** La durée dépend de votre matériel et de la taille du modèle. Pour Qwen3-0.6B avec quantification int4 sur un CPU moderne, comptez entre 5 et 15 minutes environ. Les modèles plus gros prennent proportionnellement plus longtemps.

Une fois la commande terminée vous devriez voir un répertoire `models/qwen3` contenant les fichiers compilés. Vérifiez la sortie :

```bash
ls models/qwen3
```

Vous devriez voir des fichiers tels que :
- `model.onnx` et `model.onnx.data` — poids compilés du modèle
- `genai_config.json` — configuration ONNX Runtime GenAI
- `chat_template.jinja` — modèle de chat (généré automatiquement)
- `tokenizer.json`, `tokenizer_config.json` — fichiers du tokeniseur
- Autres fichiers de vocabulaire et de configuration

---

### Exercice 3 : Compiler pour GPU (optionnel)

Si vous disposez d’un GPU NVIDIA avec support CUDA, vous pouvez compiler une variante optimisée GPU pour une inférence plus rapide :

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Note :** La compilation GPU nécessite `onnxruntime-gpu` et une installation CUDA fonctionnelle. Si ces éléments ne sont pas présents, le model builder signalera une erreur. Vous pouvez passer cet exercice et continuer avec la variante CPU.

#### Référence de compilation spécifique au matériel

| Cible | Fournisseur d’exécution (`-e`) | Précision recommandée (`-p`) |
|--------|-------------------------------|-----------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` ou `int4` |
| DirectML (GPU Windows) | `dml` | `fp16` ou `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Compromis de précision

| Précision | Taille | Vitesse | Qualité |
|-----------|--------|---------|---------|
| `fp32` | La plus grande | La plus lente | La plus précise |
| `fp16` | Grande | Rapide (GPU) | Très bonne précision |
| `int8` | Petite | Rapide | Légère perte de précision |
| `int4` | La plus petite | La plus rapide | Perte de précision modérée |

Pour la plupart des développements locaux, `int4` sur CPU fournit le meilleur compromis entre vitesse et ressources. Pour une sortie de qualité production, `fp16` sur GPU CUDA est recommandé.

---

### Exercice 4 : Créer la configuration du modèle de chat

Le model builder génère automatiquement un fichier `chat_template.jinja` et un fichier `genai_config.json` dans le répertoire de sortie. Toutefois, Foundry Local a aussi besoin d’un fichier `inference_model.json` pour savoir comment formater les prompts pour votre modèle. Ce fichier définit le nom du modèle et le template qui encadre les messages utilisateurs avec les bons tokens spéciaux.

#### Étape 1 : Inspecter la sortie compilée

Listez le contenu du répertoire du modèle compilé :

```bash
ls models/qwen3
```

Vous devriez voir des fichiers tels que :
- `model.onnx` et `model.onnx.data` — poids compilés
- `genai_config.json` — configuration ONNX Runtime GenAI (générée automatiquement)
- `chat_template.jinja` — modèle de chat (généré automatiquement)
- `tokenizer.json`, `tokenizer_config.json` — fichiers du tokeniseur
- Divers autres fichiers de configuration et vocabulaire

#### Étape 2 : Générer le fichier inference_model.json

Le fichier `inference_model.json` indique à Foundry Local comment formater les prompts. Créez un script Python nommé `generate_chat_template.py` **à la racine du dépôt** (le même répertoire qui contient votre dossier `models/`) :

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Construire une conversation minimale pour extraire le modèle de chat
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# Construire la structure inference_model.json
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```

Exécutez le script depuis la racine du dépôt :

```bash
python generate_chat_template.py
```

> **Note :** Le package `transformers` était déjà installé en dépendance de `onnxruntime-genai`. Si vous obtenez une erreur `ImportError`, exécutez d’abord `pip install transformers`.

Le script produit un fichier `inference_model.json` dans le dossier `models/qwen3`. Ce fichier dit à Foundry Local comment entoure l’entrée utilisateur avec les tokens spéciaux adaptés à Qwen3.

> **Important :** Le champ `"Name"` dans `inference_model.json` (réglé sur `qwen3-0.6b` dans ce script) est **l’alias du modèle** que vous utiliserez dans toutes les commandes et appels API suivants. Si vous changez ce nom, mettez à jour le nom du modèle dans les Exercices 6 à 10 en conséquence.

#### Étape 3 : Vérifier la configuration

Ouvrez `models/qwen3/inference_model.json` et confirmez qu’il contient un champ `Name` et un objet `PromptTemplate` avec les clés `assistant` et `prompt`. Le template de prompt doit inclure des tokens spéciaux comme `<|im_start|>` et `<|im_end|>` (les tokens exacts dépendent du modèle de chat).

> **Alternative manuelle :** Si vous préférez ne pas exécuter le script, vous pouvez créer le fichier manuellement. L’exigence clé est que le champ `prompt` contienne le template complet du chat du modèle avec `{Content}` comme espace réservé pour le message utilisateur.

---

### Exercice 5 : Vérifier la structure du répertoire du modèle

Le générateur de modèle place tous les fichiers compilés directement dans le répertoire de sortie que vous avez spécifié. Vérifiez que la structure finale semble correcte :

```bash
ls models/qwen3
```

Le répertoire doit contenir les fichiers suivants :

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **Note :** Contrairement à certains autres outils de compilation, le générateur de modèle ne crée pas de sous-répertoires imbriqués. Tous les fichiers se trouvent directement dans le dossier de sortie, ce qui est exactement ce que Foundry Local attend.

---

### Exercice 6 : Ajouter le modèle au cache Foundry Local

Indiquez à Foundry Local où trouver votre modèle compilé en ajoutant le répertoire à son cache :

```bash
foundry cache cd models/qwen3
```

Vérifiez que le modèle apparaît dans le cache :

```bash
foundry cache ls
```

Vous devriez voir votre modèle personnalisé listé aux côtés des modèles précédemment mis en cache (comme `phi-3.5-mini` ou `phi-4-mini`).

---

### Exercice 7 : Exécuter le modèle personnalisé avec la CLI

Démarrez une session de chat interactive avec votre modèle nouvellement compilé (l’alias `qwen3-0.6b` provient du champ `Name` que vous avez défini dans `inference_model.json`) :

```bash
foundry model run qwen3-0.6b --verbose
```

Le paramètre `--verbose` affiche des informations diagnostiques supplémentaires, ce qui est utile lors du premier test d’un modèle personnalisé. Si le modèle se charge avec succès, vous verrez une invite interactive. Essayez quelques messages :

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Tapez `exit` ou appuyez sur `Ctrl+C` pour terminer la session.

> **Dépannage :** Si le modèle ne parvient pas à se charger, vérifiez les points suivants :
> - Le fichier `genai_config.json` a bien été généré par le générateur de modèle.
> - Le fichier `inference_model.json` existe et contient un JSON valide.
> - Les fichiers du modèle ONNX sont dans le bon répertoire.
> - Vous disposez de suffisamment de RAM disponible (Qwen3-0.6B int4 nécessite environ 1 Go).
> - Qwen3 est un modèle de raisonnement qui produit des balises `<think>`. Si vous voyez `<think>...</think>` préfixer les réponses, c’est un comportement normal. Le modèle de prompt dans `inference_model.json` peut être ajusté pour supprimer la sortie de réflexion.

---

### Exercice 8 : Interroger le modèle personnalisé via l’API REST

Si vous avez quitté la session interactive à l’Exercice 7, le modèle peut ne plus être chargé. Démarrez d’abord le service Foundry Local et chargez le modèle :

```bash
foundry service start
foundry model load qwen3-0.6b
```

Vérifiez sur quel port le service s’exécute :

```bash
foundry service status
```

Puis envoyez une requête (remplacez `5273` par votre port réel si différent) :

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Note Windows :** La commande `curl` ci-dessus utilise une syntaxe bash. Sous Windows, utilisez plutôt la cmdlet PowerShell `Invoke-RestMethod` ci-dessous.

**PowerShell :**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### Exercice 9 : Utiliser le modèle personnalisé avec le SDK OpenAI

Vous pouvez vous connecter à votre modèle personnalisé en utilisant exactement le même code SDK OpenAI que pour les modèles intégrés (voir [Partie 3](part3-sdk-and-apis.md)). La seule différence est le nom du modèle.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local ne valide pas les clés API
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local ne valide pas les clés API
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **Point clé :** Comme Foundry Local expose une API compatible OpenAI, tout code qui fonctionne avec les modèles intégrés fonctionne aussi avec vos modèles personnalisés. Il vous suffit de changer le paramètre `model`.

---

### Exercice 10 : Tester le modèle personnalisé avec le SDK Foundry Local

Dans les laboratoires précédents, vous avez utilisé le SDK Foundry Local pour démarrer le service, découvrir le point de terminaison et gérer les modèles automatiquement. Vous pouvez suivre exactement la même procédure avec votre modèle compilé personnalisé. Le SDK gère le démarrage du service et la découverte du point de terminaison, donc votre code n’a pas besoin de coder en dur `localhost:5273`.

> **Note :** Assurez-vous que le SDK Foundry Local est installé avant d’exécuter ces exemples :
> - **Python :** `pip install foundry-local openai`
> - **JavaScript :** `npm install foundry-local-sdk openai`
> - **C# :** Ajoutez les packages NuGet `Microsoft.AI.Foundry.Local` et `OpenAI`
>
> Enregistrez chaque fichier de script **à la racine du dépôt** (le même répertoire que votre dossier `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Étape 1 : Démarrer le service Foundry Local et charger le modèle personnalisé
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Étape 2 : Vérifier le cache pour le modèle personnalisé
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Étape 3 : Charger le modèle en mémoire
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Étape 4 : Créer un client OpenAI en utilisant le point de terminaison découvert par le SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Étape 5 : Envoyer une requête de complétion de chat en streaming
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

Exécutez-le :

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// Étape 1 : Démarrer le service Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Étape 2 : Obtenir le modèle personnalisé depuis le catalogue
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Étape 3 : Charger le modèle en mémoire
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Étape 4 : Créer un client OpenAI en utilisant le point de terminaison découvert par le SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Étape 5 : Envoyer une requête de complétion de chat en streaming
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

Exécutez-le :

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **Point clé :** Le SDK Foundry Local découvre dynamiquement le point de terminaison, vous ne codez donc jamais en dur un numéro de port. C’est la méthode recommandée pour les applications en production. Votre modèle compilé personnalisé fonctionne de manière identique aux modèles du catalogue intégrés via le SDK.

---

## Choisir un modèle à compiler

Qwen3-0.6B est utilisé comme exemple de référence dans ce laboratoire car il est petit, rapide à compiler et disponible gratuitement sous licence Apache 2.0. Cependant, vous pouvez compiler de nombreux autres modèles. Voici quelques suggestions :

| Modèle | ID Hugging Face | Paramètres | Licence | Remarques |
|--------|-----------------|------------|---------|-----------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Très petit, compilation rapide, bon pour les tests |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Qualité meilleure, toujours rapide à compiler |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Qualité forte, nécessite plus de RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Nécessite acceptation de licence sur Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Haute qualité, téléchargement plus lourd et compilation plus longue |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Déjà dans le catalogue Foundry Local (utile pour comparaison) |

> **Rappel licence :** Vérifiez toujours la licence du modèle sur Hugging Face avant de l'utiliser. Certains modèles (comme Llama) requièrent d’accepter un accord de licence et de vous authentifier avec `huggingface-cli login` avant de télécharger.

---

## Concepts : quand utiliser des modèles personnalisés

| Scénario | Pourquoi compiler soi-même ? |
|----------|-----------------------------|
| **Un modèle dont vous avez besoin n’est pas dans le catalogue** | Le catalogue Foundry Local est sélectionné. Si le modèle voulu n’y figure pas, compilez-le vous-même. |
| **Modèles ajustés (fine-tuning)** | Si vous avez affiné un modèle sur des données spécifiques au domaine, vous devez compiler vos propres poids. |
| **Exigences spécifiques de quantification** | Vous souhaitez peut-être une précision ou une stratégie de quantification différente de celle par défaut dans le catalogue. |
| **Nouveaux modèles publiés** | Lorsqu’un nouveau modèle est publié sur Hugging Face, il n’est peut-être pas encore intégré au catalogue Foundry Local. Le compiler vous donne un accès immédiat. |
| **Recherche et expérimentation** | Tester différentes architectures, tailles ou configurations localement avant de valider en production. |

---

## Résumé

Dans ce laboratoire, vous avez appris à :

| Étape | Ce que vous avez fait |
|-------|----------------------|
| 1 | Installer le générateur de modèle ONNX Runtime GenAI |
| 2 | Compiler `Qwen/Qwen3-0.6B` depuis Hugging Face en un modèle ONNX optimisé |
| 3 | Créer un fichier de configuration de template de chat `inference_model.json` |
| 4 | Ajouter le modèle compilé au cache Foundry Local |
| 5 | Lancer un chat interactif avec le modèle personnalisé via la CLI |
| 6 | Interroger le modèle via l’API REST compatible OpenAI |
| 7 | Se connecter depuis Python, JavaScript et C# avec le SDK OpenAI |
| 8 | Tester le modèle personnalisé de bout en bout avec le SDK Foundry Local |

L’essentiel est que **tout modèle basé sur un transformeur peut fonctionner via Foundry Local** une fois compilé au format ONNX. L’API compatible OpenAI signifie que tout votre code applicatif existant fonctionne sans modification ; il suffit de changer le nom du modèle.

---

## Points clés à retenir

| Concept | Détail |
|---------|--------|
| Générateur de modèle ONNX Runtime GenAI | Convertit les modèles Hugging Face au format ONNX avec quantification en une seule commande |
| Format ONNX | Foundry Local nécessite des modèles ONNX avec configuration ONNX Runtime GenAI |
| Templates de chat | Le fichier `inference_model.json` indique à Foundry Local comment formater les prompts pour un modèle donné |
| Cibles matérielles | Compiler pour CPU, GPU NVIDIA (CUDA), DirectML (GPU Windows) ou WebGPU selon votre matériel |
| Quantification | La précision inférieure (int4) réduit la taille et améliore la vitesse au prix d’un peu de qualité ; fp16 conserve une haute qualité sur GPU |
| Compatibilité API | Les modèles personnalisés utilisent la même API compatible OpenAI que les modèles intégrés |
| SDK Foundry Local | Le SDK gère automatiquement le démarrage du service, la découverte des points de terminaison et le chargement des modèles, qu’ils soient du catalogue ou personnalisés |

---

## Lectures complémentaires

| Ressource | Lien |
|-----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Guide des modèles personnalisés Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Famille de modèles Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Documentation Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Étapes suivantes

Continuez avec [Partie 11 : Appels d’outils avec les modèles locaux](part11-tool-calling.md) pour apprendre à permettre à vos modèles locaux d’appeler des fonctions externes.

[← Partie 9 : Transcription vocale Whisper](part9-whisper-voice-transcription.md) | [Partie 11 : Appels d’outils →](part11-tool-calling.md)