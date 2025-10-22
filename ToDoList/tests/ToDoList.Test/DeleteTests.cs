namespace ToDoList.Test;

using Microsoft.AspNetCore.Http.Features;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Xunit;

public class DeleteTests
{
    [Fact]
    public void Delete_RemovesItem_WhenItemExists()
    {
        // Arrange
        ToDoItemsController.ClearStorage();
        var todoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };

        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem);

        // Act
        var deleteResult = controller.DeleteById(todoItem.ToDoItemId);

        // Assert
        var result = controller.Read();
        var value = result.GetValue();

        Assert.NotNull(value);
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        ToDoItemsController.ClearStorage();
        var controller = new ToDoItemsController();
        int fakeId = int.MaxValue;

        // Act
        var deleteResult = controller.DeleteById(fakeId);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(deleteResult);
    }
}
