using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Tickets.CreateTicket;

public sealed record CreateTicketCommand(string TicketId, string AttendeeId, string EventId, string Code) : ICommand;
