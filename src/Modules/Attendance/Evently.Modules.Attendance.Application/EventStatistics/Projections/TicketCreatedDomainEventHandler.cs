using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.Domain.Tickets.DomainEvents;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class TicketCreatedDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<TicketCreatedDomainEvent>
{
    public override async Task Handle(
        TicketCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            UPDATE attendance."EventStatistics" es
            SET "TicketsSold" = (
                SELECT COUNT(*)
                FROM attendance."Tickets" t
                WHERE t."EventId" = es."EventId")
            WHERE es."EventId" = @EventId
            """;

        await connection.ExecuteAsync(sql, domainEvent);
    }
}
