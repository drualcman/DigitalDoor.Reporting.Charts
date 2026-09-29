namespace DotNetBasics.Charts.Rendering.Bars;

internal sealed class BarRowGeometry
{
    private const double BarAreaRatio = 0.75;
    private const double TextAreaRatio = 0.25;
    private const double ValueAreaRatio = 0.30;
    private const double LabelAreaRatio = 0.70;
    public const double InnerPadding = 5;

    public BarRowGeometry(ColumnsBarChartParams parameters, ColumnLabelPlacement labelPlacement)
    {
        TotalWidth = parameters.MaxWidth;
        double textAreaWidth = TotalWidth * TextAreaRatio;
        ValueWidth = textAreaWidth * ValueAreaRatio;
        LabelWidth = textAreaWidth * LabelAreaRatio;
        LabelOnItsOwnLine = labelPlacement == ColumnLabelPlacement.Top || labelPlacement == ColumnLabelPlacement.Bottom;
        LabelLineHeight = LabelOnItsOwnLine ? parameters.LabelFontSize + InnerPadding : 0;
        RowHeight = parameters.Thickness + parameters.Gap + LabelLineHeight;
        double valueReservedWidth = parameters.ShowValues ? ValueWidth : 0;
        if (LabelOnItsOwnLine)
        {
            BarX = 0;
            BarAreaWidth = TotalWidth - valueReservedWidth;
        }
        else if (labelPlacement == ColumnLabelPlacement.Left)
        {
            BarX = LabelWidth;
            BarAreaWidth = TotalWidth - LabelWidth - valueReservedWidth;
        }
        else
        {
            BarX = 0;
            BarAreaWidth = TotalWidth * BarAreaRatio;
        }
    }

    public double TotalWidth { get; }
    public double ValueWidth { get; }
    public double LabelWidth { get; }
    public bool LabelOnItsOwnLine { get; }
    public double LabelLineHeight { get; }
    public double RowHeight { get; }
    public double BarX { get; }
    public double BarAreaWidth { get; }
}
