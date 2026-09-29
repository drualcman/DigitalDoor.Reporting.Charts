namespace DotNetBasics.Charts.Svg;

internal sealed class SvgStroke
{
    public SvgStroke(string colour, double width, string dashArray = null)
    {
        Colour = colour;
        Width = width;
        DashArray = dashArray;
    }

    public string Colour { get; }
    public double Width { get; }
    public string DashArray { get; }

    public string ToAttributes()
    {
        string dashAttribute = string.IsNullOrEmpty(DashArray) ? string.Empty : $" stroke-dasharray=\"{SvgEscaper.Escape(DashArray)}\"";
        return $" stroke=\"{SvgEscaper.Escape(Colour)}\" stroke-width=\"{SvgNumber.Format(Width)}\"{dashAttribute}";
    }
}
