# Známe problémy — Foundry Local Workshop

Problémy zaznamenané počas zostavovania a testovania tohto workshopu na zariadení **Snapdragon X Elite (ARM64)** s Windows, Foundry Local SDK v0.9.0, CLI v0.8.117 a .NET SDK 10.0.

> **Naposledy overené:** 2026-03-11

---

## 1. CPU Snapdragon X Elite Nie je Rozpoznaný ONNX Runtime

**Stav:** Otvorené  
**Závažnosť:** Upozornenie (nezabraňuje behu)  
**Komponent:** ONNX Runtime / cpuinfo  
**Reprodukcia:** Každé spustenie Foundry Local služby na hardvéri Snapdragon X Elite

Pri každom spustení Foundry Local služby sa zobrazia dve upozornenia:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Dopad:** Upozornenia sú len kosmetické — inferencia funguje správne. Avšak zobrazujú sa pri každom spustení a môžu miasť účastníkov workshopu. Knižnica ONNX Runtime cpuinfo musí byť aktualizovaná, aby rozpoznávala jadrá Qualcomm Oryon CPU.

**Očakávané:** Snapdragon X Elite by mal byť rozpoznaný ako podporovaný ARM64 CPU bez zobrazovania chybových hlásení.

---

## 2. SingleAgent NullReferenceException pri Prvom Spustení

**Stav:** Otvorené (príležitostné)  
**Závažnosť:** Kritické (pád)  
**Komponent:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reprodukcia:** Spustiť `dotnet run agent` — pád ihneď po načítaní modelu

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Kontext:** Na riadku 37 sa volá `model.IsCachedAsync(default)`. Pád nastal pri prvom spustení agenta po čerstvom `foundry service stop`. Následné spustenia s rovnakým kódom už prebehli úspešne.

**Dopad:** Príležitostný — naznačuje závodnú podmienku v inicializácii služby SDK alebo dotaze do katalógu. Volanie `GetModelAsync()` môže vrátiť výsledok, ešte kým služba nie je plne pripravená.

**Očakávané:** `GetModelAsync()` by malo buď blokovať do pripravenosti služby alebo vrátiť jasnú chybovú správu, ak služba ešte nie je inicializovaná.

---

## 3. C# SDK Vyžaduje Explicitný RuntimeIdentifier

**Stav:** Otvorené — sledované v [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Závažnosť:** Nedostatok v dokumentácii  
**Komponent:** NuGet balík `Microsoft.AI.Foundry.Local`  
**Reprodukcia:** Vytvoriť .NET 8+ projekt bez `<RuntimeIdentifier>` v `.csproj`

Zostavenie zlyhá s:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Príčina:** Požiadavka na RID je očakávaná — SDK obsahuje nativne binárky (P/Invoke do `Microsoft.AI.Foundry.Local.Core` a ONNX Runtime), preto .NET potrebuje poznať platformovo špecifickú knižnicu na vyriešenie.

Je to zdokumentované na MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), kde inštrukcie na spustenie ukazujú:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Používatelia si však musia pamätať parameter `-r` zakaždým, čo sa ľahko zabudne.

**Obídenie:** Pridať do `.csproj` fallback s automatickým detekovaním, aby `dotnet run` fungoval bez parametrov:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` je vstavaná vlastnosť MSBuild, ktorá automaticky vyrieši RID hostiteľského stroja. Testovacie projekty SDK už tento vzor používajú. Explicitné parametre `-r` sú stále akceptované, ak sú poskytnuté.

> **Poznámka:** Workshopové `.csproj` už obsahuje tento fallback, takže `dotnet run` funguje hneď na každej platforme.

**Očakávané:** Šablóna `.csproj` v dokumentácii MS Learn by mala obsahovať tento automatický vzor, aby užívatelia nemuseli pamätať parameter `-r`.

---

## 4. JavaScript Whisper — Audio Prepis Vracia Prázdny alebo Binárny Výstup

**Stav:** Otvorené (regresia — zhoršené od prvotného hlásenia)  
**Závažnosť:** Vážne  
**Komponent:** Implementácia JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reprodukcia:** Spustiť `node foundry-local-whisper.mjs` — všetky audio súbory vracajú prázdny alebo binárny výstup namiesto textového prepisu

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Pôvodne len 5. audio súbor vracal prázdny výstup; od verzie v0.9.x všetkých 5 súborov vracia jediný bajt (`\ufffd`) namiesto prepísaného textu. Pythonská implementácia Whisper používaná cez OpenAI SDK tie isté súbory prepisuje správne.

**Očakávané:** `createAudioClient()` by mal vracať textový prepis zhodný s implementáciami v Pythone a C#.

---

## 5. C# SDK Dodáva iba net8.0 — Žiadny Oficiálny Cieľ .NET 9 alebo .NET 10

**Stav:** Otvorené  
**Závažnosť:** Nedostatok v dokumentácii  
**Komponent:** NuGet balík `Microsoft.AI.Foundry.Local` v0.9.0  
**Inštalačný príkaz:** `dotnet add package Microsoft.AI.Foundry.Local`

Balík NuGet dodáva iba jeden cieľový framework:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Nie je obsiahnutý `net9.0` ani `net10.0`. Naopak, balík partner `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) dodáva `net8.0`, `net9.0`, `net10.0`, `net472` a `netstandard2.0`.

### Testovanie kompatibility

| Cieľový Framework | Zostavenie | Spustenie | Poznámky |
|-------------------|------------|-----------|----------|
| net8.0 | ✅ | ✅ | Oficiálne podporovaný |
| net9.0 | ✅ | ✅ | Zostavený cez forward-compat — používa sa vo workshopových príkladoch |
| net10.0 | ✅ | ✅ | Zostavený a beží pomocou forward-compat s runtime .NET 10.0.3 |

Zostava net8.0 sa načíta na novších runtime pomocou mechanizmu forward-compat .NET, takže zostavenie prebehne úspešne. Toto však nie je zdokumentované ani testované tímom SDK.

### Prečo príklady cieľia net9.0

1. **.NET 9 je najnovšie stabilné vydanie** — väčšina účastníkov workshopu ho bude mať nainštalovaný  
2. **Forward kompatibilita funguje** — net8.0 zostava v NuGet balíku beží na runtime .NET 9 bez problémov  
3. **.NET 10 (preview/RC)** je príliš nové na cielenie v workshope, ktorý má fungovať pre každého

**Očakávané:** Budúce verzie SDK by mali zvážiť pridanie TFMs `net9.0` a `net10.0` vedľa `net8.0`, aby zodpovedali vzoru používanému `Microsoft.Agents.AI.OpenAI` a poskytli overenú podporu pre novšie runtime.

---

## 6. JavaScript ChatClient Streaming Používa Callbacky, Nie Async Iterator

**Stav:** Otvorené  
**Závažnosť:** Nedostatok v dokumentácii  
**Komponent:** JavaScript `foundry-local-sdk` v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` vrátený metódou `model.createChatClient()` poskytuje metódu `completeStreamingChat()`, ale používa **callback vzor**, namiesto vrátenia async iterátora:

```javascript
// ❌ Toto nefunguje — vyhadzuje "stream nie je asynchrónne iterovateľný"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Správny vzor — odovzdať spätné volanie
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Dopad:** Vývojári oboznámení so vzorom async iterácie v OpenAI SDK (`for await`) narazia na mätúce chyby. Callback musí byť platná funkcia, inak SDK vyhodí "Callback must be a valid function."

**Očakávané:** Zdokumentovať callback vzor v SDK referencii. Alternatívne podporiť async iterátor pre konzistentnosť s OpenAI SDK.

---

## Detaily Prostredia

| Komponent | Verzia |
|-----------|--------|
| OS | Windows 11 ARM64 |
| Hardvér | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |