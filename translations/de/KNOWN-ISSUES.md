# Bekannte Probleme — Foundry Local Workshop

Probleme beim Erstellen und Testen dieses Workshops auf einem **Snapdragon X Elite (ARM64)** Gerät unter Windows, mit Foundry Local SDK v0.9.0, CLI v0.8.117 und .NET SDK 10.0.

> **Zuletzt validiert:** 2026-03-11

---

## 1. Snapdragon X Elite CPU wird von ONNX Runtime nicht erkannt

**Status:** Offen  
**Schweregrad:** Warnung (nicht blockierend)  
**Komponente:** ONNX Runtime / cpuinfo  
**Reproduktion:** Jeder Start des Foundry Local Dienstes auf Snapdragon X Elite Hardware

Jedes Mal, wenn der Foundry Local Dienst gestartet wird, werden zwei Warnungen ausgegeben:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Auswirkung:** Die Warnungen sind kosmetisch — die Inferenz funktioniert korrekt. Sie erscheinen jedoch bei jedem Lauf und können Workshop-Teilnehmer verwirren. Die ONNX Runtime cpuinfo-Bibliothek muss aktualisiert werden, damit sie Qualcomm Oryon CPU-Kerne erkennt.

**Erwartet:** Snapdragon X Elite sollte als unterstützte ARM64 CPU erkannt werden, ohne Fehlermeldungen auf Fehler-Level auszugeben.

---

## 2. SingleAgent NullReferenceException beim ersten Lauf

**Status:** Offen (sporadisch)  
**Schweregrad:** Kritisch (Absturz)  
**Komponente:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reproduktion:** `dotnet run agent` ausführen — stürzt sofort nach Modellladen ab

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Kontext:** Zeile 37 ruft `model.IsCachedAsync(default)` auf. Der Absturz trat beim ersten Lauf des Agenten nach einem frischen `foundry service stop` auf. Folgeläufe mit dem gleichen Code funktionierten.

**Auswirkung:** Sporadisch — deutet auf einen Race-Condition im SDK bei der Dienstinitialisierung oder Katalogabfrage hin. Der `GetModelAsync()`-Aufruf kann zurückkehren, bevor der Dienst vollständig bereit ist.

**Erwartet:** `GetModelAsync()` sollte entweder blockieren, bis der Dienst bereit ist, oder eine klare Fehlermeldung zurückgeben, falls der Dienst noch nicht initialisiert ist.

---

## 3. C# SDK benötigt expliziten RuntimeIdentifier

**Status:** Offen — verfolgt in [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Schweregrad:** Dokumentationslücke  
**Komponente:** `Microsoft.AI.Foundry.Local` NuGet-Paket  
**Reproduktion:** Erstellen eines .NET 8+ Projekts ohne `<RuntimeIdentifier>` in der `.csproj`

Der Build schlägt fehl mit:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Ursache:** Die RID-Anforderung ist erwartet — das SDK enthält native Binärdateien (P/Invoke in `Microsoft.AI.Foundry.Local.Core` und ONNX Runtime), daher muss .NET wissen, welche plattformspezifische Bibliothek aufgelöst werden soll.

Dies ist in MS Learn dokumentiert ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), wo die Ausführungsanweisungen zeigen:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Benutzer müssen jedoch jedes Mal an das `-r` Flag denken, was leicht vergessen wird.

**Workaround:** Fügen Sie eine automatische Fallback-Erkennung zu Ihrer `.csproj` hinzu, sodass `dotnet run` ohne Flags funktioniert:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` ist eine eingebaute MSBuild-Eigenschaft, die automatisch auf die RID der Hostmaschine auflöst. Die Testprojekte des SDK verwenden dieses Muster bereits. Explizite `-r` Flags werden weiterhin berücksichtigt, wenn sie angegeben sind.

> **Hinweis:** Die Workshop-`.csproj` enthält diesen Fallback, so dass `dotnet run` auf jeder Plattform sofort funktioniert.

**Erwartet:** Die `.csproj`-Vorlage in der MS Learn-Dokumentation sollte dieses automatische Erkennungsmuster enthalten, damit Nutzer das `-r` Flag nicht jedes Mal angeben müssen.

---

## 4. JavaScript Whisper — Audio-Transkription liefert leere/ binäre Ausgabe

**Status:** Offen (Regression — hat sich seit Erstmeldung verschlechtert)  
**Schweregrad:** Wichtig  
**Komponente:** JavaScript Whisper-Implementierung (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reproduktion:** Führe `node foundry-local-whisper.mjs` aus — alle Audiodateien geben leere oder binäre Ausgaben anstatt Texttranskriptionen zurück

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Ursprünglich gab nur die 5. Audiodatei eine leere Ausgabe zurück; ab v0.9.x geben alle 5 Dateien ein einzelnes Byte (`\ufffd`) anstelle transkribierten Texts zurück. Die Python Whisper-Implementierung mit dem OpenAI SDK transkribiert dieselben Dateien korrekt.

**Erwartet:** `createAudioClient()` sollte eine Texttranskription zurückgeben, die mit den Python/C# Implementierungen übereinstimmt.

---

## 5. C# SDK liefert nur net8.0 — kein offizielles .NET 9 oder .NET 10 Target

**Status:** Offen  
**Schweregrad:** Dokumentationslücke  
**Komponente:** `Microsoft.AI.Foundry.Local` NuGet-Paket v0.9.0  
**Installationsbefehl:** `dotnet add package Microsoft.AI.Foundry.Local`

Das NuGet-Paket enthält nur ein Target Framework:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Kein `net9.0` oder `net10.0` TFM ist enthalten. Im Gegensatz dazu liefert das Begleitpaket `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472` und `netstandard2.0`.

### Kompatibilitätstests

| Ziel-Framework | Build | Lauf | Hinweise |
|----------------|-------|------|----------|
| net8.0         | ✅    | ✅   | Offiziell unterstützt |
| net9.0         | ✅    | ✅   | Build über Forward-Kompatibilität — in Workshop-Beispielen verwendet |
| net10.0        | ✅    | ✅   | Build und Lauf via Forward-Kompatibilität mit .NET 10.0.3 Runtime |

Die net8.0 Assembly lädt auf neueren Runtimes über das Forward-Kompatibilitäts-Mechanismus von .NET, sodass der Build erfolgreich ist. Dies ist jedoch undokumentiert und nicht vom SDK-Team getestet.

### Warum die Beispiele net9.0 verwenden

1. **.NET 9 ist die neueste stabile Version** — die meisten Workshop-Teilnehmer haben sie installiert  
2. **Forward-Kompatibilität funktioniert** — die net8.0 Assembly im NuGet-Paket läuft problemlos auf der .NET 9 Runtime  
3. **.NET 10 (Preview/RC)** ist zu neu, um in einem Workshop unterstützt zu werden, der für alle funktionieren soll

**Erwartet:** Zukünftige SDK-Versionen sollten in Erwägung ziehen, `net9.0` und `net10.0` TFMs zusammen mit `net8.0` anzubieten, wie es bei `Microsoft.Agents.AI.OpenAI` der Fall ist, um validierten Support für neuere Runtimes bereitzustellen.

---

## 6. JavaScript ChatClient Streaming nutzt Callbacks, keine Async Iteratoren

**Status:** Offen  
**Schweregrad:** Dokumentationslücke  
**Komponente:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

Der von `model.createChatClient()` zurückgegebene `ChatClient` bietet eine Methode `completeStreamingChat()`, die aber ein **Callback-Pattern** verwendet, anstatt eine async-iterable zurückzugeben:

```javascript
// ❌ Das funktioniert NICHT — wirft "Stream ist nicht asynchron iterierbar"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Richtiges Muster — eine Callback-Funktion übergeben
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Auswirkung:** Entwickler, die mit dem Async-Iteration-Pattern des OpenAI SDK (`for await`) vertraut sind, stoßen auf verwirrende Fehler. Das Callback muss eine gültige Funktion sein, sonst gibt das SDK den Fehler "Callback must be a valid function." aus.

**Erwartet:** Das Callback-Pattern sollte in der SDK-Referenz dokumentiert werden. Alternativ sollte das Async-Iterator-Pattern unterstützt werden, um Konsistenz mit dem OpenAI SDK zu gewährleisten.

---

## Umgebungsdetails

| Komponente | Version |
|------------|---------|
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