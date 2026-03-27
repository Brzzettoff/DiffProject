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
    public void Should_GetComparisonReturnNull_WhenNoData()
    {
        // Arrange
        var id = "1";
        _repoMock.Setup(x => x.GetDiffEntity(id)).Returns(new DiffEntity());
      
        // Act
        var result = _service.GetDiff(id);

        // Assert
        Assert.Null(result?.Diffs);
    }

    [Fact]
    public void Should_ReturnEquals_When_StringsAreEqual()
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
    public void Should_ReturnSizeDoNotMatch_When_InputSizesDiffer()
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
    public void Should_WithOffsets_ReturnsCorrectDifferences()
    {
        // Arrange
        var id = "3";

        var entity = new DiffEntity { Left = "AAAAAA", Right = "AQABAQ" };
        _repoMock.Setup(x => x.GetDiffEntity(id)).Returns(entity);

        // Act
        var result = _service.GetDiff(id);

        // Assert
        Assert.Equal("ContentDoNotMatch", result?.DiffResultType);
        Assert.NotNull(result.Diffs);
        Assert.Equal(3, result.Diffs.Count);

        // 1st diff
        Assert.Equal(1, result.Diffs[0].Offset);
        Assert.Equal(1, result.Diffs[0].Length);

        // 2nd diff
        Assert.Equal(3, result.Diffs[1].Offset);
        Assert.Equal(1, result.Diffs[1].Length);

        // 3rd diff
        Assert.Equal(5, result.Diffs[2].Offset);
        Assert.Equal(1, result.Diffs[2].Length);
    }
}