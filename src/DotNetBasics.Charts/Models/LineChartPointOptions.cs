namespace DotNetBasics.Charts.Models;

public class LineChartPointOptions
{
    public LineChartPointOptions(bool visibleAllPoints = false, bool visibleMaxPoint = true, bool visibleMinPoint = true,
        bool visibleMaxPointLine = false, bool visibleMinPointLine = false)
    {
        VisibleAllPoints = visibleAllPoints;
        VisibleMaxPoint = visibleMaxPoint;
        VisibleMinPoint = visibleMinPoint;
        VisibleMaxPointLine = visibleMaxPointLine;
        VisibleMinPointLine = visibleMinPointLine;
    }

    public bool VisibleAllPoints { get; }
    public bool VisibleMaxPoint { get; }
    public bool VisibleMinPoint { get; }
    public bool VisibleMaxPointLine { get; }
    public bool VisibleMinPointLine { get; }
}
