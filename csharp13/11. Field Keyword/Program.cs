Console.WriteLine("Just a hack to make a program compilable");

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
