using System.Text.RegularExpressions;

namespace DotNetBasics.Charts.Tests;

public class SvgDocumentTests
{
    public static TheoryData<string> SampleNames => new TheoryData<string>(SampleCharts.CreateAll().Keys);

    [Theory]
    [MemberData(nameof(SampleNames))]
    public void EveryChartIsAWellFormedSvgWithAFixedSize(string sampleName)
    {
        XElement root = SvgInspector.Parse(SampleCharts.CreateAll()[sampleName]);

        Assert.True(SvgInspector.ReadNumber(root, "width") > 0);
        Assert.True(SvgInspector.ReadNumber(root, "height") > 0);
        Assert.NotNull(root.Attribute("viewBox"));
        Assert.Equal(ChartFonts.DefaultFamily, (string)root.Attribute("font-family"));
    }

    [Theory]
    [MemberData(nameof(SampleNames))]
    public void EveryChartIsRenderedBySvgSkia(string sampleName)
    {
        int paintedPixels = SvgRasterizer.CountPaintedPixels(SampleCharts.CreateAll()[sampleName]);

        Assert.True(paintedPixels > 0);
    }

    [Fact]
    public void NumbersAreWrittenWithTheInvariantCulture()
    {
        string[] numericAttributes = { "x", "y", "width", "height", "cx", "cy", "r", "x1", "y1", "x2", "y2", "font-size", "stroke-width" };
        CultureInfo originalCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = new CultureInfo("es-ES");
        try
        {
            IEnumerable<XAttribute> attributes = SampleCharts.CreateAll().Values
                .SelectMany(svg => SvgInspector.Parse(svg).DescendantsAndSelf().Attributes())
                .Where(attribute => numericAttributes.Contains(attribute.Name.LocalName));
            Assert.All(attributes, attribute => Assert.Matches(new Regex("^-?\\d+(\\.\\d+)?$"), attribute.Value));
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void TextIsEscaped()
    {
        List<ChartSegment> segments = new List<ChartSegment> { new ChartSegment("<b>Sales & \"costs\"</b>", 10) };

        string svg = new ColumnChart(segments).GenerateSvg();

        Assert.Contains("<b>Sales & \"costs\"</b>", SvgInspector.Texts(svg));
        Assert.DoesNotContain("<b>", svg);
    }

    [Fact]
    public void EmptyDataCreatesAValidSvg()
    {
        List<string> svgs = new List<string>
        {
            new BarChart(null).GenerateSvg(),
            new ColumnChart(new List<ChartSegment>()).GenerateSvg(),
            new StackedBarChart(null).GenerateSvg(),
            new PieChart(null).GenerateSvg(),
            new LineChart(null).GenerateSvg(),
            new LineChart(new LineChartData(new List<LineData>())).GenerateSvg(),
            new ColumnWithLineChart(new ColumnWithLineChartData(new List<ColumnDataItem>())).GenerateSvg(),
            new AreaChart(null).GenerateSvg(),
            new StarRatingChart(null).GenerateSvg()
        };

        foreach (string svg in svgs)
        {
            SvgInspector.Parse(svg);
        }
    }
}
