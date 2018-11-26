using System;
using Challenge1.Data.Abstractions;
using Challenge1.Features.Models;
using Microsoft.AspNetCore.Mvc;

namespace Challenge1.Features
{
    [Route("messages")]
    public class MessageController : Controller
    {
        private readonly IServiceRepository _repo;

        public MessageController(IServiceRepository repository)
        {
            this._repo = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{hash}")]
        public ActionResult<MessageResponseModel> GetMessage([FromRoute] string hash)
        {
            return this.Ok();
        }

        [HttpPost]
        public ActionResult<MessageRequestModel> CreateMessage([FromBody] MessageRequestModel model)
        {
            return this.Ok();
        }
    }
}
