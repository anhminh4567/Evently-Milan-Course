using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Evently.Modules.Events.Infrastructure.Data;
internal class DbConnectionFactory : IDbConnectionFactory
{
    private readonly EventsDbContext _context;
    private readonly NpgsqlDataSource _dataSource;

    public DbConnectionFactory(EventsDbContext context, NpgsqlDataSource dataSource)
    {
        _context = context;
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
