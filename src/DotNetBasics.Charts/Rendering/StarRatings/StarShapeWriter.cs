namespace DotNetBasics.Charts.Rendering.StarRatings;

internal static class StarShapeWriter
{
    private const double StarBoxSize = 24;
    private const double StarShapeLeft = 2;
    private const double StarShapeWidth = 20;
    private const double BorderWidth = 1;

    private static readonly double[] StarOutlineCoordinates = { 12, 2, 15, 9, 22, 9, 17, 14, 19, 21, 12, 17, 5, 21, 7, 14, 2, 9, 9, 9 };

    public static void Write(SvgCanvas canvas, double left, double top, double size, double fillFraction, StarRatingParams parameters)
    {
        List<SvgPoint> points = CreatePoints(left, top, size);
        SvgStroke border = new SvgStroke(parameters.BorderColor, BorderWidth);
        string baseFill = fillFraction >= 1 ? parameters.FillColor : parameters.EmptyColor;
        canvas.Polygon(points, new SvgPolygonStyle(baseFill, border));
        if (fillFraction > 0 && fillFraction < 1)
        {
            double scale = size / StarBoxSize;
            double clipLeft = left + StarShapeLeft * scale;
            double clipWidth = StarShapeWidth * scale * fillFraction;
            string clipPathId = StableIdentifier.Create("star-clip",
                $"{SvgNumber.Format(clipLeft)}|{SvgNumber.Format(top)}|{SvgNumber.Format(clipWidth)}|{SvgNumber.Format(size)}");
            canvas.RectangleClipPath(clipPathId, clipLeft, top, clipWidth, size);
            canvas.Polygon(points, new SvgPolygonStyle(parameters.FillColor, border) { ClipPathId = clipPathId });
        }
    }

    private static List<SvgPoint> CreatePoints(double left, double top, double size)
    {
        double scale = size / StarBoxSize;
        return Enumerable.Range(0, StarOutlineCoordinates.Length / 2)
            .Select(pointIndex => new SvgPoint(left + StarOutlineCoordinates[pointIndex * 2] * scale,
                top + StarOutlineCoordinates[pointIndex * 2 + 1] * scale))
            .ToList();
    }
}
