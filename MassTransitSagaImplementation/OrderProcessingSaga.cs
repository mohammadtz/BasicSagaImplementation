using MassTransit;
using MassTransitSagaImplementation.Commands;
using MassTransitSagaImplementation.Events;

namespace MassTransitSagaImplementation;

public class OrderProcessingSaga : MassTransitStateMachine<OrderProcessingSagaState>
{
    public OrderProcessingSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreated, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentProcessed, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentFailed, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => ConfirmationEmailSent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => CancellationEmailSent, x => x.CorrelateById(context => context.Message.OrderId));

        Initially(
            When(OrderCreated)
                .Then(context => context.Saga.Order = context.Message.Order)
                .ThenAsync(async context => await context.Publish(new ProcessPaymentCommand { OrderId = context.Saga.Order.Id }))
                .TransitionTo(ProcessingPayment)
        );

        During(ProcessingPayment,
            When(PaymentProcessed)
                .ThenAsync(async context => await context.Publish(new SendConfirmationEmailCommand { CustomerEmail = context.Saga.Order.CustomerEmail, OrderId = context.Saga.Order.Id }))
                .TransitionTo(OrderProcessed),
            When(PaymentFailed)
                .ThenAsync(async context => await context.Publish(new SendCancellationEmailCommand { CustomerEmail = context.Saga.Order.CustomerEmail, OrderId = context.Saga.Order.Id }))
                .Finalize()
        );

        During(ProcessingPayment,
            Ignore(PaymentProcessed),
            Ignore(OrderCreated),
            Ignore(ConfirmationEmailSent),
            Ignore(CancellationEmailSent)
        );

        During(OrderProcessed,
            When(ConfirmationEmailSent)
                .Finalize(),
            When(CancellationEmailSent)
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }

    public State ProcessingPayment { get; private set; }
    public State OrderProcessed { get; private set; }

    public Event<OrderCreatedEvent> OrderCreated { get; private set; }
    public Event<PaymentProcessedEvent> PaymentProcessed { get; private set; }
    public Event<PaymentFailedEvent> PaymentFailed { get; private set; }
    public Event<ConfirmationEmailSentEvent> ConfirmationEmailSent { get; private set; }
    public Event<CancellationEmailSentEvent> CancellationEmailSent { get; private set; }
}