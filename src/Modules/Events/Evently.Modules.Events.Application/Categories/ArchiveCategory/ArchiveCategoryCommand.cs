using Evently.Common.Application.Messaging;

namespace Evently.Modules.Events.Application.Categories.ArchiveCategory;

public sealed record ArchiveCategoryCommand(string CategoryId) : ICommand;
