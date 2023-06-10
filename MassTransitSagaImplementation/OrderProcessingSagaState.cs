using Domain.Entities;
using MassTransit;

namespace MassTransitSagaImplementation;

public class OrderProcessingSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public State CurrentState { get; set; }
    public Order Order { get; set; }
}