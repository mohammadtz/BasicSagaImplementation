using BasicSagaImplementation.Events;
using BasicSagaImplementation.Exceptions;
using Domain.Contracts;
using Domain.Entities;

namespace BasicSagaImplementation;

public class SimpleOrderProcessingSaga
{
    private readonly IMessageBus _messageBus;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentGateway _paymentGateway;

    public SimpleOrderProcessingSaga(IMessageBus messageBus, IOrderRepository orderRepository, IPaymentGateway paymentGateway)
    {
        _messageBus = messageBus;
        _orderRepository = orderRepository;
        _paymentGateway = paymentGateway;
    }

    public async Task ProcessOrder(Order order)
    {
        try
        {
            // Step 1: Create the order
            await _orderRepository.CreateOrder(order);

            // Step 2: Process the payment
            var paymentResult = await _paymentGateway.ProcessPayment(order);

            if (paymentResult.Status == PaymentStatus.Successful)
            {
                Console.WriteLine("Payment Successful");
                // Step 3: Mark the order as paid
                await _orderRepository.MarkOrderAsPaid(order.Id);

                // Step 4: Send a confirmation email to the customer
                await _messageBus.Send(new ConfirmationEmailMessageEvent(order.CustomerEmail, "Your order has been processed successfully"));
            }
            else
            {
                Console.WriteLine("Payment Failed");
                // Step 3 (compensating action): Cancel the order
                await _orderRepository.CancelOrder(order.Id);

                // Step 4 (compensating action): Send a cancellation email to the customer
                await _messageBus.Send(new CancellationEmailMessageEvent(order.CustomerEmail, "Your order has been cancelled due to payment failure"));
            }
        }
        catch (Exception ex)
        {
            // Step 3 (compensating action): Cancel the order
            await _orderRepository.CancelOrder(order.Id);
            Console.WriteLine("Order Canceled");

            // Step 4 (compensating action): Send a cancellation email to the customer
            await _messageBus.Send(new CancellationEmailMessageEvent(order.CustomerEmail, "Your order has been cancelled due to an error"));

            throw new SagaException("Order processing failed", ex);
        }
    }
}