using System.Data.Common;
using Evently.Modules.Events.Domain.Events;
using Dapper;
using Evently.Common.Application.Messaging;
using Evently.Common.Application.Data;

namespace Evently.Modules.Events.Application.Events.GetEvent;

public record GetEventQuery(string id) : IQuery<EventResponse>;


internal sealed class GetEventQueryHandler : IQueryHandler<GetEventQuery, EventResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetEventQueryHandler(IEventRepository eventRepository, IDbConnectionFactory dbConnectionFactory)
    {
        _eventRepository = eventRepository;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<EventResponse>> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync(cancellationToken);
        const string sql =
            $"""
             SELECT
                 e."Id" AS {nameof(EventResponse.Id)},
                 e."CategoryId "AS {nameof(EventResponse.CategoryId)},
                 e."Title" AS {nameof(EventResponse.Title)},
                 e."Description" AS {nameof(EventResponse.Description)},
                 e."Location" AS {nameof(EventResponse.Location)},
                 e."StartsAtUtc" AS {nameof(EventResponse.StartsAtUtc)},
                 e."EndsAtUtc" AS {nameof(EventResponse.EndsAtUtc)},
                 tt."Id" AS {nameof(TicketTypeResponse.TicketTypeId)},
                 tt."Name" AS {nameof(TicketTypeResponse.Name)},
                 tt."Price" AS {nameof(TicketTypeResponse.Price)},
                 tt."Currency" AS {nameof(TicketTypeResponse.Currency)},
                 tt."Quantity" AS {nameof(TicketTypeResponse.Quantity)}
             FROM events."Events" e
             LEFT JOIN events."TicketTypes" tt ON tt."EventId" = e."Id"
             WHERE e."Id" = @EventId
             """;

        EventResponse? resultEvent = await connection.QuerySingleOrDefaultAsync(sql, request);
        return resultEvent;
    }
}

