namespace DotNetBasics.Charts.Rendering.Titles;

internal static class ChartTitleWriter
{
    private const double TitleSpacing = 12;

    public static double GetHeight(string title, double titleFontSize)
    {
        return HasTitle(title) ? titleFontSize + TitleSpacing : 0;
    }

    public static void Write(SvgCanvas canvas, string title, double titleFontSize, double documentWidth)
    {
        if (HasTitle(title))
        {
            string visibleTitle = ChartTextMeasure.ShortenToFit(title, titleFontSize, documentWidth);
            canvas.Text(visibleTitle, documentWidth / 2, titleFontSize, new SvgTextStyle(titleFontSize, SvgTextAnchor.Middle) { IsBold = true });
        }
    }

    private static bool HasTitle(string title)
    {
        return !string.IsNullOrWhiteSpace(title);
    }
}
