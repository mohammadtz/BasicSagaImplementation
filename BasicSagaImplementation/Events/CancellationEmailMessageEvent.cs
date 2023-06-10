using Domain.Contracts;

namespace BasicSagaImplementation.Events;

public class CancellationEmailMessageEvent : IEvent
{
    public string? To { get; }
    public string Subject { get; }

    public CancellationEmailMessageEvent(string? to, string subject)
    {
        To = to;
        Subject = subject;
    }
}

public class CancellationEmailMessageEventHandler : IEventHandler<CancellationEmailMessageEvent>
{
    public async Task Handle(CancellationEmailMessageEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await Task.CompletedTask;
            Console.WriteLine("The cancellation email has been Sent to the customer");
        }
        catch (Exception e)
        {
            Console.WriteLine("The cancellation email Sent to the customer failed");
        }
        finally
        {
            Thread.Sleep(1000);
        }
    }
}