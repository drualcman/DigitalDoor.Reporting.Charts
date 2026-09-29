namespace DotNetBasics.Charts.Rendering.Pies;

internal sealed class PieCalloutArea
{
    public PieCalloutArea(SvgPoint centre, double radius, double offset, double documentWidth, double documentHeight)
    {
        Centre = centre;
        Radius = radius;
        Offset = offset;
        DocumentWidth = documentWidth;
        DocumentHeight = documentHeight;
    }

    public SvgPoint Centre { get; }
    public double Radius { get; }
    public double Offset { get; }
    public double DocumentWidth { get; }
    public double DocumentHeight { get; }
}
