using System;
using Utf8Json;
using Utf8Json.Internal;

namespace Tinode.Client
{
    internal sealed class BlobEntityFormatter : IJsonFormatter<DraftyMessage.BlobEntity>
    {
        private readonly byte[][] _stringByteKeys;

        public BlobEntityFormatter()
        {
            _stringByteKeys = new[]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("tp"),
                JsonWriter.GetEncodedPropertyNameWithoutQuotation("EX"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("data"),
                JsonWriter.GetEncodedPropertyNameWithBeginObject("mime"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("val"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ref"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("size"),
            };
        }

        public void Serialize(ref JsonWriter writer, DraftyMessage.BlobEntity imageEntity, IJsonFormatterResolver formatterResolver)
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

            if (!string.IsNullOrEmpty(imageEntity.Name))
            {
                // "name":
                UnsafeMemory64.WriteRaw8(ref writer, _stringByteKeys[6]);
                writer.WriteString(imageEntity.Name);
            }


            if (imageEntity.Size != -1)
            {
                // "size":
                UnsafeMemory64.WriteRaw8(ref writer, _stringByteKeys[7]);
                writer.WriteInt32(imageEntity.Size);
            }
            writer.WriteEndObject();
            // }
            writer.WriteEndObject();
        }

        public DraftyMessage.BlobEntity Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            throw new NotImplementedException();
        }
    }
}