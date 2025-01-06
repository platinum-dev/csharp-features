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