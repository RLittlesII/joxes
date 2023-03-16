using System.Reactive.Disposables;
using DynamicData.Binding;

namespace Joxes.Maui.Features.Group;

public abstract class GroupingViewModelBase<T> : ObservableCollectionExtended<T>, IDisposable
{
    protected CompositeDisposable CollectMe { get; } = new CompositeDisposable();

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the resources.
    /// </summary>
    /// <param name="disposing">Disposing.</param>
    protected virtual void Dispose(bool disposing) => CollectMe.Dispose();
}