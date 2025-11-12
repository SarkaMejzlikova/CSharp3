namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;
using Xunit;

[Collection("Sequential")]
public class DeleteTests
{
    [Fact]
    public void Delete_ValidId_ReturnsNoContent()
    {
        // Arrange
        var connectionString = "Data Source=../../../IntegrationTests/data/localdb_test.db";
        var context = new ToDoItemsContext(connectionString);
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        context.ToDoItems.RemoveRange(context.ToDoItems);
        context.SaveChanges();

        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false
        };

        context.ToDoItems.Add(toDoItem);
        context.SaveChanges();

        // Act
        var result = controller.DeleteById(toDoItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Verify item was deleted
        var deletedItem = context.ToDoItems.Find(toDoItem.ToDoItemId);
        Assert.Null(deletedItem);

        context.ToDoItems.RemoveRange(context.ToDoItems);
        context.SaveChanges();
    }

    [Fact]
    public void Delete_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var connectionString = "Data Source=../../../IntegrationTests/data/localdb_test.db";
        using var context = new ToDoItemsContext(connectionString);
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        context.ToDoItems.RemoveRange(context.ToDoItems);
        context.SaveChanges();

        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false
        };

        context.ToDoItems.Add(toDoItem);
        context.SaveChanges();

        // Act
        var invalidId = -1;
        var result = controller.DeleteById(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        context.ToDoItems.RemoveRange(context.ToDoItems);
        context.SaveChanges();
    }
}
