using System.Globalization;
using DotNetBasics.Charts.Models;

namespace DotNetBasics.Charts.Samples;

public static class SampleCharts
{
    public static Dictionary<string, string> CreateAll()
    {
        List<ChartSegment> totals = SampleData.CreateTotals();
        return new Dictionary<string, string>
        {
            ["bar"] = new BarChart(totals, new ColumnsBarChartParams(showValues: true)).GenerateSvg(),
            ["bar-labels-top-total"] = new BarChart(totals, new ColumnsBarChartParams(showValues: true,
                labelPlacement: ColumnLabelPlacement.Top, fillReference: ColumnFillReference.TotalOfAllValues)).GenerateSvg(),
            ["bar-labels-left"] = new BarChart(totals, new ColumnsBarChartParams(labelPlacement: ColumnLabelPlacement.Left)).GenerateSvg(),
            ["column"] = new ColumnChart(totals, new ColumnsBarChartParams(thickness: 60, dimension: 240, showValues: true)).GenerateSvg(),
            ["column-rotated"] = new ColumnChart(totals, new ColumnsBarChartParams(thickness: 34, dimension: 240, rotatedLabels: true)).GenerateSvg(),
            ["column-rotated-bottom"] = new ColumnChart(totals, new ColumnsBarChartParams(thickness: 40, dimension: 200, rotatedLabels: true,
                labelRotationAngle: -45, labelPlacement: ColumnLabelPlacement.Bottom)).GenerateSvg(),
            ["stacked-horizontal"] = new StackedBarChart(totals, new StackedBarChartParams(showValues: true)).GenerateSvg(),
            ["stacked-vertical"] = new StackedBarChart(totals, new StackedBarChartParams(StackedBarOrientation.Vertical, thickness: 46,
                length: 320, showValues: true, labelAlignment: StackedBarLabelAlignment.Middle)).GenerateSvg(),
            ["pie"] = new PieChart(SampleData.CreatePercentages(), new PieChartParams(title: "Users")).GenerateSvg(),
            ["pie-labels"] = new PieChart(totals, new PieChartParams(width: 220, height: 220, showLabels: true, title: "Branches")).GenerateSvg(),
            ["pie-single"] = new PieChart(new[] { new ChartSegment("Everything", 100) }, new PieChartParams(showBiggestLabel: true)).GenerateSvg(),
            ["ring"] = new RingPercentageChart(69).GenerateSvg(),
            ["ring-full"] = new RingPercentageChart(100, new RingParams(width: 80, height: 80)).GenerateSvg(),
            ["line"] = new LineChart(SampleData.CreateLines()).GenerateSvg(),
            ["line-all-points"] = new LineChart(SampleData.CreateLines(), new LineChartParams(gridLineStroke: "#DDDDDD", lineSeriesWidth: 2,
                stepsY: 6, pointOptions: new LineChartPointOptions(visibleAllPoints: true, visibleMaxPointLine: true,
                visibleMinPointLine: true))).GenerateSvg(),
            ["column-with-line"] = new ColumnWithLineChart(SampleData.CreateColumnsWithLines()).GenerateSvg(),
            ["column-with-line-all"] = new ColumnWithLineChart(SampleData.CreateColumnsWithLines(),
                new ColumnWithLineChartParams { BarWidth = 30, Spacing = 25, ShowSecondaryValues = true }).GenerateSvg(),
            ["area"] = new AreaChart(SampleData.CreateMonthlyVisits()).GenerateSvg(),
            ["area-values-title"] = new AreaChart(SampleData.CreateMonthlyVisits(), new AreaChartParams(areaFill: "#59A14F",
                lineStroke: "#2E7D32", showValues: true, stepsY: 5, title: "Monthly visits")).GenerateSvg(),
            ["area-negative-rotated"] = new AreaChart(SampleData.CreateMonthlyBalance(), new AreaChartParams(width: 400, height: 260,
                areaFill: "#E15759", lineStroke: "#B22222", rotatedLabels: true, showValues: true,
                valueFormatter: value => $"{value.ToString("0", CultureInfo.InvariantCulture)} k")).GenerateSvg(),
            ["area-single"] = new AreaChart(new[] { new ChartSegment("Only month", 42) }, new AreaChartParams(width: 300, height: 200,
                showValues: true)).GenerateSvg(),
            ["stars"] = new StarRatingChart(SampleData.CreateRatings()).GenerateSvg(),
            ["stars-percentage-title"] = new StarRatingChart(SampleData.CreateRatings(), new StarRatingParams(showPercentage: true,
                title: "Customer reviews")).GenerateSvg(),
            ["stars-partial"] = new StarRatingChart(SampleData.CreateBranchRatings(), new StarRatingParams(starSize: 22, rowHeight: 30,
                width: 460, showPercentage: true, fillColor: "#FF9800", borderColor: "#E65100",
                countFormatter: count => count.ToString("N0", CultureInfo.InvariantCulture))).GenerateSvg()
        };
    }
}
