using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Application.Interfaces.Persistence;

public interface IImpresoraRepository
    : IGenericRepository<Impresora, Guid>
{
    Task<IReadOnlyList<Impresora>> GetByAgenteIdAsync(
        Guid agenteId,
        CancellationToken cancellationToken = default);
}