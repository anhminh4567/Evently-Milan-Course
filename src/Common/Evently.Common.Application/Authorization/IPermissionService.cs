using Evently.Common.Domain;

namespace Evently.Common.Application.Authorization;

// this interface is for each module to immplement their own services
// not for Common.Infra, BUT It is used in Infra ( Authorization, CustomClaimTransformation
public interface IPermissionService
{
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}
