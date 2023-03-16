using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using ReactiveMarbles.Extensions;
using ReactiveMarbles.Mvvm;
using ReactiveMarbles.PropertyChanged;

namespace Joxes.Maui.Features.Discriminator;

public class DiscriminatorViewModel : ViewModelBase
{
    public DiscriminatorViewModel(INavigationService navigationService,
                                  ICoreRegistration coreRegistration,
                                  IChuckNorrisAggregator aggregator)
        : base(navigationService, coreRegistration)
    {
        var filterChanged =
            this.WhenChanged(x => x.SelectedCategory)
                .Select(category => FilterCategory(category));

        aggregator
            .Jokes()
            .Filter(filterChanged)
            .Bind(out _jokes)
            .Subscribe(_ => { })
            .DisposeWith(CollectMe);

        Categories =
            new ReadOnlyObservableCollection<string>(new ObservableCollection<string>(
                                                         new Categories().Select(x => x.Value)
                                                                         .ToList()));

        static Func<Joke, bool> FilterCategory(string selected) => category =>
        {
            if (!string.IsNullOrEmpty(selected))
            {
                return category.Category.Value == selected;
            }

            return true;
        };
    }

    public string SelectedCategory
    {
        get => _category;
        set => this.RaiseAndSetIfChanged(ref _category, value);
    }

    public ReadOnlyObservableCollection<string> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value);
    }

    public ReadOnlyObservableCollection<Joke> Jokes => _jokes;

    private string _category;
    private ReadOnlyObservableCollection<Joke> _jokes;
    private ReadOnlyObservableCollection<string> _categories;
}