using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace SagaMasTransitSimpleExample;

public class EventsController
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRequestClient<OrderProcessInitializationEvent> _orderProcessInitializationEventRequestClient;
    private readonly IRequestClient<CheckProductStockEvent> _checkProductStockEventRequestClient;
    private readonly IRequestClient<TakePaymentEvent> _takePaymentEventRequestClient;
    private readonly IRequestClient<CreateOrderEvent> _createOrderEventRequestClient;
        
    public EventsController(
        IPublishEndpoint publishEndpoint,
        IRequestClient<OrderProcessInitializationEvent> orderProcessInitializationEventRequestClient,
        IRequestClient<CheckProductStockEvent> checkProductStockEventRequestClient,
        IRequestClient<TakePaymentEvent> takePaymentEventRequestClient,
        IRequestClient<CreateOrderEvent> createOrderEventRequestClient)
    {
        _publishEndpoint = publishEndpoint;
        _orderProcessInitializationEventRequestClient = orderProcessInitializationEventRequestClient;
        _checkProductStockEventRequestClient = checkProductStockEventRequestClient;
        _takePaymentEventRequestClient = takePaymentEventRequestClient;
        _createOrderEventRequestClient = createOrderEventRequestClient;
    }

    [HttpPost("initialize/order")]
    public async Task<IActionResult> OrderProcessInitializedEvent([FromBody] EventCommonRequest request)
    {
        if (!request.IsRequestResponsePattern)
        {
            await _publishEndpoint.Publish<OrderProcessInitializationEvent>(new {request.OrderId});
            return new NoContentResult();
        }

        var result = await _orderProcessInitializationEventRequestClient.GetResponse<OrderProcessInitiazationDto>(new {request.OrderId});

        return new NoContentResult();
    }
}

public class CreateOrderEvent
{
}

public class TakePaymentEvent
{
}

public class CheckProductStockEvent
{
}

public class EventCommonRequest
{
    public Guid OrderId { get; set; }
    public bool IsRequestResponsePattern { get; set; }
}