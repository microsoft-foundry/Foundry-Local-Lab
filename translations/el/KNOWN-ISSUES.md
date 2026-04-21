# Γνωστά Προβλήματα — Foundry Local Workshop

Προβλήματα που προέκυψαν κατά τη δημιουργία και δοκιμή αυτού του εργαστηρίου σε μια συσκευή **Snapdragon X Elite (ARM64)** που τρέχει Windows, με Foundry Local SDK v0.9.0, CLI v0.8.117 και .NET SDK 10.0.

> **Τελευταία επικύρωση:** 2026-03-11

---

## 1. Η CPU Snapdragon X Elite Δεν Αναγνωρίζεται από το ONNX Runtime

**Κατάσταση:** Ανοιχτό  
**Σοβαρότητα:** Προειδοποίηση (μη μπλοκάρισμα)  
**Συστατικό:** ONNX Runtime / cpuinfo  
**Αναπαραγωγή:** Κάθε εκκίνηση υπηρεσίας Foundry Local σε υλικό Snapdragon X Elite  

Κάθε φορά που ξεκινά η υπηρεσία Foundry Local, εκπέμπονται δύο προειδοποιήσεις:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Επίπτωση:** Οι προειδοποιήσεις είναι μόνο για αισθητικό λόγο — η συμπερασματολογία λειτουργεί σωστά. Ωστόσο, εμφανίζονται σε κάθε εκτέλεση και μπορούν να μπερδέψουν τους συμμετέχοντες του εργαστηρίου. Η βιβλιοθήκη cpuinfo του ONNX Runtime χρειάζεται ενημέρωση για να αναγνωρίζει τους πυρήνες CPU Qualcomm Oryon.

**Αναμενόμενο:** Η Snapdragon X Elite θα πρέπει να αναγνωρίζεται ως υποστηριζόμενη CPU ARM64 χωρίς να εκπέμπονται μηνύματα σφάλματος επιπέδου error.

---

## 2. SingleAgent NullReferenceException στην Πρώτη Εκτέλεση

**Κατάσταση:** Ανοιχτό (διακοπτόμενο)  
**Σοβαρότητα:** Κρίσιμο (κατάρρευση)  
**Συστατικό:** Foundry Local C# SDK + Microsoft Agent Framework  
**Αναπαραγωγή:** Εκτέλεση `dotnet run agent` — καταρρέει αμέσως μετά τη φόρτωση του μοντέλου  

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Πλαίσιο:** Η γραμμή 37 καλεί `model.IsCachedAsync(default)`. Η κατάρρευση συνέβη στην πρώτη εκτέλεση του agent μετά από μια νέα `foundry service stop`. Οι επόμενες εκτελέσεις με τον ίδιο κώδικα πέτυχαν.

**Επίπτωση:** Διακοπτόμενο — υποδεικνύει κατάσταση race condition στην αρχικοποίηση της υπηρεσίας του SDK ή στο ερώτημα καταλόγου. Η κλήση `GetModelAsync()` μπορεί να επιστρέφει πριν η υπηρεσία είναι πλήρως έτοιμη.

**Αναμενόμενο:** Η `GetModelAsync()` είτε πρέπει να μπλοκάρει μέχρι να είναι έτοιμη η υπηρεσία, είτε να επιστρέφει ένα σαφές μήνυμα σφάλματος αν η υπηρεσία δεν έχει ολοκληρώσει την αρχικοποίηση.

---

## 3. Το C# SDK Απαιτεί Ρητό RuntimeIdentifier

**Κατάσταση:** Ανοιχτό — παρακολουθείται στο [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Σοβαρότητα:** Κενό τεκμηρίωσης  
**Συστατικό:** Πακέτο NuGet `Microsoft.AI.Foundry.Local`  
**Αναπαραγωγή:** Δημιουργία έργου .NET 8+ χωρίς `<RuntimeIdentifier>` στο `.csproj`  

Η κατασκευή αποτυγχάνει με:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Βασική αιτία:** Η απαίτηση του RID είναι αναμενόμενη — το SDK περιέχει εγγενείς δυαδικούς (P/Invoke στο `Microsoft.AI.Foundry.Local.Core` και ONNX Runtime), οπότε το .NET χρειάζεται να ξέρει ποια πλατφόρμα να επιλύσει.

Αυτό τεκμηριώνεται στο MS Learn ([Πώς να χρησιμοποιήσετε εγγενείς ολοκληρώσεις συνομιλίας](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), όπου οι οδηγίες εκτέλεσης δείχνουν:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Ωστόσο, οι χρήστες πρέπει να θυμούνται το flag `-r` κάθε φορά, κάτι που είναι εύκολο να ξεχαστεί.

**Παράκαμψη:** Προσθέστε μια αυτόματη ανίχνευση fallback στο `.csproj` ώστε το `dotnet run` να λειτουργεί χωρίς flags:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
Το `$(NETCoreSdkRuntimeIdentifier)` είναι μια ενσωματωμένη ιδιότητα MSBuild που επιλύει αυτόματα το RID της μηχανής υποδοχής. Τα ίδια τα test projects του SDK ήδη χρησιμοποιούν αυτό το μοτίβο. Τα ρητά `-r` flags παραμένουν σεβαστά όταν παρέχονται.

> **Σημείωση:** Το `.csproj` του εργαστηρίου περιλαμβάνει αυτή τη fallback, ώστε το `dotnet run` να λειτουργεί αμέσως σε οποιαδήποτε πλατφόρμα.

**Αναμενόμενο:** Το πρότυπο `.csproj` στα έγγραφα MS Learn θα πρέπει να περιλαμβάνει αυτό το μοτίβο αυτόματης ανίχνευσης ώστε οι χρήστες να μην χρειάζεται να θυμούνται το flag `-r`.

---

## 4. JavaScript Whisper — Η Μεταγραφή Ήχου Επιστρέφει Κενή/Δυαδική Έξοδο

**Κατάσταση:** Ανοιχτό (υποχώρηση — επιδεινώθηκε από την αρχική αναφορά)  
**Σοβαρότητα:** Μεγάλη  
**Συστατικό:** Υλοποίηση JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Αναπαραγωγή:** Εκτέλεση `node foundry-local-whisper.mjs` — όλα τα αρχεία ήχου επιστρέφουν κενό ή δυαδική έξοδο αντί για κείμενο μεταγραφής  

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Αρχικά μόνο το 5ο αρχείο ήχου επέστρεφε κενό· από την έκδοση v0.9.x, όλα τα 5 αρχεία επιστρέφουν ένα μόνο byte (`\ufffd`) αντί για μεταγραμμένο κείμενο. Η υλοποίηση Whisper σε Python με το OpenAI SDK μεταγράφει σωστά τα ίδια αρχεία.

**Αναμενόμενο:** Η `createAudioClient()` θα πρέπει να επιστρέφει μεταγραφή κειμένου που να ταιριάζει με τις υλοποιήσεις Python/C#.

---

## 5. Το C# SDK Διανέμει Μόνο net8.0 — Δεν Υπάρχει Επίσημος Στόχος .NET 9 ή .NET 10

**Κατάσταση:** Ανοιχτό  
**Σοβαρότητα:** Κενό τεκμηρίωσης  
**Συστατικό:** Πακέτο NuGet `Microsoft.AI.Foundry.Local` v0.9.0  
**Εντολή εγκατάστασης:** `dotnet add package Microsoft.AI.Foundry.Local`  

Το πακέτο NuGet διαθέτει μόνο ένα στόχο framework:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Δεν περιλαμβάνεται TFM `net9.0` ή `net10.0`. Αντίθετα, το συνοδευτικό πακέτο `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) παρέχει `net8.0`, `net9.0`, `net10.0`, `net472` και `netstandard2.0`.

### Δοκιμές Συμβατότητας

| Στόχος Framework | Κατασκευή | Εκτέλεση | Σημειώσεις |
|-----------------|-----------|----------|------------|
| net8.0 | ✅ | ✅ | Επίσημα υποστηριζόμενο |
| net9.0 | ✅ | ✅ | Κατασκευάζεται μέσω προοδευτικής συμβατότητας — χρησιμοποιείται στα δείγματα εργαστηρίου |
| net10.0 | ✅ | ✅ | Κατασκευάζεται και εκτελείται μέσω προοδευτικής συμβατότητας με runtime .NET 10.0.3 |

Το συγκρότημα net8.0 φορτώνεται σε νεότερα runtimes μέσω του μηχανισμού προοδευτικής συμβατότητας .NET, ώστε η κατασκευή να πετυχαίνει. Ωστόσο, αυτό δεν είναι τεκμηριωμένο και δεν έχει δοκιμαστεί από την ομάδα του SDK.

### Γιατί τα δείγματα στοχεύουν net9.0

1. **Το .NET 9 είναι η τελευταία σταθερή έκδοση** — οι περισσότεροι συμμετέχοντες στο εργαστήριο θα το έχουν εγκατεστημένο  
2. **Λειτουργεί η προοδευτική συμβατότητα** — το συγκρότημα net8.0 στο πακέτο NuGet τρέχει στο runtime του .NET 9 χωρίς προβλήματα  
3. **Το .NET 10 (preview/RC)** είναι πολύ νέο για να χρησιμοποιηθεί σε εργαστήριο που πρέπει να λειτουργεί για όλους  

**Αναμενόμενο:** Μελλοντικές εκδόσεις του SDK θα πρέπει να εξετάσουν την προσθήκη TFM `net9.0` και `net10.0` παράλληλα με το `net8.0` ώστε να ταιριάζουν με το μοτίβο του `Microsoft.Agents.AI.OpenAI` και να παρέχουν πιστοποιημένη υποστήριξη για νεότερα runtimes.

---

## 6. Το JavaScript ChatClient Streaming Χρησιμοποιεί Callbacks, Όχι Async Iterators

**Κατάσταση:** Ανοιχτό  
**Σοβαρότητα:** Κενό τεκμηρίωσης  
**Συστατικό:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`  

Το `ChatClient` που επιστρέφεται από `model.createChatClient()` παρέχει τη μέθοδο `completeStreamingChat()`, αλλά αυτή χρησιμοποιεί **pattern callback** αντί να επιστρέφει async iterable:

```javascript
// ❌ Αυτό ΔΕΝ λειτουργεί — προκαλεί σφάλμα "stream δεν είναι ασύγχρονα επαναλήψιμο"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Σωστό μοτίβο — δώστε μια συνάρτηση ανάκλησης
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Επίπτωση:** Οι προγραμματιστές εξοικειωμένοι με το async iteration pattern του OpenAI SDK (`for await`) θα αντιμετωπίσουν μπερδεμένα σφάλματα. Το callback πρέπει να είναι έγκυρη συνάρτηση αλλιώς το SDK πετάει "Callback must be a valid function."

**Αναμενόμενο:** Να τεκμηριωθεί το callback pattern στο αναλυτικό εγχειρίδιο SDK. Εναλλακτικά, να υποστηριχθεί και το async iterable pattern για συνέπεια με το OpenAI SDK.

---

## Λεπτομέρειες Περιβάλλοντος

| Συστατικό | Έκδοση |
|-----------|--------|
| OS | Windows 11 ARM64 |
| Υλικό | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |