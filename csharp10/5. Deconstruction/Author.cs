internal class Author
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public Author(string name, string lastname) =>
        (Name, LastName) = (name, lastname);

    public void Deconstruct(out string name, out string lastname) =>
        (name, lastname) = (Name, LastName);

    public override string ToString() =>
        $"{Name} {LastName}";
}
