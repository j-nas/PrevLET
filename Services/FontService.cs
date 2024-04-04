using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Figgle;
using Microsoft.JSInterop;

namespace PrevLET;

public class FontService
{
    public List<string> FontList { get; set; } = [ ];
    private readonly IJSRuntime _jsRuntime;

    public FontService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;

        var assembly = Assembly.GetExecutingAssembly();
        foreach (var resource in assembly.GetManifestResourceNames())
        {
            if (resource.EndsWith(".flf"))
            {
                var fontName = resource.Replace("PrevLET.Assets.Fonts.", "");
                FontList.Add(fontName);
            }
        }
    }

    public static string? GetFont(string fontName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly
            .GetManifestResourceNames()
            .FirstOrDefault(str => str.EndsWith(fontName));

        if (resourceName == null)
        {
            return null;
        }
        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = stream == null ? null : new StreamReader(stream);
        return reader?.ReadToEnd();
    }

    public async Task DownloadFontFile(string fontName)
    {
        var font = GetFont(fontName);
        if (font == null)
        {
            return;
        }

        var bytes = Encoding.UTF8.GetBytes(font);
        var base64 = Convert.ToBase64String(bytes);
        await _jsRuntime.InvokeVoidAsync("downloadFromBase64", base64, fontName);
    }

    public static string? Render(string text, string fontName)
    {
        var font = GetFont(fontName);
        if (font == null)
        {
            return null;
        }

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(font));
        var figgleFont = FiggleFontParser.Parse(stream);
        var rendered = figgleFont.Render(text);

        return $"{rendered}";
    }
}
