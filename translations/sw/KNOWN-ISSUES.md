# Masuala Yanayojulikana — Warsha ya Foundry Local

Masuala yaliyojitokeza wakati wa kujenga na kujaribu warsha hii kwenye kifaa cha **Snapdragon X Elite (ARM64)** kinachoendesha Windows, pamoja na Foundry Local SDK v0.9.0, CLI v0.8.117, na .NET SDK 10.0.

> **Ilikadiriwa mwisho:** 2026-03-11

---

## 1. CPU ya Snapdragon X Elite Haijatambuliwa na ONNX Runtime

**Hali:** Wazi
**Ukali:** Onyo (haizuizi)
**Sehemu:** ONNX Runtime / cpuinfo
**Kuzaa Tena:** Kila kuanzishwa kwa huduma ya Foundry Local kwenye vifaa vya Snapdragon X Elite

Kila wakati huduma ya Foundry Local inaanza, onyo mbili hutolewa:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**Athari:** Onyo hizi ni za kidunia tu — utambuzi hufanya kazi kwa usahihi. Hata hivyo, zinaonekana kila mara na zinaweza kuchanganya washiriki wa warsha. Maktaba ya ONNX Runtime cpuinfo inahitaji kusasishwa ili itambue nyimbo za CPU za Qualcomm Oryon.

**Imetarajiwa:** Snapdragon X Elite inapaswa kutambuliwa kama CPU inayoungwa mkono ya ARM64 bila kutoa ujumbe wa makosa ya kiwango cha hitilafu.

---

## 2. SingleAgent NullReferenceException Katika Mwendeshaji wa Kwanza

**Hali:** Wazi (wakati mwingine)
**Ukali:** Hatari (kugonga)
**Sehemu:** Foundry Local C# SDK + Microsoft Agent Framework
**Kuzaa Tena:** Endesha `dotnet run agent` — hugonga mara moja baada ya kupakia modeli

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**Muktadha:** Mstari wa 37 unaita `model.IsCachedAsync(default)`. Hitilafu ilitokea wakati wa mwendeshaji wa kwanza baada ya `foundry service stop` mpya. Miondoko inayofuata kwa msimbo uleule iliweza kufanikiwa.

**Athari:** Muda mrefu — inaashiria hali ya mshindano katika kuanzishwa kwa huduma katika SDK au utafutaji wa katalogi. Msimbo wa `GetModelAsync()` unaweza kurudi kabla huduma haijakomaa kabisa.

**Imetarajiwa:** `GetModelAsync()` inapaswa kuzuia hadi huduma ianze kabisa au kurudisha ujumbe wa makosa wazi ikiwa huduma haijakomaa.

---

## 3. SDK ya C# Inahitaji RuntimeIdentifier Wilde

**Hali:** Wazi — inafuatiliwa katika [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)
**Ukali:** Pengo katika nyaraka
**Sehemu:** kifurushi cha NuGet `Microsoft.AI.Foundry.Local`
**Kuzaa Tena:** Tengeneza mradi wa .NET 8+ bila `<RuntimeIdentifier>` kwenye `.csproj`

Ujenzi unashindwa kwa:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**Muzizi wa tatizo:** Hitaji la RID linatarajiwa — SDK huleta binary za asili (P/Invoke kwa `Microsoft.AI.Foundry.Local.Core` na ONNX Runtime), kwa hivyo .NET inahitaji kujua maktaba ya jukwaa maalum ya kutatua.

Hii imedokumentiwa kwenye MS Learn ([Jinsi ya kutumia chat completions za asili](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), ambapo maelekezo ya kuendesha yanaonyesha:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

Hata hivyo, watumiaji wanapaswa kukumbuka kipi `-r` kila wakati, jambo ambalo ni rahisi kusahau.

**Ufumbuzi wa muda:** Ongeza utambuzi otomatiki kwenye `.csproj` yako ili `dotnet run` ifanye kazi bila bendera zozote:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` ni mali ya MSBuild inayotatua RID ya mashine mwenyeji moja kwa moja. Miradi ya kung'arisha ya SDK tayari hutumia mtindo huu. Bendera za wazi `-r` bado zinazingatiwa inapowekwa.

> **Kumbuka:** `.csproj` ya warsha inashirikisha utambuzi huu otomatiki ili `dotnet run` ifanye kazi moja kwa moja kwenye jukwaa lolote.

**Imetarajiwa:** Kiolezo cha `.csproj` katika nyaraka za MS Learn kinapaswa kujumuisha mfano huu wa utambuzi otomatiki ili watumiaji wasilitaje kipi `-r`.

---

## 4. JavaScript Whisper — Ubadilishaji wa Sauti Hurejea Tokeo Tupu/Kibinafsi

**Hali:** Wazi (mara mojaiki — imezidi kuwa mbaya tangu ripoti ya awali)
**Ukali:** Kuu
**Sehemu:** Utekelezaji wa JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**Kuzaa Tena:** Endesha `node foundry-local-whisper.mjs` — faili zote za sauti hurejea matokeo tupu au ya binary badala ya maneno yaliyotafsiriwa

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

Awali faili ya sauti ya tano tu ilirudisha tupu; kuanzia v0.9.x, faili zote 5 hurudisha bajeti moja (`\ufffd`) badala ya maneno yaliyotafsiriwa. Utekelezaji wa Python Whisper ukitumia OpenAI SDK hutafsiri faili hizo kwa usahihi.

**Imetarajiwa:** `createAudioClient()` inapaswa kurudisha tafsiri ya maandishi inayolingana na utekelezaji wa Python/C#.

---

## 5. SDK ya C# Hutoa tu net8.0 — Hakuna Lengo Rasmi la .NET 9 au .NET 10

**Hali:** Wazi
**Ukali:** Pengo katika nyaraka
**Sehemu:** kifurushi cha NuGet `Microsoft.AI.Foundry.Local` v0.9.0
**Amri ya kusakinisha:** `dotnet add package Microsoft.AI.Foundry.Local`

Kifurushi cha NuGet kinatoa mfumo mmoja tu wa lengo:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

Hakuna TFM ya `net9.0` au `net10.0` iliyojumuishwa. Kinyume chake, kifurushi cha mshirika `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) hutolewa na `net8.0`, `net9.0`, `net10.0`, `net472`, na `netstandard2.0`.

### Upimaji wa Ulinganifu

| Mfumo wa Lengo | Jenga | Endesha | Maelezo |
|-----------------|-------|---------|---------|
| net8.0 | ✅ | ✅ | Umeungwa rasmi mkono |
| net9.0 | ✅ | ✅ | Inajengwa kupitia forward-compat — inayotumika katika mifano ya warsha |
| net10.0 | ✅ | ✅ | Inajengwa na kuendesha kupitia forward-compat na runtime ya .NET 10.0.3 |

Kipande cha net8.0 huchukua vipengele vya runtime mpya kupitia mfumo wa forward-compat ya .NET, hivyo ujenzi unafanikiwa. Hata hivyo, hii haijandikwa wala kujaribiwa na timu ya SDK.

### Kwa nini Mifano Lengo net9.0

1. **.NET 9 ni toleo thabiti la hivi karibuni** — washiriki wengi wa warsha wanaitumia
2. **Forward compatibility inafanya kazi** — kipengele cha net8.0 cha kifurushi cha NuGet kinaendeshwa kwenye runtime ya .NET 9 bila matatizo
3. **.NET 10 (preview/RC)** ni mpya mno kwa lengo la warsha inayostahili kila mtu

**Imetarajiwa:** Matoleo yajayo ya SDK yanapaswa kuzingatia kuongeza TFMs `net9.0` na `net10.0` pamoja na `net8.0` kufuata mtindo wa `Microsoft.Agents.AI.OpenAI` na kutoa msaada uliothibitishwa kwa runtimes mpya.

---

## 6. Muunganisho wa ChatClient wa JavaScript Hutumia Callbacks, Siyo Async Iterators

**Hali:** Wazi
**Ukali:** Pengo katika nyaraka
**Sehemu:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` inayorejeshwa na `model.createChatClient()` hutoa njia ya `completeStreamingChat()`, lakini inatumia **mtindo wa callback** badala ya kurudisha itereta async:

```javascript
// ❌ Hii HAUFANYI kazi — hutoa "stream si ya async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Mchoro sahihi — pitia callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**Athari:** Waendelezaji wanaojua mtindo wa iteration async wa OpenAI SDK (`for await`) watapata makosa yanayochanganya. Callback lazima iwe kazi halali au SDK hutoroka "Callback must be a valid function."

**Imetarajiwa:** Andika mtindo wa callback katika rejeleo la SDK. Vinginevyo, saidia mtindo wa async iterable ili kuendana na OpenAI SDK.

---

## Maelezo ya Mazingira

| Sehemu | Toleo |
|-----------|---------|
| OS | Windows 11 ARM64 |
| Vifaa | Snapdragon X Elite (X1E78100) |
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
**Kifungu cha Kutolewa Dhima**:
Hati hii imetafsiriwa kwa kutumia huduma ya utafsiri wa AI [Co-op Translator](https://github.com/Azure/co-op-translator). Ingawa tunajitahidi kwa usahihi, tafadhali fahamu kwamba tafsiri zilizotautomatwa zinaweza kuwa na makosa au upungufu wa usahihi. Hati ya asili kwa lugha yake ya asili inapaswa kuzingatiwa kama chanzo cha mamlaka. Kwa taarifa muhimu, tafsiri ya kitaalamu iliyofanywa na binadamu inashauriwa. Hatuwajibiki kwa kutoelewana au tafsiri potofu zinazotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->