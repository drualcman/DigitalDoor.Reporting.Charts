namespace DotNetBasics.Charts.Rendering.StackedBars;

internal sealed class StackedBarLabelWriter
{
    public const double LabelGap = 6;
    public const double LabelPadding = 8;
    private const double VerticalLabelAngle = -90;

    private readonly SvgCanvas Canvas;
    private readonly StackedBarChartParams Parameters;
    private readonly bool IsVertical;

    public StackedBarLabelWriter(SvgCanvas canvas, StackedBarChartParams parameters)
    {
        Canvas = canvas;
        Parameters = parameters;
        IsVertical = parameters.Orientation == StackedBarOrientation.Vertical;
    }

    public void WriteLabel(string label, StackedBarSegmentBox box, string foreground)
    {
        int fontSize = Parameters.LabelFontSize;
        double ascent = fontSize * ChartTextMeasure.AscentFactor;
        double descent = fontSize * ChartTextMeasure.DescentFactor;
        string colour = Parameters.LabelSide == StackedBarLabelSide.Inside ? foreground : null;
        double besideBaseline = Parameters.LabelSide switch
        {
            StackedBarLabelSide.Before => box.CrossStart - LabelGap,
            StackedBarLabelSide.After => box.CrossStart + box.Thickness + LabelGap,
            _ => IsVertical ? box.CrossStart + LabelGap : box.CrossStart + box.Thickness - LabelGap
        };
        if (IsVertical)
        {
            double baselineY = Parameters.LabelAlignment switch
            {
                StackedBarLabelAlignment.Start => box.MainStart + ascent,
                StackedBarLabelAlignment.End => box.MainStart + box.MainSize - descent,
                _ => box.MainStart + box.MainSize / 2 + (ascent - descent) / 2
            };
            string anchor = Parameters.LabelSide == StackedBarLabelSide.Before ? SvgTextAnchor.End : SvgTextAnchor.Start;
            Canvas.Text(label, besideBaseline, baselineY, new SvgTextStyle(fontSize, anchor, colour));
        }
        else
        {
            double alignedMain = Parameters.LabelAlignment switch
            {
                StackedBarLabelAlignment.Start => box.MainStart + fontSize * 0.5,
                StackedBarLabelAlignment.End => box.MainStart + box.MainSize - fontSize * 0.5,
                _ => box.MainStart + box.MainSize / 2
            };
            string anchor = Parameters.LabelSide == StackedBarLabelSide.After ? SvgTextAnchor.End : SvgTextAnchor.Start;
            Canvas.Text(label, alignedMain + (ascent - descent) / 2, besideBaseline,
                new SvgTextStyle(fontSize, anchor, colour) { RotationAngle = VerticalLabelAngle });
        }
    }

    public void WriteValue(string valueText, StackedBarSegmentBox box, string foreground)
    {
        int fontSize = Parameters.LabelFontSize;
        double textWidth = ChartTextMeasure.EstimateWidth(valueText, fontSize);
        double roomForTheTextWidth = IsVertical ? box.Thickness : box.MainSize;
        double roomForTheTextHeight = IsVertical ? box.MainSize : box.Thickness;
        if (roomForTheTextWidth >= textWidth + LabelPadding && roomForTheTextHeight >= fontSize)
        {
            double centreMain = box.MainStart + box.MainSize / 2;
            double centreCross = box.CrossStart + box.Thickness / 2;
            double x = IsVertical ? centreCross : centreMain;
            double y = ChartTextMeasure.GetCentredBaseline(IsVertical ? centreMain : centreCross, fontSize);
            Canvas.Text(valueText, x, y, new SvgTextStyle(fontSize, SvgTextAnchor.Middle, foreground));
        }
    }
}
