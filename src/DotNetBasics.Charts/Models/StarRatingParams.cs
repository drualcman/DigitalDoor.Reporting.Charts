namespace DotNetBasics.Charts.Models;

public class StarRatingParams
{
    public StarRatingParams(
        int starCount = 5,
        int starSize = 16,
        string fillColor = "#FFD700",
        string emptyColor = "#E0E0E0",
        string borderColor = "#B8860B",
        bool showCount = true,
        bool showPercentage = false,
        int rowHeight = 24,
        int width = 400,
        int labelFontSize = 12,
        string title = "",
        int titleFontSize = 16,
        Func<double, string> countFormatter = null,
        Func<double, string> percentageFormatter = null,
        string fontFamily = ChartFonts.DefaultFamily)
    {
        StarCount = starCount;
        StarSize = starSize;
        FillColor = fillColor;
        EmptyColor = emptyColor;
        BorderColor = borderColor;
        ShowCount = showCount;
        ShowPercentage = showPercentage;
        RowHeight = rowHeight;
        Width = width;
        LabelFontSize = labelFontSize;
        Title = title;
        TitleFontSize = titleFontSize;
        CountFormatter = countFormatter;
        PercentageFormatter = percentageFormatter;
        FontFamily = fontFamily;
    }

    public int StarCount { get; init; }
    public int StarSize { get; init; }
    public string FillColor { get; init; }
    public string EmptyColor { get; init; }
    public string BorderColor { get; init; }
    public bool ShowCount { get; init; }
    public bool ShowPercentage { get; init; }
    public int RowHeight { get; init; }
    public int Width { get; init; }
    public int LabelFontSize { get; init; }
    public string Title { get; init; }
    public int TitleFontSize { get; init; }
    public Func<double, string> CountFormatter { get; init; }
    public Func<double, string> PercentageFormatter { get; init; }
    public string FontFamily { get; init; }
}
