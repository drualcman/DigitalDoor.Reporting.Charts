namespace DotNetBasics.Charts.Helpers;

internal static class NiceNumber
{
    public static double Round(double roughStep)
    {
        double result = 1;
        if (roughStep > 0 && !double.IsInfinity(roughStep))
        {
            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(roughStep)));
            double fraction = roughStep / magnitude;
            double niceFraction = fraction <= 1 ? 1 : fraction <= 2 ? 2 : fraction <= 2.5 ? 2.5 : fraction <= 5 ? 5 : 10;
            result = niceFraction * magnitude;
        }
        return result;
    }
}
