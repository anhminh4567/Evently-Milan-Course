using Evently.Common.Domain;

namespace Evently.Modules.Events.Domain.Categories;

public interface ICategoryRepository: IBaseRepository<Category>
{
    Task<Category?> GetAsync(string id, CancellationToken cancellationToken = default);

    void Insert(Category category);
}
