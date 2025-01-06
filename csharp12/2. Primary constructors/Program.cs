Article article = new(0, "");
var title = article.Title;
var id = article.Id;

Console.ReadLine();

internal class Article(int id, string title) : Item(id)
{
    public string Title { get; set; } = title;
    private readonly int id = id;

    public int Id => id;

    public Article() : this(0, string.Empty) {}

    public Article(string title, string author) : this(0, title)
    {

    }
}

internal class Item(int id);