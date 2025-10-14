namespace ToDoList.Test;

using Xunit;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class GetTests
{
    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
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
            IsCompleted = false
        };

        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem1);
        controller.AddItemToStorage(todoItem2);

        // Act
        // v kontroleru je pro dostání všech položek metoda Read
        var result = controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);

        var firstToDo = value.First();
        Assert.Equal(1, firstToDo.Id);
        Assert.Equal("Jmeno1", firstToDo.Name);
        Assert.Equal("Popis1", firstToDo.Description);
        Assert.Equal(false, firstToDo.IsCompleted);
    }
}
