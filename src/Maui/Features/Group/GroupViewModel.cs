using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveMarbles.Mvvm;
using ReactiveUI;

namespace Joxes.Maui.Features.Group;

public class GroupViewModel : ViewModelBase
{
    public GroupViewModel(INavigationService navigationService,
                          ICoreRegistration coreRegistration,
                          IChuckNorrisAggregator aggregator)
        : base(navigationService, coreRegistration)
    {
        var aggregate =
            aggregator
                .Jokes()
                .Group(x => x.JokeId)
                .Transform(group => new JokeAggregateViewModel(group))
                .DisposeMany()
                .RefCount();

        aggregate
            .Sort(SortExpressionComparer<JokeAggregateViewModel>.Ascending(thing => thing.Id))
            .ObserveOn(coreRegistration.MainThreadScheduler)
            .Bind(out _jokes)
            .Subscribe(_ => { })
            .DisposeWith(CollectMe);

        aggregate
            .Select(x => x.Count > 0 && x.HasCountChanged())
            .DistinctUntilChanged()
            .ObserveOn(coreRegistration.MainThreadScheduler)
            .BindTo(this, model => model.IsGroupable);
    }

    public bool IsGroupable
    {
        get => _isGroupable;
        set => RaiseAndSetIfChanged(ref _isGroupable, value);
    }

    public ReadOnlyObservableCollection<JokeAggregateViewModel> Jokes => _jokes;

    private ReadOnlyObservableCollection<JokeAggregateViewModel> _jokes;
    private bool _isGroupable;
}