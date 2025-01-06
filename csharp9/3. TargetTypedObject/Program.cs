
namespace TargetTypedObject
{
    public class Book
    {
        public string Title { get; }
        public string Author { get; }

        public Book() { }
        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }
    class Program
    {
        static void Main()
        {
            var book = new Book("1", "2");

            Book book2 = new("1", "2");

            Book book3 = new();
        }
    }
}
