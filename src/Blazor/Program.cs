using System.Reactive.Concurrency;
using Blazor.Data;
using Blazor.Pages;
using MudBlazor.Services;
using ReactiveUI;
using Refit;
using Rocket.Surgery.Airframe.Data;

var builder = WebApplication.CreateBuilder(args);

RxApp.MainThreadScheduler = WasmScheduler.Default;
RxApp.TaskpoolScheduler = WasmScheduler.Default;

builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services
       .AddSingleton<WeatherForecastService>()
       .AddSingleton<IChuckNorrisJokeApiClient>(provider => RestService.For<IChuckNorrisJokeApiClient>("https://api.chucknorris.io/"))
       .AddSingleton<IChuckNorrisJokeService, ChuckNorrisJokeService>()
       .AddSingleton<IChuckNorrisJokes, ChuckNorrisJokes>()
       .AddTransient<ChuckNorrisViewModel>();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();