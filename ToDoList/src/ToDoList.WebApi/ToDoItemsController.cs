namespace ToDoList.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    //private static readonly List<ToDoItem> items = []; // již není potřeba
    private readonly ToDoItemsContext context;
    private readonly IRepository<ToDoItem> repository;

    // constructor
    public ToDoItemsController(ToDoItemsContext context, IRepository<ToDoItem> repository)
    {
        this.context = context;
        this.repository = repository;
    }

    [HttpPost]
    public ActionResult<ToDoItemGetResponseDto> Create(ToDoItemCreateRequestDto request)
    {
        //map to Domain object as soon as possible
        var item = request.ToDomain();

        //try to create an item
        try
        {
            repository.Create(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return CreatedAtAction(
            nameof(ReadById),
            new { toDoItemId = item.ToDoItemId },
            ToDoItemGetResponseDto.FromDomain(item)); //201
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        List<ToDoItem> itemsToGet;
        try
        {
            //itemsToGet = items;
            itemsToGet = context.ToDoItems
                .AsNoTracking()
                .ToList();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return (itemsToGet is null)
            ? NotFound() //404
            : Ok(itemsToGet.Select(ToDoItemGetResponseDto.FromDomain)); //200
    }

    [HttpGet("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> ReadById(int toDoItemId)
    {
        //try to retrieve the item by id
        ToDoItem? itemToGet;
        try
        {
            //itemToGet = items.Find(i => i.ToDoItemId == toDoItemId);
            itemToGet = context.ToDoItems
                .AsNoTracking()
                .FirstOrDefault(i => i.ToDoItemId == toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return (itemToGet is null)
            ? NotFound() //404
            : Ok(ToDoItemGetResponseDto.FromDomain(itemToGet)); //200
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        //map to Domain object as soon as possible
        var updatedItem = request.ToDomain();

        //try to update the item by retrieving it with given id
        try
        {
            var existing = context.ToDoItems.FirstOrDefault(i => i.ToDoItemId == toDoItemId);
            if (existing == null)
            {
                return NotFound(); //404
            }

            existing.Name = updatedItem.Name;
            existing.Description = updatedItem.Description;
            existing.IsCompleted = updatedItem.IsCompleted;

            context.SaveChanges();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return NoContent(); //204
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        //try to delete the item
        try
        {
            //var itemToDelete = items.Find(i => i.ToDoItemId == toDoItemId);
            var itemToDelete = context.ToDoItems
                .FirstOrDefault(i => i.ToDoItemId == toDoItemId);

            if (itemToDelete is null)
            {
                return NotFound(); //404
            }
            //items.Remove(itemToDelete);
            context.ToDoItems.Remove(itemToDelete);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client
        return NoContent(); //204
    }

    public void AddItemToStorage(ToDoItem item)
    {
        //items.Add(item);
        context.ToDoItems.Add(item);
        context.SaveChanges();
    }

    public void ClearStorage()
    {
        //items.Clear();
        context.ToDoItems.RemoveRange(context.ToDoItems);
        context.SaveChanges();
    }
}
