using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.CheckInAttendee;

public sealed record CheckInAttendeeCommand(string AttendeeId, string TicketId) : ICommand;
