using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Application.Abstractions.Clock;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.CancelEvent;
public sealed record CancelEventCommand(string EventId) : ICommand;

internal sealed class CancelEventCommandHandler
    : ICommandHandler<CancelEventCommand>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelEventCommandHandler(IDateTimeProvider dateTimeProvider, IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await _eventRepository.GetById(request.EventId, cancellationToken);
        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }
        Result result = @event.Cancel(_dateTimeProvider.UtcNow);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
