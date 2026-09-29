using SkiaSharp;
using Svg.Skia;

namespace DotNetBasics.Charts.Tests.Support;

public static class SvgRasterizer
{
    public static int CountPaintedPixels(string svg)
    {
        using SKSvg svgDocument = new SKSvg();
        SKPicture picture = svgDocument.FromSvg(svg);
        Assert.NotNull(picture);
        int width = Math.Max(1, (int)Math.Ceiling(picture.CullRect.Width));
        int height = Math.Max(1, (int)Math.Ceiling(picture.CullRect.Height));
        using SKBitmap bitmap = new SKBitmap(width, height);
        using (SKCanvas canvas = new SKCanvas(bitmap))
        {
            canvas.Clear(SKColors.Transparent);
            canvas.DrawPicture(picture);
        }
        return bitmap.Pixels.Count(pixel => pixel.Alpha > 0);
    }
}
