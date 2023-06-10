using Domain.Entities;
using MassTransit;
using MassTransitSagaImplementation.Events;
using MassTransitSagaImplementation.Services;

#pragma warning disable CS8618

namespace MassTransitSagaImplementation.Commands;

public class ProcessPaymentCommand
{
    public Guid OrderId { get; set; }
}

public class ProcessPaymentCommandHandler : IConsumer<ProcessPaymentCommand>
{
    private readonly IPaymentService _paymentService;
    private readonly IBus _bus;

    public ProcessPaymentCommandHandler(IPaymentService paymentService, IBus bus)
    {
        _paymentService = paymentService;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<ProcessPaymentCommand> context)
    {
        var paymentResult = await _paymentService.ProcessPayment(context.Message.OrderId);

        if (paymentResult.Status == PaymentStatus.Successful)
        {
            await context.Publish(new PaymentProcessedEvent { OrderId = context.Message.OrderId });
        }
        else
        {
            await context.Publish(new PaymentFailedEvent { OrderId = context.Message.OrderId });
        }
    }
}