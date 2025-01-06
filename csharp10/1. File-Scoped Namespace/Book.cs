namespace _1._File_Scoped_Namespace;
public class Book
{
    public string Title { get; set; }
    public Author Author { get; set; }
    public Book(string title, Author author) =>
        (Title, Author) = (title, author);
}