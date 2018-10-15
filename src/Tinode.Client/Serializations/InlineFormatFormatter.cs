using System;
using Utf8Json;
using Utf8Json.Internal;

namespace Tinode.Client
{
    internal sealed class InlineFormatFormatter : IJsonFormatter<DraftyMessage.InlineFormat>
        {
            private readonly byte[][] _stringByteKeys;
            private readonly byte[][] _typeKeys;

            public InlineFormatFormatter()
            {
                _stringByteKeys = new[]
                {
                    JsonWriter.GetEncodedPropertyNameWithBeginObject("at"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("len"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("key"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tp"),
                };

                _typeKeys = new[]
                {
                    JsonWriter.GetEncodedPropertyNameWithoutQuotation(string.Empty),
                    JsonWriter.GetEncodedPropertyNameWithoutQuotation("ST"),
                    JsonWriter.GetEncodedPropertyNameWithoutQuotation("EM"),
                    JsonWriter.GetEncodedPropertyNameWithoutQuotation("DL"),
                    JsonWriter.GetEncodedPropertyNameWithoutQuotation("CO"),
                    JsonWriter.GetEncodedPropertyNameWithoutQuotation("BR"),
                };
            }


            public void Serialize(ref JsonWriter writer, DraftyMessage.InlineFormat format, IJsonFormatterResolver formatterResolver)
            {
                // {"at":
                UnsafeMemory64.WriteRaw6(ref writer, _stringByteKeys[0]);
                writer.WriteInt32(format.At);
                // "len":
                UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[1]);
                writer.WriteInt32(format.Len);

                if (format.Decoration != DraftyMessage.TextDecoration.None)
                {
                    // "tp":
                    UnsafeMemory64.WriteRaw6(ref writer, _stringByteKeys[3]);
                    writer.WriteQuotation();
                    UnsafeMemory64.WriteRaw2(ref writer, _typeKeys[(int) format.Decoration]);
                    writer.WriteQuotation();
                }
                else
                {
                    // "key": 
                    UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[2]);
                    writer.WriteInt32(format.Key);
                }

                // }
                writer.WriteEndObject();
            }

            public DraftyMessage.InlineFormat Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
            {
                throw new NotImplementedException();
            }
        }

}