using System;
using Utf8Json;
using Utf8Json.Internal;

namespace Tinode.Client
{
    internal sealed class HashtagEntityFormatter : IJsonFormatter<DraftyMessage.HashTagEntity>
    {
        private readonly byte[][] _stringByteKeys;

        public HashtagEntityFormatter()
        {
            _stringByteKeys = new[]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("tp"),
                JsonWriter.GetEncodedPropertyName("HT"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("data"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("val")
            };
        }

        public void Serialize(ref JsonWriter writer, DraftyMessage.HashTagEntity value, IJsonFormatterResolver formatterResolver)
        {
            // {"tp":
            UnsafeMemory64.WriteRaw6(ref writer, _stringByteKeys[0]);
            UnsafeMemory64.WriteRaw4(ref writer, _stringByteKeys[1]);

            // "data":
            UnsafeMemory64.WriteRaw8(ref writer, _stringByteKeys[2]);
            // {
            writer.WriteBeginObject();

            // "val":
            UnsafeMemory64.WriteRaw6(ref writer, _stringByteKeys[3]);
            writer.WriteString(value.Data);

            // }
            writer.WriteEndObject();
        }

        public DraftyMessage.HashTagEntity Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            throw new NotImplementedException();
        }
    }
}