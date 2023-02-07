using System.Collections;

namespace Joxes;

public class Categories : IReadOnlyList<Category>
{
    public IEnumerator<Category> GetEnumerator() => _categories.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => _categories.Count;

    public Category this[int index] => _categories[index];

    private readonly List<Category> _categories = new List<Category>();
}