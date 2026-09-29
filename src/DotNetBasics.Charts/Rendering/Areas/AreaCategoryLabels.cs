namespace DotNetBasics.Charts.Rendering.Areas;

internal sealed class AreaCategoryLabels
{
    public const double LabelGap = 8;
    private const double MinimumSpaceBetweenLabels = 6;
    private const double HiddenLabelsBandHeight = 10;

    private readonly double FontSize;
    private readonly double RotationAngle;

    public AreaCategoryLabels(IReadOnlyList<ChartSegment> topics, AreaChartParams parameters, double estimatedPlotWidth)
    {
        FontSize = parameters.LabelFontSize;
        RotationAngle = parameters.LabelRotationAngle;
        double angleInRadians = ChartAngle.ToRadians(RotationAngle);
        double spacing = topics.Count > 1 ? estimatedPlotWidth / (topics.Count - 1) : estimatedPlotWidth;
        double widestLabel = topics.Select(topic => ChartTextMeasure.EstimateWidth(topic.Name, FontSize)).DefaultIfEmpty(0).Max();
        IsVisible = parameters.ShowLabels && topics.Count > 0;
        IsRotated = IsVisible && Math.Sin(angleInRadians) > 0 &&
            (parameters.RotatedLabels || widestLabel + MinimumSpaceBetweenLabels > spacing);
        double availableWidth = IsRotated ? widestLabel : Math.Max(0, spacing - MinimumSpaceBetweenLabels);
        Texts = topics.Select(topic => ChartTextMeasure.ShortenToFit(topic.Name, FontSize, availableWidth)).ToList();
        double widestText = Texts.Select(text => ChartTextMeasure.EstimateWidth(text, FontSize)).DefaultIfEmpty(0).Max();
        BandHeight = !IsVisible ? HiddenLabelsBandHeight : IsRotated ?
            widestText * Math.Sin(angleInRadians) + FontSize * Math.Cos(angleInRadians) + LabelGap * 1.5 :
            FontSize + LabelGap * 1.5;
        LeftOverflow = IsVisible && !IsRotated ? ChartTextMeasure.EstimateWidth(Texts[0], FontSize) / 2 : 0;
        RightOverflow = !IsVisible ? 0 : IsRotated ? widestText * Math.Cos(angleInRadians) :
            ChartTextMeasure.EstimateWidth(Texts[Texts.Count - 1], FontSize) / 2;
    }

    public IReadOnlyList<string> Texts { get; }
    public bool IsVisible { get; }
    public bool IsRotated { get; }
    public double BandHeight { get; }
    public double LeftOverflow { get; }
    public double RightOverflow { get; }

    public void Write(SvgCanvas canvas, AreaPlotArea area)
    {
        if (IsVisible)
        {
            for (int labelIndex = 0; labelIndex < Texts.Count; labelIndex++)
            {
                double x = area.GetX(labelIndex);
                SvgTextStyle style = IsRotated ?
                    new SvgTextStyle(FontSize, SvgTextAnchor.Start) { RotationAngle = RotationAngle } :
                    new SvgTextStyle(FontSize, SvgTextAnchor.Middle);
                double y = IsRotated ? area.Bottom + LabelGap : area.Bottom + LabelGap + FontSize * ChartTextMeasure.AscentFactor;
                canvas.Text(Texts[labelIndex], x, y, style);
            }
        }
    }
}
