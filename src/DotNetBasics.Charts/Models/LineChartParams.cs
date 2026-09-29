namespace DotNetBasics.Charts.Models;

public class LineChartParams
{
    public LineChartParams(
        int width = 600,
        int height = 300,
        string backgroundColor = "transparent",
        string axisStroke = "black",
        int axisWidth = 2,
        string gridLineStroke = "black",
        int gridWidth = 1,
        string lineSeriesFill = "none",
        int lineSeriesWidth = 1,
        int dotRadius = 4,
        int stepsY = 3,
        bool showX = true,
        bool showY = true,
        bool showLegend = true,
        bool rotatedXLabels = false,
        double rotationAngleXLabel = 45,
        Func<LineData, string> legendLabel = null,
        LineChartPointOptions pointOptions = null,
        int maxPointPerLine = 50,
        bool showXLines = true,
        bool showYLines = true,
        int fontSize = 12,
        string fontFamily = ChartFonts.DefaultFamily)
    {
        if (rotationAngleXLabel < 0 || rotationAngleXLabel > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(rotationAngleXLabel), rotationAngleXLabel, "Must be between 0 and 90 degrees.");
        }
        Width = width;
        Height = height;
        BackgroundColor = backgroundColor;
        AxisStroke = axisStroke;
        AxisWidth = axisWidth;
        GridLineStroke = gridLineStroke;
        GridWidth = gridWidth;
        LineSeriesFill = lineSeriesFill;
        LineSeriesWidth = lineSeriesWidth;
        DotRadius = dotRadius;
        StepsY = stepsY;
        ShowX = showX;
        ShowY = showY;
        ShowLegend = showLegend;
        RotatedXLabels = rotatedXLabels;
        RotationAngleXLabel = rotationAngleXLabel;
        LegendLabel = legendLabel;
        PointOptions = pointOptions ?? new LineChartPointOptions();
        MaxPointPerLine = maxPointPerLine;
        ShowXLines = showXLines;
        ShowYLines = showYLines;
        FontSize = fontSize;
        FontFamily = fontFamily;
    }

    public int Width { get; }
    public int Height { get; }
    public string BackgroundColor { get; }
    public string AxisStroke { get; }
    public int AxisWidth { get; }
    public string GridLineStroke { get; }
    public int GridWidth { get; }
    public string LineSeriesFill { get; }
    public int LineSeriesWidth { get; }
    public int DotRadius { get; }
    public int StepsY { get; }
    public bool ShowX { get; }
    public bool ShowY { get; }
    public bool ShowLegend { get; }
    public bool RotatedXLabels { get; }
    public double RotationAngleXLabel { get; }
    public Func<LineData, string> LegendLabel { get; }
    public LineChartPointOptions PointOptions { get; }
    public int MaxPointPerLine { get; }
    public bool ShowXLines { get; }
    public bool ShowYLines { get; }
    public int FontSize { get; }
    public string FontFamily { get; }
}
