using MassTransit;
using MassTransitSagaImplementation.Events;
using MassTransitSagaImplementation.Services;

namespace MassTransitSagaImplementation.Commands;

#pragma warning disable CS8618

public class SendCancellationEmailCommand
{
    public Guid OrderId { get; set; }
    public string? CustomerEmail { get; set; }
}

public class SendCancellationEmailCommandHandler : IConsumer<SendCancellationEmailCommand>
{
    private readonly IEmailService _emailService;

    public SendCancellationEmailCommandHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<SendCancellationEmailCommand> context)
    {
        await _emailService.SendEmail(context.Message.CustomerEmail ?? string.Empty, "Your order has been cancelled");

        await context.Publish(new CancellationEmailSentEvent { OrderId = context.Message.OrderId });
    }
}