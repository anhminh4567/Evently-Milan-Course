using System.Reflection;
using Evently.Common.Application.Data;
using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Microsoft.EntityFrameworkCore;

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
	public Task BeginTransactionAsync(CancellationToken tokeen = default)
    {
        return Database.BeginTransactionAsync(tokeen);
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
