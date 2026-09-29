namespace DotNetBasics.Charts.Tests;

public class ColumnWithLineAndColourTests
{
    [Fact]
    public void TitleLegendAndPercentagesAreWrittenInTheSvg()
    {
        string svg = new ColumnWithLineChart(SampleData.CreateColumnsWithLines()).GenerateSvg();
        List<string> texts = SvgInspector.Texts(svg);

        Assert.Contains("Total users", texts);
        Assert.Contains("Active Users", texts);
        Assert.Contains("% of Grand Total", texts);
        Assert.Contains("52%", texts);
        Assert.Contains("JAN", texts);
    }

    [Fact]
    public void SecondaryPercentageLegendUsesTheSecondaryLineColour()
    {
        ColumnWithLineChartParams parameters = new ColumnWithLineChartParams { ShowSecondaryValues = true, SecondaryPercentageLineColor = "#123456" };

        string svg = new ColumnWithLineChart(SampleData.CreateColumnsWithLines(), parameters).GenerateSvg();

        Assert.Contains("% Non Active Users", SvgInspector.Texts(svg));
        Assert.Contains(SvgInspector.Elements(svg, "polyline"), line => (string)line.Attribute("stroke") == "#123456");
    }

    [Fact]
    public void ZeroValuesDoNotBreakTheChart()
    {
        ColumnWithLineChartData data = new ColumnWithLineChartData(new[] { new ColumnDataItem("A", 0, 0), new ColumnDataItem("B", 0, 0) });

        string svg = new ColumnWithLineChart(data).GenerateSvg();

        Assert.Contains("0%", SvgInspector.Texts(svg));
    }

    [Theory]
    [InlineData("#000000", "#FFFFFF")]
    [InlineData("#FFFFFF", "#000000")]
    [InlineData("#FFD700", "#000000")]
    [InlineData("rgb(20, 40, 120)", "#FFFFFF")]
    [InlineData("hsl(0, 100%, 90%)", "#000000")]
    [InlineData("navy", "#FFFFFF")]
    public void ForegroundContrastsWithTheBackground(string background, string expectedForeground)
    {
        Assert.Equal(expectedForeground, new ChartColor(background).Foreground);
    }
}
