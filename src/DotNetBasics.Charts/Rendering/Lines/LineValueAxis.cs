namespace DotNetBasics.Charts.Rendering.Lines;

internal sealed class LineValueAxis
{
    public LineValueAxis(IReadOnlyList<LineAxisTick> ticks)
    {
        Ticks = ticks;
        Minimum = ticks.Min(tick => tick.Value);
        Maximum = ticks.Max(tick => tick.Value);
    }

    public IReadOnlyList<LineAxisTick> Ticks { get; }
    public double Minimum { get; }
    public double Maximum { get; }

    public double GetShare(double value)
    {
        double range = Maximum - Minimum;
        return range > 0 ? (value - Minimum) / range : 0.5;
    }
}
