using System;

namespace RelationalAndLogicalPatterns
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public static decimal Postage(decimal price) => price switch
        {
            < 20 => 6.99m,
            >= 20 and < 40 => 3.99m,
            >= 40 and < 60 => 2.99m,
            _ => 0
        };

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
