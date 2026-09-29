namespace DotNetBasics.Charts.Svg;

internal static class SvgPointList
{
    public static string Format(IEnumerable<SvgPoint> points)
    {
        return string.Join(" ", points.Select(point => $"{SvgNumber.Format(point.X)},{SvgNumber.Format(point.Y)}"));
    }
}
