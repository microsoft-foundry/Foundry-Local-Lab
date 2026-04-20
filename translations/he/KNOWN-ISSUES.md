# בעיות ידועות — Foundry Local Workshop

בעיות שנתקלו בעת בנייה ובדיקת סדנה זו על מכשיר **Snapdragon X Elite (ARM64)** עם Windows, עם Foundry Local SDK v0.9.0, CLI v0.8.117 ו-.NET SDK 10.0.

> **אימות אחרון:** 2026-03-11

---

## 1. המעבד Snapdragon X Elite לא מזוהה על ידי ONNX Runtime

**מצב:** פתוח  
**חומרה:** אזהרה (לא חוסמת)  
**רכיב:** ONNX Runtime / cpuinfo  
**שחזור:** בכל הפעלה של שירות Foundry Local על חומרת Snapdragon X Elite  

בכל הפעלה של שירות Foundry Local מופיעות שתי אזהרות:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**השפעה:** האזהרות הן קוסמטיות — האינפרנס עובד כראוי. עם זאת, הן מופיעות בכל הרצה ועלולות לבלבל את המשתתפים בסדנה. יש לעדכן את ספריית ONNX Runtime cpuinfo כדי לזהות את ליבות המעבד Qualcomm Oryon.

**צפוי:** למערכת Snapdragon X Elite אמורה להיות זיהוי כרכיב ARM64 נתמך ללא הפקת הודעות שגיאה ברמת שגיאה.

---

## 2. SingleAgent NullReferenceException בהרצה הראשונה

**מצב:** פתוח (לא עקבי)  
**חומרה:** קריטי (קריסה)  
**רכיב:** Foundry Local C# SDK + מסגרת Microsoft Agent  
**שחזור:** הרץ `dotnet run agent` — קורסת מיד לאחר טעינת הדגם  

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**הקשר:** שורה 37 קוראת ל- `model.IsCachedAsync(default)`. הקריסה אירעה בהרצה הראשונה של הסוכן לאחר `foundry service stop` טרי. הרצות מאוחרות יותר עם אותו קוד הצליחו.

**השפעה:** לא עקבי — מצביע על מצב תחרות באתחול השירות של ה-SDK או בשאילתא בקטלוג. הפונקציה `GetModelAsync()` עשויה להחזיר תוצאה לפני שהשירות מוכן במלואו.

**צפוי:** `GetModelAsync()` צריכה לחסום עד שהשירות יהיה מוכן או להחזיר הודעת שגיאה ברורה אם השירות טרם סיים את האתחול.

---

## 3. C# SDK דורש RuntimeIdentifier מפורש

**מצב:** פתוח — מתועד ב-[microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**חומרה:** פער במסמכים  
**רכיב:** חבילת `Microsoft.AI.Foundry.Local` בטיפוס NuGet  
**שחזור:** יצירת פרויקט .NET 8+ ללא `<RuntimeIdentifier>` בקובץ `.csproj`  

הבנייה נכשלת עם:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**סיבה:** דרישת RID צפויה — ה-SDK שולח בינאריים נייטיביים (P/Invoke אל `Microsoft.AI.Foundry.Local.Core` ו-ONNX Runtime), ולכן .NET צריך לדעת איזו ספריית פלטפורמה ספציפית לטעון.

דבר זה מתועד ב-MS Learn ([כיצד להשתמש בהשלמות צ'אט נייטיביות](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), שם הוראות ההרצה מציגות:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
עם זאת, משתמשים צריכים לזכור את הדגל `-r` בכל פעם, מה שקל לשכוח.

**פתרון ביניים:** הוסף זיהוי אוטומטי בקובץ `.csproj` שלך כך ש-`dotnet run` יעבוד ללא דגלים:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` הוא מאפיין MSBuild מובנה שמחזיר את ה-RID של מכונת הפיתוח אוטומטית. פרויקטים לבחינה של ה-SDK משתמשים כבר בתבנית זו. דגלים מפורשים `-r` יכובדו בכל מקרה.

> **הערה:** קובץ הסדנה `.csproj` כולל פתרון זה כך ש-`dotnet run` יעבוד מיד על כל פלטפורמה.

**צפוי:** תבנית `.csproj` במדריכים של MS Learn תכלול את דפוס הזיהוי האוטומטי הזה כדי שמשתמשים לא יצטרכו לזכור את הדגל `-r`.

---

## 4. JavaScript Whisper — תמלול שמע מחזיר פלט ריק/בינארי

**מצב:** פתוח (רגרסיה — החמיר מאז הדיווח הראשוני)  
**חומרה:** משמעותי  
**רכיב:** מימוש JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**שחזור:** הרץ `node foundry-local-whisper.mjs` — כל קבצי השמע מחזירים פלט ריק או בינארי במקום תמלול טקסט  

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
בראשית רק קובץ השמע החמישי החזיר פלט ריק; החל מגרסת v0.9.x כל חמשת הקבצים מחזירים בת בודד (`\ufffd`) במקום טקסט מתומלל. מימוש Python Whisper המשתמש ב-OpenAI SDK מתמלל את אותם קבצים כראוי.

**צפוי:** `createAudioClient()` צריך להחזיר תמלול טקסט תואם למימושי Python ו-C#.

---

## 5. C# SDK שולח רק net8.0 — ללא מטרות רשמיות ל-.NET 9 או 10

**מצב:** פתוח  
**חומרה:** פער במסמכים  
**רכיב:** חבילת `Microsoft.AI.Foundry.Local` NuGet v0.9.0  
**פקודת התקנה:** `dotnet add package Microsoft.AI.Foundry.Local`

חבילת NuGet שולחת רק מסגרת מטרה אחת:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
אין `net9.0` או `net10.0` כלולים. לעומת זאת, חבילת הליווי `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) כוללת `net8.0`, `net9.0`, `net10.0`, `net472` ו-`netstandard2.0`.

### בדיקת תאימות

| מסגרת מטרה | בנייה | הרצה | הערות |
|------------|-------|-------|--------|
| net8.0     | ✅    | ✅    | נתמך רשמית |
| net9.0     | ✅    | ✅    | בנייה בגישה forward-compat — בשימוש בדוגמאות הסדנה |
| net10.0    | ✅    | ✅    | בנייה והרצה בגישה forward-compat עם runtime של .NET 10.0.3 |

ההרכבה של net8.0 טוענת על סביבות ריצה חדשות דרך מנגנון forward-compat של .NET, לכן הבנייה מצליחה. עם זאת, זו אינה מתועדת ולא נבדקה על ידי צוות ה-SDK.

### מדוע הדוגמאות מתמקדות ב-net9.0

1. **.NET 9 הוא הגרסה היציבה האחרונה** — רוב המשתתפים בסדנה יתקינו אותה  
2. **forward compatibility עובד** — הרכבת net8.0 תחת החבילה עובדת על runtime של .NET 9 ללא בעיות  
3. **.NET 10 (גרסת תצוגה/RC)** חדש מדי לשימוש בסדנה שצריכה לעבוד לכולם

**צפוי:** גרסאות ה-SDK העתידיות ישקלו הוספת TFMs ל-`net9.0` ו-`net10.0` לצד `net8.0` כדי להתאים לתבנית המשמשת את `Microsoft.Agents.AI.OpenAI` ולספק תמיכה מאומתת לסביבות ריצה חדשות יותר.

---

## 6. JavaScript ChatClient Streaming משתמש בקריאות חוזרות (Callbacks), לא באיטרטורים אסינכרוניים

**מצב:** פתוח  
**חומרה:** פער במסמכים  
**רכיב:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

ה-`ChatClient` המוחזר מ-`model.createChatClient()` מספק את השיטה `completeStreamingChat()`, אך היא משתמשת בדפוס **קריאות חוזרות (callback)** במקום להחזיר איטרטור אסינכרוני:

```javascript
// ❌ זה לא עובד — זורק "הזרם אינו איטרטיבי אסינכרוני"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ דפוס נכון — העבר פונקציית קריאה חוזרת
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**השפעה:** מפתחים שמכירים את דפוס האיטרציה האסינכרונית של OpenAI SDK (`for await`) ייתקלו בשגיאות מבלבלות. הקריאה החוזרת חייבת להיות פונקציה תקפה אחרת ה-SDK זורק שגיאה "Callback must be a valid function."

**צפוי:** לתעד את דפוס הקריאות החוזרות במדריך ה-SDK. לחלופין, לתמוך בדפוס האיטרטור האסינכרוני כדי לשמור על עקביות עם OpenAI SDK.

---

## פרטי סביבה

| רכיב | גרסה |
|-------|--------|
| OS | Windows 11 ARM64 |
| חומרה | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |