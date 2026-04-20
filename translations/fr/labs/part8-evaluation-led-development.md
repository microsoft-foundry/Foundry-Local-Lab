![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partie 8 : Développement guidé par l'évaluation avec Foundry Local

> **Objectif :** Construire un cadre d’évaluation qui teste et note systématiquement vos agents IA, en utilisant le même modèle local à la fois comme agent testé et comme juge, afin que vous puissiez itérer sur les invites en toute confiance avant de déployer.

## Pourquoi le développement guidé par l’évaluation ?

Lors de la création d’agents IA, « cela semble correct » ne suffit pas. Le **développement guidé par l’évaluation** considère les sorties des agents comme du code : vous écrivez d’abord des tests, mesurez la qualité, et ne déployez que lorsque les scores atteignent un seuil.

Dans le Zava Creative Writer (Partie 7), l’**agent Éditeur** agit déjà comme un évaluateur léger ; il décide ACCEPTER ou RÉVISER. Ce atelier formalise ce modèle en un cadre d’évaluation réutilisable que vous pouvez appliquer à n’importe quel agent ou pipeline.

| Problème | Solution |
|---------|----------|
| Les modifications d’invite cassent la qualité en silence | **Dataset golden** détecte les régressions |
| Biais « ça fonctionne sur un exemple » | **Plusieurs cas de test** révèlent les cas limites |
| Évaluation qualitative subjective | **Notation par règles + juge LLM** fournit des chiffres |
| Pas de moyen de comparer les variantes d’invite | **Exécutions d’évaluation côte à côte** avec scores agrégés |

---

## Concepts clés

### 1. Datasets goldens

Un **dataset golden** est un ensemble sélectionné de cas de test avec des sorties attendues connues. Chaque cas de test inclut :

- **Entrée** : L’invite ou la question envoyée à l’agent
- **Sortie attendue** : Ce qu’une réponse correcte ou de haute qualité doit contenir (mots clés, structure, faits)
- **Catégorie** : Regroupement pour le rapport (ex. « exactitude factuelle », « ton », « exhaustivité »)

### 2. Vérifications basées sur des règles

Contrôles rapides et déterministes ne nécessitant pas de LLM :

| Vérification | Ce qu’elle teste |
|--------------|------------------|
| **Limites de longueur** | La réponse n’est ni trop courte (paresseuse) ni trop longue (verbeuse) |
| **Mots clés requis** | La réponse mentionne les termes ou entités attendues |
| **Validation de format** | Le JSON est analysable, les titres Markdown sont présents |
| **Contenu interdit** | Pas de noms de marques halluciné, pas de mentions de concurrents |

### 3. LLM en tant que juge

Utilisez le **même modèle local** pour noter ses propres sorties (ou celles d’une variante différente d’invite). Le juge reçoit :

- La question originale
- La réponse de l’agent
- Les critères de notation

Et renvoie un score structuré. Cela reflète le modèle Éditeur de la Partie 7 mais appliqué systématiquement à une suite de tests.

### 4. Cycle d’itération guidé par l’évaluation

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Prérequis

| Exigence | Détails |
|----------|---------|
| **CLI Foundry Local** | Installée avec un modèle téléchargé |
| **Runtime Langage** | **Python 3.9+** et/ou **Node.js 18+** et/ou **SDK .NET 9+** |
| **Complété** | [Partie 5 : Agents uniques](part5-single-agents.md) et [Partie 6 : Workflows multi-agents](part6-multi-agent-workflows.md) |

---

## Exercices du laboratoire

### Exercice 1 - Exécuter le cadre d’évaluation

L’atelier inclut un exemple complet d’évaluation testant un agent Foundry Local contre un dataset golden de questions liées au bricolage Zava.

<details>
<summary><strong>🐍 Python</strong></summary>

**Configuration :**  
```bash
cd python
python -m venv venv

# Windows (PowerShell) :
venv\Scripts\Activate.ps1
# macOS :
source venv/bin/activate

pip install -r requirements.txt
```
  
**Exécution :**  
```bash
python foundry-local-eval.py
```
  
**Ce qui se passe :**  
1. Connexion à Foundry Local et chargement du modèle  
2. Définition d’un dataset golden de 5 cas de test (questions produits Zava)  
3. Exécution de deux variantes d’invite pour chaque cas de test  
4. Notation de chaque réponse avec **vérifications basées sur règles** (longueur, mots clés, format)  
5. Notation de chaque réponse avec **LLM-en-juge** (le même modèle note la qualité 1-5)  
6. Affichage d’une fiche de scores comparant les deux variantes d’invite

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Configuration :**  
```bash
cd javascript
npm install
```
  
**Exécution :**  
```bash
node foundry-local-eval.mjs
```
  
**Même pipeline d’évaluation :** dataset golden, double exécution d’invite, notation règles + LLM, fiche de scores.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Configuration :**  
```bash
cd csharp
dotnet restore
```
  
**Exécution :**  
```bash
dotnet run eval
```
  
**Même pipeline d’évaluation :** dataset golden, double exécution d’invite, notation règles + LLM, fiche de scores.

</details>

---

### Exercice 2 - Comprendre le dataset golden

Examinez les cas de test définis dans l’exemple d’évaluation. Chaque cas de test contient :

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```
  
**Questions à considérer :**  
1. Pourquoi les valeurs attendues sont-elles des **mots clés** plutôt que des phrases complètes ?  
2. Combien de cas de test faut-il pour une évaluation fiable ?  
3. Quelles catégories ajouteriez-vous pour votre propre application ?

---

### Exercice 3 - Comprendre les notations basées sur règles vs LLM-en-juge

Le cadre d’évaluation utilise **deux couches de notation** :

#### Vérifications basées sur règles (rapides, déterministes)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```
  
#### LLM-en-juge (nuancées, qualitatives)

Le même modèle local agit comme juge avec une grille structurée :

```
Rate this response on a scale of 1-5:
- 1: Completely wrong or irrelevant
- 2: Partially correct but missing key information
- 3: Adequate but could be improved
- 4: Good response with minor issues
- 5: Excellent, comprehensive, well-structured

Score: 4
Reasoning: The response correctly identifies all necessary tools
and provides practical advice, but could include safety equipment.
```
  
**Questions à considérer :**  
1. Quand feriez-vous davantage confiance aux vérifications basées sur règles qu’au LLM-en-juge ?  
2. Un modèle peut-il juger de manière fiable sa propre sortie ? Quelles sont les limites ?  
3. Comment cela se compare-t-il au modèle d’agent Éditeur de la Partie 7 ?

---

### Exercice 4 - Comparer les variantes d’invite

L’exemple exécute **deux variantes d’invite** sur les mêmes cas de test :

| Variante | Style d’invite système |
|-----------|-----------------------|
| **De base** | Générique : « Vous êtes un assistant utile » |
| **Spécialisée** | Détail : « Vous êtes un expert bricolage Zava qui recommande des produits spécifiques et fournit des guides étape par étape » |

Après l’exécution, vous verrez une fiche de scores comme :

```
╔══════════════════════════════════════════════════════════════╗
║                    EVALUATION SCORECARD                      ║
╠══════════════════════════════════════════════════════════════╣
║ Prompt Variant    │ Rule Score │ LLM Score │ Combined       ║
╠═══════════════════╪════════════╪═══════════╪════════════════╣
║ Baseline          │ 0.62       │ 3.2 / 5   │ 0.62           ║
║ Specialised       │ 0.81       │ 4.1 / 5   │ 0.81           ║
╚══════════════════════════════════════════════════════════════╝
```
  
**Exercices :**  
1. Lancez l’évaluation et notez les scores pour chaque variante  
2. Modifiez l’invite spécialisée pour qu’elle soit encore plus précise. Le score s’améliore-t-il ?  
3. Ajoutez une troisième variante d’invite et comparez les trois.  
4. Essayez de changer l’alias du modèle (ex. `phi-4-mini` vs `phi-3.5-mini`) et comparez les résultats.

---

### Exercice 5 - Appliquer l’évaluation à votre propre agent

Utilisez le cadre d’évaluation comme modèle pour vos propres agents :

1. **Définissez votre dataset golden** : écrivez 5 à 10 cas de test avec des mots clés attendus.  
2. **Rédigez votre invite système** : les instructions pour l’agent que vous voulez tester.  
3. **Exécutez l’évaluation** : obtenez les scores de référence.  
4. **Itérez** : ajustez l’invite, relancez, et comparez.  
5. **Fixez un seuil** : ex. « ne pas déployer en dessous d’un score combiné de 0,75 ».

---

### Exercice 6 - Connexion au modèle Éditeur de Zava

Revenez à l’agent Éditeur du Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`) :

```python
# L'éditeur est un LLM en tant que juge en production :
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```
  
C’est le **même concept** que le LLM-en-juge de la Partie 8, mais intégré dans le pipeline de production plutôt que dans une suite de tests hors ligne. Les deux modèles :

- Utilisent une sortie JSON structurée du modèle.  
- Appliquent des critères de qualité définis dans l’invite système.  
- Prennent une décision valide/échec.

**La différence :** L’éditeur fonctionne en production (sur chaque requête). Le cadre d’évaluation fonctionne en développement (avant déploiement).

---

## Points clés à retenir

| Concept | Retenue |
|---------|----------|
| **Datasets golden** | Sélectionner les cas de test tôt ; ce sont vos tests de régression pour l’IA |
| **Vérifications basées sur règles** | Rapides, déterministes, détectent les échecs évidents (longueur, mots clés, format) |
| **LLM-en-juge** | Notation qualitative nuancée utilisant le même modèle local |
| **Comparaison d’invites** | Exécuter plusieurs variantes sur la même suite pour choisir la meilleure |
| **Avantage local** | Toute l’évaluation s’exécute localement : pas de frais API, pas de limites de taux, pas de sortie de données |
| **Eval-avant-déploiement** | Fixer des seuils de qualité et conditionner les releases aux scores d’évaluation |

---

## Prochaines étapes

- **Élargir** : Ajoutez plus de cas de test et catégories à votre dataset golden.  
- **Automatiser** : Intégrez l’évaluation dans votre pipeline CI/CD.  
- **Juges avancés** : Utilisez un modèle plus grand comme juge tout en testant la sortie d’un modèle plus petit.  
- **Suivre dans le temps** : Sauvegardez les résultats d’évaluation pour comparer les versions d’invites et de modèles.

---

## Prochain laboratoire

Continuez vers [Partie 9 : Transcription vocale avec Whisper](part9-whisper-voice-transcription.md) pour explorer la reconnaissance vocale hors ligne avec Foundry Local SDK.