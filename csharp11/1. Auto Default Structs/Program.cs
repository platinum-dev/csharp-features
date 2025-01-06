var data = new Data();
Console.WriteLine(data);
readonly struct Data
{
    public decimal Number { get; init; }
    public string Text { get; init; }
    public DateTime Date { get; init; }

    public Data()
    {
        Text = string.Empty;
    }

    public override string ToString() => $"Number: {Number}, Text: {Text}, Date: {Date}.";
}
