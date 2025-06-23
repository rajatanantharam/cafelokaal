using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    [Function("Cashier")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Simulate some processing delay
        System.Threading.Thread.Sleep(1000);

        // Send a message to the "order-placed" queue
        var orderPlacedMessage = new   
        {
            OrderId = Guid.NewGuid(),
            CustomerName = "John Doe",
            Items = new[] { "Coffee", "Croissant" },
            TotalAmount = 5.50
        };
        var serviceBusClient = new ServiceBusClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
        var sender = serviceBusClient.CreateSender("order-placed");
        var message = new ServiceBusMessage(JsonSerializer.Serialize(orderPlacedMessage))
        {
            ContentType = "application/json",
            MessageId = orderPlacedMessage.OrderId.ToString()
        };
        await sender.SendMessageAsync(message);
        _logger.LogInformation("Order placed with ID: {id}", orderPlacedMessage.OrderId);   
            
        // Close the sender
        await sender.DisposeAsync(); 
        await serviceBusClient.DisposeAsync();
        // Log the order details
        _logger.LogInformation("Order details: {orderDetails}", orderPlacedMessage);
    
        // Return a response

        _logger.LogInformation("Order placed successfully.");   
        return new OkObjectResult("Cashier!");
    }
}