using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

public class ProductResourceFilter : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        Console.WriteLine("Resource Filter: Pre Action");
        var executedContext = await next();  // action execute hota hai
        Console.WriteLine("Resource Filter: Post Action");
    }
}
