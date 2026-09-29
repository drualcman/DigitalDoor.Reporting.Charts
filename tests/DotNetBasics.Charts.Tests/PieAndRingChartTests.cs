namespace DotNetBasics.Charts.Tests;

public class PieAndRingChartTests
{
    [Fact]
    public void PieSlicesShowTheirShareOfTheTotal()
    {
        List<ChartSegment> segments = new List<ChartSegment> { new ChartSegment("Paid", 30), new ChartSegment("Pending", 10) };

        string svg = new PieChart(segments).GenerateSvg();

        Assert.Equal(2, SvgInspector.Elements(svg, "path").Count);
        Assert.Contains("Paid 75.00%", SvgInspector.Texts(svg));
        Assert.Contains("Pending 25.00%", SvgInspector.Texts(svg));
    }

    [Fact]
    public void TheBiggestSliceIsTheOnlyOneSeparated()
    {
        string svg = new PieChart(SampleData.CreatePercentages()).GenerateSvg();

        Assert.Single(SvgInspector.Elements(svg, "path"), path => path.Attribute("transform") is not null);
    }

    [Fact]
    public void ASingleSliceIsAFullCircle()
    {
        string svg = new PieChart(new[] { new ChartSegment("All", 5) }, new PieChartParams(showBiggestLabel: true)).GenerateSvg();
        string pathData = (string)SvgInspector.Elements(svg, "path").Single().Attribute("d");

        Assert.Equal(2, pathData.Count(character => character == 'A'));
        Assert.Contains("100.00%", SvgInspector.Texts(svg));
    }

    [Fact]
    public void OnlyOneSliceCanBeSelected()
    {
        List<ChartSegment> segments = new List<ChartSegment>
        {
            new ChartSegment("A", 1) { IsSelected = true },
            new ChartSegment("B", 1) { IsSelected = true }
        };

        Assert.Throws<ArgumentException>(() => new PieChart(segments).GenerateSvg());
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(40, 1)]
    [InlineData(100, 1)]
    public void RingDrawsTheProgressOnlyWhenThereIsSome(double percentage, int progressElements)
    {
        string svg = new RingPercentageChart(percentage).GenerateSvg();
        int drawnProgress = SvgInspector.Elements(svg, "path").Count + SvgInspector.Elements(svg, "circle").Count - 1;

        Assert.Equal(progressElements, drawnProgress);
        Assert.Contains($"{percentage.ToString(CultureInfo.InvariantCulture)}%", SvgInspector.Texts(svg));
    }

    [Fact]
    public void RingGradientIdentifiersAreStableAndDifferentPerRing()
    {
        string firstRing = new RingPercentageChart(40).GenerateSvg();
        string sameRing = new RingPercentageChart(40).GenerateSvg();
        string otherRing = new RingPercentageChart(41).GenerateSvg();

        Assert.Equal(firstRing, sameRing);
        Assert.NotEqual(GetGradientId(firstRing), GetGradientId(otherRing));
    }

    private static string GetGradientId(string svg)
    {
        return (string)SvgInspector.Elements(svg, "linearGradient").Single().Attribute("id");
    }
}
