using System;
using Utf8Json;
using Utf8Json.Internal;

namespace Tinode.Client
{
    internal sealed class LinkEntityFormatter : IJsonFormatter<DraftyMessage.LinkEntity>
    {
        private readonly byte[][] _stringByteKeys;

        public LinkEntityFormatter()
        {
            _stringByteKeys = new[]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("tp"),
                JsonWriter.GetEncodedPropertyNameWithoutQuotation("LN"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("data"),
                JsonWriter.GetEncodedPropertyName("url")
            };
        }

        public void Serialize(ref JsonWriter writer, DraftyMessage.LinkEntity value, IJsonFormatterResolver formatterResolver)
        {
            // {"tp":
            UnsafeMemory64.WriteRaw6(ref writer, _stringByteKeys[0]);
            writer.WriteQuotation();
            UnsafeMemory64.WriteRaw2(ref writer, _stringByteKeys[1]);
            writer.WriteQuotation();

            // "data":
            UnsafeMemory64.WriteRaw8(ref writer, _stringByteKeys[2]);
            // {
            writer.WriteBeginObject();

            // "url":
            UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[3]);
            writer.WriteString(value.Data);

            // }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        public DraftyMessage.LinkEntity Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            throw new NotImplementedException();
        }
    }
}