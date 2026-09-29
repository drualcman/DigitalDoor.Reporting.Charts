namespace DotNetBasics.Charts.Rendering.ColumnsWithLines;

internal sealed class ColumnWithLineChartBuilder
{
    private const double TitleSpacing = 12;
    private const double LegendSpacing = 8;
    private const double SideSpace = 20;

    private readonly ColumnWithLineChartData Data;
    private readonly IReadOnlyList<ColumnDataItem> Items;
    private readonly ColumnWithLineChartParams Parameters;

    public ColumnWithLineChartBuilder(ColumnWithLineChartData data, ColumnWithLineChartParams parameters)
    {
        Data = data;
        Items = (data.Data ?? Enumerable.Empty<ColumnDataItem>()).Where(item => item is not null).ToList();
        Parameters = parameters;
    }

    public string Build()
    {
        bool showTitle = Parameters.ShowTitle && !string.IsNullOrWhiteSpace(Data.Title);
        double titleHeight = showTitle ? Parameters.TitleFontSize + TitleSpacing : 0;
        double naturalChartWidth = Items.Count * (Parameters.BarWidth + Parameters.Spacing) + ColumnWithLineLayout.Margin;
        LegendRowsWriter legendWriter = Parameters.ShowLegend ?
            new LegendRowsWriter(ColumnWithLineLegendEntries.Create(Data, Parameters), Parameters.LegendFontSize,
                Math.Max(naturalChartWidth, Parameters.Width) - SideSpace) : null;
        double legendHeight = legendWriter is null ? 0 : legendWriter.Height + LegendSpacing;
        double minimumTotalWidth = Math.Max(
            showTitle ? ChartTextMeasure.EstimateWidth(Data.Title, Parameters.TitleFontSize) + SideSpace : 0,
            legendWriter is null ? 0 : legendWriter.WidestRow + SideSpace);
        ColumnWithLineLayout layout = new ColumnWithLineLayout(Items, Parameters, titleHeight + legendHeight, minimumTotalWidth);
        SvgCanvas canvas = new SvgCanvas();
        if (!string.IsNullOrWhiteSpace(Parameters.BackgroundColor) && !Parameters.BackgroundColor.Equals("transparent", StringComparison.OrdinalIgnoreCase))
        {
            canvas.Rect(0, 0, layout.TotalWidth, layout.TotalHeight, Parameters.BackgroundColor);
        }
        if (showTitle)
        {
            canvas.Text(Data.Title, layout.TotalWidth / 2, Parameters.TitleFontSize,
                new SvgTextStyle(Parameters.TitleFontSize, SvgTextAnchor.Middle) { IsBold = true });
        }
        legendWriter?.Write(canvas, titleHeight, layout.TotalWidth);
        canvas.Line(new SvgPoint(layout.AxisStartX, layout.AxisY), new SvgPoint(layout.AxisEndX, layout.AxisY), new SvgStroke(Parameters.AxisColor, 1));
        foreach (ColumnWithLineItemGeometry item in layout.Items)
        {
            canvas.Rect(item.ColumnX, item.ColumnTop, item.ColumnWidth, item.PrimaryHeight, Parameters.PrimaryColor);
            canvas.Rect(item.ColumnX, item.ColumnTop + item.PrimaryHeight, item.ColumnWidth, item.SecondaryHeight, Parameters.SecondaryColor);
        }
        ColumnWithLineSeriesWriter.WriteLines(canvas, layout, Parameters);
        foreach (ColumnWithLineItemGeometry item in layout.Items)
        {
            ColumnWithLineSeriesWriter.WritePointsAndLabels(canvas, item, Parameters);
            canvas.Text(Parameters.BottomLabelFormatter?.Invoke(item.Item) ?? item.Item.Label, item.CentreX, layout.BottomLabelY,
                new SvgTextStyle(Parameters.LabelFontSize, SvgTextAnchor.Middle));
        }
        return canvas.ToDocument(layout.TotalWidth, layout.TotalHeight, Parameters.FontFamily);
    }
}
