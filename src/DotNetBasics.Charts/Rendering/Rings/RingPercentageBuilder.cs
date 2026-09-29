namespace DotNetBasics.Charts.Rendering.Rings;

internal sealed class RingPercentageBuilder
{
    private const double ViewBoxSize = 120;
    private const double RingRadius = 52;
    private const double DefaultSize = 120;

    private readonly double Percentage;
    private readonly RingParams Parameters;

    public RingPercentageBuilder(double percentage, RingParams parameters)
    {
        Percentage = double.IsNaN(percentage) ? 0 : Math.Max(0, Math.Min(100, percentage));
        Parameters = parameters;
    }

    public string Build()
    {
        double width = Parameters.Width > 0 ? Parameters.Width : DefaultSize;
        double height = Parameters.Height > 0 ? Parameters.Height : width;
        SvgPoint centre = new SvgPoint(ViewBoxSize / 2, ViewBoxSize / 2);
        string gradientId = StableIdentifier.Create("ring-gradient",
            $"{Parameters.FromColor}|{Parameters.ToColor}|{Percentage.ToString(CultureInfo.InvariantCulture)}");
        SvgCanvas canvas = new SvgCanvas();
        canvas.VerticalLinearGradient(gradientId, Parameters.FromColor, Parameters.ToColor, centre.Y - RingRadius, centre.Y + RingRadius);
        canvas.Circle(centre.X, centre.Y, RingRadius, "none", new SvgStroke(Parameters.CircunferenceColour, Parameters.StrokeWidth));
        SvgStroke progressStroke = new SvgStroke($"url(#{gradientId})", Parameters.StrokeWidth);
        if (Percentage >= 100)
        {
            canvas.Circle(centre.X, centre.Y, RingRadius, "none", progressStroke);
        }
        else if (Percentage > 0)
        {
            canvas.Path(CreateProgressArc(centre, Percentage / 100 * 360), "none", progressStroke);
        }
        double fontSize = Parameters.FontSizeRatio > 0 ? ViewBoxSize / Parameters.FontSizeRatio : ViewBoxSize / 3.5;
        canvas.Text($"{Percentage.ToString("0.##", CultureInfo.InvariantCulture)}%", centre.X,
            ChartTextMeasure.GetCentredBaseline(centre.Y, fontSize), new SvgTextStyle(fontSize, SvgTextAnchor.Middle, Parameters.LabelColor));
        return canvas.ToDocument(width, height, ViewBoxSize, ViewBoxSize, Parameters.FontFamily);
    }

    private static string CreateProgressArc(SvgPoint centre, double sweepInDegrees)
    {
        double startAngle = -90;
        SvgPoint start = PieArcPath.GetPoint(centre, RingRadius, startAngle);
        SvgPoint end = PieArcPath.GetPoint(centre, RingRadius, startAngle + sweepInDegrees);
        int largeArcFlag = sweepInDegrees > 180 ? 1 : 0;
        return $"M {SvgNumber.Format(start.X)} {SvgNumber.Format(start.Y)} A {SvgNumber.Format(RingRadius)} {SvgNumber.Format(RingRadius)} " +
            $"0 {largeArcFlag} 1 {SvgNumber.Format(end.X)} {SvgNumber.Format(end.Y)}";
    }
}
