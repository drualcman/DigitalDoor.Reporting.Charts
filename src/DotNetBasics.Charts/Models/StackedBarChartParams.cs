namespace DotNetBasics.Charts.Models;

public class StackedBarChartParams
{
    public StackedBarChartParams(
        StackedBarOrientation orientation = StackedBarOrientation.Horizontal,
        int thickness = 40,
        int length = 600,
        string backgroundColour = "#D3D3D3",
        IEnumerable<ChartColor> chartColours = null,
        bool showValues = false,
        StackedBarLabelSide labelSide = StackedBarLabelSide.After,
        StackedBarLabelAlignment labelAlignment = StackedBarLabelAlignment.Start,
        int labelFontSize = 12,
        double minimumLabelShare = 0.03,
        double total = 0,
        Func<double, string> valueFormatter = null,
        string fontFamily = ChartFonts.DefaultFamily)
    {
        Orientation = orientation;
        Thickness = thickness;
        Length = length;
        BackgroundColour = backgroundColour;
        ChartColors = ChartColourPalette.CreateOrCopy(chartColours, ChartColourPalette.DefaultHueSeparation);
        ShowValues = showValues;
        LabelSide = labelSide;
        LabelAlignment = labelAlignment;
        LabelFontSize = labelFontSize;
        MinimumLabelShare = minimumLabelShare;
        Total = total;
        ValueFormatter = valueFormatter;
        FontFamily = fontFamily;
    }

    public StackedBarOrientation Orientation { get; init; }
    public int Thickness { get; init; }
    public int Length { get; init; }
    public string BackgroundColour { get; init; }
    public bool ShowValues { get; init; }
    public StackedBarLabelSide LabelSide { get; init; }
    public StackedBarLabelAlignment LabelAlignment { get; init; }
    public int LabelFontSize { get; init; }
    public double MinimumLabelShare { get; init; }
    public double Total { get; init; }
    public Func<double, string> ValueFormatter { get; init; }
    public string FontFamily { get; init; }
    public List<ChartColor> ChartColors { get; set; }
    public int MaxColours => ChartColors.Count;
}
