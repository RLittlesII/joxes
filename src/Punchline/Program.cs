using Joxes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
           .ConfigureFunctionsWorkerDefaults()
           .ConfigureServices((context, collection) => collection.AddTransient<IJsonSerializer, Serializer>())
           .Build();

host.Run();