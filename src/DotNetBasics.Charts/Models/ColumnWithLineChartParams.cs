namespace DotNetBasics.Charts.Models;

public class ColumnWithLineChartParams
{
    public int Width { get; set; } = 800;
    public int Height { get; set; } = 500;
    public string BackgroundColor { get; set; } = "transparent";
    public int BarWidth { get; set; } = 15;
    public int MinBarHeight { get; set; } = 150;
    public int Spacing { get; set; } = 15;
    public string PrimaryColor { get; set; } = "#4e79a7";
    public string SecondaryColor { get; set; } = "#f28e2b";
    public string GrandTotalLineColor { get; set; } = "#e15759";
    public string PrimaryPercentageLineColor { get; set; } = "#59a84b";
    public string SecondaryPercentageLineColor { get; set; } = "#ed49ff";
    public string AxisColor { get; set; } = "#ccc";
    public bool ShowTitle { get; set; } = true;
    public bool ShowLegend { get; set; } = true;
    public bool ShowGranTotal { get; set; } = true;
    public bool ShowPrimaryValues { get; set; } = true;
    public bool ShowSecondaryValues { get; set; } = false;
    public string GrandTotalLegend { get; set; } = "% of Grand Total";
    public int LabelFontSize { get; set; } = 10;
    public int TitleFontSize { get; set; } = 16;
    public int LegendFontSize { get; set; } = 12;
    public string FontFamily { get; set; } = ChartFonts.DefaultFamily;
    public Func<ColumnDataItem, string> BigTotalValueLabelFormatter { get; set; }
    public Func<ColumnDataItem, string> PrimaryValueLabelFormatter { get; set; }
    public Func<ColumnDataItem, string> SecondaryValueLabelFormatter { get; set; }
    public Func<ColumnDataItem, string> BottomLabelFormatter { get; set; }
}
