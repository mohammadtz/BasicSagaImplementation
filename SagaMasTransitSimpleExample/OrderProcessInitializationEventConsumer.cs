using MassTransit;

namespace SagaMasTransitSimpleExample;

public class OrderProcessInitializationEventConsumer : ConsumerBase<OrderProcessInitializationEvent>
{
    protected override Task ConsumeInternal(ConsumeContext<OrderProcessInitializationEvent> context)
    {
        Console.WriteLine("Order process Initialized");

        context.RespondAsync(new OrderProcessInitiazationDto
        {
            OrderId = context.Message.OrderId
        });
        
        return Task.CompletedTask;
    }
}

public class OrderProcessInitiazationDto
{
    public Guid OrderId { get; set; }
}