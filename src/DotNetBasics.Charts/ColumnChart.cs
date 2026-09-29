namespace DotNetBasics.Charts;

public class ColumnChart
{
    private readonly IReadOnlyList<ChartSegment> Topics;
    private readonly ColumnsBarChartParams Parameters;

    public ColumnChart(IEnumerable<ChartSegment> topics, ColumnsBarChartParams parameters = null)
    {
        Topics = ChartInput.ReadSegments(topics);
        Parameters = parameters ?? new ColumnsBarChartParams();
    }

    public string GenerateSvg()
    {
        return new ColumnChartBuilder(Topics, Parameters).Build();
    }
}
