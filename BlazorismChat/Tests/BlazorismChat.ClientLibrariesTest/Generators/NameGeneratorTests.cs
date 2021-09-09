using Xunit;
using FluentAssertions;
using BlazorismChat.ClientLibraries.Convertors;

namespace BlazorismChat.ClientLibrariesTests;

public class NameGeneratorTests
{
    [Fact]
    public void GeneratedName_Should_notBe_null()
    {
        var name = NameGenerator.GenerateUniqueCode();

        name.Should().NotBeNull();
        name.Should().NotContain("-");
    }
}