namespace DotNetBasics.Charts;

public class StarRatingChart
{
    private readonly IReadOnlyList<StarRatingItem> Items;
    private readonly StarRatingParams Parameters;

    public StarRatingChart(IEnumerable<StarRatingItem> items, StarRatingParams parameters = null)
    {
        Items = (items ?? Enumerable.Empty<StarRatingItem>()).Where(item => item is not null).ToList();
        Parameters = parameters ?? new StarRatingParams();
    }

    public string GenerateSvg()
    {
        return new StarRatingBuilder(Items, Parameters).Build();
    }
}
