using System;
using Challenge1.Core.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Challenge1.Features
{
    /// <summary>
    /// Controller for route /. It provides ways to redirect user to the SwaggerUI.
    /// </summary>
    [Route("")]
    public class HomeController : Controller
    {
        private readonly SwaggerOptions _swaggerOptions;

        public HomeController(IOptions<SwaggerOptions> options)
        {
            this._swaggerOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.Redirect(this._swaggerOptions.Endpoint);
        }
    }
}
