namespace DotNetBasics.Charts.Rendering.Lines;

internal sealed class LineAxisTick
{
    public LineAxisTick(double value, string text)
    {
        Value = value;
        Text = text;
    }

    public double Value { get; }
    public string Text { get; }
}
