namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //localhost:500/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{

    // in-memory storage for the demo
    private static readonly List<ToDoItem> items = [];

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) // použijeme DTO - Data Transfer Object
    {
        // map to Domain object as soon as possible
        var item = request.ToDomain();

        try
        {
            item.ToDoItemId = items.Count == 0 ? 1 : items.Max(o => o.ToDoItemId) + 1;
            items.Add(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); // 500
        }

        // 201
        return CreatedAtAction(
            nameof(ReadById),
            new { toDoItemId = item.ToDoItemId },
            ToDoItemGetResponseDto.FromDomain(item)
        );
    }

    [HttpGet]
    public IActionResult Read()
    {
        try
        {
            if (items is null)
            {
                return NotFound(); // 404 when the list itself is null
            }

            // map domain objects to DTOs
            var result = items.Select(ToDoItemGetResponseDto.FromDomain).ToList();

            return Ok(result); // 200
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    // prostá metoda HttpGet se nemůže použít dvakrát. Musí se do dalšího dát "parametr"
    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId)
    {
        try
        {
            // find the item by id
            var item = items.Find(i => i.ToDoItemId == toDoItemId);

            if (item is null)
            {
                return NotFound(); // 404 when item doesn't exist
            }

            var dto = ToDoItemGetResponseDto.FromDomain(item);
            return Ok(dto); // 200 with the DTO
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); // 500
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        try
        {
            // create updated domain instance from DTO
            var updatedItem = request.ToDomain();

            // find index of existing item
            var index = items.FindIndex(i => i.ToDoItemId == toDoItemId);

            if (index == -1)
            {
                return NotFound(); // 404 if item not found
            }

            // preserve the id
            updatedItem.ToDoItemId = toDoItemId;

            // replace at index
            items[index] = updatedItem;

            return NoContent(); // 204
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        try
        {
            var item = items.Find(i => i.ToDoItemId == toDoItemId);

            if (item is null)
            {
                return NotFound(); // 404 if not found
            }

            items.Remove(item);

            return NoContent(); // 204
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    public void AddItemToStorage(ToDoItem item)
    {
        items.Add(item);
    }
}
