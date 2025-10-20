namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Xunit;

public class PutTests
{
    [Fact]
    public void Update_ReturnsNoContent_WhenItemExists()
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

        var updateRequest = new ToDoItemUpdateRequestDto("updated", "updated desc", true);

        // Act
        var updateResult = controller.UpdateById(todoItem.ToDoItemId, updateRequest);
        var read = controller.ReadById(todoItem.ToDoItemId);
        var readValue = read.GetValue();

        // Assert
        Assert.Equal(updateRequest.Name, readValue!.Name);
        Assert.Equal(updateRequest.Description, readValue.Description);
        Assert.Equal(updateRequest.IsCompleted, readValue.IsCompleted);
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        ToDoItemsController.ClearStorage();
        var controller = new ToDoItemsController();
        int fakeId = int.MaxValue;
        var updateRequest = new ToDoItemUpdateRequestDto("x", "x", false);

        // Act
        var updateResult = controller.UpdateById(fakeId, updateRequest);

        // Assert
        Assert.IsType<NotFoundResult>(updateResult);

    }
}
