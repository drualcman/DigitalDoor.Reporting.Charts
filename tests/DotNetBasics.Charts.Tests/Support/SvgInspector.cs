namespace DotNetBasics.Charts.Tests.Support;

public static class SvgInspector
{
    private static readonly XNamespace SvgNamespace = "http://www.w3.org/2000/svg";

    public static XElement Parse(string svg)
    {
        XElement root = XDocument.Parse(svg).Root;
        Assert.NotNull(root);
        Assert.Equal(SvgNamespace + "svg", root.Name);
        return root;
    }

    public static List<XElement> Elements(string svg, string elementName)
    {
        return Parse(svg).Descendants(SvgNamespace + elementName).ToList();
    }

    public static List<string> Texts(string svg)
    {
        return Elements(svg, "text").Select(text => text.Value).ToList();
    }

    public static double ReadNumber(XElement element, string attributeName)
    {
        return double.Parse((string)element.Attribute(attributeName), CultureInfo.InvariantCulture);
    }

    public static List<(double X, double Y)> ReadPoints(XElement element)
    {
        return ((string)element.Attribute("points")).Split(' ')
            .Select(point => point.Split(','))
            .Select(coordinates => (double.Parse(coordinates[0], CultureInfo.InvariantCulture), double.Parse(coordinates[1], CultureInfo.InvariantCulture)))
            .ToList();
    }
}
