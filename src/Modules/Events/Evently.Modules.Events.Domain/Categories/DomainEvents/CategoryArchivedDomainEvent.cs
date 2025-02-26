using Evently.Common.Domain;

namespace Evently.Modules.Events.Domain.Categories.DomainEvents;

public record class CategoryArchivedDomainEvent(string categoryId) : DomainEvent
{
    public string CategoryId { get; init; } = categoryId;
}
