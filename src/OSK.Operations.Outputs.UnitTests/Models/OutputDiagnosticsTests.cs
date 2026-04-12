using OSK.Operations.Outputs.Models;
using Xunit;

namespace OSK.Operations.Outputs.UnitTests.Models;

public class OutputDiagnosticsTests
{
    #region Validation

    [Fact]
    public void Constructor_EndBeforeStart_Test_ThrowsArgumentException_ReturnsExpectation()
    {
        // Arrange
        var start = DateTime.UtcNow;
        var end = start.AddSeconds(-1);

        // Act / Assert
        Assert.Throws<ArgumentException>(() => new OutputDiagnostics(start, end));
    }

    [Fact]
    public void Constructor_ValidRange_SetsRuntime_ReturnsExpectation()
    {
        // Arrange
        var start = DateTime.UtcNow;
        var end = start.AddSeconds(2);

        // Act
        var diag = new OutputDiagnostics(start, end);

        // Assert
        Assert.Equal(start, diag.StartTime);
        Assert.Equal(end, diag.CompletionTime);
        Assert.Equal(end - start, diag.Runtime);
    }

    #endregion
}
