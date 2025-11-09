namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.WebApi;
using Xunit;

[Collection("Sequential")]
public class PostTests
{
    [Fact]
    public void Post_ValidRequest_ReturnsNewItem()
    {
        // Arrange
        var context = new ToDoItemsContext(connectionString: "Data Source=../../../IntegrationTests/data/localdb_test.db");
        var controller = new ToDoItemsController(context);
        controller.ClearStorage();

        var request = new ToDoItemCreateRequestDto(
            Name: "Jmeno",
            Description: "Popis",
            IsCompleted: false
        );

        // Act
        var result = controller.Create(request);
        var resultResult = result.Result;
        var value = result.GetValue();

        // Assert
        Assert.IsType<CreatedAtActionResult>(resultResult);
        Assert.NotNull(value);

        Assert.Equal(request.Description, value.Description);
        Assert.Equal(request.IsCompleted, value.IsCompleted);
        Assert.Equal(request.Name, value.Name);

        // Clean up
        controller.ClearStorage();
    }
}
