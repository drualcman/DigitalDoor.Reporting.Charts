namespace DotNetBasics.Charts.Rendering.Lines;

internal static class LineSeriesWriter
{
    private const string DotBorderColour = "white";
    private const string MaximumLineColour = "green";
    private const string MinimumLineColour = "red";
    private const string ReferenceLineDashes = "5,5";

    public static void Write(SvgCanvas canvas, LinePlotArea area, IReadOnlyList<LinePlotSeries> series, LineChartParams parameters)
    {
        foreach (LinePlotSeries lineSeries in series)
        {
            List<SvgPoint> points = lineSeries.Points.Select(area.GetPoint).ToList();
            canvas.Polyline(points, new SvgStroke(lineSeries.Colour, parameters.LineSeriesWidth), parameters.LineSeriesFill);
        }
        foreach (LinePlotSeries lineSeries in series)
        {
            foreach (LinePlotPoint point in GetVisiblePoints(lineSeries, parameters.PointOptions))
            {
                SvgPoint position = area.GetPoint(point);
                canvas.Circle(position.X, position.Y, parameters.DotRadius, lineSeries.Colour, new SvgStroke(DotBorderColour, 1));
            }
        }
        List<double> maximumValues = series.Where(lineSeries => lineSeries.MaximumPoint is not null)
            .Select(lineSeries => lineSeries.MaximumPoint.Value).ToList();
        List<double> minimumValues = series.Where(lineSeries => lineSeries.MinimumPoint is not null)
            .Select(lineSeries => lineSeries.MinimumPoint.Value).ToList();
        if (parameters.PointOptions.VisibleMaxPointLine && maximumValues.Count > 0)
        {
            WriteReferenceLine(canvas, area, maximumValues.Max(), MaximumLineColour);
        }
        if (parameters.PointOptions.VisibleMinPointLine && minimumValues.Count > 0)
        {
            WriteReferenceLine(canvas, area, minimumValues.Min(), MinimumLineColour);
        }
    }

    private static IEnumerable<LinePlotPoint> GetVisiblePoints(LinePlotSeries lineSeries, LineChartPointOptions pointOptions)
    {
        IEnumerable<LinePlotPoint> result = lineSeries.Points.Count == 1 ? lineSeries.Points : Enumerable.Empty<LinePlotPoint>();
        if (pointOptions.VisibleAllPoints)
        {
            result = lineSeries.Points;
        }
        else if (lineSeries.Points.Count > 1)
        {
            List<LinePlotPoint> highlightedPoints = new List<LinePlotPoint>();
            if (pointOptions.VisibleMaxPoint && lineSeries.MaximumPoint is not null)
            {
                highlightedPoints.Add(lineSeries.MaximumPoint);
            }
            if (pointOptions.VisibleMinPoint && lineSeries.MinimumPoint is not null)
            {
                highlightedPoints.Add(lineSeries.MinimumPoint);
            }
            result = highlightedPoints.Distinct();
        }
        return result;
    }

    private static void WriteReferenceLine(SvgCanvas canvas, LinePlotArea area, double value, string colour)
    {
        double y = area.GetY(value);
        canvas.Line(new SvgPoint(area.Left, y), new SvgPoint(area.Right, y), new SvgStroke(colour, 1, ReferenceLineDashes));
    }
}
