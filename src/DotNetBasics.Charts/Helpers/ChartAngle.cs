namespace DotNetBasics.Charts.Helpers;

internal static class ChartAngle
{
    public static double ToRadians(double angleInDegrees)
    {
        return angleInDegrees * Math.PI / 180.0;
    }
}
