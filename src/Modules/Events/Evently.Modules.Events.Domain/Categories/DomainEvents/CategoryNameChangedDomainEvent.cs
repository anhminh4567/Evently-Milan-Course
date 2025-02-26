using Evently.Common.Domain;

namespace Evently.Modules.Events.Domain.Categories.DomainEvents;

public sealed record CategoryNameChangedDomainEvent(string categoryId, string name) : DomainEvent
{
    public string CategoryId { get; init; } = categoryId;

    public string Name { get; init; } = name;
}
