using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Application.Interfaces.Persistence;

public interface IGenericRepository<T, TId>
    where T : BaseEntity<TId>
{
    Task<T?> GetByIdAsync(
        TId id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        T entity,
        CancellationToken cancellationToken = default);

    void Update(T entity);

    void Delete(T entity);
}