using Domain.Base;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public string? CustomerEmail { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}

public enum OrderStatus
{
    Pending = 0,
    Paid = 1,
    Cancel = 2
}