using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Challenge1.Core.ActionResults
{
    /// <summary>
    /// An error result class with response status code set to 500InternalServerError.
    /// </summary>
    public class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(object error)
            : base(error)
        {
            this.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
