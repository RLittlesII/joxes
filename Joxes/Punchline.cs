using System.Runtime.Serialization;
using LanguageExt;
using LanguageExt.ClassInstances.Const;
using LanguageExt.ClassInstances.Pred;

namespace Joxes;

public class Punchline : NewType<Punchline, string, StrLen<I1, I5000>>
{
    public Punchline(string value)
        : base(value) { }

    public Punchline(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}