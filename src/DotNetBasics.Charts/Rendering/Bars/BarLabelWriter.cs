namespace DotNetBasics.Charts.Rendering.Bars;

internal sealed class BarLabelWriter
{
    private readonly SvgCanvas Canvas;
    private readonly ColumnsBarChartParams Parameters;
    private readonly ColumnLabelPlacement LabelPlacement;
    private readonly BarRowGeometry Geometry;

    public BarLabelWriter(SvgCanvas canvas, ColumnsBarChartParams parameters, ColumnLabelPlacement labelPlacement, BarRowGeometry geometry)
    {
        Canvas = canvas;
        Parameters = parameters;
        LabelPlacement = labelPlacement;
        Geometry = geometry;
    }

    public void Write(ChartSegment topic, double rowTop, double barTop)
    {
        int fontSize = Parameters.LabelFontSize;
        double centredBaseline = ChartTextMeasure.GetCentredBaseline(barTop + Parameters.Thickness / 2.0, fontSize);
        if (Parameters.ShowValues)
        {
            Canvas.Text(ChartValueFormat.Format(topic.Value, Parameters.ValueFormatter),
                Geometry.BarX + Geometry.BarAreaWidth + BarRowGeometry.InnerPadding, centredBaseline,
                new SvgTextStyle(fontSize, SvgTextAnchor.Start));
        }
        WriteLabel(topic.Name, rowTop, barTop, centredBaseline, fontSize);
    }

    private void WriteLabel(string label, double rowTop, double barTop, double centredBaseline, int fontSize)
    {
        double padding = BarRowGeometry.InnerPadding;
        if (LabelPlacement == ColumnLabelPlacement.Top)
        {
            Canvas.Text(ChartTextMeasure.ShortenToFit(label, fontSize, Geometry.TotalWidth), 0,
                rowTop + fontSize * ChartTextMeasure.AscentFactor + 1, new SvgTextStyle(fontSize, SvgTextAnchor.Start));
        }
        else if (LabelPlacement == ColumnLabelPlacement.Bottom)
        {
            Canvas.Text(ChartTextMeasure.ShortenToFit(label, fontSize, Geometry.TotalWidth), 0,
                barTop + Parameters.Thickness + padding + fontSize * ChartTextMeasure.AscentFactor, new SvgTextStyle(fontSize, SvgTextAnchor.Start));
        }
        else if (LabelPlacement == ColumnLabelPlacement.Left)
        {
            Canvas.Text(ChartTextMeasure.ShortenToFit(label, fontSize, Geometry.LabelWidth - padding * 2), Geometry.LabelWidth - padding,
                centredBaseline, new SvgTextStyle(fontSize, SvgTextAnchor.End));
        }
        else
        {
            Canvas.Text(ChartTextMeasure.ShortenToFit(label, fontSize, Geometry.LabelWidth - padding * 2), Geometry.TotalWidth - padding,
                centredBaseline, new SvgTextStyle(fontSize, SvgTextAnchor.End));
        }
    }
}
