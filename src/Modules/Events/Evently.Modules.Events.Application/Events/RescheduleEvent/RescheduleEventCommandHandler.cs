using Evently.Common.Application.Clock;
using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.RescheduleEvent;
public sealed record RescheduleEventCommand(string EventId, DateTime StartsAtUtc, DateTime? EndsAtUtc) : ICommand;

internal sealed class RescheduleEventCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RescheduleEventCommand>
{
    public async Task<Result> Handle(RescheduleEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetById(request.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }

        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
        {
            return Result.Failure(EventErrors.StartDateInPast);
        }

        @event.Reschedule(request.StartsAtUtc, request.EndsAtUtc);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
