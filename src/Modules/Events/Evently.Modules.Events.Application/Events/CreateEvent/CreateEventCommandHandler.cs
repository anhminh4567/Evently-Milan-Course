using Evently.Modules.Events.Application.Abstractions;
using Evently.Modules.Events.Application.Abstractions.Clock;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using MediatR;


namespace Evently.Modules.Events.Application.Events.CreateEvent;

public sealed record CreateEventCommand(
    string CategoryId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc) : ICommand<string>; 
internal sealed class CreateEventCommandHandler : ICommandHandler<CreateEventCommand,string>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICategoryRepository _categoryRepository;

    public CreateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, ICategoryRepository categoryRepository)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<string>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        if (request.StartsAtUtc < _dateTimeProvider.UtcNow)
        {
            return Result.Failure<string>(EventErrors.StartDateInPast);
        }
        Category? category = await _categoryRepository.GetAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            return Result.Failure<string>(CategoryErrors.NotFound(request.CategoryId));
        }
        var result = Event.Create(
                   category,
                   request.Title,
                   request.Description,
                   request.Location,
                   request.StartsAtUtc,
                   request.EndsAtUtc);
        if (result.IsFailure)
        {
            return Result.Failure<string>(result.Error);
        }
        _eventRepository.Insert(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result.Value.Id;
    }
}

