using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Application.Categories.GetCategory;

namespace Evently.Modules.Events.Application.Categories.GetCategories;

internal sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetCategoriesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<IReadOnlyCollection<CategoryResponse>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql =
            $"""
             SELECT
                 "Id" AS {nameof(CategoryResponse.Id)},
                 "Name" AS {nameof(CategoryResponse.Name)},
                 "IsArchived" AS {nameof(CategoryResponse.IsArchived)}
             FROM events."Categories"
             """;

        List<CategoryResponse> categories = (await connection.QueryAsync<CategoryResponse>(sql, request)).AsList();

        return categories;
    }
}
