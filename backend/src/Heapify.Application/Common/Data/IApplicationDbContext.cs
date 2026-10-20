using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Heapify.Application.Common.Data;

public interface IApplicationDbContext
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    IQueryable<T> SqlQuery<T>(FormattableString sql);
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}