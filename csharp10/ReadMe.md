# Новые фичи C# 10

## File-scoped namespace declaration

В C# 9 появилась фича под названием Top Level Statement. И это позволяло в файле с точкой входа в приложение не писать бойлерплейт код

```c#
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```

Например, для приложения Hello World стало достаточным всего одной строчки кода , вместо десяти.

```c#
Console.WriteLine("Hello World!");
```



И это было круто, однако иметь такую возможность всего в одном файле не слишком полезно, особенно в большом проекте.

И в C# 10 пошли дальше, и первая фича, которую мы рассмотрим называется File-scoped namespace declaration.

Давайте посмотрим на примере. У нас есть проект, в котором есть два класса. Мы видим, что класс объявлен внутри неймспейса и находится внутри скобочек.

```c#
namespace _1._File_Scoped_Namespace
{
    public class Author
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public Author(string name, string lastname) =>
            (Name, LastName) = (name, lastname);
    }
}
```



File-scoped namespace declaration позволяет избавиться от них и сделать код ещё элегантнее. А после указания неймспейса ставится точка с запятой как после обычной инструкции.

```c#
namespace _1._File_Scoped_Namespace;
public class Author
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public Author(string name, string lastname) =>
        (Name, LastName) = (name, lastname);
}
```



 

## Extended property patterns

Следующая фича называется Extended property patterns, которая будет очень полезной для условий с вложенными свойствами.

Давайте посмотрим на это в действии. У нас есть класс Book. И мы хотим реализовать метод, который будет говорить имеется ли у книжки скидка.

```c#
    public static bool doesHaveDiscount(Book book)
    {
        return false;
    }
```



Условимся, что если фамилия автора книги соответствует определенному значению, то скидка есть, иначе нет. Напишем это условие.

```c#
public static bool doesHaveDiscount(Book book) =>
        book switch
        {
            { Author: { LastName: "Richter" } }
                or { Author: { LastName: "Price" } } => true,
            _ => false
        };
```

Мы видим вложенные свойства у объекта. Теперь мы можем упростить данную конструкцию, просто обратившись к свойству через точку.

```c#
public static bool doesHaveDiscount(Book book) =>
    book switch
    {
        { Author.LastName: "Richter" }
            or { Author.LastName: "Price" } => true,
        _ => false
    };
```

Это делает код более читаемым и чистым.



## Constant interpolated strings

Следующая фича называется Constant interpolated strings.

Интерполирование строк это очень клёво, потому что мы можем вставлять объект прямо в строку, не выходя за её пределы.

```c#
public string ThankYouMessage()
{
    return string.Empty;
}
```

Объявим две переменные, что в C# 9 было вполне допустимым. И для второй строки сделаем интерполирование.

```c#
var message = "Thank you for buying the book!";
var thankYouMessage = $"{message} Enjoy";
```

В C# 10 мы можем сделать эти строки константами, потому что вполне очевидно, что их значения не собираются изменяться.

```c#
public string ThankYouMessage()
{
    const string message = "Thank you for buying the book!";
    const string thankYouMessage = $"{message} Enjoy";
    return thankYouMessage;
}
```



## Records

Одной из новых фич, представленных в C# 9 были record'ы. В прошлом году мы подробно рассмотрели их и все другие нововведения 9-й версии в отдельном видео на нашем канале. Кратко говоря, добавление record классов принесло нам возможность делать классы иммутабельными, и вообще вести себя как структуры, например при сравнении двух классов.

C# 10 идет дальше и теперь мы можем добавлять record структуры. Давайте рассмотрим структуру, которая иммутабельна.

```csharp
public readonly record struct Point
{
    public int X {  get; }
    public int Y {  get; }
}
```



Теперь мы можем после имени структуры передать параметры.

```c#
public readonly record struct Point(int X, int Y);
```

и избавиться от всего остального кода.



Также в C# 10 мы можем добавить модификатор `sealed`, когда мы переопределяем метод `ToString()` в record типе. То есть запечатать. Запечатывание метода `ToString()` говорит компилятору - не надо создавать методы `ToString()` для всех отнаследованных record типов, используй вот этот. И компилятор так и делает.

```c#
record Circle
{
    public sealed override string ToString()
    {
        return typeof(Circle).Name;
    }
}
```



## Assignment and declaration in same deconstruction

Следующая фича относится к деконструкции.

Ранее можно было или присвоить сразу все значения во время деконструкции, или сначала их проинициализировать, а потом присвоить.

```c#
internal class Author
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public Author(string name, string lastname) =>
        (Name, LastName) = (name, lastname);

    public void Deconstruct(out string name, out string lastname) =>
        (name, lastname) = (Name, LastName);

    public override string ToString() =>
        $"{Name} {LastName}";
}

```



```csharp
var author = new Author("Andrei", "Fedotov");
Console.WriteLine(author);

(string name1, string lastName) = author;
Console.WriteLine(name1);
Console.WriteLine(lastName);

var name2 = string.Empty;
var lastname2 = string.Empty;
(name2, lastname2) = author;
Console.WriteLine(name2);
Console.WriteLine(lastname2);
```

C# 10.0  убирает ограничение предыдущей версии языка.

```csharp
var lastname3 = string.Empty;
(var name3, lastname2) = author;
Console.WriteLine(name2);
Console.WriteLine(lastname2);
```



## Global using directives

Ещё одна фича называется Global using directives.

Если вас раздражает импортировать одни и те же директивы в каждом файле в приложении, то эта фича для вас. C# 10 позволяет пометить импорты как глобальные, и они будут автоматически импортированы во все файлы приложения. 



```c#
global using System.Collections.Generic;
global using System.Linq;
```



```c#
namespace _6._Global_Using_directive;
public class Store
{
    private readonly IEnumerable<Book> Books;
    public Store(IEnumerable<Book> books) => Books = books;
    public IEnumerable<Book> GetBooks(Author author) =>
        Books.Where(b => b.Author.LastName == author.LastName);
}
```

Более того. В конфигурации проекта можно выставить специальный параметр, и тогда даже глобальные юзинги можно не прописывать.

```xml
<ImplicitUsings>enable</ImplicitUsings>
```



## Improvements of structure types

Начиная с C# 10.0 мы можем объявлять конструктор без параметров для структур.



```c#
public struct Point
{
    public Point()
    {
        X = double.NaN;
        Y = double.NaN;
    }
    public Point(double x, double y) =>
        (X, Y) = (x, y);

    public double X { get; set; }
    public double Y { get; set; }

    public override string ToString() =>
        $"X: {X}, Y: {Y}";
}

```

Будем выводить значения для разных способов объявления экземпляров структур.

```c#
var point = new Point(1,2);
Console.WriteLine(point);

var point2 = new Point();
Console.WriteLine(point2);

var point3 = default(Point);
Console.WriteLine(point3);

```

Как видим, default игнорирует конструктор без параметров и выдает дефолтное значение для типа.