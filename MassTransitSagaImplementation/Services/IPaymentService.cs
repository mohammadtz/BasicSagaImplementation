using Domain.Entities;

namespace MassTransitSagaImplementation.Services;

public interface IPaymentService
{
    Task<Payment> ProcessPayment(Guid orderId);
}