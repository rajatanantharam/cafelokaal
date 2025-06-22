using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CafeLokaal.Actors;

public class Barista
{
    private readonly ILogger<Barista> _logger;

    public Barista(ILogger<Barista> logger)
    {
        _logger = logger;
    }

    [Function(nameof(Barista))]
    public async Task Run(
        [ServiceBusTrigger("prepare-order", "cafelokaal-orders", Connection = "AzureWebJobsStorage")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Process the order, e.g., prepare the coffee
        // Here you would typically check the order details, prepare the coffee, etc.
        _logger.LogInformation("Preparing order for ID: {orderId}", message.MessageId);
        // Simulate coffee preparation delay
        await Task.Delay(2000); // Simulate a delay for coffee preparation
        _logger.LogInformation("Order prepared successfully for ID: {orderId}", message.MessageId);
        // After preparing the order, you can publish a new message to the "order-ready" topic
        _logger.LogInformation("Publishing message to order-ready topic for order ID: {orderId}", message.MessageId);  

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}