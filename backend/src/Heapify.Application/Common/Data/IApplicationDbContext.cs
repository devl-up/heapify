using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.Common.Data;

public interface IApplicationDbContext
{
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}