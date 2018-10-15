using System;
using Utf8Json;
using Utf8Json.Internal;

namespace Tinode.Client
{
    internal sealed class DraftyMessageFormatter : IJsonFormatter<DraftyMessage>
    {
        private readonly byte[][] _stringByteKeys;

        public DraftyMessageFormatter()
        {
            _stringByteKeys = new[]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("txt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("fmt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ent"),
            };
        }

        public void Serialize(ref JsonWriter writer, DraftyMessage value, IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            // {"txt":
            UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[0]);
            writer.WriteString(value.UnformattedText);

            // "fmt":
            UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[1]);

            // [
            writer.WriteBeginArray();

            var inlineFormatSerializer = formatterResolver.GetFormatter<DraftyMessage.InlineFormat>();

            var hasElements = false;
            foreach (var format in value.GetInlineFormats())
            {
                inlineFormatSerializer.Serialize(ref writer, format, formatterResolver);
                writer.WriteValueSeparator();
                hasElements = true;
            }

            if (hasElements)
                writer.AdvanceOffset(-1);

            // ]
            writer.WriteEndArray();

            // {"ent":
            UnsafeMemory64.WriteRaw7(ref writer, _stringByteKeys[2]);
            // [
            writer.WriteBeginArray();

            hasElements = false;

            foreach (var entity in value.GetEntities())
            {
                switch (entity.Type)
                {
                    case "LN":
                    {
                        formatterResolver.GetFormatter<DraftyMessage.LinkEntity>()
                            .Serialize(ref writer, (DraftyMessage.LinkEntity) entity, formatterResolver);
                        break;
                    }
                    case "IM":
                    {
                        formatterResolver.GetFormatter<DraftyMessage.ImageEntity>()
                            .Serialize(ref writer, (DraftyMessage.ImageEntity) entity, formatterResolver);
                        break;
                    }
                    case "EX":
                    {
                        formatterResolver.GetFormatter<DraftyMessage.BlobEntity>()
                            .Serialize(ref writer, (DraftyMessage.BlobEntity) entity, formatterResolver);
                        break;
                    }
                    case "MN":
                    {
                        formatterResolver.GetFormatter<DraftyMessage.MentionEntity>()
                            .Serialize(ref writer, (DraftyMessage.MentionEntity) entity, formatterResolver);
                        break;
                    }
                    case "HT":
                    {
                        formatterResolver.GetFormatter<DraftyMessage.HashTagEntity>()
                            .Serialize(ref writer, (DraftyMessage.HashTagEntity) entity, formatterResolver);
                        break;
                    }
                }

                writer.WriteValueSeparator();
                hasElements = true;
            }

            if (hasElements)
                writer.AdvanceOffset(-1);

            // ]
            writer.WriteEndArray();

            // }
            writer.WriteEndObject();
        }

        public DraftyMessage Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            throw new NotImplementedException();
        }
    }
}