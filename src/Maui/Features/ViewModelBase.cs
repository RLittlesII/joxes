using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveMarbles.Mvvm;

namespace Joxes.Maui.Features;

public abstract class ViewModelBase : RxDisposableObject, INavigatedAware
{
    protected ViewModelBase(INavigationService navigationService, ICoreRegistration coreRegistration)
    {
        NavigationService = navigationService;
        MainThread = coreRegistration.MainThreadScheduler;
        BackgroundThread = coreRegistration.TaskPoolScheduler;

        _navigatedFrom = new Subject<INavigationParameters>().DisposeWith(CollectMe);
        _navigatedTo = new Subject<INavigationParameters>().DisposeWith(CollectMe);

        NavigatedFrom =
            _navigatedFrom
                .AsObservable()
                .Publish()
                .RefCount();

        NavigatedTo =
            _navigatedTo
                .AsObservable()
                .Publish()
                .RefCount();
    }

    protected INavigationService NavigationService { get; }

    protected IScheduler MainThread { get; }

    protected IScheduler BackgroundThread { get; }

    protected IObservable<INavigationParameters> NavigatedFrom { get; }

    protected IObservable<INavigationParameters> NavigatedTo { get; }

    protected readonly CompositeDisposable CollectMe = new();

    protected override void Dispose(bool disposing) => CollectMe.Dispose();

    void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters) => _navigatedFrom.OnNext(parameters);
    void INavigatedAware.OnNavigatedTo(INavigationParameters parameters) => _navigatedTo.OnNext(parameters);

    private readonly Subject<INavigationParameters> _navigatedTo;
    private readonly Subject<INavigationParameters> _navigatedFrom;
}