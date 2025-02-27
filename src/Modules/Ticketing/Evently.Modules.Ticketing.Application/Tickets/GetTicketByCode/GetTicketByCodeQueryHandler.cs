using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Application.Tickets.GetTicket;
using Evently.Modules.Ticketing.Domain.Tickets;

namespace Evently.Modules.Ticketing.Application.Tickets.GetTicketByCode;

internal sealed class GetTicketByCodeQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketByCodeQuery, TicketResponse>
{
    public async Task<Result<TicketResponse>> Handle(GetTicketByCodeQuery request, CancellationToken cancellationToken)
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
            WHERE "Code" = @Code
            """;

        TicketResponse? ticket = await connection.QuerySingleOrDefaultAsync<TicketResponse>(sql, request);

        if (ticket is null)
        {
            return Result.Failure<TicketResponse>(TicketErrors.NotFoundCode(request.Code));
        }

        return ticket;
    }
}
