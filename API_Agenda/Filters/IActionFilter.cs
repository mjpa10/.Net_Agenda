using Microsoft.AspNetCore.Mvc.Filters;

namespace API_Agenda.Logging;

public interface IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context);
    public void OnActionExecuted(ActionExecutedContext context);
}
