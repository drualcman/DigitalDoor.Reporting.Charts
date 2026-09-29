using System.Security;

namespace DotNetBasics.Charts.Svg;

internal static class SvgEscaper
{
    public static string Escape(string text)
    {
        string textWithoutControlCharacters = new string((text ?? string.Empty)
            .Where(character => !char.IsControl(character) || character == '\t')
            .ToArray());
        return SecurityElement.Escape(textWithoutControlCharacters) ?? string.Empty;
    }
}
