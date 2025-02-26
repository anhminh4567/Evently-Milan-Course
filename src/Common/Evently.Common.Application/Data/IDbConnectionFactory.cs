using System.Data.Common;
namespace Evently.Common.Application.Data;
public interface IDbConnectionFactory
{
    Task<DbConnection> OpenConnectionAsync(CancellationToken token = default);
}
