namespace _6._Global_Using_directive;
public class Book
{
    public string Title { get; set; }
    public Author Author { get; set; }
    public Book(string title, Author author) =>
        (Title, Author) = (title, author);

    public static bool doesHaveDiscount(Book book)
    {
        return false;
    }

}
