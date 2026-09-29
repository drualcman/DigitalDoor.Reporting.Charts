namespace DotNetBasics.Charts.Helpers;

internal static class ChartColourPalette
{
    public const int DefaultTotalColours = 256;
    public const int DefaultHueSeparation = 30;

    public static List<ChartColor> Create(int totalColours, int hueSeparation)
    {
        List<ChartColor> colours = new List<ChartColor>();
        int hueIncrement = 360 / totalColours + hueSeparation;
        for (int colourIndex = 0; colourIndex < totalColours; colourIndex++)
        {
            int hue = colourIndex * hueIncrement % 360;
            int saturation = 80 + colourIndex % 2 * 20;
            int lightness = 40 + colourIndex % 3 * 20;
            colours.Add(new ChartColor($"hsl({hue}, {saturation}%, {lightness}%)"));
        }
        return colours;
    }

    public static List<ChartColor> CreateOrCopy(IEnumerable<ChartColor> chartColours, int hueSeparation)
    {
        List<ChartColor> colours = chartColours?.Where(colour => colour is not null).ToList();
        return colours is not null && colours.Count > 0 ? colours : Create(DefaultTotalColours, hueSeparation);
    }
}
