using Joxes;
using Joxes.Blazor;
using Joxes.Blazor.Data;
using Joxes.Blazor.Pages.Chuck;
using Joxes.Delivery;
using Joxes.Serialization;
using Microsoft.Azure.SignalR;
using MudBlazor.Services;
using Refit;
using Rocket.Surgery.Airframe.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddSignalR()
       .AddAzureSignalR(thing => thing.Endpoints = new ServiceEndpoint[]
                                                   {
                                                       new(
                                                           "Endpoint=https://punchline.service.signalr.net;AccessKey=oSN4ibHqW4MvcecjRt39kLPhWc3rqUe9fQRPaRLENzs=;Version=1.0;")
                                                   });
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services
       .AddSingleton<WeatherForecastService>()
       .AddSingleton<IChuckNorrisJokeApiClient>(
           _ => RestService.For<IChuckNorrisJokeApiClient>("https://api.chucknorris.io/"))
       .AddSingleton<IChuckNorrisApiContract>(
           _ => RestService.For<IChuckNorrisApiContract>("https://api.chucknorris.io/"))
       .AddSingleton<IChuckNorrisJokeService, ChuckNorrisJokeService>()
       .AddSingleton<IChuckNorrisJokes, ChuckNorrisJokes>()
       .AddTransient<ChuckNorrisViewModel>()
       .AddTransient<UserId>()
       .AddSingleton<IJsonSerializer, Serializer>()
       .AddTransient<IPunchlines, Punchlines>()
       .AddSingleton<IJokeBroadcast, JokeBroadcast>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapHub<JokeHub>("/jokes");
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();