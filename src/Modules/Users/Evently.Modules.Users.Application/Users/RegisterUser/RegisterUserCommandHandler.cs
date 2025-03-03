using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Application.Abstractions.Identity;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler: ICommandHandler<RegisterUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityProviderService _identityProviderService;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IIdentityProviderService identityProviderService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _identityProviderService = identityProviderService;
    }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        
        // first register with identity provider
        Result<string> registerWithProviderResult = await _identityProviderService.RegisterUserAsync(
            new UserModel(request.Email,request.Password,request.FirstName,request.LastName),cancellationToken);
        if(registerWithProviderResult.IsFailure)
            return Result.Failure<string>(registerWithProviderResult.Error);

        // then create user 
        var user = User.Create(request.Email, request.FirstName, request.LastName, registerWithProviderResult.Value);

        _userRepository.Insert(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //we duplicate the useer as customer to ticketing modeul
        // but this is handled by the DomainEventHandler
        //await _ticketingApi.CreateCustomerAsync(user.Id, user.Email, user.FirstName, user.LastName, cancellationToken);

        return user.Id;
    }
}
