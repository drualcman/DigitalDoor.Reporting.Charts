namespace DotNetBasics.Charts;

public class RingPercentageChart
{
    private readonly double Percentage;
    private readonly RingParams Parameters;

    public RingPercentageChart(double percentage, RingParams parameters = null)
    {
        Percentage = percentage;
        Parameters = parameters ?? new RingParams();
    }

    public string GenerateSvg()
    {
        return new RingPercentageBuilder(Percentage, Parameters).Build();
    }
}
