using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = new ObjectResult(new { message = context.Exception.Message })
        {
            StatusCode = 500
        };
        context.ExceptionHandled = true;
    }
}
