namespace DotNetBasics.Charts.Models;

public class AreaChartParams
{
    public AreaChartParams(
        int width = 600,
        int height = 300,
        string backgroundColor = "transparent",
        string areaFill = "#4E79A7",
        double areaOpacity = 0.4,
        string lineStroke = "#2F5B85",
        int lineWidth = 2,
        bool showPoints = true,
        int dotRadius = 4,
        bool showValues = false,
        bool showLabels = true,
        bool rotatedLabels = false,
        double labelRotationAngle = 45,
        int stepsY = 3,
        bool showYAxis = true,
        bool showGridLines = true,
        string axisColor = "#333333",
        string gridLineColor = "#DDDDDD",
        string title = "",
        int labelFontSize = 12,
        int titleFontSize = 16,
        Func<double, string> valueFormatter = null,
        string fontFamily = ChartFonts.DefaultFamily)
    {
        if (labelRotationAngle < 0 || labelRotationAngle > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(labelRotationAngle), labelRotationAngle, "Must be between 0 and 90 degrees.");
        }
        Width = width;
        Height = height;
        BackgroundColor = backgroundColor;
        AreaFill = areaFill;
        AreaOpacity = areaOpacity;
        LineStroke = lineStroke;
        LineWidth = lineWidth;
        ShowPoints = showPoints;
        DotRadius = dotRadius;
        ShowValues = showValues;
        ShowLabels = showLabels;
        RotatedLabels = rotatedLabels;
        LabelRotationAngle = labelRotationAngle;
        StepsY = stepsY;
        ShowYAxis = showYAxis;
        ShowGridLines = showGridLines;
        AxisColor = axisColor;
        GridLineColor = gridLineColor;
        Title = title;
        LabelFontSize = labelFontSize;
        TitleFontSize = titleFontSize;
        ValueFormatter = valueFormatter;
        FontFamily = fontFamily;
    }

    public int Width { get; init; }
    public int Height { get; init; }
    public string BackgroundColor { get; init; }
    public string AreaFill { get; init; }
    public double AreaOpacity { get; init; }
    public string LineStroke { get; init; }
    public int LineWidth { get; init; }
    public bool ShowPoints { get; init; }
    public int DotRadius { get; init; }
    public bool ShowValues { get; init; }
    public bool ShowLabels { get; init; }
    public bool RotatedLabels { get; init; }
    public double LabelRotationAngle { get; init; }
    public int StepsY { get; init; }
    public bool ShowYAxis { get; init; }
    public bool ShowGridLines { get; init; }
    public string AxisColor { get; init; }
    public string GridLineColor { get; init; }
    public string Title { get; init; }
    public int LabelFontSize { get; init; }
    public int TitleFontSize { get; init; }
    public Func<double, string> ValueFormatter { get; init; }
    public string FontFamily { get; init; }
}
