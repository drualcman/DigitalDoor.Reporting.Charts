namespace DotNetBasics.Charts.Helpers;

internal static class ColourParser
{
    private static readonly Dictionary<string, string> NamedColours = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["black"] = "#000000", ["white"] = "#FFFFFF", ["red"] = "#FF0000", ["green"] = "#008000", ["blue"] = "#0000FF",
        ["yellow"] = "#FFFF00", ["orange"] = "#FFA500", ["purple"] = "#800080", ["gray"] = "#808080", ["grey"] = "#808080",
        ["lightgray"] = "#D3D3D3", ["lightgrey"] = "#D3D3D3", ["darkgray"] = "#A9A9A9", ["navy"] = "#000080", ["teal"] = "#008080",
        ["maroon"] = "#800000", ["olive"] = "#808000", ["lime"] = "#00FF00", ["aqua"] = "#00FFFF", ["cyan"] = "#00FFFF",
        ["fuchsia"] = "#FF00FF", ["magenta"] = "#FF00FF", ["silver"] = "#C0C0C0", ["pink"] = "#FFC0CB", ["brown"] = "#A52A2A"
    };

    public static int[] ReadRgbOrNull(string colour)
    {
        string trimmedColour = (colour ?? string.Empty).Trim();
        string hexColour = NamedColours.TryGetValue(trimmedColour, out string namedHex) ? namedHex : trimmedColour;
        int[] result = null;
        if (hexColour.StartsWith("#", StringComparison.Ordinal))
        {
            result = ReadHex(hexColour.Substring(1));
        }
        else if (hexColour.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
        {
            double[] numbers = ReadNumbers(hexColour);
            result = numbers.Length >= 3 ? numbers.Take(3).Select(ToByte).ToArray() : null;
        }
        else if (hexColour.StartsWith("hsl", StringComparison.OrdinalIgnoreCase))
        {
            double[] numbers = ReadNumbers(hexColour);
            result = numbers.Length >= 3 ? ReadHex(HslColourConverter.ToHex(numbers[0], numbers[1] / 100.0, numbers[2] / 100.0).Substring(1)) : null;
        }
        return result;
    }

    private static int[] ReadHex(string digits)
    {
        string expandedDigits = digits.Length == 3 || digits.Length == 4 ?
            string.Concat(digits.Take(3).Select(digit => $"{digit}{digit}")) : digits;
        return expandedDigits.Length >= 6 && expandedDigits.Take(6).All(Uri.IsHexDigit) ?
            new[] { 0, 2, 4 }.Select(position => Convert.ToInt32(expandedDigits.Substring(position, 2), 16)).ToArray() : null;
    }

    private static double[] ReadNumbers(string functionalColour)
    {
        int openingParenthesis = functionalColour.IndexOf('(');
        int closingParenthesis = functionalColour.LastIndexOf(')');
        string arguments = openingParenthesis >= 0 && closingParenthesis > openingParenthesis ?
            functionalColour.Substring(openingParenthesis + 1, closingParenthesis - openingParenthesis - 1) : string.Empty;
        return arguments.Split(new[] { ',', ' ', '/' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(argument => double.TryParse(argument.Replace("%", string.Empty).Replace("deg", string.Empty),
                NumberStyles.Float, CultureInfo.InvariantCulture, out double number) ? number : 0)
            .ToArray();
    }

    private static int ToByte(double value)
    {
        return Math.Max(0, Math.Min(255, (int)Math.Round(value)));
    }
}
