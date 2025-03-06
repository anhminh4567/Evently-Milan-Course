using Evently.Common.Infrastructure.Repositories;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Categories;

internal sealed class CategoryRepository: BaseRepository<Category>, ICategoryRepository
{
    private EventsDbContext _currentContext => _dbContext as EventsDbContext;
    public CategoryRepository(EventsDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<Category?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _currentContext.Categories.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Category category)
    {
        _currentContext.Categories.Add(category);
    }
}
