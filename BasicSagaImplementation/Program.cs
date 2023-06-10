using BasicSagaImplementation;
using BasicSagaImplementation.Services;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddDbContext<MyContext>(options =>
    {
        options.UseInMemoryDatabase("MyDB");
    })
    .AddMediatR(x=> x.RegisterServicesFromAssembly(typeof(Program).Assembly))
    .AddTransient<IMessageBus, MessageBus>()
    .AddTransient<IOrderRepository, OrderRepository>()
    .AddTransient<IPaymentGateway, PaymentGateway>()
    .BuildServiceProvider();


var orderProcessing = new SimpleOrderProcessingSaga(
    serviceProvider.GetService<IMessageBus>()!, 
    serviceProvider.GetService<IOrderRepository>()!, 
    serviceProvider.GetService<IPaymentGateway>()!);

var order = new Order
{
    Id = Guid.NewGuid(),
    CustomerEmail = "test@test.com",
};

await orderProcessing.ProcessOrder(order);

Console.WriteLine("Done");