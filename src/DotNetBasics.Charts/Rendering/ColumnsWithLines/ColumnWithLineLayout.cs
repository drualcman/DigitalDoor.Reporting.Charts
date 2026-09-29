namespace DotNetBasics.Charts.Rendering.ColumnsWithLines;

internal sealed class ColumnWithLineLayout
{
    public const double Margin = 15;
    private const double BottomLabelDistance = 20;
    private const double AxisDistanceBelowTheColumns = 5;

    public ColumnWithLineLayout(IReadOnlyList<ColumnDataItem> items, ColumnWithLineChartParams parameters, double headerHeight,
        double minimumTotalWidth)
    {
        ChartHeight = Math.Max(parameters.MinBarHeight, parameters.Height / 2.0);
        ChartWidth = items.Count * (parameters.BarWidth + parameters.Spacing) + Margin;
        TotalWidth = Math.Max(ChartWidth, minimumTotalWidth);
        TotalHeight = headerHeight + ChartHeight + Margin * 3;
        ChartLeft = (TotalWidth - ChartWidth) / 2;
        BaseY = headerHeight + Margin + ChartHeight;
        AxisY = BaseY + AxisDistanceBelowTheColumns;
        AxisStartX = ChartLeft + Margin - parameters.Spacing;
        AxisEndX = ChartLeft + ChartWidth;
        BottomLabelY = BaseY + BottomLabelDistance;
        decimal highestColumnTotal = items.Select(item => Math.Max(0, item.PrimaryValue) + Math.Max(0, item.SecondaryValue)).DefaultIfEmpty(0).Max();
        decimal grandTotal = items.Sum(item => Math.Max(0, item.PrimaryValue) + Math.Max(0, item.SecondaryValue));
        Items = items
            .Select((item, itemIndex) => new ColumnWithLineItemGeometry(item,
                ChartLeft + Margin + itemIndex * (parameters.BarWidth + parameters.Spacing), parameters.BarWidth, BaseY, ChartHeight,
                highestColumnTotal, grandTotal))
            .ToList();
    }

    public double ChartHeight { get; }
    public double ChartWidth { get; }
    public double TotalWidth { get; }
    public double TotalHeight { get; }
    public double ChartLeft { get; }
    public double BaseY { get; }
    public double AxisY { get; }
    public double AxisStartX { get; }
    public double AxisEndX { get; }
    public double BottomLabelY { get; }
    public IReadOnlyList<ColumnWithLineItemGeometry> Items { get; }
}
