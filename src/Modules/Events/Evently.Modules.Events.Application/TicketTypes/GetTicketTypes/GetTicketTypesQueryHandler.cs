using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;

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
                 "Id" AS {nameof(TicketTypeResponse.Id)},
                 "EventId" AS {nameof(TicketTypeResponse.EventId)},
                 "Name" AS {nameof(TicketTypeResponse.Name)},
                 "Price" AS {nameof(TicketTypeResponse.Price)},
                 "Currency" AS {nameof(TicketTypeResponse.Currency)},
                 "Quantity" AS {nameof(TicketTypeResponse.Quantity)}
             FROM events."TicketTypes"
             WHERE "EventId" = @EventId
             """;

        List<TicketTypeResponse> ticketTypes =
            (await connection.QueryAsync<TicketTypeResponse>(sql, request)).AsList();

        return ticketTypes;
    }
}
