using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.Categories.DomainEvents;

public sealed record CategoryCreatedDomainEvent(string categoryId) : DomainEvent
{
    public string CategoryId { get; init; } = categoryId;
}
