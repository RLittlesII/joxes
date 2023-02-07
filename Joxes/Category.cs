using System.Runtime.Serialization;
using LanguageExt;

namespace Joxes;

public class Category : NewType<Category, string>
{
    public Category(string value)
        : base(value) { }

    public Category(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}