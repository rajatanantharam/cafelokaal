using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CafeLokaal.Actors;

public class Notifier
{
    private readonly ILogger<Notifier> _logger;

    public Notifier(ILogger<Notifier> logger)
    {
        _logger = logger;
    }

    [Function(nameof(Notifier))]
    public async Task Run(
        [ServiceBusTrigger("order-ready", "cafelokaal-orders", Connection = "AzureWebJobsStorage")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Notify the customer that their order is ready
        // Here you would typically send a notification, e.g., via email or SMS
        _logger.LogInformation("Notifying customer that order is ready for ID: {orderId}", message.MessageId);
        // Simulate notification delay
        await Task.Delay(500); // Simulate a delay for notification sending
        _logger.LogInformation("Notification sent successfully for order ID: {orderId}", message.MessageId);
        // After notifying the customer, you can publish a new message to the "order-completed" topic
        _logger.LogInformation("Publishing message to order-completed topic for order ID: {orderId}", message.MessageId);
        // Here you would typically create a new message to indicate the order is completed     
        _logger.LogInformation("Order completed for ID: {orderId}", message.MessageId);
        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}