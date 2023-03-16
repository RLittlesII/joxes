using System.Net;
using Joxes;
using Joxes.Serialization;
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

        var serializer = executionContext
                         .InstanceServices
                         .GetService<IJsonSerializer>()!;

        var jokeRequest = DeserializrJokeRequest(serializer, requestAsString);

        if (jokeRequest == null)
        {
            return req.CreateResponse(HttpStatusCode.UnprocessableEntity);
        }

        var category =
            jokeRequest
                .Categories
                .OrderBy(_ => Random.Next())
                .ToArray()[0]
                .Value;

        logger.LogInformation(category);
        var jokeResult =
            await executionContext
                  .InstanceServices
                  .GetService<IChuckNorrisApiContract>()!
                  .RandomFromCategory(category);

        // TODO: [rlittlesii: March 11, 2023] send SignalR signal
        var jokeResponse = new JokeResponse(jokeRequest.Id, jokeRequest.UserId, jokeResult, DateTimeOffset.UtcNow);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        await response.WriteStringAsync(serializer.Serialize(jokeResponse));

        return response;
    }

    private static JokeRequest? DeserializrJokeRequest(IJsonSerializer serializer, string? requestAsString)
    {
        return serializer.Deserialize<JokeRequest>(requestAsString);
    }
    //
    // [SignalROutput(HubName = "punchline", ConnectionStringSetting = "SignalRConnection")]
    // [Function(nameof(PunchlineDeliveryFunction))]
    // public static SignalRMessageAction PunchlineDeliveryFunction(
    //     [SignalRTrigger("punchline", "norris", nameof(PunchlineDeliveryFunction))]
    //     SignalRInvocationContext context)
    // {
    //     return new SignalRMessageAction("Punchline")
    //            {
    //                // broadcast to all the connected clients without specifying any connection, user or group.
    //                Arguments = new object[]
    //                            {
    //                                new NorrisJoke()
    //                            },
    //            };
    // }

    private static readonly Random Random = new();
}