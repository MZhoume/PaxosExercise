using System.Threading.Tasks;
using Challenge1.Data.Dtos;

namespace Challenge1.Data.Abstractions
{
    /// <summary>
    /// <see cref="IServiceRepository"/> is an abstraction of the <see cref="ServiceRepository"/>, provides
    /// ways for user to query data and execute commands to the specified data storage. It also allows better
    /// testability.
    /// </summary>
    public interface IServiceRepository
    {
        Task<bool> HashExistsAsync(string hash);

        Task<HashDto> FindHashAsync(string hash);

        Task AddHashAsync(HashDto dto);

        Task<bool> SaveChangesAsync();
    }
}
