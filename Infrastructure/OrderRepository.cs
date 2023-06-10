using Domain.Contracts;
using Domain.Entities;

namespace Infrastructure;

public class OrderRepository : IOrderRepository
{
    private readonly MyContext _context;

    public OrderRepository(MyContext context)
    {
        _context = context;
    }

    public async Task CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        Thread.Sleep(2000);
        Console.WriteLine($"The Order has been Created - OrderId: {order.Id}");
    }

    public async Task MarkOrderAsPaid(Guid orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null)
            throw new Exception("Order Not Found");

        order.Status = OrderStatus.Paid;
        await _context.SaveChangesAsync();
        Thread.Sleep(1000);
        Console.WriteLine("Order Marked As Paid");
    }

    public async Task CancelOrder(Guid orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null)
            throw new Exception("Order Not Found");

        order.Status = OrderStatus.Cancel;
        await _context.SaveChangesAsync();
        Thread.Sleep(1000);
        Console.WriteLine("Order Marked As Cancel");
    }
}

