namespace DotNetBasics.Charts.Models;

public class LineData
{
    public LineData(string name, string color, IEnumerable<string> values)
    {
        Name = name;
        Color = color;
        Values = values;
    }

    public string Name { get; set; }
    public string Color { get; set; }
    public IEnumerable<string> Values { get; set; }
}
