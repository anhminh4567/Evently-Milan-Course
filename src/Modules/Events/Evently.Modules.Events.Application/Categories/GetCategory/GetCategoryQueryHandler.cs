using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Application.Categories.GetCategory;

internal sealed class GetCategoryQueryHandler
    : IQueryHandler<GetCategoryQuery, CategoryResponse>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetCategoryQueryHandler(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<CategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _connectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(CategoryResponse.Id)},
                 name AS {nameof(CategoryResponse.Name)},
                 is_archived AS {nameof(CategoryResponse.IsArchived)}
             FROM events.categories
             WHERE id = @CategoryId
             """;

        CategoryResponse? category = await connection.QuerySingleOrDefaultAsync<CategoryResponse>(sql, request);

        if (category is null)
        {
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(request.CategoryId));
        }

        return category;
    }
}
