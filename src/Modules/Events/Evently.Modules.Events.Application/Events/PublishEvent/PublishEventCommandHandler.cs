using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.Events.PublishEvent;
public sealed record PublishEventCommand(string EventId) : ICommand;

internal sealed class PublishEventCommandHandler(
    IEventRepository eventRepository,
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<PublishEventCommand>
{
    public async Task<Result> Handle(PublishEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetById(request.EventId, cancellationToken);
        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }
        if (!await ticketTypeRepository.ExistsAsync(@event.Id, cancellationToken))
        {
            return Result.Failure(EventErrors.NoTicketsFound);
        }

        Result result = @event.Publish();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
