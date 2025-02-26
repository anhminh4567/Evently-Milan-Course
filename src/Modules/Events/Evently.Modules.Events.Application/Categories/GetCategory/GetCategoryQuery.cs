using Evently.Common.Application.Messaging;

namespace Evently.Modules.Events.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(string CategoryId) : IQuery<CategoryResponse>;
