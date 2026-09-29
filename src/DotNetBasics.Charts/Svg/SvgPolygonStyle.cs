namespace DotNetBasics.Charts.Svg;

internal sealed class SvgPolygonStyle
{
    public SvgPolygonStyle(string fill, SvgStroke stroke = null)
    {
        Fill = fill;
        Stroke = stroke;
    }

    public string Fill { get; }
    public SvgStroke Stroke { get; }
    public double? FillOpacity { get; init; }
    public string ClipPathId { get; init; }

    public string ToAttributes()
    {
        string opacityAttribute = FillOpacity.HasValue ?
            $" fill-opacity=\"{SvgNumber.Format(Math.Max(0, Math.Min(1, FillOpacity.Value)))}\"" : string.Empty;
        string clipPathAttribute = string.IsNullOrEmpty(ClipPathId) ? string.Empty : $" clip-path=\"url(#{SvgEscaper.Escape(ClipPathId)})\"";
        string strokeAttributes = Stroke is null ? string.Empty : $"{Stroke.ToAttributes()} stroke-linejoin=\"round\"";
        return $" fill=\"{SvgEscaper.Escape(Fill)}\"{opacityAttribute}{strokeAttributes}{clipPathAttribute}";
    }
}
