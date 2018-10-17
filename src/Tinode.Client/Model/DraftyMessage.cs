using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Utf8Json;
using Utf8Json.Internal;
using Utf8Json.Resolvers;

namespace Tinode.Client
{
    public sealed class DraftyMessage
    {
        private readonly StringBuilder _sb;
        private readonly List<InlineFormat> _inlineFormat = new List<InlineFormat>();
        private readonly List<IDraftyEntity> _entities = new List<IDraftyEntity>();
        private static readonly IJsonFormatterResolver JsonFormatter;

        public IEnumerable<InlineFormat> GetInlineFormats() => _inlineFormat;

        public IEnumerable<IDraftyEntity> GetEntities() => _entities;

        public string UnformattedText => _sb.ToString();

        static DraftyMessage()
        {
            JsonFormatter = CompositeResolver.Create(
                new DraftyMessageFormatter(),
                new InlineFormatFormatter(),
                new LinkEntityFormatter(),
                new BlobEntityFormatter(),
                new ImageEntityFormatter(),
                new HashtagEntityFormatter(),
                new MentionEntityFormatter());
        }

        public static DraftyMessage Create() => new DraftyMessage();

        public static DraftyMessage Create(string text) => new DraftyMessage(text);

        public DraftyMessage()
        {
            _sb = new StringBuilder();
        }

        public DraftyMessage(string text)
        {
            _sb = new StringBuilder(text);
        }

        public DraftyMessage Text(string text)
        {
            _sb.Append(text);
            return this;
        }

        public DraftyMessage Bold(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            _inlineFormat.Add(new InlineFormat {Key = -1, Len = text.Length, At = _sb.Length, Decoration = TextDecoration.Strong});
            _sb.Append(text);
            return this;
        }

        public DraftyMessage Bold(int position, int len)
        {
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position));
            if (len < 1) throw new ArgumentOutOfRangeException(nameof(len));
            if (position + len > _sb.Length) throw new ArgumentOutOfRangeException(nameof(position));
            _inlineFormat.Add(new InlineFormat {Key = -1, Len = len, At = position, Decoration = TextDecoration.Strong});
            return this;
        }

        public DraftyMessage Emphasize(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            _inlineFormat.Add(new InlineFormat {Key = -1, Len = text.Length, At = _sb.Length, Decoration = TextDecoration.Emphasized});
            _sb.Append(text);
            return this;
        }

        public DraftyMessage Emphasize(int position, int len)
        {
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position));
            if (len < 1) throw new ArgumentOutOfRangeException(nameof(len));
            if (position + len > _sb.Length) throw new ArgumentOutOfRangeException(nameof(position));

            _inlineFormat.Add(new InlineFormat {Key = -1, Len = len, At = position, Decoration = TextDecoration.Emphasized});

            return this;
        }

        public DraftyMessage Deleted(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            _inlineFormat.Add(new InlineFormat {Key = -1, Len = text.Length, At = _sb.Length, Decoration = TextDecoration.Delete});
            _sb.Append(text);
            return this;
        }

        public DraftyMessage Deleted(int position, int len)
        {
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position));
            if (len < 1) throw new ArgumentOutOfRangeException(nameof(len));
            if (position + len > _sb.Length) throw new ArgumentOutOfRangeException(nameof(position));
            _inlineFormat.Add(new InlineFormat {Key = -1, Len = len, At = position, Decoration = TextDecoration.Delete});
            return this;
        }

        public DraftyMessage Code(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            _inlineFormat.Add(new InlineFormat {Key = -1, Len = text.Length, At = _sb.Length, Decoration = TextDecoration.Code});
            _sb.Append(text);

            return this;
        }

        public DraftyMessage Code(int position, int len)
        {
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position));
            if (len < 1) throw new ArgumentOutOfRangeException(nameof(len));
            if (position + len > _sb.Length) throw new ArgumentOutOfRangeException(nameof(position));
            _inlineFormat.Add(new InlineFormat {Key = -1, Len = len, At = position, Decoration = TextDecoration.Code});
            return this;
        }

        public DraftyMessage NextLine() => NextLine(_sb.Length);

        public DraftyMessage NextLine(int position)
        {
            if (position > _sb.Length || position < 0) throw new ArgumentOutOfRangeException(nameof(position));
            _inlineFormat.Add(new InlineFormat {Key = -1, Len = 1, At = position, Decoration = TextDecoration.LineBreak});
            return this;
        }

        public DraftyMessage Link(string text, string uri)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            if (string.IsNullOrEmpty(uri)) throw new ArgumentException("Value cannot be null or empty.", nameof(uri));
            _inlineFormat.Add(new InlineFormat {Key = _entities.Count, At = _sb.Length, Len = text.Length, Decoration = TextDecoration.None});
            _entities.Add(new LinkEntity {Data = uri});
            _sb.Append(text);
            return this;
        }

        public DraftyMessage ImageRef(string url, string mime, int width, int height, string name = null, int size = -1)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("Value cannot be null or empty.", nameof(url));
            if (string.IsNullOrEmpty(mime)) throw new ArgumentException("Value cannot be null or empty.", nameof(mime));
            if (width < 1) throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 1) throw new ArgumentOutOfRangeException(nameof(height));

            _inlineFormat.Add(new InlineFormat {Key = _entities.Count, At = _sb.Length, Len = 1, Decoration = TextDecoration.None});
            _entities.Add(new ImageEntity {Ref = url, Mime = mime, Width = width, Height = height, Name = name, Size = size});

            _sb.Append(" ");

            return this;
        }

        public DraftyMessage Image(string data, string mime, int width, int height, string name = null, int size = -1)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentException("Value cannot be null or empty.", nameof(data));
            if (string.IsNullOrEmpty(mime)) throw new ArgumentException("Value cannot be null or empty.", nameof(mime));
            if (width < 1) throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 1) throw new ArgumentOutOfRangeException(nameof(height));


            _inlineFormat.Add(new InlineFormat {Key = _entities.Count, At = _sb.Length, Len = 1, Decoration = TextDecoration.None});
            _entities.Add(new ImageEntity {Val = data, Mime = mime, Width = width, Height = height, Name = name, Size = size});

            _sb.Append(" ");

            return this;
        }

        public DraftyMessage BlobRef(string url, string mime, string name = null, int size = -1)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("Value cannot be null or empty.", nameof(url));
            if (string.IsNullOrEmpty(mime)) throw new ArgumentException("Value cannot be null or empty.", nameof(mime));

            _inlineFormat.Add(new InlineFormat {Key = _entities.Count, At = -1, Len = 0, Decoration = TextDecoration.None});
            _entities.Add(new BlobEntity {Ref = url, Mime = mime, Name = name, Size = size});

            return this;
        }

        public DraftyMessage Blob(string data, string mime, string name = null, int size = -1)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentException("Value cannot be null or empty.", nameof(data));
            if (string.IsNullOrEmpty(mime)) throw new ArgumentException("Value cannot be null or empty.", nameof(mime));

            _inlineFormat.Add(new InlineFormat {Key = _entities.Count, At = -1, Len = 0, Decoration = TextDecoration.None});
            _entities.Add(new BlobEntity {Val = data, Mime = mime, Name = name, Size = size});

            return this;
        }

        public DraftyMessage Mention(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));

            _inlineFormat.Add(new InlineFormat {Key = _entities.Count, At = _sb.Length, Len = text.Length, Decoration = TextDecoration.None});
            _entities.Add(new MentionEntity {Data = text});

            _sb.Append(text);

            return this;
        }

        public DraftyMessage Hashtag(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));

            _inlineFormat.Add(new InlineFormat {Key = _entities.Count, At = _sb.Length, Len = text.Length, Decoration = TextDecoration.None});
            _entities.Add(new HashTagEntity {Data = text});

            _sb.Append(text);

            return this;
        }

        public byte[] ToJsonBytes() => JsonSerializer.Serialize(this, JsonFormatter);

        public string ToJsonString() => Encoding.UTF8.GetString(ToJsonBytes());

        public struct InlineFormat
        {
            public int At;
            public int Len;
            public TextDecoration Decoration;
            public int Key;
        }

        public class LinkEntity : IDraftyEntity
        {
            public string Type => "LN";
            public string Data;
        }

        public class ImageEntity : IDraftyEntity
        {
            public string Type => "IM";
            public string Mime;
            public string Val;
            public string Ref;
            public int Width;
            public int Height;
            public int Size;
            public string Name;
        }

        public class BlobEntity : IDraftyEntity
        {
            public string Type => "EX";
            public string Mime;
            public string Val;
            public string Ref;
            public int Size;
            public string Name;
        }

        public class MentionEntity : IDraftyEntity
        {
            public string Type => "MN";
            public string Data;
        }

        public class HashTagEntity : IDraftyEntity
        {
            public string Type => "HT";
            public string Data;
        }

        public interface IDraftyEntity
        {
            string Type { get; }
        }

        public enum TextDecoration
        {
            None = 0,
            Strong = 1,
            Emphasized = 2,
            Delete = 3,
            Code = 4,
            LineBreak = 5,
        }
    }
}