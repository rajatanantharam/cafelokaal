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
        [ServiceBusTrigger("order-ready", Connection = "AzureWebJobsStorage")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}