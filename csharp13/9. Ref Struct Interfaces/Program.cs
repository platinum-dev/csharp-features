var refStruct = new RefStruct { Value = 13 };
Console.WriteLine(refStruct.CheckIfValid());
refStruct.Value *= -1;
Console.WriteLine(refStruct.CheckIfValid());

ref struct RefStruct : IRefStruct
{
    public int Value { get; set; }
    public bool CheckIfValid() => Value > 0;

    public int DefaultImplementationMethod() => 1;
}

interface IRefStruct
{
    bool CheckIfValid();

    int DefaultImplementationMethod() => 1;
}