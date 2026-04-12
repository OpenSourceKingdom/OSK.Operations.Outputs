using OSK.Operations.Outputs.Models;
using Xunit;

namespace OSK.Operations.Outputs.UnitTests;

public class OutTests
{
    #region Diagnostics

    [Fact]
    public void Create_WithoutDiagnosticsScope_DiagnosticsAreNull()
    {
        // Arrange / Act
        var o = Out.Success();
        var g = Out.Success("x");

        // Assert
        Assert.Null(o.Diagnostics);
        Assert.Null(g.Diagnostics);
    }

    [Fact]
    public async Task Create_WithDiagnosticsScope_SetsTimesAndRuntime()
    {
        // Arrange / Act
        using (Out.CreateDiagnosticScope())
        {
            await Task.Delay(30);
            var o = Out.Success();

            // Assert
            Assert.NotNull(o.Diagnostics);
            Assert.True(o.Diagnostics.CompletionTime >= o.Diagnostics.StartTime);
            Assert.True(o.Diagnostics.Runtime.TotalMilliseconds >= 10, "Expected runtime to be >= 10ms.");
        }
    }

    [Fact]
    public async Task Create_WithDiagnosticsScope_TimersRestartBetweenOutputs_RecordSeparateIntervals()
    {
        // Arrange / Act
        using (Out.CreateDiagnosticScope())
        {
            await Task.Delay(30);
            var o1 = Out.Success();
            var r1 = o1.Diagnostics.Runtime.TotalMilliseconds;

            await Task.Delay(30);
            var o2 = Out.Success();
            var r2 = o2.Diagnostics.Runtime.TotalMilliseconds;

            // Assert: both intervals should be non-zero and reasonably similar
            Assert.True(r1 >= 10, $"First runtime too small: {r1}ms");
            Assert.True(r2 >= 10, $"Second runtime too small: {r2}ms");
            Assert.True(Math.Abs(r1 - r2) < 100, $"Runtimes diverged too much: r1={r1}ms r2={r2}ms");
        }
    }

    [Fact]
    public async Task Create_WithDiagnosticsScope_ScopeClearedWhenDisposed_NoDiagnosticsAfterDispose()
    {
        // Arrange
        Output oInScope;
        using (Out.CreateDiagnosticScope())
        {
            await Task.Delay(15);
            oInScope = Out.Success();
            Assert.NotNull(oInScope.Diagnostics);
        }

        // Act
        var oAfter = Out.Success();

        // Assert
        Assert.Null(oAfter.Diagnostics);
    }

    [Fact]
    public async Task Create_WithDiagnosticsScope_NestedScopes_DoNotInterfereWithDiagnostics()
    {
        // Arrange / Act
        using (Out.CreateDiagnosticScope())
        {
            await Task.Delay(10);

            using (Out.CreateDiagnosticScope())
            {
                await Task.Delay(25);
                var inner = Out.Success();
                Assert.NotNull(inner.Diagnostics);
                Assert.True(inner.Diagnostics.Runtime.TotalMilliseconds >= 15, "Inner runtime expected to be >= 15ms");
            }

            await Task.Delay(10);
            var outer = Out.Success();
            Assert.NotNull(outer.Diagnostics);
            Assert.True(outer.Diagnostics.Runtime.TotalMilliseconds >= 5, "Outer runtime expected to be >= 5ms");
        }
    }

    #endregion

    #region Success

    [Fact]
    public void OutSuccess_CreatesSuccessfulOutput_ReturnsExpectedOutput()
    {
        // Arrange & Act
        var o = Out.Success();

        // Assert
        Assert.True(o.IsSuccessful);
        Assert.Equal(OutputStatus.Success, o.StatusCode.Status);
    }

    #endregion

    #region Error

    [Fact]
    public void OutError_CreatesErrorOutput_CustomMessage_ReturnsErrorWithCustomMessage()
    {
        // Arrange & Act
        var o = Out.DataNotFound("custom");

        // Assert
        Assert.False(o.IsSuccessful);
        Assert.Contains("custom", o.GetErrorString());
    }

    #endregion
}
