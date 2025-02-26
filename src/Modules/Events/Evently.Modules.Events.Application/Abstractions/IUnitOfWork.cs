namespace Evently.Modules.Events.Application.Abstractions;
public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken tokeen = default);
    Task<int> SaveChangesAsync(CancellationToken token = default);
    Task CommitAsync(CancellationToken token = default);
    Task RollbackAsync(CancellationToken token = default);

}
