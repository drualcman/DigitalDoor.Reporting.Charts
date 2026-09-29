namespace DotNetBasics.Charts;

public class AreaChart
{
    private readonly IReadOnlyList<ChartSegment> Topics;
    private readonly AreaChartParams Parameters;

    public AreaChart(IEnumerable<ChartSegment> topics, AreaChartParams parameters = null)
    {
        Topics = ChartInput.ReadSegments(topics);
        Parameters = parameters ?? new AreaChartParams();
    }

    public string GenerateSvg()
    {
        return new AreaChartBuilder(Topics, Parameters).Build();
    }
}
