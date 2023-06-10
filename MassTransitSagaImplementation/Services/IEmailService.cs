namespace MassTransitSagaImplementation.Services;

#pragma warning disable CS8618
public interface IEmailService
{
    Task SendEmail(string customerEmail, string subject);
}