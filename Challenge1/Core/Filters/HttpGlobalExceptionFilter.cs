using System;
using Challenge1.Core.ActionResults;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Challenge1.Core.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this._environment = env ?? throw new ArgumentNullException(nameof(env));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnException(ExceptionContext context)
        {
            this._logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

            var result = new ErrorResult("An error occurred, please try again later.");
            if (this._environment.IsDevelopment())
            {
                result.Errors = context.Exception.ToString();
            }

            var response = new InternalServerErrorResult(result);
            context.Result = response;
        }
    }
}
