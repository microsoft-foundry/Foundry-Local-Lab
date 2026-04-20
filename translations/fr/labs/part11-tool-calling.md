![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partie 11 : Appel d’outils avec des modèles locaux

> **Objectif :** Permettre à votre modèle local d’appeler des fonctions externes (outils) pour qu’il puisse récupérer des données en temps réel, effectuer des calculs ou interagir avec des API — le tout en s’exécutant en privé sur votre appareil.

## Qu’est-ce que l’appel d’outils ?

L’appel d’outils (également appelé **appel de fonction**) permet à un modèle de langage de demander l’exécution de fonctions que vous définissez. Au lieu de deviner une réponse, le modèle reconnaît quand un outil serait utile et renvoie une requête structurée à votre code pour exécution. Votre application exécute la fonction, renvoie le résultat, et le modèle intègre cette information dans sa réponse finale.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Ce schéma est essentiel pour construire des agents capables de :

- **Rechercher des données en direct** (météo, cours boursiers, requêtes en base de données)
- **Effectuer des calculs précis** (mathématiques, conversions d’unités)
- **Prendre des actions** (envoyer des emails, créer des tickets, mettre à jour des enregistrements)
- **Accéder à des systèmes privés** (API internes, systèmes de fichiers)

---

## Comment fonctionne l’appel d’outils

Le processus d’appel d’outils comporte quatre étapes :

| Étape | Ce qui se passe |
|-------|-----------------|
| **1. Définir les outils** | Vous décrivez les fonctions disponibles à l’aide de JSON Schema — nom, description et paramètres |
| **2. Décision du modèle** | Le modèle reçoit votre message plus les définitions des outils. S’il juge qu’un outil est utile, il renvoie une réponse `tool_calls` au lieu d’une réponse textuelle |
| **3. Exécution locale** | Votre code analyse l’appel d’outil, exécute la fonction et collecte le résultat |
| **4. Réponse finale** | Vous renvoyez le résultat à l’outil au modèle, qui produit sa réponse finale |

> **Point clé :** Le modèle n’exécute jamais de code. Il *demande seulement* qu’un outil soit appelé. C’est votre application qui décide d’honorer ou non cette requête — vous gardez ainsi un contrôle total.

---

## Quels modèles supportent l’appel d’outils ?

Tous les modèles ne supportent pas l’appel d’outils. Dans le catalogue actuel de Foundry Local, les modèles suivants proposent cette fonctionnalité :

| Modèle | Taille | Appel d’outils |
|--------|--------|:-------------:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b | 6.3 GB | ✅ |
| qwen2.5-14b | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b | 6.3 GB | ✅ |
| qwen2.5-coder-14b | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4 | 10.4 GB | ❌ |

> **Astuce :** Pour ce laboratoire, nous utilisons **qwen2.5-0.5b** — il est petit (822 Mo à télécharger), rapide, et a un support fiable de l’appel d’outils.

---

## Objectifs d’apprentissage

À la fin de ce laboratoire, vous serez capable de :

- Expliquer le schéma d’appel d’outils et son importance pour les agents IA
- Définir des schémas d’outils avec le format OpenAI d’appel de fonctions
- Gérer les conversations à tours multiples avec appel d’outils
- Exécuter localement les appels d’outils et renvoyer les résultats au modèle
- Choisir le modèle adapté aux scénarios d’appel d’outils

---

## Prérequis

| Exigence | Détails |
|----------|---------|
| **Foundry Local CLI** | Installé et dans votre `PATH` ([Partie 1](part1-getting-started.md)) |
| **Foundry Local SDK** | SDK Python, JavaScript ou C# installé ([Partie 2](part2-foundry-local-sdk.md)) |
| **Un modèle avec appel d’outils** | qwen2.5-0.5b (sera téléchargé automatiquement) |

---

## Exercices

### Exercice 1 — Comprendre le flux d’appel d’outils

Avant d’écrire du code, examinez ce diagramme de séquence :

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Observations clés :**

1. Vous définissez les outils à l’avance comme des objets JSON Schema
2. La réponse du modèle contient `tool_calls` au lieu d’un contenu classique
3. Chaque appel d’outil a un `id` unique que vous devez référencer pour renvoyer les résultats
4. Le modèle voit tous les messages précédents *plus* les résultats des outils lorsqu’il génère la réponse finale
5. Plusieurs appels d’outils peuvent survenir dans une seule réponse

> **Discussion :** Pourquoi le modèle renvoie-t-il des appels d’outils plutôt que d’exécuter directement les fonctions ? Quels avantages en termes de sécurité cela procure-t-il ?

---

### Exercice 2 — Définir les schémas d’outils

Les outils sont définis avec le format standard d’appel de fonction OpenAI. Chaque outil requiert :

- **`type`** : Toujours `"function"`
- **`function.name`** : Un nom descriptif de la fonction (ex. `get_weather`)
- **`function.description`** : Une description claire — le modèle s’en sert pour décider quand appeler l’outil
- **`function.parameters`** : Un objet JSON Schema décrivant les arguments attendus

```json
{
  "type": "function",
  "function": {
    "name": "get_weather",
    "description": "Get the current weather for a given city",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. London"
        }
      },
      "required": ["city"]
    }
  }
}
```

> **Bonnes pratiques pour les descriptions d’outils :**
> - Soyez précis : "Obtenir la météo actuelle d’une ville donnée" est mieux que "Obtenir la météo"
> - Décrivez clairement les paramètres : le modèle lit ces descriptions pour remplir les bonnes valeurs
> - Marquez les paramètres obligatoires et optionnels — cela aide le modèle à savoir quoi demander

---

### Exercice 3 — Exécutez les exemples d’appel d’outils

Chaque exemple dans un langage définit deux outils (`get_weather` et `get_population`), envoie une question qui déclenche l’usage d’un outil, exécute l’outil localement, puis renvoie le résultat pour une réponse finale.

<details>
<summary><strong>🐍 Python</strong></summary>

**Prérequis :**
```bash
cd python
python -m venv venv

# Windows (PowerShell) :
venv\Scripts\Activate.ps1
# macOS / Linux :
source venv/bin/activate

pip install -r requirements.txt
```

**Exécution :**
```bash
python foundry-local-tool-calling.py
```

**Sortie attendue :**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Explication du code** (`python/foundry-local-tool-calling.py`) :

```python
# Définir les outils comme une liste de schémas de fonctions
tools = [
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "Get the current weather for a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "The city name"}
                },
                "required": ["city"]
            }
        }
    }
]

# Envoyer avec les outils — le modèle peut renvoyer des appels d'outils au lieu de contenu
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Vérifier si le modèle souhaite appeler un outil
if response.choices[0].message.tool_calls:
    # Exécuter l'outil et renvoyer le résultat
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Prérequis :**
```bash
cd javascript
npm install
```

**Exécution :**
```bash
node foundry-local-tool-calling.mjs
```

**Sortie attendue :**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Explication du code** (`javascript/foundry-local-tool-calling.mjs`) :

Cet exemple utilise le `ChatClient` natif du Foundry Local SDK plutôt que le SDK OpenAI, démontrant la commodité de la méthode `createChatClient()` :

```javascript
// Obtenir un ChatClient directement à partir de l'objet modèle
const chatClient = model.createChatClient();

// Envoyer avec des outils — ChatClient gère le format compatible OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Vérifier les appels d'outils
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Exécuter les outils et renvoyer les résultats
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Prérequis :**
```bash
cd csharp
dotnet restore
```

**Exécution :**
```bash
dotnet run toolcall
```

**Sortie attendue :**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Explication du code** (`csharp/ToolCalling.cs`) :

C# utilise l’aide `ChatTool.CreateFunctionTool` pour définir les outils :

```csharp
ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
    functionName: "get_weather",
    functionDescription: "Get the current weather for a given city",
    functionParameters: BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "city": { "type": "string", "description": "The city name" }
        },
        "required": ["city"]
    }
    """));

var options = new ChatCompletionOptions();
options.Tools.Add(getWeatherTool);

// Check FinishReason to see if tools were called
if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    // Execute tools and send results back
    ...
}
```

</details>

---

### Exercice 4 — Le flux de conversation avec appel d’outils

Comprendre la structure des messages est critique. Voici le flux complet, montrant le tableau `messages` à chaque étape :

**Étape 1 — Requête initiale :**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Étape 2 — Le modèle répond avec `tool_calls` (pas de contenu) :**
```json
{
  "role": "assistant",
  "tool_calls": [
    {
      "id": "call_abc123",
      "type": "function",
      "function": {
        "name": "get_weather",
        "arguments": "{\"city\": \"London\"}"
      }
    }
  ]
}
```

**Étape 3 — Vous ajoutez le message de l’assistant ET le résultat de l’outil :**
```json
[
  {"role": "system", "content": "..."},
  {"role": "user", "content": "What is the weather like in London?"},
  {"role": "assistant", "tool_calls": [...]},
  {
    "role": "tool",
    "tool_call_id": "call_abc123",
    "content": "{\"city\": \"London\", \"temperature\": \"18°C\", \"condition\": \"Partly cloudy\"}"
  }
]
```

**Étape 4 — Le modèle produit la réponse finale utilisant le résultat de l’outil.**

> **Important :** Le `tool_call_id` dans le message de l’outil doit correspondre à l’`id` de l’appel d’outil. C’est ainsi que le modèle associe les résultats aux requêtes.

---

### Exercice 5 — Appels multiples d’outils

Un modèle peut demander plusieurs appels d’outils dans une seule réponse. Essayez de modifier le message utilisateur pour déclencher plusieurs appels :

```python
# En Python — changer le message utilisateur :
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// En JavaScript — modifier le message utilisateur :
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Le modèle devrait renvoyer deux `tool_calls` — un pour `get_weather` et un autre pour `get_population`. Votre code gère déjà cela car il boucle sur tous les appels d’outil.

> **Essayez :** Modifiez le message utilisateur et lancez à nouveau l’exemple. Le modèle appelle-t-il les deux outils ?

---

### Exercice 6 — Ajoutez votre propre outil

Étendez un des exemples avec un nouvel outil. Par exemple, ajoutez un outil `get_time` :

1. Définissez le schéma de l’outil :
```json
{
  "type": "function",
  "function": {
    "name": "get_time",
    "description": "Get the current time in a given city's timezone",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. Tokyo"
        }
      },
      "required": ["city"]
    }
  }
}
```

2. Ajoutez la logique d’exécution :
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Dans une application réelle, utilisez une bibliothèque de fuseau horaire
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... outils existants ...
```

3. Ajoutez l’outil au tableau `tools` et testez avec : "Quelle heure est-il à Tokyo ?"

> **Défi :** Ajoutez un outil qui réalise un calcul, par exemple `convert_temperature` convertissant Celsius en Fahrenheit. Testez avec : "Convertir 100°F en Celsius."

---

### Exercice 7 — Appel d’outils avec le ChatClient du SDK (JavaScript)

L’exemple JavaScript utilise déjà le `ChatClient` natif du SDK au lieu du SDK OpenAI. C’est une fonctionnalité pratique qui évite d’avoir à construire soi-même un client OpenAI :

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient est créé directement à partir de l'objet modèle
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat accepte les outils comme second paramètre
const response = await chatClient.completeChat(messages, tools);
```

Comparez cela avec l’approche Python qui utilise explicitement le SDK OpenAI :

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Les deux schémas sont valides. Le `ChatClient` est plus pratique ; le SDK OpenAI donne accès à toute la gamme des paramètres OpenAI.

> **Essayez :** Modifiez l’exemple JavaScript pour utiliser le SDK OpenAI au lieu de `ChatClient`. Vous aurez besoin de `import OpenAI from "openai"` et de construire le client avec l’endpoint issu de `manager.urls[0]`.

---

### Exercice 8 — Comprendre `tool_choice`

Le paramètre `tool_choice` contrôle si le modèle *doit* utiliser un outil ou peut choisir librement :

| Valeur | Comportement |
|--------|--------------|
| `"auto"` | Le modèle décide s’il appelle un outil (par défaut) |
| `"none"` | Le modèle n’appellera aucun outil, même si disponibles |
| `"required"` | Le modèle doit appeler au moins un outil |
| `{"type": "function", "function": {"name": "get_weather"}}` | Le modèle doit appeler l’outil spécifié |

Testez chaque option dans l’exemple Python :

```python
# Forcer le modèle à appeler get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Remarque :** Toutes les options `tool_choice` ne sont pas forcément supportées par tous les modèles. Si un modèle ne supporte pas `"required"`, il peut ignorer cette option et se comporter comme avec `"auto"`.

---

## Pièges courants

| Problème | Solution |
|----------|----------|
| Le modèle n’appelle jamais les outils | Assurez-vous d’utiliser un modèle avec appel d’outils (ex. qwen2.5-0.5b). Vérifiez le tableau ci-dessus. |
| Incohérence de `tool_call_id` | Utilisez toujours l’`id` de la réponse d’appel d’outil, pas une valeur codée en dur |
| Le modèle renvoie un JSON malformé dans `arguments` | Les petits modèles peuvent produire un JSON invalide. Entourez `JSON.parse()` d’un try/catch |
| Le modèle appelle un outil qui n’existe pas | Ajoutez un gestionnaire par défaut dans votre fonction `execute_tool` |
| Boucle infinie d’appel d’outils | Limitez le nombre maximal de tours (ex. 5) pour éviter les boucles sans fin |

---

## Points clés à retenir

1. **L’appel d’outils** permet aux modèles de demander l’exécution de fonctions plutôt que de deviner des réponses
2. Le modèle **n’exécute jamais de code** ; votre application décide ce qu’elle exécute
3. Les outils sont définis comme des objets **JSON Schema** suivant le format OpenAI d’appel de fonction
4. La conversation utilise un **schéma à tours multiples** : utilisateur, puis assistant (tool_calls), puis outil (résultats), puis assistant (réponse finale)
5. Toujours utiliser un **modèle supportant l’appel d’outils** (Qwen 2.5, Phi-4-mini)
6. Le SDK offre `createChatClient()` pour faire des requêtes d’appel d’outils sans construire un client OpenAI

---

Continuez avec [Partie 12 : Construire une interface web pour le Zava Creative Writer](part12-zava-ui.md) pour ajouter une interface navigateur au pipeline multi-agents avec streaming en temps réel.

---

[← Partie 10 : Modèles personnalisés](part10-custom-models.md) | [Partie 12 : Interface du Zava Writer →](part12-zava-ui.md)