using System;
using Google.Protobuf;
using BindingFlags = System.Reflection.BindingFlags;

namespace Tinode.Client.Extensions
{
    public static class ProtobufUnsafe
    {
        private static readonly Func<byte[], ByteString> Factory;

        static ProtobufUnsafe()
        {
            Factory = BuildMethod();
        }

        public static ByteString ToByteString(this byte[] data)
        {
            if (data == null || data.Length == 0) return ByteString.Empty;

            return Factory(data);
        }


        private static Func<byte[], ByteString> BuildMethod()
        {
            var type = typeof(ByteString);
            var methodInfo = type.GetMethod("AttachBytes", BindingFlags.Static | BindingFlags.NonPublic);

            ByteString CreateByteString(byte[] data)
            {
                return (ByteString) methodInfo.Invoke(null, new object[] {data});
            }

            return CreateByteString;
        }
    }
}