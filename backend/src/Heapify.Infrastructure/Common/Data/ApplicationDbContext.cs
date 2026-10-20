using System.Reflection;
using Heapify.Application.Common.Data;
using Heapify.Domain.Users.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Heapify.Infrastructure.Common.Data;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User, Role, Guid>(options), IApplicationDbContext
{
    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return Database.BeginTransactionAsync();
    }

    public IQueryable<T> SqlQuery<T>(FormattableString sql)
    {
        return Database.SqlQuery<T>(sql);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}