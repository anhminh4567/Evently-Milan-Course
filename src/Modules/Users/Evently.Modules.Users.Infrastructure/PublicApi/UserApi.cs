using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.PublicApi;
using MediatR;

namespace Evently.Modules.Users.Infrastructure.PublicApi;
internal class UserApi : IUserApi
{
    private readonly ISender _sender;

    public UserApi(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Modules.Users.PublicApi.UserResponse> GetAsync(string userId, CancellationToken cancellationToken = default)
    {
        Common.Domain.Result<Application.Users.GetUser.UserResponse> result = await _sender.Send(new GetUserQuery(userId), cancellationToken);
        if (result.IsFailure)
            return null;
        return new Modules.Users.PublicApi.UserResponse(result.Value.Id, result.Value.Email, result.Value.FirstName, result.Value.LastName);
    }
}
