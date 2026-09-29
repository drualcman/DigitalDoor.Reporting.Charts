namespace DotNetBasics.Charts.Rendering.Lines;

internal sealed class LinePlotSeries
{
    private const string DefaultColour = "black";

    public LinePlotSeries(LineData source, IReadOnlyList<LinePlotPoint> points)
    {
        Source = source;
        Points = points;
        Colour = string.IsNullOrWhiteSpace(source.Color) ? DefaultColour : source.Color;
        MinimumPoint = points.OrderBy(point => point.Value).ThenBy(point => point.Index).FirstOrDefault();
        MaximumPoint = points.OrderByDescending(point => point.Value).ThenBy(point => point.Index).FirstOrDefault();
    }

    public LineData Source { get; }
    public IReadOnlyList<LinePlotPoint> Points { get; }
    public string Colour { get; }
    public LinePlotPoint MinimumPoint { get; }
    public LinePlotPoint MaximumPoint { get; }
}
