using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Application.Orders.GetOrders;

internal sealed class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, IReadOnlyCollection<OrderResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetOrdersQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<IReadOnlyCollection<OrderResponse>>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 "Id" AS {nameof(OrderResponse.Id)},
                 "CustomerId" AS {nameof(OrderResponse.CustomerId)},
                 "Status" AS {nameof(OrderResponse.Status)},
                 "TotalPrice" AS {nameof(OrderResponse.TotalPrice)},
                 "CreatedAtUtc" AS {nameof(OrderResponse.CreatedAtUtc)}
             FROM ticketing."Orders"
             WHERE "CustomerId" = @CustomerId
             """;

        List<OrderResponse> orders = (await connection.QueryAsync<OrderResponse>(sql, request)).AsList();

        return orders;
    }
}
