using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    [Function("Notifier")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}