using System.Text.RegularExpressions;

namespace DotNetBasics.Charts.Tests;

public class AreaChartTests
{
    [Fact]
    public void TheDocumentHasTheGivenSize()
    {
        XElement root = SvgInspector.Parse(new AreaChart(SampleData.CreateMonthlyVisits(), new AreaChartParams(width: 500, height: 250)).GenerateSvg());

        Assert.Equal(500, SvgInspector.ReadNumber(root, "width"));
        Assert.Equal(250, SvgInspector.ReadNumber(root, "height"));
        Assert.Equal("0 0 500 250", (string)root.Attribute("viewBox"));
    }

    [Fact]
    public void TheAreaGoesFromTheBaselineThroughEveryPoint()
    {
        string svg = new AreaChart(SampleData.CreateMonthlyVisits()).GenerateSvg();
        XElement polygon = SvgInspector.Elements(svg, "polygon").Single();
        List<(double X, double Y)> polygonPoints = SvgInspector.ReadPoints(polygon);
        List<(double X, double Y)> linePoints = SvgInspector.ReadPoints(SvgInspector.Elements(svg, "polyline").Single());

        Assert.Equal(9, polygonPoints.Count);
        Assert.Equal(7, linePoints.Count);
        Assert.Equal(linePoints, polygonPoints.Take(7));
        Assert.Equal(polygonPoints[7].Y, polygonPoints[8].Y);
        Assert.True(polygonPoints.Take(7).All(point => point.Y < polygonPoints[7].Y));
        Assert.Equal("0.4", (string)polygon.Attribute("fill-opacity"));
        Assert.Equal(7, SvgInspector.Elements(svg, "circle").Count);
    }

    [Fact]
    public void TheHighestValueIsTheHighestPointAndTheAxisUsesRoundTicks()
    {
        string svg = new AreaChart(SampleData.CreateMonthlyVisits()).GenerateSvg();
        List<(double X, double Y)> linePoints = SvgInspector.ReadPoints(SvgInspector.Elements(svg, "polyline").Single());

        Assert.Equal(6, linePoints.IndexOf(linePoints.OrderBy(point => point.Y).First()));
        Assert.Equal(new[] { "0", "200", "400" }, SvgInspector.Texts(svg).Take(3));
    }

    [Fact]
    public void PointsValuesAndLabelsCanBeHidden()
    {
        AreaChartParams parameters = new AreaChartParams(showPoints: false, showLabels: false, showYAxis: false);

        string svg = new AreaChart(SampleData.CreateMonthlyVisits(), parameters).GenerateSvg();

        Assert.Empty(SvgInspector.Elements(svg, "circle"));
        Assert.Empty(SvgInspector.Elements(svg, "text"));
    }

    [Fact]
    public void ValuesAndTitleAreWrittenInsideTheSvg()
    {
        AreaChartParams parameters = new AreaChartParams(showValues: true, title: "Visits", valueFormatter: value => $"{value} v");

        string svg = new AreaChart(SampleData.CreateMonthlyVisits(), parameters).GenerateSvg();
        List<string> texts = SvgInspector.Texts(svg);

        Assert.Contains("120 v", texts);
        Assert.Contains("380 v", texts);
        Assert.Contains(SvgInspector.Elements(svg, "text"), text => text.Value == "Visits" && (string)text.Attribute("font-weight") == "bold");
    }

    [Fact]
    public void RotatedLabelsUseTheGivenAngle()
    {
        string svg = new AreaChart(SampleData.CreateMonthlyVisits(), new AreaChartParams(rotatedLabels: true, labelRotationAngle: 30)).GenerateSvg();

        Assert.Equal(7, SvgInspector.Elements(svg, "text").Count(text => ((string)text.Attribute("transform") ?? "").StartsWith("rotate(30 ")));
    }

    [Fact]
    public void RotationAngleMustBeBetweenZeroAndNinety()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new AreaChartParams(labelRotationAngle: -10));
    }

    [Fact]
    public void EmptyDataDrawsOnlyTheAxes()
    {
        string svg = new AreaChart(new List<ChartSegment>()).GenerateSvg();

        Assert.Empty(SvgInspector.Elements(svg, "polygon"));
        Assert.Empty(SvgInspector.Elements(svg, "polyline"));
        Assert.NotEmpty(SvgInspector.Elements(svg, "line"));
    }

    [Fact]
    public void ASinglePointIsDrawnAsAFlatArea()
    {
        string svg = new AreaChart(new[] { new ChartSegment("Only", 10) }).GenerateSvg();
        List<(double X, double Y)> polygonPoints = SvgInspector.ReadPoints(SvgInspector.Elements(svg, "polygon").Single());

        Assert.Equal(4, polygonPoints.Count);
        Assert.Equal(polygonPoints[0].Y, polygonPoints[1].Y);
        Assert.Single(SvgInspector.Elements(svg, "circle"));
    }

    [Fact]
    public void AllZeroValuesLieOnTheBaseline()
    {
        List<ChartSegment> zeros = new List<ChartSegment> { new ChartSegment("A", 0), new ChartSegment("B", 0), new ChartSegment("C", 0) };

        string svg = new AreaChart(zeros).GenerateSvg();
        List<(double X, double Y)> polygonPoints = SvgInspector.ReadPoints(SvgInspector.Elements(svg, "polygon").Single());

        Assert.Single(polygonPoints.Select(point => point.Y).Distinct());
        Assert.Contains("0", SvgInspector.Texts(svg));
        Assert.Contains("1", SvgInspector.Texts(svg));
    }

    [Fact]
    public void NegativeValuesMoveTheBaselineBelowZero()
    {
        string svg = new AreaChart(SampleData.CreateMonthlyBalance()).GenerateSvg();
        List<(double X, double Y)> polygonPoints = SvgInspector.ReadPoints(SvgInspector.Elements(svg, "polygon").Single());
        double baselineY = polygonPoints[polygonPoints.Count - 1].Y;

        Assert.True(polygonPoints.Take(4).All(point => point.Y <= baselineY));
        Assert.Contains(SvgInspector.Texts(svg), text => text.StartsWith("-"));
        Assert.Contains("0", SvgInspector.Texts(svg));
    }

    [Fact]
    public void NumbersAreWrittenWithTheInvariantCulture()
    {
        CultureInfo originalCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = new CultureInfo("es-ES");
        try
        {
            string svg = new AreaChart(SampleData.CreateMonthlyVisits(), new AreaChartParams(areaOpacity: 0.35, showValues: true)).GenerateSvg();
            Regex pointList = new Regex("^-?\\d+(\\.\\d+)?,-?\\d+(\\.\\d+)?( -?\\d+(\\.\\d+)?,-?\\d+(\\.\\d+)?)*$");

            Assert.Matches(pointList, (string)SvgInspector.Elements(svg, "polygon").Single().Attribute("points"));
            Assert.Equal("0.35", (string)SvgInspector.Elements(svg, "polygon").Single().Attribute("fill-opacity"));
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void LabelsAreEscaped()
    {
        string svg = new AreaChart(new[] { new ChartSegment("<b>A & \"B\"</b>", 1), new ChartSegment("C", 2) }).GenerateSvg();

        Assert.Contains("<b>A & \"B\"</b>", SvgInspector.Texts(svg));
        Assert.DoesNotContain("<b>", svg);
    }

    [Fact]
    public void TheAreaIsRenderedBySvgSkia()
    {
        string svg = new AreaChart(SampleData.CreateMonthlyVisits(), new AreaChartParams(areaFill: "#00FF00", areaOpacity: 1,
            showGridLines: false, showPoints: false)).GenerateSvg();

        Assert.True(SvgRasterizer.CountPixelsWithColour(svg, "#00FF00") > 1000);
    }
}
