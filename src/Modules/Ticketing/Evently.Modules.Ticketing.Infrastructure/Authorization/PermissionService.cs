using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Common.Application.Authorization;
using Evently.Common.Application.Caching;
using Evently.Common.Domain;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;
using OpenTelemetry.Metrics;

namespace Evently.Modules.Ticketing.Infrastructure.Authorization;
internal class PermissionService : IPermissionService
{
    // this in the old module was to call to db to get user permission, 
    // but in the new module we are extracting this to a separate service
    // so it is not so straight forward we have some choices:
    // 1. make api call to user module throughh httpClient
    // 2. make grpc call to user moduule ( not supported in the lesson )
    // 3. use Request / Response messageing with MassTransit ( THIS IS DA WAY )
    
    private readonly IRequestClient<GetUserPermissionRequest> _requestClient;
    private readonly ICacheService _cacheService;
    private static readonly Error NotFound = Error.NotFound(nameof(PermissionService), "The user was not found");
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);
    public PermissionService(IRequestClient<GetUserPermissionRequest> requestClient, ICacheService cacheService)
    {
        _requestClient = requestClient;
        _cacheService = cacheService;
    }

    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        PermissionsResponse? permissionResponse = await _cacheService.GetAsync<PermissionsResponse>(CreateCacheKey(identityId));
        if (permissionResponse is not null)
            return permissionResponse;

        var request = new GetUserPermissionRequest(identityId);
        Response<PermissionsResponse, Error> response = await _requestClient.GetResponse<PermissionsResponse, Error>(request);

        if (response.Is(out Response<Error> errorResponse))
        {
            return Result.Failure<PermissionsResponse>(errorResponse.Message);
        }
        if (response.Is(out Response<PermissionsResponse> permissionRes))
        {
            await _cacheService.SetAsync(CreateCacheKey(identityId),permissionRes.Message, CacheExpiration);
            return Result.Success<PermissionsResponse>(permissionRes.Message);
        }
        return Result.Failure<PermissionsResponse>(NotFound);
    }
    private static string CreateCacheKey(string identityId) => $"user-permission:{identityId}";
}
