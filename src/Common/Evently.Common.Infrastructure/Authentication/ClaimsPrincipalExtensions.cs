using System.Security.Claims;
using Evently.Common.Application.Exceptions;

namespace Evently.Common.Infrastructure.Authentication;


// use to modify the incomming accceess token
// since it is from Keycloak, it does not match application businsess
// ==> this parse to business claims
public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirst(CustomClaims.Sub)?.Value;

        return userId is not null ?
            userId :
            throw new EventlyException("User identifier is unavailable");
    }

    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
               throw new EventlyException("User identity is unavailable");
    }

    public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
    {
        IEnumerable<Claim> permissionClaims = principal?.FindAll(CustomClaims.Permission) ??
                                              throw new EventlyException("Permissions are unavailable");

        return permissionClaims.Select(c => c.Value).ToHashSet();
    }
}
