namespace ToDoList.Test.UnitTests;

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;
using Xunit;

[Collection("Sequential")]
public class PutTests
{
    [Fact]
    public void Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);


        repositoryMock.ReadById(Arg.Any<int>()).Returns(new ToDoItem { Name = "Jmeno", Description = "Popis", IsCompleted = false });
        var someId = 1;
        var request = new ToDoItemUpdateRequestDto(Name: "Jine jmeno", Description: "Jiny popis", IsCompleted: true);

        //Act
        var result = controller.UpdateById(someId, request);

        //Assert
        Assert.IsType<NoContentResult>(result);
        repositoryMock.Received(1).ReadById(someId);
        repositoryMock.Received(1).Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.ReadById(Arg.Any<int>()).Returns(null as ToDoItem);
        int someId = 1;
        var request = new ToDoItemUpdateRequestDto(Name: "Jine jmeno", Description: "Jiny popis", IsCompleted: true);

        //Act
        var result = controller.UpdateById(someId, request);

        //Assert
        Assert.IsType<NotFoundResult>(result);
        repositoryMock.Received(1).ReadById(someId);
        repositoryMock.Received(0).Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.When(x => x.ReadById(Arg.Any<int>())).Do(x => throw new Exception());
        var someId = 1;
        var request = new ToDoItemUpdateRequestDto(Name: "Jine jmeno", Description: "Jiny popis", IsCompleted: true);

        //Act
        var result = controller.UpdateById(someId, request);

        //Assert
        Assert.IsType<ObjectResult>(result);
        repositoryMock.Received(1).ReadById(someId);
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    }
}
