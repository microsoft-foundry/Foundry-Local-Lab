# Probleme cunoscute — Ateliere locale Foundry

Probleme întâlnite în timpul construirii și testării acestui atelier pe un dispozitiv **Snapdragon X Elite (ARM64)** care rulează Windows, cu Foundry Local SDK v0.9.0, CLI v0.8.117 și .NET SDK 10.0.

> **Ultima validare:** 2026-03-11

---

## 1. CPU Snapdragon X Elite nerecunoscut de ONNX Runtime

**Stare:** Deschis
**Gravitate:** Avertisment (neblocant)
**Componentă:** ONNX Runtime / cpuinfo
**Reproducere:** De fiecare dată la pornirea serviciului Foundry Local pe hardware Snapdragon X Elite

De fiecare dată când serviciul Foundry Local pornește, sunt emise două avertismente:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**Impact:** Avertismentele sunt cosmetice — inferența funcționează corect. Totuși, apar la fiecare rulare și pot crea confuzie pentru participanții la atelier. Biblioteca cpuinfo a ONNX Runtime trebuie actualizată pentru a recunoaște nucleele CPU Qualcomm Oryon.

**Așteptat:** Snapdragon X Elite ar trebui recunoscut ca un CPU ARM64 suportat fără a emite mesaje de eroare de nivel critic.

---

## 2. NullReferenceException în SingleAgent la prima rulare

**Stare:** Deschis (intermitent)
**Gravitate:** Critic (prăbușire)
**Componentă:** Foundry Local C# SDK + Microsoft Agent Framework
**Reproducere:** Rulați `dotnet run agent` — se prăbușește imediat după încărcarea modelului

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**Context:** Linia 37 apelează `model.IsCachedAsync(default)`. Prăbușirea s-a produs la prima rulare a agentului după un `foundry service stop` proaspăt. Rulările ulterioare cu același cod au avut succes.

**Impact:** Intermitent — sugerează o condiție de cursă în inițializarea serviciului SDK sau în interogarea catalogului. Apelul `GetModelAsync()` poate reveni înainte ca serviciul să fie complet pregătit.

**Așteptat:** `GetModelAsync()` ar trebui fie să blocheze până când serviciul este gata, fie să returneze un mesaj clar de eroare dacă serviciul nu a terminat inițializarea.

---

## 3. C# SDK necesită RuntimeIdentifier explicit

**Stare:** Deschis — monitorizat în [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)
**Gravitate:** Lacună în documentație
**Componentă:** pachet NuGet `Microsoft.AI.Foundry.Local`
**Reproducere:** Creați un proiect .NET 8+ fără `<RuntimeIdentifier>` în `.csproj`

Compilarea eșuează cu:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**Cauză:** Cerința RID este așteptată — SDK-ul livrează binare native (P/Invoke în `Microsoft.AI.Foundry.Local.Core` și ONNX Runtime), deci .NET trebuie să știe ce bibliotecă specifică platformei să rezolve.

Acest lucru este documentat pe MS Learn ([Cum se folosesc completările native pentru chat](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), unde instrucțiunile de rulare arată:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

Totuși, utilizatorii trebuie să-și amintească să folosească flag-ul `-r` de fiecare dată, ceea ce poate fi ușor de uitat.

**Soluție temporară:** Adăugați un fallback de auto-detectare în `.csproj` pentru ca `dotnet run` să funcționeze fără niciun flag:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` este o proprietate MSBuild încorporată care rezolvă automat RID-ul mașinii gazdă. Propriile proiecte de test ale SDK-ului folosesc deja acest model. Flag-urile explicite `-r` sunt însă onorate când sunt specificate.

> **Notă:** `.csproj`-ul atelierului include acest fallback pentru ca `dotnet run` să funcționeze imediat pe orice platformă.

**Așteptat:** Șablonul `.csproj` din documentația MS Learn ar trebui să includă acest model de auto-detectare pentru ca utilizatorii să nu fie nevoiți să-și amintească de flag-ul `-r`.

---

## 4. JavaScript Whisper — transcriere audio returnează ieșire vidă/sau binară

**Stare:** Deschis (regresie — s-a înrăutățit de la raportarea inițială)
**Gravitate:** Major
**Componentă:** Implementare JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**Reproducere:** Rulați `node foundry-local-whisper.mjs` — toate fișierele audio returnează ieșire vidă sau binară în loc de transcriere text

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

Inițial, doar al 5-lea fișier audio returna ieșire vidă; din versiunea v0.9.x, toate cele 5 fișiere returnează un singur byte (`\ufffd`) în loc de text transcris. Implementarea Python Whisper folosind SDK-ul OpenAI transcrie corect aceleași fișiere.

**Așteptat:** `createAudioClient()` ar trebui să returneze transcriere textuală similară cu implementările în Python/C#.

---

## 5. C# SDK livrează doar net8.0 — fără ținte oficiale .NET 9 sau .NET 10

**Stare:** Deschis
**Gravitate:** Lacună în documentație
**Componentă:** pachet NuGet `Microsoft.AI.Foundry.Local` v0.9.0
**Comandă instalare:** `dotnet add package Microsoft.AI.Foundry.Local`

Pachetul NuGet livrează doar un singur framework țintă:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

Nu este inclus un TFM `net9.0` sau `net10.0`. Spre deosebire, pachetul companion `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) livrează `net8.0`, `net9.0`, `net10.0`, `net472` și `netstandard2.0`.

### Testare compatibilitate

| Framework țintă | Compilare | Rulare | Note |
|-----------------|-----------|--------|------|
| net8.0 | ✅ | ✅ | Oficial suportat |
| net9.0 | ✅ | ✅ | Se compilează prin compatibilitate înainte — folosit în exemplele din atelier |
| net10.0 | ✅ | ✅ | Se compilează și rulează prin compatibilitate înainte cu runtime .NET 10.0.3 |

Asamblarea net8.0 se încarcă pe runtime-uri mai noi prin mecanismul de forward-compat al .NET, deci compilarea reușește. Totuși, acest lucru este nedocumentat și nevalidat de echipa SDK.

### De ce folosesc exemplele net9.0

1. **.NET 9 este ultima versiune stabilă** — majoritatea participanților la atelier vor avea instalată această versiune
2. **Compatibilitatea înainte funcționează** — asamblarea net8.0 din pachetul NuGet rulează pe runtime-ul .NET 9 fără probleme
3. **.NET 10 (previzualizare/RC)** este prea nouă pentru a fi țintită într-un atelier care trebuie să funcționeze pentru oricine

**Așteptat:** Versiunile viitoare ale SDK-ului ar trebui să adauge TFMs `net9.0` și `net10.0` alături de `net8.0`, pentru a urma modelul folosit de `Microsoft.Agents.AI.OpenAI` și pentru a oferi suport validat pentru runtime-uri mai noi.

---

## 6. Streaming-ul ChatClient din JavaScript folosește callback-uri, nu iteratoare asincrone

**Stare:** Deschis
**Gravitate:** Lacună în documentație
**Componentă:** JavaScript `foundry-local-sdk` v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` returnat de `model.createChatClient()` oferă o metodă `completeStreamingChat()`, dar folosește un **model callback** în loc să returneze un iterable asincron:

```javascript
// ❌ Acest lucru NU funcționează — aruncă "stream nu este iterabil asincron"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Model corect — transmiteți un callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**Impact:** Dezvoltatorii familiarizați cu modelul de iterare asincronă al SDK-ului OpenAI (`for await`) vor întâmpina erori confuze. Callback-ul trebuie să fie o funcție validă, altfel SDK aruncă "Callback must be a valid function."

**Așteptat:** Să se documenteze modelul callback în referința SDK. Alternativ, să se suporte modelul iterable asincron pentru consistență cu SDK-ul OpenAI.

---

## Detalii mediu

| Componentă | Versiune |
|------------|----------|
| SO | Windows 11 ARM64 |
| Hardware | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |