using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Joxes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Punchline;

public class DeliveryFunction
{
    private readonly ILogger _logger;

    public DeliveryFunction(ILoggerFactory logger) => _logger = logger.CreateLogger<DeliveryFunction>();

    [Function(nameof(DeliveryFunction))]
    public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger<DeliveryFunction>();

        var requestAsString = await req.ReadAsStringAsync();

        logger.LogInformation(requestAsString);

        var requestBody = executionContext
                          .InstanceServices
                          .GetService<IJsonSerializer>()
                          .Deserialize<JokeRequest>(requestAsString);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        await response.WriteStringAsync("Welcome to Azure Functions!");

        return response;
    }
}