using System.Runtime.Serialization;
using Google.Protobuf;
using Utf8Json;

namespace Tinode.Client
{
    public class VCard
    {
        [DataMember(Name = "fn")] public string FormattedName { get; set; }
        [DataMember(Name = "n")] public VCardName Name { get; set; }
        [DataMember(Name = "org")] public string Organization { get; set; }
        [DataMember(Name = "title")] public string Title { get; set; }
        [DataMember(Name = "tel")] public VCardUserData[] Telephones { get; set; }
        [DataMember(Name = "email")] public VCardUserData[] Email { get; set; }
        [DataMember(Name = "impp")] public VCardUserData[] Impp { get; set; }
        [DataMember(Name = "photo")] public VCardPhoto Photo { get; set; }

        public static VCard FromByteString(ByteString byteString)
        {
            var a = JsonSerializer.Deserialize<VCard>(byteString.ToStringUtf8());
            return a;
        }
    }

    public class VCardUserData
    {
        [DataMember(Name = "type")] public string Type { get; set; }
        [DataMember(Name = "uri")] public string Uri { get; set; }
    }

    public class VCardPhoto
    {
        [DataMember(Name = "type")] public string Type { get; set; }
        [DataMember(Name = "data")] public string Data { get; set; }
    }

    public class VCardName
    {
        [DataMember(Name = "surname")] public string Surname { get; set; }
        [DataMember(Name = "given")] public string Given { get; set; }
        [DataMember(Name = "additional")] public string Additional { get; set; }
        [DataMember(Name = "prefix")] public string Prefix { get; set; }
        [DataMember(Name = "suffix")] public string Suffix { get; set; }
    }
}