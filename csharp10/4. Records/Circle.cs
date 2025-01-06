namespace _4._Records;
record Circle
{
    public sealed override string ToString()
    {
        return typeof(Circle).Name;
    }
}

record BigCircle : Circle
{
    //public override string ToString()
    //{
    //    return typeof(BigCircle).Name;
    //}
}
