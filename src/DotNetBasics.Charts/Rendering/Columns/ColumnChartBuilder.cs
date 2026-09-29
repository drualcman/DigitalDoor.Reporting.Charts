namespace DotNetBasics.Charts.Rendering.Columns;

internal sealed class ColumnChartBuilder
{
    private const double ValueGapAboveTheBarArea = 5;

    private readonly IReadOnlyList<ChartSegment> Topics;
    private readonly ColumnsBarChartParams Parameters;
    private readonly ColumnLabelPlacement LabelPlacement;

    public ColumnChartBuilder(IReadOnlyList<ChartSegment> topics, ColumnsBarChartParams parameters)
    {
        Topics = topics;
        Parameters = parameters;
        LabelPlacement = parameters.LabelPlacement ??
            (parameters.RotatedLabels ? ColumnLabelPlacement.Left : ColumnLabelPlacement.Bottom);
    }

    public string Build()
    {
        ColumnChartGeometry geometry = new ColumnChartGeometry(Topics, Parameters, LabelPlacement);
        double fillReference = ChartFillReferenceValue.Get(Topics, Parameters.FillReference);
        SvgCanvas canvas = new SvgCanvas();
        ColumnOutsideLabelWriter outsideLabelWriter = new ColumnOutsideLabelWriter(canvas, Parameters, geometry);
        ColumnBesideLabelWriter besideLabelWriter = new ColumnBesideLabelWriter(canvas, Parameters, geometry, LabelPlacement == ColumnLabelPlacement.Left);
        for (int topicIndex = 0; topicIndex < Topics.Count; topicIndex++)
        {
            ChartSegment topic = Topics[topicIndex];
            double columnX = geometry.GetColumnX(topicIndex);
            double columnCentreX = columnX + geometry.ColumnWidth / 2;
            double filledShare = fillReference > 0 ? Math.Min(1, Math.Max(0, topic.Value) / fillReference) : 0;
            double filledHeight = geometry.BarAreaHeight * filledShare;
            canvas.Rect(columnX, geometry.BarAreaTop, geometry.ColumnWidth, geometry.BarAreaHeight, Parameters.BackgroundColour);
            canvas.Rect(columnX, geometry.BarBottomY - filledHeight, geometry.ColumnWidth, filledHeight,
                ChartSegmentColours.GetBackground(topic, Parameters.ChartColors, topicIndex));
            if (Parameters.ShowValues)
            {
                canvas.Text(ChartValueFormat.Format(topic.Value, Parameters.ValueFormatter), columnCentreX,
                    geometry.BarAreaTop - ValueGapAboveTheBarArea, new SvgTextStyle(Parameters.LabelFontSize, SvgTextAnchor.Middle));
            }
            WriteLabel(topic.Name, columnX, columnCentreX, outsideLabelWriter, besideLabelWriter);
        }
        return canvas.ToDocument(geometry.TotalWidth, geometry.TotalHeight, Parameters.FontFamily);
    }

    private void WriteLabel(string label, double columnX, double columnCentreX,
        ColumnOutsideLabelWriter outsideLabelWriter, ColumnBesideLabelWriter besideLabelWriter)
    {
        if (LabelPlacement == ColumnLabelPlacement.Left || LabelPlacement == ColumnLabelPlacement.Right)
        {
            besideLabelWriter.Write(label, columnX);
        }
        else if (LabelPlacement == ColumnLabelPlacement.Top)
        {
            outsideLabelWriter.WriteAbove(label, columnCentreX);
        }
        else
        {
            outsideLabelWriter.WriteBelow(label, columnCentreX);
        }
    }
}
