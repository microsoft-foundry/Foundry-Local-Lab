# Žinomos problemos — Foundry Local dirbtuvės

Problemos, kurios kilo statant ir testuojant šias dirbtuves **Snapdragon X Elite (ARM64)** įrenginyje su Windows, naudojant Foundry Local SDK v0.9.0, CLI v0.8.117 ir .NET SDK 10.0.

> **Paskutinė patikra:** 2026-03-11

---

## 1. Snapdragon X Elite CPU nėra atpažįstamas ONNX Runtime

**Būsena:** Atvira  
**Svarba:** Įspėjimas (neblokuojantis)  
**Komponentas:** ONNX Runtime / cpuinfo  
**Reprodukavimas:** Kiekvienas Foundry Local paslaugos paleidimas Snapdragon X Elite aparatinėje įrangoje

Kiekvieną kartą paleidžiant Foundry Local paslaugą, išvedami du įspėjimai:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Poveikis:** Įspėjimai yra kosmetiniai — inferencijos procesas veikia teisingai. Tačiau jie rodomi kiekvieną kartą ir gali suklaidinti dirbtuvių dalyvius. ONNX Runtime cpuinfo biblioteka turi būti atnaujinta, kad atpažintų Qualcomm Oryon CPU branduolius.

**Tikėtina:** Snapdragon X Elite turėtų būti atpažintas kaip palaikomas ARM64 CPU be klaidų lygio pranešimų.

---

## 2. SingleAgent NullReferenceException pirmojo paleidimo metu

**Būsena:** Atvira (kartais)  
**Svarba:** Kritinė (programos avarija)  
**Komponentas:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reprodukavimas:** Paleiskite `dotnet run agent` — sugenda iš karto po modelio užkrovimo

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Kontekstas:** 37-oje eilutėje kviečiamas `model.IsCachedAsync(default)`. Avarija įvyko pirmojo agente paleidimo metu po šviežio `foundry service stop`. Vėlesni paleidimai su tuo pačiu kodu pavyko.

**Poveikis:** Kartotinė problema — tai rodo konkurencinį būsenos valdymo trūkumą SDK paslaugos pradžioje arba katalogo užklausoje. `GetModelAsync()` gali grąžinti rezultatą prieš paslaugai pilnai pasiruošiant.

**Tikėtina:** `GetModelAsync()` turėtų blokuoti užklausą, kol paslauga nebus pasirengusi, arba grąžinti aiškų klaidos pranešimą, jei paslauga dar nėra užbaigusi inicializacijos.

---

## 3. C# SDK reikalauja aiškaus RuntimeIdentifier nurodymo

**Būsena:** Atvira — stebima [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Svarba:** Dokumentacijos trūkumas  
**Komponentas:** `Microsoft.AI.Foundry.Local` NuGet paketas  
**Reprodukavimas:** Sukurti .NET 8+ projektą be `<RuntimeIdentifier>` `.csproj` faile

Kompiliacija nepavyksta su klaida:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Pagrindinė priežastis:** RID reikalavimas yra normalus — SDK siunčia vietinius dvejetainius failus (P/Invoke į `Microsoft.AI.Foundry.Local.Core` ir ONNX Runtime), todėl .NET turi žinoti, kuri platformai skirta biblioteka turi būti naudojama.

Tai aprašyta MS Learn dokumentacijoje ([Kaip naudoti vietines pokalbių pabaigas](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), kur vykdymo instrukcijos rodo:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Tačiau vartotojai turi kiekvieną kartą prisiminti `-r` parametrą, kuris lengvai pamirštamas.

**Sprendimas:** Pridėkite automatinio aptikimo atsarginį variantą į savo `.csproj`, kad `dotnet run` veiktų be papildomų parametrų:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` yra įmontuotas MSBuild savybė, kuri automatiškai nustato kompiuterio host'o RID. SDK testų projektai jau naudoja šį modelį. Aiškūs `-r` parametrai visada bus paisomi, jei nurodyti.

> **Pastaba:** Dirbtuvių `.csproj` apima šį atsarginį variantą, todėl `dotnet run` veikia be papildomų žingsnių bet kurioje platformoje.

**Tikėtina:** `.csproj` šablonas MS Learn dokumentacijoje turėtų apimti šį automatinio aptikimo modelį, kad vartotojai neprivalėtų prisiminti `-r` parametro.

---

## 4. JavaScript Whisper — garso transkripcija grąžina tuščią arba dvejetainį išvestį

**Būsena:** Atvira (regresija — pablogėjo nuo pirminio pranešimo)  
**Svarba:** Svarbi  
**Komponentas:** JavaScript Whisper įgyvendinimas (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reprodukavimas:** Paleiskite `node foundry-local-whisper.mjs` — visi garso failai grąžina tuščią arba dvejetainę išvestį vietoje teksto transkripcijos

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Iš pradžių tik penktasis garso failas grąžino tuščią išvestį; nuo v0.9.x versijos visi 5 failai grąžina vieną baitą (`\ufffd`) vietoje ištranskribuoto teksto. Python Whisper įgyvendinimas naudojant OpenAI SDK teisingai transkribuoja tuos pačius failus.

**Tikėtina:** `createAudioClient()` turėtų grąžinti transkripciją kaip tekstą, sutampantį su Python/C# įgyvendinimais.

---

## 5. C# SDK siunčia tik net8.0 — nėra oficialaus .NET 9 ar .NET 10 tikslo

**Būsena:** Atvira  
**Svarba:** Dokumentacijos trūkumas  
**Komponentas:** `Microsoft.AI.Foundry.Local` NuGet paketas v0.9.0  
**Įdiegimo komanda:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet paketas siunčia tik vieną taikymo rėmą:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Paketas neturi `net9.0` ar `net10.0` TFM (target framework moniker). Tuo tarpu papildomas paketas `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) siunčia `net8.0`, `net9.0`, `net10.0`, `net472` ir `netstandard2.0`.

### Suderinamumo testavimas

| Tikslinis rėmas | Kompiliacija | Paleidimas | Pastabos |
|-----------------|--------------|------------|----------|
| net8.0 | ✅ | ✅ | Oficialiai palaikomas |
| net9.0 | ✅ | ✅ | Kompiliuojasi per forward-compat — naudojama dirbtuvių pavyzdžiuose |
| net10.0 | ✅ | ✅ | Kompiliuojasi ir veikia per forward-compat su .NET 10.0.3 runtime |

Net8.0 surinkimas veikia naujesniuose vykdymo varikliuose dėl .NET forward-compat mechanizmo, todėl kompiliacija sėkminga. Tačiau tai nėra dokumentuota ir nėra testuota SDK komandos.

### Kodėl pavyzdžiai naudojami su net9.0

1. **.NET 9 yra naujausia stabili versija** — dauguma dirbtuvių dalyvių ją turi įdiegtą  
2. **Forward compatibility veikia** — net8.0 sudėtis NuGet pakete veikia .NET 9 vykdymo variklyje be problemų  
3. **.NET 10 (preview/RC)** yra per naujas, kad sukurtume dirbtuves, kurios veiktų visiems

**Tikėtina:** Ateities SDK versijos turėtų apsvarstyti `net9.0` ir `net10.0` TFM pridėjimą kartu su `net8.0`, kaip daroma su `Microsoft.Agents.AI.OpenAI` paketu, kad užtikrintų patvirtintą palaikymą naujesnėms vykdymo aplinkoms.

---

## 6. JavaScript ChatClient srautinimas naudoja atgalinius kvietimus, o ne asinchroninius iteracijos objektus

**Būsena:** Atvira  
**Svarba:** Dokumentacijos trūkumas  
**Komponentas:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient`, grąžinamas per `model.createChatClient()`, turi metodą `completeStreamingChat()`, tačiau jis naudoja **atgalinio kvietimo (callback) modelį** vietoje asinchroninės iteracijos:

```javascript
// ❌ Tai NEVEIKIA — išmeta "stream is not async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Teisingas modelis — perduokite atgalinį kvietimą
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Poveikis:** Kūrėjai, pažįstantys OpenAI SDK asinchroninės iteracijos (`for await`) modelį, susidurs su klaidingais klaidų pranešimais. Callback turi būti galiojanti funkcija, kitaip SDK mestų klaidą „Callback must be a valid function.“

**Tikėtina:** SDR dokumentacijoje turi būti aprašytas callback modelis. Arba, jei įmanoma, turėtų būti palaikomas asinchroninės iteracijos modelis, kad būtų suderinta su OpenAI SDK.

---

## Aplinkos duomenys

| Komponentas | Versija |
|-------------|---------|
| OS | Windows 11 ARM64 |
| Aparatūra | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:  
Šis dokumentas buvo išverstas naudojant dirbtinio intelekto vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors siekiame tikslumo, prašome atkreipti dėmesį, kad automatiniai vertimai gali turėti klaidų ar netikslumų. Pirminis dokumentas jo gimtąja kalba turi būti laikomas autoritetingu šaltiniu. Kritinei informacijai rekomenduojama naudoti profesionalų žmogaus atliktą vertimą. Mes neatsakome už bet kokius nesusipratimus ar klaidingą interpretaciją, kylančią dėl šio vertimo naudojimo.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->