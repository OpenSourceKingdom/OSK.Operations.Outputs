using OSK.Operations.Outputs.Models;
using Xunit;

namespace OSK.Operations.Outputs.UnitTests.Models;

public class PaginatedOutputTests
{
    #region Validation

    [Fact]
    public void Constructor_NegativeSkip_Test_ThrowsArgumentException()
    {
        // Arrange
        var items = new List<int> { 1 };

        // Act / Assert
        Assert.Throws<ArgumentException>(() => new PaginatedOutput<int>(new OutputCode(OutputStatus.Success), items, -1, 10));
    }

    [Fact]
    public void Constructor_NegativeTake_Test_ThrowsArgumentException()
    {
        // Arrange
        var items = new List<int> { 1 };

        // Act / Assert
        Assert.Throws<ArgumentException>(() => new PaginatedOutput<int>(new OutputCode(OutputStatus.Success), items, 0, -5));
    }

    [Fact]
    public void Constructor_SuccessWithNullData_Test_ThrowsArgumentException()
    {
        // Act / Assert
        Assert.Throws<ArgumentException>(() => new PaginatedOutput<int>(new OutputCode(OutputStatus.Success), null, 0, 10));
    }

    #endregion

    #region AsPage

    [Fact]
    public void AsPage_OnError_ReturnsDifferentGenericPage_ReturnsExpectedOutput()
    {
        // Arrange
        var err = new ErrorInformation(new[] { "err" });
        var page = new PaginatedOutput<int>(new OutputCode(OutputStatus.InvalidRequest), err);

        // Act
        var cast = page.AsPage<string>();

        // Assert
        Assert.False(cast.IsSuccessful);
        Assert.Equal(page.StatusCode.Status, cast.StatusCode.Status);
    }

    #endregion
}
