using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Attendance.Domain.Attendees;

namespace Evently.Modules.Attendance.Application.Attendees.GetAttendee;

internal sealed class GetAttendeeQueryQueryHandler(IDbConnectionFactory dbConnectionFactory): IQueryHandler<GetAttendeeQuery, AttendeeResponse>
{
    public async Task<Result<AttendeeResponse>> Handle(GetAttendeeQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 "Id" AS {nameof(AttendeeResponse.Id)},
                 "Email" AS {nameof(AttendeeResponse.Email)},
                 "FirstName" AS {nameof(AttendeeResponse.FirstName)},
                 "LastName" AS {nameof(AttendeeResponse.LastName)}
             FROM attendance."Attendees"
             WHERE "Id" = @CustomerId
             """;

        AttendeeResponse? customer = await connection.QuerySingleOrDefaultAsync<AttendeeResponse>(sql, request);

        if (customer is null)
        {
            return Result.Failure<AttendeeResponse>(AttendeeErrors.NotFound(request.CustomerId.ToString()));
        }

        return customer;
    }
}
