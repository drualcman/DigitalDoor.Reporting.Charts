namespace DotNetBasics.Charts.Rendering.Areas;

internal static class AreaSeriesWriter
{
    private const string DotBorderColour = "#FFFFFF";
    private const double ValueGapAboveThePoint = 4;

    public static void WriteArea(SvgCanvas canvas, AreaPlotArea area, IReadOnlyList<double> values, AreaChartParams parameters)
    {
        List<SvgPoint> linePoints = GetLinePoints(area, values);
        if (linePoints.Count > 1)
        {
            double baselineY = area.Bottom;
            List<SvgPoint> polygonPoints = new List<SvgPoint>(linePoints)
            {
                new SvgPoint(linePoints[linePoints.Count - 1].X, baselineY),
                new SvgPoint(linePoints[0].X, baselineY)
            };
            canvas.Polygon(polygonPoints, new SvgPolygonStyle(parameters.AreaFill) { FillOpacity = parameters.AreaOpacity });
        }
    }

    public static void WriteLineAndPoints(SvgCanvas canvas, AreaPlotArea area, IReadOnlyList<double> values, AreaChartParams parameters)
    {
        if (parameters.LineWidth > 0)
        {
            canvas.Polyline(GetLinePoints(area, values), new SvgStroke(parameters.LineStroke, parameters.LineWidth), "none");
        }
        for (int valueIndex = 0; valueIndex < values.Count; valueIndex++)
        {
            SvgPoint point = new SvgPoint(area.GetX(valueIndex), area.GetY(values[valueIndex]));
            if (parameters.ShowPoints)
            {
                canvas.Circle(point.X, point.Y, parameters.DotRadius, parameters.LineStroke, new SvgStroke(DotBorderColour, 1));
            }
            if (parameters.ShowValues)
            {
                double gapAboveThePoint = (parameters.ShowPoints ? Math.Max(0, parameters.DotRadius) : 0) + ValueGapAboveThePoint;
                canvas.Text(ChartValueFormat.Format(values[valueIndex], parameters.ValueFormatter), point.X, point.Y - gapAboveThePoint,
                    new SvgTextStyle(parameters.LabelFontSize, GetValueAnchor(valueIndex, values.Count)));
            }
        }
    }

    private static string GetValueAnchor(int valueIndex, int valueCount)
    {
        string result = SvgTextAnchor.Middle;
        if (valueCount > 1 && valueIndex == 0)
        {
            result = SvgTextAnchor.Start;
        }
        else if (valueCount > 1 && valueIndex == valueCount - 1)
        {
            result = SvgTextAnchor.End;
        }
        return result;
    }

    private static List<SvgPoint> GetLinePoints(AreaPlotArea area, IReadOnlyList<double> values)
    {
        List<SvgPoint> result = values.Select((value, valueIndex) => new SvgPoint(area.GetX(valueIndex), area.GetY(value))).ToList();
        if (values.Count == 1)
        {
            double y = area.GetY(values[0]);
            result = new List<SvgPoint> { new SvgPoint(area.Left, y), new SvgPoint(area.Right, y) };
        }
        return result;
    }
}
