namespace MassTransitSagaImplementation.Events;

public class CancellationEmailSentEvent
{
    public Guid OrderId { get; set; }
}