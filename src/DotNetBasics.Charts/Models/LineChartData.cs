namespace DotNetBasics.Charts.Models;

public class LineChartData
{
    public LineChartData(IEnumerable<LineData> data, IEnumerable<string> xLabels = null, IEnumerable<string> yLabels = null)
    {
        Data = data ?? Enumerable.Empty<LineData>();
        XLabels = xLabels ?? Enumerable.Empty<string>();
        YLabels = yLabels ?? Enumerable.Empty<string>();
    }

    public IEnumerable<string> XLabels { get; set; }
    public IEnumerable<string> YLabels { get; set; }
    public IEnumerable<LineData> Data { get; set; }
}
