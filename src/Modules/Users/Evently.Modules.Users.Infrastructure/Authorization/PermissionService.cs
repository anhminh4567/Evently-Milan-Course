using Evently.Common.Application.Authorization;
using Evently.Common.Domain;
using Evently.Modules.Users.Application.Users.GetUserPermissions;
using MediatR;

namespace Evently.Modules.Users.Infrastructure.Authorization;

internal sealed class PermissionService: IPermissionService
{
    private readonly ISender _sender;

    public PermissionService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        return await _sender.Send(new GetUserPermissionsQuery(identityId));
    }
}
