using SkiaSharp;
using Svg.Skia;

namespace DotNetBasics.Charts.Samples;

public static class SvgPngRenderer
{
    public static void Render(string svg, string pngPath, float scale)
    {
        using SKSvg svgDocument = new SKSvg();
        SKPicture picture = svgDocument.FromSvg(svg) ??
            throw new InvalidOperationException($"The SVG of {Path.GetFileName(pngPath)} could not be parsed.");
        SKRect bounds = picture.CullRect;
        using SKBitmap bitmap = new SKBitmap((int)Math.Ceiling(bounds.Width * scale), (int)Math.Ceiling(bounds.Height * scale));
        using (SKCanvas canvas = new SKCanvas(bitmap))
        {
            canvas.Clear(SKColors.White);
            canvas.Scale(scale);
            canvas.DrawPicture(picture);
        }
        using FileStream pngStream = File.Create(pngPath);
        bitmap.Encode(pngStream, SKEncodedImageFormat.Png, 100);
    }
}
