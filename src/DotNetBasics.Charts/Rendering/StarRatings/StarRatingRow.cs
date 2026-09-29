namespace DotNetBasics.Charts.Rendering.StarRatings;

internal sealed class StarRatingRow
{
    public StarRatingRow(string label, IReadOnlyList<double> starFillFractions, string countText, string percentageText)
    {
        Label = label ?? string.Empty;
        StarFillFractions = starFillFractions;
        CountText = countText ?? string.Empty;
        PercentageText = percentageText ?? string.Empty;
    }

    public string Label { get; }
    public IReadOnlyList<double> StarFillFractions { get; }
    public string CountText { get; }
    public string PercentageText { get; }
}
