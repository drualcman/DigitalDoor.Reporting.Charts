namespace DotNetBasics.Charts.Rendering.Columns;

internal sealed class ColumnOutsideLabelWriter
{
    private readonly SvgCanvas Canvas;
    private readonly ColumnsBarChartParams Parameters;
    private readonly ColumnChartGeometry Geometry;

    public ColumnOutsideLabelWriter(SvgCanvas canvas, ColumnsBarChartParams parameters, ColumnChartGeometry geometry)
    {
        Canvas = canvas;
        Parameters = parameters;
        Geometry = geometry;
    }

    public void WriteAbove(string label, double columnCentreX)
    {
        double baselineY = Parameters.RotatedLabels ?
            Geometry.LabelAreaOnTop - ColumnChartGeometry.LabelGap / 2 :
            Geometry.LabelAreaOnTop - ColumnChartGeometry.LabelGap / 2 - Parameters.LabelFontSize * ChartTextMeasure.DescentFactor;
        Write(label, columnCentreX, baselineY, false);
    }

    public void WriteBelow(string label, double columnCentreX)
    {
        double baselineY = Parameters.RotatedLabels ?
            Geometry.BarBottomY + ColumnChartGeometry.LabelGap / 2 :
            Geometry.BarBottomY + ColumnChartGeometry.LabelGap / 2 + Parameters.LabelFontSize * ChartTextMeasure.AscentFactor;
        Write(label, columnCentreX, baselineY, true);
    }

    private void Write(string label, double columnCentreX, double baselineY, bool labelGrowsDownwards)
    {
        int fontSize = Parameters.LabelFontSize;
        if (Parameters.RotatedLabels)
        {
            double angleInRadians = ChartAngle.ToRadians(Parameters.LabelRotationAngle);
            double baselineX = columnCentreX - fontSize * ChartTextMeasure.DescentFactor * Math.Sin(angleInRadians);
            bool readingGoesDownwards = Math.Sin(angleInRadians) > 0;
            string anchor = readingGoesDownwards == labelGrowsDownwards ? SvgTextAnchor.Start : SvgTextAnchor.End;
            Canvas.Text(label, baselineX, baselineY,
                new SvgTextStyle(fontSize, anchor) { RotationAngle = Parameters.LabelRotationAngle });
        }
        else
        {
            Canvas.Text(ChartTextMeasure.ShortenToFit(label, fontSize, Geometry.SlotWidth - 2), columnCentreX, baselineY,
                new SvgTextStyle(fontSize, SvgTextAnchor.Middle));
        }
    }
}
