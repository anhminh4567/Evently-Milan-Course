using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Attendance.Domain.Events;

namespace Evently.Modules.Attendance.Application.EventStatistics.GetEventStatistics;

internal sealed class GetEventStatisticsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetEventStatisticsQuery, EventStatisticsResponse>
{
    public async Task<Result<EventStatisticsResponse>> Handle(
        GetEventStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 "EventId" , --AS {nameof(EventStatisticsResponse.EventId)},
                 "Title" ,--AS {nameof(EventStatisticsResponse.Title)},
                 "Description" ,--AS {nameof(EventStatisticsResponse.Description)},
                 "Location" ,--AS {nameof(EventStatisticsResponse.Location)},
                 "StartsAtUtc",-- AS {nameof(EventStatisticsResponse.StartsAtUtc)},
                 "EndsAtUtc" ,--AS {nameof(EventStatisticsResponse.EndsAtUtc)},
                 "TicketsSold" ,--AS {nameof(EventStatisticsResponse.TicketsSold)},
                 "AttendeesCheckedIn",-- AS {nameof(EventStatisticsResponse.AttendeesCheckedIn)},
                 "DuplicateCheckInTickets", --AS {nameof(EventStatisticsResponse.DuplicateCheckInTickets)},
                 "InvalidCheckInTickets" --AS {nameof(EventStatisticsResponse.InvalidCheckInTickets)}
             FROM attendance."EventStatistics"
             WHERE "EventId" = @EventId
             """;

        var result =
            await connection.QuerySingleOrDefaultAsync(sql, request);
        
        if (result is null)
        {
            return Result.Failure<EventStatisticsResponse>(EventErrors.NotFound(request.EventId));
        }
        var eventStatistics = new EventStatisticsResponse(
            result.EventId,
            result.Title,
            result.Description,
            result.Location,
            result.StartsAtUtc,
            result.EndsAtUtc,
            result.TicketsSold,
            result.AttendeesCheckedIn,
            result.DuplicateCheckInTickets,
            result.InvalidCheckInTickets
        );
        return eventStatistics;
    }
}
