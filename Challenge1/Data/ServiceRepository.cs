using System;
using System.Threading.Tasks;
using Challenge1.Data.Abstractions;
using Challenge1.Data.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Challenge1.Data
{
    /// <summary>
    /// <see cref="ServiceRepository"/> provides ways for user to query data and
    /// execute commands to the specified data storage.
    /// </summary>
    public class ServiceRepository : IServiceRepository
    {
        private readonly ServiceContext _context;
        private readonly ILogger<ServiceRepository> _logger;

        public ServiceRepository(ServiceContext context, ILogger<ServiceRepository> logger)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<bool> HashExistsAsync(string hash)
        {
            this._logger.LogInformation("Querying if hash exists for value {0}.", hash);

            return this._context.Hashes.AnyAsync(h => h.Hash == hash);
        }

        public Task<HashDto> FindHashAsync(string hash)
        {
            this._logger.LogInformation("Finding hash entry for value {0}.", hash);

            return this._context.Hashes.FindAsync(hash);
        }

        public Task AddHashAsync(HashDto dto)
        {
            this._logger.LogInformation(
                "Adding a new hash entry for message {0} with value {1}.",
                dto.Value,
                dto.Hash);

            this._context.Hashes.Add(dto);
            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync()
        {
            this._logger.LogInformation("Saving changes to the data storage.");

            return await this._context.SaveChangesAsync() > 0;
        }
    }
}
