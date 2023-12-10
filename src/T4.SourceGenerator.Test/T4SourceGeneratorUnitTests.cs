using Xunit;
using VerifyCS = T4.SourceGenerator.Test.Verifiers.CSharpSourceGeneratorVerifier<T4.SourceGenerator.T4SourceGenerator>;

namespace T4.SourceGenerator.Test;

public class T4SourceGeneratorUnitTest
{
    //No diagnostics expected to show up
    [Fact]
    public async Task TestMethod1()
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
}
