namespace DotNetBasics.Charts.Models;

public class PieChartParams
{
    public PieChartParams(int width = 150, int height = 150,
        int separationOffset = 15, string title = "",
        IEnumerable<ChartColor> chartColours = null,
        bool showLabels = false, double centerTextSeparationPercentage = 0.85,
        bool separateBiggerByDefault = true, bool showBiggestLabel = false,
        bool showLegend = true, int labelFontSize = 12, int legendFontSize = 12,
        string fontFamily = ChartFonts.DefaultFamily)
    {
        Width = width;
        Height = height;
        SeparationOffset = separationOffset;
        Title = title;
        ChartColors = ChartColourPalette.CreateOrCopy(chartColours, separationOffset);
        ShowLabels = showLabels;
        CenterTextSeparationPercentage = centerTextSeparationPercentage;
        SeparateBiggerByDefault = separateBiggerByDefault;
        ShowBiggestLabel = showBiggestLabel;
        ShowLegend = showLegend;
        LabelFontSize = labelFontSize;
        LegendFontSize = legendFontSize;
        FontFamily = fontFamily;
    }

    public int Width { get; init; }
    public int Height { get; init; }
    public int SeparationOffset { get; init; }
    public string Title { get; set; }
    public List<ChartColor> ChartColors { get; set; }
    public int MaxColours => ChartColors.Count;
    public bool ShowLabels { get; set; }
    public double CenterTextSeparationPercentage { get; set; }
    public bool SeparateBiggerByDefault { get; init; }
    public bool ShowBiggestLabel { get; set; }
    public bool ShowLegend { get; set; }
    public int LabelFontSize { get; init; }
    public int LegendFontSize { get; init; }
    public string FontFamily { get; init; }
}
