using System;

namespace CovariantReturns
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
    }

    public class CollectionBook : Book
    {
        public string Edition { get; set; }
    }

    public abstract class BookService
    {
        public abstract Book GetBook();
    }

    public class CollectionBookService : BookService
    {
        public override CollectionBook GetBook()
        {
            return new CollectionBook();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
