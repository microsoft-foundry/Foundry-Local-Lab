![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partie 12 : Création d’une interface Web pour le Zava Creative Writer

> **Objectif :** Ajouter une interface frontale basée sur un navigateur au Zava Creative Writer pour que vous puissiez suivre l’exécution du pipeline multi-agent en temps réel, avec des badges de statut des agents en direct et le texte de l’article diffusé en continu, le tout servi depuis un seul serveur web local.

Dans la [Partie 7](part7-zava-creative-writer.md), vous avez exploré le Zava Creative Writer en tant qu’**application CLI** (JavaScript, C#) et une **API headless** (Python). Dans ce laboratoire, vous allez connecter une interface frontale **vanilla HTML/CSS/JavaScript** partagée à chaque backend afin que les utilisateurs puissent interagir avec le pipeline via un navigateur au lieu d’un terminal.

---

## Ce que vous allez apprendre

| Objectif | Description |
|-----------|-------------|
| Servir des fichiers statiques depuis un backend | Monter un répertoire HTML/CSS/JS aux côtés de votre route API |
| Consommer du NDJSON en streaming dans le navigateur | Utiliser Fetch API avec `ReadableStream` pour lire du JSON délimité par des retours à la ligne |
| Protocole de streaming unifié | S’assurer que les backends Python, JavaScript et C# émettent le même format de message |
| Mises à jour progressives de l’interface | Mettre à jour les badges de statut des agents et diffuser le texte de l’article token par token |
| Ajouter une couche HTTP à une application CLI | Envelopper la logique de l’orchestrateur existant dans un serveur de type Express (JS) ou une API minimale ASP.NET Core (C#) |

---

## Architecture

L’interface utilisateur est un ensemble unique de fichiers statiques (`index.html`, `style.css`, `app.js`) partagé par les trois backends. Chaque backend expose les deux mêmes routes :

![Architecture UI Zava — interface partagée avec trois backends](../../../images/part12-architecture.svg)

| Route | Méthode | But |
|-------|--------|---------|
| `/` | GET | Sert l’interface statique |
| `/api/article` | POST | Lance le pipeline multi-agent et diffuse du NDJSON |

Le front end envoie un corps JSON et lit la réponse comme un flux de messages JSON délimités par des retours à la ligne. Chaque message a un champ `type` que l’interface utilise pour mettre à jour le panneau correspondant :

| Type de message | Signification |
|-------------|---------|
| `message` | Mise à jour de statut (ex. "Démarrage de la tâche de l’agent chercheur…") |
| `researcher` | Résultats de recherche disponibles |
| `marketing` | Résultats de recherche produit disponibles |
| `writer` | Rédacteur débuté ou terminé (contient `{ start: true }` ou `{ complete: true }`) |
| `partial` | Un token unique diffusé par le Rédacteur (contient `{ text: "..." }`) |
| `editor` | Verdict de l’éditeur disponible |
| `error` | Une erreur s’est produite |

![Routage des types de messages dans le navigateur](../../../images/part12-message-types.svg)

![Séquence de streaming — Communication navigateur vers backend](../../../images/part12-streaming-sequence.svg)

---

## Prérequis

- Avoir complété la [Partie 7 : Zava Creative Writer](part7-zava-creative-writer.md)
- CLI Foundry Local installée avec le modèle `phi-3.5-mini` téléchargé
- Un navigateur web récent (Chrome, Edge, Firefox ou Safari)

---

## L’interface partagée

Avant de toucher au code backend, prenez un moment pour explorer le front end que les trois langages vont utiliser. Les fichiers sont dans `zava-creative-writer-local/ui/` :

| Fichier | But |
|------|---------|
| `index.html` | Mise en page de la page : formulaire de saisie, badges de statut des agents, zone d’affichage de l’article, panneaux de détails repliables |
| `style.css` | Style minimal avec couleurs des badges d’état (en attente, en cours, terminé, erreur) |
| `app.js` | Appel Fetch, lecteur de lignes `ReadableStream` et logique de mise à jour DOM |

> **Astuce :** Ouvrez `index.html` directement dans votre navigateur pour prévisualiser la mise en page. Rien ne fonctionnera encore car aucun backend n’est actif, mais vous pouvez voir la structure.

### Comment fonctionne le lecteur de flux

La fonction clé dans `app.js` lit le corps de la réponse par morceaux et découpe selon les retours à la ligne :

```javascript
async function readStream(response) {
  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = "";

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;

    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split("\n");
    buffer = lines.pop(); // garder la ligne incomplète en fin de fichier

    for (const line of lines) {
      const trimmed = line.trim();
      if (!trimmed) continue;
      try {
        const msg = JSON.parse(trimmed);
        if (msg && msg.type) handleMessage(msg);
      } catch { /* skip non-JSON lines */ }
    }
  }
}
```

Chaque message analysé est envoyé à `handleMessage()`, qui met à jour l’élément DOM approprié en fonction de `msg.type`.

---

## Exercices

### Exercice 1 : Exécuter le backend Python avec l’interface

La variante Python (FastAPI) dispose déjà d’un point d’accès API en streaming. Le seul changement est de monter le dossier `ui/` comme fichiers statiques.

**1.1** Allez dans le répertoire de l’API Python et installez les dépendances :

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Démarrez le serveur :

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Ouvrez votre navigateur à `http://localhost:8000`. Vous devriez voir l’interface Zava Creative Writer avec trois champs de texte et un bouton "Generate Article".

**1.4** Cliquez sur **Generate Article** avec les valeurs par défaut. Observez les badges de statut des agents passer de "Waiting" à "Running" puis "Done" au fur et à mesure que chaque agent termine son travail, et voyez le texte s’afficher dans le panneau de sortie token par token.

> **Dépannage :** Si la page affiche une réponse JSON au lieu de l’interface, assurez-vous d’exécuter le fichier `main.py` mis à jour qui monte les fichiers statiques. Le point `/api/article` fonctionne toujours à son chemin d’origine ; le montage des fichiers statiques sert l’interface sur toutes les autres routes.

**Comment ça marche :** Le `main.py` mis à jour ajoute une ligne en bas :

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Cela sert tous les fichiers depuis `zava-creative-writer-local/ui/` comme ressources statiques, avec `index.html` comme document par défaut. La route POST `/api/article` est enregistrée avant ce montage, elle a donc la priorité.

---

### Exercice 2 : Ajouter un serveur Web à la variante JavaScript

La variante JavaScript est actuellement une application CLI (`main.mjs`). Un nouveau fichier, `server.mjs`, enveloppe les mêmes modules agents derrière un serveur HTTP et sert l’interface partagée.

**2.1** Allez dans le répertoire JavaScript et installez les dépendances :

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Démarrez le serveur web :

```bash
node server.mjs
```

```powershell
node server.mjs
```

Vous devriez voir :

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Ouvrez `http://localhost:3000` dans votre navigateur et cliquez sur **Generate Article**. La même interface fonctionne de manière identique avec le backend JavaScript.

**Étudiez le code :** Ouvrez `server.mjs` et notez les points essentiels :

- Le **service des fichiers statiques** utilise les modules natifs Node.js `http`, `fs` et `path` sans framework externe.
- La **protection contre le parcours de chemin** normalise le chemin demandé et vérifie qu’il reste dans le dossier `ui/`.
- Le **streaming NDJSON** utilise une fonction d’aide `sendLine()` qui sérialise chaque objet, supprime les retours à la ligne internes et ajoute un retour à la ligne final.
- L’**orchestration des agents** réutilise sans modification les modules `researcher.mjs`, `product.mjs`, `writer.mjs` et `editor.mjs`.

<details>
<summary>Extrait clé de server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Chercheur
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Écrivain (diffusion en continu)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Exercice 3 : Ajouter une API minimale à la variante C#

La variante C# est actuellement une application console. Un nouveau projet, `csharp-web`, utilise ASP.NET Core minimal APIs pour exposer le même pipeline comme service web.

**3.1** Allez dans le projet web C# et restaurez les paquets :

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Lancez le serveur web :

```bash
dotnet run
```

```powershell
dotnet run
```

Vous devriez voir :

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Ouvrez `http://localhost:5000` dans votre navigateur et cliquez sur **Generate Article**.

**Étudiez le code :** Ouvrez `Program.cs` dans le répertoire `csharp-web` et notez :

- Le fichier projet utilise `Microsoft.NET.Sdk.Web` au lieu de `Microsoft.NET.Sdk`, ce qui ajoute le support ASP.NET Core.
- Les fichiers statiques sont servis via `UseDefaultFiles` et `UseStaticFiles` pointant vers le dossier partagé `ui/`.
- Le point `/api/article` écrit directement des lignes NDJSON dans `HttpContext.Response` et flushe après chaque ligne pour du streaming en temps réel.
- Toute la logique des agents (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) est identique à la version console.

<details>
<summary>Extrait clé de csharp-web/Program.cs</summary>

```csharp
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/event-stream; charset=utf-8";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    // Researcher
    await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
    var researchResult = RunResearcher(body.Research, feedback);
    await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

    // Writer (streaming)
    foreach (var update in completionUpdates)
    {
        if (update.ContentUpdate.Count > 0)
        {
            var text = update.ContentUpdate[0].Text;
            await SendLine(new { type = "partial", message = "token", data = new { text } });
        }
    }
});
```

</details>

---

### Exercice 4 : Explorer les badges de statut des agents

Maintenant que vous avez une interface fonctionnelle, regardez comment le front end met à jour les badges de statut.

**4.1** Ouvrez `zava-creative-writer-local/ui/app.js` dans votre éditeur.

**4.2** Trouvez la fonction `handleMessage()`. Observez comment elle associe les types de message à des mises à jour DOM :

| Type de message | Action UI |
|-------------|-----------|
| `message` contenant "researcher" | Définit le badge Researcher sur "Running" |
| `researcher` | Définit le badge Researcher sur "Done" et remplit le panneau Résultats de Recherche |
| `marketing` | Définit le badge Product Search sur "Done" et remplit le panneau Correspondances Produit |
| `writer` avec `data.start` | Définit le badge Writer sur "Running" et vide la sortie de l’article |
| `partial` | Ajoute le texte du token à la sortie de l’article |
| `writer` avec `data.complete` | Définit le badge Writer sur "Done" |
| `editor` | Définit le badge Editor sur "Done" et remplit le panneau Retour de l’Éditeur |

**4.3** Ouvrez les panneaux repliables "Résultats de Recherche", "Correspondances Produit" et "Retour de l’Éditeur" sous l’article pour inspecter le JSON brut produit par chaque agent.

---

### Exercice 5 : Personnaliser l’interface (optionnel)

Essayez une ou plusieurs des améliorations suivantes :

**5.1 Ajouter un compteur de mots.** Après la fin du Writer, affichez le nombre de mots de l’article sous le panneau de sortie. Vous pouvez calculer cela dans `handleMessage` quand `type === "writer"` et `data.complete` est vrai :

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Ajouter un indicateur de nouvelle tentative.** Quand l’Éditeur demande une révision, le pipeline est relancé. Affichez une bannière "Révision 1" ou "Révision 2" dans le panneau de statut. Écoutez un type `message` contenant "Revision" et mettez à jour un nouvel élément DOM.

**5.3 Mode sombre.** Ajoutez un bouton bascule et une classe `.dark` au `<body>`. Surchargez les couleurs de fond, de texte et des panneaux dans `style.css` via un sélecteur `body.dark`.

---

## Résumé

| Ce que vous avez fait | Comment |
|-------------|-----|
| Servi l’interface depuis le backend Python | Monté le dossier `ui/` avec `StaticFiles` dans FastAPI |
| Ajouté un serveur HTTP à la variante JavaScript | Créé `server.mjs` utilisant le module natif `http` de Node.js |
| Ajouté une API web à la variante C# | Créé un nouveau projet `csharp-web` avec des minimal APIs ASP.NET Core |
| Consommé du NDJSON en streaming dans le navigateur | Utilisé `fetch()` avec `ReadableStream` et parsing JSON ligne par ligne |
| Mis à jour l’interface en temps réel | Mappé les types de messages aux mises à jour DOM (badges, texte, panneaux repliables) |

---

## Points clés à retenir

1. Un **front end statique partagé** peut fonctionner avec n’importe quel backend parlant le même protocole de streaming, renforçant la valeur du modèle d’API compatible OpenAI.
2. Le **JSON délimité par des retours à la ligne (NDJSON)** est un format de streaming simple qui fonctionne nativement avec l’API `ReadableStream` du navigateur.
3. La **variante Python** a demandé le moins de modifications car elle possédait déjà un point FastAPI ; les variantes JavaScript et C# ont nécessité un simple wrapper HTTP.
4. Garder l’UI en **vanilla HTML/CSS/JS** évite les outils de build, dépendances de frameworks, et complexité supplémentaire pour les apprenants.
5. Les mêmes modules agents (Researcher, Product, Writer, Editor) sont réutilisés sans modification ; seule la couche de transport change.

---

## Lectures complémentaires

| Ressource | Lien |
|----------|------|
| MDN : Utilisation des Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| Fichiers statiques FastAPI | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| Fichiers statiques ASP.NET Core | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Spécification NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Continuez vers la [Partie 13 : Atelier terminé](part13-workshop-complete.md) pour un résumé de tout ce que vous avez construit au cours de cet atelier.

---
[← Partie 11 : Appel d’outil](part11-tool-calling.md) | [Partie 13 : Atelier terminé →](part13-workshop-complete.md)