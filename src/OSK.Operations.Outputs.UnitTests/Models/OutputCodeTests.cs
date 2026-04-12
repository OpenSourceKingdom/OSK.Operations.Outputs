using OSK.Operations.Outputs.Models;
using Xunit;

namespace OSK.Operations.Outputs.UnitTests.Models;

public class OutputCodeTests
{
    #region IsSuccessful

    [Fact]
    public void IsSuccessful_For200_ReturnsTrue()
    {
        // Arrange
        var code = new OutputCode(OutputStatus.Success);

        // Act
        var success = code.IsSuccessful;

        // Assert
        Assert.True(success);
    }

    [Fact]
    public void IsSuccessful_For400_ReturnsFalse()
    {
        // Arrange
        var code = new OutputCode(OutputStatus.InvalidRequest);

        // Act
        var success = code.IsSuccessful;

        // Assert
        Assert.False(success);
    }

    #endregion

    #region TryParse

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(".")]
    [InlineData("200.")]
    [InlineData(".1")]
    public void TryParse_InvalidOutputCodeString_ReturnsFalse(string? code)
    {
        // Arrange & Act
        var ok = OutputCode.TryParse(code, out var _);

        // Assert
        Assert.False(ok);
    }

    [Fact]
    public void TryParse_StatusOnly_ReturnsTrueAndParsesStatus_ReturnsExpectedOutputCode()
    {
        // Arrange
        var text = "200";

        // Act
        var ok = OutputCode.TryParse(text, out var outputCode);

        // Assert
        Assert.True(ok);
        Assert.Equal(OutputStatus.Success, outputCode.Status);
        Assert.Equal(DetailCode.None, outputCode.DetailCode);
    }


    [Fact]
    public void TryParse_StatusAndDetailCode_ReturnsTrueAndParsesStatus_ReturnsExpectedOutputCode()
    {
        // Arrange
        var text = "200.1";

        // Act
        var ok = OutputCode.TryParse(text, out var outputCode);

        // Assert
        Assert.True(ok);
        Assert.Equal(OutputStatus.Success, outputCode.Status);
        Assert.Equal(1, outputCode.DetailCode);
    }

    #endregion
}
