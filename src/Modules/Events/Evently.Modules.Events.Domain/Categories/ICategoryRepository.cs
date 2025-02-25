using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.Categories;

public interface ICategoryRepository: IBaseRepository<Category>
{
    Task<Category?> GetAsync(string id, CancellationToken cancellationToken = default);

    void Insert(Category category);
}
