using System.Text.RegularExpressions;

namespace DotNetBasics.Charts.Tests;

public class StarRatingChartTests
{
    [Fact]
    public void EveryItemIsARowWithTheGivenNumberOfStars()
    {
        string svg = new StarRatingChart(SampleData.CreateRatings()).GenerateSvg();
        List<XElement> stars = SvgInspector.Elements(svg, "polygon").Where(polygon => polygon.Attribute("clip-path") is null).ToList();

        Assert.Equal(25, stars.Count);
        Assert.Equal(15, stars.Count(star => (string)star.Attribute("fill") == "#FFD700"));
        Assert.Equal(10, stars.Count(star => (string)star.Attribute("fill") == "#E0E0E0"));
        Assert.All(stars, star => Assert.Equal("#B8860B", (string)star.Attribute("stroke")));
        Assert.Empty(SvgInspector.Elements(svg, "clipPath"));
    }

    [Fact]
    public void TheStarShapeIsScaledToTheStarSize()
    {
        string svg = new StarRatingChart(new[] { new StarRatingItem("A", 1, 1) }, new StarRatingParams(starCount: 1, starSize: 48)).GenerateSvg();
        List<(double X, double Y)> points = SvgInspector.ReadPoints(SvgInspector.Elements(svg, "polygon").Single());

        Assert.Equal(10, points.Count);
        Assert.Equal(40, points.Max(point => point.X) - points.Min(point => point.X), 6);
        Assert.Equal(38, points.Max(point => point.Y) - points.Min(point => point.Y), 6);
    }

    [Fact]
    public void APartialStarIsClippedToItsFraction()
    {
        string svg = new StarRatingChart(new[] { new StarRatingItem("Half", 3.5, 10) }, new StarRatingParams(starSize: 24)).GenerateSvg();
        XElement clipPath = SvgInspector.Elements(svg, "clipPath").Single();
        XElement clipRectangle = clipPath.Elements().Single();
        XElement clippedStar = SvgInspector.Elements(svg, "polygon").Single(polygon => polygon.Attribute("clip-path") is not null);

        Assert.Equal("rect", clipRectangle.Name.LocalName);
        Assert.Equal(10, SvgInspector.ReadNumber(clipRectangle, "width"), 6);
        Assert.Equal(24, SvgInspector.ReadNumber(clipRectangle, "height"), 6);
        Assert.Equal($"url(#{(string)clipPath.Attribute("id")})", (string)clippedStar.Attribute("clip-path"));
        Assert.Equal("#FFD700", (string)clippedStar.Attribute("fill"));
        Assert.Equal("defs", clipPath.Parent.Name.LocalName);
    }

    [Fact]
    public void ClipPathIdentifiersAreStableAndDifferentPerFraction()
    {
        string firstChart = new StarRatingChart(new[] { new StarRatingItem("A", 2.5, 1) }).GenerateSvg();
        string sameChart = new StarRatingChart(new[] { new StarRatingItem("A", 2.5, 1) }).GenerateSvg();
        string otherChart = new StarRatingChart(new[] { new StarRatingItem("A", 2.25, 1) }).GenerateSvg();

        Assert.Equal(firstChart, sameChart);
        Assert.NotEqual(GetClipPathId(firstChart), GetClipPathId(otherChart));
    }

    [Theory]
    [InlineData(7, 5, 0)]
    [InlineData(-1, 0, 5)]
    [InlineData(double.NaN, 0, 5)]
    public void RatingsOutOfRangeAreClamped(double rating, int filledStars, int emptyStars)
    {
        string svg = new StarRatingChart(new[] { new StarRatingItem("A", rating, 1) }).GenerateSvg();
        List<XElement> stars = SvgInspector.Elements(svg, "polygon");

        Assert.Equal(filledStars, stars.Count(star => (string)star.Attribute("fill") == "#FFD700"));
        Assert.Equal(emptyStars, stars.Count(star => (string)star.Attribute("fill") == "#E0E0E0"));
    }

    [Fact]
    public void PercentagesAreTheShareOfTheTotalCountUnlessTheyAreGiven()
    {
        List<StarRatingItem> items = new List<StarRatingItem>
        {
            new StarRatingItem("Good", 5, 30),
            new StarRatingItem("Bad", 1, 10),
            new StarRatingItem("Fixed", 3, 10, 12.5)
        };

        List<string> texts = SvgInspector.Texts(new StarRatingChart(items, new StarRatingParams(showPercentage: true)).GenerateSvg());

        Assert.Contains("60.00%", texts);
        Assert.Contains("20.00%", texts);
        Assert.Contains("12.50%", texts);
    }

    [Fact]
    public void CountsAndPercentagesAreRightAlignedAndFormatted()
    {
        StarRatingParams parameters = new StarRatingParams(showPercentage: true, countFormatter: count => $"{count} votes",
            percentageFormatter: percentage => $"{Math.Round(percentage)} pct");

        string svg = new StarRatingChart(new[] { new StarRatingItem("A", 4, 3), new StarRatingItem("B", 2, 1) }, parameters).GenerateSvg();
        List<XElement> texts = SvgInspector.Elements(svg, "text");

        Assert.Equal("end", (string)texts.Single(text => text.Value == "3 votes").Attribute("text-anchor"));
        Assert.Equal("end", (string)texts.Single(text => text.Value == "75 pct").Attribute("text-anchor"));
        Assert.Equal("start", (string)texts.Single(text => text.Value == "A").Attribute("text-anchor"));
    }

    [Fact]
    public void CountCanBeHidden()
    {
        List<string> texts = SvgInspector.Texts(new StarRatingChart(SampleData.CreateRatings(), new StarRatingParams(showCount: false)).GenerateSvg());

        Assert.Equal(5, texts.Count);
    }

    [Fact]
    public void TheWidthIsFixedAndTheHeightGrowsWithTheRows()
    {
        XElement oneRow = SvgInspector.Parse(new StarRatingChart(SampleData.CreateRatings().Take(1)).GenerateSvg());
        XElement fiveRowsWithTitle = SvgInspector.Parse(new StarRatingChart(SampleData.CreateRatings(),
            new StarRatingParams(title: "Reviews", rowHeight: 30)).GenerateSvg());

        Assert.Equal(400, SvgInspector.ReadNumber(oneRow, "width"));
        Assert.Equal(400, SvgInspector.ReadNumber(fiveRowsWithTitle, "width"));
        Assert.Equal(32, SvgInspector.ReadNumber(oneRow, "height"));
        Assert.Equal(16 + 12 + 4 + 5 * 30 + 4, SvgInspector.ReadNumber(fiveRowsWithTitle, "height"));
        Assert.Contains("Reviews", fiveRowsWithTitle.Descendants().Select(element => element.Value));
    }

    [Fact]
    public void EmptyItemsCreateAValidSvgWithoutStars()
    {
        string svg = new StarRatingChart(new List<StarRatingItem>()).GenerateSvg();

        Assert.Empty(SvgInspector.Elements(svg, "polygon"));
    }

    [Fact]
    public void NumbersAreWrittenWithTheInvariantCulture()
    {
        CultureInfo originalCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = new CultureInfo("es-ES");
        try
        {
            string svg = new StarRatingChart(SampleData.CreateBranchRatings(), new StarRatingParams(starSize: 17, showPercentage: true)).GenerateSvg();
            Regex pointList = new Regex("^-?\\d+(\\.\\d+)?,-?\\d+(\\.\\d+)?( -?\\d+(\\.\\d+)?,-?\\d+(\\.\\d+)?)*$");

            Assert.All(SvgInspector.Elements(svg, "polygon"), star => Assert.Matches(pointList, (string)star.Attribute("points")));
            Assert.Contains("57.97%", SvgInspector.Texts(svg));
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void LabelsAreEscaped()
    {
        string svg = new StarRatingChart(new[] { new StarRatingItem("<b>Q & \"A\"</b>", 3, 1) }).GenerateSvg();

        Assert.Contains("<b>Q & \"A\"</b>", SvgInspector.Texts(svg));
        Assert.DoesNotContain("<b>", svg);
    }

    [Fact]
    public void SvgSkiaPaintsOnlyTheFilledFractionOfAStar()
    {
        StarRatingParams parameters = new StarRatingParams(starCount: 1, starSize: 96, fillColor: "#00FF00", emptyColor: "#0000FF",
            borderColor: "#0000FF", showCount: false);

        int fullStarPixels = CountFilledPixels(1, parameters);
        int halfStarPixels = CountFilledPixels(0.5, parameters);
        int emptyStarPixels = CountFilledPixels(0, parameters);

        Assert.True(fullStarPixels > 1000);
        Assert.InRange(halfStarPixels, fullStarPixels * 0.35, fullStarPixels * 0.65);
        Assert.Equal(0, emptyStarPixels);
    }

    private static int CountFilledPixels(double rating, StarRatingParams parameters)
    {
        return SvgRasterizer.CountPixelsWithColour(new StarRatingChart(new[] { new StarRatingItem("A", rating, 1) }, parameters).GenerateSvg(), "#00FF00");
    }

    private static string GetClipPathId(string svg)
    {
        return (string)SvgInspector.Elements(svg, "clipPath").Single().Attribute("id");
    }
}
