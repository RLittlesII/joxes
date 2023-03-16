using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.AspNetCore.SignalR.Client;

namespace Joxes;

public class JokeReceiver : IJokeReceiver
{
    public JokeReceiver(IHubConnectionBuilder hubConnectionBuilder)
    {
        _hubSub = new Subject<JokeResponse>();
        var hubConnection = hubConnectionBuilder.Build();
        hubConnection
            .On<JokeResponse>("norris", response => _hubSub.OnNext(response));

        Observable.FromAsync(cancellation => hubConnection.StartAsync(cancellation))
                  .Subscribe();
    }

    public IObservable<JokeResponse> Listen() =>
        Observable.Create<JokeResponse>(observer => _hubSub
                                                    .AsObservable()
                                                    .Subscribe(observer));

    private readonly ISubject<JokeResponse> _hubSub;
}