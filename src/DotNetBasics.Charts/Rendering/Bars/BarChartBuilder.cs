namespace DotNetBasics.Charts.Rendering.Bars;

internal sealed class BarChartBuilder
{
    private readonly IReadOnlyList<ChartSegment> Topics;
    private readonly ColumnsBarChartParams Parameters;
    private readonly ColumnLabelPlacement LabelPlacement;

    public BarChartBuilder(IReadOnlyList<ChartSegment> topics, ColumnsBarChartParams parameters)
    {
        Topics = topics;
        Parameters = parameters;
        LabelPlacement = parameters.LabelPlacement ?? ColumnLabelPlacement.Right;
    }

    public string Build()
    {
        BarRowGeometry geometry = new BarRowGeometry(Parameters, LabelPlacement);
        double fillReference = ChartFillReferenceValue.Get(Topics, Parameters.FillReference);
        SvgCanvas canvas = new SvgCanvas();
        BarLabelWriter labelWriter = new BarLabelWriter(canvas, Parameters, LabelPlacement, geometry);
        for (int topicIndex = 0; topicIndex < Topics.Count; topicIndex++)
        {
            ChartSegment topic = Topics[topicIndex];
            double rowTop = topicIndex * geometry.RowHeight;
            double barTop = LabelPlacement == ColumnLabelPlacement.Top ? rowTop + geometry.LabelLineHeight : rowTop;
            double filledShare = fillReference > 0 ? Math.Min(1, Math.Max(0, topic.Value) / fillReference) : 0;
            canvas.Rect(geometry.BarX, barTop, geometry.BarAreaWidth, Parameters.Thickness, Parameters.BackgroundColour);
            canvas.Rect(geometry.BarX, barTop, geometry.BarAreaWidth * filledShare, Parameters.Thickness,
                ChartSegmentColours.GetBackground(topic, Parameters.ChartColors, topicIndex));
            labelWriter.Write(topic, rowTop, barTop);
        }
        return canvas.ToDocument(geometry.TotalWidth, Topics.Count * geometry.RowHeight, Parameters.FontFamily);
    }
}
