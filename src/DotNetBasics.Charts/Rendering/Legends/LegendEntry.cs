namespace DotNetBasics.Charts.Rendering.Legends;

internal sealed class LegendEntry
{
    public LegendEntry(string text, string colour, LegendMarker marker)
    {
        Text = text;
        Colour = colour;
        Marker = marker;
    }

    public string Text { get; }
    public string Colour { get; }
    public LegendMarker Marker { get; }
}
