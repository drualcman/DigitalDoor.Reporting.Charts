namespace DotNetBasics.Charts.Rendering.Lines;

internal sealed class LinePlotPoint
{
    public LinePlotPoint(int index, double value)
    {
        Index = index;
        Value = value;
    }

    public int Index { get; }
    public double Value { get; }
}
