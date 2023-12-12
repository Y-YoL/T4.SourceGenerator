namespace T4.SourceGenerator.Package.Test
{
    public partial class UnitTest
    {
        partial void Test(ref string value);

        [Fact]
        public void Test1()
        {
            string value = "no generation";
            this.Test(ref value);
            Assert.NotEqual("no generation", value);
        }
    }
}
