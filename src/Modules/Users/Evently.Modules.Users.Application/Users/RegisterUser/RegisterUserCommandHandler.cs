using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.PublicApi;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler: ICommandHandler<RegisterUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITicketingApi _ticketingApi;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ITicketingApi ticketingApi)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _ticketingApi = ticketingApi;
    }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.Email, request.FirstName, request.LastName);

        _userRepository.Insert(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //we duplicate the useer as customer to ticketing modeul
        // but this is handled by the DomainEventHandler
        //await _ticketingApi.CreateCustomerAsync(user.Id, user.Email, user.FirstName, user.LastName, cancellationToken);

        return user.Id;
    }
}
