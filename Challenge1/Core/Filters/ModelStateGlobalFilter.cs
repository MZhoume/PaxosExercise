using System.Linq;
using Challenge1.Core.ActionResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Challenge1.Core.Filters
{
    /// <summary>
    /// Filter for handling model validation errors.
    /// It will cut the execution of the action short by providing appropriate <see cref="ErrorResult"/>.
    /// </summary>
    public class ModelStateGlobalFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Any())
                    .ToDictionary(
                        e => e.Key,
                        e => string.Join(" | ", e.Value.Errors.Select(m => m.ErrorMessage)));
                context.Result = new BadRequestObjectResult(new ErrorResult("Input model is invalid.", errors));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // This method is left empty intentionally.
        }
    }
}
