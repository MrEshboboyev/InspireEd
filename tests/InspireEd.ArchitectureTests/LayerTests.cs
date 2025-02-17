namespace InspireEd.ArchitectureTests;

public class LayerTests : BaseTest
{
    [Fact]
    public void Domain_Should_NotHaveDependencyOnApplications()
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn("InspireEd.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
