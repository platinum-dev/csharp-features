# Список фич C# 9

## Top-level statements

Самая простая программа на C# выглядит так

```c#
using System;
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

но в C# 9 мы можем ещё больше упростить её:

```c#
System.Console.WriteLine("Hello World!");
```

## Target-Typed Object Creation

Рассмотрим класс:

```c#
public class Book
{
	public string Title { get; set; }
	public string Author { get; set; }

	public Book()
	{

	}

	public Book(string title, string author)
	{
		Title = title;
		Author = author;
	}
}
```

Обычно мы создаем объекты так:

```c#
var book = new Book();
// or
Book book = new Book();
```

Теперь можно и так:

```c#
Book book2 = new();
Book book3 = new("1", "A1");
```

## Init-only Setters

Рассмотрим класс `Book`

```c#
public class Book
{
        public string Title { get; set; }
        public string Author { get; set; }
}
```

и мы можем инициализировать поля во время создания объекта. А также мы можем их менять потом, так как у нас есть сеттер.

```c#
var book = new Book { Author = "1", Title = "2" };
book.Title = "2";
```
А что если мы хотим установить значение поля только во время инициализации и запретить менять его в дальнейшем? в `C# 9` появилась такая возможность.

```c#
public class Book
{
        public string Title { get; init; }
        public string Author { get; init; }
}

var book = new Book { Author = "1", Title = "2" };
book.Title = "2"; // compile error
```

## Relational & Logical Patterns

```c#
public class Book
{
	public string Title { get; set; }
	public string Author { get; set; }

	public static decimal Postage(decimal price) => price switch
	{
		< 20 => 6.99m,
		>= 20 and < 40 => 5.99m,
		>= 40 and < 60 => 2.99m,
		_ => 0
	};
}
```

## Records

```c#
public class Book
{
    public string Title { get; }
    public string Author { get; }

	public Book(string title, string author)
    {
          Title = title;
          Author = author;
    }
}
```

Представим, что мы создаем объект

```c#
var book = new Book("Title1", "Author1");
```

и хотим его сериализовать и десериализовать

```c#

var json = JsonSerializer.Serialize(book);
Console.WriteLine(json);
var book2 = JsonSerializer.Deserialize<Book>(json);
var isEqual = book == book2;
Console.WriteLine($"book == book2: {isEqual}"); // false

```

и увидим, что теперь это не один и тот же объект. Но как сделать так, чтобы он был тем же самым? Мы можем переопределить `==`:

```c#
public static bool operator ==(Book left, Book right) =>
	left is object ? left.Equals(right) : right is null;
```

а также нужно переопределить оператор `!=`:

```c#
public static bool operator !=(Book left, Book right) => !(left == right);
```

а также метод `Equals` для сравнения объектов:

```c#
public override bool Equals(object obj) => obj is Book b && Equals(b);

public bool Equals(Book other) =>
	other is object &&
	Title == other.Title &&
	Author == other.Author;

```

и `GetHashCode`, и `ToString` тоже.

```c#
public override int GetHashCode() => HashCode.Combine(Title, Author);
public override string ToString() => $"{Title} - {Author}";
```

И еще сделать, чтобы наш класс реализовывал интерфейс `IEquatable<T>`.

```c#
public class Book : IEquatable<Book>
```

в итоге вон сколько кода:

```c#
public class Book : IEquatable<Book>
{
   public string Title { get; }
   public string Author { get; }

   public Book(string title, string author)
   {
       Title = title;
       Author = author;
   }

   public static bool operator ==(Book left, Book right) =>
       left is object ? left.Equals(right) : right is null;

   public static bool operator !=(Book left, Book right) => !(left == right);

   public override bool Equals(object obj) => obj is Book b && Equals(b);

   public bool Equals(Book other) =>
       other is object &&
       Title == other.Title &&
       Author == other.Author;

   public override int GetHashCode() => HashCode.Combine(Title, Author);
}
```

теперь `Console.WriteLine($"book == book2: {isEqual}");` выводит`true`.

Довольно много boilerplate кода. А еще, если мы добавим новое поле, то нужно будет обновить каждый метод.

В `C# 9` мы можем использовать тип `record` для точно такого же поведени. Это позволяет нам сделать поведение классов таким, как если бы они были структурами.

```c#
record Book(string Title, string Author)
```



## Extended partial methods

Теперь можно использовать модификатор и `return` в `partial` методах.

```c#
public partial class Book
{
	public string Title { get; set; }
	public string Author { get; set; }

	public decimal Price { get; set; }

	private partial decimal SetPrice();
}

public partial class Book
{
	private partial decimal SetPrice()
	{
		return 0m;
	}
}

```

## Covariant returns

В `C# 9` мы можем возвращать класс-наследник и перегруженных методах.

```c#
public class Book
{
	public string Title { get; set; }
	public string Author { get; set; }
}

public class CollectionBook : Book
{
	public string Edition { get; set; }
}

public abstract class BookService
{
	public abstract Book GetBook();
}

public class CollectionBookService : BookService
{
	public override CollectionBook GetBook()
	{
		return new CollectionBook();
	}
}
```

