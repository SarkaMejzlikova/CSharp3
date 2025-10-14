using System;

namespace ToDoList.Test;

public class ActionResultExtensions
{
    public static T? GetValue<T>(this Microsoft.AspNetCore.Mvc.IActionResult result) where T : class
    {
        if (result is Microsoft.AspNetCore.Mvc.ObjectResult objectResult)
        {
            return objectResult.Value as T;
        }
        return null;
    }
}
