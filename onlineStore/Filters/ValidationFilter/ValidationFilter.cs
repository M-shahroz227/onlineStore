using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        if (user != null) 
        {
            Console.WriteLine("this is a request send user information :"+user);
        }

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var user = context.HttpContext.User;
        if (user != null) 
        {
            Console.WriteLine("this is a response receive user information"+user);
        }
    }
}
