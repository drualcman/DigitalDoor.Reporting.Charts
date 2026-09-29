namespace DotNetBasics.Charts.Rendering.Columns;

internal sealed class ColumnBesideLabelWriter
{
    private readonly SvgCanvas Canvas;
    private readonly ColumnsBarChartParams Parameters;
    private readonly ColumnChartGeometry Geometry;
    private readonly bool LabelOnTheLeft;

    public ColumnBesideLabelWriter(SvgCanvas canvas, ColumnsBarChartParams parameters, ColumnChartGeometry geometry, bool labelOnTheLeft)
    {
        Canvas = canvas;
        Parameters = parameters;
        Geometry = geometry;
        LabelOnTheLeft = labelOnTheLeft;
    }

    public void Write(string label, double columnX)
    {
        int fontSize = Parameters.LabelFontSize;
        double ascent = fontSize * ChartTextMeasure.AscentFactor;
        double descent = fontSize * ChartTextMeasure.DescentFactor;
        double leftEdge = columnX - descent;
        double rightEdge = columnX + Geometry.ColumnWidth + descent;
        if (Parameters.RotatedLabels)
        {
            double angleInRadians = ChartAngle.ToRadians(Parameters.LabelRotationAngle);
            bool glyphsGrowToTheLeft = Math.Sin(angleInRadians) < 0;
            double baselineX = LabelOnTheLeft ?
                leftEdge - (glyphsGrowToTheLeft ? descent : ascent) :
                rightEdge + (glyphsGrowToTheLeft ? ascent : descent);
            double roomOnTheLeft = glyphsGrowToTheLeft ? ascent : descent;
            double roomOnTheRight = glyphsGrowToTheLeft ? descent : ascent;
            baselineX = Math.Min(Math.Max(baselineX, roomOnTheLeft), Geometry.TotalWidth - roomOnTheRight);
            string anchor = Math.Sin(angleInRadians) > 0 ? SvgTextAnchor.End : SvgTextAnchor.Start;
            string fittedLabel = ChartTextMeasure.ShortenToFit(label, fontSize, Geometry.BarAreaHeight - ColumnChartGeometry.LabelAreaPadding);
            Canvas.Text(fittedLabel, baselineX, Geometry.BarBottomY,
                new SvgTextStyle(fontSize, anchor) { RotationAngle = Parameters.LabelRotationAngle });
        }
        else
        {
            Canvas.Text(label, LabelOnTheLeft ? leftEdge : rightEdge, Geometry.BarBottomY,
                new SvgTextStyle(fontSize, LabelOnTheLeft ? SvgTextAnchor.End : SvgTextAnchor.Start));
        }
    }
}
