namespace DotNetBasics.Charts.Helpers;

internal static class ChartSegmentColours
{
    public static string GetBackground(ChartSegment segment, IReadOnlyList<ChartColor> palette, int segmentIndex)
    {
        return string.IsNullOrWhiteSpace(segment.ChartColor) ? GetPaletteColour(palette, segmentIndex).Background : segment.ChartColor;
    }

    public static string GetForeground(ChartSegment segment, IReadOnlyList<ChartColor> palette, int segmentIndex)
    {
        string result = segment.LabelColor;
        if (string.IsNullOrWhiteSpace(result))
        {
            result = string.IsNullOrWhiteSpace(segment.ChartColor) ?
                GetPaletteColour(palette, segmentIndex).Foreground : ColourContrast.GetContrastingColour(segment.ChartColor);
        }
        return result;
    }

    private static ChartColor GetPaletteColour(IReadOnlyList<ChartColor> palette, int segmentIndex)
    {
        return palette[segmentIndex % palette.Count];
    }
}
