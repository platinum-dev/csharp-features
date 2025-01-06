namespace _2._Extended_Property_Patterns;
public class Book
{
    public string Title { get; set; }
    public Author Author { get; set; }
    public Book(string title, Author author) =>
        (Title, Author) = (title, author);

    public static bool doesHaveDiscount(Book book) =>
    book switch
    {
        { Author.LastName: "Richter" }
        or { Author.LastName: "Price" } => true,
        _ => false
    };

}
