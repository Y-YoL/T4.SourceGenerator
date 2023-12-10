using Microsoft.CodeAnalysis;
using Mono.TextTemplating;

namespace T4.SourceGenerator;

public partial class T4SourceGenerator : IIncrementalGenerator
{
    private static void Generate(SourceProductionContext context, AdditionalText source)
    {
        string filename = string.Empty;

        var generator = new TemplateGenerator();
        generator.ProcessTemplate(
            source.Path,
            source.GetText()?.ToString(),
            ref filename,
            out var content);

        context.AddSource(Path.ChangeExtension(source.Path, ".g" + filename), content);
    }
}
