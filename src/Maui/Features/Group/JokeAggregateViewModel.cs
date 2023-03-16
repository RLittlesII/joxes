using System.Reactive.Disposables;
using DynamicData;
using DynamicData.Binding;

namespace Joxes.Maui.Features.Group;

public class JokeAggregateViewModel : GroupingViewModelBase<Joke>
{
    public JokeAggregateViewModel(IGroup<Joke, CorrelationId, JokeId> grouping)
    {
        Id = grouping.Key.Value;

        grouping
            .Cache
            .Connect()
            .DisposeMany()
            .Bind(this, new ObservableCollectionAdaptor<Joke, CorrelationId>(2000))
            .Subscribe()
            .DisposeWith(CollectMe);
    }

    public string Id { get; set; }
}