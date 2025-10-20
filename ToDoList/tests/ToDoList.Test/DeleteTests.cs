namespace ToDoList.Test;

using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Xunit;

public class DeleteTests
{
    [Fact]
    public void Delete_Existing_Item_Removes_And_Returns_NoContent()
    {
        // Arrange
        ToDoItemsController.items.Clear();

        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Jmeno2",
            Description = "Popis2",
            IsCompleted = true
        };
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem1);
        controller.AddItemToStorage(todoItem2);

        // Act & Assert
        var result = controller.DeleteById(1);
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);

        var read = controller.ReadById(1);
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(read.Result);
    }

    [Fact]
    public void Delete_NonExisting_Returns_NotFound()
    {
        // Arrange
        ToDoItemsController.items.Clear();
        var controller = new ToDoItemsController();

        // Act
        var result = controller.DeleteById(9999);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
    }
}
