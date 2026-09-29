namespace DotNetBasics.Charts.Helpers;

internal static class ChartInput
{
    public static IReadOnlyList<ChartSegment> ReadSegments(IEnumerable<ChartSegment> segments)
    {
        return (segments ?? Enumerable.Empty<ChartSegment>()).Where(segment => segment is not null).ToList();
    }
}
