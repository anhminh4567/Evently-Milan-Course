using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Users.Infrastructure.Users;

internal sealed class UserRepository: BaseRepository<User> , IUserRepository
{
    private UsersDbContext _context => _dbContext as UsersDbContext;
    public UserRepository(UsersDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public void Insert(User user)
    {
        _context.Users.Add(user);
    }
}
