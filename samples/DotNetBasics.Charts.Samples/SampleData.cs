using DotNetBasics.Charts.Models;

namespace DotNetBasics.Charts.Samples;

public static class SampleData
{
    public static List<ChartSegment> CreateTotals()
    {
        return new List<ChartSegment>
        {
            new ChartSegment("Tagum", 1204),
            new ChartSegment("Asuncion", 486),
            new ChartSegment("Davao & Mati", 312),
            new ChartSegment("Panabo", 198),
            new ChartSegment("New Corella with a long name", 75)
        };
    }

    public static List<ChartSegment> CreatePercentages()
    {
        return new List<ChartSegment>
        {
            new ChartSegment("Active", 45),
            new ChartSegment("Pending", 25),
            new ChartSegment("Blocked", 18),
            new ChartSegment("Deleted", 12)
        };
    }

    public static LineChartData CreateLines()
    {
        return new LineChartData(new List<LineData>
        {
            new LineData("Line 1", "green", new List<string> { "10", "50", "15", "100", "20", "30", "25" }),
            new LineData("Line 2", "blue", new List<string> { "0", "5", "50", "33", "33", "8", "12" }),
            new LineData("Line 3", "red", new List<string> { "5", "50", "10", "33", "8", "12", "15" })
        }, new List<string> { "January", "February", "March", "April", "May", "June", "July" });
    }

    public static List<ChartSegment> CreateMonthlyVisits()
    {
        return new List<ChartSegment>
        {
            new ChartSegment("Jan", 120),
            new ChartSegment("Feb", 180),
            new ChartSegment("Mar", 150),
            new ChartSegment("Apr", 260),
            new ChartSegment("May", 310),
            new ChartSegment("Jun", 240),
            new ChartSegment("Jul", 380)
        };
    }

    public static List<ChartSegment> CreateMonthlyBalance()
    {
        return new List<ChartSegment>
        {
            new ChartSegment("Q1 2026", 40),
            new ChartSegment("Q2 2026", -25),
            new ChartSegment("Q3 2026", 15),
            new ChartSegment("Q4 2026", 65)
        };
    }

    public static List<StarRatingItem> CreateRatings()
    {
        return new List<StarRatingItem>
        {
            new StarRatingItem("Excellent", 5, 128),
            new StarRatingItem("Very good", 4, 64),
            new StarRatingItem("Average", 3, 22),
            new StarRatingItem("Poor", 2, 9),
            new StarRatingItem("Terrible", 1, 4)
        };
    }

    public static List<StarRatingItem> CreateBranchRatings()
    {
        return new List<StarRatingItem>
        {
            new StarRatingItem("Tagum", 4.6, 1204),
            new StarRatingItem("Asuncion", 3.5, 486),
            new StarRatingItem("Davao & Mati", 4.25, 312),
            new StarRatingItem("New Corella with a long name", 2.8, 75)
        };
    }

    public static ColumnWithLineChartData CreateColumnsWithLines()
    {
        return new ColumnWithLineChartData(new List<ColumnDataItem>
        {
            new ColumnDataItem("JAN", 200000, 184615),
            new ColumnDataItem("FEB", 300000, 245454),
            new ColumnDataItem("MAR", 400000, 289655),
            new ColumnDataItem("APR", 500000, 319672),
            new ColumnDataItem("MAY", 600000, 452830),
            new ColumnDataItem("JUN", 700000, 466666)
        })
        {
            PrimaryLegend = "Active Users",
            SecondaryLegend = "Non Active Users",
            Title = "Total users"
        };
    }
}
