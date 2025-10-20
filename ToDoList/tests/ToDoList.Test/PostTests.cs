namespace ToDoList.Test;

using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Xunit;

public class PostTests
{
    [Fact]
    public void Create_Adds_Item_And_Returns_Created()
    {
        // Arrange
        ToDoItemsController.items.Clear();
        var controller = new ToDoItemsController();

        // Act
        var request = new ToDoItemCreateRequestDto("New", "Desc", false);
        var result = controller.Create(request);

        // CreatedAtAction returns ActionResult<ToDoItemGetResponseDto>
        var created = Assert.IsType<Microsoft.AspNetCore.Mvc.CreatedAtActionResult>(result.Result);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(created.Value);
        Assert.Equal("New", dto.Name);
    }
}
