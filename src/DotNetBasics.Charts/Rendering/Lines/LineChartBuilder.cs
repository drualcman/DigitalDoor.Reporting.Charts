namespace DotNetBasics.Charts.Rendering.Lines;

internal sealed class LineChartBuilder
{
    private const double MarginTop = 20;
    private const double MinimumMarginRight = 20;
    private const double SmallGap = 4;
    private const double LegendGap = 8;

    private readonly LineChartData Data;
    private readonly LineChartParams Parameters;
    private readonly CultureInfo Culture;

    public LineChartBuilder(LineChartData data, LineChartParams parameters, CultureInfo culture)
    {
        Data = data;
        Parameters = parameters;
        Culture = culture;
    }

    public string Build()
    {
        List<LinePlotSeries> series = LineSeriesReader.Read(Data, Parameters.MaxPointPerLine, Culture);
        int positionCount = LineSeriesReader.CountPositions(Data);
        LineValueAxis valueAxis = LineValueAxisCalculator.Calculate(series, Data?.YLabels, Parameters.StepsY, Culture);
        double widestValueLabel = valueAxis.Ticks.Max(tick => ChartTextMeasure.EstimateWidth(tick.Text, Parameters.FontSize));
        double left = (Parameters.ShowY ? widestValueLabel + LineAxesWriter.LabelGap : 0) + Parameters.AxisWidth + SmallGap;
        double estimatedPlotWidth = Parameters.Width - left - MinimumMarginRight;
        LineCategoryAxis categoryAxis = LineCategoryAxisCalculator.Calculate(Data, positionCount, Parameters, estimatedPlotWidth, Culture);
        double overflowPastThePlot = categoryAxis.RightOverflow - estimatedPlotWidth * LineCategoryAxisCalculator.PlotSidePadding;
        double marginRight = Math.Max(MinimumMarginRight, Parameters.ShowX ? overflowPastThePlot + SmallGap : 0);
        LinePlotArea area = new LinePlotArea(left, MarginTop, Parameters.Width - marginRight,
            Parameters.Height - categoryAxis.BandHeight, valueAxis, positionCount);
        LegendRowsWriter legendWriter = CreateLegendWriter(series);
        double totalHeight = Parameters.Height + (legendWriter is null ? 0 : LegendGap + legendWriter.Height);
        SvgCanvas canvas = new SvgCanvas();
        ChartBackground.Write(canvas, Parameters.BackgroundColor, Parameters.Width, totalHeight);
        LineGridWriter.Write(canvas, area, categoryAxis, Parameters);
        LineSeriesWriter.Write(canvas, area, series, Parameters);
        LineAxesWriter.Write(canvas, area, categoryAxis, Parameters);
        legendWriter?.Write(canvas, Parameters.Height + LegendGap, Parameters.Width);
        return canvas.ToDocument(Parameters.Width, totalHeight, Parameters.FontFamily);
    }

    private LegendRowsWriter CreateLegendWriter(IReadOnlyList<LinePlotSeries> series)
    {
        LegendRowsWriter result = null;
        if (Parameters.ShowLegend && series.Count > 0)
        {
            List<LegendEntry> entries = series
                .Select(lineSeries => new LegendEntry(Parameters.LegendLabel?.Invoke(lineSeries.Source) ?? lineSeries.Source.Name,
                    lineSeries.Colour, LegendMarker.Circle))
                .ToList();
            result = new LegendRowsWriter(entries, Parameters.FontSize, Parameters.Width - MinimumMarginRight);
        }
        return result;
    }
}
