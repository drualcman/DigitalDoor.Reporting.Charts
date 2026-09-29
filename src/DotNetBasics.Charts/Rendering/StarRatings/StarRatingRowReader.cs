namespace DotNetBasics.Charts.Rendering.StarRatings;

internal static class StarRatingRowReader
{
    public static List<StarRatingRow> Read(IReadOnlyList<StarRatingItem> items, StarRatingParams parameters)
    {
        int starCount = Math.Max(0, parameters.StarCount);
        double totalCount = items.Sum(item => ReadFiniteOrZero(item.Count));
        return items.Select(item => new StarRatingRow(
                item.Label,
                GetStarFillFractions(ReadFiniteOrZero(item.Stars), starCount),
                ChartValueFormat.Format(ReadFiniteOrZero(item.Count), parameters.CountFormatter),
                FormatPercentage(GetPercentage(item, totalCount), parameters.PercentageFormatter)))
            .ToList();
    }

    private static List<double> GetStarFillFractions(double stars, int starCount)
    {
        return Enumerable.Range(0, starCount).Select(starIndex => Math.Max(0, Math.Min(1, stars - starIndex))).ToList();
    }

    private static double GetPercentage(StarRatingItem item, double totalCount)
    {
        double calculatedPercentage = totalCount > 0 ? ReadFiniteOrZero(item.Count) / totalCount * 100 : 0;
        return item.Percentage.HasValue ? ReadFiniteOrZero(item.Percentage.Value) : calculatedPercentage;
    }

    private static string FormatPercentage(double percentage, Func<double, string> percentageFormatter)
    {
        return percentageFormatter is null ? ChartValueFormat.FormatPercentage(percentage / 100) : percentageFormatter(percentage);
    }

    private static double ReadFiniteOrZero(double value)
    {
        return double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;
    }
}
