using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDoList.WebApi;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using Xunit;

namespace ToDoList.WebApi.Tests;

public class ToDoItemsControllerTests
{
    [Fact]
    public void Create_Returns_CreatedAtAction_With_Location_And_Body()
    {
        // arrange
        var controller = new ToDoItemsController();
        var request = new ToDoItemCreateRequestDto("Test", "Desc", false);

        // act
        var result = controller.Create(request);

        // assert
        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(ToDoItemsController.ReadById), created.ActionName);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(created.Value);
        Assert.Equal("Test", dto.Name);
    }

    [Fact]
    public void Read_Returns_List()
    {
        var controller = new ToDoItemsController();
        var result = controller.Read();
        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsType<System.Collections.Generic.List<ToDoItemGetResponseDto>>(ok.Value);
    }

    [Fact]
    public void ReadById_Returns_404_When_Not_Found()
    {
        var controller = new ToDoItemsController();
        var result = controller.ReadById(9999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void UpdateById_Returns_404_When_Not_Found()
    {
        var controller = new ToDoItemsController();
        var request = new ToDoItemUpdateRequestDto("x","y",false);
        var result = controller.UpdateById(9999, request);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteById_Returns_404_When_Not_Found()
    {
        var controller = new ToDoItemsController();
        var result = controller.DeleteById(9999);
        Assert.IsType<NotFoundResult>(result);
    }
}
