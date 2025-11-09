namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.WebApi;
using Xunit;

[Collection("Sequential")]
public class GetByIdTests
{
    [Fact]
    public void GetById_ValidId_ReturnsItem()
    {
        // Arrange
        var context = new ToDoItemsContext("Data Source=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context, null);
        controller.ClearStorage();
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false
        };
        controller.AddItemToStorage(toDoItem);

        // Act
        var result = controller.ReadById(toDoItem.ToDoItemId);
        var resultResult = result.Result;
        var value = result.GetValue();

        // Assert
        Assert.IsType<OkObjectResult>(resultResult);
        Assert.NotNull(value);

        Assert.Equal(toDoItem.ToDoItemId, value.Id);
        Assert.Equal(toDoItem.Description, value.Description);
        Assert.Equal(toDoItem.IsCompleted, value.IsCompleted);
        Assert.Equal(toDoItem.Name, value.Name);

        // Clean up
        controller.ClearStorage();
    }

    [Fact]
    public void GetById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var context = new ToDoItemsContext("Data Source=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context, null);
        controller.ClearStorage();
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false
        };
        controller.AddItemToStorage(toDoItem);

        // Act
        var invalidId = -1;
        var result = controller.ReadById(invalidId);
        var resultResult = result.Result;

        // Assert
        Assert.IsType<NotFoundResult>(resultResult);

        // Clean up
        controller.ClearStorage();
    }
}
