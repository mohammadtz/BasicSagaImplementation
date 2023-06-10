using Domain.Entities;

namespace Domain.Contracts;

public interface IOrderRepository
{
    Task CreateOrder(Order order);
    Task MarkOrderAsPaid(Guid orderId);
    Task CancelOrder(Guid orderId);
}