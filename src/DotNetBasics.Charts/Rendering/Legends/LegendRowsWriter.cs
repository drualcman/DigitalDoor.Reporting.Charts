namespace DotNetBasics.Charts.Rendering.Legends;

internal sealed class LegendRowsWriter
{
    private const double MarkerSize = 14;
    private const double MarkerToTextGap = 6;
    private const double GapBetweenEntries = 16;
    private const double RowPadding = 6;

    private readonly IReadOnlyList<LegendEntry> Entries;
    private readonly double FontSize;
    private readonly double AvailableWidth;
    private readonly List<List<int>> Rows = new List<List<int>>();

    public LegendRowsWriter(IReadOnlyList<LegendEntry> entries, double fontSize, double availableWidth)
    {
        Entries = entries;
        FontSize = fontSize;
        AvailableWidth = availableWidth;
        ArrangeRows();
    }

    public double RowHeight => Math.Max(MarkerSize, FontSize) + RowPadding;
    public double Height => Rows.Count * RowHeight;
    public double WidestRow => Rows.Select(GetRowWidth).DefaultIfEmpty(0).Max();

    public void Write(SvgCanvas canvas, double top, double totalWidth)
    {
        for (int rowIndex = 0; rowIndex < Rows.Count; rowIndex++)
        {
            double x = (totalWidth - GetRowWidth(Rows[rowIndex])) / 2;
            double centreY = top + rowIndex * RowHeight + RowHeight / 2;
            foreach (int entryIndex in Rows[rowIndex])
            {
                LegendEntry entry = Entries[entryIndex];
                LegendMarkerWriter.Write(canvas, entry, x, centreY, MarkerSize);
                canvas.Text(entry.Text, x + MarkerSize + MarkerToTextGap, ChartTextMeasure.GetCentredBaseline(centreY, FontSize),
                    new SvgTextStyle(FontSize, SvgTextAnchor.Start));
                x += GetEntryWidth(entry) + GapBetweenEntries;
            }
        }
    }

    private void ArrangeRows()
    {
        double usedWidth = 0;
        for (int entryIndex = 0; entryIndex < Entries.Count; entryIndex++)
        {
            double entryWidth = GetEntryWidth(Entries[entryIndex]);
            if (Rows.Count == 0 || usedWidth + GapBetweenEntries + entryWidth > AvailableWidth)
            {
                Rows.Add(new List<int>());
                usedWidth = entryWidth;
            }
            else
            {
                usedWidth += GapBetweenEntries + entryWidth;
            }
            Rows[Rows.Count - 1].Add(entryIndex);
        }
    }

    private double GetRowWidth(List<int> row)
    {
        return row.Sum(entryIndex => GetEntryWidth(Entries[entryIndex])) + Math.Max(0, row.Count - 1) * GapBetweenEntries;
    }

    private double GetEntryWidth(LegendEntry entry)
    {
        return MarkerSize + MarkerToTextGap + ChartTextMeasure.EstimateWidth(entry.Text, FontSize);
    }
}
