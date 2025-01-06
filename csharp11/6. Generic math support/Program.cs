using System.Numerics;

var numbers = new[] { 1, 2, 3 };

var sum = Sum(numbers);
Console.WriteLine(sum);

T Sum<T>(T[] numbers) where T : INumber<T>
{
    T result = T.Zero;
    foreach (var n in numbers)
    {
        result += n;
    }
    return result;
}


//int Sum(int[] numbers)
//{
//    var result = 0;
//    foreach (var n in numbers)
//    {
//        result += n;
//    }
//    return result;
//}

//double SumD(double[] numbers)
//{
//    var result = 0.0;
//    foreach (var n in numbers)
//    {
//        result += n;
//    }
//    return result;
//}