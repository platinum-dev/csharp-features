namespace _3._Constant_Interpolated_Strings;
public class Book
{
    public string Title { get; set; }
    public Author Author { get; set; }
    public Book(string title, Author author) =>
        (Title, Author) = (title, author);

    public string ThankYouMessage()
    {
        const string message = "Thank you for buying the book!";
        const string thankYouMessage = $"{message} Enjoy";
        return thankYouMessage;
    }
}