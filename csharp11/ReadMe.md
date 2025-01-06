# Новые фичи C# 11

## Auto default structs

Представим, что у нас есть readonly структура с авто свойствами. Компилятор C# 11 гарантирует нам, что любое поле или автоматическое свойство, не инициализированное конструктором, будет проинициализировано автоматически. 

В C# 10 такое бы не сработало. Только если мы полностью уберем конструктор или полностью проинициализируем все свойства.

```c#
readonly struct Data
{
    public decimal Number { get; init; }
    public string Text { get; init; }
    public DateTime Date { get; init; }
    
    public Data()
    {
        Number = decimal.MinValue;
        Text = string.Empty;
    }
    
    public override string ToString() => $"Number: {Number}, Text: {Text}, Date: {Date}.";
}
```

## Generic Attributes

В предыдущих версиях для этого нам нужно было бы создать конструктор, который принимает тип и присваивать его соответствующему полю в атрибуте.

```c#
public class TypeAttribute : Attribute
{
   public TypeAttribute(Type t) => ParamType = t;

   public Type ParamType { get; }
}

[TypeAttribute(typeof(string))]
public string Method() => default;
```

В C# 11 мы можем создать кастомный атрибут и сделать его дженериком. То есть, создать дженерик класс, который в качестве базового класса имеет класс System.Attribute.

```c#
public class GenericAttribute<T> : Attribute { }

[GenericAttribute<string>()]
public string Method() => default;
```



Эта фича предоставляет более удобный синтаксис для атрибутов, которые требуют System.Type в качестве параметра. Ранее нам нужно было создавать атрибут, который принимает Type в конструкторе.



Аргументы типа должны соответствовать тем же ограничениям, что и оператор [`typeof`](https://learn.microsoft.com/ru-ru/dotnet/csharp/language-reference/operators/type-testing-and-cast#typeof-operator). Например, следующие типы не допускаются в качестве параметра типа:

- `dynamic`
- `string?` (или любой ссылочный тип, допускающий значение NULL)
- `(int X, int Y)` (или любые другие типы кортежей, использующие синтаксис кортежей C#).

Во всех случаях можно использовать базовый тип:

- `object` для `dynamic`.
- `string` вместо `string?`.
- `ValueTuple<int, int>` вместо `(int X, int Y)`.

```c#
public class GenericType<T>
{
   [GenericAttribute<T>()] // Not allowed! generic attributes must be fully constructed types.
   public string Method() => default;
}
```

## Newlines in string interpolations

Когда мы интерполируем строки, то можем использовать переменные внутри наших строк.

Теперь можно сделать сколько угодно переносов строк.

```c#
_ = $"interpolated string with value {(
    test ?
    number1
    :
number2
    )}, and some more text ...";
```

Эта функция облегчает чтение интерполированных строк, которые используют более длинные выражения C#, такие как  pattern matching в switch выражениях или запросы LINQ например.

## List Patterns

List patterns расширяет pattern matching для сопоставления последовательностей элементов в списке или массиве.

Представим, что у нас есть массив целых чисел и мы хотим что-то сделать в зависимости от того, что находится внутри этого массива. Мы можем сделать это разными способами, например

```c#
static int Transform(int[] values)
    => values switch
    {
        [1, 2, .., 10] => 1,
        [1, 2] => 2,
        [1, _] => 3,
        [1, ..] => 4,
        [..] => 50
    };
```

или вот, что еще можно делать теперь

```c#
static string TransformExtra(int[] values)
    => values switch
    {
        [1, .., var middle, _] => $"Middle {string.Join(", ", middle)}",
        [1, var second, ..] => $"second = {second}",
        [.. var all] => $"All {string.Join(", ", all)}"
    };
```

## Raw String Literals

Raw String Literals - это новый формат для строковых литералов. Теперь они могут содержать произвольный текст, включая пробелы, новые строки, встроенные кавычки и другие специальные символы, не требуя escape-последовательностей (или экранирования). 
Такой литерал начинается как минимум с трех символов двойных кавычек ("""). Таким же количеством и заканчивается. 

```c#
string longMessage = """
    This is a long message.
    It has several lines.
        Some are indented
            more than others.
    Some should start at the first column.
    Some have "quoted text" in them.
    """;

```



Любой пробел слева от закрывающих двойных кавычек будет удален из строкового литерала. Необработанные строковые литералы можно комбинировать со строковой интерполяцией для включения фигурных скобок в выходной текст. Несколько символов $ обозначают, сколько последовательных фигурных скобок начинают и заканчивают интерполяцию

```c#
var x1 = 0;
var y1 = 0;
var x2 = 1;
var y2 = 2;

var position = $$"""
You are at {{{x1}}, {{y1}}}
""";
```



```c#
var json = $$"""
{
    "Points": [
        {
            "X": {{x1}},
            "Y": {{y1}}
        },
        {
            "X": {{x2}},
            "Y": {{y2}}
        }
    ]
}
""";

Console.WriteLine($"JSON:\n{json}");

Console.WriteLine(JsonSerializer.Deserialize<Root>(json));

class Point
{
    public int X { get; set; }
    public int Y { get; set;}
}

class Root
{
    public List<Point> Points { get; set; }
    public override string ToString()
    {
        var count = Points.Count;
        StringBuilder builder = new StringBuilder();
        if (count == 0) builder.Append("No points");
        else
            for(var i = 0; i < count; ++i)
            {
                var rect = Points[i];
                builder.Append($"Point {i + 1} coordinates are: X={rect.X}, Y={rect.Y};\n");
            }
        return builder.ToString();
    }
}
```



```c#
var json = $$$"""
{
    "Points": [
        {
            "X": {{{x1}}},
            "Y": {{y1}}
        },
        {
            "X": {{x2}},
            "Y": {{y2}}
        }
    ]
}
""";

Console.WriteLine($"JSON:\n{json}");

// Console.WriteLine(JsonSerializer.Deserialize<Root>(json));
```



## Generic Math support

```c#
var numbers = new[] { 1, 2, 3 };

var sum = Sum(numbers);
Console.WriteLine(sum);

int Sum(int[] numbers)
{
    var result = 0;
    foreach (var n in numbers)
    {
        result += n;
    }
    return result;
}
```



```c#
var numbers = new[] { 1, 2, 3, 0.5 };

var sum = Sum(numbers);
Console.WriteLine(sum);

int Sum(int[] numbers)
{
    var result = 0;
    foreach (var n in numbers)
    {
        result += n;
    }
    return result;
}

double SumD(double[] numbers)
{
    var result = 0.0;
    foreach (var n in numbers)
    {
        result += n;
    }
    return result;
}
```



```c#
using System.Numerics;

var numbers = new[] { 1, 2, 3, 0.5 };

var sum = Sum(numbers);
Console.WriteLine(sum);

T Sum<T>(T[] numbers) where T : INumber<T>
{
    T result = T.Zero;
    foreach (var n in numbers)
    {
        result += n;
    }
    return result;
}
```



## Utf8 String Literals

Вы можете указать суффикс u8 в строковом литерале, чтобы указать кодировку символов UTF-8. Упрощает создание строк UTF-8.

Для read only span байтов, бинарная репрезентация строки (массив байтов символов) мы можем сделать такую же штуку. Мы добавляем u8 и это будет автоматически преобразовано в массив байтов.

```c#
ReadOnlySpan<byte> value1 = new byte[12]
{
    72, 101, 108, 108, 111, 32,
    119, 111, 114, 108, 100, 33
};

ReadOnlySpan<byte> value2 = "Hello world!"u8;

Console.WriteLine(Encoding.UTF8.GetString(value1));
Console.WriteLine(Encoding.UTF8.GetString(value2));
```

## File scoped types

Было добавлено еще одно ключевое слово. Теперь мы можем создать тип, видимость которого ограничена исходным файлом, в котором он объявлен. Новая функция может быть полезна при генерации кода, чтобы избежать коллизий имен. Теперь, если мы объявляем классы в разных файлах (но в одном и том же неймспейсе), и добавим файл-модификатор к одному из них то никаких конфликтов не возникает.

```c#
class Builder
{
}

file class Builder
{
}
```



## Required Properties

Когда мы создаем классы или рекорды, мы можем использовать init-only. Если у нас есть куча свойств, мы можем добавить ключевое слово required, чтобы сказать компилятору, что это свойство обязательное. Если мы используем этот класс и не инициализируем свойство через конструктор или инициализатор объекта, то будет ошибка.

```c#
using System.Diagnostics.CodeAnalysis;
Console.WriteLine("Hello World!");

_ = new Person
{
    FirstName = "Johny"
};
_ = new Employee("Johny", "Gear", "Secret Department");

public class Person
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public int Age { get; init; }
}

public class Employee : Person
{
    public required string Department { get; init; }
    
    [SetsRequiredMembers]
    public Employee(string firstName, string lastName, string department)
    {
        FirstName = firstName;
        LastName = lastName;
        Department = department;
    }
}
```

