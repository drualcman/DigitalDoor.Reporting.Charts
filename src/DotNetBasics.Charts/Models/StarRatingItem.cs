namespace DotNetBasics.Charts.Models;

public class StarRatingItem
{
    public StarRatingItem()
    {
    }

    public StarRatingItem(string label, double stars, double count, double? percentage = null)
    {
        Label = label;
        Stars = stars;
        Count = count;
        Percentage = percentage;
    }

    public string Label { get; set; }
    public double Stars { get; set; }
    public double Count { get; set; }
    public double? Percentage { get; set; }
}
