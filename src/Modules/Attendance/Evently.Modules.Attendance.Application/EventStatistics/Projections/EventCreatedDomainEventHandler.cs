using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Events.DomainEvents;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class EventCreatedDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<EventCreatedDomainEvent>
{
    public override async Task Handle(
        EventCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            INSERT INTO attendance."EventStatistics"(
                "EventId",
                "Title",
                "Description",
                "Location",
                "StartsAtUtc",
                "EndsAtUtc",
                "TicketsSold",
                "AttendeesCheckedIn",
                "DuplicateCheckInTickets",
                "InvalidCheckInTickets")
            VALUES (
                @EventId,
                @Title,
                @Description,
                @Location,
                @StartsAtUtc,
                @EndsAtUtc,
                @TicketsSold,
                @AttendeesCheckedIn,
                @DuplicateCheckInTickets,
                @InvalidCheckInTickets)
            """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                domainEvent.EventId,
                domainEvent.Title,
                domainEvent.Description,
                domainEvent.Location,
                domainEvent.StartsAtUtc,
                domainEvent.EndsAtUtc,
                TicketsSold = 0,
                AttendeesCheckedIn = 0,
                DuplicateCheckInTickets = Array.Empty<string>(),
                InvalidCheckInTickets = Array.Empty<string>()
            });
    }
}
