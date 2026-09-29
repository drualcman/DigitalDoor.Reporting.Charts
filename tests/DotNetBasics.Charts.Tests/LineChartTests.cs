namespace DotNetBasics.Charts.Tests;

public class LineChartTests
{
    [Fact]
    public void SeriesKeepTheirOrderInTheLegend()
    {
        string svg = new LineChart(SampleData.CreateLines()).GenerateSvg();
        List<string> texts = SvgInspector.Texts(svg);

        Assert.True(texts.IndexOf("Line 1") < texts.IndexOf("Line 2"));
        Assert.True(texts.IndexOf("Line 2") < texts.IndexOf("Line 3"));
        Assert.Equal(new[] { "green", "blue", "red" },
            SvgInspector.Elements(svg, "polyline").Select(line => (string)line.Attribute("stroke")));
    }

    [Fact]
    public void ValueAxisUsesRoundTicks()
    {
        string svg = new LineChart(SampleData.CreateLines(), new LineChartParams(stepsY: 6)).GenerateSvg();

        Assert.Equal(new[] { "0", "20", "40", "60", "80", "100" }, SvgInspector.Texts(svg).Take(6));
    }

    [Fact]
    public void ValuesAreParsedWithTheGivenCulture()
    {
        LineChartData data = new LineChartData(new[] { new LineData("Spain", "blue", new[] { "1,5", "2,5", "texto", "3,5" }) });

        string svg = new LineChart(data, new LineChartParams(showLegend: false), new CultureInfo("es-ES")).GenerateSvg();
        string points = (string)SvgInspector.Elements(svg, "polyline").Single().Attribute("points");

        Assert.Equal(3, points.Split(' ').Length);
    }

    [Fact]
    public void AllPointsAndReferenceLinesAreDrawnWhenAsked()
    {
        LineChartParams parameters = new LineChartParams(pointOptions: new LineChartPointOptions(visibleAllPoints: true,
            visibleMaxPointLine: true, visibleMinPointLine: true), showLegend: false);

        string svg = new LineChart(SampleData.CreateLines(), parameters).GenerateSvg();

        Assert.Equal(21, SvgInspector.Elements(svg, "circle").Count);
        Assert.Equal(2, SvgInspector.Elements(svg, "line").Count(line => (string)line.Attribute("stroke-dasharray") == "5,5"));
    }

    [Fact]
    public void LabelsThatDoNotFitAreRotated()
    {
        LineChartData data = new LineChartData(SampleData.CreateLines().Data,
            Enumerable.Range(1, 7).Select(month => $"Month number {month}"));

        string svg = new LineChart(data, new LineChartParams(width: 400)).GenerateSvg();

        Assert.Contains(SvgInspector.Elements(svg, "text"), text => ((string)text.Attribute("transform") ?? "").StartsWith("rotate(45 "));
    }

    [Fact]
    public void RotationAngleMustBeBetweenZeroAndNinety()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new LineChartParams(rotationAngleXLabel: 120));
    }
}
