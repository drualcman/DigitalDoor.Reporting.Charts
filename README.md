[![Nuget](https://img.shields.io/nuget/v/DotNetBasics.Charts?style=for-the-badge)](https://www.nuget.org/packages/DotNetBasics.Charts)
[![Nuget](https://img.shields.io/nuget/dt/DotNetBasics.Charts?style=for-the-badge)](https://www.nuget.org/packages/DotNetBasics.Charts)

# Description
Simple charts generated as SVG from .NET code: bar, column, stacked bar, pie, ring percentage, line, area, column with line and star rating charts.
It has no dependencies and works in any .NET application: ASP.NET Core, Blazor Server, Blazor WebAssembly, MVC, Web API and console apps.

Every chart returns an SVG `string`. You can save it as a file, return it from an API, write it in a web page, or use it as an image in a PDF or HTML report
(for example as the value of a cell in [DigitalDoor.Reporting](https://www.nuget.org/packages/DigitalDoor.Reporting), where it is drawn as a vector).

The charts are static: there are no animations, tooltips or clicks. If you need interactive charts in Blazor, use
[BlazorBasics.Charts](https://www.nuget.org/packages/BlazorBasics.Charts), which has the same charts and the same parameters.

# How to use
```csharp
using DotNetBasics.Charts;
using DotNetBasics.Charts.Models;

List<ChartSegment> totals =
[
    new ChartSegment("Tagum", 1204),
    new ChartSegment("Asuncion", 486),
    new ChartSegment("Panabo", 198)
];

string svg = new BarChart(totals, new ColumnsBarChartParams(showValues: true)).GenerateSvg();
File.WriteAllText("totals.svg", svg);
```

Every SVG has a fixed `width` and `height` in pixels and a `viewBox`, so it keeps its proportions when it is scaled to fit an image, a report cell or a page.
Numbers are always written with the invariant culture, and texts are escaped.

## Bar and column charts
```csharp
string bars = new BarChart(totals, new ColumnsBarChartParams()).GenerateSvg();
string columns = new ColumnChart(totals, new ColumnsBarChartParams(thickness: 34, dimension: 240, rotatedLabels: true)).GenerateSvg();
```
``` csharp
public ColumnsBarChartParams(
    string backgroundColour = "#D3D3D3",
    int thickness = 20,          // height of a bar or width of a column
    int dimension = 100,         // height of the column chart
    bool showValues = false,
    IEnumerable<ChartColor> chartColours = null,
    int gap = 5,                 // space between bars
    int maxWidth = 600,          // width of the chart
    bool rotatedLabels = false,  // column chart only
    double labelRotationAngle = VERTICAL_LABEL_ANGLE,
    int labelFontSize = 12,
    ColumnLabelPlacement? labelPlacement = null,
    ColumnFillReference fillReference = ColumnFillReference.HighestValue,
    Func<double, string> valueFormatter = null,
    string fontFamily = ChartFonts.DefaultFamily)
```
* `FillReference`: with `HighestValue` the biggest value fills its shape and the rest are drawn against it. With `TotalOfAllValues` each value fills its share of the total.
* `LabelPlacement`: `Bottom`, `Top`, `Left` or `Right`. Left `null`, the column chart writes the labels under the columns (or beside them when they are rotated) and the bar chart at the end of every bar.
* Labels that do not fit in their space are shortened with an ellipsis.
* `ValueFormatter` writes the values the way you want, for example `value => value.ToString("C", culture)`.

## Stacked bar chart
Every value is a slice of one single bar, so what is read is the share of each value.
```csharp
string stacked = new StackedBarChart(totals, new StackedBarChartParams(StackedBarOrientation.Vertical, showValues: true)).GenerateSvg();
```
``` csharp
public StackedBarChartParams(
    StackedBarOrientation orientation = StackedBarOrientation.Horizontal,
    int thickness = 40,
    int length = 600,
    string backgroundColour = "#D3D3D3",
    IEnumerable<ChartColor> chartColours = null,
    bool showValues = false,
    StackedBarLabelSide labelSide = StackedBarLabelSide.After,        // Before, After or Inside
    StackedBarLabelAlignment labelAlignment = StackedBarLabelAlignment.Start,
    int labelFontSize = 12,
    double minimumLabelShare = 0.03,  // segments smaller than this share are drawn without label
    double total = 0,                 // 0 = the sum of the values
    Func<double, string> valueFormatter = null,
    string fontFamily = ChartFonts.DefaultFamily)
```

## Pie chart
The values can be amounts or percentages: every slice shows its share of the total, and the legend writes it as a percentage.
```csharp
string pie = new PieChart(segments, new PieChartParams(title: "Users", showLabels: true)).GenerateSvg();
```
``` csharp
public PieChartParams(int width = 150, int height = 150,
    int separationOffset = 15,               // distance of the separated slice
    string title = "",                       // written over the legend
    IEnumerable<ChartColor> chartColours = null,
    bool showLabels = false,                 // a label with the percentage over every slice
    double centerTextSeparationPercentage = 0.85,
    bool separateBiggerByDefault = true,     // separates the biggest slice, or the one with IsSelected
    bool showBiggestLabel = false,
    bool showLegend = true,
    int labelFontSize = 12, int legendFontSize = 12,
    string fontFamily = ChartFonts.DefaultFamily)
```
`ChartSegment.SetTitleTopic` changes the text of the label of a slice.

## Ring percentage
```csharp
string ring = new RingPercentageChart(69, new RingParams(width: 100, height: 100)).GenerateSvg();
```
``` csharp
public RingParams(int width = 120, int height = 120, double fontPerspective = 3.5, string labelColor = "green",
    string fromColor = "#FFD700", string toColor = "#B22222", string circunferenceColour = "#eee", int strokeWidth = 10,
    string fontFamily = ChartFonts.DefaultFamily)
```

## Line chart
```csharp
LineChartData data = new(new List<LineData>
{
    new LineData("Line 1", "green", new List<string> { "10", "50", "15", "100", "20", "30", "25" }),
    new LineData("Line 2", "blue",  new List<string> { "0",  "5",  "50", "33",  "33", "8",  "12" })
}, xLabels: new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul" });

string lines = new LineChart(data, new LineChartParams(stepsY: 6), CultureInfo.InvariantCulture).GenerateSvg();
```
* The values are parsed with the culture given to the constructor. Values that are not numbers are skipped.
* The series are drawn in the order they are given, and the legend is written under the chart.
* `StepsY` is the number of ticks of the value axis (up to 20); a bigger number is used as the distance between ticks. The ticks are rounded (0, 20, 40...).
* The X labels are rotated by `RotationAngleXLabel` (0 to 90 degrees) when `RotatedXLabels` is on or when they do not fit, and some of them are skipped when there is no room for all.
* `LegendLabel` returns the text of each series in the legend. `Height` is the height of the chart without the legend.

``` csharp
public LineChartParams(int width = 600, int height = 300, string backgroundColor = "transparent",
    string axisStroke = "black", int axisWidth = 2, string gridLineStroke = "black", int gridWidth = 1,
    string lineSeriesFill = "none", int lineSeriesWidth = 1, int dotRadius = 4, int stepsY = 3,
    bool showX = true, bool showY = true, bool showLegend = true, bool rotatedXLabels = false,
    double rotationAngleXLabel = 45, Func<LineData, string> legendLabel = null,
    LineChartPointOptions pointOptions = null, int maxPointPerLine = 50,
    bool showXLines = true, bool showYLines = true, int fontSize = 12,
    string fontFamily = ChartFonts.DefaultFamily)

public LineChartPointOptions(bool visibleAllPoints = false, bool visibleMaxPoint = true, bool visibleMinPoint = true,
    bool visibleMaxPointLine = false, bool visibleMinPointLine = false)
```

## Area chart
The categories go along the X axis (the `Name` of every segment is its label) and the values along the Y axis, up to the highest value.
The area is filled from the baseline to the line that joins the points, and the line is drawn over it.
```csharp
List<ChartSegment> visits =
[
    new ChartSegment("Jan", 120),
    new ChartSegment("Feb", 180),
    new ChartSegment("Mar", 150),
    new ChartSegment("Apr", 260)
];

string area = new AreaChart(visits, new AreaChartParams(showValues: true, title: "Monthly visits")).GenerateSvg();
```
``` csharp
public AreaChartParams(
    int width = 600,
    int height = 300,                 // the title is inside this height
    string backgroundColor = "transparent",
    string areaFill = "#4E79A7",
    double areaOpacity = 0.4,         // 0 to 1
    string lineStroke = "#2F5B85",    // also the colour of the points
    int lineWidth = 2,                // 0 = no line
    bool showPoints = true,
    int dotRadius = 4,
    bool showValues = false,          // the value over every point
    bool showLabels = true,           // the category labels under the X axis
    bool rotatedLabels = false,
    double labelRotationAngle = 45,   // 0 to 90 degrees
    int stepsY = 3,                   // ticks of the value axis, as in the line chart
    bool showYAxis = true,
    bool showGridLines = true,
    string axisColor = "#333333",
    string gridLineColor = "#DDDDDD",
    string title = "",                // written inside the SVG when it is not empty
    int labelFontSize = 12,
    int titleFontSize = 16,
    Func<double, string> valueFormatter = null,   // values and value axis
    string fontFamily = ChartFonts.DefaultFamily)
```
* The value axis starts at 0, or below 0 when there are negative values, and uses rounded ticks.
* With one single value the area is a flat band across the chart. Without values only the axes are drawn.
* The labels are rotated when `rotatedLabels` is on or when they do not fit; otherwise they are shortened with an ellipsis.

## Star rating
One row per item with its label, the stars, the count and, optionally, the percentage. Ratings with decimals fill part of a star.
```csharp
List<StarRatingItem> ratings =
[
    new StarRatingItem("Excellent", 5, 128),
    new StarRatingItem("Very good", 4, 64),
    new StarRatingItem("Average", 3.5, 22),
    new StarRatingItem("Poor", 2, 9, percentage: 4)
];

string stars = new StarRatingChart(ratings, new StarRatingParams(showPercentage: true, title: "Customer reviews")).GenerateSvg();
```
``` csharp
public StarRatingItem(string label, double stars, double count, double? percentage = null)

public StarRatingParams(
    int starCount = 5,
    int starSize = 16,
    string fillColor = "#FFD700",
    string emptyColor = "#E0E0E0",
    string borderColor = "#B8860B",
    bool showCount = true,
    bool showPercentage = false,
    int rowHeight = 24,              // grows when the stars or the text do not fit
    int width = 400,                 // grows when the stars and numbers do not fit
    int labelFontSize = 12,
    string title = "",               // written inside the SVG when it is not empty
    int titleFontSize = 16,
    Func<double, string> countFormatter = null,
    Func<double, string> percentageFormatter = null,   // receives 0 to 100
    string fontFamily = ChartFonts.DefaultFamily)
```
* `Stars` is clamped between 0 and `starCount`. A star is partially filled with an SVG `clipPath`, with a stable id, so it works in browsers, PDF and image converters.
* The percentage of every row is `Count` divided by the total of all the counts, unless `Percentage` is given. By default it is written as `56.39%`.
* The count and the percentage are right aligned, and long labels are shortened with an ellipsis.

## Column with lines
```csharp
ColumnWithLineChartData data = new(new List<ColumnDataItem>
{
    new("JAN", 200000, 184615),
    new("FEB", 300000, 245454),
    new("MAR", 400000, 289655)
})
{
    PrimaryLegend = "Active Users",
    SecondaryLegend = "Non Active Users",
    Title = "Total users"
};

string chart = new ColumnWithLineChart(data, new ColumnWithLineChartParams { ShowSecondaryValues = true }).GenerateSvg();
```
The title and the legend are written inside the SVG. `ShowGranTotal`, `ShowPrimaryValues` and `ShowSecondaryValues` draw each line with its percentages,
and the `...LabelFormatter` functions change the texts. `GrandTotalLegend` is the text of the grand total in the legend.

# Colours
The colours are set automatically, but you can give your own list with `chartColours` or set `ChartColor` in every segment.
A colour can be `#RGB`, `#RRGGBB`, `rgb(0,0,0)`, `hsl(1,80%,40%)` or a colour name.
The text written over a colour is black or white, whichever reads better, unless you give it with `new ChartColor(background, foreground)` or `ChartSegment.LabelColor`.

# Fonts
The texts use `font-family="Arial, Helvetica, sans-serif"` (`ChartFonts.DefaultFamily`). Change it with the `fontFamily` parameter of every chart.
Text widths are estimated, because there is no font measuring outside a browser, so leave some margin with very long labels.

# Differences with BlazorBasics.Charts
* There are no animations, tooltips, clicks or loading indicators, and the parameters that only did that are not here.
* Titles and legends are part of the SVG instead of HTML around it.
* The size of every chart is fixed in pixels instead of a percentage of the page.
* The pie chart accepts any values and shows the share of each one.
* The series of the line chart keep their order, and the value axis uses rounded ticks.

# License
This project is licensed under the MIT License.

# Made with love by DrUalcman

If you find this library useful, please consider giving it a star on GitHub!
