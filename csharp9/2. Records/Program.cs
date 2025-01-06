
using System;
using System.Text.Json;

namespace Records
{
    //public class Book : IEquatable<Book>
    //{
    //    public string Title { get; }
    //    public string Author { get; }

    //    public Book(string title, string author)
    //    {
    //        Title = title;
    //        Author = author;
    //    }

    //    public static bool operator ==(Book left, Book right) =>
    //        left is object ? left.Equals(right) : right is null;

    //    public static bool operator !=(Book left, Book right) => !(left == right);

    //    public override bool Equals(object obj) => obj is Book b && Equals(b);

    //    public bool Equals(Book other) =>
    //        other is object &&
    //        Title == other.Title &&
    //        Author == other.Author;

    //    public override int GetHashCode() => HashCode.Combine(Title, Author);
    //}

    record Book(string Title, string Author);

    //record Book(string Title, string Author)
    //{
    //    public string Title { get; set; } = Title;
    //}

    class Program
    {
        static void Main()
        {
            var book = new Book("Title1", "Author1");
            //book.Title = "Title2";
            book = book with { Title = "Title2" };
            var json = JsonSerializer.Serialize(book);
            Console.WriteLine(json);

            var book2 = JsonSerializer.Deserialize<Book>(json);
            var isEqual = book == book2;
            Console.WriteLine($"book == book2: {isEqual}");
        }
    }
}
