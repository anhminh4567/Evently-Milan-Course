using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evently.Modules.Events.Application.Abstractions;
public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken tokeen = default);
    Task<int> SaveChangesAsync(CancellationToken token = default);
    Task CommitAsync(CancellationToken token = default);
    Task RollbackAsync(CancellationToken token = default);

}
