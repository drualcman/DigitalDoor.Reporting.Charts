namespace DotNetBasics.Charts.Rendering.Columns;

internal sealed class ColumnChartGeometry
{
    private const double ValueAreaRatio = 0.15;
    private const double BarAreaRatio = 0.70;
    private const double LabelAreaRatio = 0.15;
    private const double MinimumGapBetweenColumns = 5;
    public const double LabelGap = 10;
    public const double LabelAreaPadding = 10;

    public ColumnChartGeometry(IReadOnlyList<ChartSegment> topics, ColumnsBarChartParams parameters, ColumnLabelPlacement labelPlacement)
    {
        LabelsBesideTheColumns = labelPlacement == ColumnLabelPlacement.Left || labelPlacement == ColumnLabelPlacement.Right;
        TotalWidth = parameters.MaxWidth;
        double valueAreaHeight = parameters.Dimension * ValueAreaRatio;
        BarAreaHeight = parameters.Dimension * (LabelsBesideTheColumns ? BarAreaRatio + LabelAreaRatio : BarAreaRatio);
        double labelAreaHeight = LabelsBesideTheColumns ? 0 : GetLabelAreaHeight(topics, parameters, parameters.Dimension * LabelAreaRatio);
        LabelAreaOnTop = labelPlacement == ColumnLabelPlacement.Top ? labelAreaHeight : 0;
        double labelAreaAtTheBottom = labelPlacement == ColumnLabelPlacement.Bottom ? labelAreaHeight : 0;
        TotalHeight = LabelAreaOnTop + valueAreaHeight + BarAreaHeight + labelAreaAtTheBottom;
        BarAreaTop = LabelAreaOnTop + valueAreaHeight;
        BarBottomY = BarAreaTop + BarAreaHeight;
        SlotWidth = topics.Count > 0 ? TotalWidth / topics.Count : TotalWidth;
        ColumnWidth = Math.Max(1, Math.Min(parameters.Thickness, SlotWidth - MinimumGapBetweenColumns));
    }

    public bool LabelsBesideTheColumns { get; }
    public double TotalWidth { get; }
    public double TotalHeight { get; }
    public double BarAreaHeight { get; }
    public double LabelAreaOnTop { get; }
    public double BarAreaTop { get; }
    public double BarBottomY { get; }
    public double SlotWidth { get; }
    public double ColumnWidth { get; }

    public double GetColumnX(int columnIndex)
    {
        return columnIndex * SlotWidth + SlotWidth / 2 - ColumnWidth / 2;
    }

    private static double GetLabelAreaHeight(IReadOnlyList<ChartSegment> topics, ColumnsBarChartParams parameters, double defaultHeight)
    {
        double result = Math.Max(defaultHeight, parameters.LabelFontSize + LabelGap);
        if (parameters.RotatedLabels)
        {
            double longestLabelWidth = topics.Count > 0 ?
                topics.Max(topic => ChartTextMeasure.EstimateWidth(topic.Name, parameters.LabelFontSize)) : 0;
            double angleInRadians = ChartAngle.ToRadians(parameters.LabelRotationAngle);
            double heightTakenByTheLabel = Math.Abs(Math.Sin(angleInRadians)) * longestLabelWidth +
                Math.Abs(Math.Cos(angleInRadians)) * parameters.LabelFontSize;
            result = Math.Max(defaultHeight, heightTakenByTheLabel + LabelAreaPadding);
        }
        return result;
    }
}
