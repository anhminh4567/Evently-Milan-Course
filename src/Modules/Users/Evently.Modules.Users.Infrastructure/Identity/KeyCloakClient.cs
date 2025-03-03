using System.Net.Http.Json;

namespace Evently.Modules.Users.Infrastructure.Identity;

internal sealed class KeyCloakClient
{
    // the http client instance is injected through SP
    // see the UserModules.cs
    private readonly HttpClient _httpClient;

    public KeyCloakClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync(
            "users",
            user,
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
    }

    private static string ExtractIdentityIdFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {

        // this section is basically a standard
        // when create somethign API most of the time return a header like this 
        // LocalPath: <location url of the newly created item, in this case is keycloak user>
        // ==> in this path, there is a user id, which we can get from url, 
        // THATS IT !!! this is to get that segment
        //     if you lazy, just call to the endpoints LocalPath to get user id and full user detail
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string identityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

        return identityId;
    }
}
