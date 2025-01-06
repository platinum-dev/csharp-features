using Score = double;

double[] scores = [ 5.0, 4.5 ,4.0 ];
Article article = new(1, "C# features", [ 4.8, 3 ]);
var s = article.Score;
Console.WriteLine(s);

double[][] jagged = [[..scores, 3], [1, 2]];


Console.ReadLine();

internal class Article(int id, string title, Score[] scores)
{
    public Article() : this(0, string.Empty, [])
    {
    }

    public Score Score => scores switch
    {
        [] => 0,
        [var score] => score,
        [.. var all] => all.Average()
    };
}