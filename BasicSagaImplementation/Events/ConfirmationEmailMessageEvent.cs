using Domain.Contracts;

namespace BasicSagaImplementation.Events;

public class ConfirmationEmailMessageEvent : IEvent
{
    public string? To { get; }
    public string Subject { get; }

    public ConfirmationEmailMessageEvent(string? to, string subject)
    {
        To = to;
        Subject = subject;
    }
}

public class ConfirmationEmailMessageEventHandler : IEventHandler<ConfirmationEmailMessageEvent>
{
    public async Task Handle(ConfirmationEmailMessageEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await Task.CompletedTask;
            Console.WriteLine("The confirmation email has been Sent to the customer");
        }
        catch (Exception e)
        {
            Console.WriteLine("The confirmation email Sent to the customer failed");
        }
        finally
        {
            Thread.Sleep(1000);
        }
    }
}