namespace DotNetBasics.Charts.Rendering.StackedBars;

internal sealed class StackedBarSegmentBox
{
    public StackedBarSegmentBox(double mainStart, double mainSize, double crossStart, double thickness)
    {
        MainStart = mainStart;
        MainSize = mainSize;
        CrossStart = crossStart;
        Thickness = thickness;
    }

    public double MainStart { get; }
    public double MainSize { get; }
    public double CrossStart { get; }
    public double Thickness { get; }
}
