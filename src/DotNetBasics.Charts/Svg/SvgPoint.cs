namespace DotNetBasics.Charts.Svg;

internal readonly struct SvgPoint
{
    public SvgPoint(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; }
    public double Y { get; }
}
