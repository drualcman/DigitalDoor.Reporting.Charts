namespace DotNetBasics.Charts.Rendering.Pies;

internal static class PieSliceCalculator
{
    private const double FullTurnInDegrees = 360;

    public static List<PieSlice> Calculate(IReadOnlyList<ChartSegment> segments, bool separateHighlightedSlice)
    {
        if (segments.Count(segment => segment.IsSelected) > 1)
        {
            throw new ArgumentException("Only one pie segment can be selected.", nameof(segments));
        }
        double total = segments.Sum(segment => Math.Max(0, segment.Value));
        List<PieSlice> slices = new List<PieSlice>();
        double startAngle = 0;
        for (int segmentIndex = 0; segmentIndex < segments.Count; segmentIndex++)
        {
            double share = total > 0 ? Math.Max(0, segments[segmentIndex].Value) / total : 0;
            double endAngle = startAngle + share * FullTurnInDegrees;
            slices.Add(new PieSlice(segments[segmentIndex], segmentIndex, share, startAngle, endAngle));
            startAngle = endAngle;
        }
        PieSlice highlightedSlice = slices.FirstOrDefault(slice => slice.Segment.IsSelected && slice.Share > 0) ??
            slices.Where(slice => slice.Share > 0).OrderByDescending(slice => slice.Share).FirstOrDefault();
        if (highlightedSlice is not null)
        {
            highlightedSlice.IsHighlighted = true;
            highlightedSlice.IsExploded = separateHighlightedSlice && highlightedSlice.Share < 1;
        }
        return slices;
    }
}
