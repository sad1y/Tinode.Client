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
            return JsonSerializer.Deserialize<PrivateData>(byteString.ToStringUtf8());
        }
    }
}