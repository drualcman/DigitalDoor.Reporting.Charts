using System.Net;
using System.Text;
using DotNetBasics.Charts.Samples;

string outputDirectory = args.Length > 0 ? args[0] : Path.Combine(AppContext.BaseDirectory, "output");
Directory.CreateDirectory(outputDirectory);
Dictionary<string, string> charts = SampleCharts.CreateAll();
StringBuilder gallery = new StringBuilder("<!doctype html><html><head><meta charset=\"utf-8\"><title>DotNetBasics.Charts</title>" +
    "<style>body{font-family:Arial,sans-serif;margin:24px;background:#fafafa}figure{display:inline-block;margin:12px;padding:12px;" +
    "background:#fff;border:1px solid #ddd;vertical-align:top}figcaption{font-size:12px;color:#555;margin-top:6px}</style></head><body>");
foreach (KeyValuePair<string, string> chart in charts)
{
    File.WriteAllText(Path.Combine(outputDirectory, $"{chart.Key}.svg"), chart.Value);
    SvgPngRenderer.Render(chart.Value, Path.Combine(outputDirectory, $"{chart.Key}.png"), 2);
    gallery.Append($"<figure>{chart.Value}<figcaption>{WebUtility.HtmlEncode(chart.Key)}</figcaption></figure>");
    Console.WriteLine($"{chart.Key}: {chart.Value.Length} characters");
}
gallery.Append("</body></html>");
File.WriteAllText(Path.Combine(outputDirectory, "gallery.html"), gallery.ToString());
Console.WriteLine($"Written to {outputDirectory}");
