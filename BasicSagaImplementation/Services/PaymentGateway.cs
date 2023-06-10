using Domain.Contracts;
using Domain.Entities;

namespace BasicSagaImplementation.Services;

public class PaymentGateway : IPaymentGateway
{
    public async Task<Payment> ProcessPayment(Order order)
    {
        await Task.CompletedTask;
        Thread.Sleep(3000);
        return new Payment
        {
            Id = Guid.NewGuid(),
            Status = PaymentStatus.Failed,
            OrderId = order.Id
        };
    }
}
