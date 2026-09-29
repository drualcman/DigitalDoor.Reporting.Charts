namespace DotNetBasics.Charts.Rendering.Lines;

internal static class LineGridWriter
{
    public static void Write(SvgCanvas canvas, LinePlotArea area, LineCategoryAxis categoryAxis, LineChartParams parameters)
    {
        SvgStroke gridStroke = new SvgStroke(parameters.GridLineStroke, parameters.GridWidth);
        if (parameters.ShowYLines)
        {
            foreach (LineAxisTick tick in area.ValueAxis.Ticks)
            {
                double y = area.GetY(tick.Value);
                canvas.Line(new SvgPoint(area.Left, y), new SvgPoint(area.Right, y), gridStroke);
            }
        }
        if (parameters.ShowXLines)
        {
            for (int labelIndex = 0; labelIndex < categoryAxis.Labels.Count; labelIndex++)
            {
                if (categoryAxis.IsLabelVisible(labelIndex))
                {
                    double x = area.GetX(categoryAxis.GetShare(labelIndex));
                    canvas.Line(new SvgPoint(x, area.Top), new SvgPoint(x, area.Bottom), gridStroke);
                }
            }
        }
    }
}
