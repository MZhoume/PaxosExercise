using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Challenge1.Core.ActionResults
{
    public class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(object error)
            : base(error)
        {
            this.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
