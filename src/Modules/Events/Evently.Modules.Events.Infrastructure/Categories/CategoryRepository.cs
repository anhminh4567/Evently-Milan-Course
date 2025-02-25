using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Categories;

internal sealed class CategoryRepository: BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(EventsDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<Category?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Category category)
    {
		_dbContext.Categories.Add(category);
    }
}
