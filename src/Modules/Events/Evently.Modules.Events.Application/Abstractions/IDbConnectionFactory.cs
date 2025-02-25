using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace Evently.Modules.Events.Application.Abstractions;
public interface IDbConnectionFactory
{
    Task<DbConnection> OpenConnectionAsync(CancellationToken token = default);
}
