namespace DotNetBasics.Charts.Rendering.Pies;

internal sealed class PieLegendWriter
{
    private const double EntryHorizontalPadding = 12;
    private const double EntryVerticalPadding = 4;
    private const double GapBetweenEntries = 3;

    private readonly IReadOnlyList<PieSlice> Slices;
    private readonly PieChartParams Parameters;

    public PieLegendWriter(IReadOnlyList<PieSlice> slices, PieChartParams parameters)
    {
        Slices = slices;
        Parameters = parameters;
    }

    private bool HasTitle => !string.IsNullOrEmpty(Parameters.Title);
    private double EntryHeight => Parameters.LegendFontSize + EntryVerticalPadding * 2;

    public double Width => Math.Max(
        Slices.Select(slice => ChartTextMeasure.EstimateWidth(GetEntryText(slice), Parameters.LegendFontSize)).DefaultIfEmpty(0).Max(),
        HasTitle ? ChartTextMeasure.EstimateWidth(Parameters.Title, Parameters.LegendFontSize) : 0) + EntryHorizontalPadding * 2;

    public double Height => (HasTitle ? EntryHeight : 0) + Slices.Count * (EntryHeight + GapBetweenEntries);

    public void Write(SvgCanvas canvas, double left, double top)
    {
        double entryTop = top;
        int fontSize = Parameters.LegendFontSize;
        if (HasTitle)
        {
            canvas.Text(Parameters.Title, left + Width / 2, ChartTextMeasure.GetCentredBaseline(entryTop + EntryHeight / 2, fontSize),
                new SvgTextStyle(fontSize, SvgTextAnchor.Middle) { IsItalic = true });
            entryTop += EntryHeight;
        }
        foreach (PieSlice slice in Slices)
        {
            canvas.Rect(left, entryTop, Width, EntryHeight,
                ChartSegmentColours.GetBackground(slice.Segment, Parameters.ChartColors, slice.SegmentIndex));
            canvas.Text(GetEntryText(slice), left + EntryHorizontalPadding,
                ChartTextMeasure.GetCentredBaseline(entryTop + EntryHeight / 2, fontSize),
                new SvgTextStyle(fontSize, SvgTextAnchor.Start,
                    ChartSegmentColours.GetForeground(slice.Segment, Parameters.ChartColors, slice.SegmentIndex)));
            entryTop += EntryHeight + GapBetweenEntries;
        }
    }

    private static string GetEntryText(PieSlice slice)
    {
        return $"{slice.Segment.Name} {ChartValueFormat.FormatPercentage(slice.Share)}";
    }
}
