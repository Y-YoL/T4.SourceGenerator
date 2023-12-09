using System.Threading.Tasks;
using Xunit;
using VerifyCS = T4.SourceGenerator.Test.Verifiers.CSharpSourceGeneratorVerifier<T4.SourceGenerator.T4SourceGenerator>;

namespace T4.SourceGenerator.Test;

public class T4SourceGeneratorUnitTest
{
    //No diagnostics expected to show up
    [Fact]
    public async Task TestMethod1()
    {
        await new VerifyCS.Test
        {
        }.RunAsync();
    }
}
