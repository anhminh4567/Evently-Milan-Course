using System.Security.Claims;
using Evently.Common.Application.Authorization;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain;
using Evently.Common.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastructure.Authorization;

internal sealed class CustomClaimsTransformation : IClaimsTransformation
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CustomClaimsTransformation(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(c => c.Type == CustomClaims.Sub))
            return principal;
        
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        // this is our service, not from microsoft
        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
        string identityId = principal.GetIdentityId(); // this is an extension method ( no need to care )
        
        Result<PermissionsResponse> result = await permissionService.GetUserPermissionsAsync(identityId);
        if (result.IsFailure)
            throw new EventlyException(nameof(IPermissionService.GetUserPermissionsAsync), result.Error);
        
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(CustomClaims.Sub, result.Value.UserId.ToString()));

        foreach (string permission in result.Value.Permissions)
        {
            claimsIdentity.AddClaim(new Claim(CustomClaims.Permission, permission));
        }

        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}
