var c = new Calculator();

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