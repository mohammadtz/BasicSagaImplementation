using Domain.Contracts;
using MediatR;

namespace BasicSagaImplementation.Services;

public class MessageBus : IMessageBus
{
    private readonly IPublisher _publisher;

    public MessageBus(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task Send(IEvent sendData, CancellationToken cancellation = default)
    {
        await _publisher.Publish(sendData, cancellation);
    }
}
