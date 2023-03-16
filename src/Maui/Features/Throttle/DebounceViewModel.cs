using System.Collections.ObjectModel;
using DynamicData;
using ReactiveMarbles.Extensions;
using ReactiveMarbles.Mvvm;

namespace Joxes.Maui.Features.Throttle;

public class DebounceViewModel : ViewModelBase
{
    public DebounceViewModel(INavigationService navigationService, ICoreRegistration coreRegistration, IChuckNorrisAggregator aggregator)
        : base(navigationService, coreRegistration)
    {
        aggregator
            .Jokes()
            .Batch(TimeSpan.FromSeconds(3))
            .Bind(out _jokes)
            .Subscribe(_ => { })
            .DisposeWith(CollectMe);
    }

    public ReadOnlyObservableCollection<Joke> Jokes => _jokes;

    private ReadOnlyObservableCollection<Joke> _jokes;
}