using Evently.Common.Domain;

namespace Evently.Modules.Users.Domain.Users;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetAsync(string id, CancellationToken cancellationToken = default);

    void Insert(User user);
}
