namespace DotNetBasics.Charts.Helpers;

internal static class ChartTextMeasure
{
    public const double AverageCharacterWidthFactor = 0.55;
    public const double AscentFactor = 0.75;
    public const double DescentFactor = 0.25;
    public const double VerticalCentreFactor = 0.35;
    private const string Ellipsis = "…";

    public static double EstimateWidth(string text, double fontSize)
    {
        return (text?.Length ?? 0) * fontSize * AverageCharacterWidthFactor;
    }

    public static double GetCentredBaseline(double centreY, double fontSize)
    {
        return centreY + fontSize * VerticalCentreFactor;
    }

    public static string ShortenToFit(string text, double fontSize, double availableWidth)
    {
        string result = text ?? string.Empty;
        if (EstimateWidth(result, fontSize) > availableWidth)
        {
            int charactersThatFit = (int)Math.Floor(availableWidth / (fontSize * AverageCharacterWidthFactor)) - 1;
            result = charactersThatFit > 0 ? result.Substring(0, Math.Min(charactersThatFit, result.Length)) + Ellipsis : string.Empty;
        }
        return result;
    }
}
