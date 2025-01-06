# Новые фичи C# 13

## `params collections`

Само ключевое слово `params` появилось еще в версии языка 1.0. И всегда `params` параметрами должны были быть массивы. Но было бы удобно иметь возможность разработчикам также просто вызывать API, которые принимают другие типы коллекций. Например, `ReadOnlySpan<T>` или `IEnumerable`. Особенно в тех случаях, когда компилятор может избежать неявного выделения массива с целью создания коллекции.

Предыдущий релиз, C# 12, принес поддержку удобного синтаксиса создания экземпляров коллекций. Фича называлась `collection expressions`. 
Новая же фича `params collections` расширяет поддержку `params` для всех таких типов коллекций.

Так что, разработчикам больше не нужно добавлять перегрузки для `params` которые принимают `array`, конструируют целевую коллекцию и вызывают оригинальную перегрузку с этой коллекцией, что приводило к тому, что пользователи API вынуждены были использовать дополнительное выделение памяти под массив для удобства.
Вообще если честно, то я такого не видел, но ребята из Майкрософт говорят, что разработчики обычно так делали. А вы так делали? Напишите в комментариях.
Пример
```csharp
ReadOnlySpan<string> text = new(["Hello", "ReadOnlySpan", "World"]);  
Printer.PrintElements(text);  
  
internal static class Printer  
{  
    internal static void PrintElements<T>(params ReadOnlySpan<T> elements)  
    {
		foreach (var element in elements)  
        {
			Console.WriteLine(element);  
        }
    }
}
```


## Overload resolution priority
Появился новый атрибут `OverloadResolutionPriorityAttribute`. Его можно использовать для предпочтения одной перегрузке другой. Авторы библиотек могут использовать этот атрибут, чтобы убедиться, что новая, улучшенная перегрузка предпочтительнее существующей перегрузки. Например, мы можем добавить новую перегрузку метода, которая более производительная существующей. И мы не хотим ломать существующий код, который использует нашу библиотеку, но мы хотели бы, чтобы пользователи получили новую версию метода после перекомпиляции. Так что, этот атрибут используется для информирования компилятора о том, какая перегрузка должна быть предпочтена. 
Эта функция предназначена в большей степени для авторов библиотек, чтобы избежать двусмысленности при добавлении новых перегрузок. И авторы библиотек должны быть осторожны с этим атрибутом, чтобы избежать путаницы.

А мотивацией к созданию такого атрибута послужил тот факт, что авторы различных API часто попадали в ситуацию, когда для обеспечения обратной совместимости, помечали существующий устаревший метод атрибутом Obsolete навсегда. Это особенно актуально для плагинизируемых систем, где автор плагина не контролирует среду, в которой запускается плагин. И создатель среды хотел бы оставить старый метод, но заблокировать к нему доступ для нового кода. И атрибута Obsolete для этого недостаточно. Тип или метод будет виден при overload resolution и может привести к ошибкам в то время как его усовершенствованная версия уже есть, но она либо конфликтует с устаревшей версией, либо

Пример:
```csharp
using System.Collections.Immutable;  
using System.Runtime.CompilerServices;  
  
Printer.PrintElements(["Hello", "Overload Resolution", "World"]);  
  
internal static class Printer  
{  
    internal static void PrintElements<T>(ImmutableArray<T> elements)  
    {
		foreach (var element in elements)  
        {
			Console.WriteLine(element);  
        }
	}

    [OverloadResolutionPriority(1)]  
    internal static void PrintElements<T>(params ReadOnlySpan<T> elements)  
    {
		foreach (var element in elements)  
        {
			Console.WriteLine(element);  
        }
	}
}
```

## New lock object

Рантайм `.NET 9` включает новый тип для синхронизации потоков - `System.Threading.Lock`. 
Класс `Lock` может использоваться для определения областей кода, требующих взаимоисключающего доступа между потоками процесса (обычно называемых критическими секциями) для предотвращения одновременного доступа к ресурсу. То есть это блокировка другими словами. И в нее можно входить и выходить. Поток, который входит Считается, что поток, который входит в блокировку, удерживает её до тех пор, пока не выйдет из нее. В любой момент времени блокировка может быть установлена не более чем в одном потоке. В потоке может быть несколько блокировок. Поток может вводить блокировку несколько раз, прежде чем выйти из нее, например, рекурсивно. Поток, который не может ввести блокировку немедленно, может подождать, пока блокировка не будет введена или пока не истечет указанный тайм-аут.

У объекта `Lock` есть методы `Enter()` и `TryEnter()` и при их использовании стоит:
- убедиться, что поток выходит из блокировки с помощью метода `Exit()` даже в случае исключений
- При входе и выходе из блокировки в асинхронном методе нужно убедиться, что между входом и выходом нет ожидания. Блокировки удерживаются потоками, и код, следующий за ожиданием, может выполняться в другом потоке.

Рекомендуется использовать метод `EnterScope` с конструкцией `using` или `lock`, то есть чем-то, что автоматически задиспоузит возвращаемую блокировку (объект `Lock.Scope`). 

Стоит отметить, что при использовании внутри ключевого слова `lock` не новый объект `Lock`, а, например, `object` или дженерик, то будет использоваться реализация блокировки на основе монитора (`System.Threading.Monitor`).

Пример
```csharp
var modifier = new LockDemo();
modifier.Modify();

internal class LockDemo
{
    private readonly Lock _lockObj = new();

    public void Modify()
    {
        lock (_lockObj)
        {
            Console.WriteLine("I'm a critical section associated with _lockObj");
        }

        using (_lockObj.EnterScope())
        {
            Console.WriteLine("I'm another critical section associated with _lockObj");
        }

        _lockObj.Enter();
        try
        {
            Console.WriteLine("I'm also a critical section associated with _lockObj");
        }
        finally
        {
            _lockObj.Exit();
        }

        if (_lockObj.TryEnter())
        {
            try
            {
                Console.WriteLine("I'm also another critical section associated with _lockObj");
            }
            finally
            {
                _lockObj.Exit();
            }
        }
    }
}
```

## New escape sequence
Теперь можно использовать литерал `\e` как управляющую последовательность для символа  `ESCAPE`. Ранее для этого использовались `\u001b` или `\x1b`.
Использование `\x1b` не рекомендуется, потому что если следующие за `1b` символы являются допустимыми шестнадцатеричными цифрами, то эти символы становятся частью управляющей последовательности.

## Method Group Natural Type

Была добавлена оптимизация в определение перегрузки групп методов, по-английски Overload Resolution Involving Method Groups. Если у вас есть более корректный перевод, то напишите в комментариях.
Method Groups это группа методов с одинаковым именем в типе, но с разными параметрами, то есть перегрузки.

До C# 13 определялось всё по-простому и при необходимости нужно было делать приведение группы методов чтобы получить корректный

Теперь компилятор проделывает определенную работу, для того чтобы отсеять те методы, которые точно не подойдут и чтобы можно было использовать нужный метод без дополнительного приведения.

## Implicit Index Access
Неявный индекс-оператор ("с конца"), `^`, теперь может использоваться в выражениях инициализации объектов. Как в следующем примере. Кстати, этот оператор был подробно рассмотрен в одном из видео на канале ([Rutube](https://rutube.ru/video/864a544721a1393d9d7af311cc2c4c97/) и [YouTube](https://youtu.be/NWiC2xoI1z0)).

Пример
```csharp
var countdown = new TimerRemaining()  
{  
    Buffer =  
    {   [^1] = 0,
        [^2] = 1,
        [^3] = 2,  
        [^4] = 3,  
        [^5] = 4,  
        [^6] = 5,  
        [^7] = 6,  
        [^8] = 7,  
        [^9] = 8,  
        [^10] = 9  
    }  
};  
  
foreach (var item in countdown.Buffer)  
{  
    Console.WriteLine(item);  
}  
  
public class TimerRemaining  
{  
    public int[] Buffer { get; } = new int[10];  
}
```

Класс `TimerRemaining` содержит массив `Buffer` длинною 10 элементов. И в примере мы присваиваем значения этому массиву от конца к началу, используя индекс-оператор.

## `ref` and `unsafe` in iterators and `async` methods

Эта и следующие две фичи позволяют `ref struct` типам использовать новые конструкции. Вряд ли это пригодится, если вы используете самописные `ref struct` типы.

До 13-й версии, `iterator` методы (то есть, те, которые используют `yield return`) и `async` методы не могли объявить локальные `ref` переменные, а также иметь `unsafe` контекст.

В C# 13, `async` и `iterator` методы могут объявить локальные `ref` переменные, или локальные переменные типа `ref struct`. Однако эти переменные не могут быть доступны в блоке, где используется, например, `yield return`. То есть, все `yield return` и `yield break`должны быть в безопасном контексте.

Пример
```csharp
internal class Calculator
{
    internal async Task Increment()
    {
        ref var value = ref GetValue();
        await Task.Delay(1000);

        // not allowed
        // value++;

        // '<AllowUnsafeBlocks>true</AllowUnsafeBlocks>' must be declared in csproj file in order to use the 'unsafe' keyword
        unsafe
        {
        }
    }

    private int _value = 1;
    private ref int GetValue() => ref _value;

    internal IEnumerable<int> GetFibonacci(int maxValue)
    {
        var previous = 0;
        var current = 1;

        ref var value = ref GetValue();
        while (current <= maxValue)
        {
            yield return current;

            // not allowed
            // value++;
            var newCurrent = previous + current;
            previous = current;
            current = newCurrent;
        }
    }
}
```

## `allows ref struct`
До C# 13 типы `ref struct` не могли были объявлены как аргументы типа для дженерик типа или метода. Теперь можно добавить анти ограничение - `allows ref struct`, которое говорит о том, что тип аргумента может быть типом `ref struct`. Это позволяет использовать с дженерик алгоритмами такие типы как `System.Span<T>` и `System.ReadOnlySpan<T>`.
Пример
```csharp
var notifier = new Notifier<ReadOnlySpan<string>>();
var notifier2 = new Notifier<Data>();

public class Notifier<T> where T : allows ref struct
{
    // Use T as a ref struct:
    public void Notify(scoped T p)
    {
        // The parameter p must follow ref safety rules
    }
}

public ref struct Data
{
    public int Value { get; set; }
}
```

## `ref struct` interfaces
Также, до C# 13 `ref struct` типам не позволялось реализовывать интерфейсы. Теперь можно.
Однако, стоит помнить, что `ref struct` тип не может быть приведен к интерфейсному типу.
Также, должны быть реализованы все методы, объявленные в интерфейсе, даже те, у которых есть реализация по умолчанию. Подробнее про реализацию интерфейсов по умолчанию: [Rutube](https://rutube.ru/video/ea144d753c685e19db313562b7dba38a/?r=plwd), [YouTube](https://youtu.be/gxkaVOwMRnM).

Пример.
```csharp
var refStruct = new RefStruct { Value = 13 };  
Console.WriteLine(refStruct.CheckIfValid());  
refStruct.Value *= -1;  
Console.WriteLine(refStruct.CheckIfValid());  
  
ref struct RefStruct : IRefStruct  
{  
    public int Value { get; set; }  
    public bool CheckIfValid() => Value > 0;  
  
    public int DefaultImplementationMethod() => 1;  
}  
  
interface IRefStruct  
{  
    bool CheckIfValid();  
  
    int DefaultImplementationMethod() => 1;  
}
```

## More partial members
Теперь можно объявлять `partial` свойства и `partial` индексаторы. Здесь всё очень похоже на `partial` методы. В одном месте создаем декларацию, в другом реализацию. Сигнатуры, конечно, должны совпадать. Одно только ограничение - нельзя использовать автосвойства для реализации `partial` свойства. 

Пример
```csharp
var items = new Items();

public partial class Items
{
    public partial int Capacity { get; set; }
}

public partial class Items
{
    public partial int Capacity
    {
        get => _items.Count;
        set
        {
            if (value != _items.Count && value >= 0)
            {
                _items.Capacity = value;
            }
        }
    }

    private readonly List<int> _items = Enumerable.Range(0, 10).ToList();
}
```
  
## The `field` keyword
Ключевое слово `field` это превью фича. С его помощью можно не создавать отдельное приватное поле для использования в свойстве, это сделает компилятор. Фича вышла в превью потому что разработчики хотят получить обратную связь от тех, кто будет это использовать. По их мнению могут быть потенциальные breaking changes или путаница в коде в типах, где также есть поле `field`. Поживем, увидим :)

Пример.

```csharp
// <LangVersion>preview</LangVersion> must be set in csproj
class TimePeriod
{
    public double Hours {
        get;
        set => field = (value >= 0)
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value), "The value must not be negative");
    }
}
```

