using Microsoft.CodeAnalysis;

namespace T4.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public partial class T4SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var source = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".tt", StringComparison.InvariantCultureIgnoreCase))
            .WithComparer(Comparer.Instance);

        context.RegisterSourceOutput(source, static (context, source) =>
        {
            Generate(context, source);
        });
    }

    private class Comparer : IEqualityComparer<AdditionalText>
    {
        public static readonly Comparer Instance = new();

        public bool Equals(AdditionalText x, AdditionalText y)
        {
            return (X: x.GetText(), Y: y.GetText()) switch
            {
                { X: { } text1, Y: { } text2 } => text1.GetChecksum().SequenceEqual(text2.GetChecksum()),
                _ => false,
            };
        }

        public int GetHashCode(AdditionalText obj) => obj.GetHashCode();
    }
}
