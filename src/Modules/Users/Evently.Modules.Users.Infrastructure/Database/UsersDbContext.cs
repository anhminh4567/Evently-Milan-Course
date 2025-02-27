using System.Data.Common;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Evently.Modules.Users.Infrastructure.Database;

public sealed class UsersDbContext : DbContext, IUnitOfWork
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    internal DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken tokeen = default)
    {
        if (Database.CurrentTransaction is not null)
        {
            await Database.CurrentTransaction.DisposeAsync();
        }

        return (await Database.BeginTransactionAsync(tokeen)).GetDbTransaction();
    }

    public Task CommitAsync(CancellationToken token = default)
    {
        return Database.CommitTransactionAsync(token);
    }

    public Task RollbackAsync(CancellationToken token = default)
    {
        return Database.RollbackTransactionAsync(token);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
    }
}
