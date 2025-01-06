namespace _1._File_Scoped_Namespace
{
    public class Author
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public Author(string name, string lastname) =>
            (Name, LastName) = (name, lastname);
    }
}

