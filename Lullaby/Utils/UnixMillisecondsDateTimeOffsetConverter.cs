namespace Lullaby.Utils;

using System.Text.Json;
using System.Text.Json.Serialization;

public class UnixMillisecondsDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64());

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value.ToUnixTimeMilliseconds());
}
