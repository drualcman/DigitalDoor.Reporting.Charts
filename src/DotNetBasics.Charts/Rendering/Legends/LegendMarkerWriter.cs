namespace DotNetBasics.Charts.Rendering.Legends;

internal static class LegendMarkerWriter
{
    private const double SquareCornerRadius = 3;
    private const double LineThickness = 2;

    public static void Write(SvgCanvas canvas, LegendEntry entry, double left, double centreY, double markerSize)
    {
        if (entry.Marker == LegendMarker.Circle)
        {
            canvas.Circle(left + markerSize / 2, centreY, markerSize / 2, entry.Colour);
        }
        else if (entry.Marker == LegendMarker.Square)
        {
            canvas.Rect(left, centreY - markerSize / 2, markerSize, markerSize, entry.Colour, SquareCornerRadius);
        }
        else
        {
            canvas.Rect(left, centreY - LineThickness / 2, markerSize, LineThickness, entry.Colour);
        }
    }
}
