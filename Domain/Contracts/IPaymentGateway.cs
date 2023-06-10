
using Domain.Entities;

namespace Domain.Contracts;

public interface IPaymentGateway
{
    Task<Payment> ProcessPayment(Order order);
}
