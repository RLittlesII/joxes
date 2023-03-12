using System.Net;
using System.Reactive.Threading.Tasks;
using Joxes;
using Joxes.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Airframe.Data;

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

        var requestBody = serializer.Deserialize<JokeRequest>(requestAsString);

        if (requestBody == null)
        {
            return req.CreateResponse(HttpStatusCode.UnprocessableEntity);
        }

        var category =
            requestBody
                .Categories
                .OrderBy(_ => _random.Next())
                .ToArray()[0]
                .Value;

        logger.LogInformation(category);
        var jokeResult =
            await executionContext
                  .InstanceServices
                  .GetService<IChuckNorrisApiContract>()
                  .RandomFromCategory(category);

        // TODO: [rlittlesii: March 11, 2023] send SignalR signal
        var jokeResponse = new JokeResponse(requestBody.Id, requestBody.UserId, jokeResult, DateTimeOffset.UtcNow);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        await response.WriteStringAsync(serializer.Serialize(jokeResponse));

        return response;
    }

    private readonly static Random _random = new Random();
}