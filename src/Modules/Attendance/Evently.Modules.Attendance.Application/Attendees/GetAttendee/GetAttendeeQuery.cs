using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.GetAttendee;

public sealed record GetAttendeeQuery(string CustomerId) : IQuery<AttendeeResponse>;
