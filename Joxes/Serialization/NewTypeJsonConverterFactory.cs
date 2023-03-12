using System.Text.Json;
using System.Text.Json.Serialization;
using LanguageExt;
using LanguageExt.TypeClasses;

namespace Joxes.Serialization;

public class NewTypeJsonConverterFactory : JsonConverterFactory
{
    public class NewTypeConverter<NEWTYPE, PRED, ORD> : JsonConverter<NEWTYPE>
        where NEWTYPE : NewType<NEWTYPE, string, PRED, ORD>
        where PRED : struct, Pred<string>
        where ORD : struct, Ord<string>
    {
        public override NEWTYPE Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            return (NEWTYPE) Activator.CreateInstance(typeof(NEWTYPE), value);
        }

        public override void Write(Utf8JsonWriter writer, NEWTYPE value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }

    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert.BaseType is { IsGenericType: true }
         && (typeof(NewType<,>).IsAssignableFrom(typeToConvert.BaseType.GetGenericTypeDefinition())
          || typeof(NewType<,,>).IsAssignableFrom(typeToConvert.BaseType.GetGenericTypeDefinition())
          || typeof(NewType<,,,>).IsAssignableFrom(typeToConvert.BaseType.GetGenericTypeDefinition())))
        {
            return typeToConvert.BaseType.GetGenericArguments()[1] == typeof(string);
        }

        return false;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var baseType = typeToConvert.BaseType;
        while (baseType != null && typeof(NewType<,,,>) == baseType.GetGenericTypeDefinition())
        {
            baseType = baseType.BaseType;
        }

        var genericArguments = baseType!.BaseType!.GetGenericArguments();
        return (JsonConverter) Activator
            .CreateInstance(typeof(NewTypeConverter<,,>)
                                .MakeGenericType(typeToConvert,
                                                 genericArguments[2],
                                                 genericArguments[3]));
    }
}