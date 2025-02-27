using System.Data.Common;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Payments;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;

internal sealed class RefundPaymentsForEventCommandHandler : ICommandHandler<RefundPaymentsForEventCommand>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentRepository _paymentRepository;

    public RefundPaymentsForEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork, IPaymentRepository paymentRepository)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _paymentRepository = paymentRepository;
    }

    public async Task<Result> Handle(RefundPaymentsForEventCommand request, CancellationToken cancellationToken)
    {
        await using DbTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        Event? @event = await _eventRepository.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }

        IEnumerable<Payment> payments = await _paymentRepository.GetForEventAsync(@event, cancellationToken);

        foreach (Payment payment in payments)
        {
            payment.Refund(payment.Amount - (payment.AmountRefunded ?? decimal.Zero));
        }

        @event.PaymentsRefunded();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
