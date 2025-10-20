using System;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs;

// record je speciální struktura
// vytvoří se nová instance třídy
// parametry pošle klient
public record ToDoItemCreateRequestDto(string Name, string Description, bool IsCompleted)
{
    public ToDoItem ToDomain() => new()
    {
        Name = Name,
        Description = Description,
        IsCompleted = IsCompleted
    };
}
