using SkiaSharp;
using Svg.Skia;

namespace DotNetBasics.Charts.Tests.Support;

public static class SvgRasterizer
{
    public static int CountPaintedPixels(string svg)
    {
        using SKBitmap bitmap = Rasterize(svg);
        return bitmap.Pixels.Count(pixel => pixel.Alpha > 0);
    }

    public static int CountPixelsWithColour(string svg, string hexColour)
    {
        SKColor colour = SKColor.Parse(hexColour);
        using SKBitmap bitmap = Rasterize(svg);
        return bitmap.Pixels.Count(pixel => pixel.Alpha == 255 && pixel.Red == colour.Red && pixel.Green == colour.Green &&
            pixel.Blue == colour.Blue);
    }

    private static SKBitmap Rasterize(string svg)
    {
        using SKSvg svgDocument = new SKSvg();
        SKPicture picture = svgDocument.FromSvg(svg);
        Assert.NotNull(picture);
        int width = Math.Max(1, (int)Math.Ceiling(picture.CullRect.Width));
        int height = Math.Max(1, (int)Math.Ceiling(picture.CullRect.Height));
        SKBitmap bitmap = new SKBitmap(width, height);
        using (SKCanvas canvas = new SKCanvas(bitmap))
        {
            canvas.Clear(SKColors.Transparent);
            canvas.DrawPicture(picture);
        }
        return bitmap;
    }
}
