using Evently.Modules.Events.Application.Abstractions.Messaging;

namespace Evently.Modules.Events.Application.Categories.UpdateCategory;

public sealed record UpdateCategoryCommand(string CategoryId, string Name) : ICommand;
