namespace DotNetBasics.Charts.Rendering.ColumnsWithLines;

internal static class ColumnWithLineSeriesWriter
{
    private const double LineWidth = 2;
    private const double PointRadius = 5;
    private const double LabelDistanceAbovePoint = 10;

    public static void WriteLines(SvgCanvas canvas, ColumnWithLineLayout layout, ColumnWithLineChartParams parameters)
    {
        if (parameters.ShowGranTotal)
        {
            WriteLine(canvas, layout.Items.Select(item => item.GrandTotalPoint).ToList(), parameters.GrandTotalLineColor);
        }
        if (parameters.ShowPrimaryValues)
        {
            WriteLine(canvas, layout.Items.Select(item => item.PrimaryPoint).ToList(), parameters.PrimaryPercentageLineColor);
        }
        if (parameters.ShowSecondaryValues)
        {
            WriteLine(canvas, layout.Items.Select(item => item.SecondaryPoint).ToList(), parameters.SecondaryPercentageLineColor);
        }
    }

    public static void WritePointsAndLabels(SvgCanvas canvas, ColumnWithLineItemGeometry item, ColumnWithLineChartParams parameters)
    {
        if (parameters.ShowGranTotal)
        {
            WritePoint(canvas, item.GrandTotalPoint, parameters.GrandTotalLineColor,
                parameters.BigTotalValueLabelFormatter?.Invoke(item.Item) ?? FormatPercentage(item.GrandTotalPercentage), parameters.LabelFontSize);
        }
        if (parameters.ShowPrimaryValues)
        {
            WritePoint(canvas, item.PrimaryPoint, parameters.PrimaryPercentageLineColor,
                parameters.PrimaryValueLabelFormatter?.Invoke(item.Item) ?? FormatPercentage(item.PrimaryPercentage), parameters.LabelFontSize);
        }
        if (parameters.ShowSecondaryValues)
        {
            WritePoint(canvas, item.SecondaryPoint, parameters.SecondaryPercentageLineColor,
                parameters.SecondaryValueLabelFormatter?.Invoke(item.Item) ?? FormatPercentage(item.SecondaryPercentage), parameters.LabelFontSize);
        }
    }

    private static void WriteLine(SvgCanvas canvas, List<SvgPoint> points, string colour)
    {
        canvas.Polyline(points, new SvgStroke(colour, LineWidth), "none");
    }

    private static void WritePoint(SvgCanvas canvas, SvgPoint point, string colour, string label, double fontSize)
    {
        canvas.Circle(point.X, point.Y, PointRadius, colour);
        canvas.Text(label, point.X, point.Y - LabelDistanceAbovePoint, new SvgTextStyle(fontSize, SvgTextAnchor.Middle));
    }

    private static string FormatPercentage(double percentage)
    {
        return $"{((int)percentage).ToString(CultureInfo.InvariantCulture)}%";
    }
}
