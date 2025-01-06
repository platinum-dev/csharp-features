namespace _6._Global_Using_directive;
public class Store
{
    private readonly IEnumerable<Book> Books;
    public Store(IEnumerable<Book> books) => Books = books;
    public IEnumerable<Book> GetBooks(Author author) =>
        Books.Where(b => b.Author.LastName == author.LastName);
}