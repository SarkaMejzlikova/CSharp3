using System;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs;

public record ToDoItemUpdateRequestDto(string Name, string Description, bool IsCompleted)
{
	public ToDoItem ToDomain() => new()
    {
        Name = Name,
        Description = Description,
        IsCompleted = IsCompleted
    };
}
