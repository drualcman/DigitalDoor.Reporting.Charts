namespace DotNetBasics.Charts.Rendering.StackedBars;

internal sealed class StackedBarChartBuilder
{
    private readonly IReadOnlyList<ChartSegment> Segments;
    private readonly StackedBarChartParams Parameters;
    private readonly bool IsVertical;

    public StackedBarChartBuilder(IReadOnlyList<ChartSegment> segments, StackedBarChartParams parameters)
    {
        Segments = segments;
        Parameters = parameters;
        IsVertical = parameters.Orientation == StackedBarOrientation.Vertical;
    }

    public string Build()
    {
        double total = Parameters.Total > 0 ? Parameters.Total : Segments.Sum(segment => Math.Max(0, segment.Value));
        double labelBand = GetLabelBandSize(total);
        double barCrossStart = Parameters.LabelSide == StackedBarLabelSide.Before ? labelBand : 0;
        double crossSize = Parameters.Thickness + labelBand;
        SvgCanvas canvas = new SvgCanvas();
        StackedBarLabelWriter labelWriter = new StackedBarLabelWriter(canvas, Parameters);
        AddBarRect(canvas, new StackedBarSegmentBox(0, Parameters.Length, barCrossStart, Parameters.Thickness), Parameters.BackgroundColour);
        double mainOffset = 0;
        for (int segmentIndex = 0; segmentIndex < Segments.Count; segmentIndex++)
        {
            ChartSegment segment = Segments[segmentIndex];
            double share = total > 0 ? Math.Max(0, segment.Value) / total : 0;
            double mainSize = Math.Min(Parameters.Length * share, Math.Max(0, Parameters.Length - mainOffset));
            StackedBarSegmentBox box = new StackedBarSegmentBox(mainOffset, mainSize, barCrossStart, Parameters.Thickness);
            string foreground = ChartSegmentColours.GetForeground(segment, Parameters.ChartColors, segmentIndex);
            AddBarRect(canvas, box, ChartSegmentColours.GetBackground(segment, Parameters.ChartColors, segmentIndex));
            if (share >= Parameters.MinimumLabelShare)
            {
                labelWriter.WriteLabel(segment.Name, box, foreground);
            }
            if (Parameters.ShowValues && Parameters.LabelSide != StackedBarLabelSide.Inside)
            {
                labelWriter.WriteValue(ChartValueFormat.Format(segment.Value, Parameters.ValueFormatter), box, foreground);
            }
            mainOffset += mainSize;
        }
        return canvas.ToDocument(IsVertical ? crossSize : Parameters.Length, IsVertical ? Parameters.Length : crossSize, Parameters.FontFamily);
    }

    private double GetLabelBandSize(double total)
    {
        double result = 0;
        if (Parameters.LabelSide != StackedBarLabelSide.Inside)
        {
            double longestLabel = Segments
                .Where(segment => total <= 0 || Math.Max(0, segment.Value) / total >= Parameters.MinimumLabelShare)
                .Select(segment => ChartTextMeasure.EstimateWidth(segment.Name, Parameters.LabelFontSize))
                .DefaultIfEmpty(0)
                .Max();
            result = longestLabel > 0 ? longestLabel + StackedBarLabelWriter.LabelPadding : 0;
        }
        return result;
    }

    private void AddBarRect(SvgCanvas canvas, StackedBarSegmentBox box, string colour)
    {
        if (IsVertical)
        {
            canvas.Rect(box.CrossStart, box.MainStart, box.Thickness, box.MainSize, colour);
        }
        else
        {
            canvas.Rect(box.MainStart, box.CrossStart, box.MainSize, box.Thickness, colour);
        }
    }
}
