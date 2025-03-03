using System.Data.Common;
using Dapper;
using Evently.Common.Application.Authorization;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.GetUserPermissions;

internal sealed class GetUserPermissionsQueryHandler
    : IQueryHandler<GetUserPermissionsQuery, PermissionsResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetUserPermissionsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<PermissionsResponse>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();
        const string sql =
            $"""
             SELECT DISTINCT
                 u."Id" AS {nameof(UserPermission.UserId)},
                 rp."PermissionCode" AS {nameof(UserPermission.Permission)}
             FROM users."Users" u
             JOIN users."UserRoles" ur ON ur."UserId" = u."Id"
             JOIN users."RolePermissions" rp ON rp."RoleName" = ur."RolesName"
             WHERE u."IdentityId" = @IdentityId
             """;

        List<UserPermission> permissions = (await connection.QueryAsync<UserPermission>(sql, request)).AsList();

        if (!permissions.Any())
        {
            return Result.Failure<PermissionsResponse>(UserErrors.NotFound(request.IdentityId));
        }
        return new PermissionsResponse(permissions[0].UserId, permissions.Select(p => p.Permission).ToHashSet());
    }

    internal sealed class UserPermission
    {
        internal string UserId { get; init; }
        internal string Permission { get; init; }
    }
}
