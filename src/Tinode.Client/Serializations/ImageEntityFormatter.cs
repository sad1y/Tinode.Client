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
                JsonWriter.GetEncodedPropertyNameWithoutQuotation("IM"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("data"),
                JsonWriter.GetEncodedPropertyNameWithBeginObject("mime"),
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
            writer.WriteQuotation();
            UnsafeMemory64.WriteRaw2(ref writer, _stringByteKeys[1]);
            writer.WriteQuotation();

            // "data":
            UnsafeMemory64.WriteRaw8(ref writer, _stringByteKeys[2]);
            // {
            // "mime":
            UnsafeMemory64.WriteRaw8(ref writer, _stringByteKeys[3]);
            writer.WriteString(imageEntity.Mime);

            if (string.IsNullOrEmpty(imageEntity.Ref))
            {
                // "val":
                UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[4]);
                writer.WriteString(imageEntity.Val);
            }
            else
            {
                // "ref":
                UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[5]);
                writer.WriteString(imageEntity.Ref);
            }

            // "width":
            UnsafeMemory64.WriteRaw9(ref writer, _stringByteKeys[6]);
            writer.WriteInt32(imageEntity.Width);

            // "height":
            UnsafeMemory64.WriteRaw10(ref writer, _stringByteKeys[7]);
            writer.WriteInt32(imageEntity.Height);

            if (!string.IsNullOrEmpty(imageEntity.Name))
            {
                // "name":
                UnsafeMemory64.WriteRaw8(ref writer, _stringByteKeys[8]);
                writer.WriteString(imageEntity.Name);
            }

            if (imageEntity.Size != -1)
            {
                // "size":
                UnsafeMemory64.WriteRaw8(ref writer, _stringByteKeys[9]);
                writer.WriteInt32(imageEntity.Size);
            }

            writer.WriteEndObject();
            // }
            writer.WriteEndObject();
        }

        public DraftyMessage.ImageEntity Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            throw new NotImplementedException();
        }
    }
}