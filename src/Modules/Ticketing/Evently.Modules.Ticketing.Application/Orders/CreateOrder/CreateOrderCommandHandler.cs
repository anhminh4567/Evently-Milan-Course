using System.Data.Common;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CartService _cartService;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentService _paymentService;

    public CreateOrderCommandHandler(ICustomerRepository customerRepository, IOrderRepository orderRepository, ITicketTypeRepository ticketTypeRepository, IUnitOfWork unitOfWork, CartService cartService, IPaymentRepository paymentRepository, IPaymentService paymentService)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
        _ticketTypeRepository = ticketTypeRepository;
        _unitOfWork = unitOfWork;
        _cartService = cartService;
        _paymentRepository = paymentRepository;
        _paymentService = paymentService;
    }

    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await using DbTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        Customer? customer = await _customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        }

        var order = Order.Create(customer);
        Cart cart = await _cartService.GetAsync(customer.Id, cancellationToken);

        if (!cart.Items.Any())
        {
            return Result.Failure(CartErrors.Empty);
        }

        foreach (CartItem cartItem in cart.Items)
        {
            // This acquires a pessimistic lock or throws an exception if already locked.
            TicketType? ticketType = await _ticketTypeRepository.GetWithLockAsync(
                cartItem.TicketTypeId,
                cancellationToken);

            if (ticketType is null)
            {
                return Result.Failure(TicketTypeErrors.NotFound(cartItem.TicketTypeId));
            }

            Result result = ticketType.UpdateQuantity(cartItem.Quantity);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }
            order.AddItem(ticketType, cartItem.Quantity, cartItem.Price, ticketType.Currency);
        }
        _orderRepository.Insert(order);

        // We're faking a payment gateway request here...
        PaymentResponse paymentResponse = await _paymentService.ChargeAsync(order.TotalPrice, order.Currency);

        var payment = Payment.Create(
            order,
            paymentResponse.TransactionId,
            paymentResponse.Amount,
            paymentResponse.Currency);

        _paymentRepository.Insert(payment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        await _cartService.ClearAsync(customer.Id, cancellationToken);

        return Result.Success();
    }
}
