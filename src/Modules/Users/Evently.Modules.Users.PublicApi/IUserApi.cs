namespace Evently.Modules.Users.PublicApi;

public interface IUserApi
{
    Task<UserResponse> GetAsync(string userId, CancellationToken cancellationToken = default);
}
public record UserResponse(string Id, string Email, string FirstName, string LastName);
