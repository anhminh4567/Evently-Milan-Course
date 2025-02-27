namespace Evently.Common.Application.Data;
public interface IBaseUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken tokeen = default);
    Task<int> SaveChangesAsync(CancellationToken token = default);
    Task CommitAsync(CancellationToken token = default);
    Task RollbackAsync(CancellationToken token = default);

}
