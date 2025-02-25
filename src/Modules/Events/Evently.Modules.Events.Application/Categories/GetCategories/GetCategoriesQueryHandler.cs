using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Categories.GetCategory;
using Evently.Modules.Events.Domain.Abstractions;

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
                 id AS {nameof(CategoryResponse.Id)},
                 name AS {nameof(CategoryResponse.Name)},
                 is_archived AS {nameof(CategoryResponse.IsArchived)}
             FROM events.categories
             """;

        List<CategoryResponse> categories = (await connection.QueryAsync<CategoryResponse>(sql, request)).AsList();

        return categories;
    }
}
