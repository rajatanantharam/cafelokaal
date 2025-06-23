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
        [ServiceBusTrigger("order-placed", Connection = "AzureWebJobsStorage")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Simulate some processing delay      
        await Task.Delay(1000);

        // Publish a message to the order-ready queue
        var orderReadyMessage = new ServiceBusMessage(message.Body)
        {
            ContentType = message.ContentType,
            MessageId = message.MessageId
        };

        
        // Send the message to the "order-ready" queue
        var serviceBusClient = new ServiceBusClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
        var sender = serviceBusClient.CreateSender(Constants.ServiceBusQueueNameOrderReady);
        await sender.SendMessageAsync(orderReadyMessage);  
        _logger.LogInformation("Order ready message sent with ID: {id}", orderReadyMessage.MessageId); 
        // Close the sender
        await sender.DisposeAsync();
        await serviceBusClient.DisposeAsync();
    
        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}