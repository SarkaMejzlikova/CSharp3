namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.WebApi;
using Xunit;

[Collection("Sequential")]
public class DeleteTests
{
    [Fact]
    public void Delete_ValidId_ReturnsNoContent()
    {
        // Arrange
        var context = new ToDoItemsContext("Data Source=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context);
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
        var result = controller.DeleteById(toDoItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Clean up
        controller.ClearStorage();
    }

    [Fact]
    public void Delete_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var context = new ToDoItemsContext("Data Source=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context);
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
        var result = controller.DeleteById(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        // Clean up
        controller.ClearStorage();
    }
}
