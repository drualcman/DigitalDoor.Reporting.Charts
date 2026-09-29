namespace DotNetBasics.Charts.Rendering.ColumnsWithLines;

internal static class ColumnWithLineLegendEntries
{
    public static List<LegendEntry> Create(ColumnWithLineChartData data, ColumnWithLineChartParams parameters)
    {
        List<LegendEntry> entries = new List<LegendEntry>
        {
            new LegendEntry(data.PrimaryLegend, parameters.PrimaryColor, LegendMarker.Square),
            new LegendEntry(data.SecondaryLegend, parameters.SecondaryColor, LegendMarker.Square)
        };
        if (parameters.ShowGranTotal)
        {
            entries.Add(new LegendEntry(parameters.GrandTotalLegend, parameters.GrandTotalLineColor, LegendMarker.Line));
        }
        if (parameters.ShowPrimaryValues)
        {
            entries.Add(new LegendEntry($"% {data.PrimaryLegend}", parameters.PrimaryPercentageLineColor, LegendMarker.Line));
        }
        if (parameters.ShowSecondaryValues)
        {
            entries.Add(new LegendEntry($"% {data.SecondaryLegend}", parameters.SecondaryPercentageLineColor, LegendMarker.Line));
        }
        return entries.Where(entry => !string.IsNullOrWhiteSpace(entry.Text)).ToList();
    }
}
