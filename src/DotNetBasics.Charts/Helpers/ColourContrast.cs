namespace DotNetBasics.Charts.Helpers;

internal static class ColourContrast
{
    private const string DarkForeground = "#000000";
    private const string LightForeground = "#FFFFFF";
    private const double BrightnessThreshold = 150;

    public static string GetContrastingColour(string colour)
    {
        int[] rgb = ColourParser.ReadRgbOrNull(colour);
        string result = DarkForeground;
        if (rgb is not null)
        {
            double perceivedBrightness = rgb[0] * 0.299 + rgb[1] * 0.587 + rgb[2] * 0.114;
            result = perceivedBrightness >= BrightnessThreshold ? DarkForeground : LightForeground;
        }
        return result;
    }
}
