# Problemi Conosciuti — Foundry Local Workshop

Problemi riscontrati durante la creazione e il test di questo workshop su un dispositivo **Snapdragon X Elite (ARM64)** con Windows, utilizzando Foundry Local SDK v0.9.0, CLI v0.8.117 e .NET SDK 10.0.

> **Ultima convalida:** 2026-03-11

---

## 1. CPU Snapdragon X Elite Non Riconosciuta da ONNX Runtime

**Stato:** Aperto  
**Gravità:** Avviso (non bloccante)  
**Componente:** ONNX Runtime / cpuinfo  
**Riproduzione:** Ogni avvio del servizio Foundry Local su hardware Snapdragon X Elite

Ogni volta che il servizio Foundry Local viene avviato, vengono emessi due avvisi:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Impatto:** Gli avvisi sono cosmetici — l'inferenza funziona correttamente. Tuttavia, compaiono ad ogni esecuzione e possono confondere i partecipanti al workshop. La libreria ONNX Runtime cpuinfo necessita di un aggiornamento per riconoscere i core CPU Qualcomm Oryon.

**Aspettativa:** Snapdragon X Elite dovrebbe essere riconosciuto come CPU ARM64 supportata senza emettere messaggi di errore a livello di errore.

---

## 2. SingleAgent NullReferenceException al Primo Avvio

**Stato:** Aperto (intermittente)  
**Gravità:** Critico (crash)  
**Componente:** Foundry Local C# SDK + Microsoft Agent Framework  
**Riproduzione:** Eseguire `dotnet run agent` — crash immediato dopo il caricamento del modello

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Contesto:** La riga 37 chiama `model.IsCachedAsync(default)`. Il crash si è verificato al primo avvio dell'agente dopo un `foundry service stop` recente. Le esecuzioni successive con lo stesso codice sono riuscite.

**Impatto:** Intermittente — suggerisce una condizione di race nell'inizializzazione del servizio SDK o nella query del catalogo. La chiamata `GetModelAsync()` potrebbe restituire prima che il servizio sia completamente pronto.

**Aspettativa:** `GetModelAsync()` dovrebbe bloccare l'esecuzione fino a quando il servizio non è pronto o restituire un messaggio di errore chiaro se il servizio non ha completato l'inizializzazione.

---

## 3. C# SDK Richiede esplicitamente RuntimeIdentifier

**Stato:** Aperto — tracciato in [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Gravità:** Mancanza nella documentazione  
**Componente:** Pacchetto NuGet `Microsoft.AI.Foundry.Local`  
**Riproduzione:** Creare un progetto .NET 8+ senza `<RuntimeIdentifier>` nel `.csproj`

La build fallisce con:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Causa principale:** La necessità del RID è prevedibile — l'SDK include binari nativi (P/Invoke in `Microsoft.AI.Foundry.Local.Core` e ONNX Runtime), quindi .NET deve sapere quale libreria specifica della piattaforma risolvere.

Questo è documentato su MS Learn ([Come usare le chat native](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), dove nelle istruzioni di esecuzione è mostrato:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Tuttavia, gli utenti devono ricordarsi il flag `-r` ogni volta, cosa facile da dimenticare.

**Soluzione temporanea:** Aggiungere un fallback di auto-rilevamento al vostro `.csproj` in modo che `dotnet run` funzioni senza flag:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` è una proprietà MSBuild incorporata che risolve automaticamente il RID della macchina host. I progetti di test dell'SDK stessa usano già questo pattern. I flag espliciti `-r` sono ancora rispettati se forniti.

> **Nota:** Il `.csproj` del workshop include questo fallback per far funzionare `dotnet run` immediatamente su qualsiasi piattaforma.

**Aspettativa:** Il template `.csproj` nei documenti MS Learn dovrebbe includere questo pattern di auto-rilevamento, così gli utenti non devono ricordarsi il flag `-r`.

---

## 4. JavaScript Whisper — La Trascrizione Audio Ritorna Output Vuoto/Binario

**Stato:** Aperto (regressione — peggiorato rispetto al primo report)  
**Gravità:** Grave  
**Componente:** Implementazione JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Riproduzione:** Eseguire `node foundry-local-whisper.mjs` — tutti i file audio restituiscono output vuoto o binario invece della trascrizione testuale

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
In origine solo il quinto file audio restituiva vuoto; dalla versione v0.9.x, tutti e 5 i file restituiscono un singolo byte (`\ufffd`) invece del testo trascritto. L'implementazione Python Whisper usando l'SDK OpenAI trascrive correttamente gli stessi file.

**Aspettativa:** `createAudioClient()` dovrebbe restituire la trascrizione testuale corrispondente alle implementazioni Python/C#.

---

## 5. C# SDK Distribuisce Solo net8.0 — Nessun Target Ufficiale .NET 9 o .NET 10

**Stato:** Aperto  
**Gravità:** Mancanza nella documentazione  
**Componente:** Pacchetto NuGet `Microsoft.AI.Foundry.Local` v0.9.0  
**Comando di installazione:** `dotnet add package Microsoft.AI.Foundry.Local`

Il pacchetto NuGet include solo un framework di destinazione:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Non è incluso alcun TFM `net9.0` o `net10.0`. In confronto, il pacchetto complementare `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) fornisce `net8.0`, `net9.0`, `net10.0`, `net472` e `netstandard2.0`.

### Test di Compatibilità

| Framework di Destinazione | Build | Esecuzione | Note |
|--------------------------|-------|------------|-------|
| net8.0 | ✅ | ✅ | Supportato ufficialmente |
| net9.0 | ✅ | ✅ | Build via forward-compat — usato nei sample del workshop |
| net10.0 | ✅ | ✅ | Build ed eseguito via forward-compat con runtime .NET 10.0.3 |

L'assembly net8.0 viene caricato su runtime più recenti tramite il meccanismo di forward-compat di .NET, quindi la build riesce. Tuttavia, ciò è non documentato e non testato dal team SDK.

### Perché i Sample Targetizzano net9.0

1. **.NET 9 è l'ultima versione stabile** — la maggior parte dei partecipanti al workshop la avrà installata  
2. **La forward compatibility funziona** — l'assembly net8.0 nel pacchetto NuGet funziona senza problemi sul runtime .NET 9  
3. **.NET 10 (preview/RC)** è troppo recente per target in un workshop che dovrebbe funzionare per tutti

**Aspettativa:** Le future release dell'SDK dovrebbero considerare di aggiungere i TFM `net9.0` e `net10.0` insieme a `net8.0` per conformarsi al modello usato da `Microsoft.Agents.AI.OpenAI` e fornire supporto validato per runtime più recenti.

---

## 6. JavaScript ChatClient Streaming Usa Callback, Non Iteratori Async

**Stato:** Aperto  
**Gravità:** Mancanza nella documentazione  
**Componente:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

Il `ChatClient` ritornato da `model.createChatClient()` fornisce un metodo `completeStreamingChat()`, ma usa un **pattern callback** invece di restituire un iteratore async:

```javascript
// ❌ Questo NON funziona — genera l'errore "lo stream non è iterabile in modo asincrono"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Schema corretto — passa una funzione di callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Impatto:** Gli sviluppatori abituati al pattern di iterazione async dell'SDK OpenAI (`for await`) incontreranno errori confusi. La callback deve essere una funzione valida, altrimenti l'SDK genera "Callback must be a valid function."

**Aspettativa:** Documentare il pattern callback nella documentazione dell’SDK. In alternativa, supportare il pattern di iterazione async per uniformità con l’SDK OpenAI.

---

## Dettagli dell’Ambiente

| Componente | Versione |
|-----------|---------|
| OS | Windows 11 ARM64 |
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

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:  
Questo documento è stato tradotto utilizzando il servizio di traduzione AI [Co-op Translator](https://github.com/Azure/co-op-translator). Pur facendo il possibile per garantire la precisione, si prega di notare che le traduzioni automatiche possono contenere errori o imprecisioni. Il documento originale nella sua lingua nativa deve essere considerato la fonte autorevole. Per informazioni critiche, si raccomanda una traduzione professionale effettuata da un umano. Non siamo responsabili per eventuali malintesi o interpretazioni errate derivanti dall'uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->