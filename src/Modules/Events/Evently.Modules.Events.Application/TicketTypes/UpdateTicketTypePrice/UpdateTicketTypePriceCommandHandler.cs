using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class UpdateTicketTypePriceCommandHandler : ICommandHandler<UpdateTicketTypePriceCommand>
{
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTicketTypePriceCommandHandler(ITicketTypeRepository ticketTypeRepository, IUnitOfWork unitOfWork)
    {
        _ticketTypeRepository = ticketTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTicketTypePriceCommand request, CancellationToken cancellationToken)
    {
        TicketType? ticketType = await _ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);
        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));
        }
        ticketType.UpdatePrice(request.Price);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
