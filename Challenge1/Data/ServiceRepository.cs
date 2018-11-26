using System;
using Challenge1.Data.Abstractions;

namespace Challenge1.Data
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ServiceContext _context;

        public ServiceRepository(ServiceContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
