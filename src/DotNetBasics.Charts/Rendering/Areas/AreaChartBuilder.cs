namespace DotNetBasics.Charts.Rendering.Areas;

internal sealed class AreaChartBuilder
{
    private const double SidePadding = 10;
    private const double TopPadding = 8;

    private readonly IReadOnlyList<ChartSegment> Topics;
    private readonly AreaChartParams Parameters;

    public AreaChartBuilder(IReadOnlyList<ChartSegment> topics, AreaChartParams parameters)
    {
        Topics = topics;
        Parameters = parameters;
    }

    public string Build()
    {
        List<double> values = AreaValueAxisCalculator.ReadValues(Topics);
        LineValueAxis valueAxis = AreaValueAxisCalculator.Calculate(values, Parameters.StepsY, Parameters.ValueFormatter);
        double estimatedLeft = SidePadding + AreaAxesWriter.GetValueLabelsWidth(valueAxis, Parameters);
        AreaCategoryLabels categoryLabels = new AreaCategoryLabels(Topics, Parameters, Parameters.Width - estimatedLeft - SidePadding * 2);
        double left = Math.Max(estimatedLeft, categoryLabels.LeftOverflow + SidePadding);
        double right = Parameters.Width - Math.Max(SidePadding * 2, categoryLabels.RightOverflow + SidePadding);
        AreaPlotArea area = new AreaPlotArea(left, GetPlotTop(), right, Parameters.Height - categoryLabels.BandHeight, valueAxis, values.Count);
        SvgCanvas canvas = new SvgCanvas();
        ChartBackground.Write(canvas, Parameters.BackgroundColor, Parameters.Width, Parameters.Height);
        AreaAxesWriter.WriteGrid(canvas, area, Parameters);
        AreaSeriesWriter.WriteArea(canvas, area, values, Parameters);
        AreaAxesWriter.WriteAxes(canvas, area, Parameters);
        AreaSeriesWriter.WriteLineAndPoints(canvas, area, values, Parameters);
        categoryLabels.Write(canvas, area);
        ChartTitleWriter.Write(canvas, Parameters.Title, Parameters.TitleFontSize, Parameters.Width);
        return canvas.ToDocument(Parameters.Width, Parameters.Height, Parameters.FontFamily);
    }

    private double GetPlotTop()
    {
        double halfOfTheHighestMark = Math.Max(Parameters.LabelFontSize / 2.0, Math.Max(0, Parameters.DotRadius));
        double valueLabelHeight = Parameters.ShowValues ? Parameters.LabelFontSize + AreaCategoryLabels.LabelGap : 0;
        return ChartTitleWriter.GetHeight(Parameters.Title, Parameters.TitleFontSize) + TopPadding + halfOfTheHighestMark + valueLabelHeight;
    }
}
