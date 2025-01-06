/* This feature makes small optimizations to overload resolution involving method groups.
 Method groups are a group of methods with the same name on a type but with different parameters (overloads).
 Prior to C# 13 the type resolution was quite basic and you often needed to cast the method group in order to get
 the correct method.
 C# 13 takes a number of steps to remove methods that are not applicable to the scope and means the compiler is
 more likely to identify the correct method without additional casts.
*/

ResilientNotifier notifier = new ResilientNotifier();

var action8 = (Action<string>) notifier.Notify;
// var action9 = notifier.Notify;

class Notifier
{
    public void Notify(string message) {}
}

class ResilientNotifier : Notifier
{
    public void Notify<T>(T data) {}
}