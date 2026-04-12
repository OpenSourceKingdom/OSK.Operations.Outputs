using OSK.Operations.Outputs.Models;
using Xunit;

namespace OSK.Operations.Outputs.UnitTests;

public class PageHelperTests
{
    #region ExecuteAsync (T)

    [Fact]
    public async Task ExecuteAsync_PageFuncReturnsError_BailsOnFailureTrue_ReturnsPageError_ReturnsExpectation()
    {
        // Arrange
        Task<PaginatedOutput<int>> PageFunc(long s, long t, CancellationToken ct)
            => Task.FromResult(new PaginatedOutput<int>(new OutputCode(OutputStatus.InvalidRequest), new ErrorInformation(new[] { "no" })));

        Task<Output> ProcessFunc(IEnumerable<int> data, CancellationToken ct)
            => Task.FromResult(Out.Success());

        // Act
        var result = await PageHelper.ExecuteAsync<int>(PageFunc, ProcessFunc, bailOnFailure: true);

        // Assert
        Assert.False(result.IsSuccessful);
    }

    [Fact]
    public async Task ExecuteAsync_ProcessesPagesUntilComplete_ReturnsSuccess_ReturnsExpectation()
    {
        // Arrange
        int calls = 0;
        Task<PaginatedOutput<int>> PageFunc(long s, long t, CancellationToken ct)
        {
            calls++;
            var list = new List<int> { 1, 2 };
            var take = s + t > list.Count ? list.Count : t;
            return Task.FromResult(new PaginatedOutput<int>(new OutputCode(OutputStatus.Success), list, s, take));
        }

        Task<Output> ProcessFunc(IEnumerable<int> data, CancellationToken ct)
            => Task.FromResult(Out.Success());

        // Act
        var result = await PageHelper.ExecuteAsync(PageFunc, ProcessFunc, take: 100, bailOnFailure: true);

        // Assert
        Assert.True(result.IsSuccessful);
    }

    #endregion

    #region ExecuteAsync

    [Fact]
    public async Task ExecuteAsync_ReturnsSuccess_ReturnsExpectedOutput()
    {
        // Arrange
        Task<PaginatedOutput<int>> PageFunc(long s, long t, CancellationToken ct)
        {
            var list = new List<int> { 1, 2 };
            var take = s + t > list.Count ? list.Count : t;
            return Task.FromResult(new PaginatedOutput<int>(new OutputCode(OutputStatus.Success), list, s, take));
        }

        Task<Output> ProcessFunc(IEnumerable<int> data, CancellationToken ct)
        {
            var mapped = data.Select(i => $"v{i}").ToList() as ICollection<string>;
            return Task.FromResult(Out.Success());
        }

        // Act
        var result = await PageHelper.ExecuteAsync(PageFunc, ProcessFunc, take: 100);

        // Assert
        Assert.True(result.IsSuccessful);
    }

    #endregion
}
