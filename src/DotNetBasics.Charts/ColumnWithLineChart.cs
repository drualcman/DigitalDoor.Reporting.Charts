namespace DotNetBasics.Charts;

public class ColumnWithLineChart
{
    private readonly ColumnWithLineChartData Data;
    private readonly ColumnWithLineChartParams Parameters;

    public ColumnWithLineChart(ColumnWithLineChartData data, ColumnWithLineChartParams parameters = null)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        Parameters = parameters ?? new ColumnWithLineChartParams();
    }

    public string GenerateSvg()
    {
        return new ColumnWithLineChartBuilder(Data, Parameters).Build();
    }
}
