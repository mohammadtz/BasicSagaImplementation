namespace MassTransitSagaImplementation.Events;

public class PaymentProcessedEvent
{
    public Guid OrderId { get; set; }
}