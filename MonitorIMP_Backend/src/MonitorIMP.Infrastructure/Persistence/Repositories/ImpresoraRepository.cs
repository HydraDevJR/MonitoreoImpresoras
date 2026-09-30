using Microsoft.EntityFrameworkCore;
using MonitorIMP.Application.Interfaces.Persistence;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Repositories;

public class ImpresoraRepository 
    : GenericRepository<Impresora, Guid>, IImpresoraRepository
{
    public ImpresoraRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    public async Task<IReadOnlyList<Impresora>> GetByAgenteIdAsync(
        Guid agenteId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(i => i.AgenteId == agenteId && i.Activo)
            .ToListAsync(cancellationToken);
    }
}