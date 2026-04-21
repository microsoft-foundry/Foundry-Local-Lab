# Tuntud probleemid — Foundry kohalik töögrupp

Probleemid, mis tekkisid selle töötoa ehitamisel ja testimisel **Snapdragon X Elite (ARM64)** seadmel, kus töötab Windows, Foundry Local SDK v0.9.0, CLI v0.8.117 ja .NET SDK 10.0.

> **Viimati kontrollitud:** 2026-03-11

---

## 1. Snapdragon X Elite protsessorit ONNX Runtime ei tunnusta

**Staatus:** Avatud  
**Tõsidus:** Hoiatusteade (mitte-blokeeriv)  
**Komponent:** ONNX Runtime / cpuinfo  
**Taastamine:** Iga Foundry Local teenuse käivitamine Snapdragon X Elite riistvaral

Iga kord, kui Foundry Local teenus käivitatakse, kuvatakse kaks hoiatust:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Mõju:** Hoiatused on kosmeetilised — järeldamine töötab korrektselt. Kuid need ilmuvad igal käivitamisel ja võivad segadust tekitada töötoa osalejates. ONNX Runtime cpuinfo raamatukogu vajab värskendust, et tunnustada Qualcomm Oryon CPU tuumasid.

**Oodatud:** Snapdragon X Elite tuleks tunnustada toetatud ARM64 protsessorina ilma veatasemeliste sõnumiteta.

---

## 2. SingleAgent NullReferenceException esimesel käivitamisel

**Staatus:** Avatud (vahelduv)  
**Tõsidus:** Kriitiline (krahh)  
**Komponent:** Foundry Local C# SDK + Microsoft Agent Framework  
**Taastamine:** Käivita `dotnet run agent` — krahh otse pärast mudeli laadimist

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Kontekst:** Rida 37 kutsub `model.IsCachedAsync(default)`. Krahh toimus agendi esimesel käivitamisel pärast värsket `foundry service stop`. Järgmised samade koodidega jooksud õnnestusid.

**Mõju:** Vahelduv — viitab "race condition" olukorrale SDK teenuse initsialiseerimisel või kataloogi päringul. `GetModelAsync()` kutsumine võib tagastada enne, kui teenus on täielikult valmis.

**Oodatud:** `GetModelAsync()` peab kas blokeerima, kuni teenus on valmis, või tagastama selge veateate, kui teenus pole veel initsialiseeritud.

---

## 3. C# SDK vajab selgesõnalist RuntimeIdentifierit

**Staatus:** Avatud — jälgitakse aadressil [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Tõsidus:** Dokumenteerimise puudujääk  
**Komponent:** `Microsoft.AI.Foundry.Local` NuGet pakett  
**Taastamine:** Loo .NET 8+ projekt ilma `<RuntimeIdentifier>` määramata `.csproj` failis

Kompileerimine ebaõnnestub järgmise veaga:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Põhjus:** Runtime Identifieri nõue on ootuspärane — SDK tarnib natiivseid binaare (P/Invoke `Microsoft.AI.Foundry.Local.Core` ja ONNX Runtime'i), seega peab .NET teadma, millist platvormispetsiifilist raamatukogu lahendada.

See on dokumenteeritud MS Learningus ([Kuidas kasutada natiivseid jutukomplekte](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), kus käsklused näitavad:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Kuid kasutajad peavad igal korral meeles pidama `-r` lippu, mis võib kergesti ununeda.

**Lahendus:** Lisa `.csproj`-i automaatse tuvastuse varuplaan, nii et `dotnet run` töötab ilma täiendavate lippudeta:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` on MSBuild sisseehitatud atribuut, mis tuvastab automaatselt hostmasina RIDi. SDK enda testprojektid kasutavad juba seda mustrit. Selgesõnalised `-r` lipud jäävad endiselt kehtima, kui neid antakse.

> **Märkus:** Töötoa `.csproj` sisaldab seda varuplaani, nii et `dotnet run` töötab igal platvormil kohe.

**Oodatud:** MS Learn'i dokumentatsiooni `.csproj` mall peaks sisaldama seda automaatse tuvastuse mustrit, nii et kasutajad ei pea `-r` lippu meeles pidama.

---

## 4. JavaScript Whisper — heli transkriptsioon tagastab tühja või binaarse väljundi

**Staatus:** Avatud (regressioon — on halvenenud alates algsest raportist)  
**Tõsidus:** Suur  
**Komponent:** JavaScript Whisper rakendus (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Taastamine:** Käivita `node foundry-local-whisper.mjs` — kõik helifailid tagastavad tühja või binaarse väljundi teksti asemel

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Alguses tagastas ainult 5. helifail tühja; alates v0.9.x-st tagastavad kõik 5 faili ühe baidi (`\ufffd`), mitte transkribeeritud teksti. Python Whisper rakendus OpenAI SDK-ga transkribeerib samad failid korrektselt.

**Oodatud:** `createAudioClient()` peaks tagastama teksti transkriptsiooni, mis vastab Python/C# rakendustele.

---

## 5. C# SDK tarnib ainult net8.0 — ametlikku .NET 9 või .NET 10 sihtmärki pole

**Staatus:** Avatud  
**Tõsidus:** Dokumenteerimise puudujääk  
**Komponent:** `Microsoft.AI.Foundry.Local` NuGet pakk v0.9.0  
**Paigalduskäsk:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet pakett tarnib ainult ühe sihtraamistiku:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
`net9.0` või `net10.0` sihtraamistikke ei ole kaasatud. Võrdluseks kaaslaseks on `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3), mis tarnib `net8.0`, `net9.0`, `net10.0`, `net472` ja `netstandard2.0`.

### Ühilduvuse testimine

| Sihtraamistik | Koosta | Käivita | Märkused |
|---------------|--------|---------|----------|
| net8.0 | ✅ | ✅ | Ametlikult toetatud |
| net9.0 | ✅ | ✅ | Koostatakse edasiühilduvusega — kasutatud töötoa näidetes |
| net10.0 | ✅ | ✅ | Koostatakse ja käivitub edasiühilduvusega .NET 10.0.3 runtime'iga |

net8.0 kooslus laetakse uuematel runtime'idel läbi .NET edasiühilduvuse mehhanismi, nii et ehitus õnnestub. Kuid see pole SDK meeskonna poolt dokumenteeritud ega testitud.

### Miks näited sihivad net9.0

1. **.NET 9 on uusim stabiilne versioon** — enamikel töötoa osalejatel on see paigaldatud  
2. **Edasiühilduvus töötab** — NuGet paketi net8.0 kooslus töötab .NET 9 runtime'is ilma probleemideta  
3. **.NET 10 (eelvaade/RC)** on liiga uus, et sihtida töötoas, mis peaks töötama kõigil

**Oodatud:** Tulevikes peaks SDK versioonid kaaluma `net9.0` ja `net10.0` TFM lisamist koos `net8.0`-ga, sarnaselt `Microsoft.Agents.AI.OpenAI` mustrile, et pakkuda uuemate runtime'ide toetust, mille on testinud SDK meeskond.

---

## 6. JavaScript ChatClient striimimine kasutab tagasikutsumisi (callback), mitte asünkroonseid iteraatoreid

**Staatus:** Avatud  
**Tõsidus:** Dokumenteerimise puudujääk  
**Komponent:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient`, mille tagastab `model.createChatClient()`, pakub `completeStreamingChat()` meetodit, kuid see kasutab **tagasikutsumise mustrit** asemel, et tagastada asünkroonset itereeritavat objekti:

```javascript
// ❌ See EI tööta — viskab "stream ei ole asünkroonne iteratiivne"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Õige muster — anna edasi tagasikutsumine
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Mõju:** Arendajad, kes on harjunud OpenAI SDK asünkroonse iteratsiooniga (`for await`), saavad segaseid veateateid. Tagasikutsumine peab olema kehtiv funktsioon, vastasel juhul viskab SDK veateate "Callback must be a valid function."

**Oodatud:** Kirjeldada SDK viites tagasikutsumise mustrit. Või toetada asünkroonse iteraatori mustrit, et tagada kooskõla OpenAI SDK-ga.

---

## Keskkonna üksikasjad

| Komponent | Versioon |
|-----------|----------|
| OS | Windows 11 ARM64 |
| Riistvara | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |