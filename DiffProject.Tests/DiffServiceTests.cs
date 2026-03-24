using DiffProject.Api.Dao;
using DiffProject.Api.Services;
using Moq;

namespace DiffProject.Tests;

public class DiffServiceTests
{
    private readonly DiffService _service;
    private readonly Mock<IDiffRepository> _repoMock;

    public DiffServiceTests()
    {
        _repoMock = new Mock<IDiffRepository>();
        _service = new DiffService(_repoMock.Object);
    }

    [Fact]
    public void GetComparison_WhenNoData_ReturnsNull()
    {
        // Arrange
        var id = "1";
        _repoMock.Setup(x => x.GetDiffEntity(id)).Returns(new DiffEntity());
        // var entity = new DiffEntity { Left = "AAAAAA==", Right = "AAAAAA==" };
        // _repoMock.Setup(x => x.GetOrAdd(id)).Returns(entity);

        // Act
        var result = _service.GetDiff(id);

        // Assert
        //  Assert.Equal("Equals", result?.DiffResultType);
        Assert.Null(result?.Diffs);
    }

    [Fact]
    public void GetComparison_WhenStringsAreEqual_ReturnsEquals()
    {
        // Arrange
        var id = "1";
        var entity = new DiffEntity { Left = "AAAAAA==", Right = "AAAAAA==" };
        _repoMock.Setup(x => x.GetDiffEntity(id)).Returns(entity);

        // Act
        var result = _service.GetDiff(id);

        // Assert
        Assert.Equal("Equals", result?.DiffResultType);
        Assert.Null(result?.Diffs);
    }

    [Fact]
    public void GetComparison_WhenSizesDiffer_ReturnsSizeDoNotMatch()
    {
        // Arrange
        var id = "2";
        var entity = new DiffEntity { Left = "AAAAAA==", Right = "AAA=" };
        _repoMock.Setup(x => x.GetDiffEntity(id)).Returns(entity);

        // Act
        var result = _service.GetDiff(id);

        // Assert
        Assert.Equal("SizeDoNotMatch", result?.DiffResultType);
    }

    [Fact]
    public void GetComparison_WithOffsets_ReturnsCorrectDifferences()
    {
        // Arrange
        var id = "3";
        // Let's use simple strings for the logic test
        // Index: 012345
        // Left:  ABCDEF
        // Right: AXCDXF
        var entity = new DiffEntity { Left = "ABCDEF", Right = "AXCDXF" };
        _repoMock.Setup(x => x.GetDiffEntity(id)).Returns(entity);

        // Act
        var result = _service.GetDiff(id);

        // Assert
        Assert.Equal("ContentDoNotMatch", result?.DiffResultType);
        Assert.NotNull(result.Diffs);
        Assert.Equal(2, result.Diffs.Count);

        // First diff: 'B' vs 'X' at index 1
        Assert.Equal(1, result.Diffs[0].Offset);
        Assert.Equal(1, result.Diffs[0].Length);

        // Second diff: 'E' vs 'X' at index 4
        Assert.Equal(4, result.Diffs[1].Offset);
        Assert.Equal(1, result.Diffs[1].Length);
    }
}