namespace ToDoList.Test.UnitTests;

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;
using Xunit;

public class GetByIdTests
{
    [Fact]
    public void Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var someItem = new ToDoItem { Name = "testName", Description = "testDescription", IsCompleted = false };
        repositoryMock.ReadById(Arg.Any<int>()).Returns(someItem);
        int someId = 1;

        //Act
        var result = controller.ReadById(someId);

        //Assert
        Assert.IsType<ActionResult<ToDoItemGetResponseDto>>(result);
        repositoryMock.Received(1).ReadById(someId);

    }

    [Fact]
    public void Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.ReadById(Arg.Any<int>()).Returns(null as ToDoItem);
        int someId = 1;

        //Act
        var result = controller.ReadById(someId);
        var resultResult = result.Result;

        //Assert
        Assert.IsType<NotFoundResult>(resultResult);
        repositoryMock.Received(1).ReadById(someId);
    }

    [Fact]
    public void Get_ReadByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.ReadById(Arg.Any<int>()).Throws(new Exception());
        int someId = 1;

        //Act
        var result = controller.ReadById(someId);
        var resultResult = result.Result;

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(resultResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        repositoryMock.Received(1).ReadById(someId);
    }

}
