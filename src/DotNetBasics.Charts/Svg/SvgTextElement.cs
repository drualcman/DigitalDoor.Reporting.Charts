namespace DotNetBasics.Charts.Svg;

internal static class SvgTextElement
{
    public static string Create(string content, double x, double y, SvgTextStyle style)
    {
        StringBuilder element = new StringBuilder();
        element.Append($"<text x=\"{SvgNumber.Format(x)}\" y=\"{SvgNumber.Format(y)}\"");
        element.Append($" text-anchor=\"{style.Anchor}\" font-size=\"{SvgNumber.Format(style.FontSize)}\"");
        if (style.IsBold)
        {
            element.Append(" font-weight=\"bold\"");
        }
        if (style.IsItalic)
        {
            element.Append(" font-style=\"italic\"");
        }
        if (!string.IsNullOrWhiteSpace(style.Fill))
        {
            element.Append($" fill=\"{SvgEscaper.Escape(style.Fill)}\"");
        }
        if (style.RotationAngle.HasValue)
        {
            element.Append($" transform=\"rotate({SvgNumber.Format(style.RotationAngle.Value)} {SvgNumber.Format(x)} {SvgNumber.Format(y)})\"");
        }
        element.Append($">{SvgEscaper.Escape(content)}</text>");
        return element.ToString();
    }
}
