# Problèmes connus — Foundry Local Workshop

Problèmes rencontrés lors de la construction et des tests de cet atelier sur un appareil **Snapdragon X Elite (ARM64)** sous Windows, avec Foundry Local SDK v0.9.0, CLI v0.8.117 et .NET SDK 10.0.

> **Dernière validation :** 2026-03-11

---

## 1. CPU Snapdragon X Elite non reconnu par ONNX Runtime

**Statut :** Ouvert  
**Gravité :** Avertissement (non bloquant)  
**Composant :** ONNX Runtime / cpuinfo  
**Reproduction :** Chaque démarrage du service Foundry Local sur matériel Snapdragon X Elite

À chaque démarrage du service Foundry Local, deux avertissements sont émis :

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Impact :** Les avertissements sont cosmétiques — l’inférence fonctionne correctement. Cependant, ils apparaissent à chaque exécution et peuvent désorienter les participants à l’atelier. La bibliothèque cpuinfo d’ONNX Runtime doit être mise à jour pour reconnaître les cœurs CPU Qualcomm Oryon.

**Attendu :** Le Snapdragon X Elite devrait être reconnu comme un CPU ARM64 supporté sans émettre de messages d’erreur.

---

## 2. NullReferenceException SingleAgent au premier lancement

**Statut :** Ouvert (intermittent)  
**Gravité :** Critique (plantage)  
**Composant :** Foundry Local C# SDK + Microsoft Agent Framework  
**Reproduction :** Exécuter `dotnet run agent` — plantage immédiatement après le chargement du modèle

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Contexte :** La ligne 37 appelle `model.IsCachedAsync(default)`. Le plantage s’est produit lors de la première exécution de l’agent après un `foundry service stop` neuf. Les exécutions suivantes avec le même code ont réussi.

**Impact :** Intermittent — suggère une condition de course lors de l’initialisation du service ou de la requête du catalogue dans le SDK. L’appel `GetModelAsync()` peut retourner avant que le service ne soit entièrement prêt.

**Attendu :** `GetModelAsync()` doit soit bloquer jusqu’à ce que le service soit prêt, soit retourner un message d’erreur clair si le service n’a pas fini de s’initialiser.

---

## 3. C# SDK nécessite un RuntimeIdentifier explicite

**Statut :** Ouvert — suivi dans [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Gravité :** Lacune documentaire  
**Composant :** Paquet NuGet `Microsoft.AI.Foundry.Local`  
**Reproduction :** Créer un projet .NET 8+ sans `<RuntimeIdentifier>` dans le `.csproj`

La compilation échoue avec :

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Cause principale :** L’exigence du RID est attendue — le SDK embarque des binaires natifs (P/Invoke dans `Microsoft.AI.Foundry.Local.Core` et ONNX Runtime), donc .NET doit savoir quelle bibliothèque spécifique à la plateforme utiliser.

Ceci est documenté sur MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), où les instructions d’exécution montrent :

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Cependant, les utilisateurs doivent se souvenir de l’option `-r` à chaque fois, ce qui est facile à oublier.

**Solution de contournement :** Ajouter une détection automatique dans votre `.csproj` pour que `dotnet run` fonctionne sans options :

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` est une propriété MSBuild intégrée qui résout automatiquement le RID de la machine hôte. Les projets de test du SDK utilisent déjà ce modèle. Les options `-r` explicites sont toujours prises en compte si fournies.

> **Note :** Le `.csproj` de l’atelier inclut ce fallback pour que `dotnet run` fonctionne immédiatement sur toute plateforme.

**Attendu :** Le modèle `.csproj` dans la documentation MS Learn devrait inclure ce pattern de détection automatique, pour éviter aux utilisateurs d’oublier l’option `-r`.

---

## 4. Whisper JavaScript — transcription audio retourne une sortie vide/binaire

**Statut :** Ouvert (régression — aggravée depuis le rapport initial)  
**Gravité :** Majeure  
**Composant :** Implémentation JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reproduction :** Exécuter `node foundry-local-whisper.mjs` — tous les fichiers audio retournent une sortie vide ou binaire au lieu de la transcription texte

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Initialement, seul le 5ᵉ fichier audio retournait vide ; depuis la v0.9.x, les 5 fichiers retournent un octet unique (`\ufffd`) au lieu du texte transcrit. L’implémentation Python Whisper utilisant l’OpenAI SDK transcrit correctement les mêmes fichiers.

**Attendu :** `createAudioClient()` doit retourner une transcription texte conforme aux implémentations Python/C#.

---

## 5. Le SDK C# ne cible que net8.0 — pas de cible officielle .NET 9 ou .NET 10

**Statut :** Ouvert  
**Gravité :** Lacune documentaire  
**Composant :** Paquet NuGet `Microsoft.AI.Foundry.Local` v0.9.0  
**Commande d’installation :** `dotnet add package Microsoft.AI.Foundry.Local`

Le paquet NuGet ne fournit qu’un seul framework cible :

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Aucun TFM `net9.0` ou `net10.0`. En revanche, le paquet compagnon `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) fournit `net8.0`, `net9.0`, `net10.0`, `net472` et `netstandard2.0`.

### Tests de compatibilité

| Framework cible | Build | Exécution | Notes |
|-----------------|-------|-----------|-------|
| net8.0 | ✅ | ✅ | Support officiel |
| net9.0 | ✅ | ✅ | Compilation via compatibilité ascendante — utilisé dans les exemples d’atelier |
| net10.0 | ✅ | ✅ | Compilation et exécution via compatibilité ascendante avec le runtime .NET 10.0.3 |

L’assembly net8.0 s’exécute sur des runtimes plus récents via le mécanisme de compatibilité ascendante de .NET, donc la compilation réussit. Cependant, cela n’est ni documenté ni testé par l’équipe SDK.

### Pourquoi les exemples ciblent net9.0

1. **.NET 9 est la dernière version stable** — la plupart des participants l’ont installée  
2. **La compatibilité ascendante fonctionne** — l’assembly net8.0 du paquet NuGet tourne sans problème sur le runtime .NET 9  
3. **.NET 10 (preview/RC)** est trop récent pour cibler un atelier destiné à tous

**Attendu :** Les futures versions du SDK devraient envisager d’ajouter les TFMs `net9.0` et `net10.0` en plus de `net8.0` pour suivre le modèle de `Microsoft.Agents.AI.OpenAI` et fournir un support validé pour les runtimes plus récents.

---

## 6. ChatClient JavaScript Streaming utilise des callbacks, pas d’itérateurs async

**Statut :** Ouvert  
**Gravité :** Lacune documentaire  
**Composant :** JavaScript `foundry-local-sdk` v0.9.x — `ChatClient.completeStreamingChat()`

Le `ChatClient` retourné par `model.createChatClient()` fournit une méthode `completeStreamingChat()`, mais elle utilise un **pattern de callback** au lieu de retourner un itérable async :

```javascript
// ❌ Cela NE fonctionne PAS — génère l'erreur "stream n'est pas itérable de façon asynchrone"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Modèle correct — passez un callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Impact :** Les développeurs habitués au pattern d’itération async du SDK OpenAI (`for await`) seront confrontés à des erreurs déroutantes. Le callback doit être une fonction valide sinon le SDK lève "Callback must be a valid function."

**Attendu :** Documenter le pattern callback dans la référence SDK. Ou bien supporter l’itération async pour plus de cohérence avec le SDK OpenAI.

---

## Détails de l’environnement

| Composant | Version |
|-----------|---------|
| OS | Windows 11 ARM64 |
| Matériel | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| SDK OpenAI C# | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |