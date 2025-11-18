using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Frontend.Models;

public class ToDoItemView
{
    public int Id { get; set; }
    [Length(1,50)]
    public string Name { get; set; }
    [StringLength(250)]
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}
