namespace DotNetBasics.Charts.Rendering.Pies;

internal static class PieCalloutWriter
{
    private const double BoxPadding = 5;
    private const double BoxCornerRadius = 5;
    private const string BoxBackground = "#FFFFFF";
    private const string BoxBorder = "#DDDDDD";
    private const string TextColour = "#000000";

    public static void Write(SvgCanvas canvas, PieSlice slice, SvgPoint anchor, double fontSize, double documentWidth, double documentHeight)
    {
        string text = slice.Segment.SetTitleTopic is not null ? slice.Segment.ShowTitle() : ChartValueFormat.FormatPercentage(slice.Share);
        double boxWidth = ChartTextMeasure.EstimateWidth(text, fontSize) + BoxPadding * 2;
        double boxHeight = fontSize + BoxPadding * 2;
        double centreX = KeepInside(anchor.X, boxWidth, documentWidth);
        double centreY = KeepInside(anchor.Y, boxHeight, documentHeight);
        canvas.Rect(centreX - boxWidth / 2, centreY - boxHeight / 2, boxWidth, boxHeight, BoxBackground, BoxCornerRadius,
            new SvgStroke(BoxBorder, 1));
        canvas.Text(text, centreX, ChartTextMeasure.GetCentredBaseline(centreY, fontSize),
            new SvgTextStyle(fontSize, SvgTextAnchor.Middle, TextColour));
    }

    private static double KeepInside(double centre, double size, double documentSize)
    {
        double halfSize = size / 2 + 1;
        return documentSize > size ? Math.Min(Math.Max(centre, halfSize), documentSize - halfSize) : documentSize / 2;
    }
}
