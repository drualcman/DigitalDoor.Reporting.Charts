namespace DotNetBasics.Charts;

public class PieChart
{
    private readonly IReadOnlyList<ChartSegment> Segments;
    private readonly PieChartParams Parameters;

    public PieChart(IEnumerable<ChartSegment> segments, PieChartParams parameters = null)
    {
        Segments = ChartInput.ReadSegments(segments);
        Parameters = parameters ?? new PieChartParams();
    }

    public string GenerateSvg()
    {
        return new PieChartBuilder(Segments, Parameters).Build();
    }
}
