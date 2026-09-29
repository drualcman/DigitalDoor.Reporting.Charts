namespace DotNetBasics.Charts.Helpers;

internal static class ChartBackground
{
    public static bool IsTransparent(string colour)
    {
        return string.IsNullOrWhiteSpace(colour) || colour.Equals("transparent", StringComparison.OrdinalIgnoreCase) ||
            colour.Equals("none", StringComparison.OrdinalIgnoreCase);
    }

    public static void Write(SvgCanvas canvas, string colour, double width, double height)
    {
        if (!IsTransparent(colour))
        {
            canvas.Rect(0, 0, width, height, colour);
        }
    }
}
