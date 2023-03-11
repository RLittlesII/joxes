using Joxes;
using Joxes.Blazor.Data;
using Joxes.Blazor.Pages.Chuck;
using MudBlazor.Services;
using Refit;
using Rocket.Surgery.Airframe.Data;

var builder = WebApplication.CreateBuilder(args);

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
       .AddTransient<ChuckNorrisViewModel>()
       .AddTransient<UserId>()
       .AddTransient<IJsonSerializer, Serializer>()
       .AddHttpClient("Functions", client => client.BaseAddress = new Uri(" http://localhost:7071/api"));

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