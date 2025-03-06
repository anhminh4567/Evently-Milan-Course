using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Events.CreateEvent;

public sealed record CreateEventCommand(
    string EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc) : ICommand;
