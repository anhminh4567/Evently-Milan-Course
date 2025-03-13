using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class DuplicateCheckInAttemptedDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<DuplicateCheckInAttemptedDomainEvent>
{
    public override async Task Handle(
        DuplicateCheckInAttemptedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            UPDATE attendance."EventStatistics" es
            SET "DuplicateCheckInTickets" = array_append("DuplicateCheckInTickets", @TicketCode)
            WHERE es."EventId" = @EventId
            """;

        await connection.ExecuteAsync(sql, domainEvent);
    }
}
