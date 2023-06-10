using Domain.Entities;

namespace MassTransitSagaImplementation.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}