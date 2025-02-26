using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Common.Application.Data;
//using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Evently.Common.Infrastructure.Data;
internal class DbConnectionFactory : IDbConnectionFactory
{
    //private readonly EventsDbContext _context;
    private readonly NpgsqlDataSource _dataSource;

    public DbConnectionFactory( NpgsqlDataSource dataSource)//EventsDbContext context,
    {
        //_context = context;
        _dataSource = dataSource;
    }
    // we inject the datasource from singleton intead of using _context as it is scoped
    // reduce the init call
    // this connection can be shared 
    public async Task<DbConnection> OpenConnectionAsync(CancellationToken token = default)
    {
        return await _dataSource.OpenConnectionAsync(token);
    }
}
