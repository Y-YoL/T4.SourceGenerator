using System.CodeDom.Compiler;
using Mono.TextTemplating;

namespace T4.SourceGenerator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

public partial class T4SourceGenerator : IIncrementalGenerator
{
    private static void Generate(SourceProductionContext context, AdditionalText source)
    {
        string filename = string.Empty;

        var generator = new TemplateGenerator();
        var result = generator.ProcessTemplate(
            source.Path,
            source.GetText()?.ToString(),
            ref filename,
            out var content);
        if (result)
        {
            context.AddSource(Path.ChangeExtension(source.Path, ".g" + filename), content);
        }
        else
        {
            foreach (CompilerError error in generator.Errors)
            {
                var descriptor = new DiagnosticDescriptor(
                    error.ErrorNumber,
                    error.ErrorText,
                    error.ErrorText,
                    "",
                    error.IsWarning ? DiagnosticSeverity.Warning : DiagnosticSeverity.Error,
                    true);

                var position = new LinePosition(error.Line - 1, error.Column - 1);

                var diagnostic = Diagnostic.Create(
                    descriptor,
                    Location.Create(
                        source.Path,
                        new(0, source.GetText()?.Length ?? 0),
                        new(position, position)));
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
