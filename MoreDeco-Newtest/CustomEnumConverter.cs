using System;
using System.Reflection;
using Newtonsoft.Json;
using Nautilus.Handlers;
using Nautilus.Extensions;

namespace CustomItems;

public class CustomEnumConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsEnum; // This should apply to all enums, including modded ones
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        // If you're not planning to serialize this enum, just throw the NotImplementedException
        // or implement the logic to handle serialization of enums here
        if (value == null)
        {
            writer.WriteNull();
        }
        else
        {
            writer.WriteValue(value.ToString()); // Simple serialization to string
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            string enumString = (string)reader.Value;

            // Check for modded enums or custom TechType handling
            if (EnumHandler.ModdedEnumExists<TechType>(enumString))
            {
                if (TechTypeExtensions.FromString(enumString, out TechType result, true))
                {
                    return result;
                }
            }

            // If it's a standard enum or we can't find the modded one, try parsing
            if (TryParseEnum(objectType, enumString, out var enumValue))
            {
                return enumValue;
            }

            // If no valid enum found, log error and throw exception to prevent recursion
            throw new JsonSerializationException($"Unable to parse the enum value '{enumString}' for type {objectType.Name}. Make sure the value is valid.");
        }

        // Default case when token type isn't a string
        return ReadJson(reader, objectType, existingValue, serializer);
    }

    // Use reflection to dynamically call Enum.TryParse
    private static readonly MethodInfo TryParseMethodInfo = typeof(Enum).GetMethod("TryParse", new[] { typeof(string), typeof(object).MakeByRefType() });

    public static bool TryParseEnum(Type enumType, string value, out object result)
    {
        if (enumType.IsEnum)
        {
            // Use reflection to invoke TryParse dynamically
            var parameters = new object[] { value, Activator.CreateInstance(enumType) };
            bool success = (bool)TryParseMethodInfo.Invoke(null, parameters);
            result = parameters[1];
            return success;
        }

        result = null;
        return false;
    }
}
