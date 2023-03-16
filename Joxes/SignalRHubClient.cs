using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using ReactiveMarbles.Extensions;

namespace Joxes;

public class SignalRHubClient<T> : IHubClient<T>
{
    public SignalRHubClient(IHubConnectionBuilder connectionBuilder) => _connection = connectionBuilder.Build();

    IObservable<T> IHubClient<T>.Connect(string channel) => Observable.Create<T>(async (observer, cancellation) =>
    {
        var reader = await _connection.StreamAsChannelAsync<T>(channel, cancellation)
                                      .ConfigureAwait(false);
        while (!cancellation.IsCancellationRequested
            && await reader.WaitToReadAsync(cancellation)
                           .ConfigureAwait(false))
        {
            while (reader.TryRead(out var item))
            {
                observer.OnNext(item);
            }
        }

        await reader.Completion.ConfigureAwait(false);
        return Disposable.Empty;
    });

    IObservable<Unit> IHubClient<T>.Invoke(T item, string methodName) =>
        Observable.FromAsync(token => _connection.InvokeAsync<T>(methodName, item, cancellationToken: token))
                  .AsSignal();

    private readonly HubConnection _connection;
}