var author = new Author("Andrei", "Fedotov");
Console.WriteLine(author);

(string name1, string lastName) = author;
Console.WriteLine(name1);
Console.WriteLine(lastName);

var name2 = string.Empty;
var lastname2 = string.Empty;
(name2, lastname2) = author;
Console.WriteLine(name2);
Console.WriteLine(lastname2);

//== C# 10 feature


var lastname3 = string.Empty;
(var name3, lastname3) = author;
Console.WriteLine(name3);
Console.WriteLine(lastname3);


