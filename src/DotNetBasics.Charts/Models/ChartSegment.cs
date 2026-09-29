namespace DotNetBasics.Charts.Models;

public class ChartSegment
{
    public ChartSegment()
    {
    }

    public ChartSegment(string name, double value, string chartColor = null)
    {
        Name = name;
        Value = value;
        ChartColor = chartColor;
    }

    public string Name { get; set; }
    public double Value { get; set; }
    public bool IsSelected { get; set; }
    public string ChartColor { get; set; }
    public string LabelColor { get; set; }
    public Func<ChartSegment, string> SetTitleTopic { get; set; }

    internal string ShowTitle()
    {
        return SetTitleTopic is not null ? SetTitleTopic(this) : $"{Name}: {Value.ToString(CultureInfo.InvariantCulture)}";
    }
}
