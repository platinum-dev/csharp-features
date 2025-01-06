namespace _3._Constant_Interpolated_Strings;
public class Author
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public Author(string name, string lastname) =>
        (Name, LastName) = (name, lastname);
}