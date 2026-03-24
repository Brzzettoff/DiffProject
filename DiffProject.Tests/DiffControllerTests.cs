using DiffProject.Api.Controllers;
using DiffProject.Api.Dao;
using DiffProject.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DiffProject.Tests;

public class DiffControllerTests
{
    [Fact]
    public void Should_ReturnNotFound404_When_WhenServiceReturnsNull()
    {
        // Arrange
        var mockRepo = new Mock<IDiffRepository>();
        // Force the service to return null for this ID
        mockRepo.Setup(r => r.GetDiffEntity("999")).Returns(new DiffEntity());

        var service = new DiffService(mockRepo.Object);
        var controller = new DiffController(service);

        // Act
        var result = controller.GetDiff("999");

        // Assert
        //result should be 404
        Assert.IsType<NotFoundResult>(result);
    }
}