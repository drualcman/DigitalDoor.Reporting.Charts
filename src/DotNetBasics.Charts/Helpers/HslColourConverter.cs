namespace DotNetBasics.Charts.Helpers;

internal static class HslColourConverter
{
    public static string ToHex(double hue, double saturation, double lightness)
    {
        double normalizedHue = (hue % 360 + 360) % 360;
        double chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
        double huePrime = normalizedHue / 60.0;
        double secondComponent = chroma * (1 - Math.Abs(huePrime % 2 - 1));
        double[] components = GetComponents((int)Math.Floor(huePrime), chroma, secondComponent);
        double lightnessMatch = lightness - chroma / 2;
        return $"#{ToByte(components[0] + lightnessMatch):X2}{ToByte(components[1] + lightnessMatch):X2}{ToByte(components[2] + lightnessMatch):X2}";
    }

    private static double[] GetComponents(int hueSector, double chroma, double secondComponent)
    {
        double[] components = hueSector switch
        {
            0 => new[] { chroma, secondComponent, 0 },
            1 => new[] { secondComponent, chroma, 0 },
            2 => new[] { 0, chroma, secondComponent },
            3 => new[] { 0, secondComponent, chroma },
            4 => new[] { secondComponent, 0, chroma },
            _ => new[] { chroma, 0, secondComponent }
        };
        return components;
    }

    private static int ToByte(double component)
    {
        return Math.Max(0, Math.Min(255, (int)Math.Round(component * 255)));
    }
}
