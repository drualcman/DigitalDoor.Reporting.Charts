namespace DotNetBasics.Charts.Rendering.Lines;

internal static class LineAxesWriter
{
    public const double LabelGap = 8;
    private const double MinimumSpaceBetweenValueLabels = 2;

    public static void Write(SvgCanvas canvas, LinePlotArea area, LineCategoryAxis categoryAxis, LineChartParams parameters)
    {
        SvgStroke axisStroke = new SvgStroke(parameters.AxisStroke, parameters.AxisWidth);
        canvas.Line(new SvgPoint(area.Left, area.Top), new SvgPoint(area.Left, area.Bottom), axisStroke);
        canvas.Line(new SvgPoint(area.Left, area.Bottom), new SvgPoint(area.Right, area.Bottom), axisStroke);
        if (parameters.ShowY)
        {
            WriteValueLabels(canvas, area, parameters.FontSize);
        }
        if (parameters.ShowX)
        {
            WriteCategoryLabels(canvas, area, categoryAxis, parameters);
        }
    }

    private static void WriteValueLabels(SvgCanvas canvas, LinePlotArea area, double fontSize)
    {
        IReadOnlyList<LineAxisTick> ticks = area.ValueAxis.Ticks;
        double spaceNeeded = ticks.Count * (fontSize + MinimumSpaceBetweenValueLabels);
        int labelStep = spaceNeeded > area.Height ? (int)Math.Ceiling(spaceNeeded / area.Height) : 1;
        for (int tickIndex = 0; tickIndex < ticks.Count; tickIndex++)
        {
            if (tickIndex % labelStep == 0)
            {
                canvas.Text(ticks[tickIndex].Text, area.Left - LabelGap,
                    ChartTextMeasure.GetCentredBaseline(area.GetY(ticks[tickIndex].Value), fontSize),
                    new SvgTextStyle(fontSize, SvgTextAnchor.End));
            }
        }
    }

    private static void WriteCategoryLabels(SvgCanvas canvas, LinePlotArea area, LineCategoryAxis categoryAxis, LineChartParams parameters)
    {
        double fontSize = parameters.FontSize;
        for (int labelIndex = 0; labelIndex < categoryAxis.Labels.Count; labelIndex++)
        {
            if (categoryAxis.IsLabelVisible(labelIndex))
            {
                double x = area.GetX(categoryAxis.GetShare(labelIndex));
                string label = categoryAxis.Labels[labelIndex];
                if (categoryAxis.IsRotated)
                {
                    canvas.Text(label, x, area.Bottom + LabelGap,
                        new SvgTextStyle(fontSize, SvgTextAnchor.Start) { RotationAngle = parameters.RotationAngleXLabel });
                }
                else
                {
                    canvas.Text(label, x, area.Bottom + LabelGap + fontSize * ChartTextMeasure.AscentFactor,
                        new SvgTextStyle(fontSize, SvgTextAnchor.Middle));
                }
            }
        }
    }
}
