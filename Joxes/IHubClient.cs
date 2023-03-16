using System.Reactive;

namespace Joxes;

/// <summary>
/// Interface representing a pushed based client connection.
/// </summary>
/// <typeparam name="T">The client object type.</typeparam>
public interface IHubClient<T>
{
    /// <summary>
    /// Connect to the Hub.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <returns>A task to monitor the progress.</returns>
    IObservable<T> Connect(string channel);

    /// <summary>
    /// Invokes a method with the provided name.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="methodName">The method name.</param>
    /// <returns>A completion value.</returns>
    IObservable<Unit> Invoke(T item, string methodName);
}