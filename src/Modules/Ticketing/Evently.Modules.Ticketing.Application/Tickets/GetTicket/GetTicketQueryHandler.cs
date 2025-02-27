using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Tickets;

namespace Evently.Modules.Ticketing.Application.Tickets.GetTicket;

internal sealed class GetTicketQueryHandler : IQueryHandler<GetTicketQuery, TicketResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetTicketQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<TicketResponse>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();
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
            WHERE "Id" = @TicketId
            """;

        TicketResponse? ticket = await connection.QuerySingleOrDefaultAsync<TicketResponse>(sql, request);

        if (ticket is null)
        {
            return Result.Failure<TicketResponse>(TicketErrors.NotFoundId(request.TicketId));
        }

        return ticket;
    }
}
