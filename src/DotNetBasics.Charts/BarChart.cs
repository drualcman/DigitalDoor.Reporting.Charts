namespace DotNetBasics.Charts;

public class BarChart
{
    private readonly IReadOnlyList<ChartSegment> Topics;
    private readonly ColumnsBarChartParams Parameters;

    public BarChart(IEnumerable<ChartSegment> topics, ColumnsBarChartParams parameters = null)
    {
        Topics = ChartInput.ReadSegments(topics);
        Parameters = parameters ?? new ColumnsBarChartParams();
    }

    public string GenerateSvg()
    {
        return new BarChartBuilder(Topics, Parameters).Build();
    }
}
