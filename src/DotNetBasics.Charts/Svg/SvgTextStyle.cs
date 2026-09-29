namespace DotNetBasics.Charts.Svg;

internal sealed class SvgTextStyle
{
    public SvgTextStyle(double fontSize, string anchor, string fill = null)
    {
        FontSize = fontSize;
        Anchor = anchor;
        Fill = fill;
    }

    public double FontSize { get; }
    public string Anchor { get; }
    public string Fill { get; }
    public bool IsBold { get; init; }
    public bool IsItalic { get; init; }
    public double? RotationAngle { get; init; }
}
