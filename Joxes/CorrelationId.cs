using System.Runtime.Serialization;
using LanguageExt;
using LanguageExt.ClassInstances.Const;
using LanguageExt.ClassInstances.Pred;

namespace Joxes;

public class CorrelationId : NewType<CorrelationId, string, StrLen<I1, I50>>
{
    public CorrelationId()
        : this(Guid.NewGuid()
                   .ToString()) { }

    public CorrelationId(string value)
        : base(value) { }

    public CorrelationId(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}