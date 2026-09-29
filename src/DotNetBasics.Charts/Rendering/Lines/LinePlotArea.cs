namespace DotNetBasics.Charts.Rendering.Lines;

internal sealed class LinePlotArea
{
    public LinePlotArea(double left, double top, double right, double bottom, LineValueAxis valueAxis, int positionCount)
    {
        Left = left;
        Top = top;
        Right = Math.Max(left + 1, right);
        Bottom = Math.Max(top + 1, bottom);
        ValueAxis = valueAxis;
        PositionCount = positionCount;
    }

    public double Left { get; }
    public double Top { get; }
    public double Right { get; }
    public double Bottom { get; }
    public double Width => Right - Left;
    public double Height => Bottom - Top;
    public LineValueAxis ValueAxis { get; }
    public int PositionCount { get; }

    public double GetX(double share)
    {
        double padding = LineCategoryAxisCalculator.PlotSidePadding;
        return Left + Width * (padding + (1 - padding * 2) * share);
    }

    public double GetXForPosition(int positionIndex)
    {
        return GetX(PositionCount > 1 ? (double)positionIndex / (PositionCount - 1) : 0.5);
    }

    public double GetY(double value)
    {
        return Bottom - ValueAxis.GetShare(value) * Height;
    }

    public SvgPoint GetPoint(LinePlotPoint point)
    {
        return new SvgPoint(GetXForPosition(point.Index), GetY(point.Value));
    }
}
