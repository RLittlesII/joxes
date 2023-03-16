using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Joxes.Blazor;

public class JokeBroadcast : IJokeBroadcast
{
    public JokeBroadcast(IHubContext<JokeHub> context)
    {
        _context = context;
    }

    public IObservable<Unit> Broadcast(JokeResponse item) =>
        _context.Clients
                .All
                // NOTE: [rlittlesii: March 15, 2023] Beware the ides of March!
                // TODO: [rlittlesii: March 15, 2023] allow consumer to send the method
                .SendAsync("DeliverPunchline", item)
                .ToObservable()
                .Do(_ => { });

    private readonly IHubContext<JokeHub> _context;
}