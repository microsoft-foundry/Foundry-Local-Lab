# Znane problemy — Foundry Local Workshop

Problemy napotkane podczas tworzenia i testowania tego warsztatu na urządzeniu **Snapdragon X Elite (ARM64)** z systemem Windows, z Foundry Local SDK w wersji 0.9.0, CLI 0.8.117 oraz .NET SDK 10.0.

> **Ostatnia weryfikacja:** 2026-03-11

---

## 1. Procesor Snapdragon X Elite Nie Rozpoznawany przez ONNX Runtime

**Status:** Otwarte  
**Powaga:** Ostrzeżenie (nieblokujące)  
**Komponent:** ONNX Runtime / cpuinfo  
**Reprodukcja:** Każde uruchomienie usługi Foundry Local na sprzęcie Snapdragon X Elite

Za każdym razem, gdy usługa Foundry Local się uruchamia, pojawiają się dwa ostrzeżenia:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Wpływ:** Ostrzeżenia mają charakter kosmetyczny — inferencja działa poprawnie. Jednak pojawiają się przy każdym uruchomieniu i mogą wprowadzać uczestników warsztatu w błąd. Biblioteka cpuinfo dla ONNX Runtime wymaga aktualizacji, aby rozpoznawać rdzenie procesora Qualcomm Oryon.

**Oczekiwane:** Snapdragon X Elite powinien być rozpoznawany jako obsługiwany procesor ARM64 bez wyświetlania komunikatów o błędach.

---

## 2. NullReferenceException w SingleAgent przy pierwszym uruchomieniu

**Status:** Otwarte (sporadyczne)  
**Powaga:** Krytyczne (awaria)  
**Komponent:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reprodukcja:** Uruchomienie `dotnet run agent` — awaria natychmiast po załadowaniu modelu

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Kontekst:** W linii 37 wywoływana jest metoda `model.IsCachedAsync(default)`. Awaria miała miejsce przy pierwszym uruchomieniu agenta po świeżym `foundry service stop`. Kolejne uruchomienia z tym samym kodem przebiegały pomyślnie.

**Wpływ:** Sporadyczne — sugeruje wyścig w inicjalizacji usługi SDK lub zapytaniu katalogu. Wywołanie `GetModelAsync()` może zwrócić wynik przed pełnym gotowością usługi.

**Oczekiwane:** `GetModelAsync()` powinno albo blokować wywołanie do momentu gotowości usługi, albo zwracać czytelny komunikat o błędzie, jeśli inicjalizacja usługi nie została zakończona.

---

## 3. C# SDK Wymaga Jawnego RuntimeIdentifier

**Status:** Otwarte — zgłoszone w [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Powaga:** Luka w dokumentacji  
**Komponent:** Pakiet NuGet `Microsoft.AI.Foundry.Local`  
**Reprodukcja:** Utworzenie projektu .NET 8+ bez `<RuntimeIdentifier>` w pliku `.csproj`

Budowanie kończy się błędem:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Przyczyna:** Wymaganie RID jest spodziewane — SDK zawiera binaria natywne (P/Invoke do `Microsoft.AI.Foundry.Local.Core` i ONNX Runtime), więc .NET musi znać, którą platformowo-specyficzną bibliotekę wybrać.

Jest to udokumentowane w MS Learn ([Jak używać natywnej funkcji chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), gdzie instrukcje uruchomienia pokazują:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Jednak użytkownicy muszą pamiętać o flagi `-r` za każdym razem, co jest łatwe do zapomnienia.

**Rozwiązanie tymczasowe:** Dodaj do `.csproj` automatyczną detekcję jako wyjście zapasowe, aby `dotnet run` działał bez flag:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` jest wbudowaną właściwością MSBuild, która automatycznie rozpoznaje RID maszyny hosta. Projekty testowe SDK już korzystają z tego wzorca. Wprowadzone jawne flagi `-r` wciąż są respektowane.

> **Uwaga:** Warsztatowy plik `.csproj` zawiera to wyjście zapasowe, aby `dotnet run` działał od razu na każdej platformie.

**Oczekiwane:** Szablon `.csproj` w dokumentacji MS Learn powinien zawierać ten wzorzec automatycznego wykrywania, aby użytkownicy nie musieli pamiętać flagi `-r`.

---

## 4. JavaScript Whisper — Transkrypcja Audio Zwraca Pusty lub Binarny Wynik

**Status:** Otwarte (regresja — pogorszenie od początkowego raportu)  
**Powaga:** Znaczące  
**Komponent:** Implementacja JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reprodukcja:** Uruchom `node foundry-local-whisper.mjs` — wszystkie pliki audio zwracają pusty lub binarny wynik zamiast transkrypcji tekstowej

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Początkowo tylko 5. plik audio zwracał pusty wynik; od wersji v0.9.x wszystkie 5 plików zwraca pojedynczy bajt (`\ufffd`) zamiast transkrybowanego tekstu. Implementacja Python Whisper korzystająca z OpenAI SDK poprawnie transkrybuje te same pliki.

**Oczekiwane:** `createAudioClient()` powinno zwracać tekstową transkrypcję zgodną z implementacjami Python i C#.

---

## 5. C# SDK Wysyła Tylko net8.0 — Brak Oficjalnych Targetów .NET 9 lub .NET 10

**Status:** Otwarte  
**Powaga:** Luka w dokumentacji  
**Komponent:** Pakiet NuGet `Microsoft.AI.Foundry.Local` w wersji 0.9.0  
**Polecenie instalacji:** `dotnet add package Microsoft.AI.Foundry.Local`

Pakiet NuGet zawiera tylko jeden target framework:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Brak `net9.0` lub `net10.0` w TFM. Dla porównania, pakiet towarzyszący `Microsoft.Agents.AI.OpenAI` (w wersji 1.0.0-rc3) zawiera `net8.0`, `net9.0`, `net10.0`, `net472` i `netstandard2.0`.

### Testy kompatybilności

| Target Framework | Kompilacja | Uruchomienie | Uwagi |
|-----------------|------------|--------------|--------|
| net8.0 | ✅ | ✅ | Oficjalnie wspierany |
| net9.0 | ✅ | ✅ | Kompiluje się dzięki forward-compat — używany w przykładach warsztatowych |
| net10.0 | ✅ | ✅ | Kompiluje i uruchamia dzięki forward-compat z runtime .NET 10.0.3 |

Biblioteka net8.0 ładuje się na nowszych runtime’ach dzięki mechanizmowi forward-compatibility w .NET, więc kompilacja się powodzi. Jednak jest to nieudokumentowane i nieprzetestowane przez zespół SDK.

### Dlaczego przykłady targetują net9.0

1. **.NET 9 to najnowsza stabilna wersja** — większość uczestników warsztatów ją posiada  
2. **Forward compatibility działa** — biblioteka net8.0 w pakiecie NuGet działa na runtime .NET 9 bez problemów  
3. **.NET 10 (preview/RC)** jest zbyt nowy, aby targetować go na warsztacie, który ma działać dla wszystkich

**Oczekiwane:** Przyszłe wydania SDK powinny rozważyć dodanie targetów `net9.0` i `net10.0` obok `net8.0`, zgodnie ze wzorcem używanym przez `Microsoft.Agents.AI.OpenAI`, aby zapewnić zweryfikowane wsparcie dla nowszych runtime’ów.

---

## 6. Strumieniowanie w JavaScript ChatClient Używa Callbacków, Nie Async Iteratorów

**Status:** Otwarte  
**Powaga:** Luka w dokumentacji  
**Komponent:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` zwracany przez `model.createChatClient()` udostępnia metodę `completeStreamingChat()`, ale używa **wzoru callbacków** zamiast zwracać async iterable:

```javascript
// ❌ To NIE działa — wyrzuca "stream nie jest asynchronicznie iterowalny"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Poprawny wzorzec — przekaż funkcję zwrotną
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Wpływ:** Programiści zaznajomieni z asynchronicznym wzorcem iteracji OpenAI SDK (`for await`) napotkają mylące błędy. Callback musi być prawidłową funkcją, w przeciwnym razie SDK wyrzuca "Callback must be a valid function."

**Oczekiwane:** Udokumentowanie wzorca callbacków w referencji SDK. Alternatywnie, wsparcie wzorca async iterable dla spójności z OpenAI SDK.

---

## Szczegóły środowiska

| Komponent | Wersja |
|-----------|---------|
| OS | Windows 11 ARM64 |
| Sprzęt | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |