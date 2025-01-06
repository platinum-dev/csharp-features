var buffer = new Buffer();
for (var i = 0; i < 10; i++)
{
    buffer[i] = i;
}

foreach (var i in buffer)
{
    Console.WriteLine(i);
}

[System.Runtime.CompilerServices.InlineArray(10)]
internal struct Buffer
{
    private int _;
}