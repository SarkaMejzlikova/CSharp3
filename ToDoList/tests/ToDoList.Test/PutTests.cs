namespace ToDoList.Test;

using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using Xunit;

public class PutTests
{
    [Fact]
    public void Update_Existing_Item_Returns_NoContent_And_Updates()
    {
        // Arrange
        ToDoItemsController.items.Clear();
        var controller = new ToDoItemsController();

        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };
        controller.AddItemToStorage(todoItem1);

        // Act & Assert
        var update = new ToDoItemUpdateRequestDto("A","B",true);
        var result = controller.UpdateById(1, update);
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);

        var read = controller.ReadById(1);
        var ok = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(read.Result);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(ok.Value);
        Assert.Equal("A", dto.Name);
    }

    [Fact]
    public void Update_NonExisting_Returns_NotFound()
    {
        ToDoItemsController.items.Clear();
        var controller = new ToDoItemsController();


        var update = new ToDoItemUpdateRequestDto("X","Y",false);
        var result = controller.UpdateById(999, update);
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
    }
}
