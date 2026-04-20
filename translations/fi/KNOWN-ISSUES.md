# Tunnetut ongelmat — Foundry Local Workshop

Ongelmat, joita esiintyi tämän työpajan rakentamisen ja testaamisen aikana **Snapdragon X Elite (ARM64)** -laitteella, jossa ajetaan Windowsia, Foundry Local SDK v0.9.0:lla, CLI v0.8.117:lla ja .NET SDK 10.0:lla.

> **Viimeksi validoitu:** 2026-03-11

---

## 1. Snapdragon X Elite -suoritinta ei tunnisteta ONNX Runtime -ympäristössä

**Tila:** Avoin  
**Vakavuus:** Varoitus (ei estävä)  
**Komponentti:** ONNX Runtime / cpuinfo  
**Toistettavuus:** Jokaisella Foundry Local -palvelun käynnistyksellä Snapdragon X Elite -laitteistolla

Joka kerta kun Foundry Local -palvelu käynnistyy, ilmestyy kaksi varoitusta:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Vaikutus:** Varoitukset ovat kosmeettisia — päätteenteko toimii oikein. Kuitenkin ne näkyvät jokaisella ajolla ja voivat hämmentää työpajan osallistujia. ONNX Runtime cpuinfo -kirjasto pitää päivittää tunnistamaan Qualcomm Oryon -suoritinytimet.

**Odotettu:** Snapdragon X Elite pitäisi tunnistaa tuettuna ARM64-suorittimena ilman virheilmoitustason viestejä.

---

## 2. SingleAgent NullReferenceException ensimmäisellä ajokerralla

**Tila:** Avoin (satunnainen)  
**Vakavuus:** Kriittinen (kaatuminen)  
**Komponentti:** Foundry Local C# SDK + Microsoft Agent Framework  
**Toistettavuus:** Suorita `dotnet run agent` — kaatuu heti mallin lataamisen jälkeen

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Konteksti:** Rivillä 37 kutsutaan `model.IsCachedAsync(default)`. Kaatuminen tapahtui agentin ensimmäisellä ajolla heti `foundry service stop` -komennon jälkeen. Myöhemmät ajot samalla koodilla onnistuivat.

**Vaikutus:** Satunnainen — viittaa kilpailutilanteeseen SDK:n palvelun käynnistyksen tai luettelokyselyn aikana. `GetModelAsync()` voi palautua ennen kuin palvelu on täysin valmis.

**Odotettu:** `GetModelAsync()` pitäisi joko lukita kunnes palvelu on valmis tai palauttaa selkeä virheilmoitus, jos palvelu ei ole vielä valmis.

---

## 3. C# SDK vaatii eksplisiittisen RuntimeIdentifierin

**Tila:** Avoin — seurattu osoitteessa [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Vakavuus:** Dokumentaatiovaje  
**Komponentti:** `Microsoft.AI.Foundry.Local` NuGet-paketti  
**Toistettavuus:** Luo .NET 8+ -projekti ilman `<RuntimeIdentifier>` -määritystä `.csproj`-tiedostossa

Käännös epäonnistuu seuraavasti:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Juuri:** RID-vaatimuksen odotetaan olevan tarpeen — SDK sisältää natiivibinaareja (P/Invoke `Microsoft.AI.Foundry.Local.Core`-kirjastoon ja ONNX Runtimeen), joten .NET:n täytyy tietää mihin alustakohtaiseen kirjastoon viitataan.

Tämä on dokumentoitu MS Learnissa ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), jossa suoritusohjeissa näkyy:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Käyttäjien täytyy kuitenkin muistaa käyttää `-r`-valitsinta joka kerta, mikä on helppo unohtaa.

**Väliaikainen ratkaisu:** Lisää automaattinen tunnistusvarmistus `.csproj`:hisi, jotta `dotnet run` toimii ilman lippuja:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` on sisäänrakennettu MSBuild-ominaisuus, joka määrittelee automaattisesti isäntäkoneen RID:n. SDK:n omissa testiprojekteissa tätä kaavaa käytetään jo. Explisiittiset `-r`-liput toimivat edelleen, jos ne annetaan.

> **Huom:** Työpajan `.csproj` sisältää tämän varmistuksen, jotta `dotnet run` toimii suoraan millä tahansa alustalla.

**Odotettu:** MS Learnin `.csproj`-pohjassa pitäisi olla tämä automaattinen tunnistusmalli, jotta käyttäjien ei tarvitse muistaa `-r`-valitsinta.

---

## 4. JavaScript Whisper — Äänitallenteen litterointi palauttaa tyhjän tai binäärisen sisällön

**Tila:** Avoin (regressio — pahentunut alkuperäisestä raportista)  
**Vakavuus:** Merkittävä  
**Komponentti:** JavaScript Whisper -toteutus (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Toistettavuus:** Aja `node foundry-local-whisper.mjs` — kaikki äänitiedostot palauttavat tyhjän tai binäärisen sisällön tekstilitteroinnin sijaan

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Alkujaan vain viides äänitiedosto palautti tyhjän; versiossa v0.9.x kaikki viisi tiedostoa palauttavat yhden tavun (`\ufffd`) tekstin sijaan. Python Whisper -toteutus käyttäen OpenAI SDK:a litteroi samat tiedostot oikein.

**Odotettu:** `createAudioClient()` pitäisi palauttaa tekstilitterointi, joka vastaa Python/C# -toteutuksia.

---

## 5. C# SDK toimittaa vain net8.0 — ei virallista .NET 9 tai .NET 10 -tukea

**Tila:** Avoin  
**Vakavuus:** Dokumentaatiovaje  
**Komponentti:** `Microsoft.AI.Foundry.Local` NuGet-paketti v0.9.0  
**Asennuskomento:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet-paketti sisältää vain yhden tavoitekehyksen:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Ei sisällä `net9.0` tai `net10.0` TFM:iä. Verrattuna tähän, kumppanipaketti `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) sisältää `net8.0`, `net9.0`, `net10.0`, `net472` ja `netstandard2.0`.

### Yhteensopivuustestaus

| Kohdekehys | Käännös | Ajettavuus | Huomautukset |
|------------|---------|------------|--------------|
| net8.0     | ✅      | ✅         | Virallisesti tuettu |
| net9.0     | ✅      | ✅         | Käännetty eteenpäin-yhteensopivuuden avulla — käytössä työpajan esimerkeissä |
| net10.0    | ✅      | ✅         | Kääntyy ja toimii eteenpäin-yhteensopivuudella .NET 10.0.3 -ympäristössä |

net8.0-kokoonpano latautuu uudemmissa ympäristöissä .NET:n eteenpäin-yhteensopivuusmekanismin avulla, joten käännös onnistuu. Tämä ei kuitenkaan ole dokumentoitu eikä SDK-tiimin testaama.

### Miksi esimerkit kohdistuvat net9.0:aan

1. **.NET 9 on uusin vakaa julkaisu** — useimmilla työpajan osallistujilla on se asennettuna  
2. **Eteenpäin-yhteensopivuus toimii** — net8.0-kokoonpano NuGet-paketissa toimii .NET 9 -ympäristössä ongelmitta  
3. **.NET 10 (esikatselu/RC)** on liian uusi kohde työpajalle, joka haluaa toimia kaikilla

**Odotettu:** Tulevissa SDK-versioissa tulisi harkita `net9.0` ja `net10.0` -tavoitekehysten lisäämistä `net8.0` rinnalle, vastaavasti kuin `Microsoft.Agents.AI.OpenAI` käyttää, tarjoten vahvistetun tuen uudempia runtimeja varten.

---

## 6. JavaScript ChatClientin suoratoisto käyttää callbackeja, ei async-iteraatoreita

**Tila:** Avoin  
**Vakavuus:** Dokumentaatiovaje  
**Komponentti:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient`, jonka saa `model.createChatClient()`-kutsulla, tarjoaa metodin `completeStreamingChat()`, mutta se käyttää **callback-mallia** eikä palauta async-iteroitavaa olioita:

```javascript
// ❌ Tämä EI toimi — heittää "stream is not async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Oikea malli — välitä callback-funktio
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Vaikutus:** Kehittäjät, jotka ovat tottuneet OpenAI SDK:n async-iteraatio-malliin (`for await`), kohtaavat hämmentäviä virheitä. Callbackin täytyy olla kelvollinen funktio, muuten SDK antaa virheen "Callback must be a valid function."

**Odotettu:** Dokumentoida callback-malli SDK-viitteissä. Vaihtoehtoisesti tukea async-iteraatio-malli yhtenevästi OpenAI SDK:n kanssa.

---

## Ympäristön tiedot

| Komponentti | Versio |
|-------------|---------|
| Käyttöjärjestelmä | Windows 11 ARM64 |
| Laitteisto | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |