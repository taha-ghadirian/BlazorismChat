using Xunit;
using BlazorismChat.ClientLibraries.Convertors;
using FluentAssertions;

namespace BlazorismChat.ClientLibraries.ConvertorsTests;

public class TextFixerTests
{
    [Fact]
    public void FixUserName_Should_ReturnCorrectText()
    {
        // Arrange
        var textFixer = TextFixer.FixUserName("test");

        // assert
        textFixer.Should().Be("TEST");
    }

    [Fact]
    public void FixUserName_Should_ReturnCorrectText_When_TextIsNull()
    {
        // Arrange
        var textFixer = TextFixer.FixUserName(null);

        // assert
        textFixer.Should().Be("");
    }

    [Fact]
    public void FixUserName_Should_ReturnCorrectText_When_TextIsEmpty()
    {
        // Arrange
        var textFixer = TextFixer.FixUserName("");

        // assert
        textFixer.Should().Be("");
    }

    [Fact]
    public void FixUserName_Should_ReturnCorrectText_When_TextIsWhiteSpace()
    {
        // Arrange
        var textFixer = TextFixer.FixUserName(" ");

        // assert
        textFixer.Should().Be("");
    }

    [Fact]
    public void FixEmail_Should_ReturnCorrectText()
    {
        // Arrange
        var textFixer = TextFixer.FixEmail("TesT.Mail@outlook.com");

        // assert
        textFixer.Should().Be("TEST.MAIL@OUTLOOK.COM");
    }

    [Fact]
    public void FixEmail_Should_ReturnCorrectText_When_TextIsNull()
    {
        // Arrange
        var textFixer = TextFixer.FixEmail(null);

        // assert
        textFixer.Should().Be("");
    }

    [Fact]
    public void FixEmail_Should_ReturnCorrectText_When_TextIsEmpty()
    {
        // Arrange
        var textFixer = TextFixer.FixEmail("");

        // assert
        textFixer.Should().Be("");
    }

    [Fact]
    public void FixEmail_Should_ReturnCorrectText_When_TextIsWhiteSpace()
    {
        // Arrange
        var textFixer = TextFixer.FixEmail(" ");

        // assert
        textFixer.Should().Be("");
    }
}
