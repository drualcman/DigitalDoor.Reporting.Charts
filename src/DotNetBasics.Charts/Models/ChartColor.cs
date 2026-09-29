namespace DotNetBasics.Charts.Models;

public class ChartColor
{
    public ChartColor(string background)
    {
        Background = background;
        Foreground = ColourContrast.GetContrastingColour(background);
    }

    public ChartColor(string background, string foreground)
    {
        Background = background;
        Foreground = foreground;
    }

    public string Background { get; set; }
    public string Foreground { get; set; }
}
