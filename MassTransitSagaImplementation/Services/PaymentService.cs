using Domain.Entities;
using Infrastructure;

namespace MassTransitSagaImplementation.Services;

public class PaymentService : IPaymentService
{
    private readonly MyContext _context;

    public PaymentService(MyContext context)
    {
        _context = context;
    }

    public async Task<Payment> ProcessPayment(Guid orderId)
    {
        var payment = new Payment
        {
            Status = PaymentStatus.Successful,
            Id = Guid.NewGuid(),
            OrderId = orderId
        };

        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        return payment;
    }
}