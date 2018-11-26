using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Challenge1.Core.ActionResults;
using Challenge1.Data.Abstractions;
using Challenge1.Data.Dtos;
using Challenge1.Features.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Challenge1.Features
{
    /// <summary>
    /// Controller for endpoint /messages. It provides ways for user to get SHA256 hash of a given
    /// string message, and query previous messages from the generated hash.
    /// </summary>
    [Route("messages")]
    public class MessageController : Controller
    {
        private readonly IServiceRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IServiceRepository repository, IMapper mapper, ILogger<MessageController> logger)
        {
            this._repo = repository ?? throw new ArgumentNullException(nameof(repository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{hash}")]
        public async Task<ActionResult<MessageResponseModel>> GetMessage([FromRoute] string hash)
        {
            // A valid sha256 hash is of 256 bits long,
            // which encodes to 64 characters in its string representation
            if (string.IsNullOrEmpty(hash) || hash.Length != 64)
            {
                this._logger.LogInformation("Request failed because of invalid hash: {0}.", hash);

                return this.BadRequest(new ErrorResult("Please provide valid hash."));
            }

            if (!await this._repo.HashExistsAsync(hash))
            {
                this._logger.LogInformation("Request failed because of hash not found: {0}.", hash);

                return this.NotFound(new ErrorResult("Message not found."));
            }

            var dto = await this._repo.FindHashAsync(hash);
            var response = this._mapper.Map<MessageResponseModel>(dto);

            this._logger.LogInformation("Request succeeded for hash {0}.", hash);

            return this.Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<MessageRequestModel>> CreateMessage([FromBody] MessageRequestModel model)
        {
            var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Message));
            var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();

            model.Digest = hash;

            if (!await this._repo.HashExistsAsync(hash))
            {
                this._logger.LogInformation("Storing new message into data storage: {0}.", model.Message);

                var dto = this._mapper.Map<HashDto>(model);
                await this._repo.AddHashAsync(dto);

                if (!await this._repo.SaveChangesAsync())
                {
                    throw new DataException("Hash cannot be saved to database.");
                }
            }

            this._logger.LogInformation("Request succeeded for message {0}.", model.Message);

            return this.Ok(model);
        }
    }
}
