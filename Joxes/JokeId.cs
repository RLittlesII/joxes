using System.Runtime.Serialization;
using LanguageExt;

namespace Joxes;

public class JokeId : NewType<JokeId, string>
{
    public JokeId(string value)
        : base(value) { }

    public JokeId(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}