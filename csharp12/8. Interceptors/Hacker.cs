using System.Runtime.CompilerServices;
using _8._Interceptors;

namespace HackerSpace;

public static class Hacker
{
    [InterceptsLocation(
        @"C:\Users\Andrei\source\repos\platinum\csharp12-features\8. Interceptors\Program.cs",
        4,
        8)]
    public static void InterceptMessage(this Sender sender)
    {
        Console.WriteLine("You're hacked");
    }
}