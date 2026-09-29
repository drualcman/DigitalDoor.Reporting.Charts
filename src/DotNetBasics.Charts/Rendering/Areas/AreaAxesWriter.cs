namespace DotNetBasics.Charts.Rendering.Areas;

internal static class AreaAxesWriter
{
    private const double ValueLabelGap = 8;
    private const double GridLineWidth = 1;
    private const double AxisLineWidth = 1;

    public static double GetValueLabelsWidth(LineValueAxis valueAxis, AreaChartParams parameters)
    {
        return parameters.ShowYAxis ?
            valueAxis.Ticks.Max(tick => ChartTextMeasure.EstimateWidth(tick.Text, parameters.LabelFontSize)) + ValueLabelGap : 0;
    }

    public static void WriteGrid(SvgCanvas canvas, AreaPlotArea area, AreaChartParams parameters)
    {
        if (parameters.ShowGridLines)
        {
            SvgStroke gridStroke = new SvgStroke(parameters.GridLineColor, GridLineWidth);
            foreach (LineAxisTick tick in area.ValueAxis.Ticks)
            {
                double y = area.GetY(tick.Value);
                canvas.Line(new SvgPoint(area.Left, y), new SvgPoint(area.Right, y), gridStroke);
            }
        }
    }

    public static void WriteAxes(SvgCanvas canvas, AreaPlotArea area, AreaChartParams parameters)
    {
        SvgStroke axisStroke = new SvgStroke(parameters.AxisColor, AxisLineWidth);
        canvas.Line(new SvgPoint(area.Left, area.Bottom), new SvgPoint(area.Right, area.Bottom), axisStroke);
        if (parameters.ShowYAxis)
        {
            canvas.Line(new SvgPoint(area.Left, area.Top), new SvgPoint(area.Left, area.Bottom), axisStroke);
            foreach (LineAxisTick tick in area.ValueAxis.Ticks)
            {
                canvas.Text(tick.Text, area.Left - ValueLabelGap,
                    ChartTextMeasure.GetCentredBaseline(area.GetY(tick.Value), parameters.LabelFontSize),
                    new SvgTextStyle(parameters.LabelFontSize, SvgTextAnchor.End));
            }
        }
    }
}
