namespace DotNetBasics.Charts.Rendering.Lines;

internal static class LineSeriesReader
{
    public static List<LinePlotSeries> Read(LineChartData data, int maximumPointsPerLine, CultureInfo parsingCulture)
    {
        return (data?.Data ?? Enumerable.Empty<LineData>())
            .Where(lineData => lineData is not null)
            .Select(lineData => new LinePlotSeries(lineData, ReducePoints(ReadPoints(lineData, parsingCulture), maximumPointsPerLine)))
            .ToList();
    }

    public static int CountPositions(LineChartData data)
    {
        int longestSeries = (data?.Data ?? Enumerable.Empty<LineData>())
            .Where(lineData => lineData is not null)
            .Select(lineData => lineData.Values?.Count() ?? 0)
            .DefaultIfEmpty(0)
            .Max();
        return Math.Max(1, Math.Max(longestSeries, data?.XLabels?.Count() ?? 0));
    }

    private static List<LinePlotPoint> ReadPoints(LineData lineData, CultureInfo parsingCulture)
    {
        List<string> values = (lineData.Values ?? Enumerable.Empty<string>()).ToList();
        List<LinePlotPoint> points = new List<LinePlotPoint>();
        for (int valueIndex = 0; valueIndex < values.Count; valueIndex++)
        {
            if (double.TryParse(values[valueIndex], NumberStyles.Float | NumberStyles.AllowThousands, parsingCulture, out double value) &&
                !double.IsNaN(value) && !double.IsInfinity(value))
            {
                points.Add(new LinePlotPoint(valueIndex, value));
            }
        }
        return points;
    }

    private static IReadOnlyList<LinePlotPoint> ReducePoints(List<LinePlotPoint> points, int maximumPointsPerLine)
    {
        List<LinePlotPoint> result = points;
        if (maximumPointsPerLine > 1 && points.Count > maximumPointsPerLine)
        {
            int step = (int)Math.Ceiling((double)points.Count / maximumPointsPerLine);
            result = points.Where((point, position) => position % step == 0).ToList();
            if (result[result.Count - 1].Index != points[points.Count - 1].Index)
            {
                result.Add(points[points.Count - 1]);
            }
        }
        return result;
    }
}
