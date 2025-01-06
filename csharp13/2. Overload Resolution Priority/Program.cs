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