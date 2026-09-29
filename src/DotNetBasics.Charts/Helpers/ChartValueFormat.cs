namespace DotNetBasics.Charts.Helpers;

internal static class ChartValueFormat
{
    public static string Format(double value, Func<double, string> valueFormatter)
    {
        return valueFormatter is null ? value.ToString("0.##", CultureInfo.InvariantCulture) : valueFormatter(value);
    }

    public static string FormatPercentage(double share)
    {
        return $"{(share * 100).ToString("F2", CultureInfo.InvariantCulture)}%";
    }
}
