using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.TicketTypes.GetTicketType;

internal sealed class GetTicketTypeQueryHandler : IQueryHandler<GetTicketTypeQuery, TicketTypeResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetTicketTypeQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<Result<TicketTypeResponse>> Handle(
        GetTicketTypeQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql =
            $"""
             SELECT
                 "Id" AS {nameof(TicketTypeResponse.Id)},
                 "EventId" AS {nameof(TicketTypeResponse.EventId)},
                 "Name" AS {nameof(TicketTypeResponse.Name)},
                 "Price" AS {nameof(TicketTypeResponse.Price)},
                 "Currency" AS {nameof(TicketTypeResponse.Currency)},
                 "Quantity" AS {nameof(TicketTypeResponse.Quantity)}
             FROM events."TicketTypes"
             WHERE "Id" = @TicketTypeId
             """;

        TicketTypeResponse? ticketType =
            await connection.QuerySingleOrDefaultAsync<TicketTypeResponse>(sql, request);

        if (ticketType is null)
        {
            return Result.Failure<TicketTypeResponse>(TicketTypeErrors.NotFound(request.TicketTypeId));
        }
        return ticketType;
    }
}
