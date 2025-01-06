static int Transform(int[] values)
    => values switch
    {
        [1, 2, .., 10] => 1,
        [1, 2] => 2,
        [1, _] => 3,
        [1, ..] => 4,
        [..] => 50
    };

var array = new[] { 2, 3, 4, 5, 3, 4, 5 };
var result = Transform(array);
Console.WriteLine(result);

static string TransformExtra(int[] values)
    => values switch
    {
        [1, .., var middle, _] => $"Middle {string.Join(", ", middle)}",
        [3, var second, ..] => $"second = {second}",
        [.. var all] => $"All {string.Join(", ", all)}"
    };

Console.WriteLine(TransformExtra(array));
