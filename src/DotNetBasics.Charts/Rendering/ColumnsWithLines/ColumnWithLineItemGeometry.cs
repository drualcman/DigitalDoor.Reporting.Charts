namespace DotNetBasics.Charts.Rendering.ColumnsWithLines;

internal sealed class ColumnWithLineItemGeometry
{
    public ColumnWithLineItemGeometry(ColumnDataItem item, double columnX, double columnWidth, double baseY, double chartHeight,
        decimal highestColumnTotal, decimal grandTotal)
    {
        Item = item;
        decimal primaryValue = Math.Max(0, item.PrimaryValue);
        decimal secondaryValue = Math.Max(0, item.SecondaryValue);
        decimal columnTotal = primaryValue + secondaryValue;
        double columnHeight = highestColumnTotal > 0 ? (double)(columnTotal / highestColumnTotal) * chartHeight : 0;
        PrimaryPercentage = columnTotal > 0 ? (double)(primaryValue * 100m / columnTotal) : 0;
        SecondaryPercentage = columnTotal > 0 ? (double)(secondaryValue * 100m / columnTotal) : 0;
        GrandTotalPercentage = grandTotal > 0 ? (double)(columnTotal * 100m / grandTotal) : 0;
        ColumnX = columnX;
        ColumnWidth = columnWidth;
        ColumnTop = baseY - columnHeight;
        PrimaryHeight = columnHeight * PrimaryPercentage / 100;
        SecondaryHeight = columnHeight - PrimaryHeight;
        CentreX = columnX + columnWidth / 2;
        GrandTotalPoint = new SvgPoint(CentreX, baseY - chartHeight * GrandTotalPercentage / 100);
        PrimaryPoint = new SvgPoint(CentreX, baseY - columnHeight * PrimaryPercentage / 100);
        SecondaryPoint = new SvgPoint(CentreX, baseY - columnHeight * SecondaryPercentage / 100);
    }

    public ColumnDataItem Item { get; }
    public double PrimaryPercentage { get; }
    public double SecondaryPercentage { get; }
    public double GrandTotalPercentage { get; }
    public double ColumnX { get; }
    public double ColumnWidth { get; }
    public double ColumnTop { get; }
    public double PrimaryHeight { get; }
    public double SecondaryHeight { get; }
    public double CentreX { get; }
    public SvgPoint GrandTotalPoint { get; }
    public SvgPoint PrimaryPoint { get; }
    public SvgPoint SecondaryPoint { get; }
}
