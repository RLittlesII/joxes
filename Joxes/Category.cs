using System.Runtime.Serialization;
using LanguageExt;
using LanguageExt.ClassInstances.Const;
using LanguageExt.ClassInstances.Pred;

namespace Joxes;

public class Category : NewType<Category, string, StrLen<I0, I50>>
{
    public Category() : this(string.Empty){}
    public Category(string value)
        : base(value) { }

    public Category(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}