using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Common.Application.Authorization;
using Evently.Common.Domain;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;

namespace Evently.Modules.Users.Presentation.Users;
// this handle the GetUserPermissionREquest()
// called form a microservice
// use Masstransit , Request/ Response pattern
// see the caller in Evently.Ticketing.Infrastructure (  PermissionService) 
public class GetUserPermissionRequestConsumer : IConsumer<GetUserPermissionRequest>
{
    private readonly IPermissionService _permissionService;

    public GetUserPermissionRequestConsumer(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task Consume(ConsumeContext<GetUserPermissionRequest> context)
    {
        Result<PermissionsResponse> result = await _permissionService.GetUserPermissionsAsync(context.Message.identityId);
        if (result.IsSuccess)
        {
            await context.RespondAsync(result.Value);
        }else
        {
            await context.RespondAsync(result.Error);
        }
    }
}
