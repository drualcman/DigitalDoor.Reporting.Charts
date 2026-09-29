namespace DotNetBasics.Charts.Svg;

internal sealed class SvgCanvas
{
    private const double MinimumDocumentSize = 1;

    private readonly StringBuilder Definitions = new StringBuilder();
    private readonly StringBuilder Elements = new StringBuilder();

    public void Rect(double x, double y, double width, double height, string fill, double cornerRadius = 0, SvgStroke stroke = null)
    {
        if (width > 0 && height > 0)
        {
            string cornerAttribute = cornerRadius > 0 ? $" rx=\"{SvgNumber.Format(cornerRadius)}\"" : string.Empty;
            Elements.AppendLine($"<rect x=\"{SvgNumber.Format(x)}\" y=\"{SvgNumber.Format(y)}\" width=\"{SvgNumber.Format(width)}\" " +
                $"height=\"{SvgNumber.Format(height)}\"{cornerAttribute} fill=\"{SvgEscaper.Escape(fill)}\"{stroke?.ToAttributes()} />");
        }
    }

    public void Circle(double centerX, double centerY, double radius, string fill, SvgStroke stroke = null)
    {
        if (radius > 0)
        {
            Elements.AppendLine($"<circle cx=\"{SvgNumber.Format(centerX)}\" cy=\"{SvgNumber.Format(centerY)}\" " +
                $"r=\"{SvgNumber.Format(radius)}\" fill=\"{SvgEscaper.Escape(fill)}\"{stroke?.ToAttributes()} />");
        }
    }

    public void Line(SvgPoint from, SvgPoint to, SvgStroke stroke)
    {
        Elements.AppendLine($"<line x1=\"{SvgNumber.Format(from.X)}\" y1=\"{SvgNumber.Format(from.Y)}\" " +
            $"x2=\"{SvgNumber.Format(to.X)}\" y2=\"{SvgNumber.Format(to.Y)}\"{stroke.ToAttributes()} />");
    }

    public void Polyline(IReadOnlyList<SvgPoint> points, SvgStroke stroke, string fill)
    {
        if (points.Count > 1)
        {
            string pointList = string.Join(" ", points.Select(point => $"{SvgNumber.Format(point.X)},{SvgNumber.Format(point.Y)}"));
            Elements.AppendLine($"<polyline points=\"{pointList}\" fill=\"{SvgEscaper.Escape(fill)}\"{stroke.ToAttributes()} " +
                "stroke-linejoin=\"round\" stroke-linecap=\"round\" />");
        }
    }

    public void Path(string pathData, string fill, SvgStroke stroke = null, SvgPoint? translation = null)
    {
        string transformAttribute = translation.HasValue ?
            $" transform=\"translate({SvgNumber.Format(translation.Value.X)} {SvgNumber.Format(translation.Value.Y)})\"" : string.Empty;
        Elements.AppendLine($"<path d=\"{pathData}\" fill=\"{SvgEscaper.Escape(fill)}\"{stroke?.ToAttributes()}{transformAttribute} />");
    }

    public void Text(string content, double x, double y, SvgTextStyle style)
    {
        if (!string.IsNullOrEmpty(content))
        {
            Elements.AppendLine(SvgTextElement.Create(content, x, y, style));
        }
    }

    public void VerticalLinearGradient(string id, string topColour, string bottomColour, double top, double bottom)
    {
        Definitions.Append($"<linearGradient id=\"{id}\" gradientUnits=\"userSpaceOnUse\" x1=\"0\" y1=\"{SvgNumber.Format(top)}\" " +
            $"x2=\"0\" y2=\"{SvgNumber.Format(bottom)}\"><stop offset=\"0\" stop-color=\"{SvgEscaper.Escape(topColour)}\" />" +
            $"<stop offset=\"1\" stop-color=\"{SvgEscaper.Escape(bottomColour)}\" /></linearGradient>");
    }

    public string ToDocument(double width, double height, string fontFamily)
    {
        return ToDocument(width, height, width, height, fontFamily);
    }

    public string ToDocument(double width, double height, double viewBoxWidth, double viewBoxHeight, string fontFamily)
    {
        string documentWidth = SvgNumber.Format(Math.Max(MinimumDocumentSize, width));
        string documentHeight = SvgNumber.Format(Math.Max(MinimumDocumentSize, height));
        string viewBox = $"0 0 {SvgNumber.Format(Math.Max(MinimumDocumentSize, viewBoxWidth))} {SvgNumber.Format(Math.Max(MinimumDocumentSize, viewBoxHeight))}";
        string definitions = Definitions.Length > 0 ? $"<defs>{Definitions}</defs>\n" : string.Empty;
        return $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{documentWidth}\" height=\"{documentHeight}\" " +
            $"viewBox=\"{viewBox}\" font-family=\"{SvgEscaper.Escape(fontFamily)}\">\n{definitions}{Elements}</svg>";
    }
}
