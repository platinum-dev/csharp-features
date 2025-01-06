namespace InitOnlySetters
{
    public class Edition
    {
        public string Title { get; set; }
    }
    public class Book
    {
        public string Title { get; init; }
        public string Author { get; set; }
        public Edition Edition { get; set; }

    }
    class Program
    {
        static void Main()
        {
            var book = new Book { Author = "1", Title = "2" };
            var book1 = new Book();
            System.Console.WriteLine($"Edition title: {book1.Edition.Title}");
            //book.Title = "3";
        }
    }
}
