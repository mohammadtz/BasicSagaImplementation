using Domain.Base;

namespace Domain.Entities;

public class Payment : BaseEntity
{
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public Guid OrderId { get; set; }
}

public enum PaymentStatus
{
    Pending = 0,
    Successful = 1,
    Failed = 2,
}