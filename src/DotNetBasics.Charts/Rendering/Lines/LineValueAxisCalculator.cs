namespace DotNetBasics.Charts.Rendering.Lines;

internal static class LineValueAxisCalculator
{
    private const int StepsUsedAsTickCountLimit = 20;
    private const int MaximumTicks = 200;

    public static LineValueAxis Calculate(IReadOnlyList<LinePlotSeries> series, IEnumerable<string> customLabels, int stepsY,
        CultureInfo culture)
    {
        List<string> labels = (customLabels ?? Enumerable.Empty<string>()).ToList();
        return labels.Count > 1 ? CreateFromCustomLabels(labels, culture) : CreateFromValues(series, stepsY, culture);
    }

    private static LineValueAxis CreateFromCustomLabels(List<string> labels, CultureInfo culture)
    {
        List<double> labelValues = labels
            .Select((label, labelIndex) => double.TryParse(label, NumberStyles.Float | NumberStyles.AllowThousands, culture, out double value) ? value : labelIndex)
            .ToList();
        double minimum = labelValues.Min();
        double maximum = labelValues.Max() > minimum ? labelValues.Max() : minimum + 1;
        List<LineAxisTick> ticks = labels
            .Select((label, labelIndex) => new LineAxisTick(minimum + (maximum - minimum) * labelIndex / (labels.Count - 1), label))
            .ToList();
        return new LineValueAxis(ticks);
    }

    private static LineValueAxis CreateFromValues(IReadOnlyList<LinePlotSeries> series, int stepsY, CultureInfo culture)
    {
        List<double> values = series.SelectMany(lineSeries => lineSeries.Points).Select(point => point.Value).ToList();
        double minimum = values.Count > 0 ? values.Min() : 0;
        double maximum = values.Count > 0 ? values.Max() : 1;
        if (maximum - minimum <= 0)
        {
            double padding = Math.Abs(maximum) > 0 ? Math.Abs(maximum) * 0.1 : 1;
            bool valuesArePositive = minimum >= 0;
            minimum = valuesArePositive ? Math.Max(0, minimum - padding) : minimum - padding;
            maximum += padding;
        }
        double step = stepsY > StepsUsedAsTickCountLimit ? stepsY : NiceNumber.Round((maximum - minimum) / (Math.Max(2, stepsY) - 1));
        double firstTick = Math.Floor(minimum / step) * step;
        double lastTick = Math.Ceiling(maximum / step) * step;
        List<LineAxisTick> ticks = new List<LineAxisTick>();
        double tickValue = firstTick;
        while (tickValue <= lastTick + step * 0.000001 && ticks.Count < MaximumTicks)
        {
            double roundedValue = Math.Round(tickValue, 10);
            ticks.Add(new LineAxisTick(roundedValue, roundedValue.ToString("0.##", culture)));
            tickValue += step;
        }
        return new LineValueAxis(ticks.Count > 1 ? ticks : new List<LineAxisTick> { new LineAxisTick(0, "0"), new LineAxisTick(1, "1") });
    }
}
