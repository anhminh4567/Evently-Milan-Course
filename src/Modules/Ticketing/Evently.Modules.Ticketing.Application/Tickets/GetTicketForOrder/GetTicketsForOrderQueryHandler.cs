using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Application.Tickets.GetTicket;

namespace Evently.Modules.Ticketing.Application.Tickets.GetTicketForOrder;

internal sealed class GetTicketsForOrderQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketsForOrderQuery, IReadOnlyCollection<TicketResponse>>
{
    public async Task<Result<IReadOnlyCollection<TicketResponse>>> Handle(
        GetTicketsForOrderQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
                "Id" AS {nameof(TicketResponse.Id)},
                "CustomerId" AS {nameof(TicketResponse.CustomerId)},
                "OrderId" AS {nameof(TicketResponse.OrderId)},
                "EventId" AS {nameof(TicketResponse.EventId)},
                "TicketTypeId" AS {nameof(TicketResponse.TicketTypeId)},
                "Code" AS {nameof(TicketResponse.Code)},
                "CreatedAtUtc" AS {nameof(TicketResponse.CreatedAtUtc)}
            FROM ticketing."Tickets"
            WHERE "OrderId" = @OrderId
            """;

        List<TicketResponse> tickets = (await connection.QueryAsync<TicketResponse>(sql, request)).AsList();

        return tickets;
    }
}
