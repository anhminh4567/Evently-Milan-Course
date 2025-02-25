using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

internal sealed class CreateTicketTypeCommandHandler   : ICommandHandler<CreateTicketTypeCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRepository _eventRepository;
    private readonly ITicketTypeRepository _ticketTypeRepository;

    public CreateTicketTypeCommandHandler(IUnitOfWork unitOfWork, IEventRepository eventRepository, ITicketTypeRepository ticketTypeRepository)
    {
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
        _ticketTypeRepository = ticketTypeRepository;
    }

    public async Task<Result<string>> Handle(CreateTicketTypeCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await _eventRepository.GetById(request.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure<string>(EventErrors.NotFound(request.EventId));
        }
        var ticketType = TicketType.Create(@event, request.Name, request.Price, request.Currency, request.Quantity);

        _ticketTypeRepository.Insert(ticketType);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ticketType.Id;
    }
}
