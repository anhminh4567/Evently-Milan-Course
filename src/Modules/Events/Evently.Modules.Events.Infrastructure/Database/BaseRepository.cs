using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Evently.Modules.Events.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Database;
internal class BaseRepository<T> : IBaseRepository<T> where T : class
{
	protected readonly EventsDbContext _dbContext;
	protected readonly DbSet<T> _set;

	public BaseRepository(EventsDbContext dbContext)
	{
		_dbContext = dbContext;
		_set = _dbContext.Set<T>();
	}
	public virtual async Task<List<T>> GetAll(CancellationToken token = default)
	{
		return await _set.ToListAsync(token);
	}

	public virtual async Task<T?> GetById(params object[] ids)
	{
		return await _set.FindAsync(ids);
	}
	public virtual int GetCount()
	{
		return _set.Count();
	}
	public virtual void Create(T entity)
	{
		_set.Add(entity);
	}

	public virtual void Delete(T entity)
	{
		_set.Remove(entity);
	}
	public virtual void Update(T entity)
	{
		_set.Update(entity);
	}
}
