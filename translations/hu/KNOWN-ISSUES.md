# Ismert problémák — Foundry Local Workshop

A workshop összeállítása és tesztelése közben tapasztalt problémák egy **Snapdragon X Elite (ARM64)** eszközön, Windows rendszer alatt, Foundry Local SDK v0.9.0, CLI v0.8.117 és .NET SDK 10.0 verziókkal.

> **Utoljára ellenőrizve:** 2026-03-11

---

## 1. Snapdragon X Elite CPU nem ismeri fel az ONNX Runtime

**Állapot:** Nyitott  
**Súlyosság:** Figyelmeztetés (nem blokkoló)  
**Komponens:** ONNX Runtime / cpuinfo  
**Reprodukció:** Minden Foundry Local szolgáltatás indítás Snapdragon X Elite hardveren

Minden alkalommal, amikor a Foundry Local szolgáltatás elindul, két figyelmeztetést dob:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Hatás:** A figyelmeztetések esztétikai jellegűek — az inferencia helyesen működik. Azonban minden futásnál megjelennek, ami összezavarhatja a workshop résztvevőit. Az ONNX Runtime cpuinfo könyvtárát frissíteni kell, hogy felismerje a Qualcomm Oryon CPU magokat.

**Elvárt:** A Snapdragon X Elite-nek támogatott ARM64 CPU-ként kellene megjelennie anélkül, hogy hibaszintű üzeneteket adna ki.

---

## 2. SingleAgent NullReferenceException az első futtatáskor

**Állapot:** Nyitott (időszakos)  
**Súlyosság:** Kritikus (összeomlás)  
**Komponens:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reprodukció:** Futtasd: `dotnet run agent` — a modell betöltése után azonnal összeomlik

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Háttér:** A 37. sor hívja a `model.IsCachedAsync(default)` metódust. Az összeomlás az agent első futtatásakor történt friss `foundry service stop` után. A további futtatások ugyanazzal a kóddal már sikeresek voltak.

**Hatás:** Időszakos probléma — arra utal, hogy versenyhelyzet (race condition) van az SDK szolgáltatás inicializációjában vagy a katalógus lekérdezésében. A `GetModelAsync()` hívás visszatérhet, mielőtt a szolgáltatás teljesen készen állna.

**Elvárt:** A `GetModelAsync()` vagy blokkolja a hívást a szolgáltatás készenlétéig, vagy adjon világos hibajelzést, ha a szolgáltatás még nem fejezte be az inicializációt.

---

## 3. C# SDK explicit RuntimeIdentifier megadást igényel

**Állapot:** Nyitott — követve a [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) issue-ban  
**Súlyosság:** Dokumentációs hiányosság  
**Komponens:** `Microsoft.AI.Foundry.Local` NuGet csomag  
**Reprodukció:** Hozz létre egy .NET 8+ projektet `<RuntimeIdentifier>` nélkül a `.csproj` fájlban

A build hibaüzenetet ad:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Ok:** Az RID megadása elvárt — az SDK natív binárisokat tartalmaz (P/Invoke a `Microsoft.AI.Foundry.Local.Core`-ba és ONNX Runtime-ba), ezért a .NET-nek tudnia kell, melyik platform-specifikus könyvtárat oldja fel.

Ezt dokumentálja a MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), ahol a futtatási utasítások így néznek ki:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Azonban a felhasználóknak minden futtatásnál emlékezniük kell a `-r` kapcsolóra, amit könnyű elfelejteni.

**Megoldás:** Adj a `.csproj`-hoz egy automatikus felismerésre épülő visszaesési megoldást, hogy a `dotnet run` flag nélkül is működjön:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
A `$(NETCoreSdkRuntimeIdentifier)` egy beépített MSBuild tulajdonság, amely automatikusan a hosztgép RID-jét oldja fel. Az SDK saját teszt projektjei is ezt a mintát használják. Az explicit `-r` flag-eket továbbra is elfogadja a rendszer.

> **Megjegyzés:** A workshop `.csproj` fájl tartalmazza ezt a fallback-et, így a `dotnet run` bármely platformon alapból működik.

**Elvárt:** Az MS Learn dokumentáció `.csproj` sablonja tartalmazza ezt az automatikus felismerést, hogy a felhasználóknak ne kelljen emlékezniük a `-r` flag-re.

---

## 4. JavaScript Whisper — az audio átírás üres vagy bináris kimenetet ad vissza

**Állapot:** Nyitott (regresszió — az első jelentéshez képest romlott)  
**Súlyosság:** Súlyos  
**Komponens:** JavaScript Whisper implementáció (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reprodukció:** Futtasd: `node foundry-local-whisper.mjs` — az összes audió fájl vagy üres vagy bináris kimenetet ad a szöveges átírás helyett

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Eredetileg csak az 5. audió fájl adott üres kimenetet; a v0.9.x verzió óta az öt fájl mindegyike egyetlen bájtot (`\ufffd`) ad vissza szöveges átírás helyett. A Python Whisper implementáció az OpenAI SDK-t használva helyesen átírja ugyanazokat a fájlokat.

**Elvárt:** A `createAudioClient()`-nak szöveges átírást kellene visszaadnia, amely megegyezik a Python/C# implementációk eredményével.

---

## 5. C# SDK csak net8.0-át szállít — nincs hivatalos .NET 9 vagy .NET 10 támogatás

**Állapot:** Nyitott  
**Súlyosság:** Dokumentációs hiány  
**Komponens:** `Microsoft.AI.Foundry.Local` NuGet csomag v0.9.0  
**Telepítési parancs:** `dotnet add package Microsoft.AI.Foundry.Local`

A NuGet csomag csak egyetlen target framework-öt tartalmaz:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Nincs benne `net9.0` vagy `net10.0` TFM. Ezzel szemben az azt kísérő `Microsoft.Agents.AI.OpenAI` csomag (v1.0.0-rc3) támogatja a `net8.0`, `net9.0`, `net10.0`, `net472` és `netstandard2.0` verziókat.

### Kompatibilitási tesztelés

| Target framework | Build | Futás | Megjegyzés |
|-----------------|-------|-------|------------|
| net8.0          | ✅    | ✅    | Hivatalosan támogatott |
| net9.0          | ✅    | ✅    | Forward-compat használatával épül — a workshop mintákban használva |
| net10.0         | ✅    | ✅    | Épül és fut .NET 10.0.3 runtime-on forward-compat segítségével |

A net8.0 assembly betöltődik újabb runtime-okon a .NET forward-compat mechanizmusa miatt, ezért a build sikeres. Ezt azonban az SDK csapat nem dokumentálta és nem tesztelte.

### Miért célozzák meg a minták a net9.0-át

1. **.NET 9 a legfrissebb stabil kiadás** — a legtöbb workshop résztvevőnél telepítve van  
2. **Működik a forward kompatibilitás** — a NuGet net8.0 assembly-je gond nélkül fut a .NET 9 runtime-on  
3. **.NET 10 (preview/RC)** túl újnak számít egy workshop céljaihoz, ami mindenkinek működnie kell

**Elvárt:** A jövőbeli SDK kiadások vegyék fontolóra a `net9.0` és `net10.0` TFM-ek hozzáadását a `net8.0` mellé, hogy az `Microsoft.Agents.AI.OpenAI` mintáját kövessék és hivatalosan is támogassák az újabb runtime-okat.

---

## 6. JavaScript ChatClient Streaming callback-eket használ, nem async iterátorokat

**Állapot:** Nyitott  
**Súlyosság:** Dokumentációs hiány  
**Komponens:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

A `model.createChatClient()` által visszaadott `ChatClient` tartalmaz egy `completeStreamingChat()` metódust, de az **callback mintát** használ, nem pedig async iterálhatót:

```javascript
// ❌ Ez NEM működik — "stream nem aszinkron iterálható" hibát dob
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Helyes minta — adj át egy visszahívást
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Hatás:** Az OpenAI SDK async iterációs mintájával (`for await`) jártas fejlesztők zavaró hibákkal találkozhatnak. A callback-nek valóban érvényes függvénynek kell lennie, különben az SDK dobja a „Callback must be a valid function.” hibát.

**Elvárt:** Dokumentálják a callback mintát az SDK referenciában. Alternatív megoldásként támogassák az async iteráló mintát, hogy konzisztens legyen az OpenAI SDK-val.

---

## Környezeti adatok

| Komponens | Verzió |
|-----------|--------|
| OS | Windows 11 ARM64 |
| Hardver | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |