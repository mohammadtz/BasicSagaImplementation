using MassTransit;
using MassTransitSagaImplementation.Events;
using MassTransitSagaImplementation.Services;

namespace MassTransitSagaImplementation.Commands;

#pragma warning disable CS8618

public class SendConfirmationEmailCommand
{
    public Guid OrderId { get; set; }
    public string? CustomerEmail { get; set; }
}

public class SendConfirmationEmailCommandHandler : IConsumer<SendConfirmationEmailCommand>
{
    private readonly IEmailService _emailService;
    private readonly IBus _bus;

    public SendConfirmationEmailCommandHandler(IEmailService emailService, IBus bus)
    {
        _emailService = emailService;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<SendConfirmationEmailCommand> context)
    {
        await _emailService.SendEmail(context.Message.CustomerEmail ?? string.Empty, "Your order has been processed successfully");

        await context.Publish(new ConfirmationEmailSentEvent { OrderId = context.Message.OrderId });
    }
}