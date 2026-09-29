namespace DotNetBasics.Charts;

public class LineChart
{
    private readonly LineChartData Data;
    private readonly LineChartParams Parameters;
    private readonly CultureInfo ParsingCulture;

    public LineChart(LineChartData data, LineChartParams parameters = null, CultureInfo parsingCulture = null)
    {
        Data = data ?? new LineChartData(null);
        Parameters = parameters ?? new LineChartParams();
        ParsingCulture = parsingCulture ?? CultureInfo.InvariantCulture;
    }

    public string GenerateSvg()
    {
        return new LineChartBuilder(Data, Parameters, ParsingCulture).Build();
    }
}
