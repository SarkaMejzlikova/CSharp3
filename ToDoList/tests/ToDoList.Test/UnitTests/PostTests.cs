namespace ToDoList.Test.UnitTests;

using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi.Controllers;
using Xunit;
using ToDoList.Persistence.Repositories;
using ToDoList.Domain.Models;
using Microsoft.AspNetCore.Http;

[Collection("Sequential")]
public class PostTests
{
    [Fact]
    public void Post_CreateValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var request = new ToDoItemCreateRequestDto(Name: "Jmeno", Description: "Popis", IsCompleted: false);

        // Act
        var result = controller.Create(request);
        var resultResult = result.Result;
        var value = result.GetValue();

        // Assert
        Assert.IsType<CreatedAtActionResult>(resultResult);
        Assert.NotNull(value);

        Assert.Equal(request.Description, value.Description);
        Assert.Equal(request.IsCompleted, value.IsCompleted);
        Assert.Equal(request.Name, value.Name);
    }

    [Fact]
    public void Post_CreateUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.When(x => x.Create(Arg.Any<ToDoItem>())).Do(x => throw new Exception());

        var request = new ToDoItemCreateRequestDto(Name: "Jmeno", Description: "Popis", IsCompleted: false);

        // Act
        var result = controller.Create(request);
        var resultResult = result.Result;

        // Assert
        Assert.IsType<ObjectResult>(resultResult);
        repositoryMock.Received(1).Create(Arg.Any<ToDoItem>());
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)resultResult).StatusCode);
    }

}
