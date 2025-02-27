using System.Data.Common;
using System.Reflection;
using System.Threading;
using Evently.Common.Application.Data;
using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Evently.Modules.Events.Infrastructure.Database;

public sealed class EventsDbContext : DbContext , IUnitOfWork
{
    //public EventsDbContext()
    //{
    //}

    public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
    {
    }

    internal DbSet<Event> Events { get; set; }
    internal DbSet<Category> Categories { get; set; }
    internal DbSet<TicketType> TicketTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(Schemas.Events);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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

   
}
