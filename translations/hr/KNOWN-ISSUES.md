# Poznati Problemi — Foundry Local Workshop

Problemi nastali tijekom izgradnje i testiranja ovog workshopa na uređaju **Snapdragon X Elite (ARM64)** s Windowsom, koristeći Foundry Local SDK v0.9.0, CLI v0.8.117 i .NET SDK 10.0.

> **Zadnja potvrda:** 2026-03-11

---

## 1. Snapdragon X Elite CPU Nije Prepoznat od ONNX Runtime

**Status:** Otvoreno  
**Težina:** Upozorenje (neblokirajuće)  
**Komponenta:** ONNX Runtime / cpuinfo  
**Reprodukcija:** Svaki start Foundry Local servisa na Snapdragon X Elite hardveru

Svaki put kad se Foundry Local servis pokrene, ispisuju se dva upozorenja:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Utjecaj:** Upozorenja su kozmetička — inferencija radi ispravno. Međutim, pojavljuju se pri svakom pokretanju i mogu zbuniti sudionike workshopa. ONNX Runtime cpuinfo biblioteka treba biti ažurirana da prepozna Qualcomm Oryon CPU jezgre.

**Očekivano:** Snapdragon X Elite bi trebao biti prepoznat kao podržani ARM64 CPU bez ispisivanja poruka o pogrešci.

---

## 2. SingleAgent NullReferenceException pri Prvom Pokretanju

**Status:** Otvoreno (povremeno)  
**Težina:** Kritično (pad)  
**Komponenta:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reprodukcija:** Pokrenuti `dotnet run agent` — odmah pada nakon učitavanja modela

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Kontekst:** Redak 37 poziva `model.IsCachedAsync(default)`. Pad se dogodio pri prvom pokretanju agenta nakon svježeg `foundry service stop`. Naknadna pokretanja sa istim kodom su uspjela.

**Utjecaj:** Povremen — ukazuje na uvjet utrke u inicijalizaciji servisa SDK-a ili upitu kataloga. Poziv `GetModelAsync()` može vratiti rezultat prije nego što je servis u potpunosti spreman.

**Očekivano:** `GetModelAsync()` bi trebao ili čekati dok servis ne postane spreman ili vratiti jasnu poruku o pogrešci ako servis nije završio inicijalizaciju.

---

## 3. C# SDK Zahtijeva Izravan RuntimeIdentifier

**Status:** Otvoreno — praćeno u [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Težina:** Nedostatak u dokumentaciji  
**Komponenta:** `Microsoft.AI.Foundry.Local` NuGet paket  
**Reprodukcija:** Kreirati .NET 8+ projekt bez `<RuntimeIdentifier>` u `.csproj`

Gradnja neuspijeva s:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Korijen problema:** Zahtjev za RID je očekivan — SDK sadrži nativne binarije (P/Invoke u `Microsoft.AI.Foundry.Local.Core` i ONNX Runtime), stoga .NET mora znati koju platformu specifičnu biblioteku će riješiti.

Ovo je dokumentirano na MS Learn ([Kako koristiti nativne chat dovršetke](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), gdje uputstva za pokretanje prikazuju:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Međutim, korisnici moraju svaki put pamtiti `-r` zastavicu, što je lako zaboraviti.

**Zaobilazno rješenje:** Dodajte auto-otkrivajući fallback u svoj `.csproj` kako bi `dotnet run` radio bez ikakvih zastavica:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` je ugrađeno MSBuild svojstvo koje automatski razrješava RID host stroja. SDK vlastiti test projekti već koriste ovaj obrazac. Izravne `-r` zastavice se i dalje poštuju kada su navedene.

> **Napomena:** `.csproj` workshopa uključuje ovaj fallback pa `dotnet run` radi odmah na bilo kojoj platformi.

**Očekivano:** `.csproj` predložak u MS Learn dokumentaciji treba uključiti ovaj auto-otkrivajući obrazac da korisnici ne moraju pamtiti `-r` zastavicu.

---

## 4. JavaScript Whisper — Transkripcija Zvuka Vraća Prazan/Binarni Izlaz

**Status:** Otvoreno (regresija — pogoršalo se od početnog izvještaja)  
**Težina:** Važno  
**Komponenta:** JavaScript Whisper implementacija (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reprodukcija:** Pokrenuti `node foundry-local-whisper.mjs` — svi audio fajlovi vraćaju praznu ili binarnu izlaznu vrijednost umjesto tekstualne transkripcije

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Prvobitno je samo 5. audio fajl vraćao prazno; u verziji v0.9.x svi su fajlovi vratili jedan bajt (`\ufffd`) umjesto transkribiranog teksta. Python Whisper implementacija uz korištenje OpenAI SDK-a ispravno transkribira iste fajlove.

**Očekivano:** `createAudioClient()` bi trebao vratiti tekstualnu transkripciju koja odgovara Python/C# implementacijama.

---

## 5. C# SDK Podrška Samo net8.0 — Nema Službenih Ciljeva za .NET 9 ili .NET 10

**Status:** Otvoreno  
**Težina:** Nedostatak u dokumentaciji  
**Komponenta:** `Microsoft.AI.Foundry.Local` NuGet paket v0.9.0  
**Naredba za instalaciju:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet paket sadrži samo jedan ciljni okvir:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Nema uključenog `net9.0` ili `net10.0` TFM-a. Nasuprot tome, prateći paket `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) sadrži `net8.0`, `net9.0`, `net10.0`, `net472` i `netstandard2.0`.

### Testiranje Kompatibilnosti

| Ciljni okvir | Izgradnja | Pokretanje | Napomene |
|--------------|----------|------------|----------|
| net8.0       | ✅       | ✅         | Službeno podržano |
| net9.0       | ✅       | ✅         | Izgradnja putem forward-kompatibilnosti — korišteno u primjerima workshopa |
| net10.0      | ✅       | ✅         | Izgradnja i pokretanje putem forward-kompatibilnosti s .NET 10.0.3 runtime |

net8.0 assembly se učitava na novijim runtime-ovima preko .NET forward-kompatibilnog mehanizma, pa gradnja uspijeva. Ipak, to nije dokumentirano niti testirano od strane SDK tima.

### Zašto Primjeri Ciljaju net9.0

1. **.NET 9 je najnovije stabilno izdanje** — većina sudionika workshopa će ga imati instaliranog  
2. **Forward kompatibilnost funkcionira** — net8.0 assembly u NuGet paketu radi bez problema na .NET 9 runtime-u  
3. **.NET 10 (preview/RC)** je previše nov za cilj radionice koja treba raditi za sve

**Očekivano:** Buduća izdanja SDK-a trebaju razmotriti uključivanje `net9.0` i `net10.0` TFM-ova zajedno s `net8.0` da odgovaraju obrascu korištenom u `Microsoft.Agents.AI.OpenAI` i da pruže validiranu podršku za novije runtime-ove.

---

## 6. JavaScript ChatClient Streaming Koristi Povratne Pozive, Ne Async Iteratore

**Status:** Otvoreno  
**Težina:** Nedostatak u dokumentaciji  
**Komponenta:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` koji vraća `model.createChatClient()` ima metodu `completeStreamingChat()`, ali koristi **callback obrazac** umjesto da vraća async iterable:

```javascript
// ❌ Ovo NE radi — baca "stream nije asinkrono iterabilan"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Ispravan uzorak — proslijedite povratni poziv
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Utjecaj:** Programeri koji su upoznati s async iteracijskim obrascem OpenAI SDK-a (`for await`) mogu naići na zbunjujuće pogreške. Callback mora biti valjana funkcija ili SDK baca "Callback must be a valid function."

**Očekivano:** Dokumentirati callback obrazac u SDK referenci. Alternativno, podržati async iterable obrazac radi konzistentnosti s OpenAI SDK-om.

---

## Detalji Okruženja

| Komponenta | Verzija |
|------------|---------|
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

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje odgovornosti**:
Ovaj dokument je preveden korištenjem AI prevoditeljske usluge [Co-op Translator](https://github.com/Azure/co-op-translator). Iako nastojimo postići točnost, molimo imajte na umu da automatski prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na izvornom jeziku treba smatrati autoritativnim izvorom. Za kritične informacije preporučuje se profesionalni ljudski prijevod. Nismo odgovorni za bilo kakva nesporazumevanja ili krive tumačenja nastale upotrebom ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->