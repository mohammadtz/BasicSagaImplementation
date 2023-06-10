namespace MassTransitSagaImplementation.Events;

public class ConfirmationEmailSentEvent
{
    public Guid OrderId { get; set; }
}