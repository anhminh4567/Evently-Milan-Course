using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Application.TicketTypes.GetTicketTypes;

internal sealed class GetTicketTypesQueryHandler : IQueryHandler<GetTicketTypesQuery, IReadOnlyCollection<TicketTypeResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetTicketTypesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<IReadOnlyCollection<TicketTypeResponse>>> Handle(
        GetTicketTypesQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync(cancellationToken);

        const string sql =
            $"""
             SELECT
                 id AS {nameof(TicketTypeResponse.Id)},
                 event_id AS {nameof(TicketTypeResponse.EventId)},
                 name AS {nameof(TicketTypeResponse.Name)},
                 price AS {nameof(TicketTypeResponse.Price)},
                 currency AS {nameof(TicketTypeResponse.Currency)},
                 quantity AS {nameof(TicketTypeResponse.Quantity)}
             FROM events.ticket_types
             WHERE event_id = @EventId
             """;

        List<TicketTypeResponse> ticketTypes =
            (await connection.QueryAsync<TicketTypeResponse>(sql, request)).AsList();

        return ticketTypes;
    }
}
