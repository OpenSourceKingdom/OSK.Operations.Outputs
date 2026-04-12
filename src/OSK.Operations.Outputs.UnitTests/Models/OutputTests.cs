using OSK.Operations.Outputs.Models;
using Xunit;

namespace OSK.Operations.Outputs.UnitTests.Models;

public class OutputTests
{
    #region Validation

    [Fact]
    public void Constructor_SuccessWithNullData_ThrowsArgumentException()
    {
        // Arrange
        var status = new OutputCode(OutputStatus.Success);

        // Act / Assert
        Assert.Throws<ArgumentException>(() => new Output<string>(status, null!));
    }

    [Fact]
    public void Constructor_ErrorWithData_AllowsDataWhenError()
    {
        // Arrange
        var status = new OutputCode(OutputStatus.InvalidRequest);
        var err = new ErrorInformation(new[] { "bad" });

        // Act
        var output = new Output<string>(status, "value", err);

        // Assert
        Assert.False(output.IsSuccessful);
        Assert.Equal("value", output.Data);
    }

    #endregion

    #region As

    [Fact]
    public void As_CalledOnSuccessfulTypedOutput_ThrowsInvalidOperationException()
    {
        // Arrange
        var output = Out.Success("value");

        // Act / Assert
        Assert.Throws<InvalidOperationException>(output.As<int>);
    }

    #endregion

    #region Validation

    [Fact]
    public void Constructor_SuccessWithError_ThrowsArgumentException()
    {
        // Arrange
        var status = new OutputCode(OutputStatus.Success);
        var error = new ErrorInformation(new[] { "err" });

        // Act / Assert
        Assert.Throws<ArgumentException>(() => new Output(status, error));
    }

    [Fact]
    public void Constructor_ErrorWithoutErrorInformation_ThrowsArgumentException()
    {
        // Arrange
        var status = new OutputCode(OutputStatus.InvalidRequest);

        // Act / Assert
        Assert.Throws<ArgumentException>(() => new Output(status, null));
    }

    #endregion

    #region Helpers

    [Fact]
    public void WithOrigination_SetsOrigination_ReturnsOutputWithOrigination()
    {
        // Arrange
        var originator = new OriginationSource("Client", 1);
        var output = Out.Success();

        // Act
        output.WithOrigination(originator);

        // Assert
        Assert.Equal(originator, output.OriginationSource);
    }

    #endregion
}
