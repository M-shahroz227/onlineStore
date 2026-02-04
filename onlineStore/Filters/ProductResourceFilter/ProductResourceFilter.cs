using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

public class ProductResourceFilter : IAsyncResourceFilter
{
    private readonly IMemoryCache _cache;
    public ProductResourceFilter(IMemoryCache cache)
    {
        _cache = cache;
    }
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        // get userId form jwt
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        // get path form request
        var path = context.HttpContext.Request.Path.ToString();
        // cachekey
        var cachekey = $"path_{path}_UserId_{userId}";
        if(_cache.TryGetValue(cachekey,out object cacheResponse))
        {
            Console.WriteLine("data cache sy milya");
            context.Result = new OkObjectResult(cacheResponse);
            return; // action execute naii hogya
        }
        Console.WriteLine("data cache sy naii mailya");
        // action execute hona do 
        var executedcontext = await next();
        if (executedcontext.Result is OkObjectResult OkResult) 
        {
            _cache.Set(
                cachekey,
                OkResult.Value,
                TimeSpan.FromMinutes(1)
                );
            Console.WriteLine("data cache ma store hogya hai");
        }


    }
}
