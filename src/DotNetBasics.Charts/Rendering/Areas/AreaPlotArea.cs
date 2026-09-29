namespace DotNetBasics.Charts.Rendering.Areas;

internal sealed class AreaPlotArea
{
    public AreaPlotArea(double left, double top, double right, double bottom, LineValueAxis valueAxis, int pointCount)
    {
        Left = left;
        Top = top;
        Right = Math.Max(left + 1, right);
        Bottom = Math.Max(top + 1, bottom);
        ValueAxis = valueAxis;
        PointCount = pointCount;
    }

    public double Left { get; }
    public double Top { get; }
    public double Right { get; }
    public double Bottom { get; }
    public double Width => Right - Left;
    public double Height => Bottom - Top;
    public LineValueAxis ValueAxis { get; }
    public int PointCount { get; }

    public double GetX(int pointIndex)
    {
        return PointCount > 1 ? Left + Width * pointIndex / (PointCount - 1) : Left + Width / 2;
    }

    public double GetY(double value)
    {
        return Bottom - ValueAxis.GetShare(value) * Height;
    }
}
