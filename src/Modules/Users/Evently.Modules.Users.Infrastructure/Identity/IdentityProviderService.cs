using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Common.Domain;
using Evently.Modules.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;

namespace Evently.Modules.Users.Infrastructure.Identity;
internal class IdentityProviderService : IIdentityProviderService
{
    private const string PasswordCredentialType = "Password";
    private readonly KeyCloakClient _keyCloakClient;
    private readonly ILogger<IdentityProviderService> _logger;

    public IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger)
    {
        _keyCloakClient = keyCloakClient;
        _logger = logger;
    }

    // POST /admin/realms/{realms}/users
    // doc in https://www.keycloak.org/docs-api/latest/rest-api/index.html
    public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken token = default)
    {
        // register user with keycloak
        // type password (public client)
        // is temporal password, no, since we dont want user to make new pass aagain
        // assume everything is verified (email, pass..)
        var userReprentation = new UserRepresentation(user.Email, user.Email, user.FirstName, user.LastName, true, true, [
            new CredentialRepresentation(PasswordCredentialType,user.Password,false)
            ]);

        try
        {
            string identityId = await _keyCloakClient.RegisterUserAsync(userReprentation, token);
            return identityId;
        }
        catch (HttpRequestException ex) when( ex.StatusCode == System.Net.HttpStatusCode.Conflict)  
        {
            _logger.LogError(ex, "User registeer failed");
            return Result.Failure<string>(IdentityProviderErrrors.EmailIsNotUnique);
        }
    }
}
