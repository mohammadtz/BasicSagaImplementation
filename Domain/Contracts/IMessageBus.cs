namespace Domain.Contracts;

public interface IMessageBus
{
    Task Send(IEvent sendData, CancellationToken cancellation = default);
}