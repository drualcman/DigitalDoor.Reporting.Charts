namespace DotNetBasics.Charts.Rendering.Lines;

internal static class LineCategoryAxisCalculator
{
    public const double PlotSidePadding = 0.05;
    private const double LabelGap = 10;
    private const double MinimumSpaceBetweenLabels = 6;
    private const double RotatedLineSpacingFactor = 1.2;

    public static LineCategoryAxis Calculate(LineChartData data, int positionCount, LineChartParams parameters, double plotWidth,
        CultureInfo culture)
    {
        List<string> customLabels = (data?.XLabels ?? Enumerable.Empty<string>()).ToList();
        List<string> labels = customLabels.Count > 0 ? customLabels :
            Enumerable.Range(1, positionCount).Select(position => position.ToString(culture)).ToList();
        double fontSize = parameters.FontSize;
        double usableWidth = plotWidth * (1 - PlotSidePadding * 2);
        double spacing = labels.Count > 1 ? usableWidth / (labels.Count - 1) : usableWidth;
        double widestLabel = labels.Select(label => ChartTextMeasure.EstimateWidth(label, fontSize)).DefaultIfEmpty(0).Max();
        double angleInRadians = ChartAngle.ToRadians(parameters.RotationAngleXLabel);
        bool isRotated = parameters.ShowX && Math.Sin(angleInRadians) > 0 &&
            (parameters.RotatedXLabels || widestLabel + MinimumSpaceBetweenLabels > spacing);
        double requiredSpacing = isRotated ?
            fontSize * RotatedLineSpacingFactor / Math.Sin(angleInRadians) :
            widestLabel + MinimumSpaceBetweenLabels;
        int labelStep = spacing >= requiredSpacing ? 1 : (int)Math.Ceiling(requiredSpacing / Math.Max(1, spacing));
        double bandHeight = !parameters.ShowX ? LabelGap : isRotated ?
            widestLabel * Math.Sin(angleInRadians) + fontSize * Math.Cos(angleInRadians) + LabelGap * 1.5 :
            fontSize + LabelGap * 1.5;
        double rightOverflow = isRotated ? widestLabel * Math.Cos(angleInRadians) : widestLabel / 2;
        return new LineCategoryAxis(labels, isRotated, labelStep, bandHeight, rightOverflow);
    }
}
