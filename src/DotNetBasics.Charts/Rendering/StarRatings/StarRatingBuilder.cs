namespace DotNetBasics.Charts.Rendering.StarRatings;

internal sealed class StarRatingBuilder
{
    private readonly IReadOnlyList<StarRatingItem> Items;
    private readonly StarRatingParams Parameters;

    public StarRatingBuilder(IReadOnlyList<StarRatingItem> items, StarRatingParams parameters)
    {
        Items = items;
        Parameters = parameters;
    }

    public string Build()
    {
        List<StarRatingRow> rows = StarRatingRowReader.Read(Items, Parameters);
        StarRatingLayout layout = new StarRatingLayout(rows, Parameters);
        SvgCanvas canvas = new SvgCanvas();
        ChartTitleWriter.Write(canvas, Parameters.Title, Parameters.TitleFontSize, layout.TotalWidth);
        for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            WriteRow(canvas, layout, rows[rowIndex], rowIndex);
        }
        return canvas.ToDocument(layout.TotalWidth, layout.TotalHeight, Parameters.FontFamily);
    }

    private void WriteRow(SvgCanvas canvas, StarRatingLayout layout, StarRatingRow row, int rowIndex)
    {
        double fontSize = Parameters.LabelFontSize;
        double centreY = layout.GetRowCentreY(rowIndex);
        double textBaseline = ChartTextMeasure.GetCentredBaseline(centreY, fontSize);
        canvas.Text(ChartTextMeasure.ShortenToFit(row.Label, fontSize, layout.LabelWidth), layout.LabelLeft, textBaseline,
            new SvgTextStyle(fontSize, SvgTextAnchor.Start));
        for (int starIndex = 0; starIndex < row.StarFillFractions.Count; starIndex++)
        {
            StarShapeWriter.Write(canvas, layout.GetStarLeft(starIndex), centreY - layout.StarSize / 2, layout.StarSize,
                row.StarFillFractions[starIndex], Parameters);
        }
        if (Parameters.ShowCount)
        {
            canvas.Text(row.CountText, layout.CountRight, textBaseline, new SvgTextStyle(fontSize, SvgTextAnchor.End));
        }
        if (Parameters.ShowPercentage)
        {
            canvas.Text(row.PercentageText, layout.PercentageRight, textBaseline, new SvgTextStyle(fontSize, SvgTextAnchor.End));
        }
    }
}
