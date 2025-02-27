using Evently.Common.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.UpdateUser;

public sealed record UpdateUserCommand(string UserId, string FirstName, string LastName) : ICommand;
