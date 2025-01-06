namespace _6._Global_Using_directive; 
public class Author
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public Author(string name, string lastname) =>
        (Name, LastName) = (name, lastname);
}
