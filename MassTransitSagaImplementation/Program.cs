using Domain.Contracts;
using Domain.Entities;
using Infrastructure;
using MassTransit;
using MassTransitSagaImplementation;
using MassTransitSagaImplementation.Commands;
using MassTransitSagaImplementation.Events;
using MassTransitSagaImplementation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder();

//builder.Services.AddScoped<IEntityChangeTracker, DbEntityChangeTracker>();

#region Services

builder.Services.AddDbContext<MyContext>(options =>
{
    options.EnableSensitiveDataLogging();
    options.UseSqlServer("Server=.;Database=SagaPatternDb;User Id=sa;Password=Mt123456;TrustServerCertificate=True");
});

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));

//builder.Services.AddScoped<MyContext>();
//builder.Services.AddScoped<Func<MyContext>>(serviceProvider => serviceProvider.GetRequiredService<MyContext>);

builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(Program).Assembly);
    // A Transport
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("rabbitmq://localhost/"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("order-processing-saga", e =>
        {
            e.StateMachineSaga(new OrderProcessingSaga(), new InMemorySagaRepository<OrderProcessingSagaState>());

            //e.ConfigureConsumers(context);
            e.Consumer<ProcessPaymentCommandHandler>(context);
            e.Consumer<SendConfirmationEmailCommandHandler>(context);
            e.Consumer<SendCancellationEmailCommandHandler>(context);
        });
    });
});

#endregion


var app = builder.Build();

app.MapGet("/order", async ([FromServices] IBus bus, [FromServices] MyContext context) =>
{
    try
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            Status = OrderStatus.Pending,
            CustomerEmail = "test@test.com"
        };

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        await bus.Publish(new OrderCreatedEvent { Order = order });

        return JsonConvert.SerializeObject(order);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return e.Message;
    }
});

app.MapGet("/payment", async ([FromServices] IBus bus, [FromQuery] string orderId) =>
{
    try
    {
        await bus.Publish(new ProcessPaymentCommand { OrderId = Guid.Parse(orderId) });

        return "payment create successfully";
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return e.Message;
    }
});

app.Run();