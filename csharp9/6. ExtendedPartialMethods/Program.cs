using System;

namespace ExtendedPartialMethods
{
    public partial class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public decimal Price { get; set; }

        private partial decimal SetPrice();

    }

    public partial class Book
    {
        private partial decimal SetPrice()
        {
            return 0m;
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

