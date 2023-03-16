using Microsoft.AspNetCore.SignalR;

namespace Joxes.Blazor;

public class JokeHub : Hub
{
    public Task DeliverPunchline(JokeResponse response) => Clients.All.SendAsync("norris", response);
}