using Microsoft.CodeAnalysis.Testing;
using Xunit;
using VerifyCS = T4.SourceGenerator.Test.Verifiers.CSharpSourceGeneratorVerifier<T4.SourceGenerator.T4SourceGenerator>;

namespace T4.SourceGenerator.Test;

public class T4SourceGeneratorUnitTest
{
    [Fact]
    public async Task GenerateSource()
    {
        string testSource = """
<#@ template debug="false" hostspecific="false" language="C#" #>
<#@ output extension=".cs" #>

namespace Sample
{
    public class ABC
    {
<# for (int i = 0; i < 3; i++) { #>
        public int Value<#= i #>;
<# } #>
    }
}
""";

        string expected = """

namespace Sample
{
    public class ABC
    {
        public int Value0;
        public int Value1;
        public int Value2;
    }
}
""";

        await new VerifyCS.Test
        {
            TestState =
            {
                AdditionalFiles =
                {
                    ("a.tt", testSource),
                },
                GeneratedSources =
                {
                    (typeof(T4.SourceGenerator.T4SourceGenerator), "a.g.cs", expected)
                },
            },
        }.RunAsync();
    }

    [Fact]
    public async Task HasError()
    {
        string testSource = """
<#@ template debug="false" hostspecific="false" language="C#" #>
<#@ output extension=".cs" #>

<# string error = 0; #>
class A {}
""";

        await new VerifyCS.Test
        {
            TestState =
            {
                AdditionalFiles =
                {
                    ("a.tt", testSource),
                },
                ExpectedDiagnostics =
                {
                    DiagnosticResult.CompilerError("CS0029")
                        .WithSpan("a.tt", 4, 17, 4, 17)
                        .WithMessage(null),
                },
            },
        }.RunAsync();
    }
}
