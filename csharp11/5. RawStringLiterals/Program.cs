using System.Text;
using System.Text.Json;

string longMessage = """
    This is a long message.
    It has several lines.
        Some are indented
            more than others.
    Some should start at the first column.
    Some have "quoted text" in them.
    """;

var x1 = 0;
var y1 = 0;
var x2 = 1;
var y2 = 2;

var position = $$"""
You are at {{{x1}}, {{y1}}}
""";

var json = $$$"""
{
    "Points": [
        {
            "X": {{{x1}}},
            "Y": {{y1}}
        },
        {
            "X": {{x2}},
            "Y": {{y2}}
        }
    ]
}
""";

Console.WriteLine($"JSON:\n{json}");

//Console.WriteLine(JsonSerializer.Deserialize<Root>(json));

class Point
{
    public int X { get; set; }
    public int Y { get; set;}
}

class Root
{
    public List<Point> Points { get; set; }
    public override string ToString()
    {
        var count = Points.Count;
        StringBuilder builder = new StringBuilder();
        if (count == 0) builder.Append("No points");
        else
            for(var i = 0; i < count; ++i)
            {
                var rect = Points[i];
                builder.Append($"Point {i + 1} coordinates are: X={rect.X}, Y={rect.Y};\n");
            }
        return builder.ToString();
    }
}