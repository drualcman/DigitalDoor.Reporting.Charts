namespace DotNetBasics.Charts.Rendering.Pies;

internal static class PieArcPath
{
    private const double FullTurnTolerance = 0.0001;

    public static string Create(SvgPoint centre, double radius, double startAngle, double endAngle)
    {
        string pathData;
        if (endAngle - startAngle >= 360 - FullTurnTolerance)
        {
            SvgPoint top = GetPoint(centre, radius, -90);
            SvgPoint bottom = GetPoint(centre, radius, 90);
            pathData = $"M {Format(top)} A {SvgNumber.Format(radius)} {SvgNumber.Format(radius)} 0 1 1 {Format(bottom)} " +
                $"A {SvgNumber.Format(radius)} {SvgNumber.Format(radius)} 0 1 1 {Format(top)} Z";
        }
        else
        {
            SvgPoint start = GetPoint(centre, radius, startAngle);
            SvgPoint end = GetPoint(centre, radius, endAngle);
            int largeArcFlag = endAngle - startAngle > 180 ? 1 : 0;
            pathData = $"M {Format(centre)} L {Format(start)} A {SvgNumber.Format(radius)} {SvgNumber.Format(radius)} 0 " +
                $"{largeArcFlag} 1 {Format(end)} Z";
        }
        return pathData;
    }

    public static SvgPoint GetPoint(SvgPoint centre, double distance, double angleInDegrees)
    {
        double angleInRadians = ChartAngle.ToRadians(angleInDegrees);
        return new SvgPoint(centre.X + distance * Math.Cos(angleInRadians), centre.Y + distance * Math.Sin(angleInRadians));
    }

    private static string Format(SvgPoint point)
    {
        return $"{SvgNumber.Format(point.X)} {SvgNumber.Format(point.Y)}";
    }
}
