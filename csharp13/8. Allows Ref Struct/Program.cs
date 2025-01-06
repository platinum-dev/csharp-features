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