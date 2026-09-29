namespace DotNetBasics.Charts.Helpers;

internal static class ChartFillReferenceValue
{
    public static double Get(IReadOnlyList<ChartSegment> segments, ColumnFillReference fillReference)
    {
        List<double> positiveValues = segments.Select(segment => Math.Max(0, segment.Value)).ToList();
        double result = 0;
        if (positiveValues.Count > 0)
        {
            result = fillReference == ColumnFillReference.TotalOfAllValues ? positiveValues.Sum() : positiveValues.Max();
        }
        return result;
    }
}
