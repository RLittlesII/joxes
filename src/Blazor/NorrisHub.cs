using System.Reactive;
using System.Reactive.Threading.Tasks;
using Joxes.Serialization;
using Microsoft.AspNetCore.SignalR;

namespace Joxes.Blazor;

public class JokeHub : Hub
{
}

// public class JokeHub : IJokeBroadcast
// {
//     public JokeHub(IJsonSerializer serializer, IHubContext context)
//     {
//         _serializer = serializer;
//         _context = context;
//     }
//
//     public IObservable<Unit> Broadcast(JokeResponse item) =>
//         _context.Clients
//                 .All
//                 // NOTE: [rlittlesii: March 15, 2023] Beware the ides of March!
//                 // TODO: [rlittlesii: March 15, 2023] allow consumer to send the method
//                 .SendAsync("norris", _serializer.Serialize(item))
//                 .ToObservable();
//
//     private readonly IJsonSerializer _serializer;
//     private readonly IHubContext _context;
// }

public class JokeBroadcast : IJokeBroadcast
{
    public JokeBroadcast(IJsonSerializer serializer, IHubContext<JokeHub> context)
    {
        _serializer = serializer;
        _context = context;
    }

    public IObservable<Unit> Broadcast(JokeResponse item) =>
        _context.Clients
                .All
                // NOTE: [rlittlesii: March 15, 2023] Beware the ides of March!
                // TODO: [rlittlesii: March 15, 2023] allow consumer to send the method
                .SendAsync("norris", _serializer.Serialize(item))
                .ToObservable();

    private readonly IJsonSerializer _serializer;
    private readonly IHubContext<JokeHub> _context;
}
public interface ISignalRBroadcast<T>
{
    IObservable<Unit> Broadcast(T item);
}