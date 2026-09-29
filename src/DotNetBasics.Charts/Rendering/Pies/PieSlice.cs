namespace DotNetBasics.Charts.Rendering.Pies;

internal sealed class PieSlice
{
    public PieSlice(ChartSegment segment, int segmentIndex, double share, double startAngle, double endAngle)
    {
        Segment = segment;
        SegmentIndex = segmentIndex;
        Share = share;
        StartAngle = startAngle;
        EndAngle = endAngle;
    }

    public ChartSegment Segment { get; }
    public int SegmentIndex { get; }
    public double Share { get; }
    public double StartAngle { get; }
    public double EndAngle { get; }
    public double MiddleAngle => (StartAngle + EndAngle) / 2;
    public bool IsHighlighted { get; set; }
    public bool IsExploded { get; set; }
}
