using System.Reactive.Linq;
using DynamicData;

namespace Joxes;

public static class CacheFunctions
{
    /// <summary>
    /// Caches the list of <see cref="TValue"/>.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="cache">The cache.</param>
    /// <param name="clearCache">A value determining whether to clear the cache.</param>
    /// <returns>A completion notification.</returns>
    public static IObservable<IChangeSet<TValue, TKey>> Cache<TKey, TValue>(this IObservable<IEnumerable<TValue>> result,
                                                                            SourceCache<TValue, TKey> cache,
                                                                            bool clearCache)
        where TKey : notnull => result
                             .Do(x => UpdateCache(cache, clearCache)(x))
                             .Select(_ => cache.Connect()
                                               .RefCount())
                             .Switch();

    /// <summary>
    /// Caches the list of <see cref="TValue"/>.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="cache">The cache.</param>
    /// <returns>A completion notification.</returns>
    public static IObservable<TValue> Cache<TKey, TValue>(this IObservable<TValue> result, SourceCache<TValue, TKey> cache)
        where TKey : notnull => result
        .Do(x => Update(x, cache));

    private static void Update<TKey, TValue>(TValue item, ISourceCache<TValue, TKey> cache)
        where TKey : notnull => cache.AddOrUpdate(item);

    private static Action<IEnumerable<TValue>> UpdateCache<TKey, TValue>(ISourceCache<TValue, TKey> cache, bool clearCache)
        where TKey : notnull => results =>
        {
            if (clearCache)
            {
                cache
                    .Edit(updater =>
                    {
                        updater.Clear();
                        updater.AddOrUpdate(results);
                    });
            }
            else
            {
                cache.EditDiff(results, (first, second) => first != null && first.Equals(second));
            }
        };
}