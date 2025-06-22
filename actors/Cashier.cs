using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CafeLokaal.Actors;

public class Cashier
{
    private readonly ILogger<Cashier> _logger;

    public Cashier(ILogger<Cashier> logger)
    {
        _logger = logger;
    }

    [Function(nameof(Cashier))]
    public async Task Run(
        [ServiceBusTrigger("new-order", "cafelokaal-orders", Connection = "AzureWebJobsStorage")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Process the order, e.g., handle payment
        // Here you would typically check the payment status, process the payment, etc.
        // For demonstration, let's assume the payment is processed successfully
        _logger.LogInformation("Processing payment for order ID: {orderId}", message.MessageId);
        // Simulate payment processing delay
        await Task.Delay(1000); // Simulate a delay for payment processing  
        _logger.LogInformation("Payment processed successfully for order ID: {orderId}", message.MessageId);
        // After processing the payment, you can publish a new message to the "prepare-order" topic
        _logger.LogInformation("Publishing message to prepare-order topic for order ID: {orderId}", message.MessageId);
        // Here you would typically create a new message for the Barista to prepare the order
        await messageActions.CompleteMessageAsync(message);
    }
}