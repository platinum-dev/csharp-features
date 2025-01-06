using System.Text;

ReadOnlySpan<byte> value1 = new byte[12]
{
    72, 101, 108, 108, 111, 32,
    119, 111, 114, 108, 100, 33
};

ReadOnlySpan<byte> value2 = "Hello world!"u8;

Console.WriteLine(Encoding.UTF8.GetString(value1));
Console.WriteLine(Encoding.UTF8.GetString(value2));
