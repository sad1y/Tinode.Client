using System.Runtime.Serialization;
using Google.Protobuf;
using Utf8Json;

namespace Tinode.Client
{
    public class PrivateData
    {
        [DataMember(Name = "comment")] public string Comment { get; set; }
        [DataMember(Name = "arch")] public bool Arch { get; set; }

        public static PrivateData FromByteString(ByteString byteString)
        {
            if (byteString.Length == 0)
                return new PrivateData();

            var json = byteString.ToStringUtf8();
            return JsonSerializer.Deserialize<PrivateData>(json);
        }
    }
}