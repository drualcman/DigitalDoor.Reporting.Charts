namespace DotNetBasics.Charts.Models;

public class ColumnsBarChartParams
{
    public const double VERTICAL_LABEL_ANGLE = -90;

    public ColumnsBarChartParams(
        string backgroundColour = "#D3D3D3",
        int thickness = 20,
        int dimension = 100,
        bool showValues = false,
        IEnumerable<ChartColor> chartColours = null,
        int gap = 5,
        int maxWidth = 600,
        bool rotatedLabels = false,
        double labelRotationAngle = VERTICAL_LABEL_ANGLE,
        int labelFontSize = 12,
        ColumnLabelPlacement? labelPlacement = null,
        ColumnFillReference fillReference = ColumnFillReference.HighestValue,
        Func<double, string> valueFormatter = null,
        string fontFamily = ChartFonts.DefaultFamily)
    {
        BackgroundColour = backgroundColour;
        Thickness = thickness;
        Dimension = dimension;
        ShowValues = showValues;
        ChartColors = ChartColourPalette.CreateOrCopy(chartColours, ChartColourPalette.DefaultHueSeparation);
        Gap = gap;
        MaxWidth = maxWidth;
        RotatedLabels = rotatedLabels;
        LabelRotationAngle = labelRotationAngle;
        LabelFontSize = labelFontSize;
        LabelPlacement = labelPlacement;
        FillReference = fillReference;
        ValueFormatter = valueFormatter;
        FontFamily = fontFamily;
    }

    public string BackgroundColour { get; init; }
    public int Thickness { get; init; }
    public int Dimension { get; init; }
    public int MaxWidth { get; init; }
    public int Gap { get; init; }
    public bool ShowValues { get; init; }
    public bool RotatedLabels { get; init; }
    public double LabelRotationAngle { get; init; }
    public int LabelFontSize { get; init; }
    public ColumnLabelPlacement? LabelPlacement { get; init; }
    public ColumnFillReference FillReference { get; init; }
    public Func<double, string> ValueFormatter { get; init; }
    public string FontFamily { get; init; }
    public List<ChartColor> ChartColors { get; set; }
    public int MaxColours => ChartColors.Count;
}
