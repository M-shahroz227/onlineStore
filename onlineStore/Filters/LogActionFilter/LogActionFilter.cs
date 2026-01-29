using Microsoft.AspNetCore.Mvc.Filters;

namespace onlineStore.Filters.LogActionFilter
{
    public class LogActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Start the action");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("end the action");
        }
    }
}
