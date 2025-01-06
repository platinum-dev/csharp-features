public struct Point
{
    public Point()
    {
        X = double.NaN;
        Y = double.NaN;
    }
    public Point(double x, double y) =>
        (X, Y) = (x, y);

    public double X { get; set; }
    public double Y { get; set; }

    public override string ToString() =>
        $"X: {X}, Y: {Y}";
}
