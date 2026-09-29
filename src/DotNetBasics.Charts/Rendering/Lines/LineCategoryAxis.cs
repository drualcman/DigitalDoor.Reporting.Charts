namespace DotNetBasics.Charts.Rendering.Lines;

internal sealed class LineCategoryAxis
{
    public LineCategoryAxis(IReadOnlyList<string> labels, bool isRotated, int labelStep, double bandHeight, double rightOverflow)
    {
        Labels = labels;
        IsRotated = isRotated;
        LabelStep = labelStep;
        BandHeight = bandHeight;
        RightOverflow = rightOverflow;
    }

    public IReadOnlyList<string> Labels { get; }
    public bool IsRotated { get; }
    public int LabelStep { get; }
    public double BandHeight { get; }
    public double RightOverflow { get; }

    public double GetShare(int labelIndex)
    {
        return Labels.Count > 1 ? (double)labelIndex / (Labels.Count - 1) : 0.5;
    }

    public bool IsLabelVisible(int labelIndex)
    {
        return labelIndex % LabelStep == 0;
    }
}
