# SearchEngine

install the nugets below:

dotnet add package HtmlAgilityPack --version 1.11.71
dotnet add package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation --version 8.0.11
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0


create local db according to appSettings.dev.json:
    "DefaultConnection": "Server=(localDb)\\sqlexpress1;


build the project, 
open the package manager console
and run the commands below to create db:

enable-migrations
update-database (there's migration already)

run the project.


הערות:

דרישות שבוצעו:
1. צד לקוח באמצעות .NET Mvc
2. ביצוע WEB API להחזרת מידע לקליינט מהשרת (באמצעות AJAX)
3. יצירת דף חיפוש עם תיבת טקסט וכפתור חיפוש
4. שמירת כותרות תוצאות בDB באמצעות EF
5. יישום קאש בשרת
6. יישום מערכת שגיאות בסיסית
7. קליינט - עיצוב רספונסיבי
8. הצגת CARD או LIST לפי בחירה של המשתמש
9. סינון תוצאות לפי מנוע חיפוש

בעיות שנתקלתי:
1. הצגת 5 תוצאות בדיוק עבור כל מנוע חיפוש,
צריך לבצע מחקר מעמיק על מנוע החיפוש כיוון שהתוצאות שהמשתמש יקבל מכל מנוע הן שונות
וגם השפות שונות, ויש הרבה משתשנים שצריך להציב בURL כדי לקבל מידע מדוייק יותר.

הנחות יסוד על מנת לבצע את המשימה: (כיוון שהמשימה מתבצעת ללא הכוונה)
1. יש לקבל את התוצאות כמו שהן מהמנוע חיפוש ולעבוד מולן כיוון שמדובר בAPI לא רשמי
והחילוץ עצמו נעשה דרך אלמנטים בHTML


