namespace _2._Extended_Property_Patterns;
public class Author
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public Author(string name, string lastname) =>
        (Name, LastName) = (name, lastname);
}
