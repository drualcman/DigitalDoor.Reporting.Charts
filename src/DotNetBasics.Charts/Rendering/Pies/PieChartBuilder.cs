namespace DotNetBasics.Charts.Rendering.Pies;

internal sealed class PieChartBuilder
{
    private const double GapBetweenPieAndLegend = 16;

    private readonly IReadOnlyList<ChartSegment> Segments;
    private readonly PieChartParams Parameters;

    public PieChartBuilder(IReadOnlyList<ChartSegment> segments, PieChartParams parameters)
    {
        Segments = segments;
        Parameters = parameters;
    }

    public string Build()
    {
        List<PieSlice> slices = PieSliceCalculator.Calculate(Segments, Parameters.SeparateBiggerByDefault);
        PieLegendWriter legendWriter = new PieLegendWriter(slices, Parameters);
        double offset = Math.Max(0, Parameters.SeparationOffset);
        double pieBoxWidth = Parameters.Width + offset * 2;
        double pieBoxHeight = Parameters.Height + offset * 2;
        bool showLegend = Parameters.ShowLegend && slices.Count > 0;
        double totalWidth = pieBoxWidth + (showLegend ? GapBetweenPieAndLegend + legendWriter.Width : 0);
        double totalHeight = Math.Max(pieBoxHeight, showLegend ? legendWriter.Height : 0);
        SvgPoint centre = new SvgPoint(offset + Parameters.Width / 2.0, (totalHeight - pieBoxHeight) / 2 + offset + Parameters.Height / 2.0);
        double radius = Math.Min(Parameters.Width, Parameters.Height) / 2.0;
        SvgCanvas canvas = new SvgCanvas();
        foreach (PieSlice slice in slices.Where(slice => slice.Share > 0))
        {
            SvgPoint? translation = slice.IsExploded ? PieArcPath.GetPoint(new SvgPoint(0, 0), offset, slice.MiddleAngle) : null;
            canvas.Path(PieArcPath.Create(centre, radius, slice.StartAngle, slice.EndAngle),
                ChartSegmentColours.GetBackground(slice.Segment, Parameters.ChartColors, slice.SegmentIndex), null, translation);
        }
        WriteCallouts(canvas, slices, new PieCalloutArea(centre, radius, offset, totalWidth, totalHeight));
        if (showLegend)
        {
            legendWriter.Write(canvas, pieBoxWidth + GapBetweenPieAndLegend, (totalHeight - legendWriter.Height) / 2);
        }
        return canvas.ToDocument(totalWidth, totalHeight, Parameters.FontFamily);
    }

    private void WriteCallouts(SvgCanvas canvas, IReadOnlyList<PieSlice> slices, PieCalloutArea area)
    {
        IEnumerable<PieSlice> labelledSlices = Parameters.ShowLabels ?
            slices.Where(slice => slice.Share > 0) :
            slices.Where(slice => Parameters.ShowBiggestLabel && slice.IsHighlighted);
        foreach (PieSlice slice in labelledSlices)
        {
            double distanceFromCentre = area.Radius * Parameters.CenterTextSeparationPercentage + (slice.IsExploded ? area.Offset : 0);
            SvgPoint anchor = slice.Share >= 1 ? area.Centre : PieArcPath.GetPoint(area.Centre, distanceFromCentre, slice.MiddleAngle);
            PieCalloutWriter.Write(canvas, slice, anchor, Parameters.LabelFontSize, area.DocumentWidth, area.DocumentHeight);
        }
    }
}
