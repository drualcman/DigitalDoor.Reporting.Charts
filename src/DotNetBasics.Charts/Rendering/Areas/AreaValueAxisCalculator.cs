namespace DotNetBasics.Charts.Rendering.Areas;

internal static class AreaValueAxisCalculator
{
    private const int StepsUsedAsTickCountLimit = 20;
    private const int MaximumTicks = 200;

    public static List<double> ReadValues(IReadOnlyList<ChartSegment> topics)
    {
        return topics.Select(topic => double.IsNaN(topic.Value) || double.IsInfinity(topic.Value) ? 0 : topic.Value).ToList();
    }

    public static double GetBaseline(IReadOnlyList<double> values)
    {
        return values.Count > 0 ? Math.Min(0, values.Min()) : 0;
    }

    public static LineValueAxis Calculate(IReadOnlyList<double> values, int stepsY, Func<double, string> valueFormatter)
    {
        double minimum = GetBaseline(values);
        double maximum = values.Count > 0 ? Math.Max(0, values.Max()) : 0;
        if (maximum <= minimum)
        {
            maximum = minimum + 1;
        }
        double step = stepsY > StepsUsedAsTickCountLimit ? stepsY : NiceNumber.Round((maximum - minimum) / (Math.Max(2, stepsY) - 1));
        double lastTick = Math.Ceiling(maximum / step) * step;
        List<LineAxisTick> ticks = new List<LineAxisTick>();
        double tickValue = Math.Floor(minimum / step) * step;
        while (tickValue <= lastTick + step * 0.000001 && ticks.Count < MaximumTicks)
        {
            double roundedValue = Math.Round(tickValue, 10);
            ticks.Add(new LineAxisTick(roundedValue, ChartValueFormat.Format(roundedValue, valueFormatter)));
            tickValue += step;
        }
        return new LineValueAxis(ticks.Count > 1 ? ticks : new List<LineAxisTick> { new LineAxisTick(0, "0"), new LineAxisTick(1, "1") });
    }
}
