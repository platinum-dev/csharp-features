## Псевдоним любого типа (Alias any type)

Как и ребята из презентации, начнем с простых вещей. Первая фича, которую мы рассмотрим, называется - Alias any type, или псевдоним любого типа.

Теперь мы можем использовать директиву `using`, чтобы создать алиас для любого типа, а не только именованного.

Например, мы можем написать вот так:

```c#
using Dec = decimal; // вместо System.Decimal
using SomethingAbstract = (string, decimal);
using Grade = (string Course, decimal Value);
```

Мы можем попробовать использовать указатель

```c#
using Grade = decimal*;
```

Но тогда получим ошибку, в принципе мы и раньше ее получили бы. А теперь можно указать что это unsafe контекст и это будет работать

```c#
using unsafe Gradle = decimal*;
```

(только для этого также нужно разрешить `unsafe` контекст в настройках проекта):

```xml
<AllowUnsafeBlocks>true</AllowUnsafeBlocks>
```

Может возникнуть вопрос - зачем это нужно. Основная цель, которую преследовали разработчики на этапе проектирования заключается в том, что на протяжение многих лет в C# можно было объявлять алиасы для неймспейсов и именованных типов, то есть для классов, структур, интерфейсов, рекордов. И это предоставляло инструмент для объявления неконфликтующих имен в случаях, когда имена из разных неймспейсов были одинаковые. Наверное каждый видел такую ошибку где написано, что ambiguous reference, сделайте что-нибудь. И мы могли объявить алиас.

В то же время, появление в языке новых сложных конструкций привело к тому, что алиасы могли бы быть полезными, но использовать их нельзя. То есть например такие конструкции как кортежи или указатели на функции часто могут иметь большие и сложные объявления, которые может быть болезненно постоянно выписывать и обременительно пытаться читать. И алиасы могут помочь в этих случаях, указав короткое имя, которое затем можно использовать вместо полных объявлений.

В рамках рефакторингов проектов возможно лучше подумать о вынесении таких конструкций в отдельные классы с полями вместо алиасов для сложных типов.

## Основные или первичные конструкторы (Primary constructors)

Следующая фича называется Primary consturtors, или основные, первичные конструкторы. Это возможность определить конструктор прямо в объявлении класса или структуры. Как это можно делать в`record`'ах.

То есть, объявили класс и прямо рядом в скобочках написали параметры и это будет основным конструктором. Также, стоит отметить, что эти параметры будут доступны в рамках всего класса/структуры. Это можно сравнить с поведением параметров методов внутри этих методов.

```csharp
internal class Article(int id, string title)
{
}
```

В данном случае при объявлении объекта класса не получится использовать конструктор без параметров, так как конструктор по умолчанию подменяется.

Данный код приведет к ошибке:
```csharp
Article article = new();
```

Но мы можем сами объявить конструктор без параметров, если укажем ключевое слово `this` для вызова первичного конструктора.

```csharp
internal class Article(int id, string title)
{
    public Article() : this(0, string.Empty)
    {
    }
}
```

Тоже самое относится и к любым другим конструкторам:

```csharp
internal class Article(int id, string title)
{
    public Article() : this(0, string.Empty)
    {
    }

    public Article(string title, string author) : this(0, title)
    {

    }
}
```

Это может быть удобным в случае, когда мы например используем внедрение зависимостей и эти зависимости передадим в первичный конструктор - будет сильно меньше кода.

Из первичного конструктора также удобно передавать параметры в конструктор базового класса, если такой есть.

```csharp
internal class Article(int id, string title) : Item(id) { ... }

internal class Item(int id);
```

Как мы уже упомянули ранее, параметры в первичном конструкторе не становятся свойствами, как это например происходит если мы используем `record`.

```csharp
var title = article.Title;

internal record Article(string Title);
```

В классах мы явно определяем свойства. И тут можем присвоить значение из конструктора например.

```csharp
public string Title { get; set; } = title;
```

Мы можем захотеть сделать `readonly` свойство.

```csharp
public int Id => id;
```

Это не тоже самое. В этом случае произойдет захват переменной `id` из конструктора, как, например, происходит захват переменных в лямба-методах. Компилятор посмотрит и скажет, а окей, нужно сохранить эту переменную где-то. В случае с `Title` захвата не происходит, мы присваиваем и храним значение в отдельном свойстве. Тут мы явно не храним, а обращаемся к переменной id за значением.

Мы можем сделать отдельное поле, куда сохраним значение id. И теперь id уже будет ссылаться на это поле.

```csharp
private readonly int id = id;
public int Id => id;
```

Посмотрим еще один пример, представим у нас такой код:

```csharp
public int Id => title.Length;
```

Выше мы увидим предупреждение, что параметр title и захватывается, и мы его используем для инициализации свойства. То есть дважды сохраняем одно и тоже, чего вероятно делать не планировали. Тут мы конечно тоже могли бы сделать приватное поле.

Вообще, захват переменной это не плохо, просто стоит помнить об этом при написании кода более сложного, чем данный пример.

Также первичные конструкторы можно трансформировать в обычные - Visual Studio и Rider имеют такую функцию на борту.

## Выражения коллекции (Collection expressions)

Следующая фича называется "Выражения коллекции".

Возьмем за основу предыдущий пример.

В конструкторе пусть будет третий параметр - массив оценок статьи.

Тут мы также используем фичу "Псевдоним любого типа". `Score` это ни что иное как `double`.

```csharp
using Score = double;
```

```csharp
internal class Article(int id, string title, Score[] scores)
{
    public Article() : this(0, string.Empty, Array.Empty<Score>())
    {
    }
}
```

В какой-то момент мы можем захотеть использовать, например, не массив, а List на входе.

```csharp
internal class Article(int id, string title, List<Score> scores)
```

И что мы увидим, если поменяем?

Компилятор будет ругаться, потому что у нас вроде список, а мы пытаемся обмануть его и подсунуть массив.

Ну хорошо, мы просто будем использовать оператор `new()`:

```csharp
Article article = new(1, "C# features", new() { 5.0, 4.5 ,4.0 });
// ...
public Article() : this(0, string.Empty, new()) {}
```

Теперь, всё хорошо. А если мы вдруг захотели поменять список на неизменяемый список. Или обратно на массив

```csharp
internal class Article(int id, string title, ImmutableList<Score> scores)
```

На нас опять будут ругаться.

Но есть кое-что поинтереснее. C# 12 предлагает красивый способ объявления коллекций. Нам достаточно использовать квадратные скобочки.

```csharp
Article article = new(1, "C# features", [ 5.0, 4.5 ,4.0 ]);
// ...
public Article() : this(0, string.Empty, []) {}
```

Кстати для IEnumerable такой синтаксис тоже будет работать. Очень удобно.

Таким же способом можно объявлять массивы массивов:

```csharp
int[][] jagged = [[1,2,3], [4,5,6]];
// ===
double[] scores = [ 5.0, 4.5 ,4.0 ];
Article article = new(1, "C# features", scores);
double[][] jagged = [[1,2,3], [4,5,6], scores];
```

Здесь же появился spread оператор, он же оператор расширения. С его помощью можно объединять коллекции. Операндом spread оператора является что-то, что может быть перечислено. Spread оператор оценивает каждый элемент выражения.

```csharp
double[] scores = [ 5.0, 4.5 ,4.0 ];
Article article = new(1, "C# features", [..scores, 4.8, 3]);
```

Можно использовать его в шаблонах сопоставления - matching pattern

```csharp
public Score Score => scores switch
    {
        [] => 0,
        [var score] => score,
        [.. var all] => all.Average()
    };
```

Тут вернется ноль, если коллекция пустая, если один элемент, то его значение и вернется, а в остальных случаях посчитается среднее.

## Встроенные массивы (Inline arrays)

Следующая фича называется "Встроенные массивы" и будет полезна тем, кому необходимо писать высокопроизводительный код. Изначально Майкрософт сделали это для себя, а потом сделали это публичным.

Встроенные массивы позволяют разработчику создавать массив фиксированного размера в типе `struct` .

Чтобы создать такую структуру, нужно пометить её атрибутом InlineArray и описать единственное поле в ней, которое будет определять тип массива.

```csharp
[System.Runtime.CompilerServices.InlineArray(10)]
internal struct Buffer
{
    private int _;
}
```

И далее можно пользоваться этим массивом:

```csharp
var buffer = new Buffer();
for (int i = 0; i < 10; i++)
{
    buffer[i] = i;
}

foreach (var i in buffer)
{
    Console.WriteLine(i);
}
```

## Параметры лямбда выражений по умолчанию (Default lambda parameters)

Следующая фича дает возможность добавить параметр по умолчанию в лямбда выражениях. Синтаксис абсолютно такой же, как если бы мы добавляли параметр по умолчанию в любой другой метод.

```csharp
var setVolume = (int value = 0) => $"The volume is set to {value}";

Console.WriteLine(setVolume());
Console.WriteLine(setVolume(4));

```

## `ref` `readonly` параметры

Следующая фича называется `ref` `readonly` параметры.

Прежде чем ее рассматривать, я предлагаю освежить в памяти передачу параметров по ссылке.

Предположим, у нас есть такая структура.

```c#
struct Data(int value)
{
    public int Value { get; set; } = value;
}
```

при передаче структуры в качестве параметра метода передается не исходная структура а ее копия, так как значимые типы данных передаются по значению, а не по ссылке.

Для передачи по ссылке мы можем использовать такие ключевые слова как `ref`, `out`, `in`.

Ключевое слово `ref` мы используем и при описании параметра метода и при передаче в него аргумента. При этом аргумент должен быть проинициализирован заранее, а внутри метода мы можем его изменять

```c#
Data data1 = new(1);
Ref(ref data1);
Console.WriteLine(data1.Value);

void Ref(ref Data data)
{
    data.Value += 1;
}
```

Ключевое слово `out` мы также используем и при описании параметра метода и при передаче аргумента. Особенность тут в том, что аргумент может быть не проинициализирован, а может и проинициализирован, но вызываемый метод обязан присвоить значение параметру. И мы можем менять значение.

```c#
Data data2;
Out(2, out data2);
Console.WriteLine(data2.Value);

void Out(int value, out Data data)
{
    data = new(value);
    data.Value += 1;
}
```

В случае с параметрами с модификатором `in` в описании параметра метода он обязателен, при передаче аргумента необязателен. Можно для аргумента указать `ref`, но будет предупреждение, говорящее о том что в данном случае `ref` соответствует `in` и используйте пожалуйста `in`. Также можно передать выражение, например создание новой структуры. Присваивать и изменять значение внутри метода нельзя.

```c#
Data data3 = new(3);
In(data3);
In(in data3);
In(ref data3);
In(data3 = new Data());

void In(in Data data)
{
    // data.Value = 1;
    // data.Value += 1;
}
```

C# 12 привносит еще один модификатор - `ref readonly`.

Но ведь рассмотренный модификатор `in` уже позволяет передать `readonly` ссылку зачем еще и `ref readonly` сделали.

Спецификация от`Microsoft` говорит примерно следующее:

```
`in` параметры допускают как lvalues, так и rvalues и могут использоваться без каких-либо аннотаций на месте вызова. Однако API, которые захватывают или возвращают ссылки из своих параметров, хотели бы запретить rvalues, а также обеспечить некоторое указание на месте вызова о том, что ссылка захватывается. параметры ref readonly идеальны в таких случаях, поскольку они предупреждают, если используются с rvalues или без каких-либо аннотаций на месте вызова.
```

Тут могут запутать такие термины как `lvalues` и `rvalues`. Поэтому кратко о том, что это такое.

`lvalue` расшифровывается как locator value и представляет объект, который занимает идентифицируемое место в памяти.

а `rvalue` это всё остальное, то есть выражение, которое не занимает идентифицируемое место в памяти.

Вот простой пример:

```c#
int value;
// ...
value = 1;
```

Оператор присваивания ожидает `lvalue` с левой стороны и value им является.

если мы напишем:

```c#
1 = value;
```

то получим ошибку, потому что `1` это не `lvalue'. Кратко это всё.

Возвращаясь к фиче, `ref` `readonly` в таком случае будет "идеальным", как это сказано в документации. Если мы передадим в качестве аргумена `data4 = new Data()`, у нас появится предупреждение.

```c#
Data data4 = new(4);
RefReadonly(data4);
RefReadonly(in data4);
RefReadonly(ref data4);
RefReadonly(data4 = new Data());

void RefReadonly(ref readonly Data data)
{
    // data.Value = 1;
    // data.Value += 1;
}
```

А также, например есть уже существующие API которым нужны только readonly ссылки, но они используют `ref`, так как этот модификатор появился раньше модификатора `in` и поменять в коде везде `ref` на `in` это будут ломающие изменения и на уровне кода и на уровне бинарников.

Также может быть, что эти уже существующие API используют `in`, но передача rvalues им не нужна. Такие API смогут перейти на использование `ref` `readonly` почти безболезненно.

## Экспериментальный атрибут (Experimental attribute)

Предположим, что мы разработали какую-то потенциально опасную для продакшена фичу и почему-то выпустили ее, возможно для каких-то экспериментов.

```c#
var feature = new DangerFeature();

internal class DangerFeature
{
}
```

На такой случай в C# 12 появился новый атрибут - `Experimental`. Принимает параметр `diagnosticsId`. Это идентификатор, который компилятор будет использовать при сообщении об использовании API, к которому применяется этот атрибут. Если мы пометим класс этим атрибутом, то при создании объекта будет ошибка компиляции, сообщающая о том, это это экспериментальная фича и она может быть изменена или удалена в будущем. Но можно настойчиво сказать компилятору, что мы будем использовать фичу с помощью `#pragma`.

```c#
using System.Diagnostics.CodeAnalysis;

#pragma warning disable Danger_Identifier
var feature = new DangerFeature();
#pragma warning restore Danger_Identifier

[Experimental("Danger_Identifier")]
internal class DangerFeature
{
}
```

## Перехватчики (Interceptors)

Последняя фича является экспериментальной и называется Interceptors или перехватчики.

Рассмотрим на примере.

У нас есть класс отправителя сообщений и всё что он делает это отправляет сообщение - в нашем случае приветствует всех в консоли.

```c#
public class Sender
{
    public void SendMessage()
    {
        Console.WriteLine("Hello everyone!");
    }
}
```

Создадим объект отправителя и отправим сообщение:
```c#
var sender = new Sender();
sender.SendMessage();
```

Программа выведет в консоль текст "Hello everyone!".

Теперь создадим класс-перехватчик, назовем его хакером. Класс должен быть статическим и в нем мы добавляем метод расширения для класса отправителя. И выведем в консоль, что вы были взломаны.

```c#
public static class Hacker
{
    public static void InterceptMessage(this Sender sender)
    {
        Console.WriteLine("You're hacked");
    }
}
```

Для того, чтобы перехватчик заработал, нужно добавить атрибут. Но помимо этого, нам самим нужно еще и объявить его в отдельном файле.

```c#
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    sealed class InterceptsLocationAttribute(string filePath, int line, int column) : Attribute
    {
    }
}
```

Далее нужно в csproj файле включить экспериментальную фичу, указав namespace, где лежит перехватчик, в нашем случае класс хакера.

```xml
<InterceptorsPreviewNamespaces>$(InterceptorsPreviewNamespaces);HackerSpace</InterceptorsPreviewNamespaces>
```

Теперь мы можем использовать этот атрибут. В параметрах нужно указать путь до файла, где метод вызывается, строку и столбец.

Запустим и увидим сообщение: "You're hacked" вместо "Hello everyone".

Вероятно это не та фича, которой мы можем захотеть пользоваться регулярно и подойдет она больше в сфере кодогенерации.