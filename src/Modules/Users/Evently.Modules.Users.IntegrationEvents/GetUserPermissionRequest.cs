using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evently.Modules.Users.IntegrationEvents;
/// <summary>
/// this will be used by the seperate micrtoservice to get user permission
/// called in Evently.Ticketing.Infrastructure ( PermissionService )
/// </summary>
/// <param name="identityId"></param>
public record GetUserPermissionRequest(string identityId)
{
}
