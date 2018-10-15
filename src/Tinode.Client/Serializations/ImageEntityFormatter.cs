using System;
using Utf8Json;
using Utf8Json.Internal;

namespace Tinode.Client
{
    internal sealed class ImageEntityFormatter : IJsonFormatter<DraftyMessage.ImageEntity>
        {
            private readonly byte[][] _stringByteKeys;

            public ImageEntityFormatter()
            {
                _stringByteKeys = new[]
                {
                    JsonWriter.GetEncodedPropertyNameWithBeginObject("tp"),
                    JsonWriter.GetEncodedPropertyName("IM"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("data"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mime"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("val"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ref"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("width"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("height"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                    JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("size"),
                };
            }

            public void Serialize(ref JsonWriter writer, DraftyMessage.ImageEntity imageEntity, IJsonFormatterResolver formatterResolver)
            {
                // {"tp":
                UnsafeMemory64.WriteRaw6(ref writer, _stringByteKeys[0]);
                UnsafeMemory64.WriteRaw4(ref writer, _stringByteKeys[1]);

                // "data":
                UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[2]);
                // {
                writer.WriteBeginObject();

                // "mime":
                UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[10]);
                writer.WriteString(imageEntity.Mime);

                if (string.IsNullOrEmpty(imageEntity.Ref))
                {
                    // "val":
                    UnsafeMemory64.WriteRaw6(ref writer, _stringByteKeys[9]);
                    writer.WriteString(imageEntity.Val);
                }
                else
                {
                    // "ref":
                    UnsafeMemory64.WriteRaw6(ref writer, _stringByteKeys[11]);
                    writer.WriteString(imageEntity.Ref);
                }

                // "width":
                UnsafeMemory64.WriteRaw8(ref writer, _stringByteKeys[12]);
                writer.WriteInt32(imageEntity.Width);

                // "height":
                UnsafeMemory64.WriteRaw9(ref writer, _stringByteKeys[13]);
                writer.WriteInt32(imageEntity.Height);

                if (!string.IsNullOrEmpty(imageEntity.Name))
                {
                    // "name":
                    UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[14]);
                    writer.WriteString(imageEntity.Name);
                }


                if (imageEntity.Size != -1)
                {
                    // "size":
                    UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[15]);
                    writer.WriteInt32(imageEntity.Size);
                }

                // }
                writer.WriteEndObject();
            }

            public DraftyMessage.ImageEntity Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
            {
                throw new NotImplementedException();
            }
        }
}