namespace DotNetBasics.Charts.Rendering.StarRatings;

internal sealed class StarRatingLayout
{
    private const double SidePadding = 8;
    private const double VerticalPadding = 4;
    private const double ColumnGap = 12;
    private const double StarGapRatio = 0.25;
    private const double MinimumSpaceAroundTheRowContent = 4;

    public StarRatingLayout(IReadOnlyList<StarRatingRow> rows, StarRatingParams parameters)
    {
        double fontSize = parameters.LabelFontSize;
        StarSize = Math.Max(1, parameters.StarSize);
        StarGap = StarSize * StarGapRatio;
        int starCount = Math.Max(0, parameters.StarCount);
        double starsWidth = starCount > 0 ? starCount * StarSize + (starCount - 1) * StarGap : 0;
        double countWidth = parameters.ShowCount ? GetWidestText(rows.Select(row => row.CountText), fontSize) + ColumnGap : 0;
        double percentageWidth = parameters.ShowPercentage ? GetWidestText(rows.Select(row => row.PercentageText), fontSize) + ColumnGap : 0;
        double minimumWidth = SidePadding * 2 + starsWidth + countWidth + percentageWidth;
        TotalWidth = Math.Max(parameters.Width, minimumWidth);
        PercentageRight = TotalWidth - SidePadding;
        CountRight = PercentageRight - percentageWidth;
        StarsLeft = CountRight - countWidth - starsWidth;
        LabelLeft = SidePadding;
        LabelWidth = Math.Max(0, StarsLeft - ColumnGap - SidePadding);
        RowHeight = Math.Max(parameters.RowHeight, Math.Max(StarSize, fontSize) + MinimumSpaceAroundTheRowContent);
        Top = ChartTitleWriter.GetHeight(parameters.Title, parameters.TitleFontSize) + VerticalPadding;
        TotalHeight = Top + rows.Count * RowHeight + VerticalPadding;
    }

    public double TotalWidth { get; }
    public double TotalHeight { get; }
    public double Top { get; }
    public double RowHeight { get; }
    public double StarSize { get; }
    public double StarGap { get; }
    public double LabelLeft { get; }
    public double LabelWidth { get; }
    public double StarsLeft { get; }
    public double CountRight { get; }
    public double PercentageRight { get; }

    public double GetRowCentreY(int rowIndex)
    {
        return Top + rowIndex * RowHeight + RowHeight / 2;
    }

    public double GetStarLeft(int starIndex)
    {
        return StarsLeft + starIndex * (StarSize + StarGap);
    }

    private static double GetWidestText(IEnumerable<string> texts, double fontSize)
    {
        return texts.Select(text => ChartTextMeasure.EstimateWidth(text, fontSize)).DefaultIfEmpty(0).Max();
    }
}
