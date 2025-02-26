using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Evently.Common.Domain;
public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAll(CancellationToken token = default);
    Task<TEntity> GetById(params object[] ids);
    //IQueryable<TEntity> GetQuery();
    //IQueryable<TEntity> QueryFilter(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter = null);
    //IQueryable<TEntity> QueryOrderBy(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    //IQueryable<TEntity> QueryInclude<TProperty>(IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> navigation);
    //IQueryable<TEntity> QuerySplit(IQueryable<TEntity> query);
    void Create(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    int GetCount();
}
