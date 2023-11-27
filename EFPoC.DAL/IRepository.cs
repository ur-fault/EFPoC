using Microsoft.EntityFrameworkCore;

using CT = System.Threading.CancellationToken;

namespace EFPoC.DAL;

public interface IRepository<T> where T : class
{
    MyDbContext DbContext { get; }

    Task<IList<T>> GetAllAsync(CT cancellationToken = default);

    Task<T?> GetByIdAsync(int id, CT cancellationToken = default);

    Task<T> AddAsync(T entity, CT cancellationToken = default);

    // TODO: Add AddRangeAsync

    // TODO: Add UpdateAsync

    Task DeleteAsync(T entity, CT cancellationToken = default);

    // TODO: Add DeleteByIdAsync
}
