using OSK.Operations.Outputs.Models;
using Xunit;

namespace OSK.Operations.Outputs.UnitTests.Models;

public class MultiOutputBaseTests
{
    #region Constructors

    [Fact]
    public void New_MultiOutput_SetsStatusToNotContentAndEmpty()
    {
        // Arrange/Act
        var output = new MultiOutput();

        // Assert
        Assert.Equal(OutputStatus.NoContent, output.StatusCode.Status);
        Assert.Empty(output.Data);
        Assert.Null(output.Diagnostics);
    }

    #endregion

    #region With

    [Fact]
    public void With_SingleOutput_SetsInternalStatus_MultiOutputStatusMatchesOutput()
    {
        // Arrange
        var multi = new MultiOutput();
        var one = Out.Success();

        // Act
        multi.With(one);

        // Assert
        Assert.Equal(one.StatusCode.Status, multi.StatusCode.Status);
        Assert.NotNull(multi.Data);
        Assert.Single(multi.Data);
    }

    [Fact]
    public void With_MultipleOutputsHaveIdenticalStqatus_SetsInternalStatus_MultiOutputStatusMatchesOutput()
    {
        // Arrange
        var multi = new MultiOutput();
        var one = Out.Status(OutputStatus.Accepted);
        var two = Out.Status(OutputStatus.Accepted);
        var three = Out.Status(OutputStatus.Accepted);

        // Act
        multi.With(one, two, three);

        // Assert
        Assert.Equal(one.StatusCode.Status, multi.StatusCode.Status);
        Assert.NotNull(multi.Data);
        Assert.Equal(3, multi.Data.Count);
    }

    [Fact]
    public void With_DifferentOutputStatuses_SetsInternalStatus_MultiOutputStatusSetToMultiStatus()
    {
        // Arrange
        var multi = new MultiOutput();
        var first = Out.Success();
        var second = Out.InvalidRequest("bad");

        // Act
        multi.With(first);
        multi.With(second);

        // Assert
        Assert.Equal(OutputStatus.MultiStatus, multi.StatusCode.Status);
    }


    [Fact]
    public void With_SingleOutputError_SetsInternalErrorInformation_MultiStatusErrorMatchesOutput()
    {
        // Arrange
        var multi = new MultiOutput();
        var one = Out.InvalidRequest("bad");

        // Act
        multi.With(one);

        // Assert
        Assert.Equal(OutputStatus.InvalidRequest, multi.StatusCode.Status);
        Assert.NotNull(multi.ErrorInformation);
        Assert.Equal(one.ErrorInformation, multi.ErrorInformation);
    }

    [Fact]
    public void With_ErrorAggregation_CreatesAggregateErrorAndKeepsSingleAggregateMessage_ReturnsExpectation()
    {
        // Arrange
        var multi = new MultiOutput();
        var first = Out.Success();
        var second = Out.InvalidRequest("bad");

        // Act
        multi.With(first);
        multi.With(second);

        // Assert
        Assert.Equal(OutputStatus.MultiStatus, multi.StatusCode.Status);
        Assert.NotNull(multi.ErrorInformation);
        Assert.Contains("One or more outputs exist with error information", multi.GetErrorString());
    }

    [Fact]
    public void With_Diagnostics_CombinesTimesToMinAndMax_ReturnsExpectation()
    {
        // Arrange
        var multi = new MultiOutput();

        var a = Out.Status(OutputStatus.Success);
        a.Diagnostics = new OutputDiagnostics(DateTime.UtcNow.AddSeconds(-10), DateTime.UtcNow.AddSeconds(-8));

        var b = Out.Status(OutputStatus.Success);
        b.Diagnostics = new OutputDiagnostics(DateTime.UtcNow.AddSeconds(-9), DateTime.UtcNow.AddSeconds(-1));

        // Act
        multi.With(a);
        multi.With(b);

        // Assert
        Assert.NotNull(multi.Diagnostics);
        Assert.True(multi.Diagnostics.StartTime <= a.Diagnostics.StartTime);
        Assert.True(multi.Diagnostics.CompletionTime >= b.Diagnostics.CompletionTime);
    }

    #endregion
}
