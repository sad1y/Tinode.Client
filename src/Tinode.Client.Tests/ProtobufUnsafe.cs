using System;
using Google.Protobuf;
using Tinode.Client.Extensions;
using Xunit;

namespace Tinode.Client.Tests
{
    public class ProtobufUnsafeTests
    {
        [Fact]
        public void ToByteString_ReturnEmpty_IfDataNull()
        {
            byte[] data = null;

            var str = data.ToByteString();

            Assert.Equal(ByteString.Empty, str);
        }

        [Fact]
        public void ToByteString_ReturnEmpty_IfDataEmpty()
        {
            var data = Array.Empty<byte>();

            var str = data.ToByteString();

            Assert.Equal(ByteString.Empty, str);
        }

        [Fact]
        public void ToByteString_ReturnString_OnFullfilledArray()
        {
            var data = new byte[] {0x1, 0x2};

            var str = data.ToByteString();

            Assert.Equal(str.ToByteArray(), data);
        }
    }
}