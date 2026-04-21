# Známé problémy — Foundry Local Workshop

Problémy zaznamenané při sestavování a testování tohoto workshopu na zařízení **Snapdragon X Elite (ARM64)** s Windows, s Foundry Local SDK v0.9.0, CLI v0.8.117 a .NET SDK 10.0.

> **Poslední ověření:** 11. 3. 2026

---

## 1. Procesor Snapdragon X Elite není rozpoznán ONNX Runtime

**Stav:** Otevřený  
**Závažnost:** Varování (neblokující)  
**Komponenta:** ONNX Runtime / cpuinfo  
**Reprodukce:** Každé spuštění služby Foundry Local na hardwaru Snapdragon X Elite

Při každém spuštění služby Foundry Local se zobrazí dvě varování:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Dopad:** Varování jsou pouze kosmetická — inferencing funguje správně. Nicméně se objevují při každém spuštění a mohou zmást účastníky workshopu. Knihovna ONNX Runtime cpuinfo potřebuje aktualizaci, aby rozpoznávala jádra procesorů Qualcomm Oryon.

**Očekávané:** Snapdragon X Elite by měl být rozpoznán jako podporovaný ARM64 procesor bez zobrazování chybových zpráv.

---

## 2. NullReferenceException v SingleAgent při prvním spuštění

**Stav:** Otevřený (občasný)  
**Závažnost:** Kritický (pád)  
**Komponenta:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reprodukce:** Spustit `dotnet run agent` — pád ihned po načtení modelu

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Kontext:** Řádek 37 volá `model.IsCachedAsync(default)`. Pád nastal při prvním spuštění agenta po čerstvém `foundry service stop`. Následující spuštění se stejným kódem uspěla.

**Dopad:** Občasný problém — naznačuje závodní podmínku v inicializaci služby SDK nebo dotazu na katalog. Volání `GetModelAsync()` může vrátit dříve, než je služba plně připravena.

**Očekávané:** `GetModelAsync()` by mělo buď blokovat, dokud je služba připravena, nebo vrátit jasnou chybovou zprávu, pokud inicializace služby ještě neskončila.

---

## 3. C# SDK vyžaduje explicitní RuntimeIdentifier

**Stav:** Otevřený — sledováno v [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Závažnost:** Dokumentační mezera  
**Komponenta:** NuGet balík `Microsoft.AI.Foundry.Local`  
**Reprodukce:** Vytvořit projekt .NET 8+ bez `<RuntimeIdentifier>` v `.csproj`

Build selže s:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Hlavní příčina:** Požadavek na RID je očekávaný — SDK dodává nativní binárky (P/Invoke do `Microsoft.AI.Foundry.Local.Core` a ONNX Runtime), takže .NET musí vědět, kterou platformně specifickou knihovnu má načíst.

To je zdokumentováno na MS Learn ([Jak používat nativní chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), kde běhové instrukce ukazují:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Uživatelé ale musí pamatovat na parametr `-r` při každém spuštění, což se snadno zapomene.

**Řešení:** Přidat do `.csproj` automatický fallback, aby `dotnet run` fungovalo bez parametrů:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` je vestavěná vlastnost MSBuild, která automaticky určuje RID hostitelského stroje. Testovací projekty SDK již tento vzor používají. Explicitní `-r` parametry jsou při zadání stále respektovány.

> **Poznámka:** Workshopový `.csproj` tento fallback obsahuje, takže `dotnet run` funguje ihned na jakékoli platformě.

**Očekávané:** Šablona `.csproj` v dokumentaci MS Learn by měla obsahovat tento automatický detekční vzor, aby uživatelé nemuseli pamatovat na parametr `-r`.

---

## 4. JavaScript Whisper — Přepis audia vrací prázdný nebo binární výstup

**Stav:** Otevřený (regrese — zhoršeno od původní zprávy)  
**Závažnost:** Významný  
**Komponenta:** Implementace JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reprodukce:** Spustit `node foundry-local-whisper.mjs` — všechny audio soubory vrací prázdný nebo binární výstup místo textového přepisu

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Původně pouze 5. audio soubor vracel prázdný výstup; od verze v0.9.x všechny 5 souborů vrací jeden byte (`\ufffd`) místo přepsaného textu. Python implementace Whisper pomocí OpenAI SDK správně přepisuje stejné soubory.

**Očekávané:** `createAudioClient()` by mělo vracet přepis textu v souladu s Python/C# implementacemi.

---

## 5. C# SDK dodává pouze net8.0 — oficiálně nepodporuje .NET 9 ani .NET 10

**Stav:** Otevřený  
**Závažnost:** Dokumentační mezera  
**Komponenta:** NuGet balík `Microsoft.AI.Foundry.Local` v0.9.0  
**Instalační příkaz:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet balík obsahuje pouze jedno cílové framework:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Neobsahuje `net9.0` ani `net10.0`. Naproti tomu doprovodný balík `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) obsahuje `net8.0`, `net9.0`, `net10.0`, `net472` a `netstandard2.0`.

### Testování kompatibility

| Cílový framework | Build | Spuštění | Poznámky |
|------------------|-------|----------|----------|
| net8.0           | ✅    | ✅       | Oficiálně podporováno |
| net9.0           | ✅    | ✅       | Kompilace pomocí forward-compat — použití ve workshopových ukázkách |
| net10.0          | ✅    | ✅       | Kompilace a spuštění pomocí forward-compat s .NET 10.0.3 runtime |

Sestava net8.0 se načítá na novějších runtimech díky forward-compat mechanismu .NET, takže build projde. Nicméně SDK tým toto nemá zdokumentováno ani otestováno.

### Proč jsou ukázky zaměřené na net9.0

1. **.NET 9 je nejnovější stabilní verze** — většina účastníků workshopu ji bude mít nainstalovanou  
2. **Forward kompatibilita funguje** — sestava net8.0 v NuGet balíku běží bez problémů na runtime .NET 9  
3. **.NET 10 (preview/RC)** je příliš nová verze na cílení v workshopu, který má fungovat pro všechny

**Očekávané:** Budoucí verze SDK by měly zvážit přidání `net9.0` a `net10.0` TFMs vedle `net8.0`, aby odpovídaly vzoru používanému `Microsoft.Agents.AI.OpenAI` a poskytly ověřenou podporu pro novější runtime.

---

## 6. JavaScript ChatClient Streaming používá callbacky, ne async iterátory

**Stav:** Otevřený  
**Závažnost:** Dokumentační mezera  
**Komponenta:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` vrácený `model.createChatClient()` poskytuje metodu `completeStreamingChat()`, ale používá **callback pattern**, nikoli async iterable:

```javascript
// ❌ Toto NEFUNGUJE — vyhazuje "stream není asynchronně iterovatelný"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Správný vzor — předat zpětné volání
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Dopad:** Vývojáři zvyklí na OpenAI SDK async iteraci (`for await`) narazí na matoucí chyby. Callback musí být platná funkce, jinak SDK vyhodí chybu "Callback must be a valid function."

**Očekávané:** Dokumentovat callback pattern v SDK referenci. Alternativně podpořit async iterable pattern pro konzistenci s OpenAI SDK.

---

## Podrobnosti o prostředí

| Komponenta | Verze |
|------------|--------|
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