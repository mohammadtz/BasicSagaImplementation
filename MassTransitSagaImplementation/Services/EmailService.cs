namespace MassTransitSagaImplementation.Services;

public class EmailService : IEmailService
{
    public async Task SendEmail(string customerEmail, string subject)
    {
        await Task.CompletedTask;
    }
}