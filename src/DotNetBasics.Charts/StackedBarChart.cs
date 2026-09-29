namespace DotNetBasics.Charts;

public class StackedBarChart
{
    private readonly IReadOnlyList<ChartSegment> Topics;
    private readonly StackedBarChartParams Parameters;

    public StackedBarChart(IEnumerable<ChartSegment> topics, StackedBarChartParams parameters = null)
    {
        Topics = ChartInput.ReadSegments(topics);
        Parameters = parameters ?? new StackedBarChartParams();
    }

    public string GenerateSvg()
    {
        return new StackedBarChartBuilder(Topics, Parameters).Build();
    }
}
