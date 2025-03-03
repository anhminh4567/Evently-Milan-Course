using Evently.Common.Domain;

namespace Evently.Modules.Users.Application.Abstractions.Identity;

public static class IdentityProviderErrrors
{
    public static readonly Error EmailIsNotUnique = Error.Conflict(
    "Identity.EmailIsNotUnique",
    "The specified email is not unique.");
}
