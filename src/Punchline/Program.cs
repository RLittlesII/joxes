using Joxes.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Punchline;
using Refit;

var host = new HostBuilder()
           .ConfigureFunctionsWorkerDefaults()
           .ConfigureServices((context, collection) =>
                                  collection.AddTransient<IJsonSerializer, Serializer>()
                                            .AddSingleton<IChuckNorrisApiContract>(
                                                provider => RestService.For<IChuckNorrisApiContract>("https://api.chucknorris.io/")))
           .Build();

host.Run();