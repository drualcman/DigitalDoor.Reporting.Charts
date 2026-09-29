namespace DotNetBasics.Charts.Svg;

internal static class SvgNumber
{
    public static string Format(double value)
    {
        double writableValue = double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;
        return writableValue.ToString("0.####", CultureInfo.InvariantCulture);
    }
}
