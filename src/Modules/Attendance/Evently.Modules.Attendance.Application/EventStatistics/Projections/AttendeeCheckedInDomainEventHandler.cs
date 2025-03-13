using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class AttendeeCheckedInDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<AttendeeCheckedInDomainEvent>
{
    public override async Task Handle(
        AttendeeCheckedInDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            UPDATE attendance."EventStatistics" es
            SET "AttendeesCheckedIn" = (
                SELECT COUNT(*)
                FROM attendance."Tickets" t
                WHERE
                    t."EventId" = es."EventId" AND
                    t."UsedAtUtc" IS NOT NULL)
            WHERE es."EventId" = @EventId
            """;

        await connection.ExecuteAsync(sql, domainEvent);
    }
}
