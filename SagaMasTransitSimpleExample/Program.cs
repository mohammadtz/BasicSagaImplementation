using MassTransit;
using SagaMasTransitSimpleExample;

var builder = WebApplication.CreateBuilder(args);

var messageBrokerQueueSettings = builder.Configuration.GetSection("MessageBroker:QueueSettings").Get<MessageBrokerQueueSettings>(); 
var messageBrokerPersistenceSettings = builder.Configuration.GetSection("MessageBroker:StateMachinePersistence").Get<MessageBrokerPersistenceSettings>(); 


builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderStateMachine, OrderState>().MongoDbRepository(r =>
    {
        r.Connection = messageBrokerPersistenceSettings.Connection;
        r.DatabaseName = messageBrokerPersistenceSettings.DatabaseName;
        r.CollectionName = messageBrokerPersistenceSettings.CollectionName;
    });
            
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(messageBrokerQueueSettings.HostName, messageBrokerQueueSettings.VirtualHost, h =>
        {
            h.Username(messageBrokerQueueSettings.UserName);
            h.Password(messageBrokerQueueSettings.Password);
        });
                
        cfg.ConfigureEndpoints(context);
    });

    x.AddConsumer<OrderProcessInitializationEventConsumer>();
    x.AddRequestClient<OrderProcessInitializationEventConsumer>();
    
    x.AddConsumer<OrderProcessInitializationFaultEventConsumer>();
    x.AddRequestClient<OrderProcessInitializationFaultEventConsumer>();
    
    x.AddConsumer<CheckProductStockEventConsumer>();
    x.AddRequestClient<CheckProductStockEventConsumer>();
    
    x.AddConsumer<CheckProductStockFaultEventConsumer>();
    x.AddRequestClient<CheckProductStockFaultEventConsumer>();
    x.AddConsumer<TakePaymentEventConsumer>();
    x.AddRequestClient<TakePaymentEventConsumer>();
    
    x.AddConsumer<TakePaymentFaultEventConsumer>();
    x.AddRequestClient<TakePaymentFaultEventConsumer>();
    
    x.AddConsumer<CreateOrderEventConsumer>();
    x.AddRequestClient<CreateOrderEventConsumer>();
    
    x.AddConsumer<CreateOrderFaultEventConsumer>();
    x.AddRequestClient<CreateOrderFaultEventConsumer>();
    
    x.AddConsumer<OrderProcessFailedEventConsumer>();
    x.AddRequestClient<OrderProcessFailedEventConsumer>();
});


var app = builder.Build();


app.Run();