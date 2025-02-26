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
                 e.id AS {nameof(EventResponse.Id)},
                 e.category_id AS {nameof(EventResponse.CategoryId)},
                 e.title AS {nameof(EventResponse.Title)},
                 e.description AS {nameof(EventResponse.Description)},
                 e.location AS {nameof(EventResponse.Location)},
                 e.starts_at_utc AS {nameof(EventResponse.StartsAtUtc)},
                 e.ends_at_utc AS {nameof(EventResponse.EndsAtUtc)},
                 tt.id AS {nameof(TicketTypeResponse.TicketTypeId)},
                 tt.name AS {nameof(TicketTypeResponse.Name)},
                 tt.price AS {nameof(TicketTypeResponse.Price)},
                 tt.currency AS {nameof(TicketTypeResponse.Currency)},
                 tt.quantity AS {nameof(TicketTypeResponse.Quantity)}
             FROM events.events e
             LEFT JOIN events.ticket_types tt ON tt.event_id = e.id
             WHERE e.id = @EventId
             """;

        EventResponse? resultEvent = await connection.QuerySingleOrDefaultAsync(sql, request);
        return resultEvent;
    }
}

