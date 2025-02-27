using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.GetUser;

internal sealed class GetUserQueryHandler: IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetUserQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql =
            $"""
             SELECT
                 "Id" AS {nameof(UserResponse.Id)},
                 "Email" AS {nameof(UserResponse.Email)},
                 "FirstName" AS {nameof(UserResponse.FirstName)},
                 "LastName" AS {nameof(UserResponse.LastName)}
             FROM users."Users"
             WHERE "Id" = @UserId
             """;

        UserResponse? user = await connection.QuerySingleOrDefaultAsync<UserResponse>(sql, request);
        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(request.UserId));
        }
        return user;
    }
}
