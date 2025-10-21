namespace ToDoList.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class ToDoItem
{
    [Key] // určíme, že ToDoItemId je primární klíč
    public int ToDoItemId { get; set; }

    [Length(1, 50)] // omezení délky jména na 50 znaků pro tabulku
    public string Name { get; set; }

    [StringLength(250)] // omezení délky popisu na 250 znaků pro tabulku
    public string Description { get; set; }

    public bool IsCompleted { get; set; }
}
