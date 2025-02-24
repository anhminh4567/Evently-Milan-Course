namespace Evently.Modules.Events.Api.Events;

public sealed record EventResponse(
    string Id,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc);
