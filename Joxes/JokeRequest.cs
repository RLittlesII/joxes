using System.Runtime.Serialization;
using LanguageExt;

namespace Joxes;

public class JokeRequest : Record<JokeRequest>
{
    public JokeRequest() : this(new UserId(), Enumerable.Empty<Category>()){}
    public JokeRequest(UserId userId, IEnumerable<Category> categories)
        : this(new CorrelationId(), userId, categories) { }

    public JokeRequest(CorrelationId id, UserId userId, IEnumerable<Category> categories)
    {
        Id = id;
        UserId = userId;
        Categories = categories;
    }

    public JokeRequest(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    public CorrelationId Id;

    public UserId UserId;

    public IEnumerable<Category> Categories;
}