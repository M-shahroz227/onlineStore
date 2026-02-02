using Microsoft.AspNetCore.Mvc.Filters;

public class ProductResultFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        Console.WriteLine("Result Filter: Before sending response");
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        Console.WriteLine("Result Filter: After response sent");
    }
}
