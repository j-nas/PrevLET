using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Figgle;

namespace PrevLIT;

public class FontService
{
    public List<string> FontList { get; set; } = [ ];
    public string _selectedFont = "Grafitti.flf";

    public FontService()
    {
        var assembly = Assembly.GetExecutingAssembly();
        foreach (var resource in assembly.GetManifestResourceNames())
        {
            if (resource.EndsWith(".flf"))
            {
                var fontName = resource.Replace("PrevLIT.Assets.Fonts.", "");
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
        Console.WriteLine($"Loading font: {resourceName}");

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = stream == null ? null : new StreamReader(stream);
        if (reader == null)
        {
            return null;
        }
        return reader.ReadToEnd();
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
