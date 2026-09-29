namespace DotNetBasics.Charts.Tests;

public class BarAndColumnChartTests
{
    [Fact]
    public void BarsAreFilledAgainstTheHighestValue()
    {
        List<ChartSegment> segments = new List<ChartSegment> { new ChartSegment("A", 100, "#FF0000"), new ChartSegment("B", 25, "#00FF00") };

        string svg = new BarChart(segments, new ColumnsBarChartParams(maxWidth: 400)).GenerateSvg();
        List<XElement> filledBars = SvgInspector.Elements(svg, "rect").Where(rect => (string)rect.Attribute("fill") != "#D3D3D3").ToList();

        Assert.Equal(2, filledBars.Count);
        Assert.Equal(SvgInspector.ReadNumber(filledBars[0], "width") / 4, SvgInspector.ReadNumber(filledBars[1], "width"), 3);
    }

    [Fact]
    public void BarsCanBeFilledAsAShareOfTheTotal()
    {
        List<ChartSegment> segments = new List<ChartSegment> { new ChartSegment("A", 75, "#FF0000"), new ChartSegment("B", 25, "#00FF00") };

        string svg = new BarChart(segments, new ColumnsBarChartParams(maxWidth: 400, fillReference: ColumnFillReference.TotalOfAllValues)).GenerateSvg();
        XElement firstFilledBar = SvgInspector.Elements(svg, "rect").First(rect => (string)rect.Attribute("fill") == "#FF0000");

        Assert.Equal(400 * 0.75 * 0.75, SvgInspector.ReadNumber(firstFilledBar, "width"), 3);
    }

    [Fact]
    public void ValuesUseTheValueFormatter()
    {
        List<ChartSegment> segments = new List<ChartSegment> { new ChartSegment("A", 1234.5) };

        string svg = new ColumnChart(segments, new ColumnsBarChartParams(showValues: true,
            valueFormatter: value => value.ToString("N2", new CultureInfo("es-ES")) + " €")).GenerateSvg();

        Assert.Contains("1.234,50 €", SvgInspector.Texts(svg));
    }

    [Fact]
    public void LongColumnLabelsAreShortenedToTheirSlot()
    {
        List<ChartSegment> segments = Enumerable.Range(1, 6).Select(index => new ChartSegment($"A very long branch name {index}", index)).ToList();

        string svg = new ColumnChart(segments, new ColumnsBarChartParams(maxWidth: 300)).GenerateSvg();

        Assert.All(SvgInspector.Texts(svg), label => Assert.EndsWith("…", label));
    }

    [Fact]
    public void RotatedColumnLabelsAreRotatedByTheGivenAngle()
    {
        string svg = new ColumnChart(SampleData.CreateTotals(), new ColumnsBarChartParams(rotatedLabels: true, labelRotationAngle: -45,
            labelPlacement: ColumnLabelPlacement.Bottom)).GenerateSvg();

        Assert.All(SvgInspector.Elements(svg, "text"), text => Assert.StartsWith("rotate(-45 ", (string)text.Attribute("transform")));
    }

    [Fact]
    public void StackedBarSegmentsFillTheWholeLength()
    {
        string svg = new StackedBarChart(SampleData.CreateTotals(), new StackedBarChartParams(length: 500)).GenerateSvg();
        List<XElement> segmentRects = SvgInspector.Elements(svg, "rect").Skip(1).ToList();

        Assert.Equal(500, segmentRects.Sum(rect => SvgInspector.ReadNumber(rect, "width")), 2);
    }
}
