namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Xunit;

[Collection("Sequential")]
public class DeleteTests
{
    [Fact]
    public void Delete_ValidId_ReturnsNoContent()
    {
        // Arrange
        var controller = new ToDoItemsController();
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
    }

    [Fact]
    public void Delete_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();
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
    }
}
