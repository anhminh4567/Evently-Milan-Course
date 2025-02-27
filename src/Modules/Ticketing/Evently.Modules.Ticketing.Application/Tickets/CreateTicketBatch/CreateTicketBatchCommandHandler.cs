using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Tickets;

namespace Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

internal sealed class CreateTicketBatchCommandHandler  : ICommandHandler<CreateTicketBatchCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITicketRepository _ticketRepository;

    public CreateTicketBatchCommandHandler(IOrderRepository orderRepository, ITicketTypeRepository ticketTypeRepository, IUnitOfWork unitOfWork, ITicketRepository ticketRepository)
    {
        _orderRepository = orderRepository;
        _ticketTypeRepository = ticketTypeRepository;
        _unitOfWork = unitOfWork;
        _ticketRepository = ticketRepository;
    }

    public async Task<Result> Handle(CreateTicketBatchCommand request, CancellationToken cancellationToken)
    {
        Order? order = await _orderRepository.GetAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(request.OrderId));
        }

        Result result = order.IssueTickets();
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        List<Ticket> tickets = [];
        foreach (OrderItem orderItem in order.OrderItems)
        {
            TicketType? ticketType = await _ticketTypeRepository.GetAsync(orderItem.TicketTypeId, cancellationToken);
            if (ticketType is null)
            {
                return Result.Failure(TicketTypeErrors.NotFound(orderItem.TicketTypeId));
            }
            for (int i = 0; i < orderItem.Quantity; i++)
            {
                var ticket = Ticket.Create(order, ticketType);
                tickets.Add(ticket);
            }
        }

        _ticketRepository.InsertRange(tickets);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
