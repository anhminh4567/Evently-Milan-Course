namespace Evently.Modules.Attendance.Application.EventStatistics.GetEventStatistics;

public sealed record EventStatisticsResponse(
    string EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    int TicketsSold,
    int AttendeesCheckedIn,
    string[] DuplicateCheckInTickets,
    string[] InvalidCheckInTickets);
