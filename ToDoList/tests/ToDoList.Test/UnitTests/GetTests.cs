namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;
using Xunit;

public class GetTests
{
    [Fact]
    public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var someItem = new ToDoItem { Name = "testName", Description = "testDescription", IsCompleted = false };
        repositoryMock.ReadAll().Returns([someItem]);

        //Act
        var result = controller.Read();

        //Assert
        Assert.IsType<ActionResult<IEnumerable<ToDoItemGetResponseDto>>>(result);
        repositoryMock.Received(1).ReadAll();

    }
}
