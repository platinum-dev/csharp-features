using System.Diagnostics.CodeAnalysis;

_ = new Person
{
    FirstName = "Johny",
    LastName = "Gear"
};
_ = new Employee("Johny", "Gear", "Secret Department");

public class Person
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public int Age { get; init; }
}

public class Employee : Person
{
    public required string Department { get; init; }

    [SetsRequiredMembers]
    public Employee(string firstName, string lastName, string department)
    {
        FirstName = firstName;
        LastName = lastName;
        Department = department;
    }
}