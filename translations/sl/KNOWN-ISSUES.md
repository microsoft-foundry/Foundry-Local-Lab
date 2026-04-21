# Znane težave — Foundry Local delavnica

Težave, ki so se pojavile med gradnjo in testiranjem te delavnice na napravi **Snapdragon X Elite (ARM64)** z operacijskim sistemom Windows, z Foundry Local SDK v0.9.0, CLI v0.8.117 in .NET SDK 10.0.

> **Zadnja potrjena:** 2026-03-11

---

## 1. Snapdragon X Elite CPU ni prepoznan s strani ONNX Runtime

**Status:** Odprto  
**Resnost:** Opozorilo (neblokirajoče)  
**Komponenta:** ONNX Runtime / cpuinfo  
**Reprodukcija:** Vsak zagon Foundry Local storitve na strojni opremi Snapdragon X Elite

Ob vsakem zagonu Foundry Local storitve se pojavita dve opozorili:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Vpliv:** Opozorila so le kozmetična — sklepanje deluje pravilno. Vendar se pojavijo ob vsakem zagonu in lahko zmedejo udeležence delavnice. Knjižnica ONNX Runtime cpuinfo je potrebno posodobiti, da prepozna Qualcomm Oryon CPU jedra.

**Pričakovano:** Snapdragon X Elite bi moral biti prepoznan kot podprt ARM64 CPU brez sporočil o napakah na nivoju error.

---

## 2. SingleAgent NullReferenceException ob prvem zagonu

**Status:** Odprto (priključno)  
**Resnost:** Kritično (padec)  
**Komponenta:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reprodukcija:** Zaženi `dotnet run agent` — pade takoj po nalaganju modela

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Kontekst:** Vrstica 37 kliče `model.IsCachedAsync(default)`. Napaka se je pojavila ob prvem zagonu agenta po svežem `foundry service stop`. Naslednji zagoni z isto kodo so uspeli.

**Vpliv:** Priključno — nakazuje na stanje gara za inicializacijo storitve SDK ali poizvedbo kataloga. Klic `GetModelAsync()` se lahko izvrši preden je storitev popolnoma pripravljena.

**Pričakovano:** `GetModelAsync()` bi moral bodisi blokirati, dokler storitev ni pripravljena, ali vrniti jasno sporočilo o napaki, če storitev še ni zaključila inicializacije.

---

## 3. C# SDK zahteva eksplicitni RuntimeIdentifier

**Status:** Odprto — sledi se v [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Resnost:** Pomanjkljivost dokumentacije  
**Komponenta:** `Microsoft.AI.Foundry.Local` NuGet paket  
**Reprodukcija:** Ustvari .NET 8+ projekt brez `<RuntimeIdentifier>` v `.csproj`

Gradnja spodleti z:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Vzrok:** Zahteva po RID je pričakovana — SDK vsebuje nativne binarne datoteke (P/Invoke v `Microsoft.AI.Foundry.Local.Core` in ONNX Runtime), zato mora .NET vedeti, katero platformo specifično knjižnico naj razreši.

To je dokumentirano na MS Learn ([Kako uporabljati native chat zaključke](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), kjer navodila za zagon prikazujejo:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Vendar pa morajo uporabniki vsakokrat zapomniti `-r` zastavico, kar je lahko pozabljeno.

**Zaobidenje:** Dodajte samodejno odkrivanje v vaš `.csproj`, tako da `dotnet run` deluje brez zastavic:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` je vgrajena lastnost MSBuild, ki samodejno razreši RID gostiteljske naprave. Testni projekti SDK že uporabljajo ta vzorec. Eksplicitne `-r` zastavice so še vedno sprejete, če so podane.

> **Opomba:** `.csproj` delavnice vključuje to zaobidenje, da `dotnet run` deluje takoj na kateri koli platformi.

**Pričakovano:** `.csproj` predloga v MS Learn dokumentaciji naj vključuje ta samodejni vzorec, da uporabniki ne bodo morali zapomniti `-r` zastavice.

---

## 4. JavaScript Whisper — Avdio prepis vrača prazno/binarnio vsebino

**Status:** Odprto (regresija — poslabšano od začetnega poročila)  
**Resnost:** Hudo  
**Komponenta:** JavaScript Whisper implementacija (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reprodukcija:** Zaženi `node foundry-local-whisper.mjs` — vse avdio datoteke vračajo prazno ali binarno vsebino namesto besedilnega prepisa

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Sprva je prazno vračala le 5. avdio datoteka; od v0.9.x pa vseh 5 datotek vrača en bajt (`\ufffd`) namesto prepisanega teksta. Python Whisper implementacija z uporabo OpenAI SDK pravilno prepisuje iste datoteke.

**Pričakovano:** `createAudioClient()` bi moral vrniti besedilni prepis, ki je v skladu s Python/C# implementacijami.

---

## 5. C# SDK pošilja samo net8.0 — brez uradnih ciljnih okvirov .NET 9 ali .NET 10

**Status:** Odprto  
**Resnost:** Pomanjkljivost dokumentacije  
**Komponenta:** `Microsoft.AI.Foundry.Local` NuGet paket v0.9.0  
**Ukaz za namestitev:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet paket vsebuje samo en ciljni okvir:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Ne vsebuje TFM-jev `net9.0` ali `net10.0`. V primerjavi, spremljajoči paket `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) vključuje `net8.0`, `net9.0`, `net10.0`, `net472` in `netstandard2.0`.

### Testiranje združljivosti

| Ciljni okvir | Gradnja | Zagon | Opombe |
|--------------|---------|-------|--------|
| net8.0       | ✅      | ✅    | Uradno podprto |
| net9.0       | ✅      | ✅    | Gradi preko forward-compat, uporabljen v vzorcih delavnice |
| net10.0      | ✅      | ✅    | Gradi in teče preko forward-compat z .NET 10.0.3 runtime |

net8.0 zvezek se naloži na novejših runtime-ih preko mehanizma .NET forward-compat, zato gradnja uspe. Vendar pa tega SDK ekipa ne dokumentira in ne testira.

### Zakaj vzorci ciljajo net9.0

1. **.NET 9 je najnovejša stabilna izdaja** — večina udeležencev delavnice ga ima nameščenega  
2. **Forward compatibility deluje** — net8.0 zvezek v NuGet paketu teče na .NET 9 runtime brez težav  
3. **.NET 10 (preview/RC)** je premlad za ciljanje v delavnici, ki naj bi delovala za vse

**Pričakovano:** Prihodnje izdaje SDK naj dodajo `net9.0` in `net10.0` TFM-je skupaj z `net8.0`, da sledijo vzorcu `Microsoft.Agents.AI.OpenAI` in zagotovijo validirano podporo za novejše runtime.

---

## 6. JavaScript ChatClient Streaming uporablja povratne klice, ne asinhronih iteratorjev

**Status:** Odprto  
**Resnost:** Pomanjkljivost dokumentacije  
**Komponenta:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient`, ki ga vrne `model.createChatClient()`, ponuja metodo `completeStreamingChat()`, vendar uporablja **vzorec povratnega klica** namesto, da bi vrnil asinhrono iterabilen objekt:

```javascript
// ❌ To NE deluje — vrže "stream ni asinhrono iterabilen"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Pravilen vzorec — posreduj klicno funkcijo
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Vpliv:** Razvijalci, ki so vajeni asinhronega iteracijskega vzorca OpenAI SDK (`for await`), bodo naleteli na zmedene napake. Povratni klic mora biti veljavna funkcija, sicer SDK vrže "Callback must be a valid function."

**Pričakovano:** Dokumentirati vzorec povratnega klica v SDK referenci. Alternativno, podpreti asinhroni iterabilni vzorec za skladnost z OpenAI SDK.

---

## Podrobnosti okolja

| Komponenta | Verzija |
|------------|---------|
| OS | Windows 11 ARM64 |
| Strojna oprema | Snapdragon X Elite (X1E78100) |
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
**Izjava o omejitvi odgovornosti**:  
To besedilo je bilo prevedeno z uporabo AI prevajalske storitve [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, vas prosimo, da upoštevate, da lahko avtomatizirani prevodi vsebujejo napake ali netočnosti. Izvirni dokument v njegovem izvirnem jeziku velja za avtoritativni vir. Za kritične informacije se priporoča strokovni človeški prevod. Za morebitno nerazumevanje ali napačne interpretacije, ki izhajajo iz uporabe tega prevoda, ne prevzemamo odgovornosti.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->